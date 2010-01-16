using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoGame.Client;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;

namespace DemoGame.MapEditor
{
    class MapPersistentNPCDrawer : MapDrawingExtension
    {
        readonly PersistentNPCListBox _persistentNPCs;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapPersistentNPCDrawer"/> class.
        /// </summary>
        /// <param name="persistentNPCs">The <see cref="PersistentNPCListBox"/> with the NPCs to draw.</param>
        public MapPersistentNPCDrawer(PersistentNPCListBox persistentNPCs)
        {
            if (persistentNPCs == null)
                throw new ArgumentNullException("persistentNPCs");

            _persistentNPCs = persistentNPCs;
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing to the map after the given <paramref name="layer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="layer">The layer that was just drawn.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw to.</param>
        protected override void HandleDrawAfterLayer(IDrawableMap map, MapRenderLayer layer, SpriteBatch spriteBatch)
        {
            if (layer != MapRenderLayer.Chararacter)
                return;

            foreach (var c in _persistentNPCs.Items.OfType<MapEditorCharacter>())
            {
                c.Draw(spriteBatch);
            }
        }
    }
}
