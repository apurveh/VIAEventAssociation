using Microsoft.Extensions.DependencyInjection;
using UnitTests.Common.Dispatcher;
using UnitTests.Features.EventTests;
using UnitTests.Features.GuestTests;
using VIAEventAssociation.Core.Application.CommandDispatching;
using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using VIAEventAssociation.Core.Application.CommandDispatching.Dispatcher;
using VIAEventAssociation.Core.Application.CommandDispatching.Dispatcher.Decorators;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;

namespace UnitTests.DispatcherTest;

public class DispatcherInteractionTests
{
    [Fact]
    public async Task Dispatcher_Should_Call_ActivateEventHandler()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICommandHandler<ActivateEventCommand, EventId>, ActivateEventMockHandler>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        ICommandDispatcher dispatcher = new CommandDispatcher(serviceProvider);
        var eventId = EventId.GenerateId().Payload;
        var cmd = ActivateEventCommand.Create(eventId.Value).Payload;

        // Act
        var result = await dispatcher.DispatchAsync(cmd);

        // Assert
        Assert.True(result.IsSuccess);

        var handler = (ActivateEventMockHandler) serviceProvider.GetRequiredService<ICommandHandler<ActivateEventCommand, EventId>>();

        Assert.True(handler.HandleAsyncCallCount == 1);
    }
    
    [Fact]
    public async Task Dispatch_MultipleHandlersRegistered_CommandEndsUpAtCorrectHandler() {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICommandHandler<ActivateEventCommand, EventId>, ActivateEventMockHandler>();
        serviceCollection.AddScoped<ICommandHandler<RequestToJoinCommand, GuestId>, RequestToJoinMockHandler>();
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

        var handler = (ActivateEventMockHandler) serviceProvider.GetRequiredService<ICommandHandler<ActivateEventCommand, EventId>>();
        var handler2 = (RequestToJoinMockHandler) serviceProvider.GetRequiredService<ICommandHandler<RequestToJoinCommand, GuestId>>();

        Assert.True(handler.HandleAsyncCallCount == 1);
        Assert.True(handler2.HandleAsyncCallCount == 1);
    }
}