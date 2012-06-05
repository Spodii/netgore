using System.Linq;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// Extension methods for the <see cref="IIPSocket"/>.
    /// </summary>
    public static class IIPSocketExtensions
    {
        /// <summary>
        /// Terminates this connection. This is the preferred way to call <see cref="IIPSocket.Disconnect"/>
        /// on the server since you can localize the <paramref name="gameMessage"/>.
        /// </summary>
        /// <param name="socket">The <see cref="IIPSocket"/>.</param>
        /// <param name="gameMessage">The <see cref="GameMessage"/> to use for the disconnect reason. That is, the <see cref="GameMessage"/>
        /// that describes why the server closed the connection.</param>
        /// <param name="p">The <paramref name="gameMessage"/> arguments.</param>
        public static void Disconnect(this IIPSocket socket, GameMessage gameMessage, params object[] p)
        {
            var reason = GameMessageHelper.AsString(gameMessage, p);
            socket.Disconnect(reason);
        }
    }
}