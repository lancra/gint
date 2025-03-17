using Gint.Core.Changes;
using Gint.Core.Operations;

namespace Gint.Core.Facts.Operations;

public class OperationDescriptorFacts
{
    public class TheFilterMethod : OperationDescriptorFacts
    {
        public static readonly TheoryData<string, string[]> DescriptorsByScope
            = new()
            {
                {
                    OperationScope.All.Name,
                    new string[]
                    {
                        OperationDescriptor.Add.Name,
                        OperationDescriptor.Diff.Name,
                        OperationDescriptor.FragmentalRestore.Name,
                        OperationDescriptor.IntendToAdd.Name,
                        OperationDescriptor.Patch.Name,
                        OperationDescriptor.Restore.Name,
                        OperationDescriptor.Status.Name,
                        OperationDescriptor.Break.Name,
                        OperationDescriptor.Quit.Name,
                        OperationDescriptor.Help.Name,
                    }
                },
                {
                    OperationScope.File.Name,
                    new string[]
                    {
                        OperationDescriptor.Add.Name,
                        OperationDescriptor.Diff.Name,
                        OperationDescriptor.FragmentalRestore.Name,
                        OperationDescriptor.IntendToAdd.Name,
                        OperationDescriptor.Patch.Name,
                        OperationDescriptor.Restore.Name,
                        OperationDescriptor.Status.Name,
                        OperationDescriptor.Ignore.Name,
                        OperationDescriptor.Quit.Name,
                        OperationDescriptor.Help.Name,
                    }
                },
            };

        public static readonly TheoryData<string, string[]> DescriptorsWithoutAreas
            = new()
            {
                {
                    OperationScope.All.Name,
                    new string[]
                    {
                        OperationDescriptor.Break.Name,
                        OperationDescriptor.Help.Name,
                        OperationDescriptor.Quit.Name,
                        OperationDescriptor.Status.Name,
                    }
                },
                {
                    OperationScope.File.Name,
                    new string[]
                    {
                        OperationDescriptor.Help.Name,
                        OperationDescriptor.Ignore.Name,
                        OperationDescriptor.Quit.Name,
                        OperationDescriptor.Status.Name,
                    }
                },
            };

        public static readonly TheoryData<string, string[]> DescriptorsByArea
            = new()
            {
                {
                    ChangeArea.Staging.Name,
                    new string[]
                    {
                        OperationDescriptor.Diff.Name,
                        OperationDescriptor.FragmentalRestore.Name,
                        OperationDescriptor.Restore.Name,
                    }
                },
                {
                    ChangeArea.Working.Name,
                    new string[]
                    {
                        OperationDescriptor.Add.Name,
                        OperationDescriptor.Diff.Name,
                        OperationDescriptor.FragmentalRestore.Name,
                        OperationDescriptor.IntendToAdd.Name,
                        OperationDescriptor.Patch.Name,
                        OperationDescriptor.Restore.Name,
                    }
                },
            };

        [Theory]
        [MemberData(nameof(DescriptorsByScope))]
        public void ReturnsDescriptorsWithMatchingScope(string scopeName, string[] expectedNames)
        {
            // Arrange
            var scope = OperationScope.FromName(scopeName);
            var changeGroup = new ChangeGroup(
                [
                    new(
                        [
                            new(ChangeArea.Staging, ChangeIndicator.Untracked),
                            new(ChangeArea.Working, ChangeIndicator.Untracked),
                        ],
                        "foo.txt"),
                ]);

            var expected = expectedNames.Select(name => OperationDescriptor.FromName(name))
                .ToArray();

            // Act
            var actual = OperationDescriptor.Filter(scope, changeGroup);

            // Assert
            Assert.Equal(expected.Length, actual.Count);
            foreach (var descriptor in expected)
            {
                Assert.Single(actual, descriptor);
            }
        }

        [Theory]
        [MemberData(nameof(DescriptorsWithoutAreas))]
        public void ReturnsDescriptorsWithoutAreas(string scopeName, string[] expectedNames)
        {
            // Arrange
            var scope = OperationScope.FromName(scopeName);
            var changeGroup = new ChangeGroup([]);

            var expected = expectedNames.Select(name => OperationDescriptor.FromName(name))
                .ToArray();

            // Act
            var actual = OperationDescriptor.Filter(scope, changeGroup);

            // Assert
            Assert.Equal(expected.Length, actual.Count);
            foreach (var descriptor in expected)
            {
                Assert.Single(actual, descriptor);
            }
        }

        [Theory]
        [MemberData(nameof(DescriptorsByArea))]
        public void ReturnsDescriptorsWhenAreaHasActionableChanges(string areaName, string[] expectedNames)
        {
            // Arrange
            var area = ChangeArea.FromName(areaName);
            var otherArea = ChangeArea.List.First(a => a != area);
            var changeGroup = new ChangeGroup(
                [
                    new(
                        [
                            new(area, ChangeIndicator.Modified),
                            new(otherArea, ChangeIndicator.Unmodified),
                        ],
                        "foo.txt"),
                    new(
                        [
                            new(area, ChangeIndicator.Untracked),
                            new(otherArea, ChangeIndicator.Untracked),
                        ],
                        "bar.txt"),
                ]);

            var expected = expectedNames.Select(name => OperationDescriptor.FromName(name))
                .ToArray();

            // Act
            var actual = OperationDescriptor.Filter(OperationScope.All, changeGroup);

            // Assert
            foreach (var descriptor in expected)
            {
                Assert.Single(actual, descriptor);
            }
        }

        [Fact]
        public void ExcludesIntendToAddDescriptorWhenNoUntrackedIndicatorsArePresent()
        {
            // Arrange
            var changeGroup = new ChangeGroup(
                [
                    new(
                        [
                            new(ChangeArea.Staging, ChangeIndicator.Unmodified),
                            new(ChangeArea.Working, ChangeIndicator.Modified),
                        ],
                        "foo.txt"),
                ]);

            // Act
            var descriptors = OperationDescriptor.Filter(OperationScope.All, changeGroup);

            // Assert
            Assert.DoesNotContain(descriptors, descriptor => descriptor == OperationDescriptor.IntendToAdd);
        }

        [Theory]
        [MemberData(nameof(DescriptorsByScope))]
        public void SortsDescriptors(string scopeName, string[] expectedNames)
        {
            // Arrange
            var scope = OperationScope.FromName(scopeName);
            var changeGroup = new ChangeGroup(
                [
                    new(
                        [
                            new(ChangeArea.Staging, ChangeIndicator.Untracked),
                            new(ChangeArea.Working, ChangeIndicator.Untracked),
                        ],
                        "foo.txt"),
                ]);

            var expected = expectedNames.Select(name => OperationDescriptor.FromName(name))
                .ToArray();

            // Act
            var actual = OperationDescriptor.Filter(scope, changeGroup);

            // Assert
            Assert.Equal(expected.Length, actual.Count);

            var actualList = actual.ToList();
            for (var i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], actualList[i]);
            }
        }
    }
}
