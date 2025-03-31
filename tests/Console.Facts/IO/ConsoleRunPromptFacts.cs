using Gint.Console.Facts.Testbed;
using Gint.Console.IO;
using Gint.Core.Changes;
using Gint.Core.IO;
using Gint.Core.Operations;
using Gint.Core.Properties;
using Gint.Testbed.Creators;
using Moq;
using Moq.AutoMock;

namespace Gint.Console.Facts.IO;

public class ConsoleRunPromptFacts : IDisposable
{
    private readonly AutoMocker _mocker = new();
    private readonly TestApplicationConsole _console = new();

    public ConsoleRunPromptFacts()
        => _mocker.Use<IApplicationConsole>(_console);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _console.Dispose();
        }
    }

    private ConsoleRunPrompt CreateSystemUnderTest()
        => _mocker.CreateInstance<ConsoleRunPrompt>();

    public class TheOpenMethod : ConsoleRunPromptFacts
    {
        [Fact]
        public async Task ExitsWithoutExecutingOperationWhenNoChangesPresent()
        {
            // Arrange
            var context = new RunPromptContext(new("."), OperationScope.All);

            _mocker.GetMock<IChangeAccessor>()
                .Setup(changeAccessor => changeAccessor.Get(context.Pathspec, default))
                .ReturnsAsync(ChangesCreator.CreateGroup([]));

            var sut = CreateSystemUnderTest();

            // Act
            var results = await sut.Open(context, default);

            // Assert
            Assert.Empty(results);
            _mocker.GetMock<IOperation>()
                .Verify(
                    operation => operation.Execute(It.IsAny<OperationContext>(), default),
                    Times.Never());
        }

        [Fact]
        public async Task ExitsAfterIndicatedByExecutedOperation()
        {
            // Arrange
            var context = new RunPromptContext(new("."), OperationScope.All);

            var changes = ChangesCreator.CreateGroup(ChangesCreator.CreateFile());
            _mocker.GetMock<IChangeAccessor>()
                .Setup(changeAccessor => changeAccessor.Get(context.Pathspec, default))
                .ReturnsAsync(changes);

            _mocker.GetMock<IOperationAccessor>()
                .Setup(operationAccessor => operationAccessor.Filter(It.Is<OperationFilter>(filter =>
                    filter.Scope == context.Scope &&
                    filter.Changes == changes)))
                .Returns([OperationDescriptor.Quit]);

            _console.AddInputLines("q");

            _mocker.GetMock<IOperation>()
                .Setup(operation => operation.Execute(
                    It.Is<OperationContext>(operationContext =>
                        operationContext.Runspec.Descriptor == OperationDescriptor.Quit &&
                        operationContext.Runspec.Area == null &&
                        operationContext.Pathspec == context.Pathspec &&
                        operationContext.Scope == context.Scope &&
                        operationContext.Changes == changes),
                    default))
                .ReturnsAsync((OperationContext operationContext, CancellationToken _) => OperationResult.Terminal(operationContext))
                .Verifiable();

            var sut = CreateSystemUnderTest();

            // Act
            var results = await sut.Open(context, default);

            // Assert
            _mocker.Verify();
            var result = Assert.Single(results);
            Assert.True(result.Succeeded);
            Assert.True(result.Exit);
        }

        [Fact]
        public async Task ExitsAfterOperationResolvedChanges()
        {
            // Arrange
            var context = new RunPromptContext(new("foo.txt"), OperationScope.All);

            var changes = ChangesCreator.CreateGroup(ChangesCreator.CreateFile());
            _mocker.GetMock<IChangeAccessor>()
                .SetupSequence(changeAccessor => changeAccessor.Get(context.Pathspec, default))
                .ReturnsAsync(changes)
                .ReturnsAsync(ChangesCreator.CreateGroup([]));

            _mocker.GetMock<IOperationAccessor>()
                .Setup(operationAccessor => operationAccessor.Filter(It.Is<OperationFilter>(filter =>
                    filter.Scope == context.Scope &&
                    filter.Changes == changes)))
                .Returns([OperationDescriptor.Restore]);

            _console.AddInputLines("r");

            _mocker.GetMock<IOperation>()
                .Setup(operation => operation.Execute(
                    It.Is<OperationContext>(operationContext =>
                        operationContext.Runspec.Descriptor == OperationDescriptor.Restore &&
                        operationContext.Runspec.Area == null &&
                        operationContext.Pathspec == context.Pathspec &&
                        operationContext.Scope == context.Scope &&
                        operationContext.Changes == changes),
                    default))
                .ReturnsAsync((OperationContext operationContext, CancellationToken _)
                    => OperationResult.Command(operationContext, GitCommandResultCreator.CreateRunSuccess()))
                .Verifiable();

            var sut = CreateSystemUnderTest();

            // Act
            var results = await sut.Open(context, default);

            // Assert
            var result = Assert.Single(results);
            Assert.True(result.Succeeded);
            Assert.True(result.Wrote);
            _mocker.Verify();
        }

        [Fact]
        public async Task ExitsAfterExecutingMultipleOperations()
        {
            // Arrange
            var file = "foo.txt";
            var context = new RunPromptContext(new(file), OperationScope.All);

            var fileStaged = ChangesCreator.CreateFile(
                stagingIndicator: ChangeIndicator.Added,
                workingIndicator: ChangeIndicator.Unmodified,
                path: file);
            var fileUntracked = ChangesCreator.CreateFile(indicator: ChangeIndicator.Untracked, path: file);
            var fileUnstaged = ChangesCreator.CreateFile(
                stagingIndicator: ChangeIndicator.Unmodified,
                workingIndicator: ChangeIndicator.Added,
                path: file);
            var changesStaged = ChangesCreator.CreateGroup(fileStaged);
            var changesUntracked = ChangesCreator.CreateGroup(fileUntracked);
            var changesUnstaged = ChangesCreator.CreateGroup(fileUnstaged);
            _mocker.GetMock<IChangeAccessor>()
                .SetupSequence(changeAccessor => changeAccessor.Get(context.Pathspec, default))
                .ReturnsAsync(changesStaged)
                .ReturnsAsync(changesUntracked)
                .ReturnsAsync(changesUnstaged);

            _mocker.GetMock<IOperationAccessor>()
                .Setup(operationAccessor => operationAccessor.Filter(It.IsAny<OperationFilter>()))
                .Returns(
                    [
                        OperationDescriptor.IntendToAdd,
                        OperationDescriptor.Restore,
                        OperationDescriptor.Quit
                    ]);

            _console.AddInputLines("r", "n", "q");

            _mocker.GetMock<IOperation>()
                .Setup(operation => operation.Execute(
                    It.Is<OperationContext>(operationContext =>
                        operationContext.Runspec.Descriptor == OperationDescriptor.IntendToAdd ||
                        operationContext.Runspec.Descriptor == OperationDescriptor.Restore),
                    default))
                .ReturnsAsync((OperationContext operationContext, CancellationToken _)
                    => OperationResult.Command(operationContext, GitCommandResultCreator.CreateRunSuccess()))
                .Verifiable();

            _mocker.GetMock<IOperation>()
                .Setup(operation => operation.Execute(
                    It.Is<OperationContext>(operationContext => operationContext.Runspec.Descriptor == OperationDescriptor.Quit),
                    default))
                .ReturnsAsync((OperationContext operationContext, CancellationToken _)
                    => OperationResult.Terminal(operationContext))
                .Verifiable();

            var sut = CreateSystemUnderTest();

            // Act
            var results = await sut.Open(context, default);

            // Assert
            Assert.Equal(3, results.Count);
            _mocker.Verify();
        }

        [Fact]
        public async Task WritesAvailableOperations()
        {
            // Arrange
            var context = new RunPromptContext(new("."), OperationScope.All);

            _mocker.GetMock<IChangeAccessor>()
                .Setup(changeAccessor => changeAccessor.Get(context.Pathspec, default))
                .ReturnsAsync(ChangesCreator.CreateGroup(ChangesCreator.CreateFile()));

            _mocker.GetMock<IOperationAccessor>()
                .Setup(operationAccessor => operationAccessor.Filter(It.IsAny<OperationFilter>()))
                .Returns([OperationDescriptor.Add, OperationDescriptor.Diff, OperationDescriptor.Status, OperationDescriptor.Quit]);

            _console.AddInputLines("q");

            _mocker.GetMock<IOperation>()
                .Setup(operation => operation.Execute(It.IsAny<OperationContext>(), default))
                .ReturnsAsync((OperationContext operationContext, CancellationToken _) => OperationResult.Terminal(operationContext));

            var sut = CreateSystemUnderTest();

            // Act
            await sut.Open(context, default);

            // Assert
            var outputLine = Assert.Single(_console.OutputLines);
            Assert.Equal("Perform operation [a,d,s,q]: ", outputLine);
        }

        [Fact]
        public async Task WritesFileCounterWhenProvided()
        {
            // Arrange
            var context = new RunPromptContext(new("."), OperationScope.File, new(2, 4));

            _mocker.GetMock<IChangeAccessor>()
                .Setup(changeAccessor => changeAccessor.Get(context.Pathspec, default))
                .ReturnsAsync(ChangesCreator.CreateGroup(ChangesCreator.CreateFile()));

            _mocker.GetMock<IOperationAccessor>()
                .Setup(operationAccessor => operationAccessor.Filter(It.IsAny<OperationFilter>()))
                .Returns([OperationDescriptor.Quit]);

            _console.AddInputLines("q");

            _mocker.GetMock<IOperation>()
                .Setup(operation => operation.Execute(It.IsAny<OperationContext>(), default))
                .ReturnsAsync((OperationContext operationContext, CancellationToken _) => OperationResult.Terminal(operationContext));

            var sut = CreateSystemUnderTest();

            // Act
            await sut.Open(context, default);

            // Assert
            var outputLine = Assert.Single(_console.OutputLines);
            Assert.Equal("Perform operation (2/4) [q]: ", outputLine);
        }

        [Fact]
        public async Task WritesRunspecParseMessagesToStandardError()
        {
            // Arrange
            var context = new RunPromptContext(new("."), OperationScope.All);

            _mocker.GetMock<IChangeAccessor>()
                .Setup(changeAccessor => changeAccessor.Get(context.Pathspec, default))
                .ReturnsAsync(ChangesCreator.CreateGroup(ChangesCreator.CreateFile()));

            _mocker.GetMock<IOperationAccessor>()
                .Setup(operationAccessor => operationAccessor.Filter(It.IsAny<OperationFilter>()))
                .Returns([OperationDescriptor.Diff, OperationDescriptor.Quit]);

            _console.AddInputLines("z", "a", "q+", "d++", "d_", "q");

            _mocker.GetMock<IOperation>()
                .Setup(operation => operation.Execute(It.IsAny<OperationContext>(), default))
                .ReturnsAsync((OperationContext operationContext, CancellationToken _) => OperationResult.Terminal(operationContext));

            var sut = CreateSystemUnderTest();

            // Act
            await sut.Open(context, default);

            // Assert
            Assert.Equal(5, _console.ErrorLines.Count);
            Assert.Equal(Messages.UnknownOperationInput('z'), _console.ErrorLines[0]);
            Assert.Equal(Messages.InapplicableOperationInput('a'), _console.ErrorLines[1]);
            Assert.Equal(Messages.LongAreaAgnosticRunspecInput('q', "q+"), _console.ErrorLines[2]);
            Assert.Equal(Messages.LongAreaAwareRunspecInput('d', "d++"), _console.ErrorLines[3]);
            Assert.Equal(Messages.UnknownAreaInput('_'), _console.ErrorLines[4]);
        }
    }
}
