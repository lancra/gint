using Gint.Core.IO;
using Gint.Core.Operations;
using Gint.Testbed.Creators;
using Moq;
using Moq.AutoMock;

namespace Gint.Core.Facts.Operations;

public class HelpOperationFacts
{
    private readonly AutoMocker _mocker = new();

    private HelpOperation CreateSystemUnderTest()
        => _mocker.CreateInstance<HelpOperation>();

    public class TheExecuteMethod : HelpOperationFacts
    {
        [Fact]
        public async Task PrintsHelpForDescriptorsApplicableToContext()
        {
            // Arrange
            var context = OperationContextCreator.Create();

            OperationDescriptor[] expectedDescriptors =
            [
                OperationDescriptor.Add,
                OperationDescriptor.Clean,
                OperationDescriptor.Diff,
            ];
            _mocker.GetMock<IOperationAccessor>()
                .Setup(operationAccessor => operationAccessor.Filter(
                    It.Is<OperationFilter>(filter =>
                        filter.Scope == context.Scope &&
                        filter.Changes == context.Changes)))
                .Returns(expectedDescriptors);

            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Execute(context, default);

            // Assert
            Assert.True(result.Succeeded);
            _mocker.GetMock<IPrinter>()
                .Verify(printer => printer.PrintHelp(expectedDescriptors));
        }
    }
}
