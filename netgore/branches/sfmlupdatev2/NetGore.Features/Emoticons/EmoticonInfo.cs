using System.Linq;
using NetGore.IO;

namespace NetGore.Features.Emoticons
{
    /// <summary>
    /// Contains information about an emoticon.
    /// </summary>
    public class EmoticonInfo<T> : IPersistable
    {
        /// <summary>
        /// Gets or sets (protected) the <see cref="GrhIndex"/> for the sprite to display for the emoticon.
        /// </summary>
        [SyncValue]
        public GrhIndex GrhIndex { get; protected set; }

        /// <summary>
        /// Gets or sets (protected) the emoticon key for the emoticon that this information is for.
        /// </summary>
        [SyncValue]
        public T Value { get; protected set; }

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public virtual void ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public virtual void WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);
        }

        #endregion
    }
}