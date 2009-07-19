using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using Microsoft.Xna.Framework;
using NetGore.Collections;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// The actual class that handles the Say commands.
    /// </summary>
    public class SayCommands
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the User that the current command came from.
        /// </summary>
        public User User { get; internal set; }

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
    }
}
