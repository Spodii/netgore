using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A collection of <see cref="TextBoxLine"/>s.
    /// </summary>
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