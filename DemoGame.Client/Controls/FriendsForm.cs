using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.Stats;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    /// <summary>
    /// A <see cref="Form"/> containing the friends for a user.
    /// </summary>
    public class FriendsForm : Form
    {
        delegate string UserInfoLabelValueHandler(UserInfo userInfo);

        public static List<string> _friends;
        public static List<Friends> _myFriends;
        static Label[] FriendsCollection = new Label[50];

        int _updateTimeOut = 8000;
        TickCount _nextUpdateTime;

        readonly UserInfo _userInfo;
        ClientSockets _sockets = null;

        Button AddBtn, RemoveBtn, SendBtn;
        TextBox InputTextBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendsForm"/> class.
        /// </summary>
        /// <param name="userInfo">The user info.</param>
        /// <param name="parent">The parent.</param>
        /// <exception cref="ArgumentNullException"><paramref name="userInfo" /> is <c>null</c>.</exception>
        public FriendsForm(UserInfo userInfo, Control parent)
            : base(parent, Vector2.Zero, new Vector2(225, 400))
        {
            if (userInfo == null)
                throw new ArgumentNullException("userInfo");

            AddBtn = new Button(this, new Vector2(30, 300), new Vector2(30, 25)) { Text = "Add" };
            AddBtn.Clicked += AddBtn_Clicked;

            RemoveBtn = new Button(this, new Vector2(90, 300), new Vector2(30, 25)) { Text = "Remove" };
            RemoveBtn.Clicked += RemoveBtn_Clicked;

            SendBtn = new Button(this, new Vector2(150, 300), new Vector2(30, 25)) { Text = "Send" };
            SendBtn.Clicked += SendBtn_Clicked;

            // Create the input and output TextBoxes
            InputTextBox = new TextBox(this, new Vector2(10, 270), new Vector2(120, 25))
            {
                IsMultiLine = false,
                IsEnabled = true,
                Font = GameScreenHelper.DefaultChatFont,
                BorderColor = new Color(255, 255, 255, 100)
            };

            _userInfo = userInfo;

            for (int i = 0; i < 50; i++)
            {
                var position = new Vector2(10, (30 + 15 * i));

                FriendsCollection[i] = new Label(this, position) { Text = "", CanFocus = false, Tag = i, Font = GameScreenHelper.DefaultChatFont };
                FriendsCollection[i].MouseDown += Friend_Clicked;
            }
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Text = "Friend List";
        }


        /// <summary>
        /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
        /// not visible.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        /// 
        protected override void UpdateControl(TickCount currentTime)
        {
            base.UpdateControl(currentTime);

            if (currentTime >= _nextUpdateTime)
            {
                if (_sockets == null) _sockets = ClientSockets.Instance;

                using (var pw = ClientPacket.GetFriends())
                {
                    _sockets.Send(pw, ClientMessageType.General);
                }

                _nextUpdateTime = (TickCount)(currentTime + _updateTimeOut);
            }
        }

        /// <summary>
        /// Sorts the friendslist alphabetically with the online users on top
        /// </summary>
        public static void SortList()
        {
            if (_myFriends == null) return;

            _myFriends.Sort(delegate(Friends a, Friends b)
            {
                int xdiff = b.Online.CompareTo(a.Online);
                if (xdiff != 0) return xdiff;
                else return a.Name.CompareTo(b.Name);
            });

            UpdateFriendsList();
        }


        /// <summary>
        /// Updates the friendlist every x seconds
        /// </summary>
        private static void UpdateFriendsList()
        {
            // Sets the labels to empty.
            foreach (Label Friend in FriendsCollection)
            {
                Friend.Text = "";
            }

            int i = 0;

            if (_myFriends.Count != 0)
            {
                foreach (Friends Friend in _myFriends)
                {
                    if (Friend.Online)
                    {
                        FriendsCollection[i].ForeColor = SFML.Graphics.Color.LawnGreen;
                        FriendsCollection[i].Text = Friend.Name + " : " + Friend.Map;
                    }
                    else
                    {
                        FriendsCollection[i].ForeColor = SFML.Graphics.Color.Black;
                        FriendsCollection[i].Text = Friend.Name;
                    }
                    i++;

                }
            }
        }

        /// <summary>
        /// Adds a friend to the friendlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBtn_Clicked(object sender, MouseButtonEventArgs e)
        {
            // Check if the name is already in the friendlist if it is don t add it again.
            foreach (Friends Friend in _myFriends)
            {
                if (Friend.Name == InputTextBox.Text)
                {
                    InputTextBox.Text = "";
                    return;
                }
            }

            _myFriends.Add(new Friends { Name = InputTextBox.Text });
            InputTextBox.Text = "";
            SortList();

            SaveFriends();
        }

        /// <summary>
        /// Removes a friend from the friendlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveBtn_Clicked(object sender, MouseButtonEventArgs e)
        {
            _myFriends.RemoveAll((x) => x.Name == InputTextBox.Text);
            InputTextBox.Text = "";
            SortList();

            SaveFriends();
        }

        private void SendBtn_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (!String.IsNullOrEmpty(InputTextBox.Text))
                SendPrivateMessage();
        }

        /// <summary>
        /// Handles the Friend click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Friend_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                string Tag = ((Label)sender).Tag.ToString();
                var Friend = _myFriends[Convert.ToInt16(Tag)];
                InputTextBox.Text = Friend.Name;
            }
        }


        /// <summary>
        /// Saves the friendlist
        /// </summary>

        private void SaveFriends()
        {
            string FriendNames = "";

            // Make a string with the friend names so they can be stored in the Database.
            foreach (Friends _friend in _myFriends)
            {
                FriendNames += _friend.Name + ",";
            }

            FriendNames.TrimEnd(',');

            // Save the friends
            if (_sockets == null) _sockets = ClientSockets.Instance;

            using (var pw = ClientPacket.SaveFriends(FriendNames))
            {
                _sockets.Send(pw, ClientMessageType.General);
            }
        }

        /// <summary>
        /// Method used to send the private message to the right player
        /// </summary>
        private void SendPrivateMessage()
        {
            string[] TextMessage = InputTextBox.Text.Split(new char[] { ' ' }, 2);
            string TargetCharName = TextMessage[0];
            string Message = TextMessage[1];

            // Empty input textbox
            InputTextBox.Text = "";

            // Send the message
            if (_sockets == null) _sockets = ClientSockets.Instance;

            using (var pw = ClientPacket.SendPrivateMessage(TargetCharName, Message))
            {
                _sockets.Send(pw, ClientMessageType.General);
            }
        }
    }
}