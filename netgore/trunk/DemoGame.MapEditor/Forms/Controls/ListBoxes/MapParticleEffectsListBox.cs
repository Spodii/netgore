using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoGame.Client;
using NetGore.EditorTools;
using NetGore.Graphics.ParticleEngine;

namespace DemoGame.MapEditor
{
    public class MapParticleEffectsListBox : MapParticleEffectsListBoxBase<Map>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapParticleEffectsListBox"/> class.
        /// </summary>
        public MapParticleEffectsListBox()
            : base(true, false, true)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates a clone of the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Object to clone.</param>
        protected override void Clone(ParticleEmitter item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in the derived class, deletes the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Object to delete.</param>
        protected override void Delete(ParticleEmitter item)
        {
            Map.ParticleEffects.Remove(item);
        }

        /// <summary>
        /// When overridden in the derived class, gets an IEnumerable of objects to be used in this MapItemListBox.
        /// </summary>
        /// <returns>An IEnumerable of objects to be used in this MapItemListBox.</returns>
        protected override IEnumerable<ParticleEmitter> GetItems()
        {
            return Map.ParticleEffects;
        }

        /// <summary>
        /// When overridden in the derived class, centers the camera on the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Object to locate.</param>
        protected override void Locate(ParticleEmitter item)
        {
            Camera.CenterOn(item.Origin);
        }
    }
}
