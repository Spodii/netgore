using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore
{
    /// <summary>
    /// Provides helper methods for Enums.
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Gets all of the values in an Enum of the given Type.
        /// </summary>
        /// <typeparam name="T">The Type of the Enum to get the values for.</typeparam>
        /// <returns>An array containing all of the values in an Enum of Type <typeparamref name="T"/>.</returns>
        /// <exception cref="MethodAccessException"><typeparamref name="T"/> is not an Enum.</exception>
        public static T[] GetValues<T>()
        {
            if (!typeof(T).IsEnum)
                throw new MethodAccessException("Type parameter T must be an enum.");

            return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }
    }
}
