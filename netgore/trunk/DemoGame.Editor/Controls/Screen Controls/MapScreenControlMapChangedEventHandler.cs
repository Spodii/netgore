namespace DemoGame.Editor
{
    /// <summary>
    /// Delegate for handling events from the <see cref="MapScreenControl"/>.
    /// </summary>
    /// <param name="sender">The <see cref="MapScreenControl"/> the event came from.</param>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    public delegate void MapScreenControlMapChangedEventHandler(MapScreenControl sender, EditorMap oldValue, EditorMap newValue);
}