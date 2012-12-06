using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFML.Graphics
{
    public partial class Image
    {
        /// <summary>
        /// Internal constructor
        /// </summary>
        /// <param name="thisPtr">Pointer to the object in C library</param>
        internal Image(IntPtr thisPtr) :
            base(thisPtr)
        {
            if (thisPtr != IntPtr.Zero)
                myExternal = true;
        }

        /// <summary>
        /// Reloads the asset from file if it is not loaded.
        /// </summary>
        /// <param name="filename">Path of the image file to load</param>
        /// <returns>True if already loaded; false if it had to load.</returns>
        protected internal bool EnsureLoaded(string filename)
        {
            if (ThisRaw != IntPtr.Zero)
                return true;

            SetThis(sfImage_CreateFromFile(filename));

            if (ThisRaw == IntPtr.Zero)
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
                if (!disposing)
                    Context.Global.SetActive(true);

                var t = ThisRaw;

                if (t != IntPtr.Zero)
                    sfImage_Destroy(t);

                if (!disposing)
                    Context.Global.SetActive(false);
            }
        }
    }
}
