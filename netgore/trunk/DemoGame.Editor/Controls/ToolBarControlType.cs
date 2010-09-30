namespace DemoGame.Editor
{
    /// <summary>
    /// The different types of controls that a <see cref="ToolBase"/> can display itself as in a <see cref="ToolBar"/>.
    /// </summary>
    public enum ToolBarControlType : byte
    {
        None = 0,
        Button,
        Label,
        SplitButton,
        DropDownButton,
        ComboBox,
        TextBox,
        ProgressBar
    }
}