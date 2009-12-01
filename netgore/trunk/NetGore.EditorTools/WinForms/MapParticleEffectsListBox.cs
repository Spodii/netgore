using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.Graphics.ParticleEngine;

namespace NetGore.EditorTools.WinForms
{
    /// <summary>
    /// A <see cref="ListBox"/> containing the particle effects on a map.
    /// </summary>
    public abstract class MapParticleEffectsListBox<TMap> : MapItemListBox<TMap, ParticleEmitter> where TMap : class, IMap
    {
    }
}
