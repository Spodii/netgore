using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NetGore.Collections;

namespace NetGore.Editor
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

        /// <summary>
        /// Ensures the <see cref="Input"/> is initialized. Doesn't require being called, and can be called multiple times, but
        /// is helpful to call early on.
        /// </summary>
        public static void Initialize()
        {
            // Calling this method will force the static constructor to run, which will generate the KeyMessageFilter
            // and start listening for input
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
            const int VK_F10 = 0x79;
            const int VK_MENU = 0x12;
            const int WM_KEYDOWN = 0x0100;
            const int WM_KEYUP = 0x0101;
            const int WM_SYSKEYDOWN = 0x0104;
            const int WM_SYSKEYUP = 0x0105;

            /// <summary>
            /// The regular keys.
            /// </summary>
            readonly IDictionary<Keys, bool> _keyStates = new TSDictionary<Keys, bool>(EnumComparer<Keys>.Instance);

            bool _altKeyDown = false;
            bool _f10KeyDown = false;

            /// <summary>
            /// Converts the <see cref="Keys"/> to the proper keyboard key.
            /// </summary>
            /// <param name="key">The <see cref="Keys"/>.</param>
            /// <returns>The <see cref="Keys"/> to use to look up the key state.</returns>
            static Keys ConvertKey(Keys key)
            {
                switch (key)
                {
                    case Keys.Shift:
                    case Keys.LShiftKey:
                    case Keys.RShiftKey:
                        return Keys.ShiftKey;

                    case Keys.Control:
                    case Keys.LControlKey:
                    case Keys.RControlKey:
                        return Keys.ControlKey;

                    default:
                        return key;
                }
            }

            /// <summary>
            /// Checks if a key is down.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>True if the <paramref name="key"/> is down; otherwise false.</returns>
            public bool InternalIsKeyDown(Keys key)
            {
                var realKey = ConvertKey(key);

                switch (key)
                {
                    case Keys.Alt:
                        return _altKeyDown;

                    case Keys.F10:
                        return _f10KeyDown;

                    default:
                        bool pressed;
                        if (_keyStates.TryGetValue(realKey, out pressed))
                            return pressed;
                        else
                            return false;
                }
            }

            /// <summary>
            /// Sets the state of a system key.
            /// </summary>
            /// <param name="wParam">The raw key code.</param>
            /// <param name="isDown">True if the key is down; false if it is up.</param>
            void SetSysKey(int wParam, bool isDown)
            {
                switch (wParam)
                {
                    case VK_MENU:
                        _altKeyDown = isDown;
                        break;

                    case VK_F10:
                        _f10KeyDown = isDown;
                        break;
                }
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
                switch (m.Msg)
                {
                    case WM_KEYDOWN:
                        _keyStates[(Keys)m.WParam] = true;
                        break;

                    case WM_KEYUP:
                        _keyStates[(Keys)m.WParam] = false;
                        break;

                    case WM_SYSKEYDOWN:
                        SetSysKey(m.WParam.ToInt32(), true);
                        break;

                    case WM_SYSKEYUP:
                        SetSysKey(m.WParam.ToInt32(), false);
                        break;
                }

                return false;
            }

            #endregion
        }
    }
}