using System.Linq;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// Describes the visibility levels for members.
    /// </summary>
    public enum MemberVisibilityLevel : byte
    {
        /// <summary>
        /// Private (local scope only).
        /// </summary>
        Private,

        /// <summary>
        /// Protected (private + visible to inheritors).
        /// </summary>
        Protected,

        /// <summary>
        /// Internal (private + visible to namespace).
        /// </summary>
        Internal,

        /// <summary>
        /// Protected + Internal.
        /// </summary>
        ProtectedInternal,

        /// <summary>
        /// Visible to all.
        /// </summary>
        Public
    }
}