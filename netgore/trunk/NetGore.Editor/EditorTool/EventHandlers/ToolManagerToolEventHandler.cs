namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// Delegate for handling events from the <see cref="ToolManager"/>.
    /// </summary>
    /// <param name="sender">The <see cref="ToolManager"/> the event came from.</param>
    /// <param name="tool">The <see cref="Tool"/> that the event relates to.</param>
    public delegate void ToolManagerToolEventHandler(ToolManager sender, Tool tool);
}