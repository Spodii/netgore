using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using NetGore.Graphics.ParticleEngine;

namespace NetGore.EditorTools
{
    public class ParticleModifierCollectionEditor : CollectionEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleModifierCollectionEditor"/> class.
        /// </summary>
        public ParticleModifierCollectionEditor()
            : base(typeof(ParticleModifierCollection)) { }

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
        /// Indicates whether multiple collection items can be selected at once.
        /// </summary>
        /// <returns>
        /// true if it multiple collection members can be selected at the same time; otherwise, false. By default, this returns true.
        /// </returns>
        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        /// <summary>
        /// Gets the data types that this collection editor can contain.
        /// </summary>
        /// <returns>
        /// An array of data types that this collection can contain.
        /// </returns>
        protected override Type[] CreateNewItemTypes()
        {
            return ParticleModifier.ModifierTypes.ToArray();
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
            return typeof(ColorModifier);
        }

        /// <summary>
        /// Creates a new instance of the specified collection item type.
        /// </summary>
        /// <param name="itemType">The type of item to create.</param>
        /// <returns>A new instance of the specified object.</returns>
        protected override object CreateInstance(Type itemType)
        {
            return ParticleModifier.CreateModifier(itemType);
        }
    }
}
