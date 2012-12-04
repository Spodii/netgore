using System;
using System.Linq;
using NetGore.Graphics.GUI;
using SFML.Graphics;

namespace NetGore.Features.Groups
{
    public class GroupForm : Form
    {
        readonly GroupMemberListControl _memberList;

        UserGroupInformation _groupInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupForm"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public GroupForm(Control parent, Vector2 position, Vector2 clientSize) : base(parent, position, clientSize)
        {
            _memberList = new GroupMemberListControl(this, position, clientSize);
            IsVisible = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupForm"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        public GroupForm(IGUIManager guiManager, Vector2 position, Vector2 clientSize) : base(guiManager, position, clientSize)
        {
            _memberList = new GroupMemberListControl(this, position, clientSize);
            IsVisible = false;
        }

        public UserGroupInformation GroupInfo
        {
            get { return _groupInfo; }
            set
            {
                if (_groupInfo == value)
                    return;

                if (_groupInfo != null)
                    _groupInfo.GroupChanged -= GroupInfo_GroupChanged;

                _groupInfo = value;

                if (_groupInfo != null)
                {
                    _memberList.Items = _groupInfo.Members;
                    _groupInfo.GroupChanged += GroupInfo_GroupChanged;
                }
                else
                {
                    _memberList.Items = new string[0];
                }
            }
        }

        /// <summary>
        /// Handles the corresponding event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void GroupInfo_GroupChanged(UserGroupInformation sender, EventArgs e)
        {
            IsVisible = sender.IsInGroup;
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Text = "Group Members";
            IsVisible = false;
            if (GroupInfo != null)
                IsVisible = GroupInfo.IsInGroup;
        }
    }
}