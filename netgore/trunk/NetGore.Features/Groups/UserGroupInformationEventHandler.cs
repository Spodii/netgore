using System.Linq;

namespace NetGore.Features.Groups
{
    /// <summary>
    /// Delegate for handling events from the <see cref="UserGroupInformation"/>.
    /// </summary>
    /// <param name="sender">The <see cref="UserGroupInformation"/> the event came from.</param>
    public delegate void UserGroupInformationEventHandler(UserGroupInformation sender);

    /// <summary>
    /// Delegate for handling events from the <see cref="UserGroupInformation"/>.
    /// </summary>
    /// <param name="sender">The <see cref="UserGroupInformation"/> the event came from.</param>
    /// <param name="arg">The argument related to the event.</param>
    public delegate void UserGroupInformationEventHandler<in T>(UserGroupInformation sender, T arg);
}