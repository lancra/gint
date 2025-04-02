using Gint.Core.External;
using Moq;
using Moq.AutoMock;
using SimpleExec;

namespace Gint.Core.Facts.External;

public class GitCommandFacts
{
    private readonly AutoMocker _mocker = new();

    private GitCommand CreateSystemUnderTest()
        => _mocker.CreateInstance<GitCommand>();

    public class TheReadMethod : GitCommandFacts
    {
        [Fact]
        public async Task ReturnsSuccessfulResult()
        {
            // Arrange
            string[] arguments = ["add", "foo.txt"];

            _mocker.GetMock<ICommand>()
                .Setup(command => command.Read("git", arguments, default))
                .ReturnsAsync((string.Empty, string.Empty));

            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Read(default, arguments);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(0, result.ExitCode);
        }

        [Fact]
        public async Task ReturnsResultContainingOutputLinesSplitByUnixNewline()
        {
            // Arrange
            string[] arguments = ["add", "foo.txt"];

            _mocker.GetMock<ICommand>()
                .Setup(command => command.Read("git", arguments, default))
                .ReturnsAsync(("foo\nbar\nbaz", string.Empty));

            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Read(default, arguments);

            // Assert
            Assert.Equal(3, result.Output.Count);

            var outputLines = result.Output.ToList();
            Assert.Equal("foo", outputLines[0]);
            Assert.Equal("bar", outputLines[1]);
            Assert.Equal("baz", outputLines[2]);
        }

        [Fact]
        public async Task ReturnsEmptyOutputLinesWhenOutputIsNull()
        {
            // Arrange
            string[] arguments = ["add", "foo.txt"];

            _mocker.GetMock<ICommand>()
                .Setup(command => command.Read("git", arguments, default))
                .ReturnsAsync((default!, string.Empty));

            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Read(default, arguments);

            // Assert
            Assert.Empty(result.Output);
        }

        [Fact]
        public async Task ReturnsFailedResultWhenCommandThrowsExitCodeException()
        {
            // Arrange
            string[] arguments = ["add", "foo.txt"];

            var exception = new ExitCodeException(1);
            _mocker.GetMock<ICommand>()
                .Setup(command => command.Read("git", arguments, default))
                .ThrowsAsync(exception);

            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Read(default, arguments);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(exception.ExitCode, result.ExitCode);
        }
    }

    public class TheRunMethod : GitCommandFacts
    {
        [Fact]
        public async Task ReturnsSuccessfulResult()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Run(default, ["add", "foo.txt"]);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(0, result.ExitCode);
        }

        [Fact]
        public async Task ReturnsFailedResultWhenCommandThrowsExitCodeException()
        {
            // Arrange
            string[] arguments = ["add", "foo.txt"];

            var exception = new ExitCodeException(1);
            _mocker.GetMock<ICommand>()
                .Setup(command => command.Run("git", arguments, default))
                .ThrowsAsync(exception);

            var sut = CreateSystemUnderTest();

            // Act
            var result = await sut.Run(default, arguments);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(exception.ExitCode, result.ExitCode);
        }
    }
}
