using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains the IComparers that can be used to sort the item templates.
    /// </summary>
    public static class ItemTemplateComparer
    {
        static readonly SortByTypeAndValueComparer _sortByTypeAndValue = new SortByTypeAndValueComparer();

        /// <summary>
        /// Gets an IComparer that groups the item templates by type and sorts each group by value.
        /// </summary>
        public static IComparer<IItemTemplateTable> SortByTypeAndValue
        {
            get { return _sortByTypeAndValue; }
        }

        /// <summary>
        /// IComparer for sorting the item templates by Type then Value.
        /// </summary>
        sealed class SortByTypeAndValueComparer : IComparer<IItemTemplateTable>
        {
            #region IComparer<IItemTemplateTable> Members

            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <returns>
            /// Value 
            ///                     Condition 
            ///                     Less than zero
            ///                 <paramref name="x"/> is less than <paramref name="y"/>.
            ///                     Zero
            ///                 <paramref name="x"/> equals <paramref name="y"/>.
            ///                     Greater than zero
            ///                 <paramref name="x"/> is greater than <paramref name="y"/>.
            /// </returns>
            /// <param name="x">The first object to compare.
            ///                 </param><param name="y">The second object to compare.
            ///                 </param>
            public int Compare(IItemTemplateTable x, IItemTemplateTable y)
            {
                // Start by sorting by the item types
                if (x.Type != y.Type)
                    return x.Type.CompareTo(y.Type);

                // For items of the same type, order by value (ascending)
                return x.Value.CompareTo(y.Value);
            }

            #endregion
        }
    }
}