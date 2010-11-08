using System.Linq;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// Delegate for handling events from the <see cref="Tool"/>.
    /// </summary>
    /// <param name="sender">The <see cref="Tool"/> the event came from.</param>
    /// <param name="oldValue">The old (previous) value.</param>
    /// <param name="newValue">The new (current) value.</param>
    public delegate void ToolValueChangedEventHandler<in T>(Tool sender, T oldValue, T newValue);
}