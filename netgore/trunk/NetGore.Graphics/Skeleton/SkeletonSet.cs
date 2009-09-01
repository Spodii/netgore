using System;
using System.Linq;
using System.Text;
using NetGore.Globalization;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// Set of keyframes used to perform an animation
    /// </summary>
    public class SkeletonSet
    {
        /// <summary>
        /// The file suffix used for the SkeletonSet.
        /// </summary>
        public const string FileSuffix = ".skels";

        const string _framesNodeName = "Frames";
        const string _rootNodeName = "SkeletonSet";

        /// <summary>
        /// Keyframes use by the set
        /// </summary>
        SkeletonFrame[] _keyFrames;

        /// <summary>
        /// Gets the keyframes used by the set
        /// </summary>
        public SkeletonFrame[] KeyFrames
        {
            get { return _keyFrames; }
        }

        /// <summary>
        /// Constructor for the SkeletonSet
        /// </summary>
        /// <param name="keyFrames">Array of frames to use for the keyframes</param>
        public SkeletonSet(SkeletonFrame[] keyFrames)
        {
            _keyFrames = keyFrames;
        }

        public SkeletonSet(IValueReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// Creates a string to represent the SkeletonSet
        /// </summary>
        /// <returns>String representing the SkeletonSet</returns>
        public string GetFramesString()
        {
            StringBuilder sb = new StringBuilder(1000);
            foreach (SkeletonFrame frame in _keyFrames)
            {
                sb.AppendLine(frame.FileName + "/" + frame.Delay);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Loads a SkeletonSet from a string array
        /// </summary>
        /// <param name="framesTxt">Array containing the text for each frame in the format name/time, where
        /// name is the name of the skeleton model and time is the delay time of the frame</param>
        /// <returns>New skeletonSet object</returns>
        public static SkeletonSet Read(string[] framesTxt)
        {
            if (framesTxt.Length == 0)
                throw new ArgumentException("framesTxt");

            var sep = new[]
            {
                "/"
            };

            var frames = new SkeletonFrame[framesTxt.Length];
            for (int i = 0; i < frames.Length; i++)
            {
                // Split up the time and frame name
                var frameInfo = framesTxt[i].Split(sep, StringSplitOptions.RemoveEmptyEntries);

                // If there is a defined time, use it
                float frameTime;
                if (frameInfo.Length == 1 || !Parser.Invariant.TryParse(frameInfo[1], out frameTime))
                    frameTime = 200f;

                // Create the keyframe
                string filePath = ContentPaths.Build.Skeletons.Join(frameInfo[0] + Skeleton.FileSuffix);
                Skeleton newSkeleton = Skeleton.Load(filePath);
                frames[i] = new SkeletonFrame(frameInfo[0], newSkeleton, frameTime);
            }
            return new SkeletonSet(frames);
        }

        /// <summary>
        /// Loads a SkeletonSet from a string
        /// </summary>
        /// <param name="text">Delimited string containing the text for each frame in the format name/time, where
        /// name is the name of the skeleton model and time is the delay time of the frame</param>
        /// <param name="separator">String to use to split the frames</param>
        /// <returns>New SkeletonSet object</returns>
        public static SkeletonSet Read(string text, string separator)
        {
            var sep = new[]
            {
                separator
            };

            var splitText = text.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            return Read(splitText);
        }

        public SkeletonSet(string filePath) : this(new XmlValueReader(filePath, _rootNodeName))
        {
        }

        public void Read(IValueReader reader)
        {
            var loadedFrames = reader.ReadManyNodes(_framesNodeName, x => new SkeletonFrame(x));
            _keyFrames = loadedFrames;
        }

        public void Write(IValueWriter writer)
        {
            writer.WriteManyNodes(_framesNodeName, KeyFrames, ((w, item) => item.Write(w)));
        }

        /// <summary>
        /// Saves the SkeletonSet to a file.
        /// </summary>
        /// <param name="filePath">File to save to.</param>
        public void Write(string filePath)
        {
            using (IValueWriter writer = new XmlValueWriter(filePath, _rootNodeName))
            {
                Write(writer);
            }
        }
    }
}