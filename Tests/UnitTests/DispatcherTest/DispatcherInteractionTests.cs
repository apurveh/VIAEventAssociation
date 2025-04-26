// Test class for Dispatcher interaction tests

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using UnitTests.Common.Dispatcher;
using ViaEventAssociation.Core.Application.CommandDispatching;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventAssociation.Core.Application.CommandDispatching.Dispatcher;
using ViaEventAssociation.Core.Application.CommandDispatching.Dispatcher.Decorators;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Dispatcher;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using Xunit;

namespace UnitTests.DispatcherTest;

public class DispatcherInteractionTests {
    // Test to check if the command ends up at the correct handler
    [Fact]
    public async Task Dispatch_SingleHandlerRegistered_CommandEndsUpAtCorrectHandler() {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICommandHandler<ActivateEventCommand>, ActivateEventMockHandler>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        ICommandDispatcher dispatcher = new CommandDispatcher(serviceProvider);
        var eventId = EventId.GenerateId().Payload;
        var cmd = ActivateEventCommand.Create(eventId.Value).Payload;

        // Act
        var result = await dispatcher.DispatchAsync(cmd);

        // Assert
        Assert.True(result.IsSuccess);

        var handler = (ActivateEventMockHandler) serviceProvider.GetRequiredService<ICommandHandler<ActivateEventCommand>>();

        Assert.True(handler.HandleAsyncCallCount == 1);
    }

    // Test to check if the command ends up at the correct handler
    [Fact]
    public async Task Dispatch_MultipleHandlersRegistered_CommandEndsUpAtCorrectHandler() {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICommandHandler<ActivateEventCommand>, ActivateEventMockHandler>();
        serviceCollection.AddScoped<ICommandHandler<RequestToJoinCommand>, RequestToJoinMockHandler>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var evt = EventFactory.Init().Build();
        var gust = GuestFactory.InitWithDefaultsValues().Build();

        ICommandDispatcher dispatcher = new CommandDispatcher(serviceProvider);
        var dispatcherDecorator = new LoggingCommandDispatcherDecorator(dispatcher);
        var commandExecutionTimer = new CommandExecutionTimer(dispatcherDecorator);

        var eventId = EventId.GenerateId().Payload;
        var cmd = ActivateEventCommand.Create(eventId.Value).Payload;
        var cmd2 = RequestToJoinCommand.Create(evt.Id.Value, gust.Id.Value).Payload;

        // Act
        var result = await commandExecutionTimer.DispatchAsync(cmd);
        var result2 = await commandExecutionTimer.DispatchAsync(cmd2);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result2.IsSuccess);

        var handler = (ActivateEventMockHandler) serviceProvider.GetRequiredService<ICommandHandler<ActivateEventCommand>>();
        var handler2 = (RequestToJoinMockHandler) serviceProvider.GetRequiredService<ICommandHandler<RequestToJoinCommand>>();

        Assert.True(handler.HandleAsyncCallCount == 1);
        Assert.True(handler2.HandleAsyncCallCount == 1);
    }
}