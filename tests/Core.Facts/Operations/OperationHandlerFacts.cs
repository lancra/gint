using Gint.Core.Operations;
using Gint.Testbed.Creators;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.AutoMock;

namespace Gint.Core.Facts.Operations;

public class OperationHandlerFacts
{
    private readonly AutoMocker _mocker = new();

    private OperationHandler CreateSystemUnderTest()
        => _mocker.CreateInstance<OperationHandler>();

    public class TheExecuteMethod : OperationHandlerFacts
    {
        [Fact]
        public async Task ExecutesKeyedOperationFromRunspec()
        {
            // Arrange
            var descriptor = OperationDescriptor.Add;
            var context = OperationContextCreator.Create(runspec: new(descriptor, default));

            var expectedResult = OperationResult.NoOperation(context);
            var mockOperation = new Mock<IInternalOperation>();
            mockOperation.Setup(operation => operation.Execute(context, TestContext.Current.CancellationToken))
                .ReturnsAsync(expectedResult);

            var serviceProvider = new ServiceCollection()
                .AddKeyedScoped(typeof(IInternalOperation), descriptor.Value, (provider, descriptorValue) => mockOperation.Object)
                .BuildServiceProvider();
            _mocker.Use<IServiceProvider>(serviceProvider);

            var sut = CreateSystemUnderTest();

            // Act
            var actualResult = await sut.Execute(context, TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task ThrowsInvalidOperationExceptionWhenOperationIsNotRegistered()
        {
            // Arrange
            var descriptor = OperationDescriptor.Add;
            var context = OperationContextCreator.Create(runspec: new(descriptor, default));

            var serviceProvider = new ServiceCollection()
                .BuildServiceProvider();
            _mocker.Use<IServiceProvider>(serviceProvider);

            var sut = CreateSystemUnderTest();

            // Act
            var exception = await Record.ExceptionAsync(async () => await sut.Execute(context, TestContext.Current.CancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }
    }
}
