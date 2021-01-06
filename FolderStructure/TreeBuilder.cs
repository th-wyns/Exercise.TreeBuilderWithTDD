using FolderStructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FolderStructure
{
    class TreeBuilder
    {
        static readonly char[] pathSeparators = new char[] { '/', '\\' };

        readonly Dictionary<AccessPrivilege, List<string>> foldersByTypes = new Dictionary<AccessPrivilege, List<string>>();

        TreeItem root;

        public TreeBuilder WithReadableFolders(IEnumerable<string> readableFolders)
        {
            WithFolders(readableFolders, AccessPrivilege.Read);
            return this;
        }

        public TreeBuilder WithWritableFolders(IEnumerable<string> writableFolders)
        {
            WithFolders(writableFolders, AccessPrivilege.Write);
            return this;
        }

        public TreeBuilder WithFolders(IEnumerable<string> folders, AccessPrivilege accessPrivilege)
        {
            if (!foldersByTypes.ContainsKey(accessPrivilege))
            {
                foldersByTypes.Add(accessPrivilege, new List<string>(folders));
            }
            else
            {
                foldersByTypes[accessPrivilege].AddRange(folders);
            }
            return this;
        }

        public TreeItem Build()
        {
            ResetRoot();
            BuildTree();
            return root;
        }

        void ResetRoot()
        {
            root = TreeItem.CreateRoot();
        }

        void BuildTree()
        {
            foreach (var foldersByType in foldersByTypes)
            {
                AddChildren(foldersByType.Value, foldersByType.Key);
            }
        }

        void AddChildren(IReadOnlyCollection<string> folders, AccessPrivilege accessPrivilege)
        {
            foreach (var folder in folders)
            {
                AddChild(folder, accessPrivilege);
            }
        }

        void AddChild(string relativePath, AccessPrivilege accessPrivilege)
        {
            var pathSegments = relativePath.Split(pathSeparators, StringSplitOptions.RemoveEmptyEntries);
            var lastIndex = pathSegments.Length - 1;
            var node = root;

            for (int i = 0; i < lastIndex; i++)
            {
                node = node.AddChild(pathSegments[i], AccessPrivilege.None);
            }

            node.AddChild(pathSegments[lastIndex], accessPrivilege);
        }
    }
}
