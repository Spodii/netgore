using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics.ParticleEngine;

namespace NetGore.EditorTools
{
    public delegate void ParticleEmitterComboBoxHandler(ParticleEmitterComboBox sender, ParticleEmitter emitter);

    public class ParticleEmitterComboBox : TypedComboBox<Type>
    {
        static readonly IEnumerable<Type> _emitterTypes =
            TypeHelper.FindTypesThatInherit(typeof(ParticleEmitter), Type.EmptyTypes, false).Concat(new Type[]
            { typeof(ParticleEmitter) }).Distinct().OrderBy(x => x.Name).ToCompact();

        public event ParticleEmitterComboBoxHandler SelectedEmitterChanged;

        public ParticleEmitterComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        protected override IEnumerable<Type> GetInitialItems()
        {
            return _emitterTypes;
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

            ParticleEmitter emitter = (ParticleEmitter)Activator.CreateInstance(item, true);

            if (emitter != null)
                SelectedEmitterChanged(this, emitter);
        }
    }
}