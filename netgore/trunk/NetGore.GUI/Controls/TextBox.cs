using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// NOTE: Still under heavy development by Spodi. Please don't touch this file.

namespace NetGore.Graphics.GUI
{
    public class TextBox : TextControl
    {
        readonly TextBoxLines _lines = new TextBoxLines();
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

            OnKeyPress += TextBox_OnKeyPress;

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
            Append(new StyledText(text, ForeColor));
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
        }

        /// <summary>
        /// Inserts the <paramref name="text"/> into the <see cref="TextBox"/> at the current cursor position.
        /// </summary>
        /// <param name="text">The text to insert.</param>
        public void Insert(StyledText text)
        {
            // TODO: ...
        }

        void TextBox_OnKeyPress(object sender, KeyboardEventArgs e)
        {
            bool shift = e.KeyboardState.IsKeyDown(Keys.LeftShift) || e.KeyboardState.IsKeyDown(Keys.RightShift);

            foreach (var key in e.Keys)
            {
                switch (key)
                {
                    case Keys.Delete:
                    case Keys.Back:
                        // TODO: Delete
                        break;

                    default:
                        Append(GetKeyString(key, shift));
                        break;
                }
            }
        }

        class TextBoxLine
        {
            readonly List<StyledText> _texts;
            string _text;

            /// <summary>
            /// Gets the last StyledText on this line.
            /// </summary>
            StyledText LastStyledText
            {
                get
                {
                    if (_texts.Count == 0)
                        return null;

                    return _texts[_texts.Count - 1];
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TextBoxLine"/> class.
            /// </summary>
            public TextBoxLine()
            {
                _text = string.Empty;
                _texts = new List<StyledText>(4);
            }

            /// <summary>
            /// Appends styled text to the line.
            /// </summary>
            /// <param name="text">The text to append.</param>
            public void Append(StyledText text)
            {
                if (text == null)
                    return;

                // Since this class is a single line, remove any line breaks
                text = text.ToSingleline();
                if (text.Text.Length == 0)
                    return;

                // Check if we can combine with the current last StyledText
                var last = LastStyledText;
                if (last != null && last.HasSameStyle(text))
                {
                    // Append to the existing StyledText
                    _texts[_texts.Count - 1] = last + text.Text;
                    _text += text.Text;
                }
                else
                {
                    // Add the new StyledText
                    _texts.Add(text);
                    _text += text.Text;
                }
            }

            public void Draw(SpriteBatch sb, SpriteFont font, Vector2 screenPos, Color defaultColor)
            {
                for (int i = 0; i < _texts.Count; i++)
                {
                    var curr = _texts[i];
                    curr.Draw(sb, font, screenPos, defaultColor);
                    if (i < _texts.Count - 1)
                        screenPos += new Vector2(curr.GetWidth(font), 0);
                }
            }
        }

        class TextBoxLines
        {
            readonly List<TextBoxLine> _lines = new List<TextBoxLine>();

            ushort _currentLineIndex;

            /// <summary>
            /// Gets the number of lines.
            /// </summary>
            public int Count
            {
                get { return _lines.Count; }
            }

            /// <summary>
            /// Gets the current line the cursor is on.
            /// </summary>
            public TextBoxLine CurrentLine
            {
                get
                {
                    int i = CurrentLineIndex;

                    // Do some quick bounds validating
                    if (i < 0)
                    {
                        Debug.Fail("Somehow got out of the valid index range.");
                        _currentLineIndex = 0;
                        i = _currentLineIndex;
                    }
                    else if (i >= _lines.Count)
                    {
                        Debug.Fail("Somehow got out of the valid index range.");
                        _currentLineIndex = (ushort)(_lines.Count - 1);
                        i = CurrentLineIndex;
                    }

                    return _lines[i];
                }
            }

            /// <summary>
            /// Gets the index of the current line the cursor is on.
            /// </summary>
            public int CurrentLineIndex
            {
                get { return _currentLineIndex; }
            }

            /// <summary>
            /// Gets the first line.
            /// </summary>
            public TextBoxLine FirstLine
            {
                get { return _lines[0]; }
            }

            /// <summary>
            /// Gets the last line.
            /// </summary>
            public TextBoxLine LastLine
            {
                get { return _lines[_lines.Count - 1]; }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TextBoxLines"/> class.
            /// </summary>
            public TextBoxLines()
            {
                // Add the initial line
                _currentLineIndex = 0;
                _lines.Add(new TextBoxLine());
            }

            /// <summary>
            /// Draws the specified range of lines.
            /// </summary>
            /// <param name="sb">The <see cref="SpriteBatch"/> to draw to.</param>
            /// <param name="font">The <see cref="SpriteFont"/> to use.</param>
            /// <param name="start">The index of the first line to draw.</param>
            /// <param name="count">The maximum of lines to draw.</param>
            /// <param name="screenPos">The top-left corner of the location to begin drawing the text.</param>
            /// <param name="defaultColor">The default font color.</param>
            public void Draw(SpriteBatch sb, SpriteFont font, int start, int count, Vector2 screenPos, Color defaultColor)
            {
                int end = Math.Min(start + count, _lines.Count - 1);
                for (int i = start; i <= end; i++)
                {
                    var curr = _lines[i];
                    curr.Draw(sb, font, screenPos, defaultColor);
                    screenPos += new Vector2(0, font.LineSpacing);
                }
            }

            /// <summary>
            /// Moves to the next line.
            /// </summary>
            /// <param name="createIfNotExist">If true and the cursor is already at the last line, a new line
            /// will be created.</param>
            /// <returns>True if the cursor moved to the next line; otherwise false.</returns>
            public bool MoveNext(bool createIfNotExist)
            {
                if (_currentLineIndex + 1 >= _lines.Count)
                {
                    if (createIfNotExist)
                    {
                        _currentLineIndex++;
                        _lines.Add(new TextBoxLine());
                        return true;
                    }
                }
                else
                {
                    _currentLineIndex++;
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Moves to the previous line.
            /// </summary>
            /// <returns>True if cursor moved to the prevous line; otherwise false.</returns>
            public bool MovePrevious()
            {
                if (_currentLineIndex == 0)
                    return false;

                _currentLineIndex--;
                return true;
            }
        }
    }
}