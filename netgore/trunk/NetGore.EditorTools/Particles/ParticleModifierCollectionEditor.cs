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
            const string prefixToRemove = "Particle";

            var typeName = value.GetType().Name;
            if (typeName.StartsWith(prefixToRemove, StringComparison.OrdinalIgnoreCase))
                typeName = typeName.Substring(prefixToRemove.Length);

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
            return ParticleModifierBase.ModifierTypes.ToArray();
        }
    }
}
