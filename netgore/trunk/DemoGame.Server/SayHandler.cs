using System;
using System.Linq;
using System.Text;
using DemoGame.Server.Guilds;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.Features.Guilds;
using NetGore.Network;

// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace DemoGame.Server
{
    /// <summary>
    /// Handles processing what Users say.
    /// </summary>
    public class SayHandler : SayHandlerBase<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SayHandler"/> class.
        /// </summary>
        /// <param name="server">Server that the commands are coming from.</param>
        public SayHandler(Server server) : base(new SayCommands(server))
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles the output from a command.
        /// </summary>
        /// <param name="user">The user that the command came from.</param>
        /// <param name="text">The output text from the command. Will not be null or empty.</param>
        protected override void HandleCommandOutput(User user, string text)
        {
            ThreadAsserts.IsMainThread();

            using (PacketWriter pw = ServerPacket.Chat(text))
            {
                user.Send(pw);
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles text that was not a command.
        /// </summary>
        /// <param name="user">The user the <paramref name="text"/> came from.</param>
        /// <param name="text">The text that wasn't a command.</param>
        protected override void HandleNonCommand(User user, string text)
        {
            ThreadAsserts.IsMainThread();

            using (PacketWriter pw = ServerPacket.ChatSay(user.Name, user.MapEntityIndex, text))
            {
                user.Map.SendToArea(user, pw);
            }
        }

        /// <summary>
        /// The actual class that handles the Say commands.
        /// </summary>
        public class SayCommands : ISayCommands<User>
        {
            readonly Server _server;

            /// <summary>
            /// SayCommands constructor.
            /// </summary>
            /// <param name="server">The Server that the commands will come from.</param>
            public SayCommands(Server server)
            {
                if (server == null)
                    throw new ArgumentNullException("server");

                _server = server;
            }

            /// <summary>
            /// Gets the Server that the commands are coming from.
            /// </summary>
            public Server Server
            {
                get { return _server; }
            }

            /// <summary>
            /// Gets the World that the User belongs to.
            /// </summary>
            public World World
            {
                get { return Server.World; }
            }

            [SayCommand("Tell")]
            [SayCommand("Whisper")]
            public void CmdTell(string userName, string message)
            {
                // Check for a message to tell
                if (string.IsNullOrEmpty(userName))
                {
                    // Invalid message
                    using (PacketWriter pw = ServerPacket.SendMessage(GameMessage.CommandTellNoName))
                    {
                        User.Send(pw);
                    }
                    return;
                }

                // Find the user to tell
                if (string.IsNullOrEmpty(message))
                {
                    // No or invalid message
                    using (PacketWriter pw = ServerPacket.SendMessage(GameMessage.CommandTellNoMessage))
                    {
                        User.Send(pw);
                    }
                    return;
                }

                User target = World.FindUser(userName);

                // Check if the target user is available or not
                if (target != null)
                {
                    // Message to sender ("You tell...")
                    using (PacketWriter pw = ServerPacket.SendMessage(GameMessage.CommandTellSender, target.Name, message))
                    {
                        User.Send(pw);
                    }

                    // Message to receivd ("X tells you...")
                    using (PacketWriter pw = ServerPacket.SendMessage(GameMessage.CommandTellReceiver, User.Name, message))
                    {
                        target.Send(pw);
                    }
                }
                else
                {
                    // User not found
                    using (PacketWriter pw = ServerPacket.SendMessage(GameMessage.CommandTellInvalidUser, userName))
                    {
                        User.Send(pw);
                    }
                }
            }

            [SayCommand("CreateTestDamageTrap")]
            public void CreateTestDamageTrap()
            {
                // This is just a temporary test command...
                DamageTrapEntity trap = new DamageTrapEntity(User.Position, new Vector2(64, 64));
                User.Map.AddEntity(trap);
            }

            /// <summary>
            /// Requires the user to not be in a guild.
            /// </summary>
            /// <returns>If false, the command should be aborted.</returns>
            bool RequireUserNotInGuild()
            {
                if (User.Guild != null)
                {
                    using (var pw = ServerPacket.SendMessage(GameMessage.InvalidCommandMustNotBeInGuild))
                    {
                        User.Send(pw);
                    }

                    return false;
                }

                return true;
            }

            /// <summary>
            /// Requires the user to be in a guild.
            /// </summary>
            /// <returns>If false, the command should be aborted.</returns>
            bool RequireUserInGuild()
            {
                if (User.Guild == null)
                {
                    using (var pw = ServerPacket.SendMessage(GameMessage.InvalidCommandMustBeInGuild))
                    {
                        User.Send(pw);
                    }

                    return false;
                }

                return true;
            }

            static readonly GuildSettings _guildSettings = GuildSettings.Instance;

            [SayCommand("CreateGuild")]
            public void CreateGuild(string name, string tag)
            {
                if (!RequireUserNotInGuild())
                    return;

                // Valid name
                if (!_guildSettings.IsValidName(name))
                {
                    User.Send(GameMessage.GuildCreationFailedNameInvalid, name);
                    return;
                }

                if (!GuildManager.IsNameAvailable(name))
                {
                    User.Send(GameMessage.GuildCreationFailedNameNotAvailable, name);
                    return;
                }

                // Valid tag
                if (!_guildSettings.IsValidTag(tag))
                {
                    User.Send(GameMessage.GuildCreationFailedTagInvalid, tag);
                    return;
                }

                if (!GuildManager.IsTagAvailable(tag))
                {
                    User.Send(GameMessage.GuildCreationFailedTagNotAvailable, tag);
                    return;
                }

                // Create
                var guild = GuildManager.TryCreateGuild(User, name, tag);
                if (guild == null)
                {
                    User.Send(GameMessage.GuildCreationFailedUnknownReason, name, tag);
                }
                else
                {
                    User.Send(GameMessage.GuildCreationSuccessful, name, tag);
                }
            }

            public GuildManager GuildManager { get { return Server.GuildManager; } }

            [SayCommand("LeaveGuild")]
            public void LeaveGuild()
            {
                if (!RequireUserInGuild())
                    return;

                User.Guild = null;
            }

            [SayCommand("GuildMembers")]
            public void GuildMembers()
            {
                if (!RequireUserInGuild())
                    return;

                User.Guild.TryViewMembers(User);
            }

            [SayCommand("GuildOnline")]
            public void GuildOnline()
            {
                if (!RequireUserInGuild())
                    return;

                User.Guild.TryViewOnlineMembers(User);
            }

            [SayCommand("GuildKick")]
            public void GuildKick()
            {
                if (!RequireUserInGuild())
                    return;

                // TODO: ...
            }

            [SayCommand("Promote")]
            public void Promote()
            {
                if (!RequireUserInGuild())
                    return;

                // TODO: ...
            }

            [SayCommand("Demote")]
            public void Demote()
            {
                if (!RequireUserInGuild())
                    return;

                // TODO: ...
            }

            [SayCommand("RenameGuild")]
            public void RenameGuild()
            {
                if (!RequireUserInGuild())
                    return;

                // TODO: ...
            }

            [SayCommand("GuildInvite")]
            public void GuildInvite()
            {
                // TODO: ...
            }

            [SayCommand("GuildLog")]
            public void GuildLog()
            {
                if (!RequireUserInGuild())
                    return;

                // TODO: ...
            }

            [SayCommand("GSay")]
            public void GSay()
            {
                if (!RequireUserInGuild())
                    return;

                // TODO: ...
            }

            [SayCommand("GuildHelp")]
            public void GuildHelp()
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Guild commands:");
                sb.AppendLine("/CreateGuild [name] [symbol]");
                sb.AppendLine("/LeaveGuild");
                sb.AppendLine("/GuildMembers");
                sb.AppendLine("/GuildOnline");
                sb.AppendLine("/GuildKick [user]");
                sb.AppendLine("/Promote [user]");
                sb.AppendLine("/Demote [user]");
                sb.AppendLine("/RenameGuild [name]");
                sb.AppendLine("/GuildInvite [user]");
                sb.AppendLine("/GuildLog");
                sb.AppendLine("/GSay [message]");

                using (var pw = ServerPacket.Chat(sb.ToString()))
                {
                    User.Send(pw);
                }
            }

            [SayCommand("Shout")]
            public void Shout(string message)
            {
                using (PacketWriter pw = ServerPacket.SendMessage(GameMessage.CommandShout, User.Name, message))
                {
                    World.Send(pw);
                }
            }

            [SayCommand("Suicide")]
            public void Suicide()
            {
                User.Kill();
            }

            #region ISayCommands<User> Members

            /// <summary>
            /// Gets or sets the User that the current command came from.
            /// </summary>
            public User User { get; set; }

            #endregion
        }
    }
}