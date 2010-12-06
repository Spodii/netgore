using System;
using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics.ParticleEngine;

namespace NetGore.Editor.WinForms
{
    /// <summary>
    /// A list box specifically for particle effects, containing the names of all the particle effects available.
    /// </summary>
    public class ParticleEffectListBox : ListBox
    {
        /// <summary>
        /// Notifies listeners when this <see cref="ParticleEffectListBox"/> requests to create a particle effect
        /// instance.
        /// </summary>
        public event TypedEventHandler<ParticleEffectListBox, ParticleEffectListBoxCreateEventArgs> RequestCreateEffect;

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.Control.CreateControl"/> method.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (DesignMode)
                return;

            Items.Clear();
            Items.AddRange(ParticleEffectManager.Instance.ParticleEffectNames.OfType<object>().ToArray());
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.DoubleClick"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);

            if (DesignMode)
                return;

            var name = SelectedItem as string;
            if (!string.IsNullOrEmpty(name))
            {
                if (RequestCreateEffect != null)
                    RequestCreateEffect.Raise(this, new ParticleEffectListBoxCreateEventArgs(name));
            }
        }
    }
}