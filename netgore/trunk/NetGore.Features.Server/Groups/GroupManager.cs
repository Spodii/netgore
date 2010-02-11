﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Features.Groups
{
    public interface IGroupManager
    {
        /// <summary>
        /// Gets all of the active <see cref="IGroup"/>s managed by this <see cref="IGroupManager"/>.
        /// </summary>
        IEnumerable<IGroup> Groups { get; }

        /// <summary>
        /// Creates a new <see cref="IGroup"/>.
        /// </summary>
        /// <param name="founder">The <see cref="IGroupable"/> that will be the founder of the group.</param>
        /// <returns>If the group was successfully created, returns the new <see cref="IGroup"/> with the
        /// <paramref name="founder"/> set as the group's founder. Otherwise, returns null.
        /// A group may not be created by someone who is already in a group.</returns>
        IGroup TryCreateGroup(IGroupable founder);

        /// <summary>
        /// Notifies listeners when a new group has been created.
        /// </summary>
        event IGroupManagerGroupEventHandler GroupCreated;
    }

    public delegate void IGroupManagerEventHandler(IGroupManager groupManager);
    public delegate void IGroupManagerGroupEventHandler(IGroupManager groupManager, IGroup group);

    public class GroupManager : IGroupManager
    {
        readonly List<IGroup> _groups;
        readonly Func<IGroupable, IGroup, bool> _canJoinGroup;
        readonly Func<IGroupManager, IGroupable, IGroup> _tryCreateGroup;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupManager"/> class.
        /// </summary>
        /// <param name="canJoinGroup">A <see cref="Func{T,U,V}"/> that is used to determine if a
        /// <see cref="IGroupable"/> can join an <see cref="IGroup"/>.</param>
        /// <param name="tryCreateGroup">A <see cref="Func{T,U,V}"/> that is used to create a group started by
        /// an <see cref="IGroupable"/>.</param>
        public GroupManager(Func<IGroupable, IGroup, bool> canJoinGroup, Func<IGroupManager, IGroupable, IGroup> tryCreateGroup)
        {
            _canJoinGroup = canJoinGroup;
            _tryCreateGroup = tryCreateGroup;
        }

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

            // Add the new group to the list
            _groups.Add(newGroup);

            // Raise events
            OnCreateGroup(newGroup);

            if (GroupCreated != null)
                GroupCreated(this, newGroup);

            return newGroup;
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling of the
        /// <see cref="IGroupManager.GroupCreated"/> event.
        /// </summary>
        /// <param name="group">The <see cref="IGroup"/> that was created.</param>
        protected virtual void OnCreateGroup(IGroup group)
        {
        }

        /// <summary>
        /// Notifies listeners when a new group has been created.
        /// </summary>
        public event IGroupManagerGroupEventHandler GroupCreated;
    }
}
