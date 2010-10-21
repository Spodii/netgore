namespace NetGore.IO
{
    /// <summary>
    /// Delegate for handling events from the <see cref="IRuntimeSettings"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IRuntimeSettings"/> the event came from.</param>
    public delegate void RuntimeSettingsEventHandler(IRuntimeSettings sender);
}