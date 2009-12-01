using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        /// Gets the path to the particle effects directory.
        /// </summary>
        static PathString EffectsDirectory { get { return ContentPaths.Dev.ParticleEffects; } }
        
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

            var effectFiles = Directory.GetFiles(EffectsDirectory);
            return effectFiles.Select(x => ParticleEmitterFactory.GetEffectNameFromPath(x));
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.DoubleClick"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);

            var name = TypedSelectedItem;
            if (!string.IsNullOrEmpty(name))
            {
                if (RequestCreateEffect != null)
                    RequestCreateEffect(this, name);
            }
        }
    }
}
