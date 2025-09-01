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
        public async Task ReturnsSuccessfulChangeGroupResultRepresentingGitStatusOutput()
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
            var changesResult = await sut.Get(new(pathspec), TestContext.Current.CancellationToken);

            // Assert
            Assert.Empty(changesResult.Message);
            Assert.NotNull(changesResult.Changes);

            var changes = changesResult.Changes;
            Assert.Equal(5, changes.Files.Count);
            AssertFile(changes, ChangeIndicator.Unmodified, ChangeIndicator.Modified, "foo.txt");
            AssertFile(changes, ChangeIndicator.Added, ChangeIndicator.Unmodified, "bar.txt");
            AssertFile(changes, ChangeIndicator.TypeChanged, ChangeIndicator.Modified, "baz.txt");
            AssertFile(changes, ChangeIndicator.Untracked, ChangeIndicator.Untracked, "qux.txt");
            AssertFile(changes, ChangeIndicator.Ignored, ChangeIndicator.Ignored, "\"foo bar.txt\"");
        }

        [Fact]
        public async Task ReturnsErrorResultWhenExecutedInNonGitRepository()
        {
            // Arrange
            var pathspec = ".";
            var commandResult = new GitCommandResult(128, []);

            _mocker.MockGitRead($"status --short --untracked-files {pathspec}", commandResult);

            var sut = CreateSystemUnderTest();

            // Act
            var changesResult = await sut.Get(new(pathspec), TestContext.Current.CancellationToken);

            // Assert
            Assert.Null(changesResult.Changes);
            Assert.Equal(Messages.StatusAccessNonGitRepository, changesResult.Message);
        }

        [Fact]
        public async Task ReturnsErrorResultWhenFailureEncountered()
        {
            // Arrange
            var pathspec = ".";
            var commandResult = new GitCommandResult(1, []);

            _mocker.MockGitRead($"status --short --untracked-files {pathspec}", commandResult);

            var sut = CreateSystemUnderTest();

            // Act
            var changesResult = await sut.Get(new(pathspec), TestContext.Current.CancellationToken);

            // Assert
            Assert.Null(changesResult.Changes);
            Assert.Equal(Messages.StatusAccessFailure(commandResult.ExitCode), changesResult.Message);
        }
    }
}
