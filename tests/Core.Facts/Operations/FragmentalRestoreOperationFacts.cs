using Gint.Core.Changes;
using Gint.Core.Facts.Testbed;
using Gint.Core.Operations;
using Gint.Testbed.Creators;
using Moq.AutoMock;

namespace Gint.Core.Facts.Operations;

public class FragmentalRestoreOperationFacts
{
    private readonly AutoMocker _mocker = new();

    private FragmentalRestoreOperation CreateSystemUnderTest()
        => _mocker.CreateInstance<FragmentalRestoreOperation>();

    public class TheExecuteMethod : FragmentalRestoreOperationFacts
    {
        [Fact]
        public async Task ExecutesGitRestorePatchInWorkingAreaForPathspec()
        {
            // Arrange
            var context = OperationContextCreator.Create(pathspec: new("foo.txt"));

            _mocker.MockGitRun("restore --patch foo.txt");
            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, TestContext.Current.CancellationToken);

            // Assert
            Assert.True(result.Succeeded);
            _mocker.Verify();
        }

        [Fact]
        public async Task ExecutesGitRestorePatchInWorkingAreaWhenSpecified()
        {
            // Arrange
            var context = OperationContextCreator.Create(
                runspec: new(OperationDescriptor.Diff, ChangeArea.Working),
                pathspec: new("foo.txt"));

            _mocker.MockGitRun("restore --patch foo.txt");
            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, TestContext.Current.CancellationToken);

            // Assert
            Assert.True(result.Succeeded);
            _mocker.Verify();
        }

        [Fact]
        public async Task ExecutesGitRestorePatchInStagingAreaWhenSpecified()
        {
            // Arrange
            var context = OperationContextCreator.Create(
                runspec: new(OperationDescriptor.Diff, ChangeArea.Staging),
                pathspec: new("foo.txt"));

            _mocker.MockGitRun("restore --patch --staged foo.txt");
            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, TestContext.Current.CancellationToken);

            // Assert
            Assert.True(result.Succeeded);
            _mocker.Verify();
        }
    }
}
