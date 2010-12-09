using System;
using System.Linq;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// <see cref="EventArgs"/> for an event for an <see cref="IGuild"/> when the name or tag changes.
    /// </summary>
    public class GuildRenameEventArgs : EventArgs
    {
        readonly IGuildMember _invoker;
        readonly string _newName;
        readonly string _oldName;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildRenameEventArgs"/> class.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <param name="oldName">The old name or tag for the <see cref="IGuild"/>.</param>
        /// <param name="newName">The new name or tag for the <see cref="IGuild"/>.</param>
        public GuildRenameEventArgs(IGuildMember invoker, string oldName, string newName)
        {
            _invoker = invoker;
            _oldName = oldName;
            _newName = newName;
        }

        /// <summary>
        /// Gets the guild member that invoked the event.
        /// </summary>
        public IGuildMember Invoker
        {
            get { return _invoker; }
        }

        /// <summary>
        /// Gets the new name or tag for the <see cref="IGuild"/>.
        /// </summary>
        public string NewName
        {
            get { return _newName; }
        }

        /// <summary>
        /// Gets the old name or tag for the <see cref="IGuild"/>.
        /// </summary>
        public string OldName
        {
            get { return _oldName; }
        }
    }
}