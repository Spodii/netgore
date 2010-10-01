using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NetGore;
using NetGore.Collections;

namespace DemoGame.Editor
{
    /// <summary>
    /// Helper method for polling the input state.
    /// </summary>
    public static class Input
    {
        static readonly KeyMessageFilter _keyMessageFilter = new KeyMessageFilter();

        /// <summary>
        /// Initializes the <see cref="Input"/> class.
        /// </summary>
        static Input()
        {
            Application.AddMessageFilter(_keyMessageFilter);
        }

        public static bool IsAltDown
        {
            get { return IsKeyDown(Keys.Alt); }
        }

        public static bool IsCtrlDown
        {
            get { return IsKeyDown(Keys.ControlKey); }
        }

        public static bool IsShiftDown
        {
            get { return IsKeyDown(Keys.ShiftKey); }
        }

        public static bool AreKeysDown(params Keys[] keys)
        {
            if (keys == null || keys.Length == 0)
                return true;

            return keys.All(IsKeyDown);
        }

        public static bool AreKeysUp(params Keys[] keys)
        {
            return !AreKeysDown(keys);
        }

        public static bool IsKeyDown(Keys key)
        {
            return _keyMessageFilter.InternalIsKeyDown(key);
        }

        public static bool IsKeyUp(Keys key)
        {
            return !IsKeyDown(key);
        }

        class KeyMessageFilter : IMessageFilter
        {
            const int WM_KEYDOWN = 0x0100;
            const int WM_KEYUP = 0x0101;

            readonly IDictionary<Keys, bool> _keyStates = new TSDictionary<Keys, bool>(EnumComparer<Keys>.Instance);

            public bool InternalIsKeyDown(Keys k)
            {
                bool pressed;

                if (_keyStates.TryGetValue(k, out pressed))
                    return pressed;

                return false;
            }

            #region IMessageFilter Members

            /// <summary>
            /// Filters out a message before it is dispatched.
            /// </summary>
            /// <param name="m">The message to be dispatched. You cannot modify this message.</param>
            /// <returns>
            /// true to filter the message and stop it from being dispatched; false to allow the message to continue to the next filter or control.
            /// </returns>
            bool IMessageFilter.PreFilterMessage(ref Message m)
            {
                if (m.Msg == WM_KEYDOWN)
                    _keyStates[(Keys)m.WParam] = true;

                if (m.Msg == WM_KEYUP)
                    _keyStates[(Keys)m.WParam] = false;

                return false;
            }

            #endregion
        }
    }
}