using Gint.Core.Changes;
using Gint.Core.Operations;
using Gint.Testbed.Creators;
using Moq.AutoMock;

namespace Gint.Core.Facts.Operations;

public class OperationAccessorFacts
{
    private readonly AutoMocker _mocker = new();

    private OperationAccessor CreateSystemUnderTest()
        => _mocker.CreateInstance<OperationAccessor>();

    public class TheFilterMethod : OperationAccessorFacts
    {
        public static readonly TheoryData<string, string[]> DescriptorsByScope
            = new()
            {
                {
                    OperationScope.All.Name,
                    new string[]
                    {
                        OperationDescriptor.Add.Name,
                        OperationDescriptor.Clean.Name,
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
                        OperationDescriptor.Clean.Name,
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
                        OperationDescriptor.Clean.Name,
                        OperationDescriptor.Diff.Name,
                        OperationDescriptor.FragmentalRestore.Name,
                        OperationDescriptor.IntendToAdd.Name,
                        OperationDescriptor.Restore.Name,
                    }
                },
                {
                    ChangeArea.Working.Name,
                    new string[]
                    {
                        OperationDescriptor.Add.Name,
                        OperationDescriptor.Clean.Name,
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
            var changes = ChangesCreator.CreateGroup(
                ChangesCreator.CreateFile(indicator: ChangeIndicator.Untracked),
                ChangesCreator.CreateFile());

            var expected = expectedNames.Select(name => OperationDescriptor.FromName(name))
                .ToArray();

            var sut = CreateSystemUnderTest();

            // Act
            var actual = sut.Filter(new(scope, changes));

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
            var changes = ChangesCreator.CreateGroup(
                ChangesCreator.CreateFile(indicator: ChangeIndicator.Unknown, path: "foo.txt"),
                ChangesCreator.CreateFile(indicator: ChangeIndicator.Unknown, path: "bar.txt"));

            var expected = expectedNames.Select(name => OperationDescriptor.FromName(name))
                .ToArray();

            var sut = CreateSystemUnderTest();

            // Act
            var actual = sut.Filter(new(scope, changes));

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

            var changes = ChangesCreator.CreateGroup(
                ChangesCreator.CreateFile(
                    areaIndicators:
                    [
                        new(area, ChangeIndicator.Modified),
                        new(area, ChangeIndicator.Unmodified),
                    ]),
                ChangesCreator.CreateFile(indicator: ChangeIndicator.Untracked));

            var expected = expectedNames.Select(name => OperationDescriptor.FromName(name))
                .ToArray();

            var sut = CreateSystemUnderTest();

            // Act
            var actual = sut.Filter(new(OperationScope.All, changes));

            // Assert
            foreach (var descriptor in expected)
            {
                Assert.Single(actual, descriptor);
            }
        }

        [Fact]
        public void ExcludesUntrackedOnlyDescriptorsWhenUntrackedIndicatorsAreNotPresent()
        {
            // Arrange
            var changes = ChangesCreator.CreateGroup(ChangesCreator.CreateFile());
            var sut = CreateSystemUnderTest();

            // Act
            var descriptors = sut.Filter(new(OperationScope.All, changes));

            // Assert
            Assert.DoesNotContain(descriptors, descriptor => descriptor == OperationDescriptor.Clean);
            Assert.DoesNotContain(descriptors, descriptor => descriptor == OperationDescriptor.IntendToAdd);
        }

        [Fact]
        public void ExcludesBreakDescriptorWhenSingleFileHasChanges()
        {
            // Arrange
            var changes = ChangesCreator.CreateGroup(ChangesCreator.CreateFile());
            var sut = CreateSystemUnderTest();

            // Act
            var descriptors = sut.Filter(new(OperationScope.All, changes));

            // Assert
            Assert.DoesNotContain(descriptors, descriptor => descriptor == OperationDescriptor.Break);
        }

        [Theory]
        [MemberData(nameof(DescriptorsByScope))]
        public void SortsDescriptors(string scopeName, string[] expectedNames)
        {
            // Arrange
            var scope = OperationScope.FromName(scopeName);
            var changes = ChangesCreator.CreateGroup(
                ChangesCreator.CreateFile(indicator: ChangeIndicator.Untracked),
                ChangesCreator.CreateFile());

            var expected = expectedNames.Select(name => OperationDescriptor.FromName(name))
                .ToArray();

            var sut = CreateSystemUnderTest();

            // Act
            var actual = sut.Filter(new(scope, changes));

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
