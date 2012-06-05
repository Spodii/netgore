using System;
using System.Linq;
using System.Runtime.Serialization;

namespace NetGore.Graphics
{
    /// <summary>
    /// <see cref="Exception"/> for when a <see cref="GrhData"/> could not be found.
    /// </summary>
    [Serializable]
    public sealed class GrhDataNotFoundException : GrhDataException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrhDataNotFoundException"/> class.
        /// </summary>
        public GrhDataNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class.
        /// </summary>
        /// <param name="grhIndex">Index of the GRH.</param>
        public GrhDataNotFoundException(GrhIndex grhIndex) : base(string.Format("No GrhData found for GrhIndex `{0}`.", grhIndex))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="title">The title.</param>
        public GrhDataNotFoundException(string category, string title)
            : base(string.Format("No GrhData found for GrhIndex with category `{0}` and title `{1}`.", category, title))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class.
        /// </summary>
        /// <param name="categorization">The categorization.</param>
        public GrhDataNotFoundException(string categorization)
            : base(string.Format("No GrhData found for GrhIndex with categorization `{0}`.", categorization))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class.
        /// </summary>
        /// <param name="grhIndex">Index of the GRH.</param>
        /// <param name="innerException">The inner exception.</param>
        public GrhDataNotFoundException(GrhIndex grhIndex, Exception innerException)
            : base(string.Format("No GrhData found for GrhIndex `{0}`.", grhIndex), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="title">The title.</param>
        /// <param name="innerException">The inner exception.</param>
        public GrhDataNotFoundException(string category, string title, Exception innerException)
            : base(
                string.Format("No GrhData found for GrhIndex with category `{0}` and title `{1}`.", category, title),
                innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class.
        /// </summary>
        /// <param name="categorization">The categorization.</param>
        /// <param name="innerException">The inner exception.</param>
        public GrhDataNotFoundException(string categorization, Exception innerException)
            : base(string.Format("No GrhData found for GrhIndex with categorization `{0}`.", categorization), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhDataNotFoundException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds
        /// the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that
        /// contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null
        /// or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        GrhDataNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}