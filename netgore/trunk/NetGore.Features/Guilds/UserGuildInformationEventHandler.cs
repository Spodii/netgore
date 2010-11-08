namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Delegate for handling events from the <see cref="UserGuildInformation"/>.
    /// </summary>
    /// <param name="sender">The <see cref="UserGuildInformation"/> the event came from.</param>
    /// <param name="member">The guild member the event is related to.</param>
    public delegate void UserGuildInformationEventHandler<in T>(UserGuildInformation sender, T member);

    /// <summary>
    /// Delegate for handling events from the <see cref="UserGuildInformation"/>.
    /// </summary>
    /// <param name="sender">The <see cref="UserGuildInformation"/> the event came from.</param>
    public delegate void UserGuildInformationEventHandler(UserGuildInformation sender);
}