using System.Collections.Generic;
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

            readonly IEnumerable<ISpatial> _spatials;

            Vector2 _position;

            /// <summary>
            /// Initializes a new instance of the <see cref="MoveManyTransBox"/> class.
            /// </summary>
            /// <param name="spatials">The <see cref="ISpatial"/>s.</param>
            /// <param name="position">The position.</param>
            public MoveManyTransBox(IEnumerable<ISpatial> spatials, Vector2 position)
            {
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
                foreach (var s in _spatials)
                {
                    s.TryMove(s.Position + offset);
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
                var p = camera.ToScreen(Position).Round();
                var s = Size.Round();
                var r = new Rectangle((int)p.X, (int)p.Y, (int)s.X, (int)s.Y);
                SystemSprites.Move.Draw(spriteBatch, r, Color.White);
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