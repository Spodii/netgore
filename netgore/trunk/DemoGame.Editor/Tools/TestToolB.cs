namespace DemoGame.Editor
{
    public class TestToolB : Tool
    {
        public TestToolB(ToolManager toolManager)
            : base("Test Tool B", toolManager, ToolBarControlType.Button, ToolBarVisibility.Map)
        {
        }
    }
}