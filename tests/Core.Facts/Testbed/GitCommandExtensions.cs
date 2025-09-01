using Gint.Core.External;
using Gint.Testbed.Creators;
using Moq;
using Moq.AutoMock;

namespace Gint.Core.Facts.Testbed;

internal static class GitCommandExtensions
{
    public static void MockGitRead(this AutoMocker mocker, string arguments, GitCommandResult result)
    {
        var argumentSegments = arguments.Split(' ');
        mocker.GetMock<IGitCommand>()
            .Setup(command => command.Read(TestContext.Current.CancellationToken, argumentSegments))
            .ReturnsAsync(result)
            .Verifiable();
    }

    public static void MockGitRun(this AutoMocker mocker, string arguments, GitCommandResult? result = default)
    {
        var argumentSegments = arguments.Split(' ');
        result ??= GitCommandResultCreator.CreateRunSuccess();

        mocker.GetMock<IGitCommand>()
            .Setup(command => command.Run(TestContext.Current.CancellationToken, argumentSegments))
            .ReturnsAsync(result)
            .Verifiable();
    }
}
