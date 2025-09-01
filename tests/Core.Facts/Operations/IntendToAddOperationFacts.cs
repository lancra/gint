using Gint.Core.Facts.Testbed;
using Gint.Core.Operations;
using Gint.Testbed.Creators;
using Moq.AutoMock;

namespace Gint.Core.Facts.Operations;

public class IntendToAddOperationFacts
{
    private readonly AutoMocker _mocker = new();

    private IntendToAddOperation CreateSystemUnderTest()
        => _mocker.CreateInstance<IntendToAddOperation>();

    public class TheExecuteMethod : IntendToAddOperationFacts
    {
        [Fact]
        public async Task ExecutesGitIntentToAddForPathspec()
        {
            // Arrange
            var context = OperationContextCreator.Create(pathspec: new("foo.txt"));

            _mocker.MockGitRun("add --intent-to-add foo.txt");
            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, TestContext.Current.CancellationToken);

            // Assert
            Assert.True(result.Succeeded);
            _mocker.Verify();
        }
    }
}
