using System.Linq;
using SFML.Window;

namespace NetGore
{
    /// <summary>
    /// Extension methods for the <see cref="Keyboard.Key"/> enum.
    /// </summary>
    public static class KeysExtensions
    {
        /// <summary>
        /// Gets the corresponding numeric value for function <see cref="Keyboard.Key"/>, or null if the
        /// key is not a numeric key. That is, the keyboard key for F1 will return 1, while "A" will return null.
        /// </summary>
        /// <param name="key">The key to get the corresponding numeric value for.</param>
        /// <returns>The numeric value for the <paramref name="key"/>, or null if not a numeric key.</returns>
        public static int? GetFunctionKeyAsValue(this Keyboard.Key key)
        {
            switch (key)
            {
                case Keyboard.Key.F1:
                    return 1;
                case Keyboard.Key.F2:
                    return 2;
                case Keyboard.Key.F3:
                    return 3;
                case Keyboard.Key.F4:
                    return 4;
                case Keyboard.Key.F5:
                    return 5;
                case Keyboard.Key.F6:
                    return 6;
                case Keyboard.Key.F7:
                    return 7;
                case Keyboard.Key.F8:
                    return 8;
                case Keyboard.Key.F9:
                    return 9;
                case Keyboard.Key.F10:
                    return 10;
                case Keyboard.Key.F11:
                    return 11;
                case Keyboard.Key.F12:
                    return 12;
                case Keyboard.Key.F13:
                    return 13;
                case Keyboard.Key.F14:
                    return 14;
                case Keyboard.Key.F15:
                    return 15;

                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the corresponding numeric value for numeric <see cref="Keyboard.Key"/>, or null if the
        /// key is not a numeric key. That is, the keyboard key for the number 1 or numpad key 1 will return
        /// 1, while "A" will return null.
        /// </summary>
        /// <param name="key">The key to get the corresponding numeric value for.</param>
        /// <returns>The numeric value for the <paramref name="key"/>, or null if not a numeric key.</returns>
        public static int? GetNumericKeyAsValue(this Keyboard.Key key)
        {
            switch (key)
            {
                case Keyboard.Key.Num0:
                case Keyboard.Key.Numpad0:
                    return 0;

                case Keyboard.Key.Num1:
                case Keyboard.Key.Numpad1:
                    return 1;

                case Keyboard.Key.Num2:
                case Keyboard.Key.Numpad2:
                    return 2;

                case Keyboard.Key.Num3:
                case Keyboard.Key.Numpad3:
                    return 3;

                case Keyboard.Key.Num4:
                case Keyboard.Key.Numpad4:
                    return 4;

                case Keyboard.Key.Num5:
                case Keyboard.Key.Numpad5:
                    return 5;

                case Keyboard.Key.Num6:
                case Keyboard.Key.Numpad6:
                    return 6;

                case Keyboard.Key.Num7:
                case Keyboard.Key.Numpad7:
                    return 7;

                case Keyboard.Key.Num8:
                case Keyboard.Key.Numpad8:
                    return 8;

                case Keyboard.Key.Num9:
                case Keyboard.Key.Numpad9:
                    return 9;

                default:
                    return null;
            }
        }
    }
}