using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Enum containing all of the different in-game messages, including messages sent from the Server
    /// to the Client.
    /// </summary>
    public enum GameMessage : byte
    {
        /// <summary>
        /// Invalid Say chat command.
        /// </summary>
        InvalidCommand,

        /// <summary>
        /// Message received when a User shouts.
        /// </summary>
        CommandShout,

        /// <summary>
        /// Tell command contains no name.
        /// </summary>
        CommandTellNoName,

        /// <summary>
        /// Tell command contaisn no message.
        /// </summary>
        CommandTellNoMessage,

        /// <summary>
        /// Message received by the sender of a Tell command.
        /// </summary>
        CommandTellSender,

        /// <summary>
        /// Message received by the receiver of a Tell command.
        /// </summary>
        CommandTellReceiver,

        /// <summary>
        /// Tell command contains the name of a User that does not exist.
        /// </summary>
        CommandTellInvalidUser,

        /// <summary>
        /// Tell command contains the name of a User that exists, but is offline.
        /// </summary>
        CommandTellOfflineUser,

        /// <summary>
        /// Tried to log in, but an invalid account name was given.
        LoginInvalidName,

        /// <summary>
        /// Tried to log in, but an invalid password was given.
        /// </summary>
        LoginInvalidPassword,

        /// <summary>
        /// Tried to log in, but the account is already in use.
        /// </summary>
        LoginAccountInUse
    }
}