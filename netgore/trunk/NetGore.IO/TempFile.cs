using System;
using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// Contains information about a temporary file.
    /// </summary>
    public sealed class TempFile : IDisposable
    {
        readonly string _filePath;
        volatile bool _isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TempFile"/> class.
        /// </summary>
        public TempFile() : this(ContentPaths.GetTempFilePath())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TempFile"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        internal TempFile(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Gets the file path to the temporary file.
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
        }

        #region IDisposable Members

        /// <summary>
        /// Releases the temporary file. This must be called after the file has been properly closed.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            ContentPaths.FreeTempFile(_filePath);
        }

        #endregion
    }
}