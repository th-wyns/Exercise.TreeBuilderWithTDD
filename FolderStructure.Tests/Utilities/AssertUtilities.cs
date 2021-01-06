using System;
using System.Collections.Generic;
using Xunit;

namespace FolderStructure.Tests.Utilities
{
    static class AssertUtilities
    {
        public static void CollectionContainsSubcollection<T>(IEnumerable<T> collection, IEnumerable<T> subcollection)
        {
            foreach (var item in subcollection)
            {
                Assert.Contains(item, collection);
            }
        }

        public static void CollectionDoesNotContainSubcollection<T>(IEnumerable<T> collection, IEnumerable<T> subcollection)
        {
            foreach (var item in subcollection)
            {
                Assert.DoesNotContain(item, collection);
            }
        }
    }
}
