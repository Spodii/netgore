using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFML. Audio
{
    public partial class SoundBuffer
    {
        protected SoundBuffer()
            : base(IntPtr.Zero)
        {
        }

        /// <summary>
        /// Reloads the asset from file if it is not loaded.
        /// </summary>
        /// <param name="filename">Path of the sound file to load</param>
        /// <returns>True if already loaded; false if it had to load.</returns>
        protected internal bool EnsureLoaded(string filename)
        {
            if (CPointerRaw != IntPtr.Zero)
                return true;

            SetThis(sfSoundBuffer_createFromFile(filename));

            if (CPointerRaw == IntPtr.Zero)
                throw new LoadingFailedException("sound buffer", filename);

            return false;
        }

        /// <summary>
        /// Handle the destruction of the object
        /// </summary>
        /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
        protected override void Destroy(bool disposing)
        {
            var t = CPointerRaw;

            if (t != IntPtr.Zero)
                sfSoundBuffer_destroy(t);
        }
    }
}
