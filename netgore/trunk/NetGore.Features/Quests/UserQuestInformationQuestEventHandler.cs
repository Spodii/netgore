namespace NetGore.Features.Quests
{
    /// <summary>
    /// Delegate for handling events from the <see cref="UserQuestInformation"/>.
    /// </summary>
    /// <param name="sender">The <see cref="UserQuestInformation"/> the event came from.</param>
    /// <param name="quest">The <see cref="QuestID"/> for the quest related to the event.</param>
    public delegate void UserQuestInformationQuestEventHandler(UserQuestInformation sender, QuestID quest);
}