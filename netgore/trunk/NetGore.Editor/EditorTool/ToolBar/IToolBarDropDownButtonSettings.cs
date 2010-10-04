using System.Linq;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// Contains the settings specific to the <see cref="ToolBarControlType.DropDownButton"/>.
    /// </summary>
    public interface IToolBarDropDownButtonSettings : IToolBarDropDownItemSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use the Text property or the
        /// <see cref="System.Windows.Forms.ToolStripItem.ToolTipText"/> property for the
        /// <see cref="System.Windows.Forms.ToolStripDropDownButton"/> ToolTip.
        /// </summary>
        bool AutoToolTip { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether an arrow is displayed on the
        /// <see cref="System.Windows.Forms.ToolStripDropDownButton"/>,
        /// which indicates that further options are available in a drop-down list.
        /// </summary>
        bool ShowDropDownArrow { set; get; }
    }
}