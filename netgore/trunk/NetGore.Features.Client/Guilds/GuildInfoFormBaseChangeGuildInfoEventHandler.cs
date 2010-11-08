namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Delegate for handling when the <see cref="UserGuildInformation"/> changes.
    /// </summary>
    /// <param name="sender">The <see cref="GuildInfoFormBase"/> that the event came from.</param>
    /// <param name="newValue">The old <see cref="UserGuildInformation"/> value.</param>
    /// <param name="oldValue">The new <see cref="UserGuildInformation"/> value.</param>
    public delegate void GuildInfoFormBaseChangeGuildInfoEventHandler(
        GuildInfoFormBase sender, UserGuildInformation newValue, UserGuildInformation oldValue);
}