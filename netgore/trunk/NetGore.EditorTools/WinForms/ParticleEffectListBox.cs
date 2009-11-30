using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.Graphics.ParticleEngine;

namespace NetGore.EditorTools.WinForms
{
    /// <summary>
    /// A list box specifically for particle effects.
    /// </summary>
    public class ParticleEffectListBox : TypedListBox<ParticleEmitter>
    {
        /// <summary>
        /// Gets the items to initially populate the <see cref="Control"/>'s collection with.
        /// </summary>
        /// <returns>
        /// The items to initially populate the <see cref="Control"/>'s collection with.
        /// </returns>
        protected override IEnumerable<ParticleEmitter> GetInitialItems()
        {
            return base.GetInitialItems();
        }
    }
}
