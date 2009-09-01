using System;
using System.IO;
using System.Linq;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// A single keyframe used in a skeleton animation
    /// </summary>
    public class SkeletonFrame
    {
        const string _delayValueKey = "Delay";
        const string _fileNameValueKey = "FileName";

        /// <summary>
        /// Amount of time the animation will stay on this frame in milliseconds
        /// </summary>
        float _delay;

        /// <summary>
        /// File name of the frame
        /// </summary>
        string _fileName;

        /// <summary>
        /// Skeleton used for the frame
        /// </summary>
        Skeleton _skeleton;

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

        public SkeletonFrame(IValueReader reader, ContentPaths contentPath)
        {
            Read(reader, contentPath);

            if (Skeleton == null)
                throw new Exception("Skeleton is null.");
        }

        public void Read(IValueReader reader, ContentPaths contentPath)
        {
            _delay = reader.ReadFloat(_delayValueKey);
            _fileName = reader.ReadString(_fileNameValueKey);
            _skeleton = new Skeleton(_fileName, contentPath);
        }

        public void Write(IValueWriter writer)
        {
            writer.Write(_delayValueKey, Delay);
            writer.Write(_fileNameValueKey, FileName);
        }
    }
}