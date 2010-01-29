using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;
using NetGore.Network;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Contains the guild information for the client, and methods the server can use to send the data to the client.
    /// </summary>
    public abstract class UserGuildInformationBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly List<KeyValuePair<GuildMemberNameRank, bool>> _members = new List<KeyValuePair<GuildMemberNameRank, bool>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="UserGuildInformationBase"/> class.
        /// </summary>
        protected UserGuildInformationBase()
        {
            Name = string.Empty;
            Tag = string.Empty;
            InGuild = false;
        }

        /// <summary>
        /// Gets if the client is in a guild at all.
        /// </summary>
        public bool InGuild { get; private set; }

        /// <summary>
        /// Gets all of the guild members. If the user is not in a guild, this will be empty.
        /// </summary>
        public IEnumerable<GuildMemberNameRank> Members
        {
            get
            {
                if (!InGuild)
                    return Enumerable.Empty<GuildMemberNameRank>();

                return _members.Select(x => x.Key);
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
                if (!InGuild)
                    return Enumerable.Empty<GuildMemberNameRank>();

                return _members.Where(x => x.Value).Select(x => x.Key);
            }
        }

        /// <summary>
        /// Gets the guild's tag, or an empty string if not in a guild.
        /// </summary>
        public string Tag { get; private set; }

        /// <summary>
        /// Finds the list index for a member with the given name.
        /// </summary>
        /// <param name="memberName">The member's name.</param>
        /// <returns>The index of the member with the given <paramref name="memberName"/>, or -1 if invalid.</returns>
        int FindMemberIndexByName(string memberName)
        {
            for (int i = 0; i < _members.Count; i++)
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(_members[i].Key.Name, memberName))
                    return i;
            }

            return -1;
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
                case GuildInfoMessages.SetMembers:
                    ReadSetMembers(bitStream);
                    return;

                case GuildInfoMessages.AddMember:
                    ReadAddMember(bitStream);
                    return;

                case GuildInfoMessages.RemoveMember:
                    ReadRemoveMember(bitStream);
                    return;

                case GuildInfoMessages.SetOnlineMembers:
                    ReadSetOnlineMembers(bitStream);
                    return;

                case GuildInfoMessages.AddOnlineMember:
                    ReadAddOnlineMember(bitStream);
                    return;

                case GuildInfoMessages.RemoveOnlineMember:
                    ReadRemoveOnlineMember(bitStream);
                    return;

                case GuildInfoMessages.SetNameTag:
                    ReadSetNameTag(bitStream);
                    return;

                default:
                    const string errmsg = "Unknown GuildInfoMessages value `{0}`. Could not parse!";
                    string err = string.Format(errmsg, id);
                    log.Fatal(err);
                    Debug.Fail(err);
                    return;
            }
        }

        void ReadAddMember(IValueReader r)
        {
            var member = r.ReadGuildMemberNameRank(null);
            _members.Add(new KeyValuePair<GuildMemberNameRank, bool>(member, false));
        }

        void ReadAddOnlineMember(BitStream pw)
        {
            string name = pw.ReadString();
            var index = FindMemberIndexByName(name);
            SetOnlineValue(index, true);
        }

        void ReadRemoveMember(BitStream r)
        {
            var memberName = r.ReadString();
            int removeCount = _members.RemoveAll(x => x.Key.Name == memberName);

            Debug.Assert(removeCount != 0, "Nobody with the name " + memberName + " existed in the collection.");
            Debug.Assert(removeCount < 2, "How the hell did we remove more than one item?");
        }

        void ReadRemoveOnlineMember(BitStream pw)
        {
            string name = pw.ReadString();
            var index = FindMemberIndexByName(name);
            SetOnlineValue(index, false);
        }

        void ReadSetMembers(BitStream r)
        {
            _members.Clear();

            ushort length = r.ReadUShort();

            for (int i = 0; i < length; i++)
            {
                var member = r.ReadGuildMemberNameRank(null);
                _members.Add(new KeyValuePair<GuildMemberNameRank, bool>(member, false));
            }
        }

        void ReadSetNameTag(BitStream r)
        {
            var inGuild = r.ReadBool();
            if (!inGuild)
            {
                _members.Clear();
                InGuild = false;
                return;
            }

            InGuild = true;
            Name = r.ReadString();
            Tag = r.ReadString();
        }

        void ReadSetOnlineMembers(BitStream r)
        {
            for (int i = 0; i < _members.Count; i++)
            {
                SetOnlineValue(i, false);
            }

            Debug.Assert(_members.All(x => !x.Value), "One or more members failed to get to not online...");

            ushort length = r.ReadUShort();

            for (int i = 0; i < length; i++)
            {
                var memberName = r.ReadString();
                var index = FindMemberIndexByName(memberName);
                Debug.Assert(index < 0 || _members[index].Value, "Member was already set as online... huh...");
                SetOnlineValue(index, true);
            }
        }

        /// <summary>
        /// Sets the online value for the member at the given index.
        /// </summary>
        /// <param name="i">The member's list index.</param>
        /// <param name="online">True to set them as online; false to set them as offline.</param>
        void SetOnlineValue(int i, bool online)
        {
            if (i < 0)
            {
                Debug.Fail("Invalid index...");
                return;
            }

            var v = _members[i];
            if (v.Value != online)
                _members[i] = new KeyValuePair<GuildMemberNameRank, bool>(v.Key, online);
        }

        public static void WriteAddMember(PacketWriter pw, GuildMemberNameRank member)
        {
            pw.WriteEnum(GuildInfoMessages.AddMember);
            pw.Write(null, member);
        }

        public static void WriteAddOnlineMember(PacketWriter pw, string memberName)
        {
            pw.WriteEnum(GuildInfoMessages.AddOnlineMember);
            pw.Write(memberName);
        }

        public static void WriteOnlineMembers(PacketWriter pw, IEnumerable<string> memberNames)
        {
            var a = memberNames.ToArray();
            pw.WriteEnum(GuildInfoMessages.SetOnlineMembers);
            pw.Write((ushort)a.Length);
            for (int i = 0; i < a.Length; i++)
            {
                pw.Write(a[i]);
            }
        }

        public static void WriteRemoveMember(PacketWriter pw, string memberName)
        {
            pw.WriteEnum(GuildInfoMessages.RemoveMember);
            pw.Write(memberName);
        }

        public static void WriteRemoveOnlineMember(PacketWriter pw, string memberName)
        {
            pw.WriteEnum(GuildInfoMessages.RemoveOnlineMember);
            pw.Write(memberName);
        }

        public static void WriteSetMembers(PacketWriter pw, IEnumerable<GuildMemberNameRank> members)
        {
            var a = members.ToArray();

            pw.WriteEnum(GuildInfoMessages.SetMembers);
            pw.Write((ushort)a.Length);
            for (int i = 0; i < a.Length; i++)
            {
                pw.Write(null, a[i]);
            }
        }

        public static void WriteSetNameTag(PacketWriter pw, bool inGuild, string name, string tag)
        {
            pw.WriteEnum(GuildInfoMessages.SetNameTag);
            pw.Write(inGuild);

            if (inGuild)
            {
                pw.Write(name);
                pw.Write(tag);
            }
        }

        /// <summary>
        /// Enum of the different packet messages for this class.
        /// </summary>
        enum GuildInfoMessages
        {
            /// <summary>
            /// Sets all of the members in the guild.
            /// </summary>
            SetMembers,

            /// <summary>
            /// Adds a single member.
            /// </summary>
            AddMember,

            /// <summary>
            /// Removes a single member.
            /// </summary>
            RemoveMember,

            /// <summary>
            /// Sets all of the online members.
            /// </summary>
            SetOnlineMembers,

            /// <summary>
            /// Adds a single online member
            /// </summary>
            AddOnlineMember,

            /// <summary>
            /// Removes a single online member.
            /// </summary>
            RemoveOnlineMember,

            /// <summary>
            /// Sets the name and tag of the guild, along with if the guild even exists.
            /// </summary>
            SetNameTag
        }
    }
}