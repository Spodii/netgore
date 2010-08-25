using System.Collections.Generic;

namespace GoreUpdater
{
    /// <summary>
    /// Interface for an object that generates a stand-alone script file or executable that can be used to replace files that
    /// are currently in use, then re-load the program that was used to generate the file when completed.
    /// </summary>
    public interface IOfflineFileReplacer
    {
        /// <summary>
        /// Adds a job to replace a file. If a job for the <paramref name="filePath"/> already exists, the path will
        /// be updated to the <paramref name="newFilePath"/>.
        /// </summary>
        /// <param name="filePath">The path to the file to move.</param>
        /// <param name="newFilePath">The path to move the <paramref name="filePath"/> to.</param>
        /// <returns>True if the job was successfully added; otherwise false.</returns>
        bool AddJob(string filePath, string newFilePath);

        /// <summary>
        /// Removes a job.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>True if the job was successfully removed; false if the <paramref name="filePath"/> was invalid
        /// or no job exists for the given <paramref name="filePath"/>.</returns>
        bool RemoveJob(string filePath);

        /// <summary>
        /// Gets all of the queued jobs.
        /// </summary>
        /// <returns>All of the queued jobs and their corresponding destination.</returns>
        IEnumerable<KeyValuePair<string, string>> GetJobs();
    }
}