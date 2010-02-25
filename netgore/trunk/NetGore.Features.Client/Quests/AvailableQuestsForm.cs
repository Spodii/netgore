using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics.GUI;

namespace NetGore.Features.Quests
{
    public class AvailableQuestsForm : Form
    {
        const int _padding = 3;
        const int _padding2 = _padding * 2;
        static readonly IEnumerable<IQuestDescription> _emptyQuests = Enumerable.Empty<IQuestDescription>();

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
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public AvailableQuestsForm(Control parent, Vector2 position, Vector2 clientSize) : base(parent, position, clientSize)
        {
            CreateChildren();
            IsVisible = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailableQuestsForm"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        public AvailableQuestsForm(IGUIManager guiManager, Vector2 position, Vector2 clientSize)
            : base(guiManager, position, clientSize)
        {
            CreateChildren();
            IsVisible = false;
        }

        /// <summary>
        /// Displays this form using the given <see cref="QuestID"/>s and sets the focus.
        /// </summary>
        /// <param name="availableQuests">The available quest IDs.</param>
        public void Display(IEnumerable<IQuestDescription> availableQuests)
        {
            AvailableQuests = availableQuests;
            IsVisible = true;
            SetFocus();
        }

        /// <summary>
        /// Gets or sets the available quests to display.
        /// </summary>
        public IEnumerable<IQuestDescription> AvailableQuests
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
        /// Creates the children controls for this form.
        /// </summary>
        void CreateChildren()
        {
            _lblAvailableQuests = new Label(this, Vector2.Zero) { Text = "Available Quests:" };
            _lstQuests = new QuestDescriptionListBox(this, Vector2.Zero, new Vector2(32)) { Items = _emptyQuests, ShowPaging = false };
            _lblQuestInfo = new Label(this, Vector2.Zero) { Text = "Quest Information:" };
            _txtQuestInfo = new TextBox(this, Vector2.Zero, new Vector2(32)) { IsMultiLine = true, IsEnabled = false };

            RepositionChildren();
        }

        /// <summary>
        /// Draws the Control.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw to.</param>
        protected override void DrawControl(SpriteBatch spriteBatch)
        {
            // Check if we need to update the size of the items list
            if (_lstQuests.Items.Count() != _lastItemsListSize)
                RepositionChildren();

            base.DrawControl(spriteBatch);
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
            _lstQuests.ClientSize = new Vector2(cs.X - _lstQuests.Border.Width - _padding2, (Math.Min(1, _lastItemsListSize) * _lstQuests.ItemHeight) + _lstQuests.Border.Height + 5);

            _lblQuestInfo.Position = new Vector2(_padding, _lstQuests.Position.Y + _lstQuests.Size.Y + _padding2);

            var p = new Vector2(_padding, _lblQuestInfo.Position.Y + _lblQuestInfo.Size.Y + _padding);
            _txtQuestInfo.Size = new Vector2(cs.X - _padding2, Math.Max(16, cs.Y - p.Y - _padding));
            _txtQuestInfo.Position = p;
        }
    }
}