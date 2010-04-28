using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// Defines how an <see cref="ISprite"/> animates.
    /// </summary>
    public enum AnimType : byte
    {
        /// <summary>
        /// <see cref="Grh"/> that will not animate.
        /// </summary>
        None,

        /// <summary>
        /// <see cref="Grh"/> will loop once then change to <see cref="AnimType.None"/> back on the first frame.
        /// </summary>
        LoopOnce,

        /// <summary>
        /// <see cref="Grh"/> will loop forever.
        /// </summary>
        Loop
    }
}