using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics.ParticleEngine;
using NetGore.World;

namespace NetGore.Editor.WinForms
{
    /// <summary>
    /// A <see cref="ListBox"/> containing the particle effects on a map.
    /// </summary>
    public abstract class MapParticleEffectsListBoxBase<TMap> : MapItemListBox<TMap, ParticleEffectReference>
        where TMap : class, IMap
    {
    }
}