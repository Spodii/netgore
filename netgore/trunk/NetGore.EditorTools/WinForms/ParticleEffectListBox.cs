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
    /// A list box specifically for particle effects, containing the names of all the particle effects available.
    /// </summary>
    public class ParticleEffectListBox : TypedListBox<string>
    {
        /// <summary>
        /// Gets the path to the particle effects directory.
        /// </summary>
        static PathString EffectsDirectory { get { return ContentPaths.Dev.ParticleEffects; } }

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
    }
}
