using System;
using System.IO;
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
        TempFile(string filePath)
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

        /// <summary>
        /// Moves the temporary file to the target file path, then disposes of the <see cref="TempFile"/>. Any directories
        /// needed will be created.
        /// </summary>
        /// <param name="targetFilePath">The file path to move the temp file to.</param>
        public void MoveTo(string targetFilePath)
        {
            // Check if we need to create the directory
            var dir = Path.GetDirectoryName(targetFilePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // If the file already exists, delete it
            if (File.Exists(targetFilePath))
                File.Delete(targetFilePath);

            // Copy over the file
            File.Copy(_filePath, targetFilePath, true);

            File.Delete(_filePath);

            // Dispose of the TempFile
            Dispose();
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