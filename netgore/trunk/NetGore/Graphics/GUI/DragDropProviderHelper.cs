using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Helper methods for using drag-and-drop.
    /// </summary>
    public static class DragDropProviderHelper
    {
        /// <summary>
        /// Initializes the <see cref="DragDropProviderHelper"/> class.
        /// </summary>
        static DragDropProviderHelper()
        {
            HighlightInnerColor = new Color(0, 0, 0, 0);
            HighlightOuterColor = new Color(0, 255, 0, 150);
            HighlightOuterColor = Color.White;
            HighlightBorderThickness = 2;
        }

        /// <summary>
        /// Gets or sets the thickness of the highlighting border.
        /// </summary>
        public static int HighlightBorderThickness { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Color"/> to use for inside of a highlighted area. Usually is an invisible color.
        /// </summary>
        public static Color HighlightInnerColor { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Color"/> to use for outside of a highlighted area.
        /// </summary>
        public static Color HighlightOuterColor { get; set; }

        /// <summary>
        /// Draws a highlighted rectangle around the target area.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="area">The area to draw the highlight at.</param>
        public static void DrawDropHighlight(ISpriteBatch spriteBatch, Rectangle area)
        {
            RenderRectangle.Draw(spriteBatch, area, HighlightInnerColor, HighlightOuterColor, HighlightBorderThickness);
        }
    }
}