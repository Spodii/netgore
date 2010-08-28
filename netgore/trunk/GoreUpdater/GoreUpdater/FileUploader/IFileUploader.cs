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
        /// Notifies listeners when an asynchronous request to delete a directory has been completed.
        /// </summary>
        event FileUploaderDeleteDirEventHandler DeleteDirectoryComplete;

        /// <summary>
        /// Notifies listeners when an asynchronous request to delete a directory has encountered an error.
        /// </summary>
        event FileUploaderDeleteDirErrorEventHandler DeleteDirectoryError;

        /// <summary>
        /// Notifies listeners when an asynchronous download has been completed.
        /// </summary>
        event FileUploaderDownloadEventHandler DownloadComplete;

        /// <summary>
        /// Notifies listeners when there has been an error related to one of the asynchronous download jobs.
        /// The job in question will still be re-attempted by default.
        /// </summary>
        event FileUploaderDownloadErrorEventHandler DownloadError;

        /// <summary>
        /// Notifies listeners when the <see cref="IFileUploader.TestConnection"/> method has produced a message
        /// related to the status of the connection testing. This only contains status update messages, not error
        /// messages.
        /// </summary>
        event FileUploaderTestConnectionMessageEventHandler TestConnectionMessage;

        /// <summary>
        /// Notifies listeners when an asynchronous upload has been completed.
        /// </summary>
        event FileUploaderUploadEventHandler UploadComplete;

        /// <summary>
        /// Notifies listeners when there has been an error related to one of the asynchronous upload jobs.
        /// The job in question will still be re-attempted by default.
        /// </summary>
        event FileUploaderUploadErrorEventHandler UploadError;

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
        /// Removes a file from the asynchronous download queue and aborts it.
        /// </summary>
        /// <param name="localPath">The fully qualified local path of the download to cancel.</param>
        /// <returns>True if the job was removed; otherwise false.</returns>
        /// <exception cref="ObjectDisposedException">This object is disposed.</exception>
        bool CancelAsyncDownload(string localPath);

        /// <summary>
        /// Removes a file from the asynchronous upload queue and aborts it.
        /// </summary>
        /// <param name="remotePath">The remote path for the upload to cancel.</param>
        /// <returns>True if the job was removed; otherwise false.</returns>
        /// <exception cref="ObjectDisposedException">This object is disposed.</exception>
        bool CancelAsyncUpload(string remotePath);

        /// <summary>
        /// Deletes a directory synchronously. If the root directory is specified, then all files and folders in the root
        /// directory will be deleted, but the root directory itself will not be deleted. Otherwise, the specified directory
        /// will be deleted along with all files and folders under it.
        /// </summary>
        /// <param name="targetPath">The relative path of the directory to delete.</param>
        /// <param name="requireExists">If false, and the <paramref name="targetPath"/> does not exist, then the deletion
        /// will fail silently.</param>
        /// <exception cref="ObjectDisposedException">This object is disposed.</exception>
        void DeleteDirectory(string targetPath, bool requireExists = false);

        /// <summary>
        /// Deletes a directory asynchronously. If the root directory is specified, then all files and folders in the root
        /// directory will be deleted, but the root directory itself will not be deleted. Otherwise, the specified directory
        /// will be deleted along with all files and folders under it.
        /// </summary>
        /// <param name="targetPath">The relative path of the directory to delete.</param>
        /// <returns>True if the directory deletion task was enqueued; false if the <paramref name="targetPath"/> is already
        /// queued for deletion, or if the <paramref name="targetPath"/> is invalid.</returns>
        /// <exception cref="ObjectDisposedException">This object is disposed.</exception>
        bool DeleteDirectoryAsync(string targetPath);

        /// <summary>
        /// Synchronously downloads a remote file and returns the contents of the downloaded file as an array of bytes.
        /// </summary>
        /// <param name="remoteFile">The remote file to download.</param>
        /// <param name="requireExists">If false, and the remote file does not exist, a null will be returned instead.</param>
        /// <returns>The downloaded file's contents.</returns>
        /// <exception cref="ObjectDisposedException">This object is disposed.</exception>
        byte[] Download(string remoteFile, bool requireExists = false);

        /// <summary>
        /// Synchronously downloads a remote file and returns the contents of the downloaded file as a string.
        /// </summary>
        /// <param name="remoteFile">The remote file to download.</param>
        /// <param name="requireExists">If false, and the remote file does not exist, a null will be returned instead.</param>
        /// <returns>The downloaded file's contents.</returns>
        /// <exception cref="ObjectDisposedException">This object is disposed.</exception>
        string DownloadAsString(string remoteFile, bool requireExists = false);

        /// <summary>
        /// Enqueues a file for asynchronous downloading.
        /// </summary>
        /// <param name="remotePath">The path to the file to download on the destination.</param>
        /// <param name="sourcePath">The fully qualified path to download the file to.</param>
        /// <returns>True if the file was enqueued; false if either of the arguments were invalid, or the file already
        /// exists in the queue.</returns>
        /// <exception cref="ObjectDisposedException">This object is disposed.</exception>
        bool DownloadAsync(string remotePath, string sourcePath);

        /// <summary>
        /// Enqueues multiple files for asynchronous downloading.
        /// </summary>
        /// <param name="files">The files to download, where the key is the remote file path, and the value is the
        /// fully qualified local path to download the file to.</param>
        /// <exception cref="ObjectDisposedException">This object is disposed.</exception>
        void DownloadAsync(IEnumerable<KeyValuePair<string, string>> files);

        /// <summary>
        /// Tests the connection of the <see cref="IFileUploader"/> and ensures that the needed operations can be performed.
        /// The test runs synchronously.
        /// </summary>
        /// <param name="userState">An optional object that can be used. When the <see cref="TestConnectionMessage"/> event is raised,
        /// this object is passed back through the event, allowing you to differentiate between multiple connection tests.</param>
        /// <param name="error">When this method returns false, contains a string describing the error encountered during testing.</param>
        /// <returns>True if the test was successful; otherwise false.</returns>
        bool TestConnection(object userState, out string error);

        /// <summary>
        /// Enqueues a file for asynchronous uploading.
        /// </summary>
        /// <param name="sourcePath">The path to the local file to upload.</param>
        /// <param name="remotePath">The path to upload the file to on the destination.</param>
        /// <returns>True if the file was enqueued; false if either of the arguments were invalid, or the file already
        /// exists in the queue.</returns>
        /// <exception cref="ObjectDisposedException">This object is disposed.</exception>
        bool UploadAsync(string sourcePath, string remotePath);

        /// <summary>
        /// Enqueues multiple files for asynchronous uploading.
        /// </summary>
        /// <param name="files">The files to upload, where the key is the source path, and the value is the
        /// path to upload the file on the destination.</param>
        /// <exception cref="ObjectDisposedException">This object is disposed.</exception>
        void UploadAsync(IEnumerable<KeyValuePair<string, string>> files);
    }
}