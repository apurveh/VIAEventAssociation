using Moq;
using VIAEventAssociation.Core.Application.CommandDispatching.Commands;
using VIAEventAssociation.Core.Application.CommandDispatching.Dispatcher;
using VIAEventAssociation.Core.Application.CommandDispatching.Dispatcher.Decorators;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.DispatcherTest;

public class DispatchDecoratorTests
{
    private readonly Mock<ICommandDispatcher> _mockInnerDispatcher;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly ICommand<EventId> _sampleCommand;


    public DispatchDecoratorTests() {
        _mockInnerDispatcher = new Mock<ICommandDispatcher>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _sampleCommand = new Mock<ICommand<EventId>>().Object;

        _mockInnerDispatcher.Setup(d => d.DispatchAsync(It.IsAny<ICommand<EventId>>())).ReturnsAsync(Result.Ok);
    }

    [Fact]
    public async Task LoggingCommandDispatcherDecorator_CallsNextAndLogs() {
        // Arrange
        var loggingDecorator = new LoggingCommandDispatcherDecorator(_mockInnerDispatcher.Object);

        // Act
        var result = await loggingDecorator.DispatchAsync(_sampleCommand);

        // Assert
        Assert.Equal(Result.Ok, result);
        _mockInnerDispatcher.Verify(d => d.DispatchAsync(_sampleCommand), Times.Once);
    }

    [Fact]
    public async Task TransactionCommandDispatcherDecorator_CommitsTransactionOnSuccess() {
        // Arrange
        var transactionDecorator = new TransactionCommandDispatcherDecorator(_mockInnerDispatcher.Object, _mockUnitOfWork.Object);

        // Act
        var result = await transactionDecorator.DispatchAsync(_sampleCommand);

        // Assert
        Assert.Equal(Result.Ok, result);
        _mockInnerDispatcher.Verify(d => d.DispatchAsync(_sampleCommand), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task TransactionCommandDispatcherDecorator_DoesNotCommitTransactionOnFailure() {
        // Arrange
        _mockInnerDispatcher.Setup(d => d.DispatchAsync(It.IsAny<ICommand<EventId>>())).ReturnsAsync(Error.Unknown);
        var transactionDecorator = new TransactionCommandDispatcherDecorator(_mockInnerDispatcher.Object, _mockUnitOfWork.Object);

        // Act
        var result = await transactionDecorator.DispatchAsync(_sampleCommand);

        // Assert
        Assert.Equal(Error.Unknown, result.Error);
        _mockInnerDispatcher.Verify(d => d.DispatchAsync(_sampleCommand), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CommandExecutionTimer_DecoratorMeasuresExecutionTime() {
        // Arrange
        var timerDecorator = new CommandExecutionTimer(_mockInnerDispatcher.Object);

        // Act
        var result = await timerDecorator.DispatchAsync(_sampleCommand);

        // Assert
        Assert.Equal(Result.Ok, result);
        _mockInnerDispatcher.Verify(d => d.DispatchAsync(_sampleCommand), Times.Once);
    }

    [Fact]
    public async Task ChainedDecorators_HandleCommandInOrder() {
        // Arrange
        var transactionDecorator = new TransactionCommandDispatcherDecorator(_mockInnerDispatcher.Object, _mockUnitOfWork.Object);
        var loggingDecorator = new LoggingCommandDispatcherDecorator(transactionDecorator);
        var timerDecorator = new CommandExecutionTimer(loggingDecorator);

        // Act
        var result = await timerDecorator.DispatchAsync(_sampleCommand);

        // Assert
        Assert.Equal(Result.Ok, result);
        _mockInnerDispatcher.Verify(d => d.DispatchAsync(_sampleCommand), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}