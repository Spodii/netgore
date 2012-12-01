using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A bubble of text that appears near an <see cref="Entity"/> and is used to denote chatting text.
    /// </summary>
    public class ChatBubble : Form
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly Dictionary<Entity, ChatBubble> _chatBubbles = new Dictionary<Entity, ChatBubble>();
        static readonly object _chatBubblesSync = new object();

        readonly Entity _owner;
        readonly ChatBubbleText _textControl;

        TickCount _deathTime;

        /// <summary>
        /// Initializes the <see cref="ChatBubble"/> class.
        /// </summary>
        static ChatBubble()
        {
            Lifespan = 5000;
            MaxChatBubbleWidth = 128;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatBubble"/> class.
        /// </summary>
        /// <param name="parent">The parent <see cref="Control"/>.</param>
        /// <param name="owner">The <see cref="Entity"/> that this <see cref="ChatBubble"/> is for.</param>
        /// <param name="text">The text to display.</param>
        /// <exception cref="ArgumentNullException"><paramref name="owner" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="text" /> is <c>null</c>.</exception>
        public ChatBubble(Control parent, Entity owner, string text) : base(parent, Vector2.Zero, Vector2.Zero)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("text");

            _owner = owner;
            _deathTime = (TickCount)(TickCount.Now + Lifespan);
            _textControl = new ChatBubbleText(this, text) { Font = Font };

            _owner.Disposed -= ChatBubble_Owner_Disposed;
            _owner.Disposed += ChatBubble_Owner_Disposed;
        }

        /// <summary>
        /// Gets the text displayed in this chat bubble.
        /// </summary>
        public string ChatText
        {
            get
            {
                if (_textControl == null)
                    return string.Empty;
                else
                    return _textControl.Text;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Func{T1,T2,T3,TResult}"/> used to create a <see cref="ChatBubble"/> instance. If null, the default
        /// method will be used to create a <see cref="ChatBubble"/> instance. Can be manually set to create a derived class instance.
        /// The arguments are 1. the parent control, 2. the owner entity and 3. the chat bubble text.
        /// </summary>
        public static Func<Control, Entity, string, ChatBubble> CreateChatBubbleInstance { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Func{T,TResult}"/> used to find the screen position of the top-left corner of a <see cref="ISpatial"/>
        /// for determining where to draw a <see cref="ChatBubble"/>.
        /// </summary>
        public static Func<ISpatial, Vector2> GetTopLeftCornerHandler { get; set; }

        /// <summary>
        /// Gets or sets the lifespan for <see cref="ChatBubble"/>s in milliseconds. Default value is 5000 (5 seconds).
        /// </summary>
        public static int Lifespan { get; set; }

        /// <summary>
        /// Gets or sets the maximum width of a chat bubble in pixels. Default is 128.
        /// </summary>
        public static ushort MaxChatBubbleWidth { get; set; }

        /// <summary>
        /// Gets the <see cref="Entity"/> that this <see cref="ChatBubble"/> is attached to.
        /// </summary>
        public Entity Owner
        {
            get { return _owner; }
        }

        /// <summary>
        /// Handles when the <see cref="Owner"/> of the <see cref="ChatBubble"/> is disposed.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> that was disposed.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void ChatBubble_Owner_Disposed(Entity entity, EventArgs e)
        {
            Debug.Assert(entity == Owner);

            // When the owner is disposed, dispose of the chat bubble, too
            Dispose();
        }

        /// <summary>
        /// Clears all <see cref="ChatBubble"/> instances that were created with <see cref="ChatBubble.Create"/>.
        /// </summary>
        public static void ClearAll()
        {
            IEnumerable<ChatBubble> toDispose;
            lock (_chatBubblesSync)
            {
                toDispose = _chatBubbles.Values.ToImmutable();
            }

            foreach (var cb in toDispose)
            {
                cb.Dispose();
            }
        }

        /// <summary>
        /// Creates a <see cref="ChatBubble"/> instance.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="text">The text.</param>
        /// <returns>The <see cref="ChatBubble"/> instance, or null if any <see cref="Exception"/>s errors occured
        /// while trying to make the <see cref="ChatBubble"/> or any of the supplied parameters were invalid.</returns>
        public static ChatBubble Create(Control parent, Entity owner, string text)
        {
            if (parent == null)
            {
                const string errmsg = "ChatBubble.Create() failed - parent was null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return null;
            }

            if (owner == null)
            {
                const string errmsg = "ChatBubble.Create() failed - owner was null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return null;
            }

            if (string.IsNullOrEmpty(text))
            {
                const string errmsg = "ChatBubble.Create() failed - text was null or empty.";
                if (log.IsWarnEnabled)
                    log.Warn(errmsg);
                return null;
            }

            lock (_chatBubblesSync)
            {
                ChatBubble c = null;

                try
                {
                    // If the ChatBubble already exists for the given Entity, reuse that one
                    if (_chatBubbles.TryGetValue(owner, out c) && c != null)
                    {
                        c._textControl.ChangeTextAndResize(text);
                        c._deathTime = (TickCount)(TickCount.Now + Lifespan);
                        c.Update(TickCount.Now);
                        return c;
                    }

                    // Create a new ChatBubble
                    if (CreateChatBubbleInstance != null)
                        c = CreateChatBubbleInstance(parent, owner, text);
                    else
                        c = new ChatBubble(parent, owner, text);

                    c.Update(TickCount.Now);
                }
                catch (Exception ex)
                {
                    const string errmsg = "Error attempting to create ChatBubble for entity `{0}` with text `{1}`. Exception: {2}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, owner, text, ex);
                    Debug.Fail(string.Format(errmsg, owner, text, ex));
                }

                if (c == null)
                    return null;

                _chatBubbles.Add(owner, c);

                return c;
            }
        }

        /// <summary>
        /// Disposes of the Control and all its resources.
        /// </summary>
        /// <param name="disposeManaged">If true, managed resources need to be disposed. If false,
        /// this was raised by a destructor which means the managed resources are already disposed.</param>
        protected override void Dispose(bool disposeManaged)
        {
            Debug.Assert(disposeManaged, "Why was our ChatBubble garbage collected?");

            if (disposeManaged)
            {
                if (Owner == null)
                {
                    const string errmsg = "ChatBubble `{0}` has no owner.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, this);
                    Debug.Fail(string.Format(errmsg, this));
                }
                else
                {
                    // Remove the bubble from the dictionary
                    lock (_chatBubblesSync)
                    {
                        if (!_chatBubbles.Remove(Owner))
                        {
                            const string errmsg =
                                "Tried to remove ChatBubble `{0}` for entity `{1}`, but it was already gone from the collection...?";
                            if (log.IsErrorEnabled)
                                log.ErrorFormat(errmsg, this, Owner);
                            Debug.Fail(string.Format(errmsg, this, Owner));
                        }
                    }
                }
            }

            base.Dispose(disposeManaged);
        }

        /// <summary>
        /// Gets the position to use to draw the <see cref="ChatBubble"/>.
        /// </summary>
        /// <returns>The position to use to draw the <see cref="ChatBubble"/>.</returns>
        protected virtual Vector2 GetDrawOffset()
        {
            if (Owner == null)
            {
                const string errmsg = "ChatBubble `{0}`'s owner is null.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
                return Vector2.Zero;
            }

            // Get the top-left corner (using the default implementation if needed)
            Vector2 p;
            if (GetTopLeftCornerHandler != null)
                p = GetTopLeftCornerHandler(Owner);
            else
                p = GetTopLeftCorner(Owner);

            // Move to the center of the Character
            p.X += Owner.Size.X / 2f;
            p -= Size / 2f;

            // Move up a little to avoid covering their name
            p.Y -= 35f;

            return p.Floor();
        }

        /// <summary>
        /// Gets the top-left corner to use for drawing for the given <paramref name="target"/>.
        /// </summary>
        /// <param name="target">The <see cref="ISpatial"/> to attach the bubble to.</param>
        /// <returns>The coordinate of the top-left corner of the <paramref name="target"/> to use for drawing.</returns>
        static Vector2 GetTopLeftCorner(ISpatial target)
        {
            return target.Position;
        }

        /// <summary>
        /// When overridden in the derived class, loads the skinning information for the <see cref="Control"/>
        /// from the given <paramref name="skinManager"/>.
        /// </summary>
        /// <param name="skinManager">The <see cref="ISkinManager"/> to load the skinning information from.</param>
        public override void LoadSkin(ISkinManager skinManager)
        {
            Border = skinManager.GetBorder("Chat Bubble");
        }

        /// <summary>
        /// Handles when the <see cref="TextControl.Font"/> has changed.
        /// This is called immediately before <see cref="TextControl.FontChanged"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.FontChanged"/> when possible.
        /// </summary>
        protected override void OnFontChanged()
        {
            base.OnFontChanged();

            if (_textControl != null)
                _textControl.Font = Font;
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            CanDrag = false;
            CanFocus = false;
            ResizeToChildren = true;
            ResizeToChildrenPadding = 1;
            IsCloseButtonVisible = false;
            Text = string.Empty;
            IsVisible = true;
        }

        /// <summary>
        /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
        /// not visible.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected override void UpdateControl(TickCount currentTime)
        {
            if (IsDisposed)
                return;

            if (_deathTime <= currentTime)
            {
                Dispose();
                return;
            }

            Position = GetDrawOffset();

            base.UpdateControl(currentTime);
        }

        /// <summary>
        /// A control that displays the text area for a <see cref="ChatBubble"/>.
        /// </summary>
        sealed class ChatBubbleText : TextBox
        {
            /// <summary>
            /// The initial height of the textbox. Doesn't really matter too much what the value is.
            /// </summary>
            const int _initialHeight = 256;

            /// <summary>
            /// Initializes a new instance of the <see cref="ChatBubbleText"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="text">The text.</param>
            public ChatBubbleText(Control parent, string text)
                : base(parent, Vector2.Zero, new Vector2(MaxChatBubbleWidth, _initialHeight))
            {
                IsMultiLine = true;

                ChangeTextAndResize(text);
            }

            /// <summary>
            /// Changes the control's text and properly resizes it to fit the text. Use this instead of setting the
            /// <see cref="Text"/> property when resizing.
            /// </summary>
            public void ChangeTextAndResize(string newText)
            {
                if (Text == newText)
                    return;

                Clear();
                ClientSize = new Vector2(MaxChatBubbleWidth, _initialHeight);
                Text = newText;
                ResizeToFitText();
            }

            /// <summary>
            /// When overridden in the derived class, loads the skinning information for the <see cref="Control"/>
            /// from the given <paramref name="skinManager"/>.
            /// </summary>
            /// <param name="skinManager">The <see cref="ISkinManager"/> to load the skinning information from.</param>
            public override void LoadSkin(ISkinManager skinManager)
            {
                // Do not skin the ChatBubbleText
            }

            /// <summary>
            /// Handles when the <see cref="TextControl.Font"/> has changed.
            /// This is called immediately before <see cref="TextControl.FontChanged"/>.
            /// Override this method instead of using an event hook on <see cref="TextControl.FontChanged"/> when possible.
            /// </summary>
            protected override void OnFontChanged()
            {
                base.OnFontChanged();

                // Make sure we resize the textbox to fit the text
                ResizeToFitText(MaxChatBubbleWidth);
            }

            /// <summary>
            /// Handles when the <see cref="TextControl.Text"/> has changed.
            /// This is called immediately before <see cref="TextControl.TextChanged"/>.
            /// Override this method instead of using an event hook on <see cref="TextControl.TextChanged"/> when possible.
            /// </summary>
            protected override void OnTextChanged()
            {
                base.OnTextChanged();

                // Make sure we resize the textbox to fit the text
                ResizeToFitText(MaxChatBubbleWidth);
            }

            /// <summary>
            /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
            /// base class's method to ensure that changes to settings are hierchical.
            /// </summary>
            protected override void SetDefaultValues()
            {
                base.SetDefaultValues();

                IsBoundToParentArea = false;
                IsMultiLine = true;
                Border = ControlBorder.Empty;
                CanFocus = false;
                CanDrag = false;
                Font = ((ChatBubble)Parent).Font;
                IsEnabled = false;
            }
        }
    }
}