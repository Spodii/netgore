using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Editor.Tools
{
    public class TestToolA : ToolBase
    {
        public TestToolA(ToolManager toolManager)
            : base("Test Tool A", toolManager, ToolBarControlType.Label)
        {
        }
    }

    public class TestToolB : ToolBase
    {
        public TestToolB(ToolManager toolManager)
            : base("Test Tool B", toolManager, ToolBarControlType.Button)
        {
        }
    }
}
