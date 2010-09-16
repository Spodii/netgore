using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Describes the different ways the client can connect to the server.
    /// </summary>
    public enum ClientConnectType
    {
        /// <summary>
        /// Connect for the purpose of logging into an existing account.
        /// </summary>
        Login,

        /// <summary>
        /// Connect for the purpose of creating a new account.
        /// </summary>
        NewAccount
    }
}