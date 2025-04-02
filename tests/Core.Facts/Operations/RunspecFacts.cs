using Gint.Core.Changes;
using Gint.Core.Operations;
using Gint.Core.Properties;

namespace Gint.Core.Facts.Operations;

public class RunspecFacts
{
    public class TheParseMethod : RunspecFacts
    {
        public static readonly TheoryData<string, string, string?> ValidRunspecInputs =
            new()
            {
                { "s", OperationDescriptor.Status.Name, null },
                { "d", OperationDescriptor.Diff.Name, null },
                { "f+", OperationDescriptor.FragmentalRestore.Name, ChangeArea.Staging.Name },
                { "r-", OperationDescriptor.Restore.Name, ChangeArea.Working.Name },
            };

        [Theory]
        [MemberData(nameof(ValidRunspecInputs))]
        public void ReturnsSuccessWhenRunspecIsValid(string text, string expectedDescriptorValue, string? expectedAreaValue)
        {
            // Arrange
            var expectedDescriptor = OperationDescriptor.FromName(expectedDescriptorValue);
            var expectedArea = !string.IsNullOrEmpty(expectedAreaValue) ? ChangeArea.FromName(expectedAreaValue) : default;

            // Act
            var result = Runspec.Parse(text, OperationDescriptor.List);

            // Assert
            Assert.Equal(string.Empty, result.Message);
            Assert.NotNull(result.Runspec);
            Assert.Equal(expectedDescriptor, result.Runspec.Descriptor);
            Assert.Equal(expectedArea, result.Runspec.Area);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ReturnsErrorWithoutMessageForMissingText(string? text)
        {
            // Act
            var result = Runspec.Parse(text, []);

            // Assert
            Assert.Equal(string.Empty, result.Message);
            Assert.Null(result.Runspec);
        }

        [Fact]
        public void ReturnsUnknownOperationErrorWhenOperationLetterIsInvalid()
        {
            // Arrange
            var text = "z";

            // Act
            var result = Runspec.Parse(text, []);

            // Assert
            Assert.Equal(Messages.UnknownOperationInput(text), result.Message);
            Assert.Null(result.Runspec);
        }

        [Fact]
        public void ReturnsInapplicableOperationErrorWhenOperationIsNotInProvidedOptions()
        {
            // Arrange
            var text = "d";

            // Act
            var result = Runspec.Parse(text, [OperationDescriptor.Add]);

            // Assert
            Assert.Equal(Messages.InapplicableOperationInput(text), result.Message);
            Assert.Null(result.Runspec);
        }

        [Fact]
        public void ReturnsLongAreaAgnosticRunspecErrorWhenTextExceedsOneCharacterForDescriptorWithoutAreas()
        {
            // Arrange
            var text = "a+";

            // Act
            var result = Runspec.Parse(text, [OperationDescriptor.Add]);

            // Assert
            Assert.Equal(Messages.LongAreaAgnosticRunspecInput('a', text), result.Message);
            Assert.Null(result.Runspec);
        }

        [Fact]
        public void ReturnsLongAreaAwareRunspecErrorWhenTextExceedsTwoCharactersForDescriptorWithAreas()
        {
            // Arrange
            var text = "d++";

            // Act
            var result = Runspec.Parse(text, [OperationDescriptor.Diff]);

            // Assert
            Assert.Equal(Messages.LongAreaAwareRunspecInput('d', text), result.Message);
            Assert.Null(result.Runspec);
        }

        [Fact]
        public void ReturnsUnknownAreaErrorWhenAreaLetterIsInvalid()
        {
            // Arrange
            var text = "f_";

            // Act
            var result = Runspec.Parse(text, [OperationDescriptor.FragmentalRestore]);

            // Assert
            Assert.Equal(Messages.UnknownAreaInput('_'), result.Message);
            Assert.Null(result.Runspec);
        }
    }
}
