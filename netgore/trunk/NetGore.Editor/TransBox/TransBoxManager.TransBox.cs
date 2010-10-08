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
            TransBox(TransBoxType type, ISpatial spatial)
            {
                _type = type;
                _spatial = spatial;

                _size = GetTransBoxSize(type);
                _position = GetPosition();
            }

            /// <summary>
            /// Gets the position to use for the <see cref="TransBox"/>.
            /// </summary>
            /// <returns>The position for the <see cref="TransBox"/>.</returns>
            Vector2 GetPosition()
            {
                switch (_type)
                {
                    case TransBoxType.Bottom:
                        return new Vector2(_spatial.Center.X - (Size.X / 2f), _spatial.Max.Y);

                    case TransBoxType.BottomLeft:
                        return new Vector2(_spatial.Position.X - Size.X, _spatial.Max.Y);

                    case TransBoxType.BottomRight:
                        return _spatial.Max;

                    case TransBoxType.Left:
                        return new Vector2(_spatial.Position.X - Size.X, _spatial.Center.Y - (Size.Y / 2f));

                    case TransBoxType.Move:
                        return new Vector2(_spatial.Center.X - (Size.X / 2f),
                                           _spatial.Position.Y - Size.Y - GetTransBoxSize(TransBoxType.Top).Y);

                    case TransBoxType.Right:
                        return new Vector2(_spatial.Max.X, _spatial.Center.Y - (Size.Y / 2f));

                    case TransBoxType.Top:
                        return new Vector2(_spatial.Center.X - (Size.X / 2f), _spatial.Position.Y - Size.Y);

                    case TransBoxType.TopLeft:
                        return _spatial.Position - Size;

                    case TransBoxType.TopRight:
                        return new Vector2(_spatial.Max.X, _spatial.Position.Y - Size.Y);

                    default:
                        const string errmsg = "Unsupported TransBoxType `{0}`.";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, _type);
                        Debug.Fail(string.Format(errmsg, _type));
                        return _spatial.Center;
                }
            }

            /// <summary>
            /// Creates a series of transformation boxes around an entity.
            /// </summary>
            /// <param name="entity">Entity to create the transformation boxes for.</param>
            public static IEnumerable<ITransBox> SurroundEntity(ISpatial entity)
            {
                var ret = new List<ITransBox>(entity.SupportsResize ? 9 : 1);
                var scaleSize = GetTransBoxSize(TransBoxType.BottomLeft);

                // Move box
                ret.Add(new TransBox(TransBoxType.Move, entity));

                if (entity.SupportsResize)
                {
                    // Four corners
                    ret.Add(new TransBox(TransBoxType.TopLeft, entity));
                    ret.Add(new TransBox(TransBoxType.TopRight, entity));
                    ret.Add(new TransBox(TransBoxType.BottomLeft, entity));
                    ret.Add(new TransBox(TransBoxType.BottomRight, entity));

                    // Horizontal sides
                    if (entity.Size.X > scaleSize.X + 4)
                    {
                        ret.Add(new TransBox(TransBoxType.Top, entity));
                        ret.Add(new TransBox(TransBoxType.Bottom, entity));
                    }

                    // Vertical sides
                    if (entity.Size.Y > scaleSize.Y + 4)
                    {
                        ret.Add(new TransBox(TransBoxType.Left, entity));
                        ret.Add(new TransBox(TransBoxType.Right, entity));
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
                return (lo.X <= w.X) && (lo.Y <= w.Y) && (hi.X >= w.X) && (hi.Y >= w.Y);
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
                        _spatial.TryResize(_spatial.Size + new Vector2(0, offset.Y));
                        break;

                    case TransBoxType.BottomLeft:
                        if (_spatial.TryResize(_spatial.Size + new Vector2(-offset.X, offset.Y)))
                            _spatial.TryMove(_spatial.Position + new Vector2(offset.X, 0));
                        break;

                    case TransBoxType.BottomRight:
                        _spatial.TryResize(_spatial.Size + offset);
                        break;

                    case TransBoxType.Left:
                        if (_spatial.TryResize(_spatial.Size + new Vector2(-offset.X, 0)))
                            _spatial.TryMove(_spatial.Position + new Vector2(offset.X, 0));
                        break;

                    case TransBoxType.Move:
                        _spatial.TryMove(_spatial.Position + offset);
                        break;

                    case TransBoxType.Right:
                        _spatial.TryResize(_spatial.Size + new Vector2(offset.X, 0));
                        break;

                    case TransBoxType.Top:
                        if (_spatial.TryResize(_spatial.Size + new Vector2(0, -offset.Y)))
                            _spatial.TryMove(_spatial.Position + new Vector2(0, offset.Y));
                        break;

                    case TransBoxType.TopLeft:
                        if (_spatial.TryResize(_spatial.Size + -offset))
                            _spatial.TryMove(_spatial.Position + offset);
                        break;

                    case TransBoxType.TopRight:
                        if (_spatial.TryResize(_spatial.Size + new Vector2(offset.X, -offset.Y)))
                            _spatial.TryMove(_spatial.Position + new Vector2(0, offset.Y));
                        break;

                    default:
                        const string errmsg = "Unsupported TransBoxType `{0}`.";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, _type);
                        Debug.Fail(string.Format(errmsg, _type));
                        break;
                }

                _position = GetPosition();
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

            /// <summary>
            /// Updates the <see cref="ITransBox"/>.
            /// </summary>
            /// <param name="currentTime">The current time.</param>
            public void Update(TickCount currentTime)
            {
                _position = GetPosition();
            }

            #endregion
        }
    }
}