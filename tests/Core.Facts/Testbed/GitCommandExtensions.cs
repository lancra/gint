using Gint.Core.External;
using Gint.Testbed.Creators;
using Moq;
using Moq.AutoMock;

namespace Gint.Core.Facts.Testbed;

internal static class GitCommandExtensions
{
    public static void MockGitRun(this AutoMocker mocker, string arguments, GitCommandResult? result = default)
    {
        var argumentSegments = arguments.Split(' ');
        result ??= GitCommandResultCreator.CreateRunSuccess();

        mocker.GetMock<IGitCommand>()
            .Setup(command => command.Run(default, argumentSegments))
            .ReturnsAsync(result)
            .Verifiable();
    }
}
