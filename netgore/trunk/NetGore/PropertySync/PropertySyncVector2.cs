using System.Reflection;
using Microsoft.Xna.Framework;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Implementation of a PropertySyncBase that handles synchronizing a Vector2.
    /// </summary>
    [PropertySyncHandler(typeof(Vector2))]
    public sealed class PropertySyncVector2 : PropertySyncBase<Vector2>
    {
        /// <summary>
        /// PropertySyncVector2 constructor.
        /// </summary>
        /// <param name="bindObject">Object to bind to.</param>
        /// <param name="p">PropertyInfo for the property to bind to.</param>
        public PropertySyncVector2(object bindObject, PropertyInfo p) : base(bindObject, p)
        {
        }

        /// <summary>
        /// When overridden in the derived class, reads a value with the specified name from an IValueReader.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="reader">IValueReader to read from.</param>
        /// <returns>Value read from the IValueReader.</returns>
        protected override Vector2 Read(string name, IValueReader reader)
        {
            return reader.ReadVector2(name);
        }

        /// <summary>
        /// When overridden in the derived class, writes a value to an IValueWriter with the specified name.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="value">Value to write.</param>
        protected override void Write(string name, IValueWriter writer, Vector2 value)
        {
            writer.Write(name, value);
        }
    }
}