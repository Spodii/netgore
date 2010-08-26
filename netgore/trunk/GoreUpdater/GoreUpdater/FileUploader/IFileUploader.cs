using System;
using System.Collections.Generic;
using System.Text;

namespace GoreUpdater
{

    /// <summary>
    /// Interface for an object that can handle uploading files to a destination.
    /// All implementations must be completely thread-safe.
    /// </summary>
    public interface IFileUploader : IDisposable
    {
        /// <summary>
        /// Notifies listeners when there has been an error related to one of the upload jobs. The job in question will still
        /// be re-attempted by default.
        /// </summary>
        event FileUploaderErrorEventHandler UploadError;

        /// <summary>
        /// Notifies listeners when an upload has been completed.
        /// </summary>
        event FileUploaderUploadEventHandler UploadComplete;

        /// <summary>
        /// Notifies listeners when a request to delete a directory has been completed.
        /// </summary>
        event FileUploaderDeleteDirEventHandler DeleteDirectoryComplete;

        /// <summary>
        /// Notifies listeners when a request to delete a directory has encountered an error.
        /// </summary>
        event FileUploaderDeleteDirErrorEventHandler DeleteDirectoryError;

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
        /// Gets or sets if files that already exist on the destination will be skipped.
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
        bool DeleteDirectory(string targetPath);

        /// <summary>
        /// Enqueues a file for uploading.
        /// </summary>
        /// <param name="sourcePath">The path to the local file to upload.</param>
        /// <param name="targetPath">The path to upload the file to on the destination.</param>
        /// <returns>True if the file was enqueued; false if either of the arguments were invalid, or the file already
        /// exists in the queue.</returns>
        bool Enqueue(string sourcePath, string targetPath);

        /// <summary>
        /// Enqueues multiple files for uploading.
        /// </summary>
        /// <param name="files">The files to upload, where the key is the source path, and the value is the
        /// path to upload the file on the destination.</param>
        void Enqueue(IEnumerable<KeyValuePair<string, string>> files);
    }

    /// <summary>
    /// Delegate for handling error events from the <see cref="IFileUploader"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IFileUploader"/> that the event came from.</param>
    /// <param name="localFile">The local file for the job related to the error.</param>
    /// <param name="remoteFile">The remote file for the job related to the error.</param>
    /// <param name="error">A string containing the error message.</param>
    /// <param name="attemptCount">The number of times this particular job has been attempted. This value is incremented every
    /// time the job is attempted, even if it fails for a different reason.
    /// Once this value reaches 255, it will no longer increment.</param>
    public delegate void FileUploaderErrorEventHandler(IFileUploader sender, string localFile, string remoteFile, string error, byte attemptCount);

    /// <summary>
    /// Delegate for handling upload events from the <see cref="IFileUploader"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IFileUploader"/> that the event came from.</param>
    /// <param name="localFile">The local file for the job that finished.</param>
    /// <param name="remoteFile">The remote file for the job that finished.</param>
    public delegate void FileUploaderUploadEventHandler(IFileUploader sender, string localFile, string remoteFile);

    /// <summary>
    /// Delegate for handling upload events from the <see cref="IFileUploader"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IFileUploader"/> that the event came from.</param>
    /// <param name="path">The relative path on the remote system of the directory deleted.</param>
    public delegate void FileUploaderDeleteDirEventHandler(IFileUploader sender, string path);

    /// <summary>
    /// Delegate for handling upload events from the <see cref="IFileUploader"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IFileUploader"/> that the event came from.</param>
    /// <param name="path">The relative path on the remote system of the directory deleted.</param>
    /// <param name="error">The error message for why the directory could not be deleted.</param>
    /// <param name="attemptCount">The number of times this particular job has been attempted. This value is incremented every
    /// time the job is attempted, even if it fails for a different reason.
    /// Once this value reaches 255, it will no longer increment.</param>
    public delegate void FileUploaderDeleteDirErrorEventHandler(IFileUploader sender, string path, string error, byte attemptCount);
}
