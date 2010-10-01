using System.Linq;

namespace DemoGame.Editor
{
    public class TestToolA : Tool
    {
        public TestToolA(ToolManager toolManager)
            : base("Global1", toolManager, ToolBarControlType.Button, ToolBarVisibility.Global)
        {
        }
    }

    public class MapGridTool : Tool
    {
        public MapGridTool(ToolManager toolManager)
            : base("Map Grid", toolManager, ToolBarControlType.Button, ToolBarVisibility.Map)
        {
        }
    }
}