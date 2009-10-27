using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// NOTE: Still under heavy development by Spodi. Please don't touch this file.

namespace NetGore.Graphics.GUI
{
    public class TextBox : TextControl, IEditableText
    {
        readonly EditableTextHandler _editableTextHandler;
        readonly TextBoxLines _lines = new TextBoxLines();
        int _currentTime;

        /// <summary>
        /// The position of the cursor on the current line. This is equal to the index of the character the cursor
        /// is immediately before. For example, 2 would mean the cursor is immediately before the character on
        /// the line at index 2 (or between the second and 3rd character).
        /// </summary>
        int _cursorLinePosition = 0;

        bool _isMultiLine = false;

        /// <summary>
        /// Gets if this <see cref="TextBox"/> supports multiple lines.
        /// </summary>
        public bool IsMultiLine
        {
            get { return _isMultiLine; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        /// <param name="gui">GUIManager used by this Control.</param>
        /// <param name="settings">Settings for this TextControl.</param>
        /// <param name="font">SpriteFont used to write the text.</param>
        /// <param name="position">Position of the Control relative to its parent.</param>
        /// <param name="size">Size of the Control.</param>
        /// <param name="parent">Control that this Control belongs to.</param>
        public TextBox(GUIManagerBase gui, TextControlSettings settings, SpriteFont font, Vector2 position, Vector2 size,
                       Control parent) : base(gui, settings, string.Empty, font, position, size, parent)
        {
            IsVisible = true;
            CanDrag = false;
            CanFocus = true;

            OnKeyDown += TextBox_OnKeyDown;
            _editableTextHandler = new EditableTextHandler(this);

            Append("abcdef\nghi\r\njklj");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        /// <param name="gui">GUIManager used by this Control.</param>
        /// <param name="font">SpriteFont used to write the text.</param>
        /// <param name="position">Position of the Control relative to its parent.</param>
        /// <param name="size">Size of the Control.</param>
        /// <param name="parent">Control that this Control belongs to.</param>
        public TextBox(GUIManagerBase gui, SpriteFont font, Vector2 position, Vector2 size, Control parent)
            : this(gui, gui.TextBoxSettings, font, position, size, parent)
        {
        }

        /// <summary>
        /// Appends the <paramref name="text"/> to the <see cref="TextBox"/> at the end.
        /// </summary>
        /// <param name="text">The text to append.</param>
        public void Append(string text)
        {
            Append(new StyledText(text));
        }

        /// <summary>
        /// Appends the <paramref name="text"/> to the <see cref="TextBox"/> at the end.
        /// </summary>
        /// <param name="text">The text to append.</param>
        public void Append(StyledText text)
        {
            if (!IsMultiLine)
                _lines.LastLine.Append(text);
            else
            {
                // TODO: ...
            }
        }

        /// <summary>
        /// Draws the Control
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to</param>
        protected override void DrawControl(SpriteBatch spriteBatch)
        {
            base.DrawControl(spriteBatch);

            _lines.Draw(spriteBatch, Font, 0, _lines.Count, ScreenPosition, ForeColor);

            if (HasFocus && ((_currentTime % 1000) < 500)) // HACK: Grab the flash rate from registry
                DrawCursor(spriteBatch);
        }

        void DrawCursor(SpriteBatch sb)
        {
            int offset = 0;
            if (_cursorLinePosition > 0)
                offset = (int)Font.MeasureString(_lines.CurrentLine.LineText.Substring(0, _cursorLinePosition)).X;

            var sp = ScreenPosition;
            var p1 = sp + new Vector2(offset, 0);
            var p2 = p1 + new Vector2(0, Font.LineSpacing);

            XNALine.Draw(sb, p1, p2, Color.Black);
        }

        /// <summary>
        /// Inserts the <paramref name="text"/> into the <see cref="TextBox"/> at the current cursor position.
        /// </summary>
        /// <param name="text">The text to insert.</param>
        public void Insert(StyledText text)
        {
            _lines.CurrentLine.Insert(text, _cursorLinePosition);
            ((IEditableText)this).MoveCursor(MoveCursorDirection.Right);
        }

        void TextBox_OnKeyDown(object sender, KeyboardEventArgs e)
        {
            // Notify of only the last key pressed
            _editableTextHandler.NotifyKeyPressed(e.Keys.LastOrDefault());
        }

        /// <summary>
        /// Updates the <see cref="Control"/> for anything other than the mouse or keyboard.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected override void UpdateControl(int currentTime)
        {
            _currentTime = currentTime;

            _editableTextHandler.Update(currentTime, GUIManager.KeyboardState);
            base.UpdateControl(currentTime);
        }

        #region IEditableText Members

        /// <summary>
        /// Inserts the specified character to the <see cref="Control"/>'s text at the current position
        /// of the text cursor.
        /// </summary>
        /// <param name="c">The character to insert.</param>
        void IEditableText.InsertChar(string c)
        {
            _lines.CurrentLine.Insert(c, _cursorLinePosition);
            ((IEditableText)this).MoveCursor(MoveCursorDirection.Right);
        }

        /// <summary>
        /// Deletes the character from the <see cref="Control"/>'s text immediately before the current position
        /// of the text cursor. When applicable, if the cursor is at the start of the line, the cursor be moved
        /// to the previous line and the remainder of the line will be appended to the end of the previous line.
        /// </summary>
        void IEditableText.DeleteChar()
        {
            // TODO: ...
        }

        /// <summary>
        /// Moves the cursor in the specified <paramref name="direction"/> by one character.
        /// </summary>
        /// <param name="direction">The direction to move the cursor.</param>
        void IEditableText.MoveCursor(MoveCursorDirection direction)
        {
            switch (direction)
            {
                case MoveCursorDirection.Left:
                    if (_cursorLinePosition > 0)
                        _cursorLinePosition--;
                    else
                    {
                        if (IsMultiLine && _lines.MovePrevious())
                        {
                            _cursorLinePosition = _lines.CurrentLine.LineText.Length + 1;
                        }
                        else
                        {
                            _cursorLinePosition = 0;
                        }
                    }
                    break;

                case MoveCursorDirection.Right:
                    if (_cursorLinePosition < _lines.CurrentLine.LineText.Length)
                        _cursorLinePosition++;
                    else
                    {
                        if (IsMultiLine && _lines.MoveNext(false))
                        {
                            _cursorLinePosition = 0;
                        }
                        else
                        {
                            _cursorLinePosition = _lines.CurrentLine.LineText.Length;
                        }
                    }
                    break;

                case MoveCursorDirection.Up:
                    _lines.MovePrevious();
                    break;

                case MoveCursorDirection.Down:
                    _lines.MoveNext(false);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        /// <summary>
        /// Breaks the line at the current position of the text cursor.
        /// </summary>
        public void BreakLine()
        {
            // TODO: ...
        }

        #endregion
    }
}