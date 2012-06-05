using System.Linq;
using Keys = SFML.Window.KeyCode;

namespace NetGore
{
    /// <summary>
    /// Extension methods for the <see cref="Keys"/> enum.
    /// </summary>
    public static class KeysExtensions
    {
        /// <summary>
        /// Gets the corresponding numeric value for function <see cref="Keys"/>, or null if the
        /// key is not a numeric key. That is, the keyboard key for F1 will return 1, while "A" will return null.
        /// </summary>
        /// <param name="key">The key to get the corresponding numeric value for.</param>
        /// <returns>The numeric value for the <paramref name="key"/>, or null if not a numeric key.</returns>
        public static int? GetFunctionKeyAsValue(this Keys key)
        {
            switch (key)
            {
                case Keys.F1:
                    return 1;
                case Keys.F2:
                    return 2;
                case Keys.F3:
                    return 3;
                case Keys.F4:
                    return 4;
                case Keys.F5:
                    return 5;
                case Keys.F6:
                    return 6;
                case Keys.F7:
                    return 7;
                case Keys.F8:
                    return 8;
                case Keys.F9:
                    return 9;
                case Keys.F10:
                    return 10;
                case Keys.F11:
                    return 11;
                case Keys.F12:
                    return 12;
                case Keys.F13:
                    return 13;
                case Keys.F14:
                    return 14;
                case Keys.F15:
                    return 15;

                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the corresponding numeric value for numeric <see cref="Keys"/>, or null if the
        /// key is not a numeric key. That is, the keyboard key for the number 1 or numpad key 1 will return
        /// 1, while "A" will return null.
        /// </summary>
        /// <param name="key">The key to get the corresponding numeric value for.</param>
        /// <returns>The numeric value for the <paramref name="key"/>, or null if not a numeric key.</returns>
        public static int? GetNumericKeyAsValue(this Keys key)
        {
            switch (key)
            {
                case Keys.Num0:
                case Keys.Numpad0:
                    return 0;

                case Keys.Num1:
                case Keys.Numpad1:
                    return 1;

                case Keys.Num2:
                case Keys.Numpad2:
                    return 2;

                case Keys.Num3:
                case Keys.Numpad3:
                    return 3;

                case Keys.Num4:
                case Keys.Numpad4:
                    return 4;

                case Keys.Num5:
                case Keys.Numpad5:
                    return 5;

                case Keys.Num6:
                case Keys.Numpad6:
                    return 6;

                case Keys.Num7:
                case Keys.Numpad7:
                    return 7;

                case Keys.Num8:
                case Keys.Numpad8:
                    return 8;

                case Keys.Num9:
                case Keys.Numpad9:
                    return 9;

                default:
                    return null;
            }
        }
    }
}