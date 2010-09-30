using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Editor
{
    public class TestToolA : ToolBase
    {
        public TestToolA(ToolManager toolManager)
            : base("Test Tool A", toolManager, ToolBarControlType.Label)
        {
        }
    }
}
