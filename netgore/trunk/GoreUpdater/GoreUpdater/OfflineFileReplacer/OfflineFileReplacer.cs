using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;

namespace GoreUpdater
{
    /// <summary>
    /// Implementation of the <see cref="IOfflineFileReplacer"/> using a Windows batch file.
    /// </summary>
    public class BatchOfflineFileReplacer : IOfflineFileReplacer
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly string _appPath;
        readonly Dictionary<string, string> _jobs = new Dictionary<string, string>(StringComparer.Ordinal);
        readonly object _jobsSync = new object();
        readonly string _outputFilePath;

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
                {
                    if (log.IsDebugEnabled)
                        log.DebugFormat("Creating directory: {0}", dir);

                    Directory.CreateDirectory(dir);
                }
            }

            // Delete the output file if it already exists
            try
            {
                if (File.Exists(outputFilePath))
                {
                    if (log.IsDebugEnabled)
                        log.DebugFormat("Deleting file: {0}", outputFilePath);

                    File.Delete(outputFilePath);
                }
            }
            catch (IOException ex)
            {
                if (log.IsDebugEnabled)
                    log.DebugFormat("Failed to delete file `{0}`. Exception: {1}", outputFilePath, ex);

                Debug.Fail(ex.ToString());
            }
        }

        /// <summary>
        /// Creates the contents for the batch file.
        /// </summary>
        /// <returns>The contents for the batch file.</returns>
        string GetFileContents()
        {
            var sb = new StringBuilder();

            // Turn echo off
            sb.AppendLine("@ECHO OFF");

            // Require that the batch file first waits for this process to finish
            var currProc = Process.GetCurrentProcess();
            sb.AppendLine(string.Format("ECHO Waiting for `{0}` (ID {1}) to close...", currProc.ProcessName, currProc.Id));
            sb.AppendLine(":CHECKPROGRAMSTATE");
            sb.AppendLine(string.Format("TASKLIST /FI \"PID eq {0}\" > NUL", currProc.Id));
            sb.AppendLine("IF NOT ERRORLEVEL 0 GOTO CHECKPROGRAMSTATE");
            sb.AppendLine("ECHO Process closed");

            // Add a wait for 2 seconds, accomplished by abusing the ping command to ping localhost twice
            sb.AppendLine("ECHO Delaying for 2 seconds...");
            sb.AppendLine("PING -n 2 127.0.0.1 > NUL");
            sb.AppendLine("ECHO Delay complete");

            // Add the individual move jobs
            foreach (var job in _jobs)
            {
                sb.AppendLine(string.Format("ECHO Moving file: `{0}` to `{1}`", job.Key, job.Value));
                var s = string.Format("MOVE \"{0}\" \"{1}\"", job.Key, job.Value);
                sb.AppendLine(s);
            }

            // Add a call to re-load the program
            if (!string.IsNullOrEmpty(_appPath))
            {
                sb.AppendLine(string.Format("CALL Restarting application `{0}`", _appPath));
                sb.AppendLine(string.Format("START \"\" \"{0}\"", _appPath));
            }

            // Finally, add a call for the batch file to delete itself
            sb.AppendLine("CALL Deleting self...");
            sb.AppendLine(string.Format("DEL \"{0}\"", FilePath));

            return sb.ToString();
        }

        /// <summary>
        /// Writes out the batch file. The <see cref="_jobsSync"/> lock must be acquired already.
        /// </summary>
        void WriteFile()
        {
            if (_jobs.Count == 0)
            {
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat(
                        "Writing OfflineFileReplacer `{0}`'s contents to file `{1}`: No replace jobs, so deleting file instead.",
                        this, _outputFilePath);
                }

                // No jobs - just delete the old file
                if (File.Exists(_outputFilePath))
                    File.Delete(_outputFilePath);
            }
            else
            {
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("Writing OfflineFileReplacer `{0}`'s contents to file `{1}`: {2} jobs to write.", this,
                                    _outputFilePath, _jobs.Count);
                }

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

        #region IOfflineFileReplacer Members

        /// <summary>
        /// Gets the path to the output file for this <see cref="IOfflineFileReplacer"/>.
        /// </summary>
        public string FilePath
        {
            get { return _outputFilePath; }
        }

        /// <summary>
        /// Gets the number of jobs in this <see cref="IOfflineFileReplacer"/>.
        /// </summary>
        public int JobCount
        {
            get
            {
                lock (_jobsSync)
                {
                    return _jobs.Count;
                }
            }
        }

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

            if (log.IsDebugEnabled)
                log.DebugFormat("Adding file replace job to `{0}`: Move `{1}` to `{2}`.", this, filePath, newFilePath);

            lock (_jobsSync)
            {
                if (_jobs.ContainsKey(filePath))
                    _jobs[filePath] = newFilePath;
                else
                    _jobs.Add(filePath, newFilePath);

                WriteFile();
            }

            return true;
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

            if (log.IsDebugEnabled)
                log.DebugFormat("Removing job from `{0}` for file path `{1}`.", this, filePath);

            bool removed;
            lock (_jobsSync)
            {
                removed = _jobs.Remove(filePath);

                if (removed)
                    WriteFile();
            }

            return removed;
        }

        #endregion
    }
}