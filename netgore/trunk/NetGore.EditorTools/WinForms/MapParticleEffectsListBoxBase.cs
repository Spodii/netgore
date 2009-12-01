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
    }
}
