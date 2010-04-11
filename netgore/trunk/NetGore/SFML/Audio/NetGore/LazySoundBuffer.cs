using System.Linq;
using SFML.Audio;

namespace SFML.Graphics
{
    /// <summary>
    /// An implementation of <see cref="SoundBuffer"/> that will load on-demand.
    /// </summary>
    public class LazySoundBuffer : SoundBuffer
    {
        readonly string _filename;

        /// <summary>
        /// Gets the file name that this sound buffer uses to load.
        /// </summary>
        public string FileName { get { return _filename; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazySoundBuffer"/> class.
        /// </summary>
        /// <param name="filename">Path of the sound file to load</param>
        public LazySoundBuffer(string filename)
        {
            _filename = filename;
        }

        /// <summary>
        /// Access to the internal pointer of the object.
        /// For internal use only
        /// </summary>
        public override System.IntPtr This
        {
            get
            {
                if (!EnsureLoaded(FileName))
                    OnReload();

                return base.This;
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles when the <see cref="LazyImage"/> is reloaded.
        /// </summary>
        protected virtual void OnReload()
        {
        }

        /// <summary>
        /// Explicitely dispose the object
        /// </summary>
        public override void Dispose()
        {
            Dispose(true);
        }
    }
}