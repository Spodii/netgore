using System;
using System.Linq;
using System.Security;
using Microsoft.Win32;
using Microsoft.Xna.Framework.Input;
using NetGore.Globalization;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Handles processing the keystrokes sent to a control and parsing the keystrokes to edit the
    /// text contained in the <see cref="IEditableText"/>.
    /// </summary>
    public class EditableTextHandler
    {
        /// <summary>
        /// The default number of milliseconds to wait before repeating the keystroke of a held key. Used
        /// when the value cannot be acquired from the registry.
        /// </summary>
        const int _defaultKeyboardRepeatDelay = 750;

        /// <summary>
        /// The default number of milliseconds to wait between each repeat of the held key. Used when the
        /// value cannot be acquired from the registry.
        /// </summary>
        const int _defaultKeyboardRepeatRate = 31;

        /// <summary>
        /// The default blink rate of the insertion point cursor in milliseconds.
        /// </summary>
        const int _defaultCursorBlinkRate = 500;

        /// <summary>
        /// The blink rate of the insertion point cursor in milliseconds.
        /// </summary>
        public static readonly int CursorBlinkRate;

        /// <summary>
        /// The number of milliseconds to wait before repeating the keystroke of a held key.
        /// </summary>
        public static readonly int KeyboardRepeatDelay;

        /// <summary>
        /// The number of milliseconds to wait between each repeat of the held key.
        /// </summary>
        public static readonly int KeyboardRepeatRate;

        readonly IEditableText _source;

        Keys? _currentKey = null;
        int _currentTime = 0;
        KeyboardState _ks;
        int _repeatTime = int.MaxValue;

        /// <summary>
        /// Gets if the Shift key is currently down.
        /// </summary>
        bool IsShiftDown
        {
            get { return _ks.IsKeyDown(Keys.LeftShift) || _ks.IsKeyDown(Keys.RightShift); }
        }

        /// <summary>
        /// Gets the <see cref="IEditableText"/> that this <see cref="EditableTextHandler"/> handles.
        /// </summary>
        public IEditableText Source
        {
            get { return _source; }
        }

        /// <summary>
        /// Initializes the <see cref="EditableTextHandler"/> class.
        /// </summary>
        static EditableTextHandler()
        {
            int delay = _defaultKeyboardRepeatDelay;
            int rate = _defaultKeyboardRepeatRate;
            int blinkRate = _defaultCursorBlinkRate;

            try
            {
                var regSubKey = Registry.CurrentUser.OpenSubKey("Control Panel");
                if (regSubKey != null)
                {
                    var desktopKey = regSubKey.OpenSubKey("Desktop");
                    if (desktopKey != null)
                    {
                        object blinkRateObj = regSubKey.GetValue("CursorBlinkRate");
                        string blinkRateStr = blinkRateObj != null ? blinkRateObj.ToString() : null;

                        if (blinkRateStr == null || !int.TryParse(blinkRateStr, out blinkRate))
                            blinkRate = _defaultCursorBlinkRate;

                        // Keep in a reasonable range
                        if (blinkRate < 100)
                            blinkRate = 100;
                        if (blinkRate > 2000)
                            blinkRate = 2000;
                    }

                    var keyboardKey = regSubKey.OpenSubKey("Keyboard");
                    if (keyboardKey != null)
                    {
                        object delayObj = regSubKey.GetValue("KeyboardDelay");
                        object rateObj = regSubKey.GetValue("KeyboardSpeed");

                        string delayStr = delayObj != null ? delayObj.ToString() : null;
                        string rateStr = rateObj != null ? rateObj.ToString() : null;

                        if (delayStr == null || !Parser.Current.TryParse(delayStr, out delay))
                            delay = _defaultKeyboardRepeatDelay;
                        else
                        {
                            // Delay is stored as a lookup value, not an absolute value
                            // This switch will convert the lookup value to a usable value
                            // The values are according to the Windows specifications
                            // See: http://technet.microsoft.com/en-us/library/cc978658.aspx
                            switch (delay)
                            {
                                case 0:
                                    delay = 250;
                                    break;
                                case 1:
                                    delay = 500;
                                    break;
                                case 2:
                                    delay = 750;
                                    break;
                                case 3:
                                    delay = 1000;
                                    break;
                                default:
                                    delay = _defaultKeyboardRepeatDelay;
                                    break;
                            }
                        }

                        if (rateStr == null || !Parser.Current.TryParse(rateStr, out rate) || rate < 0 || rate > 31)
                            rate = _defaultKeyboardRepeatRate;
                        else
                        {
                            // Equation made to find approximate delay in ms according to:
                            // http://technet.microsoft.com/en-us/library/cc978659.aspx
                            rate = (500 - (rate * 15));
                        }
                    }
                }
            }
            catch (SecurityException)
            {
                // Permission to the keys denied
                delay = _defaultKeyboardRepeatDelay;
                rate = _defaultKeyboardRepeatRate;
                blinkRate = _defaultCursorBlinkRate;
            }
            catch (ObjectDisposedException)
            {
                // Key objects were disposed for some reason
                delay = _defaultKeyboardRepeatDelay;
                rate = _defaultKeyboardRepeatRate;
                blinkRate = _defaultCursorBlinkRate;
            }

            KeyboardRepeatDelay = delay;
            KeyboardRepeatRate = rate;
            CursorBlinkRate = blinkRate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IEditableText"/> class.
        /// </summary>
        /// <param name="source">The <see cref="IEditableText"/> to control the input of.</param>
        public EditableTextHandler(IEditableText source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            _source = source;
        }

        /// <summary>
        /// Changes the _currentKey value.
        /// </summary>
        /// <param name="key">The new key, or null if the current key was raised.</param>
        void ChangeCurrentKey(Keys? key)
        {
            _currentKey = key;
            _repeatTime = _currentTime + KeyboardRepeatDelay;

            if (_currentKey != null)
                HandleKey(_currentKey.Value, IsShiftDown);
        }

        /// <summary>
        /// Handles a single key.
        /// </summary>
        /// <param name="key">The key to handle.</param>
        /// <param name="isShiftDown">If true, shift will be treated as being down.</param>
        void HandleKey(Keys key, bool isShiftDown)
        {
            switch (key)
            {
                case Keys.Left:
                    Source.MoveCursor(MoveCursorDirection.Left);
                    break;

                case Keys.Right:
                    Source.MoveCursor(MoveCursorDirection.Right);
                    break;

                case Keys.Up:
                    Source.MoveCursor(MoveCursorDirection.Up);
                    break;

                case Keys.Down:
                    Source.MoveCursor(MoveCursorDirection.Down);
                    break;

                case Keys.Back:
                    Source.DeleteChar();
                    break;

                case Keys.Enter:
                    Source.BreakLine();
                    break;

                default:
                    var s = TextControl.GetKeyString(key, isShiftDown);
                    if (!string.IsNullOrEmpty(s))
                        Source.InsertChar(s);
                    break;
            }
        }

        /// <summary>
        /// Notifies this <see cref="EditableTextHandler"/> that a key was just pressed. This should only be called once
        /// per key per each time a key is pressed.
        /// </summary>
        /// <param name="key">The key that was just pressed.</param>
        public void NotifyKeyPressed(Keys key)
        {
            ChangeCurrentKey(key);
        }

        /// <summary>
        /// Updates the <see cref="EditableTextHandler"/>.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <param name="ks">The latest keyboard state.</param>
        public void Update(int currentTime, KeyboardState ks)
        {
            _currentTime = currentTime;
            _ks = ks;

            // Check if our current key was raised
            if (_currentKey != null)
            {
                if (ks.IsKeyUp(_currentKey.Value))
                {
                    // Key was raised - set to null
                    ChangeCurrentKey(null);
                }
                else
                {
                    // Check to repeat the key
                    if (_currentTime >= _repeatTime)
                    {
                        _repeatTime = _currentTime + KeyboardRepeatRate;
                        HandleKey(_currentKey.Value, IsShiftDown);
                    }
                }
            }
        }
    }
}