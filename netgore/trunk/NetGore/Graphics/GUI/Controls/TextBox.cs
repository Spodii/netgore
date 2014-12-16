using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A box of text.
    /// </summary>
    public class TextBox : TextControl, IEditableText
    {
        /// <summary>
        /// The name of this <see cref="Control"/> for when looking up the skin information.
        /// </summary>
        const string _controlSkinName = "TextBox";

        readonly EditableTextHandler _editableTextHandler;
        readonly TextBoxLines _lines = new TextBoxLines();
        readonly NumCharsToDrawCache _numCharsToDraw = new NumCharsToDrawCache();

        int _bufferTruncateSize = 100;

        TickCount _currentTime;

        /// <summary>
        /// Keeps track of the time at which the cursor started as visible.
        /// </summary>
        TickCount _cursorBlinkTimer;

        int _cursorLinePosition = 0;
        bool _hasTextChanged = false;

        bool _isMultiLine = true;

        /// <summary>
        /// The index of the first line to draw.
        /// </summary>
        int _lineBufferOffset = 0;

        int _lineCharBufferOffset = 0;
        int _maxBufferSize = 200;

        /// <summary>
        /// The maximum number of visible lines to draw (how many lines fit into the visible area).
        /// </summary>
        int _maxVisibleLines = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextControl"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public TextBox(Control parent, Vector2 position, Vector2 clientSize) : base(parent, position, clientSize)
        {
            _numCharsToDraw.TextBox = this;
            _editableTextHandler = new EditableTextHandler(this);

            // Set the initial line length and number of visible lines
            UpdateMaxLineLength();
            UpdateMaxVisibleLines();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextControl"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        public TextBox(IGUIManager guiManager, Vector2 position, Vector2 clientSize) : base(guiManager, position, clientSize)
        {
            _numCharsToDraw.TextBox = this;
            _editableTextHandler = new EditableTextHandler(this);

            // Set the initial line length and number of visible lines
            UpdateMaxLineLength();
            UpdateMaxVisibleLines();
        }

        /// <summary>
        /// Gets or sets a <see cref="Func{T,U}"/> used to determine if only certain keys should be accepted when entering
        /// text into this <see cref="TextBox"/>. If null, all of the default keys will be accepted. Default is null.
        /// </summary>
        public Func<TextEventArgs, bool> AllowKeysHandler { get; set; }

        /// <summary>
        /// Gets or sets the number of lines to trim the buffer down to when the number of lines in this
        /// <see cref="TextBox"/> has exceeded the <see cref="TextBox.MaxBufferSize"/>. For best performance, this
        /// value should be at least 20% less than the <see cref="TextBox.MaxBufferSize"/> since truncating has nearly
        /// the same overhead no matter how many lines are truncated. This value should always be less than the
        /// <see cref="TextBox.MaxBufferSize"/>.
        /// By default, this value is equal to 100.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than zero.</exception>
        [SyncValue]
        [DefaultValue(100)]
        public int BufferTruncateSize
        {
            get { return _bufferTruncateSize; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");

                _bufferTruncateSize = value;
            }
        }

        /// <summary>
        /// Gets or set the position of the cursor on the current line. This is equal to the index of the character
        /// the cursor is immediately before. For example, 2 would mean the cursor is immediately before the character
        /// on the line at index 2 (or between the second and 3rd character).
        /// </summary>
        public int CursorLinePosition
        {
            get { return _cursorLinePosition; }
            set
            {
                _cursorLinePosition = value;
                EnsureCursorPositionValid();
                EnsureHorizontalOffsetValid();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Font"/> used by the <see cref="TextControl"/>.
        /// </summary>
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                if (base.Font == value)
                    return;

                base.Font = value;

                UpdateMaxVisibleLines();
                UpdateMaxLineLength();
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="Func{T,U}"/> used to determine if certain keys should be ignored when entering text into
        /// this <see cref="TextBox"/>. If null, the default keys will be ignored. Default is null.
        /// </summary>
        public Func<TextEventArgs, bool> IgnoreKeysHandler { get; set; }

        /// <summary>
        /// Gets or sets if this <see cref="TextBox"/> supports multiple lines. When changing from multiple lines to
        /// a single line, all line breaks will be forever lost and replaced with a space instead.
        /// </summary>
        [SyncValue]
        public virtual bool IsMultiLine
        {
            get { return _isMultiLine; }
            set
            {
                if (_isMultiLine == value)
                    return;

                _isMultiLine = value;

                UpdateMaxLineLength();
                UpdateMaxVisibleLines();

                // Re-add all the content
                var contents = _lines.GetText();
                Clear();

                foreach (var line in contents)
                {
                    if (line.Count > 0)
                        AppendLine(line);
                }
            }
        }

        /// <summary>
        /// Gets or sets the index of the first line to draw. Value must be greater than or equal to zero and less than
        /// or equal <see cref="LineCount"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than zero or greater
        /// than <see cref="LineCount"/>.</exception>
        public int LineBufferOffset
        {
            get { return _lineBufferOffset; }
            set
            {
                if (_lineBufferOffset == value)
                    return;

                if (value < 0 || value > LineCount)
                    throw new ArgumentOutOfRangeException("value");

                _lineBufferOffset = value;
            }
        }

        /// <summary>
        /// Gets or sets the index of the first character in the line to draw.
        /// </summary>
        protected int LineCharBufferOffset
        {
            get { return _lineCharBufferOffset; }
            set
            {
                _lineCharBufferOffset = value;
                _numCharsToDraw.Invalidate();
                EnsureHorizontalOffsetValid();
            }
        }

        /// <summary>
        /// Gets the number of lines in the <see cref="TextBox"/>.
        /// </summary>
        public int LineCount
        {
            get { return _lines.Count; }
        }

        /// <summary>
        /// Gets the lines in the <see cref="TextBox"/>.
        /// </summary>
        protected TextBoxLines Lines
        {
            get { return _lines; }
        }

        /// <summary>
        /// Gets or sets the maximum line buffer of a multi-line <see cref="TextBox"/>. Once this value has been reached,
        /// the number of lines in the <see cref="TextBox"/> will be reduced to <see cref="TextBox.BufferTruncateSize"/>.
        /// If less than or equal to zero, truncating will not be performed.
        /// By default, this value is equal to 200.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than or equal to zero.</exception>
        [SyncValue]
        [DefaultValue(200)]
        public int MaxBufferSize
        {
            get { return _maxBufferSize; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");

                _maxBufferSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum length of the Text in characters. If this value is less than or equal to 0, then
        /// no limit will be assumed. The limitation only applies to input characters through key events. This limit
        /// can still be surpassed by manually setting the text.
        /// </summary>
        [SyncValue]
        public int MaxInputTextLength { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets the maximum number of possible visible lines. Only valid is <see cref="IsMultiLine"/> is true.
        /// </summary>
        public int MaxVisibleLines
        {
            get { return _maxVisibleLines; }
        }

        /// <summary>
        /// Gets the number of characters to draw in a row. Only used if <see cref="IsMultiLine"/> is false, since otherwise
        /// we just use word wrapping.
        /// </summary>
        protected int NumCharsToDraw
        {
            get { return _numCharsToDraw; }
        }

        /// <summary>
        /// Gets or sets the text in this <see cref="TextBox"/>. Please beware that setting the text through this
        /// method will result in all font styling to be lost.
        /// </summary>
        public override string Text
        {
            get { return _lines.GetRawText(); }
            set
            {
                if (value == null)
                    value = string.Empty;

                if (StringComparer.Ordinal.Equals(_lines.GetRawText(), value))
                    return;

                CursorLinePosition = 0;
                Clear();
                Append(value);
            }
        }

        /// <summary>
        /// Appends the <paramref name="text"/> to the <see cref="TextBox"/> at the end.
        /// </summary>
        /// <param name="text">The text to append.</param>
        public void Append(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            Append(new StyledText(text));
        }

        /// <summary>
        /// Appends the <paramref name="text"/> to the <see cref="TextBox"/> at the end.
        /// </summary>
        /// <param name="text">The text to append.</param>
        public void Append(IEnumerable<StyledText> text)
        {
            foreach (var t in text)
            {
                Append(t);
            }
        }

        /// <summary>
        /// Appends the <paramref name="text"/> to the <see cref="TextBox"/> at the end.
        /// </summary>
        /// <param name="text">The text to append.</param>
        public void Append(StyledText text)
        {
            if (text.Text.Length == 0)
                return;

            if (!IsMultiLine)
                _lines.LastLine.Append(text.ToSingleline());
            else
            {
                var textLines = StyledText.ToMultiline(text);
                _lines.LastLine.Append(textLines[0]);

                for (var i = 1; i < textLines.Count; i++)
                {
                    var newLine = new TextBoxLine(_lines);
                    _lines.Insert(_lines.Count, newLine);
                    newLine.Append(textLines[i]);
                }

                TruncateIfNeeded();
            }

            _numCharsToDraw.Invalidate();

            _hasTextChanged = true;
        }

        /// <summary>
        /// Appends the <paramref name="text"/> to the <see cref="TextBox"/> at the end, then inserts a line break.
        /// </summary>
        /// <param name="text">The text to append.</param>
        public void AppendLine(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            AppendLine(new StyledText(text));
        }

        /// <summary>
        /// Appends the <paramref name="text"/> to the <see cref="TextBox"/> at the end, then inserts a line break.
        /// </summary>
        /// <param name="text">The text to append.</param>
        public void AppendLine(StyledText text)
        {
            Append(text);

            if (IsMultiLine)
            {
                _lines.Insert(_lines.Count, new TextBoxLine(_lines));
                TruncateIfNeeded();
            }
        }

        /// <summary>
        /// Appends the <paramref name="text"/> to the <see cref="TextBox"/> at the end, then inserts a line break.
        /// </summary>
        /// <param name="text">The text to append.</param>
        public void AppendLine(IEnumerable<StyledText> text)
        {
            foreach (var t in text)
            {
                Append(t);
            }

            if (IsMultiLine)
            {
                _lines.Insert(_lines.Count, new TextBoxLine(_lines));
                TruncateIfNeeded();
            }
            else
                Append(" ");
        }

        /// <summary>
        /// Clears all the text in this <see cref="TextBox"/>.
        /// </summary>
        public void Clear()
        {
            _cursorLinePosition = 0;
            _lineBufferOffset = 0;
            _lineCharBufferOffset = 0;
            _lines.Clear();
            _hasTextChanged = true;

            Debug.Assert(_lines.Count == 1);
            Debug.Assert(_lines.CurrentLineIndex == 0);
            Debug.Assert(_cursorLinePosition == 0);
        }

        /// <summary>
        /// Draws the Control.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        protected override void DrawControl(ISpriteBatch spriteBatch)
        {
            base.DrawControl(spriteBatch);

            // Get the text offset
            var textDrawOffset = ScreenPosition + new Vector2(Border.LeftWidth, Border.TopHeight);

            // Draw the text
            DrawControlText(spriteBatch, textDrawOffset);

            // Draw the text cursor
            DrawCursor(spriteBatch, textDrawOffset);
        }

        /// <summary>
        /// Draws the text to display for the <see cref="TextBox"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="offset">The offset to draw the text at.</param>
        protected virtual void DrawControlText(ISpriteBatch spriteBatch, Vector2 offset)
        {
            if (IsMultiLine)
                _lines.Draw(spriteBatch, Font, LineBufferOffset, MaxVisibleLines, offset, ForeColor);
            else
                _lines.FirstLine.Draw(spriteBatch, Font, offset, ForeColor, LineCharBufferOffset, _numCharsToDraw);
        }

        /// <summary>
        /// Draws the text insertion cursor.
        /// </summary>
        /// <param name="sb">The <see cref="SpriteBatch"/> to draw to.</param>
        /// <param name="textPos">The screen position to start drawing the text at.</param>
        void DrawCursor(ISpriteBatch sb, Vector2 textPos)
        {
            if (!HasFocus || !IsEnabled)
                return;

            if (_cursorBlinkTimer + EditableTextHandler.CursorBlinkRate < _currentTime)
                return;

            var offset = 0;
            if (CursorLinePosition > 0)
            {
                var len = Math.Min(CursorLinePosition, _lines.CurrentLine.LineText.Length);
                offset =
                    GetTextCursorOffset(_lines.CurrentLine.LineText.Substring(LineCharBufferOffset, len - LineCharBufferOffset));
            }

            var visibleLineOffset = (_lines.CurrentLineIndex - LineBufferOffset) * Font.GetLineSpacing();

            var p1 = textPos + new Vector2(offset, visibleLineOffset);
            var p2 = p1 + new Vector2(0, Font.GetLineSpacing());

            RenderLine.Draw(sb, p1, p2, Color.Black);
        }

        void EnsureCursorPositionValid()
        {
            // Make sure the is on a character that exists in the line
            if (CursorLinePosition < 0)
                _cursorLinePosition = 0;
            else if (CursorLinePosition > _lines.CurrentLine.LineText.Length)
                _cursorLinePosition = _lines.CurrentLine.LineText.Length;

            if (IsMultiLine)
            {
                // Change the buffer so that the line the cursor is on is visible
                if (LineBufferOffset < _lines.CurrentLineIndex - MaxVisibleLines + 1)
                    LineBufferOffset = _lines.CurrentLineIndex - MaxVisibleLines + 1;
                else if (LineBufferOffset > _lines.CurrentLineIndex)
                    LineBufferOffset = _lines.CurrentLineIndex;
            }
            else
                EnsureHorizontalOffsetValid();
        }

        void EnsureHorizontalOffsetValid()
        {
            if (IsMultiLine)
                return;

            // Make sure the cursor is in view
            if (LineCharBufferOffset < _cursorLinePosition - _numCharsToDraw)
                _lineCharBufferOffset = _cursorLinePosition - _numCharsToDraw;
            else if (LineCharBufferOffset > _cursorLinePosition)
                _lineCharBufferOffset = _cursorLinePosition;

            // Make sure we don't move the buffer more than we need to (show as many characters as possible on the right side)
            var firstFitting = _lines.CurrentLine.CountFittingCharactersRight(Font, (int)ClientSize.X);
            if (_lineCharBufferOffset > firstFitting)
                _lineCharBufferOffset = firstFitting;

            // Double-check we are in a legal range
            if (_lineCharBufferOffset < 0)
                _lineCharBufferOffset = 0;
            else if (_lineCharBufferOffset > _lines.CurrentLine.LineText.Length)
                _lineCharBufferOffset = _lines.CurrentLine.LineText.Length;
        }

        void GetCharacterAtPoint(Vector2 point, out int lineIndex, out int lineCharIndex)
        {
            // Get the line
            if (IsMultiLine)
            {
                lineIndex = LineBufferOffset + (int)Math.Floor(point.Y / Font.GetLineSpacing());
                if (lineIndex >= _lines.Count)
                    lineIndex = _lines.Count - 1;
            }
            else
                lineIndex = 0;

            // Get the character
            if (IsMultiLine)
                lineCharIndex = StyledText.FindLastFittingChar(_lines[lineIndex].LineText, Font, (int)point.X);
            else
            {
                var substr = _lines[lineIndex].LineText.Substring(LineCharBufferOffset);
                lineCharIndex = StyledText.FindLastFittingChar(substr, Font, (int)point.X);
                lineCharIndex += LineCharBufferOffset;
            }

            if (lineCharIndex > _lines[lineIndex].LineText.Length)
                lineCharIndex = _lines[lineIndex].LineText.Length;
        }

        /// <summary>
        /// Gets the offset for the text cursor for the given text. This is usually just the length of the text.
        /// </summary>
        /// <param name="text">The text to get the offset for.</param>
        /// <returns>The X offset for the cursor.</returns>
        protected virtual int GetTextCursorOffset(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            return (int)Font.MeasureString(text).X;
        }

        /// <summary>
        /// Inserts the <paramref name="text"/> into the <see cref="TextBox"/> at the current cursor position.
        /// </summary>
        /// <param name="text">The text to insert.</param>
        public void Insert(StyledText text)
        {
            _lines.CurrentLine.Insert(text, CursorLinePosition);
            ((IEditableText)this).MoveCursor(MoveCursorDirection.Right);
            _numCharsToDraw.Invalidate();
            TruncateIfNeeded();
        }

        /// <summary>
        /// When overridden in the derived class, loads the skinning information for the <see cref="Control"/>
        /// from the given <paramref name="skinManager"/>.
        /// </summary>
        /// <param name="skinManager">The <see cref="ISkinManager"/> to load the skinning information from.</param>
        public override void LoadSkin(ISkinManager skinManager)
        {
            Border = skinManager.GetBorder(_controlSkinName);
        }

        /// <summary>
        /// Handles when this <see cref="Control"/> was clicked.
        /// This is called immediately before <see cref="Control.OnClick"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnClick"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnClick(MouseButtonEventArgs e)
        {
            base.OnClick(e);

            if (!IsEnabled || !IsVisible)
                return;

            int lineIndex;
            int lineCharIndex;
            GetCharacterAtPoint(e.Location(), out lineIndex, out lineCharIndex);

            _lines.MoveTo(lineIndex);
            CursorLinePosition = lineCharIndex;
        }

        /// <summary>
        /// Handles when a key is being pressed while the <see cref="Control"/> has focus.
        /// This is called immediately before <see cref="Control.KeyPressed"/>.
        /// Override this method instead of using an event hook on <see cref="Control.KeyPressed"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnKeyPressed(KeyEventArgs e)
        {
            if (_editableTextHandler == null)
                return;

            _editableTextHandler.HandleKey(e);

            base.OnKeyPressed(e);
        }

        /// <summary>
        /// Handles when the <see cref="Control.Size"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.Resized"/>.
        /// Override this method instead of using an event hook on <see cref="Control.Resized"/> when possible.
        /// </summary>
        protected override void OnResized()
        {
            base.OnResized();

            if (IsMultiLine)
            {
                UpdateMaxLineLength();
                UpdateMaxVisibleLines();
            }
            else
                _numCharsToDraw.Invalidate();
        }

        /// <summary>
        /// Handles when text has been entered into this <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.TextEntered"/>.
        /// Override this method instead of using an event hook on <see cref="Control.TextEntered"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnTextEntered(TextEventArgs e)
        {
            if (_editableTextHandler == null)
                return;

            if (!IsEnabled)
                return;

            if (IgnoreKeysHandler != null && IgnoreKeysHandler(e))
                return;

            if (AllowKeysHandler != null && !AllowKeysHandler(e))
                return;

            _editableTextHandler.HandleText(e);

            base.OnTextEntered(e);

            _hasTextChanged = true;
        }

        void ResetCursorBlink()
        {
            _cursorBlinkTimer = _currentTime;
        }

        /// <summary>
        /// Resizes the <see cref="TextBox"/> to the minimum size needed to fit all of the text.
        /// </summary>
        /// <param name="maxWidth">If greater than 0, the width of the <see cref="TextBox"/> will not exceed this value.</param>
        /// <param name="maxHeight">If greater than 0, the height of the <see cref="TextBox"/> will not exceed this value.</param>
        public void ResizeToFitText(int maxWidth = 0, int maxHeight = 0)
        {
            var reqWidth = 2;
            var reqHeight = 2;

            // Reset the view to the first line
            CursorLinePosition = 0;
            LineBufferOffset = 0;
            LineCharBufferOffset = 0;

            // Get the required width
            for (var i = 0; i < LineCount; i++)
            {
                var line = _lines[i];
                var width = line.GetWidth(Font);

                if (width > reqWidth)
                    reqWidth = width;
            }

            if (LineCount > 0)
            {
                // Find the required height by using the number of lines and the spacing between lines
                reqHeight = LineCount * Font.GetLineSpacing();
            }

            // Keep in bounds of the max sizes
            if (maxWidth > 0 && reqWidth > maxWidth)
                reqWidth = maxWidth;

            if (maxHeight > 0 && reqHeight > maxHeight)
                reqHeight = maxHeight;

            // Resize
            ClientSize = new Vector2(reqWidth, reqHeight);
        }

        /// <summary>
        /// Checks if the <see cref="TextBox"/>'s lines need to be truncated, and if they do, truncates.
        /// </summary>
        void TruncateIfNeeded()
        {
            // Can't truncate if not multiline or no max buffer size has been set
            if (!IsMultiLine || MaxBufferSize <= 0)
                return;

            // Check if we have exceeded the maximum buffer size
            if (_lines.Count <= MaxBufferSize)
                return;

            // Truncate down to the BufferTruncateSize
            _lines.Truncate(_lines.Count - BufferTruncateSize);

            // Update some values to ensure they are valid for our new size
            if (LineBufferOffset >= _lines.Count)
                LineBufferOffset = _lines.Count - 1;

            EnsureHorizontalOffsetValid();
            EnsureCursorPositionValid();
            _numCharsToDraw.Invalidate();
        }

        /// <summary>
        /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
        /// not visible.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected override void UpdateControl(TickCount currentTime)
        {
            _currentTime = currentTime;

            if (_cursorBlinkTimer + (EditableTextHandler.CursorBlinkRate * 2) < currentTime)
                ResetCursorBlink();

            base.UpdateControl(currentTime);

            if (_hasTextChanged)
            {
                _hasTextChanged = false;
                InvokeTextChanged();
            }
        }

        /// <summary>
        /// Updates the maximum line length set in the <see cref="TextBoxLines"/> (<see cref="_lines"/>).
        /// </summary>
        void UpdateMaxLineLength()
        {
            if (IsMultiLine)
                _lines.SetMaxLineLength((int)ClientSize.X, Font);
            else
                _lines.RemoveMaxLineLength();
        }

        /// <summary>
        /// Updates the count of the maximum number of visible lines (for multi-line <see cref="TextBox"/>es).
        /// </summary>
        void UpdateMaxVisibleLines()
        {
            if (Font == null)
                return;

            if (IsMultiLine)
                _maxVisibleLines = (int)Math.Floor(ClientSize.Y / Font.GetLineSpacing());
        }

        #region IEditableText Members

        /// <summary>
        /// Breaks the line at the current position of the text cursor.
        /// </summary>
        public void BreakLine()
        {
            if (!IsMultiLine)
                return;

            _lines.BreakLine(CursorLinePosition);

            // Move the cursor to the first character of the next line
            if (_lines.MoveNext(false))
                CursorLinePosition = 0;

            ResetCursorBlink();

            InvokeTextChanged();
        }

        /// <summary>
        /// Deletes the character from the <see cref="Control"/>'s text immediately before the current position
        /// of the text cursor. When applicable, if the cursor is at the start of the line, the cursor be moved
        /// to the previous line and the remainder of the line will be appended to the end of the previous line.
        /// </summary>
        void IEditableText.DeleteCharLeft()
        {
            var charToDelete = CursorLinePosition - 1;
            if (charToDelete < 0)
            {
                if (IsMultiLine && _lines.CurrentLineIndex > 0)
                {
                    var oldLineLength = _lines[_lines.CurrentLineIndex - 1].LineText.Length;

                    // Join the current line to the previous line
                    _lines.JoinLineWithPrevious(_lines.CurrentLineIndex);

                    // Move the cursor to the previous line at where the line used to end
                    _lines.MovePrevious();
                    CursorLinePosition = oldLineLength;
                }
            }
            else
            {
                // Delete the character behind the cursor
                _lines.CurrentLine.Remove(charToDelete);
                CursorLinePosition--;
            }

            _numCharsToDraw.Invalidate();

            ResetCursorBlink();

            InvokeTextChanged();
        }

        /// <summary>
        /// Deletes the character from the <see cref="Control"/>'s text immediately after the current position
        /// of the text cursor. When applicable, if the cursor is at the end of the line, the cursor be moved
        /// to the next line and the remainder of the line will be appended to the start of the previous line.
        /// </summary>
        void IEditableText.DeleteCharRight()
        {
            var charToDelete = CursorLinePosition;

            if (_lines.CurrentLine.LineText.Length == charToDelete)
            {
                if (IsMultiLine && _lines.Count > 1)
                {
                    _lines.JoinLineWithPrevious(_lines.CurrentLineIndex + 1);
                }
            }
            else
            {
                // Delete the character in front of the cursor
                if (charToDelete < _lines.CurrentLine.LineText.Length)
                    _lines.CurrentLine.Remove(charToDelete);
            }

            _numCharsToDraw.Invalidate();

            ResetCursorBlink();

            InvokeTextChanged();
        }

        /// <summary>
        /// Inserts the specified character to the <see cref="Control"/>'s text at the current position
        /// of the text cursor.
        /// </summary>
        /// <param name="c">The character to insert.</param>
        void IEditableText.InsertChar(string c)
        {
            // HACK: This stuff with lineOldLen and oldLine is a cheap way to make the cursor move to the end of the text
            // removed from one line and appended to the next when the line breaks
            var lineOldLen = _lines.CurrentLine.LineText.Length;
            var oldLine = _lines.CurrentLineIndex;

            if (MaxInputTextLength > 0 && _lines.GetTextLength() >= MaxInputTextLength)
                return;

            _lines.CurrentLine.Insert(c, CursorLinePosition);
            ((IEditableText)this).MoveCursor(MoveCursorDirection.Right);

            if (IsMultiLine && oldLine < _lines.CurrentLineIndex && oldLine < _lines.Count)
            {
                for (var i = 0; i < lineOldLen - _lines[oldLine].LineText.Length + 1; i++)
                {
                    ((IEditableText)this).MoveCursor(MoveCursorDirection.Right);
                }
            }

            _numCharsToDraw.Invalidate();

            ResetCursorBlink();

            InvokeTextChanged();
        }

        /// <summary>
        /// Moves the cursor in the specified <paramref name="direction"/> by one character.
        /// </summary>
        /// <param name="direction">The direction to move the cursor.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="direction"/> is not a defined value of the
        /// <see cref="MoveCursorDirection"/> enum.</exception>
        void IEditableText.MoveCursor(MoveCursorDirection direction)
        {
            switch (direction)
            {
                case MoveCursorDirection.Left:
                    if (CursorLinePosition > 0)
                        CursorLinePosition--;
                    else
                    {
                        if (IsMultiLine && _lines.MovePrevious())
                            CursorLinePosition = _lines.CurrentLine.LineText.Length;
                        else
                            CursorLinePosition = 0;
                    }

                    ResetCursorBlink();
                    break;

                case MoveCursorDirection.Right:
                    if (CursorLinePosition < _lines.CurrentLine.LineText.Length)
                        CursorLinePosition++;
                    else
                    {
                        if (IsMultiLine && _lines.MoveNext(false))
                            CursorLinePosition = 0;
                        else
                            CursorLinePosition = _lines.CurrentLine.LineText.Length;
                    }

                    ResetCursorBlink();
                    break;

                case MoveCursorDirection.Up:
                    if (!IsMultiLine)
                        break;

                    if (_lines.MovePrevious())
                    {
                        // Line changed, so update the cursor position
                        EnsureCursorPositionValid();
                    }

                    ResetCursorBlink();
                    break;

                case MoveCursorDirection.Down:
                    if (!IsMultiLine)
                        break;

                    if (_lines.MoveNext(false))
                    {
                        // Line changed, so update the cursor position
                        EnsureCursorPositionValid();
                    }

                    ResetCursorBlink();
                    break;

                case MoveCursorDirection.Start:
                    if (IsMultiLine)
                        _lines.MoveTo(0);

                    CursorLinePosition = 0;
                    ResetCursorBlink();
                    break;

                case MoveCursorDirection.End:
                    if (IsMultiLine)
                        _lines.MoveTo(_lines.LastLineIndex);

                    CursorLinePosition = _lines.CurrentLine.LineText.Length;
                    ResetCursorBlink();
                    break;

                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        #endregion

        /// <summary>
        /// Contains and manages the number of characters to draw. This allows for an easy means of calculating
        /// the value only when it is needed, allowing the value to be invalidated multiple times without updating
        /// each time it is invalidated.
        /// </summary>
        class NumCharsToDrawCache
        {
            const short _invalidateValue = -1;

            short _value = _invalidateValue;

            public TextBox TextBox { get; set; }

            /// <summary>
            /// Gets the number of characters to draw.
            /// </summary>
            int Value
            {
                get
                {
                    if (_value == _invalidateValue)
                        Update();

                    return _value;
                }
            }

            /// <summary>
            /// Invalidates the cached value. Use whenever the line changes.
            /// </summary>
            public void Invalidate()
            {
                _value = _invalidateValue;
            }

            /// <summary>
            /// Updates the cached value.
            /// </summary>
            void Update()
            {
                if (TextBox == null)
                    return;

                var currLine = TextBox._lines.CurrentLine;

                _value =
                    (short)
                    currLine.CountFittingCharactersLeft(TextBox.Font, TextBox.LineCharBufferOffset, (int)TextBox.ClientSize.X);
            }

            /// <summary>
            /// Performs an implicit conversion from <see cref="NetGore.Graphics.GUI.TextBox.NumCharsToDrawCache"/> to
            /// <see cref="System.Int32"/>.
            /// </summary>
            /// <param name="c">The <see cref="NumCharsToDrawCache"/>.</param>
            /// <returns>The result of the conversion.</returns>
            public static implicit operator int(NumCharsToDrawCache c)
            {
                return c.Value;
            }
        }
    }
}