using System.Linq;

namespace NetGore.Features.Groups
{
    /// <summary>
    /// Describes the method used to share rewards between the members of a group.
    /// </summary>
    public enum GroupShareMode : byte
    {
        /// <summary>
        /// No sharing takes place. All rewards are distributed as normal.
        /// </summary>
        NoSharing,

        /// <summary>
        /// Rewards are shared equally among all members in range.
        /// </summary>
        EqualShare
    }
}