using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Contains the flags of the different permissions a user can have.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags]
    public enum UserPermissions : byte
    {
        /* The actual values of the permission levels do not matter. The default should be 0, but that is about the
         * only restriction. This is because comparisons should not use decimal comparison operators (<, >, ==, etc)
         * and should instead use binary operators to check for flags being set.
         * 
         * Some permissions expand on others, such as an MinorAdmin is still a Moderator. See the example permissions
         * to see how to handle cases like these.
         * 
         * Permission flags don't all have to be GOOD things. For example, a permission level can be created for
         * users who repeatedly cause minor problems (harassment, swearing, etc). You can then check if a user has
         * this permission flag set and, if they do, reject them from performing certain actions.
         */

        /// <summary>
        /// Moderation permissions. Includes very basic user moderation control such as banning, kicking, and muting.
        /// </summary>
        Moderator = 1 << 0,

        /// <summary>
        /// Lesser administration permissions. Includes all permissions of a <see cref="Moderator"/>, plus administration
        /// actions that cannot be so easily abused and are more oriented to resolving issues with players rather
        /// than with the game itself.
        /// </summary>
        LesserAdmin = (1 << 1) | Moderator,

        /// <summary>
        /// Administration permissions. Includes all permissions of a <see cref="LesserAdmin"/>, plus most administration
        /// actions. This permission level has access to most all commands to control aspects of the game.
        /// </summary>
        Admin = (1 << 2) | LesserAdmin,

        /// <summary>
        /// Ultimate permissions. Includes all permissions of a <see cref="Admin"/>, plus permissions to any other
        /// commands. This is generally reserved for users who need to control the server directly, such as shutting down
        /// or restarting the server.
        /// </summary>
        Owner = (1 << 3) | Admin,

        /// <summary>
        /// No special permissions. This is the default value.
        /// </summary>
        None = 0,
    }
}