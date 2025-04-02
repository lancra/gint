using Gint.Core.Changes;
using Gint.Core.Facts.Testbed;
using Gint.Core.Operations;
using Gint.Testbed.Creators;
using Moq;
using Moq.AutoMock;

namespace Gint.Core.Facts.Operations;

public class PatchOperationFacts
{
    private readonly AutoMocker _mocker = new();

    private PatchOperation CreateSystemUnderTest()
        => _mocker.CreateInstance<PatchOperation>();

    public class TheExecuteMethod : PatchOperationFacts
    {
        [Fact]
        public async Task ExecutesGitAddPatchForPathspec()
        {
            // Arrange
            var context = OperationContextCreator.Create(pathspec: new("foo.txt"));

            _mocker.MockGitRun("add --patch foo.txt");
            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, default);

            // Assert
            Assert.True(result.Succeeded);
            _mocker.Verify();
        }

        [Fact]
        public async Task ExecutesIntendToAddOperationPriorToGitAddPatchForUntrackedFile()
        {
            // Arrange
            var file = "foo.txt";
            var context = OperationContextCreator.Create(
                pathspec: new(file),
                changes: ChangesCreator.CreateGroup(
                    ChangesCreator.CreateFile(ChangeIndicator.Untracked, file)));

            _mocker.GetMock<IOperation>()
                .Setup(operation => operation.Execute(
                    It.Is<OperationContext>(context => true),
                    default))
                .ReturnsAsync((OperationContext innerContext, CancellationToken _)
                    => OperationResult.Command(innerContext, GitCommandResultCreator.CreateRunSuccess()))
                .Verifiable();

            _mocker.MockGitRun("add --patch foo.txt");
            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, default);

            // Assert
            Assert.True(result.Succeeded);
            _mocker.Verify();
        }

        [Fact]
        public async Task ReturnsIntendToAddOperationResultWhenFailedForUntrackedFile()
        {
            // Arrange
            var file = "foo.txt";
            var context = OperationContextCreator.Create(
                pathspec: new(file),
                changes: ChangesCreator.CreateGroup(
                    ChangesCreator.CreateFile(ChangeIndicator.Untracked, file)));

            _mocker.GetMock<IOperation>()
                .Setup(operation => operation.Execute(
                    It.Is<OperationContext>(context => true),
                    default))
                .ReturnsAsync((OperationContext innerContext, CancellationToken _)
                    => OperationResult.Command(innerContext, GitCommandResultCreator.CreateRunFailure()));

            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, default);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(OperationDescriptor.IntendToAdd, result.Context.Runspec.Descriptor);
        }

        [Fact]
        public async Task ReturnsFailedResultWhenAddPatchFails()
        {
            // Arrange
            var file = "foo.txt";
            var context = OperationContextCreator.Create(
                pathspec: new(file),
                changes: ChangesCreator.CreateGroup(
                    ChangesCreator.CreateFile(ChangeIndicator.Untracked, file)));

            _mocker.GetMock<IOperation>()
                .Setup(operation => operation.Execute(
                    It.Is<OperationContext>(context => true),
                    default))
                .ReturnsAsync((OperationContext innerContext, CancellationToken _)
                    => OperationResult.Command(innerContext, GitCommandResultCreator.CreateRunSuccess()));

            _mocker.MockGitRun("add --patch foo.txt", GitCommandResultCreator.CreateRunFailure());
            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, default);

            // Assert
            Assert.False(result.Succeeded);
        }
    }
}
