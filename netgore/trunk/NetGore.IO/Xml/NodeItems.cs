using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// A set of node items, and the name of the node the items belong to.
    /// </summary>
    public struct NodeItems
    {
        /// <summary>
        /// Items that belong to the node.
        /// </summary>
        public readonly IEnumerable<NodeItem> Items;

        /// <summary>
        /// Name of the node that the items belong to.
        /// </summary>
        public readonly string Name;

        public NodeItems(string node, IEnumerable<NodeItem> items)
        {
            Name = node;
            Items = new List<NodeItem>(items);
        }

        /// <summary>
        /// Gets the Items in the form of a Dictionary indexed by the item's name.
        /// </summary>
        /// <returns>The Items in the form of a Dictionary indexed by the item's name.</returns>
        public IDictionary<string, string> ToDictionary()
        {
            var ret = new Dictionary<string, string>(Items.Count(), StringComparer.OrdinalIgnoreCase);
            foreach (NodeItem item in Items)
            {
                ret.Add(item.Name, item.Value);
            }
            return ret;
        }
    }
}