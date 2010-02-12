using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;

namespace NetGore.Features.Groups
{
    public class UserGroupInformation
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly StringComparer _membersListComparer = StringComparer.OrdinalIgnoreCase;

        readonly List<string> _members = new List<string>();

        string _founder;

        /// <summary>
        /// Gets the name of the group's founder, or null if the user is not in a group.
        /// </summary>
        public string GroupFounder
        {
            get { return _founder; }
        }

        /// <summary>
        /// Gets if the user is in a group.
        /// </summary>
        public bool IsInGroup
        {
            get { return _founder != null; }
        }

        /// <summary>
        /// Gets the members in the group. If the user is not in a group, this will be empty.
        /// </summary>
        public IEnumerable<string> Members
        {
            get { return _members; }
        }

        /// <summary>
        /// Reads the data from the server related to the user group information. This should only be used by the client.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> containing the data.</param>
        public void Read(BitStream bs)
        {
            var id = bs.ReadEnum<GroupInfoMessages>();
            switch (id)
            {
                case GroupInfoMessages.AddMember:
                    ReadAddMember(bs);
                    break;

                case GroupInfoMessages.RemoveMember:
                    ReadRemoveMember(bs);
                    break;

                case GroupInfoMessages.SetGroup:
                    ReadSetGroup(bs);
                    break;

                default:
                    const string errmsg = "Unknown GroupInfoMessages value `{0}`. Could not parse!";
                    string err = string.Format(errmsg, id);
                    log.Fatal(err);
                    Debug.Fail(err);
                    return;
            }
        }

        /// <summary>
        /// Handles <see cref="GroupInfoMessages.AddMember"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to read from.</param>
        void ReadAddMember(BitStream bs)
        {
            string name = bs.ReadString();

            if (!_members.Contains(name, _membersListComparer))
                _members.Add(name);
            else
            {
                const string errmsg = "Tried to add `{0}` to the group member list, but they were already in the list!";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, name);
                Debug.Fail(string.Format(errmsg, name));
            }
        }

        /// <summary>
        /// Handles <see cref="GroupInfoMessages.RemoveMember"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to read from.</param>
        void ReadRemoveMember(BitStream bs)
        {
            string name = bs.ReadString();

            if (!_members.Remove(name))
            {
                const string errmsg = "Tried to remove `{0}` from the group member list, but they were not in the list!";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, name);
                Debug.Fail(string.Format(errmsg, name));
            }
        }

        /// <summary>
        /// Handles <see cref="GroupInfoMessages.SetGroup"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to read from.</param>
        void ReadSetGroup(BitStream bs)
        {
            _members.Clear();
            _founder = null;

            bool isInGroup = bs.ReadBool();

            if (!isInGroup)
                return;

            // Read the group members
            byte numMembers = bs.ReadByte();
            var members = bs.ReadStrings(numMembers);
            _members.AddRange(members);

            // Read the founder's name
            _founder = bs.ReadString();
        }

        /// <summary>
        /// Appends a message to a <see cref="BitStream"/> for synchronizing the client's group information for when
        /// a member is added to the group.
        /// The message is then read and handled by the receiver using <see cref="UserGroupInformation.Read"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to append the message to.</param>
        /// <param name="memberName">The name of the group member to add.</param>
        public static void WriteAddMember(BitStream bs, string memberName)
        {
            bs.WriteEnum(GroupInfoMessages.AddMember);
            bs.Write(memberName);
        }

        /// <summary>
        /// Appends a message to a <see cref="BitStream"/> for synchronizing all of the client's group information.
        /// The message is then read and handled by the receiver using <see cref="UserGroupInformation.Read"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to append the message to.</param>
        /// <param name="group">The group information. Can be null for when the user is not in a group.</param>
        public static void WriteGroupInfo(BitStream bs, IGroup group)
        {
            bs.WriteEnum(GroupInfoMessages.SetGroup);

            // Check if in a group
            if (group == null)
            {
                bs.Write(false);
                return;
            }

            bs.Write(true);

            // Write the group member names
            var members = group.Members.Select(x => x.Name).ToArray();
            Debug.Assert(members.Length <= byte.MaxValue);
            bs.Write((byte)members.Length);
            bs.Write(members, 0, members.Length);

            // Write the founder's name
            bs.Write(group.Founder.Name);
        }

        /// <summary>
        /// Appends a message to a <see cref="BitStream"/> for synchronizing the client's group information for when
        /// a member is removed from the group.
        /// The message is then read and handled by the receiver using <see cref="UserGroupInformation.Read"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to append the message to.</param>
        /// <param name="memberName">The name of the group member to remove.</param>
        public static void WriteRemoveMember(BitStream bs, string memberName)
        {
            bs.WriteEnum(GroupInfoMessages.RemoveMember);
            bs.Write(memberName);
        }

        /// <summary>
        /// Enum of the different packet messages for this class.
        /// </summary>
        enum GroupInfoMessages
        {
            /// <summary>
            /// Sets all the initial group information.
            /// </summary>
            SetGroup,

            /// <summary>
            /// Removes a member from the group.
            /// </summary>
            RemoveMember,

            /// <summary>
            /// Adds a member to the group.
            /// </summary>
            AddMember,
        }
    }
}