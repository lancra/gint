using Gint.Core.IO;
using Gint.Core.Operations;
using Gint.Testbed.Creators;
using Moq.AutoMock;

namespace Gint.Core.Facts.Operations;

public class StatusOperationFacts
{
    private readonly AutoMocker _mocker = new();

    private StatusOperation CreateSystemUnderTest()
        => _mocker.CreateInstance<StatusOperation>();

    public class TheExecuteMethod : StatusOperationFacts
    {
        [Fact]
        public async Task PrintsChangesFromContext()
        {
            // Arrange
            var context = OperationContextCreator.Create(changes: ChangesCreator.CreateGroup());
            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, TestContext.Current.CancellationToken);

            // Assert
            Assert.True(result.Succeeded);
            _mocker.GetMock<IPrinter>()
                .Verify(printer => printer.PrintChanges(context.Changes));
        }
    }
}
