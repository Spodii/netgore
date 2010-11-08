using System.Linq;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// Delegate for handling events from the <see cref="Tool"/>.
    /// </summary>
    /// <param name="sender">The <see cref="Tool"/> the event came from.</param>
    public delegate void ToolEventHandler(Tool sender);
}