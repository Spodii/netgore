using System;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Base of a <see cref="Control"/> that contains a text display.
    /// </summary>
    public abstract class TextControl : Control
    {
        static readonly object _eventFontChanged = new object();
        static readonly object _eventTextChanged = new object();

        Font _font = null;
        Color _foreColor = Color.Black;
        string _text = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextControl"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        protected TextControl(Control parent, Vector2 position, Vector2 clientSize) : base(parent, position, clientSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextControl"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        protected TextControl(IGUIManager guiManager, Vector2 position, Vector2 clientSize)
            : base(guiManager, position, clientSize)
        {
        }

        /// <summary>
        /// Notifies listeners when the <see cref="TextControl.Font"/> has changed.
        /// </summary>
        public event TypedEventHandler<Control> FontChanged
        {
            add { Events.AddHandler(_eventFontChanged, value); }
            remove { Events.RemoveHandler(_eventFontChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="TextControl.Text"/> has changed.
        /// </summary>
        public event TypedEventHandler<Control> TextChanged
        {
            add { Events.AddHandler(_eventTextChanged, value); }
            remove { Events.RemoveHandler(_eventTextChanged, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="Font"/> used by the <see cref="TextControl"/>.
        /// </summary>
        public virtual Font Font
        {
            get { return _font; }
            set
            {
                // Ensure the value has changed
                if (_font == value)
                    return;

                _font = value;

                InvokeFontChanged();
            }
        }

        /// <summary>
        /// Gets or sets the default foreground color used by the <see cref="TextControl"/>.
        /// </summary>
        [SyncValue]
        public Color ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }

        /// <summary>
        /// Gets or sets the text of the TextControl.
        /// </summary>
        public virtual string Text
        {
            get { return _text; }
            set
            {
                if (value == null)
                    value = string.Empty;

                if (StringComparer.Ordinal.Equals(value, _text))
                    return;

                _text = value;

                InvokeTextChanged();
            }
        }

        /// <summary>
        /// Draws the text for the control.
        /// </summary>
        /// <param name="spriteBatch"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="position">Position relative to the Control to draw the text.</param>
        protected virtual void DrawText(ISpriteBatch spriteBatch, Vector2 position)
        {
            // Ensure the font is valid
            if (string.IsNullOrEmpty(Text) || Font == null || Font.IsDisposed)
                return;

            // Draw the text
            spriteBatch.DrawString(Font, Text, ScreenPosition + position, ForeColor);
        }

        /// <summary>
        /// Gets the string for a key if the key represents a valid printable character. If possible, try to use
        /// a <see cref="TextEventArgs"/> instead.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <param name="shift">If true, shift will be treated as being pressed.</param>
        /// <returns>String for the key, or String.Empty if invalid.</returns>
        public static string GetKeyString(Keyboard.Key key, bool shift)
        {
            if (shift)
            {
                // Shift is down
                return GetKeyStringShift(key);
            }
            else
            {
                // Shift is not down
                return GetKeyStringNoShift(key);
            }
        }

        /// <summary>
        /// Gets the string for a key if the key represents a valid printable character with shift up.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>String for the key, or String.Empty if invalid.</returns>
        static string GetKeyStringNoShift(Keyboard.Key key)
        {
            switch (key)
            {
                    // Alpha
                case Keyboard.Key.A:
                    return "a";
                case Keyboard.Key.B:
                    return "b";
                case Keyboard.Key.C:
                    return "c";
                case Keyboard.Key.D:
                    return "d";
                case Keyboard.Key.E:
                    return "e";
                case Keyboard.Key.F:
                    return "f";
                case Keyboard.Key.G:
                    return "g";
                case Keyboard.Key.H:
                    return "h";
                case Keyboard.Key.I:
                    return "i";
                case Keyboard.Key.J:
                    return "j";
                case Keyboard.Key.K:
                    return "k";
                case Keyboard.Key.L:
                    return "l";
                case Keyboard.Key.M:
                    return "m";
                case Keyboard.Key.N:
                    return "n";
                case Keyboard.Key.O:
                    return "o";
                case Keyboard.Key.P:
                    return "p";
                case Keyboard.Key.Q:
                    return "q";
                case Keyboard.Key.R:
                    return "r";
                case Keyboard.Key.S:
                    return "s";
                case Keyboard.Key.T:
                    return "t";
                case Keyboard.Key.U:
                    return "u";
                case Keyboard.Key.V:
                    return "v";
                case Keyboard.Key.W:
                    return "w";
                case Keyboard.Key.X:
                    return "x";
                case Keyboard.Key.Y:
                    return "y";
                case Keyboard.Key.Z:
                    return "z";

                    // Numeric
                case Keyboard.Key.Num0:
                case Keyboard.Key.Numpad0:
                    return "0";
                case Keyboard.Key.Num1:
                case Keyboard.Key.Numpad1:
                    return "1";
                case Keyboard.Key.Num2:
                case Keyboard.Key.Numpad2:
                    return "2";
                case Keyboard.Key.Num3:
                case Keyboard.Key.Numpad3:
                    return "3";
                case Keyboard.Key.Num4:
                case Keyboard.Key.Numpad4:
                    return "4";
                case Keyboard.Key.Num5:
                case Keyboard.Key.Numpad5:
                    return "5";
                case Keyboard.Key.Num6:
                case Keyboard.Key.Numpad6:
                    return "6";
                case Keyboard.Key.Num7:
                case Keyboard.Key.Numpad7:
                    return "7";
                case Keyboard.Key.Num8:
                case Keyboard.Key.Numpad8:
                    return "8";
                case Keyboard.Key.Num9:
                case Keyboard.Key.Numpad9:
                    return "9";

                    // Misc
                case Keyboard.Key.Divide:
                    return "/";
                case Keyboard.Key.Return:
                    return Environment.NewLine;
                case Keyboard.Key.Multiply:
                    return "*";
                case Keyboard.Key.RBracket:
                    return "]";
                case Keyboard.Key.Comma:
                    return ",";
                case Keyboard.Key.Subtract:
                    return "-";
                case Keyboard.Key.LBracket:
                    return "[";
                case Keyboard.Key.Period:
                    return ".";
                case Keyboard.Key.BackSlash:
                    return "\\";
                case Keyboard.Key.Add:
                    return "=";
                case Keyboard.Key.Slash:
                    return "/";
                case Keyboard.Key.Quote:
                    return "'";
                case Keyboard.Key.SemiColon:
                    return ";";
                case Keyboard.Key.Tilde:
                    return "`";
                case Keyboard.Key.Space:
                    return " ";

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Gets the string for a key if the key represents a valid printable character with shift down.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>String for the key, or String.Empty if invalid.</returns>
        static string GetKeyStringShift(Keyboard.Key key)
        {
            switch (key)
            {
                    // Alpha
                case Keyboard.Key.A:
                    return "A";
                case Keyboard.Key.B:
                    return "B";
                case Keyboard.Key.C:
                    return "C";
                case Keyboard.Key.D:
                    return "D";
                case Keyboard.Key.E:
                    return "E";
                case Keyboard.Key.F:
                    return "F";
                case Keyboard.Key.G:
                    return "G";
                case Keyboard.Key.H:
                    return "H";
                case Keyboard.Key.I:
                    return "I";
                case Keyboard.Key.J:
                    return "J";
                case Keyboard.Key.K:
                    return "K";
                case Keyboard.Key.L:
                    return "L";
                case Keyboard.Key.M:
                    return "M";
                case Keyboard.Key.N:
                    return "N";
                case Keyboard.Key.O:
                    return "O";
                case Keyboard.Key.P:
                    return "P";
                case Keyboard.Key.Q:
                    return "Q";
                case Keyboard.Key.R:
                    return "R";
                case Keyboard.Key.S:
                    return "S";
                case Keyboard.Key.T:
                    return "T";
                case Keyboard.Key.U:
                    return "U";
                case Keyboard.Key.V:
                    return "V";
                case Keyboard.Key.W:
                    return "W";
                case Keyboard.Key.X:
                    return "X";
                case Keyboard.Key.Y:
                    return "Y";
                case Keyboard.Key.Z:
                    return "Z";

                    // Numeric
                case Keyboard.Key.Num0:
                    return ")";
                case Keyboard.Key.Num1:
                    return "!";
                case Keyboard.Key.Num2:
                    return "@";
                case Keyboard.Key.Num3:
                    return "#";
                case Keyboard.Key.Num4:
                    return "$";
                case Keyboard.Key.Num5:
                    return "%";
                case Keyboard.Key.Num6:
                    return "^";
                case Keyboard.Key.Num7:
                    return "&";
                case Keyboard.Key.Num8:
                    return "*";
                case Keyboard.Key.Num9:
                    return "(";

                    // Misc
                case Keyboard.Key.Divide:
                    return "/";
                case Keyboard.Key.Return:
                    return Environment.NewLine;
                case Keyboard.Key.Multiply:
                    return "*";
                case Keyboard.Key.BackSlash:
                    return "|";
                case Keyboard.Key.LBracket:
                    return "}";
                case Keyboard.Key.Comma:
                    return "<";
                case Keyboard.Key.Subtract:
                    return "_";
                case Keyboard.Key.RBracket:
                    return "{";
                case Keyboard.Key.Period:
                    return ">";
                case Keyboard.Key.Add:
                    return "+";
                case Keyboard.Key.Slash:
                    return "?";
                case Keyboard.Key.Quote:
                    return "\"";
                case Keyboard.Key.SemiColon:
                    return ":";
                case Keyboard.Key.Tilde:
                    return "~";
                case Keyboard.Key.Space:
                    return " ";

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeFontChanged()
        {
            OnFontChanged();
            var handler = Events[_eventFontChanged] as TypedEventHandler<Control>;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        protected void InvokeTextChanged()
        {
            OnTextChanged();
            var handler = Events[_eventTextChanged] as TypedEventHandler<Control>;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Checks if a key is part of the alphabet
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if part of the alphabet, else false</returns>
        public static bool IsKeyAlpha(Keyboard.Key key)
        {
            switch (key)
            {
                case Keyboard.Key.A:
                case Keyboard.Key.B:
                case Keyboard.Key.C:
                case Keyboard.Key.D:
                case Keyboard.Key.E:
                case Keyboard.Key.F:
                case Keyboard.Key.G:
                case Keyboard.Key.H:
                case Keyboard.Key.I:
                case Keyboard.Key.J:
                case Keyboard.Key.K:
                case Keyboard.Key.L:
                case Keyboard.Key.M:
                case Keyboard.Key.N:
                case Keyboard.Key.O:
                case Keyboard.Key.P:
                case Keyboard.Key.Q:
                case Keyboard.Key.R:
                case Keyboard.Key.S:
                case Keyboard.Key.T:
                case Keyboard.Key.U:
                case Keyboard.Key.V:
                case Keyboard.Key.W:
                case Keyboard.Key.X:
                case Keyboard.Key.Y:
                case Keyboard.Key.Z:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks if a key is numeric
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if numeric, else false</returns>
        public static bool IsKeyNumeric(Keyboard.Key key)
        {
            switch (key)
            {
                case Keyboard.Key.Num0:
                case Keyboard.Key.Num1:
                case Keyboard.Key.Num2:
                case Keyboard.Key.Num3:
                case Keyboard.Key.Num4:
                case Keyboard.Key.Num5:
                case Keyboard.Key.Num6:
                case Keyboard.Key.Num7:
                case Keyboard.Key.Num8:
                case Keyboard.Key.Num9:
                case Keyboard.Key.Numpad0:
                case Keyboard.Key.Numpad1:
                case Keyboard.Key.Numpad2:
                case Keyboard.Key.Numpad3:
                case Keyboard.Key.Numpad4:
                case Keyboard.Key.Numpad5:
                case Keyboard.Key.Numpad6:
                case Keyboard.Key.Numpad7:
                case Keyboard.Key.Numpad8:
                case Keyboard.Key.Numpad9:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Handles when the <see cref="TextControl.Font"/> has changed.
        /// This is called immediately before <see cref="TextControl.FontChanged"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.FontChanged"/> when possible.
        /// </summary>
        protected virtual void OnFontChanged()
        {
        }

        /// <summary>
        /// Handles when the <see cref="TextControl.Text"/> has changed.
        /// This is called immediately before <see cref="TextControl.TextChanged"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.TextChanged"/> when possible.
        /// </summary>
        protected virtual void OnTextChanged()
        {
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Text = string.Empty;
            Font = GUIManager.Font;
            ForeColor = Color.Black;
        }
    }
}