using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// Contains the information derived from an automatic animated <see cref="GrhData"/>'s file path.
    /// </summary>
    public class AutomaticAnimationInfo
    {
        /// <summary>
        /// Absolute path to the frames directory.
        /// </summary>
        public readonly string Dir;

        /// <summary>
        /// Speed of the animation.
        /// </summary>
        public readonly int Speed;

        /// <summary>
        /// Title of the animation sprite.
        /// </summary>
        public readonly string Title;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomaticAnimationInfo"/> class.
        /// </summary>
        /// <param name="dir">Absolute path to the animation's frames directory.</param>
        /// <param name="title">Title to give the animation.</param>
        /// <param name="speed">Speed to give the animation.</param>
        public AutomaticAnimationInfo(string dir, string title, int speed)
        {
            Dir = dir;
            Title = title;
            Speed = speed;
        }
    }
}