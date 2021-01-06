using FolderStructure.Models;
using System.Collections.Generic;

namespace FolderStructure
{
    public static class FolderStructureUtilities
    {
        public static TreeItem GetWritableFolderStructure(List<string> readableFolders, List<string> writableFolders)
        {
            var writableFolderStructure = new TreeBuilder()
                .WithReadableFolders(readableFolders)
                .WithWritableFolders(writableFolders)
                .Build()
                .GetWritableFolderStructure();

            return writableFolderStructure;
        }
    }
}
