using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// Contains the information derived from an automatic animated <see cref="GrhData"/>'s file path.
    /// </summary>
    public class AutomaticAnimationInfo
    {
        readonly string _dir;
        readonly int _speed;
        readonly string _title;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomaticAnimationInfo"/> class.
        /// </summary>
        /// <param name="dir">Absolute path to the animation's frames directory.</param>
        /// <param name="title">Title to give the animation.</param>
        /// <param name="speed">Speed to give the animation.</param>
        public AutomaticAnimationInfo(string dir, string title, int speed)
        {
            _dir = dir;
            _title = title;
            _speed = speed;
        }

        /// <summary>
        /// Gets the absolute path to the frames directory.
        /// </summary>
        public string Dir
        {
            get { return _dir; }
        }

        /// <summary>
        /// Gets the speed of the animation.
        /// </summary>
        public int Speed
        {
            get { return _speed; }
        }

        /// <summary>
        /// Gets the title of the animation sprite.
        /// </summary>
        public string Title
        {
            get { return _title; }
        }
    }
}