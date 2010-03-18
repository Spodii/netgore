using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NetGore.Collections;
using NetGore.Graphics.ParticleEngine;

namespace NetGore.EditorTools
{
    public delegate void ParticleEmitterComboBoxHandler(ParticleEmitterComboBox sender, ParticleEmitter emitter);

    /// <summary>
    /// A <see cref="ComboBox"/> containing the <see cref="ParticleEmitter"/>s.
    /// </summary>
    public class ParticleEmitterComboBox : TypedComboBox<Type>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterComboBox"/> class.
        /// </summary>
        public ParticleEmitterComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Notifies listeners when the selected <see cref="ParticleEmitter"/> has changed.
        /// </summary>
        public event ParticleEmitterComboBoxHandler SelectedEmitterChanged;

        /// <summary>
        /// Gets the items to initially populate the <see cref="ComboBox"/> with.
        /// </summary>
        /// <returns>
        /// The items to initially populate the <see cref="ComboBox"/> with.
        /// </returns>
        protected override IEnumerable<Type> GetInitialItems()
        {
            if (DesignMode)
                return Enumerable.Empty<Type>();

            return ParticleEmitter.EmitterTypes;
        }

        /// <summary>
        /// Gets the string to display for an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The string to display.</returns>
        public override string ItemToString(Type item)
        {
            return item.Name;
        }

        /// <summary>
        /// Handles when the selected value changes.
        /// </summary>
        /// <param name="item">The new selected value.</param>
        protected override void OnTypedSelectedValueChanged(Type item)
        {
            base.OnTypedSelectedValueChanged(item);

            if (SelectedEmitterChanged == null)
                return;

            ParticleEmitter emitter = (ParticleEmitter)TypeFactory.GetTypeInstance(item);

            if (emitter != null)
                SelectedEmitterChanged(this, emitter);
        }
    }
}