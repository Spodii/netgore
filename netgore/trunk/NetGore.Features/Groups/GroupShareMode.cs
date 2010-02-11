using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Features.Groups
{
    /// <summary>
    /// Describes the method used to share rewards between the members of a group.
    /// </summary>
    public enum GroupShareMode
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
