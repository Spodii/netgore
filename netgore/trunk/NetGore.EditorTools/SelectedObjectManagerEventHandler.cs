using System.Linq;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Handles events from the <see cref="SelectedObjectsManager{T}"/>.
    /// </summary>
    /// <param name="sender">The <see cref="SelectedObjectsManager{T}"/> the event came from.</param>
    /// <param name="e">The event argument.</param>
    public delegate void SelectedObjectManagerEventHandler<T, in U>(SelectedObjectsManager<T> sender, U e) where T : class;

    /// <summary>
    /// Handles events from the <see cref="SelectedObjectsManager{T}"/>.
    /// </summary>
    /// <param name="sender">The <see cref="SelectedObjectsManager{T}"/> the event came from.</param>
    public delegate void SelectedObjectManagerEventHandler<T>(SelectedObjectsManager<T> sender) where T : class;
}