using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Lidgren.Network;
using log4net;
using NetGore;
using NetGore.Graphics.GUI;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// The client socket manager.
    /// </summary>
    public class ClientSockets : ClientSocketManagerBase, IGetTime, ISocketSender
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static ClientSockets _instance;

        readonly IMessageProcessorManager _messageProcessorManager;
        readonly ClientPacketHandler _packetHandler;
        readonly IScreenManager _screenManager;

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

            // Try to parse as a GameMessage
            var ret = GameMessageCollection.CurrentLanguage.TryGetMessageFromString(msg);
            if (ret != null)
                return ret;

            // Could not parse
            return base.ParseCustomDisconnectMessage(msg);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSockets"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> instance.</param>
        /// <exception cref="MethodAccessException">An instance of this object has already been created.</exception>
        ClientSockets(IScreenManager screenManager) : base(GameData.NetworkAppIdentifier)
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
            return Connect(GameData.ServerIP, GameData.ServerUDPPort);
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
        /// <param name="data">The data that was received.</param>
        protected override void OnReceiveData(IIPSocket sender, BitStream data)
        {
            base.OnReceiveData(sender, data);

            // Process the received data
            _messageProcessorManager.Process(sender, data);
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

        #region ISocketSender Members

        /// <summary>
        /// Sends data to the server.
        /// </summary>
        /// <param name="data">BitStream containing the data to send.</param>
        public void Send(BitStream data)
        {
            var sock = RemoteSocket;
            if (sock == null)
            {
                const string errmsg = "Could not send data - connection not established!";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            try
            {
                sock.Send(data);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to send data. Exception: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                Debug.Fail(string.Format(errmsg, ex));
            }
        }

        /// <summary>
        /// Asynchronously sends data to the socket.
        /// </summary>
        /// <param name="data">Data to send.</param>
        public void Send(byte[] data)
        {
            var sock = RemoteSocket;
            if (sock == null)
            {
                const string errmsg = "Could not send data - connection not established!";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            try
            {
                sock.Send(data);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to send data. Exception: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                Debug.Fail(string.Format(errmsg, ex));
            }
        }

        #endregion
    }
}