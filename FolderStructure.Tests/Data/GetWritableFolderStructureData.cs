using System.Collections.Generic;

namespace FolderStructure.Tests.Data
{
    class GetWritableFolderStructureData
    {
        static class Case1
        {
            public static List<string> WritableFolders => new List<string>()
            {
                "/var/usr/wyns",
                "/var/usr/th",
                "/var/doc/1",
                "/var/mnt/",
                "/x/y/z",
                "/un/acc"
            };

            public static List<string> ReadableFolders => new List<string>()
            {
                "/var",
                "/var/usr",
                "/var/doc",
                "/var/doc/2",
                "/var/prn",
                "/var/mnt/x",
                "/x"
            };
        }

        static public IEnumerable<object[]> ShouldNotReturnReadOnlyLeaves()
        {
            var case1ResultShouldNotContain = new List<string>()
            {
                "/var/prn/wyns",
                "/var/mnt/x"
            };
            yield return new[] { Case1.WritableFolders, Case1.ReadableFolders, case1ResultShouldNotContain };
        }

        static public IEnumerable<object[]> ShouldNotIncludeUnaccessableWritableLeaves()
        {
            var case1ResultShouldNotContain = new List<string>()
            {
                "/x/y/z",
                "/un/acc"
            };
            yield return new[] { Case1.WritableFolders, Case1.ReadableFolders, case1ResultShouldNotContain };
        }

        static public IEnumerable<object[]> ShouldReturnReadOnlyNodesWithWritableLeaves()
        {
            var case1ResultShouldContain = new List<string>()
            {
                "/var",
                "/var/usr",
                "/var/doc",
            };
            yield return new[] { Case1.WritableFolders, Case1.ReadableFolders, case1ResultShouldContain };
        }

        static public IEnumerable<object[]> ShouldReturnAccessableWritableLeaves()
        {
            var case1ResultShouldContain = new List<string>()
            {
                "/var/usr/wyns",
                "/var/usr/th",
                "/var/doc/1"
            };
            yield return new[] { Case1.WritableFolders, Case1.ReadableFolders, case1ResultShouldContain };
        }
    }
}
