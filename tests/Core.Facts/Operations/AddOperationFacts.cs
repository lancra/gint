using Gint.Core.Facts.Testbed;
using Gint.Core.Operations;
using Gint.Testbed.Creators;
using Moq.AutoMock;

namespace Gint.Core.Facts.Operations;

public class AddOperationFacts
{
    private readonly AutoMocker _mocker = new();

    private AddOperation CreateSystemUnderTest()
        => _mocker.CreateInstance<AddOperation>();

    public class TheExecuteMethod : AddOperationFacts
    {
        [Fact]
        public async Task ExecutesGitAddForPathspec()
        {
            // Arrange
            var context = OperationContextCreator.Create(pathspec: new("foo.txt"));

            _mocker.MockGitRun("add foo.txt");
            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, TestContext.Current.CancellationToken);

            // Assert
            Assert.True(result.Succeeded);
            _mocker.Verify();
        }
    }
}
