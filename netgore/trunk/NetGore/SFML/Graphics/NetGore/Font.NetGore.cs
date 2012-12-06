using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFML.Graphics
{
    public partial class Font
    {
        uint _defaultSize = 36;

        /// <summary>
        /// Gets or sets the default size used for this <see cref="Font"/>.
        /// </summary>
        public uint DefaultSize
        {
            get { return _defaultSize; }
            set { _defaultSize = value; }
        }

        /// <summary>
        /// Reloads the asset from file if it is not loaded.
        /// </summary>
        /// <param name="filename">Font file to load</param>
        /// <returns>True if already loaded; false if it had to reload.</returns>
        /// <exception cref="LoadingFailedException"/>
        protected internal bool EnsureLoaded(string filename)
        {
            if (ThisRaw != IntPtr.Zero)
                return true;

            var ptr = sfFont_CreateFromFile(filename);
            SetThis(ptr);

            if (ThisRaw == IntPtr.Zero)
                throw new LoadingFailedException("font", filename);

            return false;
        }

        /// <summary>
        /// Get a glyph in the font
        /// </summary>
        /// <param name="codePoint">Unicode code point of the character to get</param>
        /// <param name="bold">Retrieve the bold version or the regular one?</param>
        /// <returns>The glyph corresponding to the character</returns>
        public Glyph GetGlyph(uint codePoint, bool bold)
        {
            return GetGlyph(codePoint, DefaultSize, bold);
        }

        /// <summary>
        /// Get the kerning offset between two glyphs
        /// </summary>
        /// <param name="first">Unicode code point of the first character</param>
        /// <param name="second">Unicode code point of the second character</param>
        /// <returns>Kerning offset, in pixels</returns>
        public int GetKerning(uint first, uint second)
        {
            return GetKerning(first, second, DefaultSize);
        }

        /// <summary>
        /// Get spacing between two consecutive lines
        /// </summary>
        /// <returns>Line spacing, in pixels</returns>
        public int GetLineSpacing()
        {
            // NOTE: Custom method
            return GetLineSpacing(DefaultSize);
        }

        /// <summary>
        /// Get the image containing the glyphs of a given size
        /// </summary>
        /// <returns>Image storing the glyphs for the given size</returns>
        public Image GetImage()
        {
            return GetImage(DefaultSize);
        }

        /// <summary>
        /// Handle the destruction of the object
        /// </summary>
        /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
        protected override void Destroy(bool disposing)
        {
            if (this != ourDefaultFont)
            {
                var t = ThisRaw;

                if (!disposing)
                    Context.Global.SetActive(true);

                if (t != IntPtr.Zero)
                    sfFont_Destroy(t);

                if (disposing)
                {
                    foreach (var image in myImages.Values)
                    {
                        image.Dispose();
                    }
                }

                myImages.Clear();

                if (!disposing)
                    Context.Global.SetActive(false);
            }
        }

        protected internal Font()
            : base(IntPtr.Zero)
        {
        }
    }
}
