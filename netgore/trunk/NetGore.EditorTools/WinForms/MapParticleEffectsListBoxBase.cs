using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.Graphics.ParticleEngine;

namespace NetGore.EditorTools
{
    /// <summary>
    /// A <see cref="ListBox"/> containing the particle effects on a map.
    /// </summary>
    public abstract class MapParticleEffectsListBoxBase<TMap> : MapItemListBox<TMap, ParticleEmitter> where TMap : class, IMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapItemListBox&lt;TMap, TItem&gt;"/> class.
        /// </summary>
        /// <param name="supportsLocate">Whether or not the Locate operation is supported.</param>
        /// <param name="supportsClone">Whether or not the Clone operation is supported.</param>
        /// <param name="supportsDelete">Whether or not the Delete operation is supported.</param>
        protected MapParticleEffectsListBoxBase(bool supportsLocate, bool supportsClone, bool supportsDelete) : base(supportsLocate, supportsClone, supportsDelete)
        {
        }
    }
}
