using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    public static class DragDropProviderHelper
    {
        public static void DrawDropHighlight(ISpriteBatch spriteBatch, Rectangle area)
        {
            RenderRectangle.Draw(spriteBatch, area, new Color(0, 0, 0, 0), new Color(0, 255, 0, 150), 4);
        }
    }
}