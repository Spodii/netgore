using System;
using System.Linq;
using System.Security;
using System.Text;
using Microsoft.Win32;
using SFML.Window;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Handles processing the keystrokes sent to a control and parsing the keystrokes to edit the
    /// text contained in the <see cref="IEditableText"/>.
    /// </summary>
    public class EditableTextHandler
    {
        /// <summary>
        /// The default blink rate of the insertion point cursor in milliseconds.
        /// </summary>
        const int _defaultCursorBlinkRate = 500;

        /// <summary>
        /// The blink rate of the insertion point cursor in milliseconds.
        /// </summary>
        public static readonly int CursorBlinkRate;

        readonly IEditableText _source;

        /// <summary>
        /// Initializes the <see cref="EditableTextHandler"/> class.
        /// </summary>
        static EditableTextHandler()
        {
            var blinkRate = _defaultCursorBlinkRate;

            try
            {
                var regSubKey = Registry.CurrentUser.OpenSubKey("Control Panel");
                if (regSubKey != null)
                {
                    var desktopKey = regSubKey.OpenSubKey("Desktop");
                    if (desktopKey != null)
                    {
                        var blinkRateObj = regSubKey.GetValue("CursorBlinkRate");
                        var blinkRateStr = blinkRateObj != null ? blinkRateObj.ToString() : null;

                        if (blinkRateStr == null || !int.TryParse(blinkRateStr, out blinkRate))
                            blinkRate = _defaultCursorBlinkRate;

                        // Keep in a reasonable range
                        if (blinkRate < 100)
                            blinkRate = 100;
                        if (blinkRate > 2000)
                            blinkRate = 2000;
                    }
                }
            }
            catch (SecurityException)
            {
                // Permission to the keys denied
                blinkRate = _defaultCursorBlinkRate;
            }
            catch (ObjectDisposedException)
            {
                // Key objects were disposed for some reason
                blinkRate = _defaultCursorBlinkRate;
            }

            CursorBlinkRate = blinkRate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IEditableText"/> class.
        /// </summary>
        /// <param name="source">The <see cref="IEditableText"/> to control the input of.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
        public EditableTextHandler(IEditableText source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            _source = source;
        }

        /// <summary>
        /// Gets the <see cref="IEditableText"/> that this <see cref="EditableTextHandler"/> handles.
        /// </summary>
        public IEditableText Source
        {
            get { return _source; }
        }

        /// <summary>
        /// Checks if a character is valid to be inserted.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <returns>True if the character <paramref name="c"/> can be inserted into the <see cref="TextBox"/>; otherwise false.</returns>
        static bool CanInsertChar(string c)
        {
            var utf8Code = Encoding.UTF8.GetBytes(c);

            // Invalid length
            if (utf8Code.Length <= 0)
                return false;

            // Length greather than 1... foreign characters? Whatever it is, probably want to allow it.
            if (utf8Code.Length > 1)
                return true;

            // Do not allow characters below 32 since we either handles those on a specific
            // case-by-case basis, or not at all
            if (utf8Code[0] < 32)
                return false;

            return true;
        }

        public void HandleKey(KeyEventArgs e)
        {
            switch (e.Code)
            {
                case KeyCode.Left:
                    Source.MoveCursor(MoveCursorDirection.Left);
                    break;

                case KeyCode.Right:
                    Source.MoveCursor(MoveCursorDirection.Right);
                    break;

                case KeyCode.Up:
                    Source.MoveCursor(MoveCursorDirection.Up);
                    break;

                case KeyCode.Down:
                    Source.MoveCursor(MoveCursorDirection.Down);
                    break;
            }
        }

        /// <summary>
        /// Handles when text is requested to be inserted into this <see cref="EditableTextHandler"/>.
        /// </summary>
        /// <param name="e">The <see cref="TextEventArgs"/>.</param>
        public void HandleText(TextEventArgs e)
        {
            var s = e.Unicode;

            // Ensure we received a valid string
            if (string.IsNullOrEmpty(s))
                return;

            switch (s)
            {
                case "\b":
                    // Delete character
                    Source.DeleteChar();
                    break;

                case "\r":
                    // Line break
                    Source.BreakLine();
                    break;

                default:
                    // Make sure it is a valid character to insert
                    if (!CanInsertChar(s))
                        return;

                    // Insert the character
                    Source.InsertChar(s);
                    break;
            }
        }
    }
}