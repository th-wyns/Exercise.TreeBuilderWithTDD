using FolderStructure.Tests.Data;
using FolderStructure.Tests.Utilities;
using System.Collections.Generic;
using Xunit;

namespace FolderStructure.Tests
{
    public class GetWritableFolderStructure
    {
        [Theory]
        [MemberData(nameof(GetWritableFolderStructureData.ShouldNotReturnReadOnlyLeaves), MemberType = typeof(GetWritableFolderStructureData))]
        public void ShouldNotReturnReadOnlyLeaves(List<string> writableFolders, List<string> readableFolders, List<string> resultShouldNotContain)
        {
            var result = FolderStructureUtilities.GetWritableFolderStructure(readableFolders, writableFolders);
            var resultFolders = result.GetLeafFolders();
            AssertUtilities.CollectionDoesNotContainSubcollection(resultFolders, resultShouldNotContain);
        }

        [Theory]
        [MemberData(nameof(GetWritableFolderStructureData.ShouldNotIncludeUnaccessableWritableLeaves), MemberType = typeof(GetWritableFolderStructureData))]
        public void ShouldNotIncludeUnaccessableWritableLeaves(List<string> writableFolders, List<string> readableFolders, List<string> resultShouldNotContain)
        {
            var result = FolderStructureUtilities.GetWritableFolderStructure(readableFolders, writableFolders);
            var resultFolders = result.GetLeafFolders();
            AssertUtilities.CollectionDoesNotContainSubcollection(resultFolders, resultShouldNotContain);
        }

        [Theory]
        [MemberData(nameof(GetWritableFolderStructureData.ShouldReturnReadOnlyNodesWithWritableLeaves), MemberType = typeof(GetWritableFolderStructureData))]
        public void ShouldReturnReadOnlyNodesWithWritableLeaves(List<string> writableFolders, List<string> readableFolders, List<string> resultShouldContain)
        {
            var result = FolderStructureUtilities.GetWritableFolderStructure(readableFolders, writableFolders);
            var resultFolders = result.GetFolders();
            AssertUtilities.CollectionContainsSubcollection(resultFolders, resultShouldContain);
        }

        [Theory]
        [MemberData(nameof(GetWritableFolderStructureData.ShouldReturnAccessableWritableLeaves), MemberType = typeof(GetWritableFolderStructureData))]
        public void ShouldReturnAccessableWritableLeaves(List<string> writableFolders, List<string> readableFolders, List<string> resultShouldContain)
        {
            var result = FolderStructureUtilities.GetWritableFolderStructure(readableFolders, writableFolders);
            var resultFolders = result.GetLeafFolders();
            AssertUtilities.CollectionContainsSubcollection(resultFolders, resultShouldContain);
        }
    }
}
