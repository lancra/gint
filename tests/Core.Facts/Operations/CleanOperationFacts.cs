using Gint.Core.Facts.Testbed;
using Gint.Core.Operations;
using Gint.Testbed.Creators;
using Moq.AutoMock;

namespace Gint.Core.Facts.Operations;

public class CleanOperationFacts
{
    private readonly AutoMocker _mocker = new();

    private CleanOperation CreateSystemUnderTest()
        => _mocker.CreateInstance<CleanOperation>();

    public class TheExecuteMethod : CleanOperationFacts
    {
        [Fact]
        public async Task ExecutesGitCleanForceForPathspec()
        {
            // Arrange
            var context = OperationContextCreator.Create(pathspec: new("foo.txt"));

            _mocker.MockGitRun("clean --force foo.txt");
            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, default);

            // Assert
            Assert.True(result.Succeeded);
            _mocker.Verify();
        }
    }
}
