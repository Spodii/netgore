using System.Windows.Forms;
using NetGore.Editor.EditorTool;
using SFML.Graphics;

namespace DemoGame.Editor
{
    public class MapWallCursorTool : MapCursorToolBase
    {
        // TODO: !! ...

        public MapWallCursorTool(ToolManager toolManager, ToolSettings settings, bool canSelect, bool canSelectArea) : base(toolManager, settings, canSelect, canSelectArea)
        {
        }
    }
}