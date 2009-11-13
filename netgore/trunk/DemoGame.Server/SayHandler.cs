using System;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;
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
                user.Map.SendToArea(user.Center, pw);
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
                // NOTE: This is a temporary command
                DamageTrapEntity trap = new DamageTrapEntity();
                trap.Resize(new Vector2(64, 64));
                trap.Teleport(User.Position);
                User.Map.AddEntity(trap);
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