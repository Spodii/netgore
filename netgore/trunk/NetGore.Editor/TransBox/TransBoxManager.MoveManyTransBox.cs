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
        /// A <see cref="ITransBox"/> for moving multiple <see cref="ISpatial"/>s at once.
        /// </summary>
        sealed class MoveManyTransBox : ITransBox
        {
            static readonly Vector2 _size = GetTransBoxSize(TransBoxType.Move);

            readonly Dictionary<ISpatial, Vector2> _initialPos = new Dictionary<ISpatial, Vector2>();
            readonly TransBoxManager _owner;
            readonly ICamera2D _camera;
            readonly IEnumerable<ISpatial> _spatials;

            Vector2 _position;
            Vector2 _selectPos;

            /// <summary>
            /// Initializes a new instance of the <see cref="MoveManyTransBox"/> class.
            /// </summary>
            /// <param name="owner">The <see cref="TransBoxManager"/>.</param>
            /// <param name="spatials">The <see cref="ISpatial"/>s.</param>
            /// <param name="position">The position.</param>
            /// <param name="camera">The camera.</param>
            public MoveManyTransBox(TransBoxManager owner, IEnumerable<ISpatial> spatials, Vector2 position, ICamera2D camera)
            {
                _owner = owner;
                _camera = camera;
                _position = position;
                _spatials = spatials.ToImmutable();
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
                get { return Cursors.SizeAll; }
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
            /// <param name="cursorWorldPos">The current world position of the cursor.</param>
            public void CursorMoved(Vector2 cursorWorldPos)
            {
                _position = cursorWorldPos - (Size / 2f);

                var delta = cursorWorldPos - _selectPos;

                foreach (var s in _spatials)
                {
                    Vector2 initPos;
                    if (!_initialPos.TryGetValue(s, out initPos))
                    {
                        const string errmsg = "Couldn't find initial position for spatial `{0}`.";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, s);
                        Debug.Fail(string.Format(errmsg, s));
                        continue;
                    }

                    var newPos = initPos + delta;
                    if (_owner != null && _owner.GridAligner != null)
                        newPos = _owner.GridAligner.Align(newPos);

                    s.TryMove(newPos);
                }
            }

            /// <summary>
            /// Notifies the <see cref="ITransBox"/> that it has been un-selected.
            /// </summary>
            /// <param name="cursorWorldPos">The world position of the cursor.</param>
            void ITransBox.Deselect(Vector2 cursorWorldPos)
            {
                _initialPos.Clear();
            }

            /// <summary>
            /// Draws the <see cref="ITransBox"/>.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
            /// <param name="camera">The <see cref="ICamera2D"/>.</param>
            public void Draw(ISpriteBatch spriteBatch, ICamera2D camera)
            {
                Vector2 p = camera.ToScreen(Position).Round();
                Vector2 s = Size.Round();
                Rectangle r = new Rectangle(p.X, p.Y, s.X, s.Y);
                SystemSprites.Move.Draw(spriteBatch, r, Color.White);
            }

            /// <summary>
            /// Notifies the <see cref="ITransBox"/> that it has been selected.
            /// </summary>
            /// <param name="cursorWorldPos">The world position of the cursor.</param>
            void ITransBox.Select(Vector2 cursorWorldPos)
            {
                _selectPos = cursorWorldPos;

                foreach (var s in _spatials)
                {
                    _initialPos.Add(s, s.Position);
                }
            }

            /// <summary>
            /// Updates the <see cref="ITransBox"/>.
            /// </summary>
            /// <param name="currentTime">The current time.</param>
            public void Update(TickCount currentTime)
            {
            }

            #endregion
        }
    }
}