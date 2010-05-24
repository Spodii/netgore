using System;
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
        static readonly object _textSync = new object();
        static readonly Text _text = new Text();

        /// <summary>
        /// Measures the size of a string.
        /// </summary>
        /// <param name="font">The <see cref="Font"/>.</param>
        /// <param name="str">The string to measure.</param>
        /// <returns>The size of the <paramref name="str"/>.</returns>
        public static Vector2 MeasureString(this Font font, string str)
        {
            lock (_textSync)
            {
                try
                {
                    _text.Font = font;
                    _text.Size = font.DefaultSize;
                    _text.DisplayedString = str;

                    var r = _text.GetRect();

                    return new Vector2(r.Width, r.Height);
                }
                catch (AccessViolationException ex)
                {
                    // TODO: Would be nice to just make this exception to stop happening completely

                    // Try to keep exceptions transparent since they likely mean that something, for some reason, was disposed
                    // and will likely not be a persistant issue
                    const string errmsg = "Failed to measure string `{0}` with font `{1}`: {2}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, str, font, ex);

                    return new Vector2(4, 4);
                }
            }
        }

        /// <summary>
        /// Measures the size of a string.
        /// </summary>
        /// <param name="font">The <see cref="Font"/>.</param>
        /// <param name="str">The string to measure.</param>
        /// <returns>The size of the <paramref name="str"/>.</returns>
        public static Vector2 MeasureString(this Font font, StringBuilder str)
        {
            return MeasureString(font, str.ToString());
        }
    }
}