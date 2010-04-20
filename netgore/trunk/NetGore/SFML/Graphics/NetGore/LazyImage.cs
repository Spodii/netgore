using System;
using System.Linq;

namespace SFML.Graphics
{
    /// <summary>
    /// An implementation of <see cref="Image"/> that will load on-demand.
    /// </summary>
    public class LazyImage : Image
    {
        readonly string _filename;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyImage"/> class.
        /// </summary>
        /// <param name="filename">The file name.</param>
        public LazyImage(string filename) : base(IntPtr.Zero)
        {
            _filename = filename;
        }

        /// <summary>
        /// Gets the file name that this image uses to load.
        /// </summary>
        public string FileName
        {
            get { return _filename; }
        }

        /// <summary>
        /// Access to the internal pointer of the object.
        /// For internal use only
        /// </summary>
        public override IntPtr This
        {
            get
            {
                if (!EnsureLoaded(FileName))
                    OnReload();

                return base.This;
            }
        }

        /// <summary>
        /// Explicitely dispose the object
        /// </summary>
        public override void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// When overridden in the derived class, handles when the <see cref="LazyImage"/> is reloaded.
        /// </summary>
        protected virtual void OnReload()
        {
        }
    }
}