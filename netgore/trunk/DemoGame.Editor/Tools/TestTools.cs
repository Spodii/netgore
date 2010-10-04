using System.Linq;
using NetGore.EditorTools;

namespace DemoGame.Editor
{
    public class TestToolA : Tool
    {
        public TestToolA(ToolManager toolManager)
            : base(toolManager, "A", ToolBarControlType.Button, ToolBarVisibility.Global)
        {
        }
    }
    public class TestToolB : Tool
    {
        public TestToolB(ToolManager toolManager)
            : base(toolManager, "B", ToolBarControlType.Button, ToolBarVisibility.Global)
        {
        }
    }
    public class TestToolC : Tool
    {
        public TestToolC(ToolManager toolManager)
            : base(toolManager, "C", ToolBarControlType.Button, ToolBarVisibility.Global)
        {
        }
    }
    public class TestToolD : Tool
    {
        public TestToolD(ToolManager toolManager)
            : base(toolManager, "D", ToolBarControlType.Button, ToolBarVisibility.Global)
        {
        }
    }
}