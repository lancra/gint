using Gint.Core.Changes;
using Gint.Core.Operations;
using Gint.Testbed.Creators;

namespace Gint.Core.Facts.Operations;

public class OperationResultFacts
{
    public class TheCommandMethod : OperationResultFacts
    {
        [Fact]
        public void ReturnsResultContainingOperationContext()
        {
            // Arrange
            var context = OperationContextCreator.Create();
            var commandResult = GitCommandResultCreator.CreateRunSuccess();

            // Act
            var result = OperationResult.Command(context, commandResult);

            // Assert
            Assert.Equal(context, result.Context);
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(0, true)]
        public void ReturnsResultIndicatingSuccessUsingCommandResult(int exitCode, bool succeeded)
        {
            // Arrange
            var context = OperationContextCreator.Create();
            var commandResult = GitCommandResultCreator.CreateRun(exitCode);

            // Act
            var result = OperationResult.Command(context, commandResult);

            // Assert
            Assert.Equal(succeeded, result.Succeeded);
        }

        [Fact]
        public void ReturnsResultIndicatingWritesWhenCommandSucceededForWriteDescriptor()
        {
            // Arrange
            var context = OperationContextCreator.Create(runspec: new(OperationDescriptor.Add, default));
            var commandResult = GitCommandResultCreator.CreateRunSuccess();

            // Act
            var result = OperationResult.Command(context, commandResult);

            // Assert
            Assert.True(result.Wrote);
        }

        [Fact]
        public void ReturnsResultIndicatingNoWritesWhenCommandFailedForWriteDescriptor()
        {
            // Arrange
            var context = OperationContextCreator.Create(runspec: new(OperationDescriptor.Add, default));
            var commandResult = GitCommandResultCreator.CreateRunFailure();

            // Act
            var result = OperationResult.Command(context, commandResult);

            // Assert
            Assert.False(result.Wrote);
        }

        [Fact]
        public void ReturnsResultIndicatingNoWritesWhenCommandSucceededForReadDescriptor()
        {
            // Arrange
            var context = OperationContextCreator.Create(runspec: new(OperationDescriptor.Diff, ChangeArea.Working));
            var commandResult = GitCommandResultCreator.CreateRunSuccess();

            // Act
            var result = OperationResult.Command(context, commandResult);

            // Assert
            Assert.False(result.Wrote);
        }

        [Fact]
        public void ReturnsResultIndicatingExitWhenCommandWroteChangesAtFileScope()
        {
            // Arrange
            var context = OperationContextCreator.Create(runspec: new(OperationDescriptor.Add, default), scope: OperationScope.File);
            var commandResult = GitCommandResultCreator.CreateRunSuccess();

            // Act
            var result = OperationResult.Command(context, commandResult);

            // Assert
            Assert.True(result.Exit);
        }
    }

    public class TheDerivedMethod : OperationResultFacts
    {
        [Fact]
        public void ReturnsResultContainingOperationContext()
        {
            // Arrange
            var context = OperationContextCreator.Create();

            // Act
            var result = OperationResult.Derived(context, []);

            // Assert
            Assert.Equal(context, result.Context);
        }

        [Fact]
        public void ReturnsResultContainingInnerResults()
        {
            // Arrange
            var context = OperationContextCreator.Create();

            var innerResultOne = OperationResult.Command(OperationContextCreator.Create(), GitCommandResultCreator.CreateRunSuccess());
            var innerResultTwo = OperationResult.Command(OperationContextCreator.Create(), GitCommandResultCreator.CreateRunSuccess());

            // Act
            var result = OperationResult.Derived(context, [innerResultOne, innerResultTwo]);

            // Assert
            Assert.Equal(2, result.InnerResults.Count);
            Assert.Contains(result.InnerResults, innerResult => innerResult == innerResultOne);
            Assert.Contains(result.InnerResults, innerResult => innerResult == innerResultTwo);
        }

        [Fact]
        public void ReturnsResultIndicatingSuccessWhenAllInnerResultsSucceeded()
        {
            // Arrange
            var context = OperationContextCreator.Create();
            var innerResults = new List<OperationResult>
            {
                OperationResult.Command(OperationContextCreator.Create(), GitCommandResultCreator.CreateRunSuccess()),
                OperationResult.Command(OperationContextCreator.Create(), GitCommandResultCreator.CreateRunSuccess()),
            };

            // Act
            var result = OperationResult.Derived(context, innerResults);

            // Assert
            Assert.True(result.Succeeded);
        }

        [Fact]
        public void ReturnsResultIndicatingFailureWhenAnyInnerResultsFailed()
        {
            // Arrange
            var context = OperationContextCreator.Create();
            var innerResults = new List<OperationResult>
            {
                OperationResult.Command(OperationContextCreator.Create(), GitCommandResultCreator.CreateRunSuccess()),
                OperationResult.Command(OperationContextCreator.Create(), GitCommandResultCreator.CreateRunFailure()),
                OperationResult.Command(OperationContextCreator.Create(), GitCommandResultCreator.CreateRunSuccess()),
            };

            // Act
            var result = OperationResult.Derived(context, innerResults);

            // Assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void ReturnsResultIndicatingWritesWhenAnyCommandSucceededForWriteDescriptor()
        {
            // Arrange
            var context = OperationContextCreator.Create();
            var innerResults = new List<OperationResult>
            {
                OperationResult.Command(
                    OperationContextCreator.Create(runspec: new(OperationDescriptor.Add, default)),
                    GitCommandResultCreator.CreateRunSuccess()),
                OperationResult.Command(
                    OperationContextCreator.Create(runspec: new(OperationDescriptor.Add, default)),
                    GitCommandResultCreator.CreateRunFailure()),
                OperationResult.Command(
                    OperationContextCreator.Create(runspec: new(OperationDescriptor.Diff, ChangeArea.Working)),
                    GitCommandResultCreator.CreateRunSuccess()),
            };

            // Act
            var result = OperationResult.Derived(context, innerResults);

            // Assert
            Assert.True(result.Wrote);
        }

        [Fact]
        public void ReturnsResultIndicatingNoWritesWhenNoCommandSucceededForWriteDescriptor()
        {
            // Arrange
            var context = OperationContextCreator.Create();
            var innerResults = new List<OperationResult>
            {
                OperationResult.Command(
                    OperationContextCreator.Create(runspec: new(OperationDescriptor.Add, default)),
                    GitCommandResultCreator.CreateRunFailure()),
                OperationResult.Command(
                    OperationContextCreator.Create(runspec: new(OperationDescriptor.Diff, ChangeArea.Working)),
                    GitCommandResultCreator.CreateRunSuccess()),
            };

            // Act
            var result = OperationResult.Derived(context, innerResults);

            // Assert
            Assert.False(result.Wrote);
        }

        [Fact]
        public void ReturnsResultIndicatingExitWhenCommandWroteChangesAtFileScope()
        {
            // Arrange
            var context = OperationContextCreator.Create(scope: OperationScope.File);
            var innerResults = new List<OperationResult>
            {
                OperationResult.Command(
                    OperationContextCreator.Create(runspec: new(OperationDescriptor.Add, default)),
                    GitCommandResultCreator.CreateRunSuccess()),
                OperationResult.Command(
                    OperationContextCreator.Create(runspec: new(OperationDescriptor.Diff, ChangeArea.Working)),
                    GitCommandResultCreator.CreateRunSuccess()),
            };

            // Act
            var result = OperationResult.Derived(context, innerResults);

            // Assert
            Assert.True(result.Exit);
        }

        [Fact]
        public void ReturnsResultIndicatingExitWhenAnyQuitDescriptorSpecified()
        {
            // Arrange
            var context = OperationContextCreator.Create();
            var innerResults = new List<OperationResult>
            {
                OperationResult.Command(
                    OperationContextCreator.Create(runspec: new(OperationDescriptor.Add, default)),
                    GitCommandResultCreator.CreateRunSuccess()),
                OperationResult.Command(
                    OperationContextCreator.Create(runspec: new(OperationDescriptor.Quit, default)),
                    GitCommandResultCreator.CreateRunSuccess()),
            };

            // Act
            var result = OperationResult.Derived(context, innerResults);

            // Assert
            Assert.True(result.Exit);
        }
    }

    public class TheNoOperationMethod : OperationResultFacts
    {
        [Fact]
        public void ReturnsResultContainingOperationContext()
        {
            // Arrange
            var context = OperationContextCreator.Create();

            // Act
            var result = OperationResult.NoOperation(context);

            // Assert
            Assert.Equal(context, result.Context);
        }

        [Fact]
        public void ReturnsResultIndicatingNoOperationPerformed()
        {
            // Arrange
            var context = OperationContextCreator.Create();

            // Act
            var result = OperationResult.NoOperation(context);

            // Assert
            Assert.True(result.Succeeded);
            Assert.False(result.Wrote);
            Assert.False(result.Exit);
        }
    }

    public class TheTerminalMethod : OperationResultFacts
    {
        [Fact]
        public void ReturnsResultContainingOperationContext()
        {
            // Arrange
            var context = OperationContextCreator.Create();

            // Act
            var result = OperationResult.Terminal(context);

            // Assert
            Assert.Equal(context, result.Context);
        }

        [Fact]
        public void ReturnsResultIndicatingSpecificationOfTerminalOperation()
        {
            // Arrange
            var context = OperationContextCreator.Create();

            // Act
            var result = OperationResult.Terminal(context);

            // Assert
            Assert.True(result.Succeeded);
            Assert.False(result.Wrote);
            Assert.True(result.Exit);
        }
    }
}
