using System;
using System.Collections.Generic;
using NetGore;
using NetGore.Graphics.GUI;
using SFML.Graphics;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// A <see cref="Form"/> containing all the users.
    /// </summary>
    public class OnlineUsersForm : Form
    {
        static ListBox<String> _lbOnlineUsers;
        public static List<String> Online;
       

        readonly ClientSockets _sockets = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnlineUsersForm"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <exception cref="ArgumentNullException"><paramref name="userInfo" /> is <c>null</c>.</exception>
        public OnlineUsersForm(Control parent) : base(parent, Vector2.Zero, new Vector2(225, 400))
        {
            _sockets = ClientSockets.Instance;

            this.VisibleChanged -= OnlineUsersForm_VisibleChanged;
            this.VisibleChanged += OnlineUsersForm_VisibleChanged;

            Online = new List<String>();

            var items = new List<String>();
            for (var i = 0; i < (Online.Count); i++)
            {
                items.Add(i.ToString());
            }

            _lbOnlineUsers = new ListBox<String>(this, new Vector2(20, 20), this.ClientSize - new Vector2(50, 50)) { Items = items, ShowPaging = true, CanSelect = true };
        }

        private void OnlineUsersForm_VisibleChanged(Control sender, EventArgs e)
        {
            if (Sockets == null || !Sockets.IsConnected)
                return;

            using (var pw = ClientPacket.GetOnlineUsers())
            {
                _sockets.Send(pw, ClientMessageType.GUI);
            }

            if (this.IsVisible)
            {
                Text = (_lbOnlineUsers == null ? "Users Online" : "Users Online: " + _lbOnlineUsers.Items.Count);
                UpdateUsersList();
            }
        }

        /// <summary>
        /// Gets the <see cref="IClientSocketManager"/> instance used to let the client communicate with the server.
        /// </summary>
        public IClientSocketManager Sockets
        {
            get { return _sockets; }
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierarchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();
        }

        /// <summary>
        /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
        /// not visible.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected override void UpdateControl(TickCount currentTime)
        {
            base.UpdateControl(currentTime);
        }

        /// <summary>
        /// Updates the users list
        /// </summary>
        public static void UpdateUsersList()
        {
            if (Online == null)
                return;

            // Clear the users list
            _lbOnlineUsers.Items.Clear();

            if (Online != null || !Online.IsEmpty())
            {
                foreach (string user in Online)
                {
                    _lbOnlineUsers.Items.Add(user);
                }
            }
        }
    }
}