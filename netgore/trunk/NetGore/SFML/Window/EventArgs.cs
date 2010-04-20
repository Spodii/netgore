using System;
using System.Linq;

namespace SFML
{
    namespace Window
    {
        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Keyboard event parameters
        /// </summary>
        ////////////////////////////////////////////////////////////
        public class KeyEventArgs : EventArgs
        {
            ////////////////////////////////////////////////////////////

            /// <summary>Is the Alt modifier pressed?</summary>
            public bool Alt;

            /// <summary>Code of the key (see KeyCode enum)</summary>
            public KeyCode Code;

            /// <summary>Is the Control modifier pressed?</summary>
            public bool Control;

            /// <summary>Is the Shift modifier pressed?</summary>
            public bool Shift;

            /// <summary>
            /// Construct the key arguments from a key event
            /// </summary>
            /// <param name="e">Key event</param>
            ////////////////////////////////////////////////////////////
            public KeyEventArgs(KeyEvent e)
            {
                Code = e.Code;
                Alt = e.Alt;
                Control = e.Control;
                Shift = e.Shift;
            }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Text event parameters
        /// </summary>
        ////////////////////////////////////////////////////////////
        public class TextEventArgs : EventArgs
        {
            ////////////////////////////////////////////////////////////

            /// <summary>UTF-16 value of the character</summary>
            public string Unicode;

            /// <summary>
            /// Construct the text arguments from a text event
            /// </summary>
            /// <param name="e">Text event</param>
            ////////////////////////////////////////////////////////////
            public TextEventArgs(TextEvent e)
            {
                Unicode = Char.ConvertFromUtf32((int)e.Unicode);
            }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Mouse move event parameters
        /// </summary>
        ////////////////////////////////////////////////////////////
        public class MouseMoveEventArgs : EventArgs
        {
            ////////////////////////////////////////////////////////////

            /// <summary>X coordinate of the mouse cursor</summary>
            public int X;

            /// <summary>Y coordinate of the mouse cursor</summary>
            public int Y;

            /// <summary>
            /// Construct the mouse move arguments from a mouse move event
            /// </summary>
            /// <param name="e">Mouse move event</param>
            ////////////////////////////////////////////////////////////
            public MouseMoveEventArgs(MouseMoveEvent e)
            {
                X = e.X;
                Y = e.Y;
            }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Mouse buttons event parameters
        /// </summary>
        ////////////////////////////////////////////////////////////
        public class MouseButtonEventArgs : EventArgs
        {
            ////////////////////////////////////////////////////////////

            /// <summary>Code of the button (see MouseButton enum)</summary>
            public MouseButton Button;

            /// <summary>X coordinate of the mouse cursor</summary>
            public int X;

            /// <summary>Y coordinate of the mouse cursor</summary>
            public int Y;

            /// <summary>
            /// Construct the mouse button arguments from a mouse button event
            /// </summary>
            /// <param name="e">Mouse button event</param>
            ////////////////////////////////////////////////////////////
            public MouseButtonEventArgs(MouseButtonEvent e)
            {
                Button = e.Button;
                X = e.X;
                Y = e.Y;
            }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Mouse wheel event parameters
        /// </summary>
        ////////////////////////////////////////////////////////////
        public class MouseWheelEventArgs : EventArgs
        {
            ////////////////////////////////////////////////////////////

            /// <summary>Scroll amount</summary>
            public int Delta;

            /// <summary>
            /// Construct the mouse wheel arguments from a mouse wheel event
            /// </summary>
            /// <param name="e">Mouse wheel event</param>
            ////////////////////////////////////////////////////////////
            public MouseWheelEventArgs(MouseWheelEvent e)
            {
                Delta = e.Delta;
            }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Joystick axis move event parameters
        /// </summary>
        ////////////////////////////////////////////////////////////
        public class JoyMoveEventArgs : EventArgs
        {
            ////////////////////////////////////////////////////////////

            /// <summary>Joystick axis (see JoyAxis enum)</summary>
            public JoyAxis Axis;

            /// <summary>Index of the joystick which triggered the event</summary>
            public uint JoystickId;

            /// <summary>Current position of the axis</summary>
            public float Position;

            /// <summary>
            /// Construct the joystick move arguments from a joystick move event
            /// </summary>
            /// <param name="e">Joystick move event</param>
            ////////////////////////////////////////////////////////////
            public JoyMoveEventArgs(JoyMoveEvent e)
            {
                JoystickId = e.JoystickId;
                Axis = e.Axis;
                Position = e.Position;
            }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Joystick buttons event parameters
        /// </summary>
        ////////////////////////////////////////////////////////////
        public class JoyButtonEventArgs : EventArgs
        {
            ////////////////////////////////////////////////////////////

            /// <summary>Index of the button</summary>
            public uint Button;

            /// <summary>Index of the joystick which triggered the event</summary>
            public uint JoystickId;

            /// <summary>
            /// Construct the joystick button arguments from a joystick button event
            /// </summary>
            /// <param name="e">Joystick button event</param>
            ////////////////////////////////////////////////////////////
            public JoyButtonEventArgs(JoyButtonEvent e)
            {
                JoystickId = e.JoystickId;
                Button = e.Button;
            }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Size event parameters
        /// </summary>
        ////////////////////////////////////////////////////////////
        public class SizeEventArgs : EventArgs
        {
            ////////////////////////////////////////////////////////////

            /// <summary>New height of the window</summary>
            public uint Height;

            /// <summary>New width of the window</summary>
            public uint Width;

            /// <summary>
            /// Construct the size arguments from a size event
            /// </summary>
            /// <param name="e">Size event</param>
            ////////////////////////////////////////////////////////////
            public SizeEventArgs(SizeEvent e)
            {
                Width = e.Width;
                Height = e.Height;
            }
        }
    }
}