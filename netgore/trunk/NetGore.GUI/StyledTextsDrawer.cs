using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    public class StyledTextsDrawer
    {
        SpriteFont _font;

        /// <summary>
        /// Initializes a new instance of the <see cref="StyledTextsDrawer"/> class.
        /// </summary>
        /// <param name="font">The font.</param>
        public StyledTextsDrawer(SpriteFont font)
        {
            if (font == null)
                throw new ArgumentNullException("font");

            _font = font;
        }

        /// <summary>
        /// Gets or sets the font used to draw. Cannot be null.
        /// </summary>
        public SpriteFont Font
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

        public void Draw(SpriteBatch sb, Color defaultColor, Vector2 offset)
        {
            foreach (var item in _textsWithPos)
            {
                item.StyledText.Draw(sb, Font, offset + item.Position, defaultColor);
            }
        }

        IEnumerable<IEnumerable<StyledText>> _texts;
        readonly List<StyledTextWithPosition> _textsWithPos = new List<StyledTextWithPosition>();

        public IEnumerable<IEnumerable<StyledText>> Texts { get { return _texts; } }

        void UpdatePositions()
        {
            _textsWithPos.Clear();
            if (_texts == null)
                return;

            Vector2 pos = Vector2.Zero;

            foreach (var line in _texts)
            {
                pos.X = 0;

                var last = line.Last();
                foreach (var item in line)
                {
                    _textsWithPos.Add(new StyledTextWithPosition(item, pos));
                    if (item != last)
                        pos.X += item.GetWidth(Font);
                }

                pos.Y += Font.LineSpacing;
            }
        }

        public void SetStyledTexts(IEnumerable<IEnumerable<StyledText>> styledTexts)
        {
            if (styledTexts == null || styledTexts.Count() == 0)
            {
                _texts = null;
            }
            else
            {
                List<IEnumerable<StyledText>> lines = new List<IEnumerable<StyledText>>(styledTexts.Count());
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

        public void ClearTexts()
        {
            _texts = null;
            UpdatePositions();
        }

        public void SetStyledTexts(IEnumerable<List<StyledText>> styledTexts)
        {
            if (styledTexts == null || styledTexts.Count() == 0)
            {
                _texts = null;
            }
            else
            {
                List<IEnumerable<StyledText>> lines = new List<IEnumerable<StyledText>>(styledTexts.Count());
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

        class StyledTextWithPosition
        {
            readonly StyledText _styledText;
            readonly Vector2 _position;

            public StyledText StyledText { get { return _styledText; } }
            public Vector2 Position { get { return _position; } }

            public StyledTextWithPosition(StyledText styledText, Vector2 position)
            {
                _styledText = styledText;
                _position = position;
            }
        }
    }
}
