using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// A set of node items, and the name of the node the items belong to.
    /// </summary>
    public struct NodeItems
    {
        readonly IEnumerable<NodeItem> _items;
        readonly string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeItems"/> struct.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="items">The items.</param>
        public NodeItems(string node, IEnumerable<NodeItem> items)
        {
            _name = node;
            _items = new List<NodeItem>(items);
        }

        /// <summary>
        /// Gets the items that belong to the node.
        /// </summary>
        public IEnumerable<NodeItem> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets the name of the node that the items belong to.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the Items in the form of a Dictionary indexed by the item's name.
        /// </summary>
        /// <returns>The Items in the form of a Dictionary indexed by the item's name.</returns>
        public IDictionary<string, string> ToDictionary()
        {
            var ret = new Dictionary<string, string>(_items.Count(), StringComparer.OrdinalIgnoreCase);
            foreach (NodeItem item in _items)
            {
                ret.Add(item.Name, item.Value);
            }
            return ret;
        }
    }
}