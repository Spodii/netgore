using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace GoreUpdater
{
    /// <summary>
    /// Implementation of the <see cref="IOfflineFileReplacer"/> using a Windows batch file.
    /// </summary>
    public class BatchOfflineFileReplacer : IOfflineFileReplacer
    {
        readonly string _outputFilePath;
        readonly Dictionary<string, string> _jobs = new Dictionary<string, string>(StringComparer.Ordinal);
        readonly object _jobsSync = new object();
        readonly string _appPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchOfflineFileReplacer"/> class.
        /// </summary>
        /// <param name="outputFilePath">The path to save the generated batch script file to.</param>
        /// <param name="appPath">The path to the application to run after the script runs. Can be null or an empty string
        /// to run nothing.</param>
        public BatchOfflineFileReplacer(string outputFilePath, string appPath)
        {
            _outputFilePath = outputFilePath;
            _appPath = appPath;

            // Ensure the output file path exists
            var dir = Path.GetDirectoryName(outputFilePath);
            if (dir != null)
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
        }

        #region Implementation of IOfflineFileReplacer

        /// <summary>
        /// Adds a job to replace a file. If a job for the <paramref name="filePath"/> already exists, the path will
        /// be updated to the <paramref name="newFilePath"/>.
        /// </summary>
        /// <param name="filePath">The path to the file to move.</param>
        /// <param name="newFilePath">The path to move the <paramref name="filePath"/> to.</param>
        /// <returns>True if the job was successfully added; otherwise false.</returns>
        public bool AddJob(string filePath, string newFilePath)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(newFilePath))
                return false;

            lock (_jobsSync)
            {
                if (_jobs.ContainsKey(filePath))
                {
                    _jobs[filePath] = newFilePath;
                }
                else
                {
                    _jobs.Add(filePath, newFilePath);
                }

                WriteFile();
            }

            return true;
        }

        /// <summary>
        /// Creates the contents for the batch file.
        /// </summary>
        /// <returns>The contents for the batch file.</returns>
        string GetFileContents()
        {
            StringBuilder sb = new StringBuilder();

            // Require that the batch file first waits for this process to finish
            var currProc = Process.GetCurrentProcess();
            sb.AppendLine(string.Format("tasklist /FI \"PID eq {0}\"", currProc.Id));

            // Add a wait for 2 seconds, accomplished by abusing the ping command to ping localhost twice
            sb.AppendLine("ping -n 2 127.0.0.1 > nul");

            // Add the individual move jobs
            foreach (var job in _jobs)
            {
                var s = string.Format("move \"{0}\" \"{1}\"", job.Key, job.Value);
                sb.AppendLine(s);
            }

            // Add a call to re-load the program
            if (!string.IsNullOrEmpty(_appPath))
                sb.AppendLine(string.Format("START \"{0}\"", _appPath));

            return sb.ToString();
        }

        /// <summary>
        /// Writes out the batch file. The <see cref="_jobsSync"/> lock must be acquired already.
        /// </summary>
        void WriteFile()
        {
            if (_jobs.Count == 0)
            {
                // No jobs - just delete the old file
                if (File.Exists(_outputFilePath))
                    File.Delete(_outputFilePath);
            }
            else
            {
                // Build the file contents
                var fileText = GetFileContents();

                // Write out to a temporary file
                var tempFilePath = _outputFilePath + ".tmp";
                File.WriteAllText(tempFilePath, fileText);

                // Move the file
                File.Copy(tempFilePath, _outputFilePath, true);
                
                // Delete the temp file
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
            }
        }

        /// <summary>
        /// Removes a job.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>True if the job was successfully removed; false if the <paramref name="filePath"/> was invalid
        /// or no job exists for the given <paramref name="filePath"/>.</returns>
        public bool RemoveJob(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            bool removed;
            lock (_jobsSync)
            {
                removed = _jobs.Remove(filePath);

                if (removed)
                {
                    WriteFile();
                }
            }

            return removed;
        }

        /// <summary>
        /// Gets all of the queued jobs.
        /// </summary>
        /// <returns>All of the queued jobs and their corresponding destination.</returns>
        public IEnumerable<KeyValuePair<string, string>> GetJobs()
        {
            lock (_jobsSync)
            {
                return _jobs.ToArray();
            }
        }

        #endregion
    }
}
