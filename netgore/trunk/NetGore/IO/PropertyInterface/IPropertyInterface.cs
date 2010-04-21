using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// Interface for an object that provides a typed interface to the property of a Type for an object instance.
    /// </summary>
    /// <typeparam name="TObj">Type of the object to get the property of.</typeparam>
    /// <typeparam name="T">The type of the property.</typeparam>
    public interface IPropertyInterface<in TObj, T>
    {
        /// <summary>
        /// Gets the value of the property for the given <paramref name="obj"/> instance.
        /// </summary>
        /// <param name="obj">The object instance to get the property value for.</param>
        /// <returns>The value of the property for the given <paramref name="obj"/> instance.</returns>
        T Get(TObj obj);

        /// <summary>
        /// Sets the value of the property for the given <paramref name="obj"/> instance.
        /// </summary>
        /// <param name="obj">The object instance to set the property value for.</param>
        /// <param name="value">The value to set the <paramref name="obj"/>'s property to.</param>
        void Set(TObj obj, T value);
    }
}