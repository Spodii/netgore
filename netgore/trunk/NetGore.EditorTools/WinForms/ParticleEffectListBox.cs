using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics.ParticleEngine;
using NetGore.IO;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Handles when the <see cref="ParticleEffectListBox"/> requests to create a particle effect instance.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="effectName">The name of the particle effect to create.</param>
    public delegate void ParticleEffectListBoxCreateEventHandler(ParticleEffectListBox sender, string effectName);

    /// <summary>
    /// A list box specifically for particle effects, containing the names of all the particle effects available.
    /// </summary>
    public class ParticleEffectListBox : TypedListBox<string>
    {
        /// <summary>
        /// Notifies listeners when this <see cref="ParticleEffectListBox"/> requests to create a particle effect
        /// instance.
        /// </summary>
        public event ParticleEffectListBoxCreateEventHandler RequestCreateEffect;

        /// <summary>
        /// Gets the items to initially populate the <see cref="Control"/>'s collection with.
        /// </summary>
        /// <returns>
        /// The items to initially populate the <see cref="Control"/>'s collection with.
        /// </returns>
        protected override IEnumerable<string> GetInitialItems()
        {
            // Don't return the values while in design mode
            if (DesignMode)
                return Enumerable.Empty<string>();

            return ParticleEffectManager.Instance.ParticleEffectNames;
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

            var name = TypedSelectedItem;
            if (!string.IsNullOrEmpty(name))
            {
                if (RequestCreateEffect != null)
                    RequestCreateEffect(this, name);
            }
        }
    }
}