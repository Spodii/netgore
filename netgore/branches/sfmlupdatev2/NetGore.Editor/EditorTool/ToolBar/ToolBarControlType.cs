using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// The different types of controls that a <see cref="Tool"/> can display itself as in a <see cref="ToolBar"/>.
    /// </summary>
    public enum ToolBarControlType : byte
    {
        /// <summary>
        /// No control type.
        /// </summary>
        None = 0,

        /// <summary>
        /// A <see cref="ToolBar"/> item that uses <see cref="ToolStripButton"/>.
        /// </summary>
        Button,

        /// <summary>
        /// A <see cref="ToolBar"/> item that uses <see cref="ToolStripLabel"/>.
        /// </summary>
        Label,

        /// <summary>
        /// A <see cref="ToolBar"/> item that uses <see cref="ToolStripSplitButton"/>.
        /// </summary>
        SplitButton,

        /// <summary>
        /// A <see cref="ToolBar"/> item that uses <see cref="ToolStripDropDownButton"/>.
        /// </summary>
        DropDownButton,

        /// <summary>
        /// A <see cref="ToolBar"/> item that uses <see cref="ToolStripComboBox"/>.
        /// </summary>
        ComboBox,

        /// <summary>
        /// A <see cref="ToolBar"/> item that uses <see cref="ToolStripTextBox"/>.
        /// </summary>
        TextBox,

        /// <summary>
        /// A <see cref="ToolBar"/> item that uses <see cref="ToolStripProgressBar"/>.
        /// </summary>
        ProgressBar
    }
}