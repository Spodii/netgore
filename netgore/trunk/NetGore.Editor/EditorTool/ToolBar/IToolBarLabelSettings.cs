using System.Linq;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// Contains the settings specific to the <see cref="ToolBarControlType.Label"/>.
    /// </summary>
    public interface IToolBarLabelSettings : IToolBarControlSettings
    {
        /// <summary>
        /// Gets or sets if the <see cref="Tool"/>'s <see cref="NetGore.Editor.EditorTool.Tool.IsEnabled"/> state
        /// will be toggle by clicking this control. Some types of controls may ignore this value when it makes no logical
        /// sense to behave this way.
        /// </summary>
        bool ClickToEnable { set; get; }
    }
}