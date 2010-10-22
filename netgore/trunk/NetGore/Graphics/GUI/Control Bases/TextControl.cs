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
        public event ControlEventHandler FontChanged
        {
            add { Events.AddHandler(_eventFontChanged, value); }
            remove { Events.RemoveHandler(_eventFontChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="TextControl.Text"/> has changed.
        /// </summary>
        public event ControlEventHandler TextChanged
        {
            add { Events.AddHandler(_eventTextChanged, value); }
            remove { Events.RemoveHandler(_eventTextChanged, value); }
        }

        /// <summary>
        /// Gets or sets the SpriteFont used by the TextControl.
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
        /// Gets or sets the default foreground color used by the TextControl.
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
        /// Gets the string for a key if the key represents a valid ASCII character. If possible, try to use
        /// a <see cref="TextEventArgs"/> instead.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <param name="shift">If true, shift will be treated as being pressed.</param>
        /// <returns>String for the key, or String.Empty if invalid.</returns>
        public static string GetKeyString(KeyCode key, bool shift)
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
        /// Gets the string for a key if the key represents a valid ASCII character with shift up.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>String for the key, or String.Empty if invalid.</returns>
        static string GetKeyStringNoShift(KeyCode key)
        {
            switch (key)
            {
                    // Alpha
                case KeyCode.A:
                    return "a";
                case KeyCode.B:
                    return "b";
                case KeyCode.C:
                    return "c";
                case KeyCode.D:
                    return "d";
                case KeyCode.E:
                    return "e";
                case KeyCode.F:
                    return "f";
                case KeyCode.G:
                    return "g";
                case KeyCode.H:
                    return "h";
                case KeyCode.I:
                    return "i";
                case KeyCode.J:
                    return "j";
                case KeyCode.K:
                    return "k";
                case KeyCode.L:
                    return "l";
                case KeyCode.M:
                    return "m";
                case KeyCode.N:
                    return "n";
                case KeyCode.O:
                    return "o";
                case KeyCode.P:
                    return "p";
                case KeyCode.Q:
                    return "q";
                case KeyCode.R:
                    return "r";
                case KeyCode.S:
                    return "s";
                case KeyCode.T:
                    return "t";
                case KeyCode.U:
                    return "u";
                case KeyCode.V:
                    return "v";
                case KeyCode.W:
                    return "w";
                case KeyCode.X:
                    return "x";
                case KeyCode.Y:
                    return "y";
                case KeyCode.Z:
                    return "z";

                    // Numeric
                case KeyCode.Num0:
                case KeyCode.Numpad0:
                    return "0";
                case KeyCode.Num1:
                case KeyCode.Numpad1:
                    return "1";
                case KeyCode.Num2:
                case KeyCode.Numpad2:
                    return "2";
                case KeyCode.Num3:
                case KeyCode.Numpad3:
                    return "3";
                case KeyCode.Num4:
                case KeyCode.Numpad4:
                    return "4";
                case KeyCode.Num5:
                case KeyCode.Numpad5:
                    return "5";
                case KeyCode.Num6:
                case KeyCode.Numpad6:
                    return "6";
                case KeyCode.Num7:
                case KeyCode.Numpad7:
                    return "7";
                case KeyCode.Num8:
                case KeyCode.Numpad8:
                    return "8";
                case KeyCode.Num9:
                case KeyCode.Numpad9:
                    return "9";

                    // Misc
                case KeyCode.Divide:
                    return "/";
                case KeyCode.Return:
                    return Environment.NewLine;
                case KeyCode.Multiply:
                    return "*";
                case KeyCode.RBracket:
                    return "]";
                case KeyCode.Comma:
                    return ",";
                case KeyCode.Subtract:
                    return "-";
                case KeyCode.LBracket:
                    return "[";
                case KeyCode.Period:
                    return ".";
                case KeyCode.BackSlash:
                    return "\\";
                case KeyCode.Add:
                    return "=";
                case KeyCode.Slash:
                    return "/";
                case KeyCode.Quote:
                    return "'";
                case KeyCode.SemiColon:
                    return ";";
                case KeyCode.Tilde:
                    return "`";
                case KeyCode.Space:
                    return " ";

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Gets the string for a key if the key represents a valid ASCII character with shift down.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>String for the key, or String.Empty if invalid.</returns>
        static string GetKeyStringShift(KeyCode key)
        {
            switch (key)
            {
                    // Alpha
                case KeyCode.A:
                    return "A";
                case KeyCode.B:
                    return "B";
                case KeyCode.C:
                    return "C";
                case KeyCode.D:
                    return "D";
                case KeyCode.E:
                    return "E";
                case KeyCode.F:
                    return "F";
                case KeyCode.G:
                    return "G";
                case KeyCode.H:
                    return "H";
                case KeyCode.I:
                    return "I";
                case KeyCode.J:
                    return "J";
                case KeyCode.K:
                    return "K";
                case KeyCode.L:
                    return "L";
                case KeyCode.M:
                    return "M";
                case KeyCode.N:
                    return "N";
                case KeyCode.O:
                    return "O";
                case KeyCode.P:
                    return "P";
                case KeyCode.Q:
                    return "Q";
                case KeyCode.R:
                    return "R";
                case KeyCode.S:
                    return "S";
                case KeyCode.T:
                    return "T";
                case KeyCode.U:
                    return "U";
                case KeyCode.V:
                    return "V";
                case KeyCode.W:
                    return "W";
                case KeyCode.X:
                    return "X";
                case KeyCode.Y:
                    return "Y";
                case KeyCode.Z:
                    return "Z";

                    // Numeric
                case KeyCode.Num0:
                    return ")";
                case KeyCode.Num1:
                    return "!";
                case KeyCode.Num2:
                    return "@";
                case KeyCode.Num3:
                    return "#";
                case KeyCode.Num4:
                    return "$";
                case KeyCode.Num5:
                    return "%";
                case KeyCode.Num6:
                    return "^";
                case KeyCode.Num7:
                    return "&";
                case KeyCode.Num8:
                    return "*";
                case KeyCode.Num9:
                    return "(";

                    // Misc
                case KeyCode.Divide:
                    return "/";
                case KeyCode.Return:
                    return Environment.NewLine;
                case KeyCode.Multiply:
                    return "*";
                case KeyCode.BackSlash:
                    return "|";
                case KeyCode.LBracket:
                    return "}";
                case KeyCode.Comma:
                    return "<";
                case KeyCode.Subtract:
                    return "_";
                case KeyCode.RBracket:
                    return "{";
                case KeyCode.Period:
                    return ">";
                case KeyCode.Add:
                    return "+";
                case KeyCode.Slash:
                    return "?";
                case KeyCode.Quote:
                    return "\"";
                case KeyCode.SemiColon:
                    return ":";
                case KeyCode.Tilde:
                    return "~";
                case KeyCode.Space:
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
            var handler = Events[_eventFontChanged] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        protected void InvokeTextChanged()
        {
            OnTextChanged();
            var handler = Events[_eventTextChanged] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Checks if a key is part of the alphabet
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if part of the alphabet, else false</returns>
        public static bool IsKeyAlpha(KeyCode key)
        {
            switch (key)
            {
                case KeyCode.A:
                case KeyCode.B:
                case KeyCode.C:
                case KeyCode.D:
                case KeyCode.E:
                case KeyCode.F:
                case KeyCode.G:
                case KeyCode.H:
                case KeyCode.I:
                case KeyCode.J:
                case KeyCode.K:
                case KeyCode.L:
                case KeyCode.M:
                case KeyCode.N:
                case KeyCode.O:
                case KeyCode.P:
                case KeyCode.Q:
                case KeyCode.R:
                case KeyCode.S:
                case KeyCode.T:
                case KeyCode.U:
                case KeyCode.V:
                case KeyCode.W:
                case KeyCode.X:
                case KeyCode.Y:
                case KeyCode.Z:
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
        public static bool IsKeyNumeric(KeyCode key)
        {
            switch (key)
            {
                case KeyCode.Num0:
                case KeyCode.Num1:
                case KeyCode.Num2:
                case KeyCode.Num3:
                case KeyCode.Num4:
                case KeyCode.Num5:
                case KeyCode.Num6:
                case KeyCode.Num7:
                case KeyCode.Num8:
                case KeyCode.Num9:
                case KeyCode.Numpad0:
                case KeyCode.Numpad1:
                case KeyCode.Numpad2:
                case KeyCode.Numpad3:
                case KeyCode.Numpad4:
                case KeyCode.Numpad5:
                case KeyCode.Numpad6:
                case KeyCode.Numpad7:
                case KeyCode.Numpad8:
                case KeyCode.Numpad9:
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