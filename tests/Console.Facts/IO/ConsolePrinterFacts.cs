using Gint.Console.Facts.Testbed;
using Gint.Console.IO;
using Gint.Core.Changes;
using Gint.Core.Operations;
using Gint.Testbed.Creators;
using Moq.AutoMock;

namespace Gint.Console.Facts.IO;

public class ConsolePrinterFacts : IDisposable
{
    private readonly AutoMocker _mocker = new();
    private readonly TestApplicationConsole _console = new();

    public ConsolePrinterFacts()
        => _mocker.Use<IApplicationConsole>(_console);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _console.Dispose();
        }
    }

    private ConsolePrinter CreateSystemUnderTest()
        => _mocker.CreateInstance<ConsolePrinter>();

    public class ThePrintChangesMethod : ConsolePrinterFacts
    {
        [Fact]
        public void OutputsChangesUsingGitFormat()
        {
            // Arrange
            var changes = ChangesCreator.CreateGroup(
                ChangesCreator.CreateFile(
                    stagingIndicator: ChangeIndicator.Modified,
                    workingIndicator: ChangeIndicator.Unmodified,
                    path: "foo.txt"),
                ChangesCreator.CreateFile(
                    stagingIndicator: ChangeIndicator.TypeChanged,
                    workingIndicator: ChangeIndicator.Modified,
                    path: "bar.txt"),
                ChangesCreator.CreateFile(
                    stagingIndicator: ChangeIndicator.Unmodified,
                    workingIndicator: ChangeIndicator.Added,
                    path: "baz.txt"),
                ChangesCreator.CreateFile(
                    stagingIndicator: ChangeIndicator.Modified,
                    workingIndicator: ChangeIndicator.Modified,
                    path: "qux.txt"));

            var sut = CreateSystemUnderTest();

            // Act
            sut.PrintChanges(changes);

            // Assert
            Assert.Equal(4, _console.OutputLines.Count);
            Assert.Equal("M  foo.txt", _console.OutputLines[0]);
            Assert.Equal("TM bar.txt", _console.OutputLines[1]);
            Assert.Equal(" A baz.txt", _console.OutputLines[2]);
            Assert.Equal("MM qux.txt", _console.OutputLines[3]);
        }
    }

    public class ThePrintHelpMethod : ConsolePrinterFacts
    {
        [Fact]
        public void OutputsValueAndDescriptionForEachDescriptor()
        {
            // Arrange
            var descriptors = new List<OperationDescriptor>
            {
                OperationDescriptor.Add,
                OperationDescriptor.Diff,
                OperationDescriptor.IntendToAdd,
                OperationDescriptor.Restore,
            };

            var sut = CreateSystemUnderTest();

            // Act
            sut.PrintHelp(descriptors);

            // Assert
            Assert.Equal(4, _console.OutputLines.Count);
            Assert.Equal("a - Add changes to the staging area.", _console.OutputLines[0]);
            Assert.Equal("d - Show the differences introduced by changes.", _console.OutputLines[1]);
            Assert.Equal("n - Mark changes as intended to be added.", _console.OutputLines[2]);
            Assert.Equal("r - Restore changes.", _console.OutputLines[3]);
        }

        [Fact]
        public void OutputsNothingWhenNoDescriptorsProvided()
        {
            // Arrange
            var descriptors = Array.Empty<OperationDescriptor>();

            var sut = CreateSystemUnderTest();

            // Act
            sut.PrintHelp(descriptors);

            // Assert
            Assert.Empty(_console.OutputLines);
        }
    }
}
