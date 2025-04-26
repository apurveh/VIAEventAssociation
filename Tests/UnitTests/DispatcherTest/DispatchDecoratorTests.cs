using System.Threading.Tasks;
using Moq;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Application.CommandDispatching.Dispatcher;
using ViaEventAssociation.Core.Application.CommandDispatching.Dispatcher.Decorators;
using ViaEventAssociation.Core.Application.Features.Dispatcher;
using ViaEventAssociation.Core.Domain;
using Xunit;

namespace UnitTests.DispatcherTest;

public class DispatchDecoratorTests {
    private readonly Mock<ICommandDispatcher> _mockInnerDispatcher;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Command _sampleCommand;


    public DispatchDecoratorTests() {
        _mockInnerDispatcher = new Mock<ICommandDispatcher>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _sampleCommand = new Mock<Command>().Object;

        // Setup the mock dispatcher to return a successful result by default
        _mockInnerDispatcher.Setup(d => d.DispatchAsync(It.IsAny<Command>())).ReturnsAsync(Result.Ok);
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
        _mockInnerDispatcher.Setup(d => d.DispatchAsync(It.IsAny<Command>())).ReturnsAsync(Error.Unknown);
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