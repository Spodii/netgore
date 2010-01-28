using System.Linq;
using NetGore.IO;

namespace DemoGame.Server
{
    /// <summary>
    /// Interface for an object that can be used to communicate directly to a single client.
    /// </summary>
    public interface IClientCommunicator
    {
        /// <summary>
        /// Sends data to the User. This method is thread-safe.
        /// </summary>
        /// <param name="data">BitStream containing the data to send to the User.</param>
        void Send(BitStream data);

        /// <summary>
        /// Sends data to the User. This method is thread-safe.
        /// </summary>
        /// <param name="data">BitStream containing the data to send to the User.</param>
        /// <param name="reliable">Whether or not the data should be sent over a reliable stream.</param>
        void Send(BitStream data, bool reliable);

        /// <summary>
        /// Sends data to the User.
        /// </summary>
        /// <param name="message">GameMessage to send to the User.</param>
        void Send(GameMessage message);

        /// <summary>
        /// Sends data to the User.
        /// </summary>
        /// <param name="message">GameMessage to send to the User.</param>
        /// <param name="parameters">Message parameters.</param>
        void Send(GameMessage message, params object[] parameters);
    }
}