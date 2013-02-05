using System.Linq;
using NetGore;

namespace DemoGame.Server
{
    /// <summary>
    /// Handles processing what Users say.
    /// </summary>
    public class SayHandler : SayHandlerBase<User, SayHandlerCommandAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SayHandler"/> class.
        /// </summary>
        /// <param name="server">Server that the commands are coming from.</param>
        public SayHandler(Server server) : base(new SayHandlerCommands(server))
        {
        }

        /// <summary>
        /// Gets if the given <paramref name="user"/> is allowed to invoke the given command.
        /// </summary>
        /// <param name="user">The user invoking the command.</param>
        /// <param name="commandData">The information about the command to be invoked.</param>
        /// <returns>True if the command can be invoked; otherwise false.</returns>
        protected override bool AllowInvokeCommand(User user, StringCommandParserCommandData<SayHandlerCommandAttribute> commandData)
        {
            // Check for a valid user
            if (user == null)
                return false;

            // No permissions required
            if (commandData.Attribute.Permissions == UserPermissions.None)
                return true;

            // Check for permission level
            if (!user.Permissions.IsSet(commandData.Attribute.Permissions))
                return false;

            return base.AllowInvokeCommand(user, commandData);
        }

        /// <summary>
        /// Gets the message to display when the user is not allowed to invoke a command.
        /// </summary>
        /// <param name="user">The user invoking the command.</param>
        /// <param name="commandData">The information about the command to be invoked.</param>
        /// <returns>The message to display to the <paramref name="user"/>, or null or empty to display nothing.</returns>
        protected override string GetCommandNotAllowedMessage(User user, StringCommandParserCommandData<SayHandlerCommandAttribute> commandData)
        {
            return null;
        }

        /// <summary>
        /// When overridden in the derived class, handles the output from a command.
        /// </summary>
        /// <param name="user">The user that the command came from.</param>
        /// <param name="text">The output text from the command. Will not be null or empty.</param>
        protected override void HandleCommandOutput(User user, string text, string orginalMessage)
        {
            ThreadAsserts.IsMainThread();

            using (var pw = ServerPacket.Chat(text))
            {
                user.Send(pw, ServerMessageType.GUIChat);
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles text that was not a command.
        /// </summary>
        /// <param name="user">The user the <paramref name="text"/> came from.</param>
        /// <param name="text">The text that wasn't a command.</param>
        /// <param name="orginalMessage">The orginal message that was inititally input.</param>
        protected override void HandleNonCommand(User user, string text)
        {
            ThreadAsserts.IsMainThread();

            using (var pw = ServerPacket.ChatSay(user.Name, user.MapEntityIndex, text))
            {
                user.Map.SendToArea(user, pw, ServerMessageType.GUIChat);
            }

            EventCounterManager.User.Increment(user.ID, UserEventCounterType.ChatLocalTimes);
            EventCounterManager.User.Increment(user.ID, UserEventCounterType.ChatLocalChars, text.Length);
        }
    }
}