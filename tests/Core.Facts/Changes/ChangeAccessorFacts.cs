using Gint.Core.Changes;
using Gint.Core.External;
using Gint.Core.Facts.Testbed;
using Gint.Core.Properties;
using Moq.AutoMock;

namespace Gint.Core.Facts.Changes;

public class ChangeAccessorFacts
{
    private readonly AutoMocker _mocker = new();

    private ChangeAccessor CreateSystemUnderTest()
        => _mocker.CreateInstance<ChangeAccessor>();

    public class TheGetMethod : ChangeAccessorFacts
    {
        [Fact]
        public async Task ReturnsChangeGroupRepresentingGitStatusOutput()
        {
            // Arrange
            var pathspec = ".";
            var commandResult = new GitCommandResult(
                0,
                [
                    " M foo.txt",
                    "A  bar.txt",
                    "TM baz.txt",
                    "?? qux.txt",
                    "!! \"foo bar.txt\"",
                ]);

            _mocker.MockGitRead($"status --short --untracked-files {pathspec}", commandResult);

            var sut = CreateSystemUnderTest();

            void AssertFile(ChangeGroup changes, ChangeIndicator stagingIndicator, ChangeIndicator workingIndicator, string path)
                => Assert.Single(
                    changes.Files,
                    file =>
                        file.Indicators.Any(areaIndicator =>
                            areaIndicator.Area == ChangeArea.Staging &&
                            areaIndicator.Indicator == stagingIndicator) &&
                        file.Indicators.Any(areaIndicator =>
                            areaIndicator.Area == ChangeArea.Working &&
                            areaIndicator.Indicator == workingIndicator) &&
                        file.Path == path);

            // Act
            var changes = await sut.Get(new(pathspec), default);

            // Assert
            Assert.Equal(5, changes.Files.Count);
            AssertFile(changes, ChangeIndicator.Unmodified, ChangeIndicator.Modified, "foo.txt");
            AssertFile(changes, ChangeIndicator.Added, ChangeIndicator.Unmodified, "bar.txt");
            AssertFile(changes, ChangeIndicator.TypeChanged, ChangeIndicator.Modified, "baz.txt");
            AssertFile(changes, ChangeIndicator.Untracked, ChangeIndicator.Untracked, "qux.txt");
            AssertFile(changes, ChangeIndicator.Ignored, ChangeIndicator.Ignored, "\"foo bar.txt\"");
        }

        [Fact]
        public async Task ThrowsInvalidOperationExceptionWhenGitStatusCommandFails()
        {
            // Arrange
            var pathspec = ".";
            var commandResult = new GitCommandResult(1, []);

            _mocker.MockGitRead($"status --short --untracked-files {pathspec}", commandResult);

            var sut = CreateSystemUnderTest();

            // Act
            var exception = await Record.ExceptionAsync(async () => await sut.Get(new(pathspec), default));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
            Assert.Equal(Messages.StatusAccessFailure(commandResult.ExitCode), exception.Message);
        }
    }
}
