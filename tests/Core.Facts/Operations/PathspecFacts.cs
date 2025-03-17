using Gint.Core.Operations;

namespace Gint.Core.Facts.Operations;

public class PathspecFacts
{
    public class TheImplicitStringOperator : PathspecFacts
    {
        [Fact]
        public void ReturnsPattern()
        {
            // Arrange
            var pattern = "foo.txt";
            var sut = new Pathspec(pattern);

            // Assert
            Assert.Equal(pattern, sut);
        }

        [Fact]
        public void ReturnsEmptyStringWhenPathspecIsNull()
        {
            // Arrange
            var pattern = string.Empty;
            var sut = default(Pathspec?);

            // Assert
            Assert.Equal(pattern, sut!);
        }
    }

    public class TheToStringMethod : PathspecFacts
    {
        [Fact]
        public void ReturnsPattern()
        {
            // Arrange
            var pattern = "foo.txt";
            var sut = new Pathspec(pattern);

            // Act
            var result = sut.ToString();

            // Assert
            Assert.Equal(pattern, result);
        }
    }
}
