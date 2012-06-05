using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.Features.Groups
{
    /// <summary>
    /// Manages all of the <see cref="IGroup"/>s.
    /// </summary>
    public class GroupManager : IGroupManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly TypedEventHandler<IGroup> _groupDisbandHandler;
        readonly List<IGroup> _groups = new List<IGroup>();
        readonly Func<IGroupManager, IGroupable, IGroup> _tryCreateGroup;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupManager"/> class.
        /// </summary>
        /// <param name="tryCreateGroup">A func that is used to create a group started by
        /// an <see cref="IGroupable"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="tryCreateGroup"/> is null.</exception>
        public GroupManager(Func<IGroupManager, IGroupable, IGroup> tryCreateGroup)
        {
            if (tryCreateGroup == null)
                throw new ArgumentNullException("tryCreateGroup");

            _groupDisbandHandler = Group_Disbanded;
            _tryCreateGroup = tryCreateGroup;
        }

        /// <summary>
        /// Handles when a <see cref="IGroup"/> in this <see cref="GroupManager"/> is disbanded.
        /// </summary>
        /// <param name="sender">The group that was disbanded.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void Group_Disbanded(IGroup sender, EventArgs e)
        {
            sender.Disbanded -= _groupDisbandHandler;
            if (!_groups.Remove(sender))
            {
                const string errmsg =
                    "Tried to remove disbanded group `{0}` from group manager `{1}`, but it was not in the _groups list!";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, sender, this);
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling of the
        /// <see cref="IGroupManager.GroupCreated"/> event.
        /// </summary>
        /// <param name="group">The <see cref="IGroup"/> that was created.</param>
        protected virtual void OnCreateGroup(IGroup group)
        {
        }

        #region IGroupManager Members

        /// <summary>
        /// Notifies listeners when a new group has been created.
        /// </summary>
        public event TypedEventHandler<IGroupManager, EventArgs<IGroup>> GroupCreated;

        /// <summary>
        /// Gets all of the active <see cref="IGroup"/>s managed by this <see cref="IGroupManager"/>.
        /// </summary>
        public IEnumerable<IGroup> Groups
        {
            get { return _groups; }
        }

        /// <summary>
        /// Creates a new <see cref="IGroup"/>.
        /// </summary>
        /// <param name="founder">The <see cref="IGroupable"/> that will be the founder of the group.</param>
        /// <returns>If the group was successfully created, returns the new <see cref="IGroup"/> with the
        /// <paramref name="founder"/> set as the group's founder. Otherwise, returns null.
        /// A group may not be created by someone who is already in a group.</returns>
        public IGroup TryCreateGroup(IGroupable founder)
        {
            // Make sure not already in a group
            if (founder.Group != null)
                return null;

            // Create the group
            var newGroup = _tryCreateGroup(this, founder);
            if (newGroup == null)
                return null;

            // Make sure the founder had their group property updated
            founder.Group = newGroup;

            // Add the new group to the list
            _groups.Add(newGroup);

            // Listen for when the group is disbanded so we can remove it
            newGroup.Disbanded += _groupDisbandHandler;

            // Raise events
            OnCreateGroup(newGroup);

            if (GroupCreated != null)
                GroupCreated.Raise(this, EventArgsHelper.Create(newGroup));

            return newGroup;
        }

        #endregion
    }
}