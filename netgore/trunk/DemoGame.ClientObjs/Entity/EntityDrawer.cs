using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Assists in drawing different types of Entities.
    /// </summary>
    public static class EntityDrawer
    {
        /// <summary>
        /// Color for an arrow
        /// </summary>
        static readonly Color _arrowColor = new Color(255, 255, 255, 150);

        /// <summary>
        /// Border color of the Entity
        /// </summary>
        static readonly Color _borderColor = new Color(0, 0, 0, 255);

        /// <summary>
        /// Basic Entity color
        /// </summary>
        static readonly Color _entityColor = new Color(0, 0, 255, 150);

        /// <summary>
        /// Color of the destination of a TeleportEntity
        /// </summary>
        static readonly Color _teleDestColor = new Color(255, 0, 0, 75);

        /// <summary>
        /// Color of the source TeleportEntity
        /// </summary>
        static readonly Color _teleSourceColor = new Color(0, 255, 0, 150);

        /// <summary>
        /// Color of WallEntities
        /// </summary>
        static readonly Color _wallColor = new Color(255, 255, 255, 100);

        /// <summary>
        /// Draws an Entity
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        /// <param name="entity">Entity to draw</param>
        public static void Draw(SpriteBatch sb, Entity entity)
        {
            WallEntityBase wallEntity;
            TeleportEntity teleportEntity;

            // Check for a different entity type
            if ((wallEntity = entity as WallEntityBase) != null)
                Draw(sb, wallEntity);
            else if ((teleportEntity = entity as TeleportEntity) != null)
                Draw(sb, teleportEntity);
            else
            {
                // Draw a normal entity using the CollisionBox
                DrawCB(sb, entity.CB, entity.CollisionType, _entityColor);
            }
        }

        /// <summary>
        /// Draws a TeleportEntity
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        /// <param name="tele">TeleportEntity to draw</param>
        public static void Draw(SpriteBatch sb, TeleportEntity tele)
        {
            // Source
            DrawCB(sb, tele.CB, tele.CollisionType, _teleSourceColor);

            // Dest
            CollisionBox destCB = new CollisionBox(tele.Destination, tele.CB.Width, tele.CB.Height);
            DrawCB(sb, destCB, CollisionType.Full, _teleDestColor);

            // Arrow
            Vector2 centerOffset = tele.CB.Size / 2;
            XNAArrow.Draw(sb, tele.Position + centerOffset, tele.Destination + centerOffset, _arrowColor);
        }

        /// <summary>
        /// Draws a WallEntity
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        /// <param name="wall">WallEntity to draw</param>
        public static void Draw(SpriteBatch sb, WallEntityBase wall)
        {
            Draw(sb, wall, Vector2.Zero);
        }

        /// <summary>
        /// Draws a WallEntity
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        /// <param name="wall">WallEntity to draw</param>
        /// <param name="offset">Offset to draw the WallEntity at from the original position</param>
        public static void Draw(SpriteBatch sb, WallEntityBase wall, Vector2 offset)
        {
            // Find the positon to draw to
            Vector2 p = wall.Position + offset;
            Rectangle dest = new Rectangle((int)p.X, (int)p.Y, (int)wall.CB.Width, (int)wall.CB.Height);

            // Draw the collision box
            switch (wall.CollisionType)
            {
                case CollisionType.Full:
                    XNARectangle.Draw(sb, dest, _wallColor);
                    break;

                case CollisionType.TriangleTopLeft:
                    XNATriangle.Draw(sb, dest, _wallColor, SpriteEffects.FlipHorizontally, 0f, Vector2.Zero);
                    break;

                case CollisionType.TriangleTopRight:
                    XNATriangle.Draw(sb, dest, _wallColor, SpriteEffects.None, 0f, Vector2.Zero);
                    break;
            }
        }

        /// <summary>
        /// Draws a CollisionBox
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        /// <param name="cb">CollisionBox to draw</param>
        /// <param name="ct">Type of collision the CollisionBox supports</param>
        /// <param name="color">Color to draw the CollisionBox</param>
        static void DrawCB(SpriteBatch sb, CollisionBox cb, CollisionType ct, Color color)
        {
            Rectangle dest = cb.ToRectangle();

            // LATER: Border support for the other shapes
            switch (ct)
            {
                case CollisionType.Full:
                    XNARectangle.Draw(sb, dest, color, _borderColor);
                    break;

                case CollisionType.TriangleTopLeft:
                    XNATriangle.Draw(sb, dest, color, SpriteEffects.FlipHorizontally, 0f, Vector2.Zero);
                    break;

                case CollisionType.TriangleTopRight:
                    XNATriangle.Draw(sb, dest, color, SpriteEffects.None, 0f, Vector2.Zero);
                    break;
            }
        }
    }
}