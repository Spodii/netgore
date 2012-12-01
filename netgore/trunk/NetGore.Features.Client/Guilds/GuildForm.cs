using System;
using System.Linq;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// A <see cref="Form"/> that displays information about the client's guild.
    /// </summary>
    public class GuildForm : GuildInfoFormBase
    {
        Button _btnJoinLeave;
        Button _btnMembers;
        Button _btnOnline;
        GuildMembersForm _frmMembers;
        GuildOnlineMembersForm _frmOnline;
        bool _lastIsInGuild;
        Label _lblName;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildForm"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public GuildForm(Control parent, Vector2 position) : base(parent, position, new Vector2(125, 275))
        {
            IsVisible = false;
            CreateControls();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildForm"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        public GuildForm(IGUIManager guiManager, Vector2 position) : base(guiManager, position, new Vector2(125, 275))
        {
            IsVisible = false;
            CreateControls();
        }

        /// <summary>
        /// Notifies listeners when a request to join a guild has been created from this <see cref="GuildForm"/>.
        /// </summary>
        public event TypedEventHandler<GuildForm> JoinRequested;

        /// <summary>
        /// Notifies listeners when a request to leave a guild has been created from this <see cref="GuildForm"/>.
        /// </summary>
        public event TypedEventHandler<GuildForm> LeaveRequested;

        void CreateControls()
        {
            var buttonSize = new Vector2(100, 20);

            _lblName = new Label(this, Vector2.Zero);

            _btnJoinLeave = new Button(this, Vector2.Zero, buttonSize);
            _btnMembers = new Button(this, Vector2.Zero, buttonSize) { Text = "Members" };
            _btnOnline = new Button(this, Vector2.Zero, buttonSize) { Text = "Online" };

            _btnJoinLeave.Clicked += _btnJoinLeave_Clicked;
            _btnMembers.Clicked += btnMembers_Clicked;
            _btnOnline.Clicked += btnOnline_Clicked;

            var formSize = new Vector2(200, 200);
            _frmOnline = new GuildOnlineMembersForm(Parent, new Vector2(200, 200), formSize) { GuildInfo = GuildInfo, IsVisible = false };
            _frmMembers = new GuildMembersForm(Parent, new Vector2(250, 250), formSize) { GuildInfo = GuildInfo, IsVisible = false };

            RelocateControls();
        }

        /// <summary>
        /// Draws the Control.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        protected override void DrawControl(ISpriteBatch spriteBatch)
        {
            var iig = IsInGuild;
            if (iig != _lastIsInGuild)
            {
                UpdateGuildStatus();
                _lastIsInGuild = iig;
            }

            base.DrawControl(spriteBatch);
        }

        static Vector2 GetPositionBelow(Control control)
        {
            return control.Position + new Vector2(0, control.Size.Y + 4);
        }

        /// <summary>
        /// When overridden in the derived class, loads the skinning information for the <see cref="Control"/>
        /// from the given <paramref name="skinManager"/>.
        /// </summary>
        /// <param name="skinManager">The <see cref="ISkinManager"/> to load the skinning information from.</param>
        public override void LoadSkin(ISkinManager skinManager)
        {
            base.LoadSkin(skinManager);

            RelocateControls();
        }

        /// <summary>
        /// Handles when the <see cref="Control.Border"/> has changed.
        /// This is called immediately before <see cref="Control.BorderChanged"/>.
        /// Override this method instead of using an event hook on <see cref="Control.BorderChanged"/> when possible.
        /// </summary>
        protected override void OnBorderChanged()
        {
            base.OnBorderChanged();

            RelocateControls();
        }

        /// <summary>
        /// When overridden in the derived class, handles when the <see cref="UserGuildInformation"/> changes.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        protected override void OnGuildInfoChanged(UserGuildInformation newValue, UserGuildInformation oldValue)
        {
            base.OnGuildInfoChanged(newValue, oldValue);

            if (_frmOnline != null)
            {
                _frmOnline.GuildInfo = newValue;
                _frmMembers.GuildInfo = newValue;
            }

            UpdateGuildStatus();

            if (oldValue != null)
            {
                oldValue.GuildChanged -= UserGuildInfo_GuildChanged;
            }

            if (newValue != null)
            {
                newValue.GuildChanged -= UserGuildInfo_GuildChanged;
                newValue.GuildChanged += UserGuildInfo_GuildChanged;
            }
        }

        /// <summary>
        /// Handles when the <see cref="Control.Size"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.Resized"/>.
        /// Override this method instead of using an event hook on <see cref="Control.Resized"/> when possible.
        /// </summary>
        protected override void OnResized()
        {
            base.OnResized();

            RelocateControls();
        }

        void RelocateControls()
        {
            if (_lblName == null)
            {
                CreateControls();
                return;
            }

            _lblName.Position = Vector2.Zero;
            _btnJoinLeave.Position = GetPositionBelow(_lblName);
            _btnMembers.Position = GetPositionBelow(_btnJoinLeave);
            _btnOnline.Position = GetPositionBelow(_btnMembers);
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Text = "Guild";
        }

        void UpdateGuildStatus()
        {
            if (IsInGuild)
            {
                _lblName.Text = GuildInfo.Name + " [" + GuildInfo.Tag + "]";
                _btnJoinLeave.Text = "Leave guild";
                _btnOnline.IsEnabled = true;
                _btnMembers.IsEnabled = true;
            }
            else
            {
                _lblName.Text = "Not in a guild";
                _btnJoinLeave.Text = "Join guild";
                _btnOnline.IsEnabled = false;
                _btnMembers.IsEnabled = false;
            }
        }

        /// <summary>
        /// Handles the corresponding event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void UserGuildInfo_GuildChanged(UserGuildInformation sender, EventArgs e)
        {
            UpdateGuildStatus();
        }

        /// <summary>
        /// Handles the Clicked event of the _btnJoinLeave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        void _btnJoinLeave_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (IsInGuild)
            {
                // Leave guild
                if (LeaveRequested != null)
                    LeaveRequested.Raise(this, EventArgs.Empty);
            }
            else
            {
                // Join guild
                if (JoinRequested != null)
                    JoinRequested.Raise(this, EventArgs.Empty);
            }
        }

        void btnMembers_Clicked(object sender, MouseButtonEventArgs e)
        {
            _frmMembers.IsVisible = !_frmMembers.IsVisible;
        }

        void btnOnline_Clicked(object sender, MouseButtonEventArgs e)
        {
            _frmOnline.IsVisible = !_frmOnline.IsVisible;
        }
    }
}