using Gint.Core.Changes;
using Gint.Core.Facts.Testbed;
using Gint.Core.Operations;
using Gint.Testbed.Creators;
using Moq.AutoMock;

namespace Gint.Core.Facts.Operations;

public class DiffOperationFacts
{
    private readonly AutoMocker _mocker = new();

    private DiffOperation CreateSystemUnderTest()
        => _mocker.CreateInstance<DiffOperation>();

    public class TheExecuteMethod : DiffOperationFacts
    {
        [Fact]
        public async Task ExecutesGitDiffInWorkingAreaForPathspec()
        {
            // Arrange
            var context = OperationContextCreator.Create(pathspec: new("foo.txt"));

            _mocker.MockGitRun("diff foo.txt");
            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, default);

            // Assert
            Assert.True(result.Succeeded);
            _mocker.Verify();
        }

        [Fact]
        public async Task ExecutesGitDiffInWorkingAreaWhenSpecified()
        {
            // Arrange
            var context = OperationContextCreator.Create(
                runspec: new(OperationDescriptor.Diff, ChangeArea.Working),
                pathspec: new("foo.txt"));

            _mocker.MockGitRun("diff foo.txt");
            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, default);

            // Assert
            Assert.True(result.Succeeded);
            _mocker.Verify();
        }

        [Fact]
        public async Task ExecutesGitDiffInStagingAreaWhenSpecified()
        {
            // Arrange
            var context = OperationContextCreator.Create(
                runspec: new(OperationDescriptor.Diff, ChangeArea.Staging),
                pathspec: new("foo.txt"));

            _mocker.MockGitRun("diff --staged foo.txt");
            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, default);

            // Assert
            Assert.True(result.Succeeded);
            _mocker.Verify();
        }
    }
}
