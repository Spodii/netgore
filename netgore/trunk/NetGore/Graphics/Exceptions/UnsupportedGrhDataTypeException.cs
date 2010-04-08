using System;
using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// Exception for when a <see cref="GrhData"/> is of an unsupported derived type.
    /// </summary>
    public class UnsupportedGrhDataTypeException : GrhDataException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message.
        /// </summary>
        /// <param name="grhData">The <see cref="GrhData"/> related to the <see cref="Exception"/>.</param>
        public UnsupportedGrhDataTypeException(GrhData grhData)
            : base(
                grhData,
                string.Format("GrhData `{0}` is of derived type `{1}`, which is not supported by this operation.", grhData,
                              grhData.GetType()))
        {
        }
    }
}