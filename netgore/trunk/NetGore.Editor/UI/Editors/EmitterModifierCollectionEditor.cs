using System;
using System.ComponentModel.Design;
using System.Linq;
using NetGore.Collections;
using NetGore.Graphics.ParticleEngine;

namespace NetGore.Editor.UI
{
    /// <summary>
    /// Provides a design-timer user interface for the <see cref="EmitterModifierCollection"/>.
    /// </summary>
    public class EmitterModifierCollectionEditor : CollectionEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmitterModifierCollectionEditor"/> class.
        /// </summary>
        public EmitterModifierCollectionEditor() : base(typeof(EmitterModifierCollection))
        {
        }

        /// <summary>
        /// Indicates whether multiple collection items can be selected at once.
        /// </summary>
        /// <returns>
        /// true if it multiple collection members can be selected at the same time; otherwise, false.
        /// By default, this returns true.
        /// </returns>
        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        /// <summary>
        /// Gets the data type that this collection contains.
        /// </summary>
        /// <returns>
        /// The data type of the items in the collection, or an <see cref="T:System.Object"/> if no
        /// Item property can be located on the collection.
        /// </returns>
        protected override Type CreateCollectionItemType()
        {
            return typeof(EmitterBurstEmitModifier);
        }

        /// <summary>
        /// Creates a new instance of the specified collection item type.
        /// </summary>
        /// <param name="itemType">The type of item to create.</param>
        /// <returns>A new instance of the specified object.</returns>
        protected override object CreateInstance(Type itemType)
        {
            return TypeFactory.GetTypeInstance(itemType);
        }

        /// <summary>
        /// Gets the data types that this collection editor can contain.
        /// </summary>
        /// <returns>
        /// An array of data types that this collection can contain.
        /// </returns>
        protected override Type[] CreateNewItemTypes()
        {
            return EmitterModifierFactory.Instance.ToArray();
        }

        /// <summary>
        /// Retrieves the display text for the given list item.
        /// </summary>
        /// <param name="value">The list item for which to retrieve display text.</param>
        /// <returns>
        /// The display text for <paramref name="value"/>.
        /// </returns>
        protected override string GetDisplayText(object value)
        {
            var typeName = value.GetType().Name;
            return typeName;
        }

        /// <summary>
        /// Gets an array of objects containing the specified collection.
        /// </summary>
        /// <param name="editValue">The collection to edit.</param>
        /// <returns>
        /// An array containing the collection objects, or an empty object array if the specified collection
        /// does not inherit from <see cref="T:System.Collections.ICollection"/>.
        /// </returns>
        protected override object[] GetItems(object editValue)
        {
            return ((EmitterModifierCollection)editValue).Cast<object>().ToArray();
        }

        /// <summary>
        /// Sets the specified array as the items of the collection.
        /// </summary>
        /// <param name="editValue">The collection to edit.</param>
        /// <param name="value">An array of objects to set as the collection items.</param>
        /// <returns>
        /// The newly created collection object or, otherwise, the collection indicated by the
        /// <paramref name="editValue"/> parameter.
        /// </returns>
        protected override object SetItems(object editValue, object[] value)
        {
            var c = ((EmitterModifierCollection)editValue);
            c.Clear();
            c.AddRange(value.Cast<EmitterModifier>());
            return c;
        }
    }
}