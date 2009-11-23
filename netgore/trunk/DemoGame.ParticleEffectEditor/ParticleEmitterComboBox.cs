using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NetGore;
using NetGore.Collections;
using NetGore.EditorTools;
using NetGore.Graphics.ParticleEngine;

namespace DemoGame.ParticleEffectEditor
{
    public delegate void ParticleEmitterComboBoxHandler(ParticleEmitterComboBox sender, ParticleEmitter emitter);

    public class ParticleEmitterComboBox : TypedComboBox<Type>
    {
        static readonly IEnumerable<Type> _emitterTypes =
            TypeHelper.FindTypesThatInherit(typeof(ParticleEmitter), Type.EmptyTypes, false).Concat(new Type[]
            { typeof(ParticleEmitter) }).Distinct().ToCompact();

        public event ParticleEmitterComboBoxHandler SelectedEmitterChanged;
     
        public ParticleEmitterComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
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

        protected override IEnumerable<Type> GetInitialItems()
        {
            return _emitterTypes;
        }

        protected override string ItemToString(Type item)
        {
            return item.Name;
        }
    }
}