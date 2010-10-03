using System.Linq;

namespace DemoGame.Editor
{
    public class TestToolA : Tool
    {
        public TestToolA(ToolManager toolManager)
            : base(toolManager, "Global1", ToolBarControlType.Button, ToolBarVisibility.Global)
        {
        }
    }
}