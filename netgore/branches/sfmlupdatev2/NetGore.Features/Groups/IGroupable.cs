using System;
using System.Linq;

namespace NetGore.Features.Groups
{
    /// <summary>
    /// Interface for something that can be part of a group.
    /// </summary>
    public interface IGroupable : IDisposable
    {
        /// <summary>
        /// Gets or sets the group that this <see cref="IGroupable"/> is currently part of. This property should only be
        /// set by the <see cref="IGroup"/> and <see cref="IGroupable"/>, and should never be used to try and add or
        /// remove a <see cref="IGroupable"/> from a <see cref="IGroup"/>.
        /// </summary>
        IGroup Group { get; set; }

        /// <summary>
        /// Gets the unique name of this group member.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets if this <see cref="IGroupable"/> is close enough to the <paramref name="other"/> to
        /// share group-based rewards with them. This method should return the same value for two
        /// <see cref="IGroupable"/>s, despite which one is used as the caller and which is used as
        /// the parameter.
        /// </summary>
        /// <param name="other">The <see cref="IGroupable"/> to check if within distance of.</param>
        /// <returns>True if the <paramref name="other"/> is close enough to this <see cref="IGroupable"/>
        /// to share group-based rewards with them.</returns>
        bool IsInShareDistance(IGroupable other);

        /// <summary>
        /// Notifies this <see cref="IGroupable"/> that they have been invited to join another group. This should only
        /// be called by the <see cref="IGroup"/>.
        /// </summary>
        /// <param name="group">The <see cref="IGroup"/> that this <see cref="IGroupable"/> was invited to join.</param>
        void NotifyInvited(IGroup group);
    }
}