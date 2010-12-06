using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Contains the guild information for the client, and methods the server can use to send the data to the client.
    /// </summary>
    public class UserGuildInformation
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// List of all the members in the guild.
        /// </summary>
        readonly List<GuildMemberNameRank> _members = new List<GuildMemberNameRank>();

        /// <summary>
        /// List of the names of all the online members.
        /// </summary>
        readonly List<string> _onlineMembers = new List<string>();

        bool _inGuild = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserGuildInformation"/> class.
        /// </summary>
        public UserGuildInformation()
        {
            Name = string.Empty;
            Tag = string.Empty;
            InGuild = false;
        }

        /// <summary>
        /// Notifies listeners when the guild has changed. This can be either the user leaving a guild, joining a new
        /// guild, or having the initial guild being set.
        /// </summary>
        public event TypedEventHandler<UserGuildInformation> GuildChanged;

        /// <summary>
        /// Notifies listeners when a guild member has been added.
        /// </summary>
        public event TypedEventHandler<UserGuildInformation, EventArgs<GuildMemberNameRank>> MemberAdded;

        /// <summary>
        /// Notifies listeners when a guild member's rank has been updated.
        /// </summary>
        public event TypedEventHandler<UserGuildInformation, EventArgs<GuildMemberNameRank>> MemberRankUpdated;

        /// <summary>
        /// Notifies listeners when a guild member has been removed.
        /// </summary>
        public event TypedEventHandler<UserGuildInformation, EventArgs<string>> MemberRemoved;

        /// <summary>
        /// Notifies listeners when an offline guild member has come online.
        /// </summary>
        public event TypedEventHandler<UserGuildInformation, EventArgs<string>> OnlineMemberAdded;

        /// <summary>
        /// Notifies listeners when an online guild member has gone offline.
        /// </summary>
        public event TypedEventHandler<UserGuildInformation, EventArgs<string>> OnlineMemberRemoved;

        /// <summary>
        /// Gets if the client is in a guild at all.
        /// </summary>
        public bool InGuild
        {
            get { return _inGuild; }
            set
            {
                if (_inGuild == value)
                    return;

                _inGuild = value;

                if (!_inGuild)
                {
                    Name = string.Empty;
                    Tag = string.Empty;
                    _members.Clear();
                    _onlineMembers.Clear();
                }

                if (GuildChanged != null)
                    GuildChanged.Raise(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets all of the guild members. If the user is not in a guild, this will be empty.
        /// </summary>
        public IEnumerable<GuildMemberNameRank> Members
        {
            get
            {
                if (!InGuild)
                    return Enumerable.Empty<GuildMemberNameRank>();

                return _members;
            }
        }

        /// <summary>
        /// Gets the name of the guild, or an empty string if not in a guild.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the online members in the guild. If the user is not in a guild, this will be empty.
        /// </summary>
        public IEnumerable<GuildMemberNameRank> OnlineMembers
        {
            get
            {
                if (InGuild)
                {
                    for (var i = 0; i < _members.Count; i++)
                    {
                        if (_onlineMembers.Contains(_members[i].Name))
                            yield return _members[i];
                    }
                }
            }
        }

        /// <summary>
        /// Gets the guild's tag, or an empty string if not in a guild.
        /// </summary>
        public string Tag { get; private set; }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling the corresponding event without
        /// the overhead of using event hooks. Therefore, it is recommended that this overload is used instead of
        /// the corresponding event when possible.
        /// </summary>
        protected virtual void OnGuildChanged()
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling the corresponding event without
        /// the overhead of using event hooks. Therefore, it is recommended that this overload is used instead of
        /// the corresponding event when possible.
        /// </summary>
        /// <param name="member">The member the event is related to.</param>
        protected virtual void OnMemberAdded(GuildMemberNameRank member)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling the corresponding event without
        /// the overhead of using event hooks. Therefore, it is recommended that this overload is used instead of
        /// the corresponding event when possible.
        /// </summary>
        /// <param name="member">The member the event is related to.</param>
        protected virtual void OnMemberRankUpdated(GuildMemberNameRank member)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling the corresponding event without
        /// the overhead of using event hooks. Therefore, it is recommended that this overload is used instead of
        /// the corresponding event when possible.
        /// </summary>
        /// <param name="name">The member the event is related to.</param>
        protected virtual void OnMemberRemoved(string name)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling the corresponding event without
        /// the overhead of using event hooks. Therefore, it is recommended that this overload is used instead of
        /// the corresponding event when possible.
        /// </summary>
        /// <param name="name">The member the event is related to.</param>
        protected virtual void OnOnlineMemberAdded(string name)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling the corresponding event without
        /// the overhead of using event hooks. Therefore, it is recommended that this overload is used instead of
        /// the corresponding event when possible.
        /// </summary>
        /// <param name="name">The member the event is related to.</param>
        protected virtual void OnOnlineMemberRemoved(string name)
        {
        }

        /// <summary>
        /// Reads the data from the server related to the user guild information. This should only be used by the client.
        /// </summary>
        /// <param name="bitStream">The <see cref="BitStream"/> containing the data.</param>
        public void Read(BitStream bitStream)
        {
            var id = bitStream.ReadEnum<GuildInfoMessages>();
            switch (id)
            {
                case GuildInfoMessages.SetGuild:
                    ReadSetGuild(bitStream);
                    return;

                case GuildInfoMessages.AddMember:
                    ReadAddMember(bitStream);
                    return;

                case GuildInfoMessages.RemoveMember:
                    ReadRemoveMember(bitStream);
                    return;

                case GuildInfoMessages.AddOnlineMember:
                    ReadAddOnlineMember(bitStream);
                    return;

                case GuildInfoMessages.RemoveOnlineMember:
                    ReadRemoveOnlineMember(bitStream);
                    return;

                case GuildInfoMessages.UpdateRank:
                    ReadUpdateRank(bitStream);
                    return;

                case GuildInfoMessages.UpdateNameTag:
                    ReadUpdateNameTag(bitStream);
                    return;

                default:
                    const string errmsg = "Unknown GuildInfoMessages value `{0}`. Could not parse!";
                    var err = string.Format(errmsg, id);
                    log.Fatal(err);
                    Debug.Fail(err);
                    return;
            }
        }

        /// <summary>
        /// Reads the <see cref="GuildInfoMessages.AddMember"/> message.
        /// </summary>
        /// <param name="r">The stream to read the message from.</param>
        void ReadAddMember(IValueReader r)
        {
            var member = r.ReadGuildMemberNameRank(null);
            _members.Add(member);
            _members.Sort();

            OnMemberAdded(member);

            if (MemberAdded != null)
                MemberAdded.Raise(this, EventArgsHelper.Create(member));
        }

        /// <summary>
        /// Reads the <see cref="GuildInfoMessages.AddOnlineMember"/> message.
        /// </summary>
        /// <param name="r">The stream to read the message from.</param>
        void ReadAddOnlineMember(BitStream r)
        {
            var name = r.ReadString();
            SetOnlineValue(name, true);

            OnOnlineMemberAdded(name);

            if (OnlineMemberAdded != null)
                OnlineMemberAdded.Raise(this, EventArgsHelper.Create(name));
        }

        /// <summary>
        /// Reads the <see cref="GuildInfoMessages.RemoveMember"/> message.
        /// </summary>
        /// <param name="r">The stream to read the message from.</param>
        void ReadRemoveMember(BitStream r)
        {
            var name = r.ReadString();
            var removeCount = _members.RemoveAll(x => StringComparer.OrdinalIgnoreCase.Equals(x.Name, name));

            Debug.Assert(removeCount != 0, "Nobody with the name " + name + " existed in the collection.");
            Debug.Assert(removeCount < 2, "How the hell did we remove more than one item?");

            OnMemberRemoved(name);

            if (MemberRemoved != null)
                MemberRemoved.Raise(this, EventArgsHelper.Create(name));
        }

        /// <summary>
        /// Reads the <see cref="GuildInfoMessages.RemoveOnlineMember"/> message.
        /// </summary>
        /// <param name="r">The stream to read the message from.</param>
        void ReadRemoveOnlineMember(BitStream r)
        {
            var name = r.ReadString();
            SetOnlineValue(name, false);

            OnOnlineMemberRemoved(name);

            if (OnlineMemberRemoved != null)
                OnlineMemberRemoved.Raise(this, EventArgsHelper.Create(name));
        }

        /// <summary>
        /// Reads the <see cref="GuildInfoMessages.SetGuild"/> message.
        /// </summary>
        /// <param name="r">The stream to read the message from.</param>
        void ReadSetGuild(BitStream r)
        {
            _members.Clear();
            _onlineMembers.Clear();

            InGuild = r.ReadBool();

            if (InGuild)
            {
                Name = r.ReadString();
                Tag = r.ReadString();

                var numMembers = r.ReadUShort();
                for (var i = 0; i < numMembers; i++)
                {
                    var v = r.ReadGuildMemberNameRank(null);
                    _members.Add(v);
                }

                var onlineMembers = r.ReadUShort();
                for (var i = 0; i < onlineMembers; i++)
                {
                    var name = r.ReadString();
                    SetOnlineValue(name, true);
                }

                _members.Sort();
            }

            OnGuildChanged();

            if (GuildChanged != null)
                GuildChanged.Raise(this, EventArgs.Empty);
        }

        /// <summary>
        /// Reads the <see cref="GuildInfoMessages.UpdateNameTag"/> message.
        /// </summary>
        /// <param name="r">The stream to read the message from.</param>
        void ReadUpdateNameTag(BitStream r)
        {
            Name = r.ReadString();
            Tag = r.ReadString();
        }

        /// <summary>
        /// Reads the <see cref="GuildInfoMessages.UpdateRank"/> message.
        /// </summary>
        /// <param name="r">The stream to read the message from.</param>
        void ReadUpdateRank(IValueReader r)
        {
            var member = r.ReadGuildMemberNameRank(null);
            _members.RemoveAll(x => StringComparer.OrdinalIgnoreCase.Equals(x.Name, member.Name));
            _members.Add(member);
            _members.Sort();

            OnMemberRankUpdated(member);

            if (MemberRankUpdated != null)
                MemberRankUpdated.Raise(this, EventArgsHelper.Create(member));
        }

        /// <summary>
        /// Sets the online value for the member at the given index.
        /// </summary>
        /// <param name="name">The member's name.</param>
        /// <param name="online">True to set them as online; false to set them as offline.</param>
        void SetOnlineValue(string name, bool online)
        {
            if (!online)
            {
                // Remove online status
                _onlineMembers.Remove(name);
            }
            else
            {
                // Add online status
                if (!_onlineMembers.Contains(name, StringComparer.OrdinalIgnoreCase))
                    _onlineMembers.Add(name);
            }
        }

        /// <summary>
        /// Appends a message to a <see cref="BitStream"/> for synchronizing the client's guild information for when
        /// a member is added to the guild.
        /// The message is then read and handled by the receiver using <see cref="UserGuildInformation.Read"/>.
        /// </summary>
        /// <param name="pw">The <see cref="BitStream"/> to append the message to.</param>
        /// <param name="member">The guild member that was added.</param>
        public static void WriteAddMember(BitStream pw, GuildMemberNameRank member)
        {
            pw.WriteEnum(GuildInfoMessages.AddMember);
            pw.Write(null, member);
        }

        /// <summary>
        /// Appends a message to a <see cref="BitStream"/> for synchronizing the client's guild information for when
        /// a guild member comes online.
        /// The message is then read and handled by the receiver using <see cref="UserGuildInformation.Read"/>.
        /// </summary>
        /// <param name="pw">The <see cref="BitStream"/> to append the message to.</param>
        /// <param name="memberName">The name of the guild member that came online.</param>
        public static void WriteAddOnlineMember(BitStream pw, string memberName)
        {
            pw.WriteEnum(GuildInfoMessages.AddOnlineMember);
            pw.Write(memberName);
        }

        /// <summary>
        /// Appends a message to a <see cref="BitStream"/> for synchronizing the client's guild information for when
        /// the guild changes or the user is receiving their first update on the guild state and all values have to be sent.
        /// The message is then read and handled by the receiver using <see cref="UserGuildInformation.Read"/>.
        /// </summary>
        /// <param name="pw">The <see cref="BitStream"/> to append the message to.</param>
        /// <param name="guild">The guild to send the state values for.</param>
        public static void WriteGuildInfo(BitStream pw, IGuild guild)
        {
            pw.WriteEnum(GuildInfoMessages.SetGuild);

            if (guild == null)
            {
                pw.Write(false);
                return;
            }

            var members = guild.GetMembers().ToArray();
            var onlineMembers = guild.OnlineMembers.ToArray();

            pw.Write(true);
            pw.Write(guild.Name);
            pw.Write(guild.Tag);

            pw.Write((ushort)members.Length);
            for (var i = 0; i < members.Length; i++)
            {
                pw.Write(null, members[i]);
            }

            pw.Write((ushort)onlineMembers.Length);
            for (var i = 0; i < onlineMembers.Length; i++)
            {
                pw.Write(onlineMembers[i].Name);
            }
        }

        /// <summary>
        /// Appends a message to a <see cref="BitStream"/> for synchronizing the client's guild information for when
        /// a member is removed from the guild.
        /// The message is then read and handled by the receiver using <see cref="UserGuildInformation.Read"/>.
        /// </summary>
        /// <param name="pw">The <see cref="BitStream"/> to append the message to.</param>
        /// <param name="memberName">The name of the guild member to remove.</param>
        public static void WriteRemoveMember(BitStream pw, string memberName)
        {
            pw.WriteEnum(GuildInfoMessages.RemoveMember);
            pw.Write(memberName);
        }

        /// <summary>
        /// Appends a message to a <see cref="BitStream"/> for synchronizing the client's guild information for when
        /// a member goes offline.
        /// The message is then read and handled by the receiver using <see cref="UserGuildInformation.Read"/>.
        /// </summary>
        /// <param name="pw">The <see cref="BitStream"/> to append the message to.</param>
        /// <param name="memberName">The name of the guild member that went offline.</param>
        public static void WriteRemoveOnlineMember(BitStream pw, string memberName)
        {
            pw.WriteEnum(GuildInfoMessages.RemoveOnlineMember);
            pw.Write(memberName);
        }

        /// <summary>
        /// Appends a message to a <see cref="BitStream"/> for synchronizing the client's guild information for when
        /// a member's rank changes.
        /// The message is then read and handled by the receiver using <see cref="UserGuildInformation.Read"/>.
        /// </summary>
        /// <param name="pw">The <see cref="BitStream"/> to append the message to.</param>
        /// <param name="member">The guild member who's rank changed.</param>
        public static void WriteUpdateMemberRank(BitStream pw, GuildMemberNameRank member)
        {
            pw.WriteEnum(GuildInfoMessages.UpdateRank);
            pw.Write(null, member);
        }

        /// <summary>
        /// Appends a message to a <see cref="BitStream"/> for synchronizing the client's guild information for when
        /// a the guild's name or tag changes and needs to be updated.
        /// The message is then read and handled by the receiver using <see cref="UserGuildInformation.Read"/>.
        /// </summary>
        /// <param name="pw">The <see cref="BitStream"/> to append the message to.</param>
        /// <param name="name">The new guild name.</param>
        /// <param name="tag">The new guild tag.</param>
        public static void WriteUpdateNameTag(BitStream pw, string name, string tag)
        {
            pw.WriteEnum(GuildInfoMessages.UpdateNameTag);
            pw.Write(name);
            pw.Write(tag);
        }

        /// <summary>
        /// Enum of the different packet messages for this class.
        /// </summary>
        enum GuildInfoMessages
        {
            /// <summary>
            /// Adds a single member.
            /// </summary>
            AddMember,

            /// <summary>
            /// Removes a single member.
            /// </summary>
            RemoveMember,

            /// <summary>
            /// Adds a single online member
            /// </summary>
            AddOnlineMember,

            /// <summary>
            /// Removes a single online member.
            /// </summary>
            RemoveOnlineMember,

            /// <summary>
            /// Sets all the initial guild information.
            /// </summary>
            SetGuild,

            /// <summary>
            /// Updates the rank of a single member.
            /// </summary>
            UpdateRank,

            /// <summary>
            /// Updates the guild's name and tag.
            /// </summary>
            UpdateNameTag,
        }
    }
}