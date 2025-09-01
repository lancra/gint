using Gint.Core.Operations;
using Gint.Testbed.Creators;
using Moq.AutoMock;

namespace Gint.Core.Facts.Operations;

public class QuitOperationFacts
{
    private readonly AutoMocker _mocker = new();

    private QuitOperation CreateSystemUnderTest()
        => _mocker.CreateInstance<QuitOperation>();

    public class TheExecuteMethod : QuitOperationFacts
    {
        [Fact]
        public async Task ReturnsTerminalOperationResult()
        {
            // Arrange
            var context = OperationContextCreator.Create(scope: OperationScope.File);
            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, TestContext.Current.CancellationToken);

            // Assert
            Assert.True(result.Succeeded);
            Assert.True(result.Exit);
        }
    }
}
