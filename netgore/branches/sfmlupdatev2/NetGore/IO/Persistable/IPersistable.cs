using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// Interface for an object that can have its state written to and read from binary data. This is similar to
    /// serialization except for that objects are not instantiated, and the object reading from the stream
    /// will only change in state, not reference. So it is not as powerful as serialization, but the intended
    /// purposes is a bit different.
    /// </summary>
    public interface IPersistable
    {
        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        void ReadState(IValueReader reader);

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        void WriteState(IValueWriter writer);
    }
}