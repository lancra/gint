using Gint.Core.Changes;
using Gint.Core.IO;
using Gint.Core.Operations;
using Gint.Testbed.Creators;
using Moq;
using Moq.AutoMock;

namespace Gint.Core.Facts.Operations;

public class BreakOperationFacts
{
    private readonly AutoMocker _mocker = new();

    private BreakOperation CreateSystemUnderTest()
        => _mocker.CreateInstance<BreakOperation>();

    private void MockPromptOpen(ChangeFile file, int expectedCounter, int totalFiles, IReadOnlyCollection<OperationResult> results)
        => _mocker.GetMock<IRunPrompt>()
        .Setup(prompt => prompt.Open(
            It.Is<RunPromptContext>(promptContext =>
                promptContext.Pathspec.Pattern == file.Path &&
                promptContext.Scope == OperationScope.File &&
                promptContext.Counter != null &&
                promptContext.Counter.Current == expectedCounter &&
                promptContext.Counter.Total == totalFiles),
            TestContext.Current.CancellationToken))
        .ReturnsAsync(results)
        .Verifiable();

    public class TheExecuteMethod : BreakOperationFacts
    {
        [Fact]
        public async Task OpensRunPromptForEachActionableFile()
        {
            // Arrange
            var fileOne = ChangesCreator.CreateFile();
            var fileTwo = ChangesCreator.CreateFile();
            var fileThree = ChangesCreator.CreateFile();
            var changes = ChangesCreator.CreateGroup([fileOne, fileTwo, fileThree]);
            var context = OperationContextCreator.Create(changes: changes);

            MockPromptOpen(fileOne, 1, changes.Files.Count, [OperationResult.NoOperation(context)]);
            MockPromptOpen(fileTwo, 2, changes.Files.Count, [OperationResult.NoOperation(context)]);
            MockPromptOpen(fileThree, 3, changes.Files.Count, [OperationResult.NoOperation(context)]);

            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, TestContext.Current.CancellationToken);

            // Assert
            Assert.True(result.Succeeded);
            _mocker.Verify();
        }

        [Fact]
        public async Task ReturnsFailedResultWhenAnyPromptFailed()
        {
            // Arrange
            var fileOne = ChangesCreator.CreateFile();
            var fileTwo = ChangesCreator.CreateFile();
            var changes = ChangesCreator.CreateGroup([fileOne, fileTwo]);
            var context = OperationContextCreator.Create(changes: changes);

            MockPromptOpen(
                fileOne,
                1,
                changes.Files.Count,
                [OperationResult.Command(context, GitCommandResultCreator.CreateRunFailure())]);
            MockPromptOpen(fileTwo, 2, changes.Files.Count, [OperationResult.NoOperation(context)]);

            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, TestContext.Current.CancellationToken);

            // Assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public async Task IgnoresFilesWithoutActionableChanges()
        {
            // Arrange
            var fileOne = ChangesCreator.CreateFile(indicator: ChangeIndicator.Ignored);
            var fileTwo = ChangesCreator.CreateFile();
            var fileThree = ChangesCreator.CreateFile();
            var changes = ChangesCreator.CreateGroup([fileOne, fileTwo, fileThree]);
            var context = OperationContextCreator.Create(changes: changes);

            MockPromptOpen(fileTwo, 1, 2, [OperationResult.NoOperation(context)]);
            MockPromptOpen(fileThree, 2, 2, [OperationResult.NoOperation(context)]);

            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, TestContext.Current.CancellationToken);

            // Assert
            Assert.True(result.Succeeded);
            _mocker.GetMock<IRunPrompt>()
                .Verify(
                    prompt => prompt.Open(It.IsAny<RunPromptContext>(), TestContext.Current.CancellationToken),
                    Times.Exactly(2));
        }

        [Fact]
        public async Task ExitsEarlyWhenQuitOperationIsExecuted()
        {
            // Arrange
            var fileOne = ChangesCreator.CreateFile();
            var fileTwo = ChangesCreator.CreateFile();
            var changes = ChangesCreator.CreateGroup([fileOne, fileTwo]);
            var context = OperationContextCreator.Create(changes: changes);
            var quitContext = context with { Runspec = new(OperationDescriptor.Quit, default), };

            MockPromptOpen(
                fileOne,
                1,
                changes.Files.Count,
                [OperationResult.Terminal(quitContext)]);
            MockPromptOpen(fileTwo, 2, changes.Files.Count, [OperationResult.NoOperation(context)]);

            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, TestContext.Current.CancellationToken);

            // Assert
            Assert.True(result.Succeeded);
            Assert.True(result.Exit);
            _mocker.GetMock<IRunPrompt>()
                .Verify(
                    prompt => prompt.Open(It.IsAny<RunPromptContext>(), TestContext.Current.CancellationToken),
                    Times.Once);
        }
    }
}
