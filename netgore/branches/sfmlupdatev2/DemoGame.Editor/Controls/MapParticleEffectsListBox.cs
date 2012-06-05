using System.Collections.Generic;
using System.Linq;
using NetGore.Editor.WinForms;
using NetGore.Graphics.ParticleEngine;

namespace DemoGame.Editor
{
    public class MapParticleEffectsListBox : MapParticleEffectsListBoxBase<EditorMap>
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