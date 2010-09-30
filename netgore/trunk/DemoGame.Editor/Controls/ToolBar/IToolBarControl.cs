namespace DemoGame.Editor
{
    /// <summary>
    /// Interface for a <see cref="Editor.Tool"/>'s control used in a <see cref="ToolBar"/>.
    /// </summary>
    public interface IToolBarControl
    {
        /// <summary>
        /// Gets the <see cref="Editor.Tool"/> that this control is for.
        /// </summary>
        Tool Tool { get; }

        /// <summary>
        /// Gets the <see cref="IToolBarControlSettings"/> for this control. Can be safely up-casted to the appropriate
        /// interface for a more specific type using the <see cref="IToolBarControl.ControlType"/> property.
        /// </summary>
        /// <example>
        /// When <see cref="IToolBarControl.ControlType"/> equals <see cref="ToolBarControlType.Button"/>, the
        /// <see cref="IToolBarControl.ControlSettings"/> can be up-casted to <see cref="IToolBarButtonSettings"/>.
        /// </example>
        IToolBarControlSettings ControlSettings { get; }

        /// <summary>
        /// Gets if this control is currently on a <see cref="ToolBar"/>.
        /// </summary>
        bool IsOnToolBar { get; }

        /// <summary>
        /// Gets the <see cref="ToolBarControlType"/> that describes the type of this control.
        /// </summary>
        ToolBarControlType ControlType { get; }
    }
}