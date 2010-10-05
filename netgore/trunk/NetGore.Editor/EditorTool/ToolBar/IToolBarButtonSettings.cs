using System.Linq;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// Contains the settings specific to the <see cref="ToolBarControlType.Button"/>.
    /// </summary>
    public interface IToolBarButtonSettings : IToolBarControlSettings
    {
        /// <summary>
        /// Gets or sets if the <see cref="Tool"/>'s <see cref="NetGore.Editor.EditorTool.Tool.IsEnabled"/> state
        /// will be toggle by clicking this control.
        /// </summary>
        bool ClickToEnable { set; get; }
    }
}