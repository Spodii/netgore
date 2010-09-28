using System.Collections.Generic;
using System.Linq;
using DemoGame.Client;
using NetGore.EditorTools;
using NetGore.Graphics.ParticleEngine;

namespace DemoGame.MapEditor
{
    public class MapParticleEffectsListBox : MapParticleEffectsListBoxBase<Map>
    {
        /// <summary>
        /// When overridden in the derived class, gets an IEnumerable of objects to be used in this MapItemListBox.
        /// </summary>
        /// <returns>An IEnumerable of objects to be used in this MapItemListBox.</returns>
        protected override IEnumerable<ParticleEffectReference> GetItems()
        {
            return Map.ParticleEffects;
        }
    }
}