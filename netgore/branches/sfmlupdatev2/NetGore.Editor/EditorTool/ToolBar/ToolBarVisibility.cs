using System.Linq;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// Describes the different places that a <see cref="Tool"/> is visible in a <see cref="ToolBar"/>.
    /// </summary>
    public enum ToolBarVisibility : byte
    {
        /// <summary>
        /// Not visible from on any <see cref="ToolBar"/>.
        /// </summary>
        None,

        /// <summary>
        /// The <see cref="Tool"/> is visible from every part of the editor.
        /// </summary>
        Global,

        /// <summary>
        /// The <see cref="Tool"/> is only displayed when editing maps.
        /// </summary>
        Map,
    }
}