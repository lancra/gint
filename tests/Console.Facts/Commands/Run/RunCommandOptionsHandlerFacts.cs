using Gint.Console.Commands.Run;
using Gint.Core.IO;
using Gint.Core.Operations;
using Moq;
using Moq.AutoMock;

namespace Gint.Console.Facts.Commands.Run;

public class RunCommandOptionsHandlerFacts
{
    private readonly AutoMocker _mocker = new();

    private RunCommandOptionsHandler CreateSystemUnderTest()
        => _mocker.CreateInstance<RunCommandOptionsHandler>();

    public class TheHandleMethod : RunCommandOptionsHandlerFacts
    {
        [Fact]
        public async Task OpensPromptForPathspecInAllScope()
        {
            // Arrange
            var options = new RunCommandOptions
            {
                Pathspec = "foo.txt",
            };

            _mocker.GetMock<IRunPrompt>()
                .Setup(
                    prompt => prompt.Open(
                        It.Is<RunPromptContext>(context =>
                            context.Pathspec.Pattern == options.Pathspec &&
                            context.Scope == OperationScope.All),
                        default))
                .ReturnsAsync([]);

            var sut = CreateSystemUnderTest();

            // Act
            await sut.Handle(options, default);

            // Assert
            _mocker.Verify();
        }

        [Fact]
        public async Task ReturnsSuccessExitCode()
        {
            // Arrange
            var options = new RunCommandOptions
            {
                Pathspec = "foo.txt",
            };

            _mocker.GetMock<IRunPrompt>()
                .Setup(
                    prompt => prompt.Open(
                        It.Is<RunPromptContext>(context =>
                            context.Pathspec.Pattern == options.Pathspec &&
                            context.Scope == OperationScope.All),
                        default))
                .ReturnsAsync([]);

            var sut = CreateSystemUnderTest();

            // Act
            var exitCode = await sut.Handle(options, default);

            // Assert
            Assert.Equal(ExitCode.Success, exitCode);
        }
    }
}
