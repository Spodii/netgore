using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace NetGore.AI
{
    /// <summary>
    /// Attribute used to denote a class as being for handling AI. This must be attached to all classes handling AI.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class AIAttribute : Attribute
    {
        readonly AIID _id;

        /// <summary>
        /// Initializes a new instance of the <see cref="AIAttribute"/> class.
        /// </summary>
        /// <param name="id">The unique ID of the AI. This must be unique for each individual attribute, and must
        /// be a valid value for the <see cref="AIID"/>.</param>
        public AIAttribute(int id)
        {
            _id = new AIID(id);
        }

        /// <summary>
        /// Gets the <see cref="AIID"/> of this attribute.
        /// </summary>
        public AIID ID
        {
            get { return _id; }
        }
    }
}