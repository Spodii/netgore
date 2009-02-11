using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Platyform.Extensions;

namespace Platyform.Graphics
{
    /// <summary>
    /// Set of keyframes used to perform an animation
    /// </summary>
    public class SkeletonSet
    {
        /// <summary>
        /// Keyframes use by the set
        /// </summary>
        readonly SkeletonFrame[] _keyFrames;

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
        /// Loads a SkeletonSet from a file
        /// </summary>
        /// <param name="filePath">File to load from</param>
        /// <returns>New SkeletonSet as defined by the file</returns>
        public static SkeletonSet Load(string filePath)
        {
            var frames = new List<SkeletonFrame>();
            var fileInfo = XmlInfoReader.ReadFile(filePath, true);

            // Store the directory the file we are loading is in so we can load the frames
            string localPath = Path.GetDirectoryName(filePath);

            // Loop through every frame block, load the info and place it into the frames list
            foreach (var dic in fileInfo)
            {
                float delay = float.Parse(dic["Frame.Delay"]);
                string fileName = dic["Frame.FileName"];
                string path = Path.Combine(localPath, fileName + ".skel");
                Skeleton skel = Skeleton.Load(path);
                frames.Add(new SkeletonFrame(fileName, skel, delay));
            }

            return new SkeletonSet(frames.ToArray());
        }

        /// <summary>
        /// Loads a SkeletonSet from a string array
        /// </summary>
        /// <param name="framesTxt">Array containing the text for each frame in the format name/time, where
        /// name is the name of the skeleton model and time is the delay time of the frame</param>
        /// <returns>New skeletonSet object</returns>
        public static SkeletonSet Load(string[] framesTxt)
        {
            if (framesTxt.Length == 0)
                throw new ArgumentException("framesTxt");

            var frames = new SkeletonFrame[framesTxt.Length];
            for (int i = 0; i < frames.Length; i++)
            {
                // Split up the time and frame name
                var frameInfo = framesTxt[i].Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

                // If there is a defined time, use it
                float frameTime;
                if (frameInfo.Length == 1 || !float.TryParse(frameInfo[1], out frameTime))
                    frameTime = 200f;

                // Create the keyframe
                string filePath = ContentPaths.Build.Skeletons.Join(frameInfo[0] + ".skel");
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
        public static SkeletonSet Load(string text, string separator)
        {
            return Load(text.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        /// Saves a SkeletonSet to a file
        /// </summary>
        /// <param name="skelSet">SkeletonSet to save</param>
        /// <param name="filePath">File to save to</param>
        public static void Save(SkeletonSet skelSet, string filePath)
        {
            using (Stream s = File.Open(filePath, FileMode.Create))
            {
                XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
                using (XmlWriter w = XmlWriter.Create(s, settings))
                {
                    if (w == null)
                        throw new Exception("Failed to create XmlWriter for saving SkeletonSet.");

                    w.WriteStartDocument();
                    w.WriteStartElement("Set");

                    // Save all individual frames
                    foreach (SkeletonFrame f in skelSet.KeyFrames)
                    {
                        w.WriteStartElement("Frame");
                        w.WriteElementString("Delay", f.Delay.ToString());
                        w.WriteElementString("FileName", f.FileName);
                        w.WriteEndElement();
                    }

                    w.WriteEndElement();
                    w.WriteEndDocument();
                }
            }
        }

        /// <summary>
        /// Saves the SkeletonSet to a file
        /// </summary>
        /// <param name="filePath">File to save to</param>
        public void Save(string filePath)
        {
            Save(this, filePath);
        }
    }
}