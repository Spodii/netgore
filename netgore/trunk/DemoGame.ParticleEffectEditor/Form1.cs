using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.Editor.WinForms;
using NetGore.Graphics.ParticleEngine;

namespace DemoGame.ParticleEffectEditor
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Delegate for handling events for properties changing on the <see cref="Form1"/>.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="sender">The sender.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public delegate void PropertyChangedEventHandler<T>(Form1 sender, T oldValue, T newValue);

        ParticleEffect _particleEffect;

        /// <summary>
        /// Gets or sets the <see cref="IParticleEffect"/> to edit.
        /// </summary>
        public ParticleEffect ParticleEffect
        {
            get
            {
                return _particleEffect;
            }
            set
            {
                if (_particleEffect == value)
                    return;

                var oldValue = _particleEffect;
                _particleEffect = value;

                // Raise events
                OnParticleEffectChanged(oldValue, value);

                if (ParticleEffectChanged != null)
                    ParticleEffectChanged(this, oldValue, value);
            }
        }

        protected virtual void OnParticleEffectChanged(ParticleEffect oldValue, ParticleEffect newValue)
        {
            // Update the PropertyGrid
            pgEffect.SelectedObject = ParticleEffect;

            // Update the emitters list
            lstEmitters.Items.Clear();

            if (newValue != null)
            {
                lstEmitters.AddItems(newValue.Emitters.OfType<IParticleEmitter>());
            }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="Form1.ParticleEffect"/> property has changed.
        /// </summary>
        public PropertyChangedEventHandler<IParticleEffect> ParticleEffectChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        private void lstEmitters_TypedSelectedItemChanged(TypedListBox<IParticleEmitter> sender, IParticleEmitter value)
        {
            pgEmitter.SelectedObject = value;
        }

    }
}
