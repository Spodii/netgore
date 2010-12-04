using System;

namespace DemoGame.Client
{
    public class ClientPeerTradeInfoHandlerEventArgs : EventArgs
    {
        readonly GameMessage _gameMessage;
        readonly string[] _args;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientPeerTradeInfoHandlerEventArgs"/> class.
        /// </summary>
        /// <param name="gameMessage">The<see cref="GameMessage"/>.</param>
        /// <param name="args">The the arguments for the message.</param>
        public ClientPeerTradeInfoHandlerEventArgs(GameMessage gameMessage, string[] args)
        {
            _gameMessage = gameMessage;
            _args = args;
        }

        /// <summary>
        /// Gets the <see cref="GameMessage"/>.
        /// </summary>
        public GameMessage GameMessage { get { return _gameMessage; } }

        /// <summary>
        /// Gets the arguments for the message.
        /// </summary>
        public string[] Args { get { return _args; } }
    }
}