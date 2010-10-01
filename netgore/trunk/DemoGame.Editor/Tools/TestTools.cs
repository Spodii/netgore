using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Editor
{
    public class TestToolA : Tool
    {
        public TestToolA(ToolManager toolManager)
            : base("Global1", toolManager, ToolBarControlType.Button, ToolBarVisibility.Global)
        {
        }
    }

    public class TestToolB : Tool
    {
        public TestToolB(ToolManager toolManager)
            : base("Global2", toolManager, ToolBarControlType.Button, ToolBarVisibility.Global)
        {
        }
    }

    public class TestToolC : Tool
    {
        public TestToolC(ToolManager toolManager)
            : base("Map1", toolManager, ToolBarControlType.Button, ToolBarVisibility.Map)
        {
        }
    }

    public class TestToolD : Tool
    {
        public TestToolD(ToolManager toolManager)
            : base("Map2", toolManager, ToolBarControlType.Button, ToolBarVisibility.Map)
        {
        }
    }
}
