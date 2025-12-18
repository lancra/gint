using Gint.Console.Commands.Run;
using Gint.Core.IO;
using Gint.Core.Operations;
using Moq;
using Moq.AutoMock;

namespace Gint.Console.Facts.Commands.Run;

public class RunCommandHandlerFacts
{
    private readonly AutoMocker _mocker = new();

    private RunCommandHandler CreateSystemUnderTest()
        => _mocker.CreateInstance<RunCommandHandler>();

    public class TheHandleMethod : RunCommandHandlerFacts
    {
        [Fact]
        public async Task OpensPromptForPathspecInAllScope()
        {
            // Arrange
            var options = new RunCommandParameters
            {
                Pathspec = "foo.txt",
            };

            _mocker.GetMock<IRunPrompt>()
                .Setup(
                    prompt => prompt.Open(
                        It.Is<RunPromptContext>(context =>
                            context.Pathspec.Pattern == options.Pathspec &&
                            context.Scope == OperationScope.All),
                        TestContext.Current.CancellationToken))
                .ReturnsAsync([]);

            var sut = CreateSystemUnderTest();

            // Act
            await sut.HandleAsync(options, TestContext.Current.CancellationToken);

            // Assert
            _mocker.Verify();
        }

        [Fact]
        public async Task ReturnsSuccessExitCode()
        {
            // Arrange
            var options = new RunCommandParameters
            {
                Pathspec = "foo.txt",
            };

            _mocker.GetMock<IRunPrompt>()
                .Setup(
                    prompt => prompt.Open(
                        It.Is<RunPromptContext>(context =>
                            context.Pathspec.Pattern == options.Pathspec &&
                            context.Scope == OperationScope.All),
                        TestContext.Current.CancellationToken))
                .ReturnsAsync([]);

            var sut = CreateSystemUnderTest();

            // Act
            var exitCode = await sut.HandleAsync(options, TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(ExitCode.Success, exitCode);
        }
    }
}
