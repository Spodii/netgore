namespace DemoGame.Editor
{
    /// <summary>
    /// Interface for a <see cref="ToolBase"/>'s control used in a <see cref="ToolBar"/>.
    /// </summary>
    public interface IToolBarControl
    {
        /// <summary>
        /// Gets the <see cref="ToolBase"/> that this control is for.
        /// </summary>
        ToolBase Tool { get; }

        /// <summary>
        /// Gets the <see cref="ToolBarControlType"/> that describes the type of this control.
        /// </summary>
        ToolBarControlType ControlType { get; }
    }
}