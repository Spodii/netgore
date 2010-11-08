using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace SFML
{
    namespace Window
    {
        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Definition of key codes for keyboard events
        /// </summary>
        ////////////////////////////////////////////////////////////
        public enum KeyCode
        {
            A = 'a',
            B = 'b',
            C = 'c',
            D = 'd',
            E = 'e',
            F = 'f',
            G = 'g',
            H = 'h',
            I = 'i',
            J = 'j',
            K = 'k',
            L = 'l',
            M = 'm',
            N = 'n',
            O = 'o',
            P = 'p',
            Q = 'q',
            R = 'r',
            S = 's',
            T = 't',
            U = 'u',
            V = 'v',
            W = 'w',
            X = 'x',
            Y = 'y',
            Z = 'z',
            Num0 = '0',
            Num1 = '1',
            Num2 = '2',
            Num3 = '3',
            Num4 = '4',
            Num5 = '5',
            Num6 = '6',
            Num7 = '7',
            Num8 = '8',
            Num9 = '9',
            Escape = 256,
            LControl,
            LShift,
            LAlt,
            LSystem, // OS specific key (left side) : windows (Win and Linux), apple (MacOS), ...
            RControl,
            RShift,
            RAlt,
            RSystem, // OS specific key (right side) : windows (Win and Linux), apple (MacOS), ...
            Menu,
            LBracket, // [
            RBracket, // ]
            SemiColon, // ;
            Comma, // ,
            Period, // .
            Quote, // '
            Slash, // /
            BackSlash,
            Tilde, // ~
            Equal, // =
            Dash, // -
            Space,
            Return,
            Back,
            Tab,
            PageUp,
            PageDown,
            End,
            Home,
            Insert,
            Delete,
            Add, // +
            Subtract, // -
            Multiply, // *
            Divide, // /
            Left, // Left arrow
            Right, // Right arrow
            Up, // Up arrow
            Down, // Down arrow
            Numpad0,
            Numpad1,
            Numpad2,
            Numpad3,
            Numpad4,
            Numpad5,
            Numpad6,
            Numpad7,
            Numpad8,
            Numpad9,
            F1,
            F2,
            F3,
            F4,
            F5,
            F6,
            F7,
            F8,
            F9,
            F10,
            F11,
            F12,
            F13,
            F14,
            F15,
            Pause
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Definition of button codes for mouse events
        /// </summary>
        ////////////////////////////////////////////////////////////
        public enum MouseButton
        {
            /// <summary>Left mouse button</summary>
            Left,

            /// <summary>Right mouse button</summary>
            Right,

            /// <summary>Center (wheel) mouse button</summary>
            Middle,

            /// <summary>First extra button</summary>
            XButton1,

            /// <summary>Second extra button</summary>
            XButton2
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Definition of joystick axis for joystick events
        /// </summary>
        ////////////////////////////////////////////////////////////
        public enum JoyAxis
        {
            /// <summary>X axis</summary>
            AxisX,

            /// <summary>Y axis</summary>
            AxisY,

            /// <summary>Z axis</summary>
            AxisZ,

            /// <summary>R axis</summary>
            AxisR,

            /// <summary>U axis</summary>
            AxisU,

            /// <summary>V axis</summary>
            AxisV,

            /// <summary>Point of view</summary>
            AxisPOV
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Enumeration of the different types of events
        /// </summary>
        ////////////////////////////////////////////////////////////
        public enum EventType
        {
            /// <summary>Event triggered when a window is manually closed</summary>
            Closed,

            /// <summary>Event triggered when a window is resized</summary>
            Resized,

            /// <summary>Event triggered when a window loses the focus</summary>
            LostFocus,

            /// <summary>Event triggered when a window gains the focus</summary>
            GainedFocus,

            /// <summary>Event triggered when a valid character is entered</summary>
            TextEntered,

            /// <summary>Event triggered when a keyboard key is pressed</summary>
            KeyPressed,

            /// <summary>Event triggered when a keyboard key is released</summary>
            KeyReleased,

            /// <summary>Event triggered when the mouse wheel is scrolled</summary>
            MouseWheelMoved,

            /// <summary>Event triggered when a mouse button is pressed</summary>
            MouseButtonPressed,

            /// <summary>Event triggered when a mouse button is released</summary>
            MouseButtonReleased,

            /// <summary>Event triggered when the mouse moves within the area of a window</summary>
            MouseMoved,

            /// <summary>Event triggered when the mouse enters the area of a window</summary>
            MouseEntered,

            /// <summary>Event triggered when the mouse leaves the area of a window</summary>
            MouseLeft,

            /// <summary>Event triggered when a joystick button is pressed</summary>
            JoyButtonPressed,

            /// <summary>Event triggered when a joystick button is released</summary>
            JoyButtonReleased,

            /// <summary>Event triggered when a joystick axis moves</summary>
            JoyMoved
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Keyboard event parameters
        /// </summary>
        ////////////////////////////////////////////////////////////
        [StructLayout(LayoutKind.Sequential)]
        public struct KeyEvent : IEquatable<KeyEvent>
        {
            /// <summary>Code of the key (see KeyCode enum)</summary>
            public KeyCode Code;

            /// <summary>Is the Alt modifier pressed?</summary>
            public int Alt;

            /// <summary>Is the Control modifier pressed?</summary>
            public int Control;

            /// <summary>Is the Shift modifier pressed?</summary>
            public int Shift;

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            public bool Equals(KeyEvent other)
            {
                return Equals(other.Code, Code) && other.Alt == Alt && other.Control == Control && other.Shift == Shift;
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    int result = Code.GetHashCode();
                    result = (result * 397) ^ Alt;
                    result = (result * 397) ^ Control;
                    result = (result * 397) ^ Shift;
                    return result;
                }
            }

            /// <summary>
            /// Implements the operator ==.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(KeyEvent left, KeyEvent right)
            {
                return left.Equals(right);
            }

            /// <summary>
            /// Implements the operator !=.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(KeyEvent left, KeyEvent right)
            {
                return !left.Equals(right);
            }

            /// <summary>
            /// Indicates whether this instance and a specified object are equal.
            /// </summary>
            /// <param name="obj">Another object to compare to.</param>
            /// <returns>
            /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
            /// </returns>
            public override bool Equals(object obj)
            {
                return obj is KeyEvent && this == (KeyEvent)obj;
            }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Text event parameters
        /// </summary>
        ////////////////////////////////////////////////////////////
        [StructLayout(LayoutKind.Sequential)]
        public struct TextEvent : IEquatable<TextEvent>
        {
            /// <summary>UTF-32 value of the character</summary>
            public uint Unicode;

            /// <summary>
            /// Indicates whether this instance and a specified object are equal.
            /// </summary>
            /// <param name="obj">Another object to compare to.</param>
            /// <returns>
            /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
            /// </returns>
            public override bool Equals(object obj)
            {
                return obj is TextEvent && this == (TextEvent)obj;
            }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            public bool Equals(TextEvent other)
            {
                return other.Unicode == Unicode;
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public override int GetHashCode()
            {
                return Unicode.GetHashCode();
            }

            /// <summary>
            /// Implements the operator ==.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(TextEvent left, TextEvent right)
            {
                return left.Equals(right);
            }

            /// <summary>
            /// Implements the operator !=.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(TextEvent left, TextEvent right)
            {
                return !left.Equals(right);
            }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Mouse move event parameters
        /// </summary>
        ////////////////////////////////////////////////////////////
        [StructLayout(LayoutKind.Sequential)]
        public struct MouseMoveEvent : IEquatable<MouseMoveEvent>
        {
            /// <summary>X coordinate of the mouse cursor</summary>
            public int X;

            /// <summary>Y coordinate of the mouse cursor</summary>
            public int Y;

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public bool Equals(MouseMoveEvent other)
            {
                return other.X == X && other.Y == Y;
            }

            /// <summary>
            /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
            /// </summary>
            /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
            /// <returns>
            /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
            /// </returns>
            public override bool Equals(object obj)
            {
                return obj is MouseMoveEvent && this == (MouseMoveEvent)obj;
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    return (X * 397) ^ Y;
                }
            }

            /// <summary>
            /// Implements the operator ==.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(MouseMoveEvent left, MouseMoveEvent right)
            {
                return left.Equals(right);
            }

            /// <summary>
            /// Implements the operator !=.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(MouseMoveEvent left, MouseMoveEvent right)
            {
                return !left.Equals(right);
            }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Mouse buttons event parameters
        /// </summary>
        ////////////////////////////////////////////////////////////
        [StructLayout(LayoutKind.Sequential)]
        public struct MouseButtonEvent : IEquatable<MouseButtonEvent>
        {
            /// <summary>Code of the button (see MouseButton enum)</summary>
            public MouseButton Button;

            /// <summary>X coordinate of the mouse cursor</summary>
            public int X;

            /// <summary>Y coordinate of the mouse cursor</summary>
            public int Y;

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            public bool Equals(MouseButtonEvent other)
            {
                return Equals(other.Button, Button) && other.X == X && other.Y == Y;
            }

            /// <summary>
            /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
            /// </summary>
            /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
            /// <returns>
            /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
            /// </returns>
            public override bool Equals(object obj)
            {
                return obj is MouseButtonEvent && this == (MouseButtonEvent)obj;
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    int result = Button.GetHashCode();
                    result = (result * 397) ^ X;
                    result = (result * 397) ^ Y;
                    return result;
                }
            }

            /// <summary>
            /// Implements the operator ==.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(MouseButtonEvent left, MouseButtonEvent right)
            {
                return left.Equals(right);
            }

            /// <summary>
            /// Implements the operator !=.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(MouseButtonEvent left, MouseButtonEvent right)
            {
                return !left.Equals(right);
            }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Mouse wheel event parameters
        /// </summary>
        ////////////////////////////////////////////////////////////
        [StructLayout(LayoutKind.Sequential)]
        public struct MouseWheelEvent : IEquatable<MouseWheelEvent>
        {
            /// <summary>Scroll amount</summary>
            public int Delta;

            /// <summary>X coordinate of the mouse cursor</summary>
            public int X;

            /// <summary>Y coordinate of the mouse cursor</summary>
            public int Y;

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public bool Equals(MouseWheelEvent other)
            {
                return other.Delta == Delta && other.X == X && other.Y == Y;
            }

            /// <summary>
            /// Indicates whether this instance and a specified object are equal.
            /// </summary>
            /// <param name="obj">Another object to compare to.</param>
            /// <returns>
            /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
            /// </returns>
            public override bool Equals(object obj)
            {
                return obj is MouseWheelEvent && this == (MouseWheelEvent)obj;
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    int result = Delta;
                    result = (result * 397) ^ X;
                    result = (result * 397) ^ Y;
                    return result;
                }
            }

            /// <summary>
            /// Implements the operator ==.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(MouseWheelEvent left, MouseWheelEvent right)
            {
                return left.Equals(right);
            }

            /// <summary>
            /// Implements the operator !=.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(MouseWheelEvent left, MouseWheelEvent right)
            {
                return !left.Equals(right);
            }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Joystick axis move event parameters
        /// </summary>
        ////////////////////////////////////////////////////////////
        [StructLayout(LayoutKind.Sequential)]
        public struct JoyMoveEvent : IEquatable<JoyMoveEvent>
        {
            /// <summary>Index of the joystick which triggered the event</summary>
            public uint JoystickId;

            /// <summary>Joystick axis (see JoyAxis enum)</summary>
            public JoyAxis Axis;

            /// <summary>Current position of the axis</summary>
            public float Position;

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public bool Equals(JoyMoveEvent other)
            {
                return other.JoystickId == JoystickId && Equals(other.Axis, Axis) && other.Position.Equals(Position);
            }

            /// <summary>
            /// Indicates whether this instance and a specified object are equal.
            /// </summary>
            /// <param name="obj">Another object to compare to.</param>
            /// <returns>
            /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
            /// </returns>
            public override bool Equals(object obj)
            {
                return obj is JoyMoveEvent && this == (JoyMoveEvent)obj;
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    int result = JoystickId.GetHashCode();
                    result = (result * 397) ^ Axis.GetHashCode();
                    result = (result * 397) ^ Position.GetHashCode();
                    return result;
                }
            }

            /// <summary>
            /// Implements the operator ==.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(JoyMoveEvent left, JoyMoveEvent right)
            {
                return left.Equals(right);
            }

            /// <summary>
            /// Implements the operator !=.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(JoyMoveEvent left, JoyMoveEvent right)
            {
                return !left.Equals(right);
            }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Joystick buttons event parameters
        /// </summary>
        ////////////////////////////////////////////////////////////
        [StructLayout(LayoutKind.Sequential)]
        public struct JoyButtonEvent : IEquatable<JoyButtonEvent>
        {
            /// <summary>Index of the joystick which triggered the event</summary>
            public uint JoystickId;

            /// <summary>Index of the button</summary>
            public uint Button;

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public bool Equals(JoyButtonEvent other)
            {
                return other.JoystickId == JoystickId && other.Button == Button;
            }

            /// <summary>
            /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
            /// </summary>
            /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
            /// <returns>
            /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
            /// </returns>
            public override bool Equals(object obj)
            {
                return obj is JoyButtonEvent && this == (JoyButtonEvent)obj;
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    return (JoystickId.GetHashCode() * 397) ^ Button.GetHashCode();
                }
            }

            /// <summary>
            /// Implements the operator ==.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(JoyButtonEvent left, JoyButtonEvent right)
            {
                return left.Equals(right);
            }

            /// <summary>
            /// Implements the operator !=.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(JoyButtonEvent left, JoyButtonEvent right)
            {
                return !left.Equals(right);
            }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Size event parameters
        /// </summary>
        ////////////////////////////////////////////////////////////
        [StructLayout(LayoutKind.Sequential)]
        public struct SizeEvent : IEquatable<SizeEvent>
        {
            /// <summary>New width of the window</summary>
            public uint Width;

            /// <summary>New height of the window</summary>
            public uint Height;

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public bool Equals(SizeEvent other)
            {
                return other.Width == Width && other.Height == Height;
            }

            /// <summary>
            /// Indicates whether this instance and a specified object are equal.
            /// </summary>
            /// <param name="obj">Another object to compare to.</param>
            /// <returns>
            /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
            /// </returns>
            public override bool Equals(object obj)
            {
                return obj is SizeEvent && this == (SizeEvent)obj;
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    return (Width.GetHashCode() * 397) ^ Height.GetHashCode();
                }
            }

            /// <summary>
            /// Implements the operator ==.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(SizeEvent left, SizeEvent right)
            {
                return left.Equals(right);
            }

            /// <summary>
            /// Implements the operator !=.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(SizeEvent left, SizeEvent right)
            {
                return !left.Equals(right);
            }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Event defines a system event and its parameters
        /// </summary>
        ////////////////////////////////////////////////////////////
        [StructLayout(LayoutKind.Explicit, Size = 20)]
        public struct Event : IEquatable<Event>
        {
            /// <summary>Type of event (see EventType enum)</summary>
            [FieldOffset(0)]
            public EventType Type;

            /// <summary>Arguments for key events (KeyPressed, KeyReleased)</summary>
            [FieldOffset(4)]
            public KeyEvent Key;

            /// <summary>Arguments for text events (TextEntered)</summary>
            [FieldOffset(4)]
            public TextEvent Text;

            /// <summary>Arguments for mouse move events (MouseMoved)</summary>
            [FieldOffset(4)]
            public MouseMoveEvent MouseMove;

            /// <summary>Arguments for mouse button events (MouseButtonPressed, MouseButtonReleased)</summary>
            [FieldOffset(4)]
            public MouseButtonEvent MouseButton;

            /// <summary>Arguments for mouse wheel events (MouseWheelMoved)</summary>
            [FieldOffset(4)]
            public MouseWheelEvent MouseWheel;

            /// <summary>Arguments for joystick axis events (JoyMoved)</summary>
            [FieldOffset(4)]
            public JoyMoveEvent JoyMove;

            /// <summary>Arguments for joystick button events (JoyButtonPressed, JoyButtonReleased)</summary>
            [FieldOffset(4)]
            public JoyButtonEvent JoyButton;

            /// <summary>Arguments for size events (Resized)</summary>
            [FieldOffset(4)]
            public SizeEvent Size;

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            public bool Equals(Event other)
            {
                return Equals(other.Type, Type) && other.Key.Equals(Key) && other.Text.Equals(Text) && other.MouseMove.Equals(MouseMove) && other.MouseButton.Equals(MouseButton) && other.MouseWheel.Equals(MouseWheel) && other.JoyMove.Equals(JoyMove) && other.JoyButton.Equals(JoyButton) && other.Size.Equals(Size);
            }

            /// <summary>
            /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
            /// </summary>
            /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
            /// <returns>
            /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
            /// </returns>
            public override bool Equals(object obj)
            {
                return obj is Event && this == (Event)obj;
            }

            /// <summary>
            /// Returns the hash code for this instance.
            /// </summary>
            /// <returns>
            /// A 32-bit signed integer that is the hash code for this instance.
            /// </returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    int result = Type.GetHashCode();
                    result = (result * 397) ^ Key.GetHashCode();
                    result = (result * 397) ^ Text.GetHashCode();
                    result = (result * 397) ^ MouseMove.GetHashCode();
                    result = (result * 397) ^ MouseButton.GetHashCode();
                    result = (result * 397) ^ MouseWheel.GetHashCode();
                    result = (result * 397) ^ JoyMove.GetHashCode();
                    result = (result * 397) ^ JoyButton.GetHashCode();
                    result = (result * 397) ^ Size.GetHashCode();
                    return result;
                }
            }

            /// <summary>
            /// Implements the operator ==.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(Event left, Event right)
            {
                return left.Equals(right);
            }

            /// <summary>
            /// Implements the operator !=.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(Event left, Event right)
            {
                return !left.Equals(right);
            }
        }
    }
}