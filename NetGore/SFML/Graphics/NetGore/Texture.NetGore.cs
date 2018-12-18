using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFML.Graphics
{
    public partial class Texture
    {
        static readonly Texture _blank;
        static readonly Texture _white;
        static readonly Texture _black;

        /// <summary>
        /// Gets a 1x1 blank texture.
        /// </summary>
        public static Texture BlankPixel { get { return _blank; } }

        /// <summary>
        /// Gets a 1x1 white texture.
        /// </summary>
        public static Texture WhitePixel { get { return _white; } }

        /// <summary>
        /// Gets a 1x1 black texture.
        /// </summary>
        public static Texture BlackPixel { get { return _black; } }

        static Texture()
        {
            using (var img = new Image(1, 1, new Color(0, 0, 0, 0)))
                _blank = new Texture(img);

            using (var img = new Image(1, 1, Color.White))
                _white = new Texture(img);

            using (var img = new Image(1, 1, Color.Black))
                _black = new Texture(img);
        }

        /// <summary>
        /// Internal constructor
        /// </summary>
        /// <param name="cPointer">Pointer to the object in C library</param>
        internal Texture(IntPtr cPointer) :
            base(cPointer)
        {
            if (cPointer != IntPtr.Zero)
                myExternal = true;
        }

        /// <summary>
        /// Reloads the asset from file if it is not loaded.
        /// </summary>
        /// <param name="filename">Path of the image file to load</param>
        /// <returns>True if already loaded; false if it had to load.</returns>
        protected internal bool EnsureLoaded(string filename)
        {
            if (CPointerRaw != IntPtr.Zero)
                return true;

            IntRect intRectZero =new IntRect(0, 0, 0, 0);
            SetThis(sfTexture_createFromFile(filename, ref intRectZero));

            if (CPointerRaw == IntPtr.Zero)
                throw new LoadingFailedException("image", filename);

            return false;
        }

        /// <summary>
        /// Handle the destruction of the object
        /// </summary>
        /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
        protected override void Destroy(bool disposing)
        {
            if (!myExternal)
            {
                var t = CPointerRaw;

                if (t != IntPtr.Zero)
                {
                    if (!disposing)
                        Context.Global.SetActive(true);

                    sfTexture_destroy(t);

                    if (!disposing)
                        Context.Global.SetActive(false);
                }
            }
        }
    }
}
