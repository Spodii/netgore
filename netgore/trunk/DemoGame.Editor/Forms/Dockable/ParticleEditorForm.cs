using System;
using System.Linq;
using System.Windows.Forms;
using NetGore.Collections;
using NetGore.Editor;
using NetGore.Editor.Docking;
using NetGore.Editor.WinForms;
using NetGore.Graphics;
using NetGore.Graphics.ParticleEngine;

namespace DemoGame.Editor
{
    public partial class ParticleEditorForm : DockContent
    {
        /// <summary>
        /// Delegate for handling events for properties changing on the <see cref="ParticleEditorForm"/>.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="sender">The sender.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public delegate void PropertyChangedEventHandler<in T>(ParticleEditorForm sender, T oldValue, T newValue);

        /// <summary>
        /// Notifies listeners when the <see cref="ParticleEditorForm.ParticleEffect"/> property has changed.
        /// </summary>
        public PropertyChangedEventHandler<IParticleEffect> ParticleEffectChanged;

        GrhData _defaultEmitterSprite;
        IParticleEffect _particleEffect;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEditorForm"/> class.
        /// </summary>
        public ParticleEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the <see cref="IParticleEffect"/> to edit.
        /// </summary>
        public IParticleEffect ParticleEffect
        {
            get { return _particleEffect; }
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
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode)
                return;

            // Load the default emitter sprite
            _defaultEmitterSprite = GrhInfo.GetData("Particle", "sparkle alpha");
        }

        /// <summary>
        /// Handles when the <see cref="ParticleEditorForm.ParticleEffect"/> property has changed.
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
                lstEmitters.Items.AddRange(newValue.Emitters.Cast<object>().ToArray());

                // Select the first emitter
                if (lstEmitters.Items.Count > 0)
                    lstEmitters.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Handles when a <see cref="IParticleEmitter"/> is added to the <see cref="ParticleEditorForm.ParticleEffect"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="emitter">The emitter.</param>
        void ParticleEffect_EmitterAdded(IParticleEffect sender, IParticleEmitter emitter)
        {
            if (!lstEmitters.Items.Contains(emitter))
                lstEmitters.AddItemAndReselect(emitter);
        }

        /// <summary>
        /// Handles when a <see cref="IParticleEmitter"/> is removed from the <see cref="ParticleEditorForm.ParticleEffect"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="emitter">The emitter.</param>
        void ParticleEffect_EmitterRemoved(IParticleEffect sender, IParticleEmitter emitter)
        {
            lstEmitters.RemoveItemAndReselect(emitter);
        }

        /// <summary>
        /// Handles the Click event of the btnClone control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnClone_Click(object sender, EventArgs e)
        {
            var emitter = pgEmitter.SelectedObject as ParticleEmitter;
            if (emitter == null)
                return;

            var newEmitter = emitter.DeepCopy(emitter.Owner);

            if (!lstEmitters.Items.Contains(newEmitter))
                lstEmitters.Items.Add(newEmitter);

            lstEmitters.SelectedItem = newEmitter;
        }

        /// <summary>
        /// Handles the Click event of the btnDeleteEmitter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnDeleteEmitter_Click(object sender, EventArgs e)
        {
            // Ensure a valid ParticleEffect is selected
            var pe = ParticleEffect;
            if (pe == null)
                return;

            // Get the selected emitter
            var emitter = lstEmitters.SelectedItem as ParticleEmitter;
            if (emitter == null)
                return;

            // Confirm deletion
            const string confirmMsg = "Are you sure you with to delete the Particle Emitter `{0}`?";
            if (MessageBox.Show(string.Format(confirmMsg, emitter.Name), "Delete emitter?", MessageBoxButtons.YesNo) ==
                DialogResult.No)
                return;

            // Delete
            emitter.Dispose();

            lstEmitters.RemoveItemAndReselect(emitter);
        }

        /// <summary>
        /// Handles the Click event of the btnNewEmitter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnNewEmitter_Click(object sender, EventArgs e)
        {
            // Ensure a valid ParticleEffect is selected
            var pe = ParticleEffect;
            if (pe == null)
                return;

            // Add new emitter. Default to PointEmitter. Type can be changed by the user later.
            var emitter = new PointEmitter(pe);
            if (emitter.Sprite.GrhData == null)
                emitter.Sprite.SetGrh(_defaultEmitterSprite);

            if (!lstEmitters.Items.Contains(emitter))
                lstEmitters.Items.Add(emitter);
        }

        void cmbEmitterType_SelectedEmitterChanged(ParticleEmitterComboBox sender, Type newType)
        {
            if (newType == null)
                return;

            var emitter = pgEmitter.SelectedObject as ParticleEmitter;
            if (emitter == null)
                return;

            if (emitter.GetType() == newType)
                return;

            const string confirmMsg = "Are you sure you wish to change the emitter type from {0} to {1}?";
            if (
                MessageBox.Show(string.Format(confirmMsg, emitter.GetType().Name, newType.Name), "Change emitter type?",
                                MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            var newEmitter = (IParticleEmitter)TypeFactory.GetTypeInstance(newType, emitter.Owner);

            emitter.CopyValuesTo(newEmitter);

            emitter.Dispose();
            lstEmitters.Items.Remove(emitter);

            if (!lstEmitters.Items.Contains(newEmitter))
                lstEmitters.Items.Add(newEmitter);

            lstEmitters.SelectedItem = newEmitter;
        }

        /// <summary>
        /// Handles the MouseMove event of the gameScreen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void gameScreen_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == 0)
                return;

            var emitter = lstEmitters.SelectedItem as ParticleEmitter;
            if (emitter == null)
                return;

            var camera = gameScreen.Camera;
            if (camera == null)
                return;

            emitter.Origin = camera.ToWorld(e.Position());

            if (!lstEmitters.Items.Contains(emitter))
                lstEmitters.Items.Add(emitter);
        }

        /// <summary>
        /// Handles the SelectedValueChanged event of the lstEmitters control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lstEmitters_SelectedValueChanged(object sender, EventArgs e)
        {
            pgEmitter.SelectedObject = null;

            var emitter = lstEmitters.SelectedItem as IParticleEmitter;
            if (emitter != null)
            {
                cmbEmitterType.Enabled = true;
                cmbEmitterType.SelectedItem = emitter.GetType();
            }
            else
            {
                cmbEmitterType.Enabled = false;
                cmbEmitterType.SelectedIndex = -1;
            }

            pgEmitter.SelectedObject = lstEmitters.SelectedItem;
        }

        /// <summary>
        /// Handles the MouseDown event of the gameScreen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void gameScreen_MouseDown(object sender, MouseEventArgs e)
        {
            gameScreen_MouseMove(sender, e);
        }
    }
}