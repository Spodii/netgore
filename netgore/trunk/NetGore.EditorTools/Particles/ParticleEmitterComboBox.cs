using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NetGore.Collections;
using NetGore.Graphics.ParticleEngine;

namespace NetGore.EditorTools
{
    public delegate void ParticleEmitterComboBoxHandler(ParticleEmitterComboBox sender, ParticleEmitter emitter);

    public class ParticleEmitterComboBox : TypedComboBox<Type>
    {
        public event ParticleEmitterComboBoxHandler SelectedEmitterChanged;

        public ParticleEmitterComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Gets the items to initially populate the <see cref="ComboBox"/> with.
        /// </summary>
        /// <returns>
        /// The items to initially populate the <see cref="ComboBox"/> with.
        /// </returns>
        protected override IEnumerable<Type> GetInitialItems()
        {
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