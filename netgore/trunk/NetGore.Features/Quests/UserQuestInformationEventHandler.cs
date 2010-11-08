using System.Linq;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Delegate for handling events from the <see cref="UserQuestInformation"/>.
    /// </summary>
    /// <param name="sender">The <see cref="UserQuestInformation"/> the event came from.</param>
    public delegate void UserQuestInformationEventHandler(UserQuestInformation sender);
}