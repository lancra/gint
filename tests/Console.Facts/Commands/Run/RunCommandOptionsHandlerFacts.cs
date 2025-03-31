using Gint.Console.Commands.Run;
using Gint.Core.IO;
using Gint.Core.Operations;
using Gint.Testbed.Creators;
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
        public async Task ReturnsSuccessExitCodeWhenAllResultsSucceeded()
        {
            // Arrange
            var options = new RunCommandOptions
            {
                Pathspec = "foo.txt",
            };

            OperationResult[] results =
            [
                OperationResult.NoOperation(OperationContextCreator.Create()),
                OperationResult.NoOperation(OperationContextCreator.Create()),
            ];
            _mocker.GetMock<IRunPrompt>()
                .Setup(prompt => prompt.Open(
                    It.Is<RunPromptContext>(context =>
                        context.Pathspec.Pattern == options.Pathspec &&
                        context.Scope == OperationScope.All),
                    default))
                .ReturnsAsync(results);

            var sut = CreateSystemUnderTest();

            // Act
            var exitCode = await sut.Handle(options, default);

            // Assert
            Assert.Equal(ExitCode.Success, exitCode);
        }

        [Fact]
        public async Task ReturnsErrorExitCodeWhenAnyResultsFailed()
        {
            // Arrange
            var options = new RunCommandOptions
            {
                Pathspec = "foo.txt",
            };

            OperationResult[] results =
            [
                OperationResult.NoOperation(OperationContextCreator.Create()),
                OperationResult.Command(OperationContextCreator.Create(), GitCommandResultCreator.CreateRunFailure()),
                OperationResult.NoOperation(OperationContextCreator.Create()),
            ];
            _mocker.GetMock<IRunPrompt>()
                .Setup(prompt => prompt.Open(
                    It.Is<RunPromptContext>(context =>
                        context.Pathspec.Pattern == options.Pathspec &&
                        context.Scope == OperationScope.All),
                    default))
                .ReturnsAsync(results);

            var sut = CreateSystemUnderTest();

            // Act
            var exitCode = await sut.Handle(options, default);

            // Assert
            Assert.Equal(ExitCode.Error, exitCode);
        }
    }
}
