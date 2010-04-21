using System;
using System.Linq;

namespace SFML.Graphics
{
    /// <summary>
    /// An implementation of <see cref="Font"/> that will load on-demand.
    /// </summary>
    public class LazyFont : Font
    {
        readonly ushort _charSize;
        readonly string _filename;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyFont"/> class.
        /// </summary>
        /// <param name="filename">Font file to load</param>
        /// <param name="charSize">Character size</param>
        /// <exception cref="LoadingFailedException"/>
        public LazyFont(string filename, uint charSize = 30u)
        {
            if (charSize > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("charSize");

            _filename = filename;
            _charSize = (ushort)charSize;
        }

        /// <summary>
        /// Gets the char size to use when loading.
        /// </summary>
        public uint CharSize
        {
            get { return _charSize; }
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
                if (!EnsureLoaded(FileName, CharSize, string.Empty))
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
        /// When overridden in the derived class, handles when the <see cref="LazyFont"/> is reloaded.
        /// </summary>
        protected virtual void OnReload()
        {
        }
    }
}