using System.Linq;

namespace NetGore.IO.PropertySync
{
    /// <summary>
    /// Interface for an object that detects when an object's property value has changed and can read/write
    /// it to a <see cref="IValueReader"/> and <see cref="IValueWriter"/> respectively.
    /// </summary>
    public interface IPropertySync
    {
        /// <summary>
        /// Gets the name of the synchronized value. This is what populates the Name parameter of the IValueReader
        /// and IValueWriter functions.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets if this property should be skipped when synchronizing over the network.
        /// </summary>
        bool SkipNetworkSync { get; }

        /// <summary>
        /// Gets if the Property's value has changed and needs to be re-synchronized.
        /// </summary>
        /// <param name="binder">The object to bind to to get the property value from or set the property value on.</param>
        /// <returns>
        /// True if the Property needs to be re-synchronized, else False.
        /// </returns>
        bool HasValueChanged(object binder);

        /// <summary>
        /// Reads the Property's value from an IValueReader and updates the
        /// Property's value with the value read.
        /// </summary>
        /// <param name="binder">The object to bind to to get the property value from or set the property value on.</param>
        /// <param name="reader">IValueReader to read the Property's new value from.</param>
        void ReadValue(object binder, IValueReader reader);

        /// <summary>
        /// Writes the Property's value to an IValueWriter.
        /// </summary>
        /// <param name="binder">The object to bind to to get the property value from or set the property value on.</param>
        /// <param name="writer">IValueWriter to write the Property's value to.</param>
        void WriteValue(object binder, IValueWriter writer);
    }
}