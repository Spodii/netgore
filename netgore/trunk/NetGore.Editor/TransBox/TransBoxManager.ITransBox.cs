using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics;
using SFML.Graphics;

namespace NetGore.Editor
{
    public partial class TransBoxManager
    {
        /// <summary>
        /// Interface for a transformation box in the <see cref="TransBoxManager"/>.
        /// </summary>
        interface ITransBox
        {
            /// <summary>
            /// Gets the max (bottom-right) point of the <see cref="ITransBox"/>.
            /// </summary>
            Vector2 Max { get; }

            /// <summary>
            /// Gets the <see cref="Cursor"/> to display when this <see cref="ITransBox"/> is selected or the mouse is over it.
            /// </summary>
            Cursor MouseCursor { get; }

            /// <summary>
            /// Gets the position of the <see cref="ITransBox"/>.
            /// </summary>
            Vector2 Position { get; }

            /// <summary>
            /// Gets the size of the <see cref="ITransBox"/>.
            /// </summary>
            Vector2 Size { get; }

            /// <summary>
            /// Gets the camera describing the view area.
            /// </summary>
            ICamera2D Camera { get; }

            /// <summary>
            /// Checks if this <see cref="ITransBox"/> contains the given world point.
            /// </summary>
            /// <param name="worldPos">The world point.</param>
            /// <returns>True if this <see cref="ITransBox"/> contains the <paramref name="worldPos"/>; otherwise false.</returns>
            bool ContainsPoint(Vector2 worldPos);

            /// <summary>
            /// Handles when the mouse cursor moves while this <see cref="ITransBox"/> is selected.
            /// </summary>
            /// <param name="cursorWorldPos">The current world position of the cursor.</param>
            void CursorMoved(Vector2 cursorWorldPos);

            /// <summary>
            /// Notifies the <see cref="ITransBox"/> that it has been un-selected.
            /// </summary>
            /// <param name="cursorWorldPos">The world position of the cursor.</param>
            void Deselect(Vector2 cursorWorldPos);

            /// <summary>
            /// Draws the <see cref="ITransBox"/>.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
            /// <param name="camera">The <see cref="ICamera2D"/>.</param>
            void Draw(ISpriteBatch spriteBatch, ICamera2D camera);

            /// <summary>
            /// Notifies the <see cref="ITransBox"/> that it has been selected.
            /// </summary>
            /// <param name="cursorWorldPos">The world position of the cursor.</param>
            void Select(Vector2 cursorWorldPos);

            /// <summary>
            /// Updates the <see cref="ITransBox"/>.
            /// </summary>
            /// <param name="currentTime">The current time.</param>
            void Update(TickCount currentTime);
        }
    }
}