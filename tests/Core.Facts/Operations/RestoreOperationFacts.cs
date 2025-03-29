using Gint.Core.Changes;
using Gint.Core.Facts.Testbed;
using Gint.Core.Operations;
using Gint.Testbed.Creators;
using Moq.AutoMock;

namespace Gint.Core.Facts.Operations;

public class RestoreOperationFacts
{
    private readonly AutoMocker _mocker = new();

    private RestoreOperation CreateSystemUnderTest()
        => _mocker.CreateInstance<RestoreOperation>();

    public class TheExecuteMethod : RestoreOperationFacts
    {
        [Fact]
        public async Task ExecutesGitRestoreInWorkingAreaForPathspec()
        {
            // Arrange
            var context = OperationContextCreator.Create(pathspec: new("foo.txt"));

            _mocker.MockGitRun("restore foo.txt");
            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, default);

            // Assert
            Assert.True(result.Succeeded);
            _mocker.Verify();
        }

        [Fact]
        public async Task ExecutesGitRestoreInWorkingAreaWhenSpecified()
        {
            // Arrange
            var context = OperationContextCreator.Create(
                runspec: new(OperationDescriptor.Restore, ChangeArea.Working),
                pathspec: new("foo.txt"));

            _mocker.MockGitRun("restore foo.txt");
            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, default);

            // Assert
            Assert.True(result.Succeeded);
            _mocker.Verify();
        }

        [Fact]
        public async Task ExecutesGitRestoreInStagingAreaWhenSpecified()
        {
            // Arrange
            var context = OperationContextCreator.Create(
                runspec: new(OperationDescriptor.Restore, ChangeArea.Staging),
                pathspec: new("foo.txt"));

            _mocker.MockGitRun("restore --staged foo.txt");
            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, default);

            // Assert
            Assert.True(result.Succeeded);
            _mocker.Verify();
        }
    }
}
