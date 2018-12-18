using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Extension methods for the <see cref="Font"/> class.
    /// </summary>
    public static class FontExtensions
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly Text _text = new Text();
        static readonly object _textSync = new object();

        /// <summary>
        /// Measures the size of a string.
        /// </summary>
        /// <param name="font">The <see cref="Font"/>.</param>
        /// <param name="str">The string to measure.</param>
        /// <param name="fontSize">The size of the font. If equal to 0, the <see cref="Font.DefaultSize"/>
        /// will be used instead.</param>
        /// <returns>The size of the <paramref name="str"/>.</returns>
        public static Vector2 MeasureString(this Font font, string str, uint fontSize = 0u)
        {
            if (fontSize <= 0)
                fontSize = font.DefaultSize;

            lock (_textSync)
            {
                try
                {
                    Debug.Assert(!_text.IsDisposed);

                    _text.Font = font;
                    _text.CharacterSize = fontSize;
                    _text.DisplayedString = str;

                    var r = _text.GetGlobalBounds();

                    return new Vector2(r.Width, r.Height);
                }
                catch (AccessViolationException ex)
                {
                    // Try to keep exceptions transparent since they likely mean that something, for some reason, was disposed
                    // and will likely not be a persistant issue
                    const string errmsg = "Failed to measure string `{0}` with font `{1}`: {2}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, str, font, ex);
                    Debug.Fail(string.Format(errmsg, str, font, ex));

                    return new Vector2(4, 4);
                }
            }
        }

        /// <summary>
        /// Measures the size of a string.
        /// </summary>
        /// <param name="font">The <see cref="Font"/>.</param>
        /// <param name="str">The string to measure.</param>
        /// <param name="fontSize">The size of the font. If equal to 0, the <see cref="Font.DefaultSize"/>
        /// will be used instead.</param>
        /// <returns>The size of the <paramref name="str"/>.</returns>
        public static Vector2 MeasureString(this Font font, StringBuilder str, uint fontSize = 0u)
        {
            return MeasureString(font, str.ToString());
        }
    }
}