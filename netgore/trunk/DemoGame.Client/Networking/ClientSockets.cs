using System;
using System.Linq;
using DemoGame.Client.Properties;
using Lidgren.Network;
using NetGore;
using NetGore.Graphics.GUI;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// The client socket manager.
    /// </summary>
    public class ClientSockets : ClientSocketManager, IGetTime
    {
        static ClientSockets _instance;

        readonly IMessageProcessorManager _messageProcessorManager;
        readonly ClientPacketHandler _packetHandler;
        readonly IScreenManager _screenManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSockets"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> instance.</param>
        /// <exception cref="MethodAccessException">An instance of this object has already been created.</exception>
        ClientSockets(IScreenManager screenManager) : base(CommonConfig.NetworkAppIdentifier)
        {
            if (_instance != null)
                throw new MethodAccessException("ClientSockets instance was already created. Use that instead.");

            _screenManager = screenManager;
            _packetHandler = new ClientPacketHandler(this, ScreenManager, DynamicEntityFactory.Instance);

            // When debugging, use the StatMessageProcessorManager instead (same thing as the other, but provides network statistics)
#if DEBUG
            var m = new StatMessageProcessorManager(_packetHandler, EnumHelper<ServerPacketID>.BitsRequired);
            m.Stats.EnableFileOutput(ContentPaths.Build.Root.Join("netstats_in" + EngineSettings.DataFileSuffix));
            _messageProcessorManager = m;
#else
            _messageProcessorManager = new MessageProcessorManager(_packetHandler, EnumHelper<ServerPacketID>.BitsRequired);
#endif
        }

        /// <summary>
        /// Gets the last <see cref="ClientSockets"/> instance. Will be null if it has not been created yet.
        /// </summary>
        public static ClientSockets Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the ClientPacketHandler used to handle data from this ClientSockets.
        /// </summary>
        public ClientPacketHandler PacketHandler
        {
            get { return _packetHandler; }
        }

        public IScreenManager ScreenManager
        {
            get { return _screenManager; }
        }

        /// <summary>
        /// Attempts to connect to the server using the default server address.
        /// </summary>
        /// <returns>
        /// True if the connection attempt was successfully started. Does not mean that the connection was established, but
        /// just that it can be attempted. Will return false if a connection is already established or being established.</returns>
        public bool Connect()
        {
            return Connect(ClientSettings.Default.Network_ServerIP, CommonConfig.ServerPort);
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional configuring of the <see cref="NetPeerConfiguration"/> instance
        /// that will be used for this <see cref="ClientSocketManager"/>.
        /// </summary>
        protected override void InitNetPeerConfig(NetPeerConfiguration config)
        {
            base.InitNetPeerConfig(config);

            config.PingInterval = CommonConfig.PingInterval;
            config.ConnectionTimeout = CommonConfig.ConnectionTimeout;
        }

        /// <summary>
        /// Initializes the <see cref="ClientSockets"/> instance. This only needs to be called once.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> instance.</param>
        /// <exception cref="ArgumentNullException"><see cref="screenManager"/> is null.</exception>
        public static void Initialize(IScreenManager screenManager)
        {
            if (screenManager == null)
                throw new ArgumentNullException("screenManager");

            if (Instance != null)
                return;

            _instance = new ClientSockets(screenManager);
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling received data from an <see cref="IIPSocket"/>.
        /// </summary>
        /// <param name="sender">The <see cref="IIPSocket"/> that the data came from.</param>
        /// <param name="data">The data that was received. This <see cref="BitStream"/> instance is reused internally, so it
        /// is vital that you do NOT hold a reference to it when this method returns. This should be no problem since you should
        /// not be holding onto raw received data anyways, but if you must, you can always make a deep copy.</param>
        protected override void OnReceiveData(IIPSocket sender, BitStream data)
        {
            base.OnReceiveData(sender, data);

            // Process the received data
            _messageProcessorManager.Process(sender, data);
        }

        /// <summary>
        /// When overridden in the derived class, parses the string sent from the server containing the reason why we
        /// were disconnected. The message is specified when the server calls <see cref="IIPSocket.Disconnect"/>
        /// with parameters.
        /// </summary>
        /// <param name="msg">The message that was sent containing the reason for disconnecting. Can be empty if the
        /// server gave no reason.</param>
        /// <returns>The parsed <paramref name="msg"/>, or the original <paramref name="msg"/> if no known way to parse
        /// it was found.</returns>
        protected override string ParseCustomDisconnectMessage(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                return base.ParseCustomDisconnectMessage(msg);

            // Check for special messages defined by the network library
            if (msg.StartsWith("Timed out", StringComparison.OrdinalIgnoreCase))
                return GameMessageCollection.CurrentLanguage.GetMessage(GameMessage.DisconnectTimedOut);
            if (msg.StartsWith("Failed to complete handshake", StringComparison.OrdinalIgnoreCase))
                return GameMessageCollection.CurrentLanguage.GetMessage(GameMessage.DisconnectNoReasonSpecified);
            if (msg.StartsWith("Connection was reset by remote host", StringComparison.OrdinalIgnoreCase))
                return GameMessageCollection.CurrentLanguage.GetMessage(GameMessage.DisconnectNoReasonSpecified);
            if (msg.StartsWith("Connection forcibly closed", StringComparison.OrdinalIgnoreCase))
                return GameMessageCollection.CurrentLanguage.GetMessage(GameMessage.DisconnectNoReasonSpecified);

            // Try to parse as a GameMessage
            var ret = GameMessageCollection.CurrentLanguage.TryGetMessageFromString(msg);
            if (ret != null)
                return ret;

            // Could not parse
            ret = base.ParseCustomDisconnectMessage(msg);
            return ret;
        }

        #region IGetTime Members

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Current time.</returns>
        public TickCount GetTime()
        {
            return _packetHandler.GetTime();
        }

        #endregion
    }
}