using System;

namespace NetGore.Graphics
{
    /// <summary>
    /// A single keyframe used in a skeleton animation
    /// </summary>
    public class SkeletonFrame
    {
        /// <summary>
        /// Amount of time the animation will stay on this frame in milliseconds
        /// </summary>
        readonly float _delay;

        /// <summary>
        /// File name of the frame
        /// </summary>
        readonly string _fileName;

        /// <summary>
        /// Skeleton used for the frame
        /// </summary>
        readonly Skeleton _skeleton;

        /// <summary>
        /// Gets the amount of time the animation will stay on this frame in milliseconds.
        /// A value of 0 will result in the delay being found by the other frame - useful for stopped animation frames.
        /// </summary>
        public float Delay
        {
            get { return _delay; }
        }

        /// <summary>
        /// Gets the file name of the frame
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }

        /// <summary>
        /// Gets the skeleton used for the frame
        /// </summary>
        public Skeleton Skeleton
        {
            get { return _skeleton; }
        }

        /// <summary>
        /// Skeleton frame constructor
        /// </summary>
        /// <param name="fileName">Path to the file used to load the frame</param>
        /// <param name="skeleton">Skeleton to use for the frame</param>
        /// <param name="delay">Amount of time the animation will stay on this frame in milliseconds.
        /// A value of 0 will result in the delay being found by the other frame - useful for stopped animation frames.</param>
        public SkeletonFrame(string fileName, Skeleton skeleton, float delay)
        {
            if (skeleton == null)
                throw new ArgumentNullException("skeleton");

            _fileName = fileName;
            _skeleton = skeleton;
            _delay = delay;
        }

        /// <summary>
        /// Skeleton frame constructor
        /// </summary>
        /// <param name="fileName">Path to the file used to load the frame</param>
        /// <param name="skeleton">Skeleton to use for the frame</param>
        public SkeletonFrame(string fileName, Skeleton skeleton) : this(fileName, skeleton, 0f)
        {
        }
    }
}