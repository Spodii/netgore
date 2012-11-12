using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Improves and simplifies the drawing of multiple <see cref="StyledText"/>s at the cost of a little
    /// initialization overhead.
    /// </summary>
    public class StyledTextsDrawer
    {
        readonly List<StyledTextWithPosition> _textsWithPos = new List<StyledTextWithPosition>();
        Font _font;

        IEnumerable<IEnumerable<StyledText>> _texts;

        /// <summary>
        /// Initializes a new instance of the <see cref="StyledTextsDrawer"/> class.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <exception cref="ArgumentNullException"><paramref name="font" /> is <c>null</c>.</exception>
        public StyledTextsDrawer(Font font)
        {
            if (font == null)
                throw new ArgumentNullException("font");

            _font = font;
        }

        /// <summary>
        /// Gets or sets the <see cref="Font"/> used to draw. Cannot be null.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        public Font Font
        {
            get { return _font; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (_font == value)
                    return;

                _font = value;
                UpdatePositions();
            }
        }

        /// <summary>
        /// Gets the <see cref="StyledText"/>s being drawn. Can be null.
        /// </summary>
        public IEnumerable<IEnumerable<StyledText>> Texts
        {
            get { return _texts; }
        }

        /// <summary>
        /// Clears the <see cref="StyledText"/>s from this <see cref="StyledTextsDrawer"/>.
        /// </summary>
        public void ClearTexts()
        {
            _texts = null;
            UpdatePositions();
        }

        /// <summary>
        /// Draws the <see cref="StyledText"/>s in this <see cref="StyledTextsDrawer"/>.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/>.</param>
        /// <param name="defaultColor">The default color.</param>
        /// <param name="offset">The offset to start drawing the text at.</param>
        public void Draw(ISpriteBatch sb, Color defaultColor, Vector2 offset)
        {
            foreach (var item in _textsWithPos)
            {
                item.StyledText.Draw(sb, Font, offset + item.Position, defaultColor);
            }
        }

        /// <summary>
        /// Sets the <see cref="StyledText"/>s drawn by this <see cref="StyledTextsDrawer"/>.
        /// </summary>
        /// <param name="styledTexts">The <see cref="StyledText"/>s to draw, where the outter collection
        /// is the lines and the inner collection is the <see cref="StyledText"/>s on each line.</param>
        public void SetStyledTexts(IEnumerable<IEnumerable<StyledText>> styledTexts)
        {
            if (styledTexts == null || styledTexts.IsEmpty())
                _texts = null;
            else
            {
                var lines = new List<IEnumerable<StyledText>>(styledTexts.Count());
                foreach (var line in styledTexts)
                {
                    if (line.Count() == 1)
                        lines.Add(line);
                    else
                        lines.Add(StyledText.Concat(line.ToArray()));
                }

                _texts = lines;
            }
            UpdatePositions();
        }

        /// <summary>
        /// Sets the <see cref="StyledText"/>s drawn by this <see cref="StyledTextsDrawer"/>.
        /// </summary>
        /// <param name="styledTexts">The <see cref="StyledText"/>s to draw, where the outter collection
        /// is the lines and the inner collection is the <see cref="StyledText"/>s on each line.</param>
        public void SetStyledTexts(IEnumerable<List<StyledText>> styledTexts)
        {
            if (styledTexts == null || styledTexts.IsEmpty())
                _texts = null;
            else
            {
                var lines = new List<IEnumerable<StyledText>>(styledTexts.Count());
                foreach (var line in styledTexts)
                {
                    if (line.Count() == 1)
                        lines.Add(line);
                    else
                        lines.Add(StyledText.Concat(line));
                }

                _texts = lines;
            }

            UpdatePositions();
        }

        /// <summary>
        /// Updates all the positions. Needs to be done whenever the text or font changes.
        /// </summary>
        void UpdatePositions()
        {
            _textsWithPos.Clear();
            if (_texts == null)
                return;

            var pos = Vector2.Zero;

            foreach (var line in _texts)
            {
                pos.X = 0;

                if (!line.IsEmpty())
                {
                    var last = line.Last();
                    foreach (var item in line)
                    {
                        _textsWithPos.Add(new StyledTextWithPosition(item, pos));
                        if (item != last)
                            pos.X += item.GetWidth(Font);
                    }
                }

                pos.Y += Font.GetLineSpacing();
            }
        }

        /// <summary>
        /// A container for a <see cref="StyledText"/> that contains the position to draw it at.
        /// </summary>
        class StyledTextWithPosition
        {
            readonly Vector2 _position;
            readonly StyledText _styledText;

            /// <summary>
            /// Initializes a new instance of the <see cref="StyledTextWithPosition"/> class.
            /// </summary>
            /// <param name="styledText">The styled text.</param>
            /// <param name="position">The position.</param>
            public StyledTextWithPosition(StyledText styledText, Vector2 position)
            {
                _styledText = styledText;
                _position = position;
            }

            /// <summary>
            /// Gets the position.
            /// </summary>
            public Vector2 Position
            {
                get { return _position; }
            }

            /// <summary>
            /// Gets the styled text.
            /// </summary>
            public StyledText StyledText
            {
                get { return _styledText; }
            }
        }
    }
}