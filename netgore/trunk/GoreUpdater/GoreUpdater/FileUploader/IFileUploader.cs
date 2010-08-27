using System;
using System.Collections.Generic;
using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Interface for an object that can handle uploading files to a destination.
    /// All implementations must be completely thread-safe.
    /// </summary>
    public interface IFileUploader : IDisposable
    {
        /// <summary>
        /// Notifies listeners when a request to delete a directory has been completed.
        /// </summary>
        event FileUploaderDeleteDirEventHandler DeleteDirectoryComplete;

        /// <summary>
        /// Notifies listeners when a request to delete a directory has encountered an error.
        /// </summary>
        event FileUploaderDeleteDirErrorEventHandler DeleteDirectoryError;

        /// <summary>
        /// Notifies listeners when an upload has been completed.
        /// </summary>
        event FileUploaderUploadEventHandler UploadComplete;

        /// <summary>
        /// Notifies listeners when there has been an error related to one of the upload jobs. The job in question will still
        /// be re-attempted by default.
        /// </summary>
        event FileUploaderErrorEventHandler UploadError;

        /// <summary>
        /// Gets if the <see cref="IFileUploader"/> is currently busy uploading files. This will be false when the queue is empty
        /// and all active file transfers have finished.
        /// </summary>
        bool IsBusy { get; }

        /// <summary>
        /// Gets if this object has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets or sets if files that already exist on the destination will be skipped. Default value is true.
        /// </summary>
        bool SkipIfExists { get; set; }

        /// <summary>
        /// Removes a file from the transfer queue and aborts it.
        /// </summary>
        /// <param name="targetPath">The remote path for the transfer to cancel.</param>
        /// <returns>True if the job was removed; otherwise false.</returns>
        bool CancelTransfer(string targetPath);

        /// <summary>
        /// Deletes a directory asynchronously. If the root directory is specified, then all files and folders in the root
        /// directory will be deleted, but the root directory itself will not be deleted. Otherwise, the specified directory
        /// will be deleted along with all files and folders under it.
        /// </summary>
        /// <param name="targetPath">The relative path of the directory to delete.</param>
        /// <returns>True if the directory deletion task was enqueued; false if the <paramref name="targetPath"/> is already
        /// queued for deletion, or if the <paramref name="targetPath"/> is invalid.</returns>
        bool DeleteDirectoryAsync(string targetPath);

        /// <summary>
        /// Synchronously downloads a remote file and returns the contents of the downloaded file as an array of bytes.
        /// </summary>
        /// <param name="remoteFile">The remote file to download.</param>
        /// <returns>The downloaded file's contents.</returns>
        byte[] DownloadFile(string remoteFile);

        /// <summary>
        /// Synchronously downloads a remote file and returns the contents of the downloaded file as a string.
        /// </summary>
        /// <param name="remoteFile">The remote file to download.</param>
        /// <returns>The downloaded file's contents.</returns>
        string DownloadFileAsString(string remoteFile);

        /// <summary>
        /// Enqueues a file for asynchronous uploading.
        /// </summary>
        /// <param name="sourcePath">The path to the local file to upload.</param>
        /// <param name="targetPath">The path to upload the file to on the destination.</param>
        /// <returns>True if the file was enqueued; false if either of the arguments were invalid, or the file already
        /// exists in the queue.</returns>
        bool EnqueueAsync(string sourcePath, string targetPath);

        /// <summary>
        /// Enqueues multiple files for asynchronous uploading.
        /// </summary>
        /// <param name="files">The files to upload, where the key is the source path, and the value is the
        /// path to upload the file on the destination.</param>
        void EnqueueAsync(IEnumerable<KeyValuePair<string, string>> files);
    }
}