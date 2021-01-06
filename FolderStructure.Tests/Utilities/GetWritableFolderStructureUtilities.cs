using FolderStructure.Models;
using System.Collections.Generic;
using System.Linq;

namespace FolderStructure.Tests.Utilities
{
    static class GetWritableFolderStructureUtilities
    {
        public static IEnumerable<string> GetLeafFolders(this TreeItem root)
        {
            var leafFullPaths = root.Leaves.Select(node => node.FullPath);
            return leafFullPaths;
        }

        public static IEnumerable<string> GetFolders(this TreeItem root)
        {
            TreeItem node = root;
            Stack<TreeItem> stack = new Stack<TreeItem>();

            while (node != null)
            {
                yield return node.FullPath;

                foreach (var child in node.Children)
                {
                    stack.Push(child);
                }

                stack.TryPop(out node);
            }
        }
    }
}
