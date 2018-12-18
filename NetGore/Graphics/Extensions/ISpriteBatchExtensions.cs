using System.Linq;
using System.Reflection;
using log4net;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Extensions for the <see cref="ISpriteBatch"/> interface.
    /// </summary>
    public static class ISpriteBatchExtensions
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The <see cref="SFML.Graphics.Sprite"/> used for repeated draws.
        /// </summary>
        static readonly SFML.Graphics.Sprite _repeatSprite = new SFML.Graphics.Sprite { Scale = Vector2.One, Rotation = 0f, Origin = Vector2.Zero };

        /// <summary>
        /// Checks if a <see cref="ISprite"/> is valid to be drawn.
        /// </summary>
        /// <param name="sprite">The sprite to draw.</param>
        /// <returns>True if the <paramref name="sprite"/> can be used to draw; otherwise false.</returns>
        static bool CanDrawSprite(ISprite sprite)
        {
            if (sprite == null)
            {
                const string errmsg = "Attempted to draw using a null sprite.";
                if (log.IsWarnEnabled)
                    log.Warn(errmsg);
                return false;
            }

            if (sprite.Texture == null || sprite.Texture.IsDisposed)
            {
                const string errmsg = "Attempted to draw using sprite `{0}`, but the texture is not set or is disposed.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, sprite);
                return false;
            }

            if (sprite.Size.X < 1 || sprite.Size.Y < 1)
            {
                const string errmsg = "Attempted to draw using sprite `{0}`, but the size (width and/or height) is < 1.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, sprite);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Draws a string with shading.
        /// </summary>
        /// <param name="spriteBatch"><see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="font"><see cref="Font"/> to draw the string with.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="position">The position of the top-left corner of the string to draw.</param>
        /// <param name="fontColor">The font color.</param>
        /// <param name="borderColor">The shading color.</param>
        public static void DrawStringShaded(this ISpriteBatch spriteBatch, Font font, string text, Vector2 position,
                                            Color fontColor, Color borderColor)
        {
            position = position.Round();

            spriteBatch.DrawString(font, text, position - new Vector2(0, 1), borderColor);
            spriteBatch.DrawString(font, text, position - new Vector2(1, 0), borderColor);
            spriteBatch.DrawString(font, text, position + new Vector2(0, 1), borderColor);
            spriteBatch.DrawString(font, text, position + new Vector2(1, 0), borderColor);

            spriteBatch.DrawString(font, text, position, fontColor);
        }

        /// <summary>
        /// Draws a <see cref="ISprite"/> tiled on the X axis.
        /// The <see cref="ISprite"/> is never scaled. If a fraction of a sprite must be drawn (the amount to draw is not
        /// perfectly divisible by the sprite's size) then only a portion of the sprite will be drawn.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="minX">The starting X coordinate to draw at.</param>
        /// <param name="maxX">The ending X coordinate to draw at.</param>
        /// <param name="y">The Y coordinate to draw at.</param>
        /// <param name="s">The <see cref="ISprite"/> to draw.</param>
        /// <param name="color">The color to draw the <see cref="ISprite"/>.</param>
        /// <param name="drawHeight">When a value greater than 0 is given, this will be used as the height of the drawn sprite instead
        /// of the default height for the <paramref name="s"/>. This has no affect on the actual tiling.</param>
        public static void DrawTiledX(this ISpriteBatch sb, int minX, int maxX, int y, ISprite s, Color color, int drawHeight = 0)
        {
            if (!CanDrawSprite(s))
                return;

            if (maxX < minX)
            {
                const string errmsg = "Unable to draw sprite `{0}` since MaxX ({1}) < MinX ({2}).";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, s, maxX, minX);
                return;
            }

            var src = s.Source;
            var destSize = maxX - minX;
            var fullSprites = destSize / s.Source.Width;
            var remainder = destSize % s.Source.Width;

            if (drawHeight <= 0)
                drawHeight = (int)s.Size.Y;

            // Set the sprite in general
            _repeatSprite.Color = color;
            _repeatSprite.Texture = s.Texture;
            _repeatSprite.Scale = Vector2.One;
            // NOTE: Line removed in SFML upgrade, and not sure if still needed: _repeatSprite.Height = drawHeight;

            // Set up the sprite for the full pieces
            if (fullSprites > 0)
            {
                _repeatSprite.TextureRect = new IntRect(src.X, src.Y, src.Width, src.Height);

                // Draw all the full pieces
                for (var x = 0; x < fullSprites; x++)
                {
                    _repeatSprite.Position = new Vector2(minX + (x * src.Width), y);
                    sb.Draw(_repeatSprite);
                }
            }

            // Draw the remaining partial piece
            if (remainder > 0)
            {
                _repeatSprite.TextureRect = new IntRect(src.X, src.Y, remainder, src.Height);
                _repeatSprite.Position = new Vector2(maxX - remainder, y);

                sb.Draw(_repeatSprite);
            }
        }

        /// <summary>
        /// Draws a <see cref="ISprite"/> tiled on both the X and Y axis.
        /// The <see cref="ISprite"/> is never scaled. If a fraction of a sprite must be drawn (the amount to draw is not
        /// perfectly divisible by the sprite's size) then only a portion of the sprite will be drawn.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="minX">The starting X coordinate to draw at.</param>
        /// <param name="maxX">The ending X coordinate to draw at.</param>
        /// <param name="minY">The starting Y coordinate to draw at.</param>
        /// <param name="maxY">The ending Y coordinate to draw at.</param>
        /// <param name="s">The <see cref="ISprite"/> to draw.</param>
        /// <param name="color">The color to draw the <see cref="ISprite"/>.</param>
        public static void DrawTiledXY(this ISpriteBatch sb, int minX, int maxX, int minY, int maxY, ISprite s, Color color)
        {
            if (!CanDrawSprite(s))
                return;

            if (maxX < minX)
            {
                const string errmsg = "Unable to draw sprite `{0}` since MaxX ({1}) < MinX ({2}).";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, s, maxX, minX);
                return;
            }

            if (maxY < minY)
            {
                const string errmsg = "Unable to draw sprite `{0}` since MaxY ({1}) < MinY ({2}).";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, s, maxY, minY);
                return;
            }

            var src = s.Source;
            var destSizeX = maxX - minX;
            var destSizeY = maxY - minY;
            var fullSpritesX = destSizeX / s.Source.Width;
            var fullSpritesY = destSizeY / s.Source.Height;
            var remainderX = destSizeX % s.Source.Width;
            var remainderY = destSizeY % s.Source.Height;

            // Set the sprite in general
            _repeatSprite.Color = color;
            _repeatSprite.Texture = s.Texture;
            _repeatSprite.Scale = Vector2.One;

            // Set up the sprite for the full pieces
            if (fullSpritesX > 0 || fullSpritesY > 0)
            {
                _repeatSprite.TextureRect = new IntRect(src.X, src.Y, src.Width, src.Height);

                // Draw all the full pieces
                for (var x = 0; x < fullSpritesX; x++)
                {
                    for (var y = 0; y < fullSpritesY; y++)
                    {
                        _repeatSprite.Position = new Vector2(minX + (x * src.Width), minY + (y * src.Height));
                        sb.Draw(_repeatSprite);
                    }
                }
            }

            // Draw the remaining partial pieces on the sides
            if (remainderX > 0 && fullSpritesY > 0)
            {
                _repeatSprite.TextureRect = new IntRect(src.X, src.Y, remainderX, src.Height);

                float x = maxX - remainderX;
                for (var y = 0; y < fullSpritesY; y++)
                {
                    _repeatSprite.Position = new Vector2(x, minY + (y * src.Height));
                    sb.Draw(_repeatSprite);
                }
            }

            if (remainderY > 0 && fullSpritesX > 0)
            {
                _repeatSprite.TextureRect = new IntRect(src.X, src.Y, src.Width, remainderY);

                float y = maxY - remainderY;
                for (var x = 0; x < fullSpritesX; x++)
                {
                    _repeatSprite.Position = new Vector2(minX + (x * src.Width), y);
                    sb.Draw(_repeatSprite);
                }
            }

            // Draw the single partial piece partial for both axis (bottom-right corner)
            if (remainderX > 0 && remainderY > 0)
            {
                _repeatSprite.TextureRect = new IntRect(src.X, src.Y, remainderX, remainderY);
                _repeatSprite.Position = new Vector2(maxX - remainderX, maxY - remainderY);
                sb.Draw(_repeatSprite);
            }
        }

        /// <summary>
        /// Draws a <see cref="ISprite"/> tiled on the Y axis.
        /// The <see cref="ISprite"/> is never scaled. If a fraction of a sprite must be drawn (the amount to draw is not
        /// perfectly divisible by the sprite's size) then only a portion of the sprite will be drawn.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="minY">The starting Y coordinate to draw at.</param>
        /// <param name="maxY">The ending Y coordinate to draw at.</param>
        /// <param name="x">The X coordinate to draw at.</param>
        /// <param name="s">The <see cref="ISprite"/> to draw.</param>
        /// <param name="color">The color to draw the <see cref="ISprite"/>.</param>
        /// <param name="drawWidth">When a value greater than 0 is given, this will be used as the width of the drawn sprite instead
        /// of the default width for the <paramref name="s"/>. This has no affect on the actual tiling.</param>
        public static void DrawTiledY(this ISpriteBatch sb, int minY, int maxY, int x, ISprite s, Color color, int drawWidth = 0)
        {
            if (!CanDrawSprite(s))
                return;

            if (maxY < minY)
            {
                const string errmsg = "Unable to draw sprite `{0}` since MaxY ({1}) < MinY ({2}).";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, s, maxY, minY);
                return;
            }

            var src = s.Source;
            var destSize = maxY - minY;
            var fullSprites = destSize / s.Source.Height;
            var remainder = destSize % s.Source.Height;

            if (drawWidth <= 0)
                drawWidth = (int)s.Size.X;

            // Set the sprite in general
            _repeatSprite.Color = color;
            _repeatSprite.Texture = s.Texture;
            _repeatSprite.Scale = Vector2.One;
            // NOTE: Line removed in SFML upgrade, and not sure if still needed: _repeatSprite.Width = drawWidth;

            // Set up the sprite for the full pieces
            if (fullSprites > 0)
            {
                _repeatSprite.TextureRect = new IntRect(src.X, src.Y, src.Width, src.Height);

                // Draw all the full pieces
                for (var y = 0; y < fullSprites; y++)
                {
                    _repeatSprite.Position = new Vector2(x, minY + (y * src.Height));
                    sb.Draw(_repeatSprite);
                }
            }

            // Draw the remaining partial piece
            if (remainder > 0)
            {
                _repeatSprite.TextureRect = new IntRect(src.X, src.Y, src.Width, remainder);
                _repeatSprite.Position = new Vector2(x, maxY - remainder);
                sb.Draw(_repeatSprite);
            }
        }
    }
}