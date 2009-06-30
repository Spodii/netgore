using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Implementation of a PropertySyncBase that handles synchronizing an ItemType.
    /// </summary>
    [PropertySyncHandler(typeof(ItemType))]
    public sealed class PropertySyncItemType : PropertySyncBase<ItemType>
    {
        /// <summary>
        /// PropertySyncCollisionType constructor.
        /// </summary>
        /// <param name="bindObject">Object to bind to.</param>
        /// <param name="p">PropertyInfo for the property to bind to.</param>
        public PropertySyncItemType(object bindObject, PropertyInfo p) : base(bindObject, p)
        {
        }

        /// <summary>
        /// When overridden in the derived class, reads a value with the specified name from an IValueReader.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="reader">IValueReader to read from.</param>
        /// <returns>Value read from the IValueReader.</returns>
        protected override ItemType Read(string name, IValueReader reader)
        {
            return reader.ReadItemType(name);
        }

        /// <summary>
        /// When overridden in the derived class, writes a value to an IValueWriter with the specified name.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="value">Value to write.</param>
        protected override void Write(string name, IValueWriter writer, ItemType value)
        {
            writer.Write(name, value);
        }
    }
}