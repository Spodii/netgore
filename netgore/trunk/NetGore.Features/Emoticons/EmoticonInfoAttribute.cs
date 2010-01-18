using System;
using System.Linq;

namespace NetGore.Features.Emoticons
{
    /// <summary>
    /// Provides some additional metadata about the <see cref="Emoticon"/> enum values.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EmoticonInfoAttribute : Attribute
    {
        readonly GrhIndex _grhIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmoticonInfoAttribute"/> class.
        /// </summary>
        /// <param name="grhIndex">The <see cref="GrhIndex"/> for the sprite to display.</param>
        public EmoticonInfoAttribute(int grhIndex)
        {
            _grhIndex = new GrhIndex(grhIndex);
        }

        /// <summary>
        /// Gets the <see cref="GrhIndex"/> for the sprite to display for the corresponding <see cref="Emoticon"/>.
        /// </summary>
        public GrhIndex GrhIndex
        {
            get { return _grhIndex; }
        }

        /// <summary>
        /// Gets the <see cref="Emoticon"/> this <see cref="EmoticonInfoAttribute"/> is attached to.
        /// </summary>
        public Emoticon Value { get; internal set; }
    }
}