using Gint.Core.Operations;
using Gint.Testbed.Creators;
using Moq.AutoMock;

namespace Gint.Core.Facts.Operations;

public class IgnoreOperationFacts
{
    private readonly AutoMocker _mocker = new();

    private IgnoreOperation CreateSystemUnderTest()
        => _mocker.CreateInstance<IgnoreOperation>();

    public class TheExecuteMethod : IgnoreOperationFacts
    {
        [Fact]
        public async Task ReturnsTerminalOperationResult()
        {
            // Arrange
            var context = OperationContextCreator.Create(scope: OperationScope.File);
            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, default);

            // Assert
            Assert.True(result.Succeeded);
            Assert.True(result.Exit);
        }
    }
}
