using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A collection of <see cref="TextBoxLine"/>s.
    /// </summary>
    public class TextBoxLines
    {
        readonly List<TextBoxLine> _lines = new List<TextBoxLine>();

        ushort _currentLineIndex;
        Font _font;
        int _maxLineLength = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxLines"/> class.
        /// </summary>
        public TextBoxLines()
        {
            // Add the initial line
            _currentLineIndex = 0;
            _lines.Add(new TextBoxLine(this));
        }

        /// <summary>
        /// Gets the <see cref="TextBoxLine"/> at the given index.
        /// </summary>
        /// <param name="lineIndex">The index of the line.</param>
        /// <returns>The <see cref="TextBoxLine"/> at the given index.</returns>
        public TextBoxLine this[int lineIndex]
        {
            get { return _lines[lineIndex]; }
        }

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
                var i = CurrentLineIndex;

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
        /// Gets the last line index.
        /// </summary>
        public int LastLineIndex
        {
            get { return _lines.Count - 1; }
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
        /// Gets the maximum length of a line in pixels. If this value is greater than zero, lines will be
        /// automatically broken to fit the length constraint. If this value is less than or equal to zero, no wrapping
        /// will be performed.
        /// </summary>
        public int MaxLineLength
        {
            get { return _maxLineLength; }
        }

        /// <summary>
        /// Appends a new line to this <see cref="TextBoxLines"/>.
        /// </summary>
        /// <param name="line">The <see cref="TextBoxLine"/> describing the line to append.</param>
        public void Append(TextBoxLine line)
        {
            line.TextBoxLines = this;
            _lines.Add(line);
        }

        /// <summary>
        /// Breaks a line into two pieces, and inserts the right side of the broken line into the next line. Essentially,
        /// just putting a line break into an existing line.
        /// </summary>
        /// <param name="lineIndex">The index of the line to break.</param>
        /// <param name="lineCharacterIndex">The index of the character in the line at which to split at. The actual
        /// split will happen immediately before the character with this index, so the character with this index
        /// will end up on the next line as the first character.</param>
        public void BreakLine(int lineIndex, int lineCharacterIndex)
        {
            // Perform the split
            TextBoxLine first;
            TextBoxLine second;
            _lines[lineIndex].SplitAt(lineCharacterIndex, out first, out second);

            first.TextBoxLines = this;
            second.TextBoxLines = this;

            // Insert the new line and re-set the reference of the current line
            _lines[lineIndex] = first;
            _lines.Insert(lineIndex + 1, second);
        }

        /// <summary>
        /// Breaks a line into two pieces, and inserts the right side of the broken line into the next line. Essentially,
        /// just putting a line break into an existing line.
        /// </summary>
        /// <param name="lineCharacterIndex">The index of the character in the line at which to split at. The actual
        /// split will happen immediately before the character with this index, so the character with this index
        /// will end up on the next line as the first character.</param>
        public void BreakLine(int lineCharacterIndex)
        {
            BreakLine(CurrentLineIndex, lineCharacterIndex);
        }

        /// <summary>
        /// Clears all of the text. Only one line will remain, and the line will be empty.
        /// </summary>
        public void Clear()
        {
            _lines.Clear();
            _lines.Add(new TextBoxLine(this));
            _currentLineIndex = 0;
        }

        /// <summary>
        /// Draws the specified range of lines.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="font">The <see cref="Font"/> to use.</param>
        /// <param name="start">The index of the first line to draw.</param>
        /// <param name="count">The maximum of lines to draw.</param>
        /// <param name="screenPos">The top-left corner of the location to begin drawing the text.</param>
        /// <param name="defaultColor">The default font color.</param>
        public void Draw(ISpriteBatch sb, Font font, int start, int count, Vector2 screenPos, Color defaultColor)
        {
            var end = Math.Min(start + count, _lines.Count);
            for (var i = start; i < end; i++)
            {
                var curr = _lines[i];
                curr.Draw(sb, font, screenPos, defaultColor);
                screenPos += new Vector2(0, font.GetLineSpacing());
            }
        }

        /// <summary>
        /// Gets the raw string of text that makes up all the lines of text.
        /// </summary>
        /// <returns>The raw string of text.</returns>
        public string GetRawText()
        {
            var lines = GetText();

            var sb = new StringBuilder();
            foreach (var line in lines)
            {
                foreach (var lineText in line)
                {
                    sb.Append(lineText.Text);
                }

                if (line != lines.Last())
                    sb.AppendLine();
            }

            return sb.ToString();

            /*
            StringBuilder sb = new StringBuilder(1024);
            for (int i = 0; i < _lines.Count; i++)
            {
                var curr = _lines[i];
                if (sb.Length > 0 && !curr.WasAutoBroken)
                    sb.AppendLine();
                sb.Append(curr.LineText);
            }

            return sb.ToString();
            */
        }

        /// <summary>
        /// Gets all of the text. If <see cref="MaxLineLength"/> is greater than zero, lines that were automatically
        /// broken from being too long will be concatenated back into a single line.
        /// </summary>
        /// <returns>A List containing a List of the <see cref="StyledText"/>s that each line is composed of.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<List<StyledText>> GetText()
        {
            var retLines = new List<List<StyledText>>();
            List<StyledText> currRetLine = null;

            for (var i = 0; i < _lines.Count; i++)
            {
                var curr = _lines[i];
                if (currRetLine == null || !curr.WasAutoBroken)
                {
                    if (currRetLine != null)
                        retLines.Add(currRetLine);
                    currRetLine = new List<StyledText>();
                }

                currRetLine.AddRange(curr.GetLineTexts);
            }

            if (currRetLine != null)
                retLines.Add(currRetLine);

            return retLines;
        }

        /// <summary>
        /// Gets the length of all of the text in all of the lines.
        /// </summary>
        /// <returns>The length of all the characters in the text box.</returns>
        public int GetTextLength()
        {
            return GetRawText().Length;
        }

        /// <summary>
        /// Inserts a line of text after the line the cursor is currently on.
        /// </summary>
        /// <param name="line">The <see cref="TextBoxLine"/> describing the line to insert.</param>
        public void Insert(TextBoxLine line)
        {
            Insert(CurrentLineIndex + 1, line);
        }

        /// <summary>
        /// Inserts a line of text.
        /// </summary>
        /// <param name="lineIndex">The index at which to insert the line.</param>
        /// <param name="line">The <see cref="TextBoxLine"/> describing the line to insert.</param>
        public void Insert(int lineIndex, TextBoxLine line)
        {
            line.TextBoxLines = this;
            _lines.Insert(lineIndex, line);
        }

        /// <summary>
        /// Joins the line at the given index with the line at the previous index.
        /// </summary>
        /// <param name="lineIndex">The index of the line that will be joined with the previous line. Must be
        /// greater than 0 and less than <see cref="Count"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="lineIndex"/> is less than or equal to 0, or greater
        /// than or equal to <see cref="Count"/>.</exception>
        public void JoinLineWithPrevious(int lineIndex)
        {
            if (lineIndex <= 0 || lineIndex >= Count)
                throw new ArgumentOutOfRangeException("lineIndex");

            var prevLine = _lines[lineIndex - 1];
            var currLine = _lines[lineIndex];

            // Append the line
            prevLine.Append(currLine);

            // Remove the appended line
            _lines.RemoveAt(lineIndex);
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
                    _lines.Add(new TextBoxLine(this));
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

        /// <summary>
        /// Moves to the line with the given index.
        /// </summary>
        /// <param name="lineIndex">Index of the line to move to.</param>
        /// <returns>True if the move was successful; otherwise false.</returns>
        public bool MoveTo(int lineIndex)
        {
            if (lineIndex < 0 || lineIndex >= Count)
                return false;

            _currentLineIndex = (ushort)lineIndex;

            return true;
        }

        /// <summary>
        /// A callback for the <see cref="TextBoxLine"/> to notify this <see cref="TextBoxLines"/> that the line
        /// was increased in length.
        /// </summary>
        /// <param name="line">The <see cref="TextBoxLine"/> that had text added to it.</param>
        internal void NotifyTextAdded(TextBoxLine line)
        {
            if (_font == null)
                return;

            var width = line.GetWidth(_font);
            if (width <= _maxLineLength)
                return;

            var newLines = StyledText.ToMultiline(line.GetLineTexts, false, _font, _maxLineLength);
            if (newLines.Count <= 1)
                return;

            // Avoid infinite recursion by not updating if the text didn't change
            var first = newLines[0];
            if (first.Sum(x => x.Text.Length) == line.LineText.Length &&
                StyledText.ToString(first).Equals(line.LineText, StringComparison.Ordinal))
                return;

            line.Clear();
            line.Append(newLines[0]);

            var lineIndex = _lines.IndexOf(line);

            if (lineIndex != _lines.Count - 1 && _lines[lineIndex + 1].WasAutoBroken)
            {
                // We're not at the last line AND the next line was auto-split, so keep inserting at the
                // start of the next line in reverse
                for (var i = newLines.Count - 1; i > 0; i--)
                {
                    foreach (var j in newLines[i].Reverse<StyledText>())
                    {
                        _lines[lineIndex + 1].Insert(j, 0);
                    }
                }
            }
            else
            {
                // We're at the last line or the next line was not an auto-split line
                for (var i = 1; i < newLines.Count; i++)
                {
                    var newLine = new TextBoxLine(this, true);
                    newLine.Append(newLines[i]);
                    _lines.Insert(lineIndex + i, newLine);
                }
            }
        }

        /// <summary>
        /// Rebuilds all of the lines and re-applies wrapping.
        /// </summary>
        void RebuildLines()
        {
            var contents = GetText();

            Clear();

            for (var i = 0; i < contents.Count; i++)
            {
                TextBoxLine newLine;
                if (i < _lines.Count)
                    newLine = _lines[i];
                else
                {
                    newLine = new TextBoxLine(this);
                    Append(newLine);
                }

                newLine.Append(contents[i]);
            }
        }

        /// <summary>
        /// Removes the maximum length requirement for a line.
        /// </summary>
        public void RemoveMaxLineLength()
        {
            if (_maxLineLength <= 0)
                return;

            _maxLineLength = 0;
            _font = null;

            RebuildLines();
        }

        /// <summary>
        /// Sets the maximum length for a line before it is automatically wrapped.
        /// </summary>
        /// <param name="maxLineLength">The maximum length of a line in pixels.</param>
        /// <param name="font">The font used on the text.</param>
        public void SetMaxLineLength(int maxLineLength, Font font)
        {
            // Check if we can avoid re-wrapping
            if (_maxLineLength <= 0 && maxLineLength <= 0)
            {
                _maxLineLength = maxLineLength;
                _font = font;
                return;
            }

            if (_font == font && _maxLineLength == maxLineLength)
                return;

            // Set the new values
            _maxLineLength = maxLineLength;
            _font = font;

            // Perform the wrapping by removing and adding all text
            if (_maxLineLength > 0 && font != null)
                RebuildLines();
        }

        /// <summary>
        /// Truncates the specified number of lines from the start of the collection.
        /// </summary>
        /// <param name="numLines">The number of lines to truncate.</param>
        public void Truncate(int numLines)
        {
            if (numLines <= 0)
                return;

            if (numLines >= _lines.Count)
                Clear();
            else
            {
                _lines.RemoveRange(0, numLines);
                _currentLineIndex = (ushort)Math.Max(0, _lines.Count - numLines);
                if (_currentLineIndex > _lines.Count)
                    _currentLineIndex = (ushort)(_lines.Count - 1);
            }
        }
    }
}