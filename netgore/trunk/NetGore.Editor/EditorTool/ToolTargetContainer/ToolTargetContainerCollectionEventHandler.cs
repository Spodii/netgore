using System.Linq;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// Delegate for handling events from the <see cref="ToolTargetContainerCollection"/>.
    /// </summary>
    /// <param name="sender">The <see cref="ToolTargetContainerCollection"/> the event came from.</param>
    /// <param name="c">The <see cref="IToolTargetContainer"/> related to the event.</param>
    public delegate void ToolTargetContainerCollectionEventHandler(ToolTargetContainerCollection sender, IToolTargetContainer c);
}