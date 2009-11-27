using System;
using System.Linq;
using NetGore.IO;

namespace NetGore.Tests
{
    /// <summary>
    /// Base class for a <see cref="IValueReader"/> and <see cref="IValueWriter"/> creator. This is used to
    /// create a variety of <see cref="IValueReader"/>s and <see cref="IValueWriter"/>s using different backends
    /// to test a large number of I/O mediums and methods at once.
    /// </summary>
    abstract class ReaderWriterCreatorBase : IDisposable
    {
        /// <summary>
        /// When overridden in the derived class, gets if name lookup is supported.
        /// </summary>
        public abstract bool SupportsNameLookup { get; }

        /// <summary>
        /// When overridden in the derived class, gets if nodes are supported.
        /// </summary>
        public abstract bool SupportsNodes { get; }

        /// <summary>
        /// When overridden in the derived class, gets the IValueReader instance used to read the values
        /// written by the IValueWriter created with GetWriter().
        /// </summary>
        /// <returns>The IValueWriter instance.</returns>
        public abstract IValueReader GetReader();

        /// <summary>
        /// When overridden in the derived class, gets the IValueWriter instance. This method is always called first.
        /// </summary>
        /// <returns>The IValueReader instance.</returns>
        public abstract IValueWriter GetWriter();

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
        }

        #endregion
    }
}