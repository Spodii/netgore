namespace GoreUpdater
{
    /// <summary>
    /// Describes the different states the <see cref="UpdateClient"/> can be in. For the most part, the enum value of
    /// each state is ordered based on the order that the <see cref="UpdateClient"/> progresses through them. That is, the
    /// lower the value, generally the earlier in the update process, while a higher value means closer to the end. However, this
    /// behavior should not be relied on.
    /// </summary>
    public enum UpdateClientState
    {
        /// <summary>
        /// Updating has not started.
        /// </summary>
        NotStarted,

        /// <summary>
        /// Start has been called and the objects are being created.
        /// </summary>
        Initializing,

        /// <summary>
        /// The current version and server lists are being downloaded from the master servers.
        /// </summary>
        ReadingMasterServers,

        /// <summary>
        /// Reading from the master servers has completed, and the results are being processed.
        /// </summary>
        DoneReadingMasterServers,

        /// <summary>
        /// Downloading the new files from the file servers.
        /// </summary>
        UpdatingFiles,

        /// <summary>
        /// The update process has completed.
        /// </summary>
        Completed,
    }
}