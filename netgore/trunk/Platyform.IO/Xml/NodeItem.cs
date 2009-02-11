using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Platyform.IO
{
    /// <summary>
    /// An individual node item, containing the name of the item and its value.
    /// </summary>
    public struct NodeItem
    {
        /// <summary>
        /// Name of the node item.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Value of the node item.
        /// </summary>
        public readonly string Value;

        public NodeItem(string name, object value)
        {
            Name = name;
            Value = value.ToString();
        }
    }
}