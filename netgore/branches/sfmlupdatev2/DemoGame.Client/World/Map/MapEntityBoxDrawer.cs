using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.Graphics;
using NetGore.World;

namespace DemoGame.Client
{
    /// <summary>
    /// Draws the visual representation of each <see cref="Entity"/> in the map.
    /// </summary>
    public class MapEntityBoxDrawer : MapDrawingExtension
    {
        /// <summary>
        /// These types will be drawn even if they are not in the view area. This is for types that draw outside of their visible
        /// area, such as to show destination indicators for a teleporter.
        /// </summary>
        static readonly IEnumerable<Type> _alwaysDrawTypes = new Type[] { typeof(TeleportEntityBase) };

        /// <summary>
        /// These types will never be drawn by the <see cref="MapEntityBoxDrawer"/>. They often indicate types that handle
        /// the drawing in a different way.
        /// </summary>
        static readonly IEnumerable<Type> _neverDrawTypes = new Type[] { typeof(WallEntityBase) };

        static bool GetEntitiesToAlwaysDrawFilter(Entity e)
        {
            var t = e.GetType();

            // Skip the entities we will never draw (_neverDrawTypes)
            if (_neverDrawTypes.Any(x => t.IsSubclassOf(x) || t == x))
                return false;

            // Get the entities we will always draw (_alwaysDrawTypes)
            return _alwaysDrawTypes.Any(x => t.IsSubclassOf(x) || t == x);
        }

        static bool GetEntitiesToDrawFilter(Entity e)
        {
            var t = e.GetType();

            // Skip the entities we will never draw (_neverDrawTypes)
            if (_neverDrawTypes.Any(x => t.IsSubclassOf(x) || t == x))
                return false;

            // Skip the entities we will always draw (_alwaysDrawTypes) since we will just add all of those later
            if (_alwaysDrawTypes.Any(x => t.IsSubclassOf(x) || t == x))
                return false;

            return true;
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing to the map after the given <see cref="MapRenderLayer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="e">The <see cref="NetGore.Graphics.DrawableMapDrawLayerEventArgs"/> instance containing the event data.</param>
        protected override void HandleDrawAfterLayer(IDrawableMap map, DrawableMapDrawLayerEventArgs e)
        {
            if (e.Layer != MapRenderLayer.SpriteForeground)
                return;

            // Get the visible area
            var visibleArea = e.Camera.GetViewArea();

            // Get and draw all entities, skipping those that we will never draw () and those that
            // we will always draw (_alwaysDrawTypes)
            var toDraw = map.Spatial.GetMany<Entity>(visibleArea, GetEntitiesToDrawFilter);

            // Add the entities we will always draw
            toDraw = toDraw.Concat(map.Spatial.GetMany<Entity>(GetEntitiesToAlwaysDrawFilter));

            // Draw
            foreach (var entity in toDraw)
            {
                EntityDrawer.Draw(e.SpriteBatch, e.Camera, entity);
            }
        }
    }
}