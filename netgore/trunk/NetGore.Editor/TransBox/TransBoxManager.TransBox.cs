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
            readonly TransBoxManager _owner;
            readonly Vector2 _size;
            readonly ISpatial _spatial;
            readonly TransBoxType _type;
            readonly ICamera2D _camera;

            Vector2 _position;
            Vector2 _selectPos;
            Vector2 _spatialInitPos;
            Vector2 _spatialInitSize;

            /// <summary>
            /// Initializes a new instance of the <see cref="TransBox"/> class.
            /// </summary>
            /// <param name="owner">The <see cref="TransBoxManager"/>.</param>
            /// <param name="type">The <see cref="TransBoxType"/>.</param>
            /// <param name="spatial">The <see cref="ISpatial"/>.</param>
            TransBox(TransBoxManager owner, TransBoxType type, ISpatial spatial, ICamera2D camera)
            {
                _owner = owner;
                _camera = camera;

                _type = type;
                _spatial = spatial;

                _size = GetTransBoxSize(type);
                _position = GetPosition();
            }

            Vector2 Align(Vector2 v)
            {
                if (_owner == null || _owner.GridAligner == null)
                    return v;

                return _owner.GridAligner.Align(v);
            }

            /// <summary>
            /// Gets the position to use for the <see cref="TransBox"/>.
            /// </summary>
            /// <returns>The position for the <see cref="TransBox"/>.</returns>
            Vector2 GetPosition()
            {
                Vector2 pos = GetPositionNoClamping();
                return Camera.ClampScreenPosition(new Rectangle(pos.X, pos.Y, Size.X, Size.Y));
            }

            /// <summary>
            /// Gets the position, before clamping to screen.
            /// </summary>
            Vector2 GetPositionNoClamping()
            {
                switch (_type)
                {
                    case TransBoxType.Bottom:
                        return new Vector2((_spatial.Center.X ) - ((Size.X ) / 2f), _spatial.Max.Y );

                    case TransBoxType.BottomLeft:
                        return new Vector2((_spatial.Position.X ) - (Size.X ), _spatial.Max.Y );

                    case TransBoxType.BottomRight:
                        return _spatial.Max ;

                    case TransBoxType.Left:
                        return new Vector2((_spatial.Position.X ) - (Size.X  ), (_spatial.Center.Y ) - (Size.Y  / 2f));

                    case TransBoxType.Move:
                        return new Vector2((_spatial.Center.X ) - (Size.X  / 2f),
                            (_spatial.Position.Y ) - (Size.Y ) - (GetTransBoxSize(TransBoxType.Top).Y ));

                    case TransBoxType.Right:
                        return new Vector2(_spatial.Max.X , (_spatial.Center.Y ) - (Size.Y  / 2f));

                    case TransBoxType.Top:
                        return new Vector2((_spatial.Center.X ) - ((Size.X ) / 2f), (_spatial.Position.Y ) - (Size.Y ));

                    case TransBoxType.TopLeft:
                        return (_spatial.Position ) - Size ;

                    case TransBoxType.TopRight:
                        return new Vector2(_spatial.Max.X , (_spatial.Position.Y ) - (Size.Y ));

                    default:
                        const string errmsg = "Unsupported TransBoxType `{0}`.";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, _type);
                        Debug.Fail(string.Format(errmsg, _type));
                        return _spatial.Center;
                }
            }

            static Vector2 GetResizeVector(TransBoxType t)
            {
                switch (t)
                {
                    case TransBoxType.Bottom:
                        return new Vector2(0, 1);

                    case TransBoxType.BottomLeft:
                        return new Vector2(-1, 1);

                    case TransBoxType.BottomRight:
                        return new Vector2(1, 1);

                    case TransBoxType.Left:
                        return new Vector2(-1, 0);

                    case TransBoxType.Right:
                        return new Vector2(1, 0);

                    case TransBoxType.Top:
                        return new Vector2(0, -1);

                    case TransBoxType.TopLeft:
                        return new Vector2(-1, -1);

                    case TransBoxType.TopRight:
                        return new Vector2(1, -1);

                    default:
                        const string errmsg = "Unsupported TransBoxType `{0}`.";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, t);
                        Debug.Fail(string.Format(errmsg, t));
                        return Vector2.Zero;
                }
            }

            /// <summary>
            /// Creates a series of transformation boxes around an entity.
            /// </summary>
            /// <param name="transBoxManager">The <see cref="TransBoxManager"/>.</param>
            /// <param name="entity">Entity to create the transformation boxes for.</param>
            /// <returns></returns>
            public static IEnumerable<ITransBox> SurroundEntity(TransBoxManager transBoxManager, ISpatial entity, ICamera2D camera)
            {
                var ret = new List<ITransBox>(entity.SupportsResize ? 9 : 1);
                var scaleSize = GetTransBoxSize(TransBoxType.BottomLeft);

                // Move box
                ret.Add(new TransBox(transBoxManager, TransBoxType.Move, entity, camera));

                if (entity.SupportsResize)
                {
                    // Four corners
                    ret.Add(new TransBox(transBoxManager, TransBoxType.TopLeft, entity, camera));
                    ret.Add(new TransBox(transBoxManager, TransBoxType.TopRight, entity, camera));
                    ret.Add(new TransBox(transBoxManager, TransBoxType.BottomLeft, entity, camera));
                    ret.Add(new TransBox(transBoxManager, TransBoxType.BottomRight, entity, camera));

                    // Horizontal sides
                    if (entity.Size.X > scaleSize.X + 4)
                    {
                        ret.Add(new TransBox(transBoxManager, TransBoxType.Top, entity, camera));
                        ret.Add(new TransBox(transBoxManager, TransBoxType.Bottom, entity, camera));
                    }

                    // Vertical sides
                    if (entity.Size.Y > scaleSize.Y + 4)
                    {
                        ret.Add(new TransBox(transBoxManager, TransBoxType.Left, entity, camera));
                        ret.Add(new TransBox(transBoxManager, TransBoxType.Right, entity, camera));
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
            /// Gets the camera describing the view area.
            /// </summary>
            public ICamera2D Camera { get { return _camera; } }

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
            /// <param name="cursorWorldPos">The amount the cursor has moved.</param>
            public void CursorMoved(Vector2 cursorWorldPos)
            {
                _position = cursorWorldPos - (Size / 2f);

                var delta = cursorWorldPos - _selectPos;

                // Handle move, which is easy enough
                if (_type == TransBoxType.Move)
                {
                    _spatial.TryMove(Align(_spatialInitPos + delta));
                    return;
                }

                // Handle resizing, which is the harder part
                var resizeVector = GetResizeVector(_type);

                var newSize = _spatialInitSize + (delta * resizeVector);
                newSize = Align(_spatialInitPos + newSize) - _spatialInitPos;

                newSize = Vector2.Max(Vector2.One, newSize);

                var sizeDelta = newSize - _spatialInitSize;

                var moveVector = sizeDelta * Vector2.Min(resizeVector, Vector2.Zero);
                var newPos = _spatialInitPos + moveVector;

                // When resizing with a negative vector (so resizing to the left or up), offset the position so that the
                // right/bottom side remains in the same place. If the resize vector is 0, ensure it does not resize.
                var alignPos = Align(newPos);
                if (resizeVector.X < 0)
                {
                    newSize.X += newPos.X - alignPos.X;
                    newPos.X = alignPos.X;
                }
                else if (resizeVector.X == 0)
                    newSize.X = _spatialInitSize.X;

                if (resizeVector.Y < 0)
                {
                    newSize.Y += newPos.Y - alignPos.Y;
                    newPos.Y = alignPos.Y;
                }
                else if (resizeVector.Y == 0)
                    newSize.Y = _spatialInitSize.Y;

                // Apply the new size and position
                if (!_spatial.TryResize(newSize))
                    return;

                _spatial.TryMove(newPos);
            }

            /// <summary>
            /// Notifies the <see cref="ITransBox"/> that it has been un-selected.
            /// </summary>
            /// <param name="cursorWorldPos">The world position of the cursor.</param>
            void ITransBox.Deselect(Vector2 cursorWorldPos)
            {
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

                Vector2 p = camera.ToScreen(Position).Round();
                Vector2 s = Size.Round();
                Rectangle r = new Rectangle(p.X, p.Y, s.X, s.Y);
                sprite.Draw(spriteBatch, r, Color.Green);
            }

            /// <summary>
            /// Notifies the <see cref="ITransBox"/> that it has been selected.
            /// </summary>
            /// <param name="cursorWorldPos">The world position of the cursor.</param>
            void ITransBox.Select(Vector2 cursorWorldPos)
            {
                _selectPos = cursorWorldPos;
                _spatialInitPos = _spatial.Position;
                _spatialInitSize = _spatial.Size;
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
