using System.Linq;
using NetGore.Graphics;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Assists in drawing different types of Entities.
    /// </summary>
    public static class EntityDrawer
    {
        /// <summary>
        /// Color for an arrow.
        /// </summary>
        static readonly Color _arrowColor = new Color(255, 255, 255, 150);

        /// <summary>
        /// Border color of the Entity.
        /// </summary>
        static readonly Color _borderColor = new Color(0, 0, 0, 255);

        /// <summary>
        /// Basic Entity color.
        /// </summary>
        static readonly Color _entityColor = new Color(0, 0, 255, 150);

        /// <summary>
        /// Color of the destination of a TeleportEntity.
        /// </summary>
        static readonly Color _teleDestColor = new Color(255, 0, 0, 75);

        /// <summary>
        /// Color of the source TeleportEntity.
        /// </summary>
        static readonly Color _teleSourceColor = new Color(0, 255, 0, 150);

        /// <summary>
        /// Color of WallEntities.
        /// </summary>
        static readonly Color _wallColor = new Color(255, 255, 255, 100);

        /// <summary>
        /// Draws an Entity.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the current view.</param>
        /// <param name="entity">Entity to draw.</param>
        public static void Draw(ISpriteBatch sb, ICamera2D camera, Entity entity)
        {
            WallEntityBase wallEntity;
            TeleportEntity teleportEntity;

            // Check for a different entity type
            if ((wallEntity = entity as WallEntityBase) != null)
                Draw(sb, camera, wallEntity);
            else if ((teleportEntity = entity as TeleportEntity) != null)
                Draw(sb, camera, teleportEntity);
            else
            {
                // Draw a normal entity using the CollisionBox
                Draw(sb, entity.ToRectangle(), _entityColor);
            }
        }

        /// <summary>
        /// Draws a TeleportEntity.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the current view.</param>
        /// <param name="tele">TeleportEntity to draw.</param>
        public static void Draw(ISpriteBatch sb, ICamera2D camera, TeleportEntityBase tele)
        {
            // Draw the source rectangle
            Draw(sb, tele.ToRectangle(), _teleSourceColor);

            // Draw the destination rectangle and the arrow pointing to it only if the map is the same
            if (camera.Map != null && camera.Map.ID == tele.DestinationMap)
            {
                var destRect = new Rectangle((int)tele.Destination.X, (int)tele.Destination.Y, (int)tele.Size.X, (int)tele.Size.Y);
                Draw(sb, destRect, _teleDestColor);

                // Arrow
                var centerOffset = tele.Size / 2;
                RenderArrow.Draw(sb, tele.Position + centerOffset, tele.Destination + centerOffset, _arrowColor);
            }
        }

        /// <summary>
        /// Draws a WallEntity.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the current view.</param>
        /// <param name="wall">WallEntity to draw.</param>
        public static void Draw(ISpriteBatch sb, ICamera2D camera, WallEntityBase wall)
        {
            Draw(sb, camera, wall, Vector2.Zero);
        }

        /// <summary>
        /// Draws a WallEntity
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the current view.</param>
        /// <param name="wall">WallEntity to draw</param>
        /// <param name="offset">Offset to draw the WallEntity at from the original position</param>
        public static void Draw(ISpriteBatch sb, ICamera2D camera, WallEntityBase wall, Vector2 offset)
        {
            // Find the positon to draw to
            var p = wall.Position + offset;
            var dest = new Rectangle((int)p.X, (int)p.Y, (int)wall.Size.X, (int)wall.Size.Y);

            // Draw the collision area
            RenderRectangle.Draw(sb, dest, _wallColor);
        }

        /// <summary>
        /// Draws a <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="rect">The <see cref="Rectangle"/> to draw.</param>
        /// <param name="color">Color to draw the CollisionBox.</param>
        static void Draw(ISpriteBatch sb, Rectangle rect, Color color)
        {
            RenderRectangle.Draw(sb, rect, color, _borderColor);
        }
    }
}