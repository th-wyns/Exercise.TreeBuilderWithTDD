using System.Collections.ObjectModel;

namespace FolderStructure.Models
{
    class TreeItemCollection : KeyedCollection<string, TreeItem>
    {
        protected override string GetKeyForItem(TreeItem item)
        {
            return item.Name;
        }
    }
}
