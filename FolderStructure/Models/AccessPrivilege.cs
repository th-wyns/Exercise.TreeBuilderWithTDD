using System;

namespace FolderStructure.Models
{
    [Flags]
    public enum AccessPrivilege { None = 0, Read = 1, Write = 2 }
}
