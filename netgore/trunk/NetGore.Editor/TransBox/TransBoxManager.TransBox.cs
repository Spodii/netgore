using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Editor
{
    public partial class TransBoxManager
    {
        /// <summary>
        /// An <see cref="ITransBox"/> that is for a single <see cref="ISpatial"/>, and performs a single operation such
        /// as moving a single <see cref="ISpatial"/> or resizing it in one of the eight directions.
        /// </summary>
        sealed class TransBox : ITransBox
        {
            readonly Vector2 _size;
            readonly ISpatial _spatial;
            readonly TransBoxType _type;

            Vector2 _position;

            /// <summary>
            /// Initializes a new instance of the <see cref="TransBox"/> class.
            /// </summary>
            /// <param name="type">The <see cref="TransBoxType"/>.</param>
            /// <param name="spatial">The <see cref="ISpatial"/>.</param>
            /// <param name="position">The position.</param>
            TransBox(TransBoxType type, ISpatial spatial, Vector2 position)
            {
                _type = type;
                _position = position;
                _spatial = spatial;

                _size = GetTransBoxSize(type);
            }

            /// <summary>
            /// Creates a series of transformation boxes around an entity.
            /// </summary>
            /// <param name="entity">Entity to create the transformation boxes for.</param>
            public static IEnumerable<ITransBox> SurroundEntity(ISpatial entity)
            {
                var ret = new List<ITransBox>(entity.SupportsResize ? 9 : 1);

                var min = entity.Position;
                var max = entity.Max;

                var moveSize = GetTransBoxSize(TransBoxType.Move);
                var moveHalfSize = moveSize / 2f;

                var scaleSize = GetTransBoxSize(TransBoxType.BottomLeft);
                var scaleHalfSize = scaleSize / 2f;

                // Find the center of the sides for the resize and move icons
                var sizeCenter = min + (entity.Size / 2f) - scaleHalfSize;
                sizeCenter = sizeCenter.Round();

                var moveCenterX = min.X + (entity.Size.X / 2f) - moveHalfSize.X;
                moveCenterX = (float)Math.Round(moveCenterX);

                // Move box
                ret.Add(new TransBox(TransBoxType.Move, entity, new Vector2(moveCenterX, min.Y - moveSize.Y - scaleSize.Y)));

                if (entity.SupportsResize)
                {
                    // Four corners
                    ret.Add(new TransBox(TransBoxType.TopLeft, entity, new Vector2(min.X - scaleSize.X, min.Y - scaleSize.Y)));
                    ret.Add(new TransBox(TransBoxType.TopRight, entity, new Vector2(max.X, min.Y - scaleSize.Y)));
                    ret.Add(new TransBox(TransBoxType.BottomLeft, entity, new Vector2(min.X - scaleSize.X, max.Y)));
                    ret.Add(new TransBox(TransBoxType.BottomRight, entity, max));

                    // Horizontal sides
                    if (entity.Size.X > scaleSize.X + 4)
                    {
                        ret.Add(new TransBox(TransBoxType.Top, entity, new Vector2(sizeCenter.X, min.Y - scaleSize.Y)));
                        ret.Add(new TransBox(TransBoxType.Bottom, entity, new Vector2(sizeCenter.X, max.Y)));
                    }

                    // Vertical sides
                    if (entity.Size.Y > scaleSize.Y + 4)
                    {
                        ret.Add(new TransBox(TransBoxType.Left, entity, new Vector2(min.X - scaleSize.X, sizeCenter.Y)));
                        ret.Add(new TransBox(TransBoxType.Right, entity, new Vector2(max.X, sizeCenter.Y)));
                    }
                }

                return ret;
            }

            #region ITransBox Members

            /// <summary>
            /// Gets the max (bottom-right) point of the <see cref="ITransBox"/>.
            /// </summary>
            public Vector2 Max
            {
                get { return Position + Size; }
            }

            /// <summary>
            /// Gets the <see cref="Cursor"/> to display when this <see cref="ITransBox"/> is selected or the mouse is over it.
            /// </summary>
            public Cursor MouseCursor
            {
                get { return GetCursor(_type); }
            }

            /// <summary>
            /// Gets the position of the <see cref="ITransBox"/>.
            /// </summary>
            public Vector2 Position
            {
                get { return _position; }
            }

            /// <summary>
            /// Gets the size of the <see cref="ITransBox"/>.
            /// </summary>
            public Vector2 Size
            {
                get { return _size; }
            }

            /// <summary>
            /// Checks if this <see cref="ITransBox"/> contains the given world point.
            /// </summary>
            /// <param name="worldPos">The world point.</param>
            /// <returns>True if this <see cref="ITransBox"/> contains the <paramref name="worldPos"/>; otherwise false.</returns>
            public bool ContainsPoint(Vector2 worldPos)
            {
                var w = worldPos;
                var lo = Position;
                var hi = Max;
                return (lo.X >= w.X && lo.Y >= w.Y && hi.X <= w.X && hi.Y <= w.Y);
            }

            /// <summary>
            /// Handles when the mouse cursor moves while this <see cref="ITransBox"/> is selected.
            /// </summary>
            /// <param name="offset">The amount the cursor has moved.</param>
            public void CursorMoved(Vector2 offset)
            {
                switch (_type)
                {
                    case TransBoxType.Bottom:
                        _spatial.TryResize(new Vector2(0, offset.Y));
                        break;

                    case TransBoxType.BottomLeft:
                        if (_spatial.TryResize(new Vector2(-offset.X, offset.Y)))
                            _spatial.TryMove(new Vector2(-offset.X, 0));
                        break;

                    case TransBoxType.BottomRight:
                        _spatial.TryResize(offset);
                        break;

                    case TransBoxType.Left:
                        if (_spatial.TryResize(new Vector2(-offset.X, 0)))
                            _spatial.TryMove(new Vector2(-offset.X, 0));
                        break;

                    case TransBoxType.Move:
                        _spatial.TryMove(offset);
                        break;

                    case TransBoxType.Right:
                        _spatial.TryResize(new Vector2(offset.X, 0));
                        break;

                    case TransBoxType.Top:
                        if (_spatial.TryResize(new Vector2(0, -offset.Y)))
                            _spatial.TryMove(new Vector2(0, -offset.Y));
                        break;

                    case TransBoxType.TopLeft:
                        if (_spatial.TryResize(-offset))
                            _spatial.TryMove(-offset);
                        break;

                    case TransBoxType.TopRight:
                        if (_spatial.TryResize(new Vector2(offset.X, -offset.Y)))
                            _spatial.TryResize(new Vector2(offset.X, -offset.Y));
                        break;

                    default:
                        const string errmsg = "Unsupported TransBoxType `{0}`.";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, _type);
                        Debug.Fail(string.Format(errmsg, _type));
                        break;
                }

                _position += offset;
            }

            /// <summary>
            /// Draws the <see cref="ITransBox"/>.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
            /// <param name="camera">The <see cref="ICamera2D"/>.</param>
            public void Draw(ISpriteBatch spriteBatch, ICamera2D camera)
            {
                var sprite = GetSprite(_type);
                if (sprite == null)
                    return;

                var p = camera.ToScreen(Position).Round();
                var s = Size.Round();
                var r = new Rectangle((int)p.X, (int)p.Y, (int)s.X, (int)s.Y);

                sprite.Draw(spriteBatch, r, Color.White);
            }

            #endregion
        }
    }
}