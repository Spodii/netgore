using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore;
using NetGore.Content;
using NetGore.Editor;
using NetGore.Editor.UI;
using NetGore.Editor.WinForms;
using NetGore.Graphics;
using NetGore.Graphics.ParticleEngine;
using NetGore.IO;

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
        public delegate void PropertyChangedEventHandler<in T>(Form1 sender, T oldValue, T newValue);

        IParticleEffect _particleEffect;

        /// <summary>
        /// Gets or sets the <see cref="IParticleEffect"/> to edit.
        /// </summary>
        public IParticleEffect ParticleEffect
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

                // Remove listeners from the old emitter
                if (oldValue != null)
                {
                    value.EmitterAdded -= ParticleEffect_EmitterAdded;
                    value.EmitterRemoved -= ParticleEffect_EmitterRemoved;
                }

                // Add listeners to the new emitter
                if (value != null)
                {
                    value.EmitterAdded += ParticleEffect_EmitterAdded;
                    value.EmitterRemoved += ParticleEffect_EmitterRemoved;
                }

                gameScreen.ParticleEffect = value;

                // Raise events
                OnParticleEffectChanged(oldValue, value);

                if (ParticleEffectChanged != null)
                    ParticleEffectChanged(this, oldValue, value);
            }
        }

        /// <summary>
        /// Handles when a <see cref="IParticleEmitter"/> is added to the <see cref="Form1.ParticleEffect"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="emitter">The emitter.</param>
        void ParticleEffect_EmitterAdded(IParticleEffect sender, IParticleEmitter emitter)
        {
            if (!lstEmitters.Items.Contains(emitter))
                lstEmitters.AddItemAndReselect(emitter);
        }

        /// <summary>
        /// Handles when a <see cref="IParticleEmitter"/> is removed from the <see cref="Form1.ParticleEffect"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="emitter">The emitter.</param>
        void ParticleEffect_EmitterRemoved(IParticleEffect sender, IParticleEmitter emitter)
        {
            lstEmitters.RemoveItemAndReselect(emitter);
        }

        /// <summary>
        /// Handles when the <see cref="Form1.ParticleEffect"/> property has changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnParticleEffectChanged(IParticleEffect oldValue, IParticleEffect newValue)
        {
            // Update the PropertyGrid
            pgEffect.SelectedObject = ParticleEffect;

            // Update the emitters list
            lstEmitters.Items.Clear();

            if (newValue != null)
            {
                lstEmitters.AddItems(newValue.Emitters);

                // Select the first emitter
                if (lstEmitters.Items.Count > 0)
                    lstEmitters.SelectedIndex = 0;
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

        IContentManager _content;

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // NOTE: !! Temp

            _content = ContentManager.Create();
            GrhInfo.Load(ContentPaths.Build, _content);

            CustomUITypeEditors.AddEditors();

            ParticleEffect = ParticleEffectManager.Instance.TryCreateEffect(ParticleEffectManager.Instance.ParticleEffectNames.FirstOrDefault());
        }

        private void lstEmitters_TypedSelectedItemChanged(TypedListBox<IParticleEmitter> sender, IParticleEmitter value)
        {
            pgEmitter.SelectedObject = value;
        }

        /// <summary>
        /// Handles the Click event of the btnDeleteEmitter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnDeleteEmitter_Click(object sender, EventArgs e)
        {
            // Ensure a valid ParticleEffect is selected
            var pe = ParticleEffect;
            if (pe == null)
                return;

            // Get the selected emitter
            var emitter = lstEmitters.TypedSelectedItem;
            if (emitter == null)
                return;

            // Confirm deletion
            const string confirmMsg = "Are you sure you with to delete the Particle Emitter `{0}`?";
            if (MessageBox.Show(string.Format(confirmMsg, emitter.Name), "Delete emitter?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // Delete
            emitter.Dispose();
        }

        /// <summary>
        /// Handles the Click event of the btnNewEmitter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnNewEmitter_Click(object sender, EventArgs e)
        {
            // Ensure a valid ParticleEffect is selected
            var pe = ParticleEffect;
            if (pe == null)
                return;

            // Add new emitter. Default to PointEmitter. Type can be changed by the user later.
            new PointEmitter(pe);
        }

    }
}
