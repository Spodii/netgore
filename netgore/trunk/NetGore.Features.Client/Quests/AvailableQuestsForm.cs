using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.World;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Features.Quests
{
    public class AvailableQuestsForm : Form
    {
        const int _padding = 3;
        const int _padding2 = _padding * 2;

        static readonly IQuestDescription[] _emptyQuests = Enumerable.Empty<IQuestDescription>().ToArray();
        static readonly object _eventQuestAccepted = new object();

        readonly Func<QuestID, bool> _hasFinishQuestReqs;
        readonly Func<QuestID, bool> _hasStartQuestReqs;

        Button _btnAccept;
        Button _btnClose;
        int _lastItemsListSize = 0;
        Label _lblAvailableQuests;
        Label _lblQuestInfo;
        QuestDescriptionListBox _lstQuests;
        TextBox _txtQuestInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailableQuestsForm"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <param name="hasStartQuestReqs">A func used to check if the user has the requirements to start a quest.</param>
        /// <param name="hasFinishQuestReqs">A func used to check if the user has the requirements to finish a quest.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public AvailableQuestsForm(Control parent, Vector2 position, Vector2 clientSize, Func<QuestID, bool> hasStartQuestReqs, Func<QuestID, bool> hasFinishQuestReqs) 
            : base(parent, position, clientSize)
        {
            _hasStartQuestReqs = hasStartQuestReqs;
            _hasFinishQuestReqs = hasFinishQuestReqs;

            CreateChildren();
            IsVisible = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailableQuestsForm"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <param name="hasStartQuestReqs">A func used to check if the user has the requirements to start a quest.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        public AvailableQuestsForm(IGUIManager guiManager, Vector2 position, Vector2 clientSize, Func<QuestID, bool> hasStartQuestReqs) 
            : base(guiManager, position, clientSize)
        {
            _hasStartQuestReqs = hasStartQuestReqs;

            CreateChildren();
            IsVisible = false;
        }

        /// <summary>
        /// Notifies listeners when a quest has been clicked to be accepted.
        /// </summary>
        public event TypedEventHandler<Control, EventArgs<IQuestDescription>> QuestAccepted
        {
            add { Events.AddHandler(_eventQuestAccepted, value); }
            remove { Events.RemoveHandler(_eventQuestAccepted, value); }
        }

        /// <summary>
        /// Gets or sets the available quests to display.
        /// </summary>
        public IList<IQuestDescription> AvailableQuests
        {
            get { return _lstQuests.Items; }
            set
            {
                if (value == null)
                    value = _emptyQuests;

                if (AvailableQuests == value)
                    return;

                _lstQuests.Items = value;
            }
        }

        /// <summary>
        /// Gets or sets the index of the entity providing the quests displayed by this form.
        /// </summary>
        public MapEntityIndex QuestProviderIndex { get; set; }

        /// <summary>
        /// Creates the children controls for this form.
        /// </summary>
        void CreateChildren()
        {
            _lblAvailableQuests = new Label(this, Vector2.Zero) { Text = "Available Quests:" };

            _lstQuests = new QuestDescriptionListBox(this, Vector2.Zero, new Vector2(32), _hasStartQuestReqs, _hasFinishQuestReqs) 
            { Items = _emptyQuests, ShowPaging = false };
            _lstQuests.SelectedIndexChanged += lstQuests_SelectedIndexChanged;

            _lblQuestInfo = new Label(this, Vector2.Zero) { Text = "Quest Information:" };

            _txtQuestInfo = new TextBox(this, Vector2.Zero, new Vector2(32)) { IsMultiLine = true, IsEnabled = false };

            _btnAccept = new Button(this, Vector2.Zero, new Vector2(90, 18)) { Text = "Accept Quest" };
            _btnAccept.Clicked += btnAccept_Clicked;

            _btnClose = new Button(this, Vector2.Zero, new Vector2(60, 18)) { Text = "Close" };
            _btnClose.Clicked += btnClose_Clicked;

            RepositionChildren();
        }

        /// <summary>
        /// Displays this form using the given <see cref="QuestID"/>s and sets the focus.
        /// </summary>
        /// <param name="availableQuests">The available quest IDs.</param>
        /// <param name="questProviderIndex">The index of the entity providing the quests.</param>
        public void Display(IList<IQuestDescription> availableQuests, MapEntityIndex questProviderIndex)
        {
            AvailableQuests = availableQuests;
            QuestProviderIndex = questProviderIndex;
            IsVisible = true;
            SetFocus();
        }

        /// <summary>
        /// Draws the Control.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        protected override void DrawControl(ISpriteBatch spriteBatch)
        {
            // Check if we need to update the size of the items list
            if (_lstQuests.Items.Count() != _lastItemsListSize)
                RepositionChildren();

            base.DrawControl(spriteBatch);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="questDescription">The quest description.</param>
        void InvokeQuestAccepted(IQuestDescription questDescription)
        {
            OnQuestAccepted(questDescription);
            var handler = Events[_eventQuestAccepted] as TypedEventHandler<Control, EventArgs<IQuestDescription>>;
            if (handler != null)
                handler(this, EventArgsHelper.Create(questDescription));
        }

        /// <summary>
        /// Handles when the <see cref="Control.Border"/> has changed.
        /// This is called immediately before <see cref="Control.BorderChanged"/>.
        /// Override this method instead of using an event hook on <see cref="Control.BorderChanged"/> when possible.
        /// </summary>
        protected override void OnBorderChanged()
        {
            base.OnBorderChanged();

            RepositionChildren();
        }

        /// <summary>
        /// Handles when a quest has been clicked to be accepted.
        /// This is called immediately before <see cref="AvailableQuestsForm.QuestAccepted"/>.
        /// Override this method instead of using an event hook on <see cref="AvailableQuestsForm.QuestAccepted"/> when possible.
        /// </summary>
        /// <param name="questDescription">The quest description.</param>
        protected virtual void OnQuestAccepted(IQuestDescription questDescription)
        {
        }

        /// <summary>
        /// Handles when the <see cref="Control.Size"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.Resized"/>.
        /// Override this method instead of using an event hook on <see cref="Control.Resized"/> when possible.
        /// </summary>
        protected override void OnResized()
        {
            base.OnResized();

            RepositionChildren();
        }

        /// <summary>
        /// Updates the position of all the children controls.
        /// </summary>
        void RepositionChildren()
        {
            // Ensure the controls have been created first
            if (_lblAvailableQuests == null)
                return;

            _lastItemsListSize = _lstQuests.Items.Count();

            var cs = ClientSize;

            _lblAvailableQuests.Position = new Vector2(_padding);

            _lstQuests.Position = new Vector2(_padding, _lblAvailableQuests.Position.Y + _lblAvailableQuests.Size.Y + _padding);
            _lstQuests.ClientSize = new Vector2(cs.X - _lstQuests.Border.Width - _padding2,
                (Math.Max(1, _lastItemsListSize) * _lstQuests.ItemHeight) + _lstQuests.Border.Height + 5);

            Debug.Assert(_lstQuests.ItemsPerPage >= _lastItemsListSize);

            _lblQuestInfo.Position = new Vector2(_padding, _lstQuests.Position.Y + _lstQuests.Size.Y + _padding2);

            _btnAccept.Position = cs - _btnAccept.Size - new Vector2(_padding);
            _btnClose.Position = _btnAccept.Position - new Vector2(_padding + _btnClose.Size.X, 0);

            var p = new Vector2(_padding, _lblQuestInfo.Position.Y + _lblQuestInfo.Size.Y + _padding);
            _txtQuestInfo.Size = new Vector2(cs.X - _padding2, Math.Max(16, _btnAccept.Position.Y - p.Y - _padding));
            _txtQuestInfo.Position = p;
        }

        /// <summary>
        /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
        /// not visible.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected override void UpdateControl(TickCount currentTime)
        {
            base.UpdateControl(currentTime);

            // If there are no quests listed, close the form
            if (AvailableQuests.IsEmpty())
                IsVisible = false;
        }

        /// <summary>
        /// Handles the Clicked event of the <see cref="_btnAccept"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        void btnAccept_Clicked(object sender, MouseButtonEventArgs e)
        {
            var selItem = _lstQuests.SelectedItem;
            if (selItem == null)
                return;

            InvokeQuestAccepted(selItem);
        }

        /// <summary>
        /// Handles the Clicked event of the <see cref="_btnClose"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        void btnClose_Clicked(object sender, MouseButtonEventArgs e)
        {
            IsVisible = false;
        }

        /// <summary>
        /// Handles when the available quests list's index changes.
        /// </summary>
        void lstQuests_SelectedIndexChanged(Control sender, EventArgs e)
        {
            // Get the selected item
            var selectedItem = _lstQuests.SelectedItem;

            var acceptButtonText = "Accept";

            if (selectedItem == null)
            {
                _txtQuestInfo.Text = string.Empty;
                _btnAccept.IsEnabled = false;
            }
            else
            {
                _txtQuestInfo.Text = selectedItem.Description;
                _btnAccept.IsEnabled = true;

                if (_hasFinishQuestReqs(selectedItem.QuestID))
                    acceptButtonText = "Turn in!";
            }

            _btnAccept.Text = acceptButtonText;
        }
    }
}