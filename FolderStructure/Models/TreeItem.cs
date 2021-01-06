using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FolderStructure.Models
{
    public class TreeItem
    {
        static readonly Func<TreeItem, bool> IsWritableWithAccess = (node) => node.AccessPrivilege.HasFlag(AccessPrivilege.Write) && node.IsReachable;

        readonly TreeItemCollection childrenInternal = new TreeItemCollection();

        string fullPath = null;

        public string Name { get; private set; }
        public IReadOnlyList<TreeItem> Children => childrenInternal;
        public IEnumerable<TreeItem> Leaves => GetLeaves();
        public string FullPath => fullPath ??= GetFullPath();

        TreeItem Parent { get; }
        AccessPrivilege AccessPrivilege { get; set; }
        bool IsReachable => GetIsReachable();
        IEnumerable<TreeItem> AncestorsOrdered => GetAncestorsOrdered();
        int Level => GetLevel();

        public TreeItem(TreeItem parent, string name, AccessPrivilege accessPrivilege)
        {
            Parent = parent;
            Name = name;
            AccessPrivilege = accessPrivilege;
        }

        public override string ToString()
        {
            var output = new StringBuilder();
            foreach (var node in TraverseDepthFirst())
            {
                var indentation = new string(' ', node.Level * 2);
                output.AppendLine($"{indentation}{node.Name}");
            }
            return output.ToString();
        }

        internal static TreeItem CreateRoot() => new TreeItem(null, "/", AccessPrivilege.Write);

        internal TreeItem AddChild(string name, AccessPrivilege accessPrivilege)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"Value required", nameof(name));
            }

            if (!childrenInternal.TryGetValue(name, out var child))
            {
                child = new TreeItem(this, name, accessPrivilege);
                childrenInternal.Add(child);
            }
            else
            {
                child.AccessPrivilege |= accessPrivilege;
            }
            return child;
        }

        internal TreeItem GetWritableFolderStructure()
        {
            var writableLeaves = Leaves.Where(IsWritableWithAccess);
            var writableRoot = CreateRoot();

            foreach (var writableLeaf in writableLeaves)
            {
                var writeableNode = writableRoot;
                var ancestorsWithoutRoot = writableLeaf.AncestorsOrdered.Skip(1);

                foreach (var ancestor in ancestorsWithoutRoot)
                {
                    var child = writeableNode.AddChild(ancestor.Name, ancestor.AccessPrivilege);
                    writeableNode = child;
                }

                writeableNode.AddChild(writableLeaf.Name, writableLeaf.AccessPrivilege);
            }

            return writableRoot;
        }

        IEnumerable<TreeItem> GetAncestors()
        {
            var node = this;
            while ((node = node.Parent) != null)
            {
                yield return node;
            }
        }

        IEnumerable<TreeItem> GetAncestorsOrdered()
        {
            var rerverseAncestors = new Stack<TreeItem>(GetAncestors());
            return rerverseAncestors;
        }

        bool GetIsReachable()
        {
            return GetAncestors().All(node => node.AccessPrivilege > AccessPrivilege.None);
        }

        IEnumerable<TreeItem> GetLeaves()
        {
            foreach (var node in TraverseDepthFirst())
            {
                if (node.childrenInternal.Count == 0)
                {
                    yield return node;
                }
            }
        }

        string GetFullPath()
        {
            var ancestorsWithoutRoot = GetAncestorsOrdered().Skip(1);
            var pathBuilder = new StringBuilder();

            foreach (var ancestor in ancestorsWithoutRoot)
            {
                pathBuilder.Append("/");
                pathBuilder.Append(ancestor.Name);
            }

            var notRoot = Parent != null;
            if (notRoot)
            {
                pathBuilder.Append("/");
            }
            pathBuilder.Append(Name);
            return pathBuilder.ToString();
        }

        int GetLevel()
        {
            return GetAncestors().Count();
        }

        IEnumerable<TreeItem> TraverseDepthFirst()
        {
            Stack<TreeItem> stack = new Stack<TreeItem>();
            TreeItem node = this;

            while (node != null)
            {
                yield return node;

                foreach (var child in node.childrenInternal)
                {
                    stack.Push(child);
                }

                stack.TryPop(out node);
            }
        }
    }
}
