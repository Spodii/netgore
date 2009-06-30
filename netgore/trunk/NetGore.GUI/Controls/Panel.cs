using Microsoft.Xna.Framework;

namespace NetGore.Graphics.GUI
{
    public class Panel : Control
    {
        public Panel(GUIManagerBase gui, PanelSettings settings, Vector2 position, Vector2 size, Control parent)
            : base(gui, settings, position, size, parent)
        {
            CanDrag = false;
        }

        public Panel(Vector2 position, PanelSettings settings, Vector2 size, Control parent)
            : this(parent.GUIManager, settings, position, size, parent)
        {
        }

        public Panel(Vector2 position, Vector2 size, Control parent)
            : this(position, parent.GUIManager.PanelSettings, size, parent)
        {
        }

        public Panel(GUIManagerBase gui, Vector2 position, Vector2 size) : this(gui, gui.PanelSettings, position, size, null)
        {
        }
    }
}