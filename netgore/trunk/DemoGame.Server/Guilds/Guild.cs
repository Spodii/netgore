using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using NetGore.Db;
using NetGore.Features.Guilds;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Server.Guilds
{
    public class Guild : GuildBase, IGuildTable
    {
        static readonly CountGuildFoundersQuery _countGuildFoundersQuery;
        static readonly DeleteGuildQuery _deleteGuildQuery;
        static readonly SelectGuildEventsQuery _selectGuildEventsQuery;
        static readonly SelectGuildMembersListQuery _selectGuildMembersListQuery;
        static readonly UpdateGuildNameQuery _updateGuildNameQuery;
        static readonly UpdateGuildQuery _updateGuildQuery;
        static readonly UpdateGuildTagQuery _updateGuildTagQuery;

        readonly DateTime _created;

        /// <summary>
        /// Initializes the <see cref="Guild"/> class.
        /// </summary>
        static Guild()
        {
            var dbController = DbControllerBase.GetInstance();
            _countGuildFoundersQuery = dbController.GetQuery<CountGuildFoundersQuery>();
            _deleteGuildQuery = dbController.GetQuery<DeleteGuildQuery>();
            _updateGuildNameQuery = dbController.GetQuery<UpdateGuildNameQuery>();
            _updateGuildTagQuery = dbController.GetQuery<UpdateGuildTagQuery>();
            _selectGuildMembersListQuery = dbController.GetQuery<SelectGuildMembersListQuery>();
            _updateGuildQuery = dbController.GetQuery<UpdateGuildQuery>();
            _selectGuildEventsQuery = dbController.GetQuery<SelectGuildEventsQuery>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Guild"/> class.
        /// </summary>
        /// <param name="guildManager">The guild manager.</param>
        /// <param name="guildInfo">The guild info.</param>
        public Guild(IGuildManager guildManager, IGuildTable guildInfo)
            : base(guildManager, guildInfo.ID, guildInfo.Name, guildInfo.Tag)
        {
            _created = guildInfo.Created;
        }

        /// <summary>
        /// Gets the name and rank for all the members in the guild.
        /// </summary>
        /// <returns>The name and rank for all the members in the guild.</returns>
        public override IEnumerable<GuildMemberNameRank> GetMembers()
        {
            return _selectGuildMembersListQuery.Execute(ID);
        }

        /// <summary>
        /// When overridden in the derived class, gets the number of founders (highest rank) in the guild.
        /// </summary>
        /// <returns>The number of founders (highest rank) in the guild.</returns>
        protected override int GetNumberOfFounders()
        {
            return _countGuildFoundersQuery.Execute(ID);
        }

        /// <summary>
        /// When overridden in the derived class, handles destroying the guild. This needs to remove all members
        /// in the guild from the guild, and remove the guild itself from the database.
        /// </summary>
        protected override void HandleDestroyed()
        {
            // Remove all online members in the guild from the guild
            foreach (var member in OnlineMembers)
            {
                member.Guild = null;
            }

            // Delete the guild from the database, which will also remove all members not logged in from the guild
            _deleteGuildQuery.Execute(ID);
        }

        /// <summary>
        /// When overridden in the derived class, attempts to set the name of this guild to the given
        /// <paramref name="newName"/> in the database. The <paramref name="newName"/> does not need to
        /// be checked if valid.
        /// </summary>
        /// <param name="newName">The new name for the guild.</param>
        /// <returns>True if the name was successfully changed; otherwise false.</returns>
        protected override bool InternalTryChangeName(string newName)
        {
            return _updateGuildNameQuery.ExecuteWithResult(new UpdateGuildNameQuery.QueryArgs(ID, newName)) > 0;
        }

        /// <summary>
        /// When overridden in the derived class, attempts to set the tag of this guild to the given
        /// <paramref name="newTag"/> in the database. The <paramref name="newTag"/> does not need to
        /// be checked if valid.
        /// </summary>
        /// <param name="newTag">The new tag for the guild.</param>
        /// <returns>True if the tag was successfully changed; otherwise false.</returns>
        protected override bool InternalTryChangeTag(string newTag)
        {
            return _updateGuildTagQuery.ExecuteWithResult(new UpdateGuildTagQuery.QueryArgs(ID, newTag)) > 0;
        }

        /// <summary>
        /// When overridden in the derived class, does the actually handling of viewing the event log for the guild.
        /// This should send the latest entries of the guild event log to the <paramref name="invoker"/>.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully viewed the log; otherwise false.</returns>
        protected override bool InternalTryViewEventLog(IGuildMember invoker)
        {
            var user = invoker as INetworkSender;
            if (user == null)
                return false;

            var events = _selectGuildEventsQuery.Execute(ID);

            foreach (var e in events)
            {
                using (var pw = ServerPacket.Chat(e.ID + ": " + (GuildEvents)e.EventID))
                {
                    user.Send(pw, ServerMessageType.GUI);
                }
            }

            return true;
        }

        /// <summary>
        /// When overridden in the derived class, displays the members of the guild to the <paramref name="invoker"/>.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully viewed the member list; otherwise false.</returns>
        protected override bool InternalTryViewMembers(IGuildMember invoker)
        {
            var user = invoker as INetworkSender;
            if (user == null)
                return false;

            // Get the list of members and their ranks
            SendGuildMemberList(user, "All guild members:", GetMembers());

            return true;
        }

        /// <summary>
        /// When overridden in the derived class, displays the online members of the guild to the <paramref name="invoker"/>.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully viewed the online member list; otherwise false.</returns>
        protected override bool InternalTryViewOnlineMembers(IGuildMember invoker)
        {
            var user = invoker as INetworkSender;
            if (user == null)
                return false;

            var members = OnlineMembers.OrderBy(x => (int)x.GuildRank).Select(x => new GuildMemberNameRank(x.Name, x.GuildRank));
            SendGuildMemberList(user, "Online guild members:", members);

            return true;
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling after a new guild member is added.
        /// Use this instead of the corresponding event when possible.
        /// </summary>
        /// <param name="newMember"></param>
        protected override void OnMemberAdded(IGuildMember newMember)
        {
            base.OnMemberAdded(newMember);

            var v = new GuildMemberNameRank(newMember.Name, newMember.GuildRank);
            using (var pw = ServerPacket.GuildInfo(x => UserGuildInformation.WriteAddMember(x, v)))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling after a guild member is kicked.
        /// Use this instead of the corresponding event when possible.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <param name="target">The optional guild member the event involves.</param>
        protected override void OnMemberKicked(IGuildMember invoker, IGuildMember target)
        {
            base.OnMemberKicked(invoker, target);

            using (var pw = ServerPacket.GuildInfo(x => UserGuildInformation.WriteRemoveMember(x, target.Name)))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling after a guild member is promoted.
        /// Use this instead of the corresponding event when possible.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <param name="target">The optional guild member the event involves.</param>
        protected override void OnMemberPromoted(IGuildMember invoker, IGuildMember target)
        {
            base.OnMemberPromoted(invoker, target);

            var v = new GuildMemberNameRank(target.Name, target.GuildRank);
            using (var pw = ServerPacket.GuildInfo(x => UserGuildInformation.WriteUpdateMemberRank(x, v)))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling after the guild's name has changed.
        /// Use this instead of the corresponding event when possible.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        protected override void OnNameChanged(IGuildMember invoker, string oldName, string newName)
        {
            using (var pw = ServerPacket.SendMessage(GameMessage.GuildRenamed, oldName, newName, invoker.Name))
            {
                Send(pw);
            }

            using (var pw = ServerPacket.GuildInfo(x => UserGuildInformation.WriteUpdateNameTag(x, Name, Tag)))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of when a member of this
        /// guild has come online.
        /// </summary>
        /// <param name="guildMember">The guild member that came online.</param>
        protected override void OnOnlineUserAdded(IGuildMember guildMember)
        {
            base.OnOnlineUserAdded(guildMember);

            using (var pw = ServerPacket.GuildInfo(x => UserGuildInformation.WriteAddOnlineMember(x, guildMember.Name)))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of when a member of this
        /// guild has gone offline.
        /// </summary>
        /// <param name="guildMember">The guild member that went offline.</param>
        protected override void OnOnlineUserRemoved(IGuildMember guildMember)
        {
            base.OnOnlineUserRemoved(guildMember);

            using (var pw = ServerPacket.GuildInfo(x => UserGuildInformation.WriteRemoveOnlineMember(x, guildMember.Name)))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling after the guild's tag has changed.
        /// Use this instead of the corresponding event when possible.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <param name="oldTag">The old tag.</param>
        /// <param name="newTag">The new tag.</param>
        protected override void OnTagChanged(IGuildMember invoker, string oldTag, string newTag)
        {
            using (var pw = ServerPacket.SendMessage(GameMessage.GuildRetagged, oldTag, newTag, invoker.Name))
            {
                Send(pw);
            }

            using (var pw = ServerPacket.GuildInfo(x => UserGuildInformation.WriteUpdateNameTag(x, Name, Tag)))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// When overridden in the derived class, saves all of the guild's information to the database.
        /// </summary>
        public override void Save()
        {
            _updateGuildQuery.Execute(this);
        }

        /// <summary>
        /// Sends a message to all guild members.
        /// </summary>
        /// <param name="data">The <see cref="BitStream"/> containing the data to send.</param>
        public void Send(BitStream data)
        {
            foreach (var member in OnlineMembers.OfType<INetworkSender>())
            {
                member.Send(data, ServerMessageType.GUIChat);
            }
        }

        /// <summary>
        /// Sends a list of guild member names and ranks to a <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user to send the information to.</param>
        /// <param name="header">The heading to give the list.</param>
        /// <param name="members">The guild members and their ranks to include in the list.</param>
        static void SendGuildMemberList(INetworkSender user, string header, IEnumerable<GuildMemberNameRank> members)
        {
            // Build the string
            var sb = new StringBuilder();
            sb.AppendLine(header);

            foreach (var member in members)
            {
                sb.AppendLine(string.Format("{0}: {1}", member.Name, member.Rank));
            }

            // Send
            using (var pw = ServerPacket.Chat(sb.ToString()))
            {
                user.Send(pw, ServerMessageType.GUI);
            }
        }

        #region IGuildTable Members

        /// <summary>
        /// Gets the value of the database column `created`.
        /// </summary>
        public DateTime Created
        {
            get { return _created; }
        }

        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        GuildID IGuildTable.ID
        {
            get { return ID; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IGuildTable IGuildTable.DeepCopy()
        {
            return new GuildTable(this);
        }

        #endregion
    }
}