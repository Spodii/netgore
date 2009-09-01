using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NetGore;
using NetGore.Globalization;
using NetGore.Graphics;

namespace DemoGame.SkeletonEditor
{
    /// <summary>
    /// Provides helper methods for loading Skeletons in ways that only the editor will need to load the.m
    /// </summary>
    public static class SkeletonLoader
    {
        public const string BasicSkeletonBodyName = "basic";
        public const string StandingSkeletonName = "stand";
        public const string WalkingSkeletonSetName = "walk";
        public const string FallingSkeletonSetName = "fall";
        public const string JumpingSkeletonSetName = "jump";

        /// <summary>
        /// Gets a SkeletonSet for the standing Skeleton.
        /// </summary>
        /// <returns>A SkeletonSet for the standing Skeleton.</returns>
        public static SkeletonSet GetStandingSkeletonSet()
        {
            Skeleton newSkeleton = new Skeleton(StandingSkeletonName, ContentPaths.Dev);
            SkeletonFrame nFrame0 = new SkeletonFrame(StandingSkeletonName, newSkeleton);
            SkeletonSet newSet = new SkeletonSet(new[] { nFrame0 });
            return newSet;
        }

        /// <summary>
        /// Loads a SkeletonBodyInfo from the given FilePath. Assumes the loading is from the ContentPaths.Dev.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The loaded SkeletonBodyInfo, or null if the SkeletonBodyInfo failed to load.</returns>
        public static SkeletonBodyInfo LoadSkeletonBodyInfo(string filePath)
        {
            string bodyName = Path.GetFileNameWithoutExtension(filePath);
            string realFilePath = SkeletonBodyInfo.GetFilePath(bodyName, ContentPaths.Dev);

            // Make sure the file exists
            if (!File.Exists(realFilePath))
            {
                const string errmsg = "Failed to load SkeletonBodyInfo `{0}` from `{1}` - file does not exist.";
                string err = string.Format(errmsg, bodyName, filePath);
                MessageBox.Show(err);
                return null;
            }

            // Try to load the skeleton
            SkeletonBodyInfo ret;
            try
            {
                ret = new SkeletonBodyInfo(bodyName, ContentPaths.Dev);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to load SkeletonBodyInfo `{0}` from `{1}`:{2}{3})";
                string err = string.Format(errmsg, bodyName, filePath, Environment.NewLine, ex);
                MessageBox.Show(err);
                return null;
            }

            return ret;
        }

        /// <summary>
        /// Loads a SkeletonSet from the given FilePath. Assumes the loading is from the ContentPaths.Dev.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The loaded SkeletonSet, or null if the SkeletonSet failed to load.</returns>
        public static SkeletonSet LoadSkeletonSet(string filePath)
        {
            string skeletonSetName = Path.GetFileNameWithoutExtension(filePath);
            string realFilePath = SkeletonSet.GetFilePath(skeletonSetName, ContentPaths.Dev);

            // Make sure the file exists
            if (!File.Exists(realFilePath))
            {
                const string errmsg = "Failed to load SkeletonSet `{0}` from `{1}` - file does not exist.";
                string err = string.Format(errmsg, skeletonSetName, filePath);
                MessageBox.Show(err);
                return null;
            }

            // Try to load the skeleton
            SkeletonSet ret;
            try
            {
                ret = new SkeletonSet(skeletonSetName, ContentPaths.Dev);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to load SkeletonSet `{0}` from `{1}`:{2}{3})";
                string err = string.Format(errmsg, skeletonSetName, filePath, Environment.NewLine, ex);
                MessageBox.Show(err);
                return null;
            }

            return ret;
        }

        /// <summary>
        /// Loads a Skeleton from the given FilePath. Assumes the loading is from the ContentPaths.Dev.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The loaded Skeleton, or null if the Skeleton failed to load.</returns>
        public static Skeleton LoadSkeleton(string filePath)
        {
            string skeletonName = Path.GetFileNameWithoutExtension(filePath);
            string realFilePath = Skeleton.GetFilePath(skeletonName, ContentPaths.Dev);

            // Make sure the file exists
            if (!File.Exists(realFilePath))
            {
                const string errmsg = "Failed to load Skeleton `{0}` from `{1}` - file does not exist.";
                string err = string.Format(errmsg, skeletonName, filePath);
                MessageBox.Show(err);
                return null;
            }

            // Try to load the skeleton
            Skeleton ret;
            try
            {
                ret = new Skeleton(skeletonName, ContentPaths.Dev);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to load Skeleton `{0}` from `{1}`:{2}{3})";
                string err = string.Format(errmsg, skeletonName, filePath, Environment.NewLine, ex);
                MessageBox.Show(err);
                return null;
            }

            return ret;
        }

        /// <summary>
        /// Loads a SkeletonSet from a string array.
        /// </summary>
        /// <param name="framesTxt">Array containing the text for each frame in the format name/time, where
        /// name is the name of the skeleton model and time is the delay time of the frame.</param>
        /// <returns>The loaded SkeletonSet.</returns>
        public static SkeletonSet LoadSkeletonSetFromString(string[] framesTxt)
        {
            if (framesTxt.Length == 0)
                throw new ArgumentException("framesTxt");

            var sep = new[] { "/" };

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
                Skeleton newSkeleton = new Skeleton(frameInfo[0], ContentPaths.Dev);
                frames[i] = new SkeletonFrame(frameInfo[0], newSkeleton, frameTime);
            }
            return new SkeletonSet(frames);
        }

        /// <summary>
        /// Loads a SkeletonSet from a string.
        /// </summary>
        /// <param name="text">Delimited string containing the text for each frame in the format name/time, where
        /// name is the name of the skeleton model and time is the delay time of the frame</param>
        /// <param name="separator">String to use to split the frames</param>
        /// <returns>The loaded SkeletonSet.</returns>
        public static SkeletonSet LoadSkeletonSetFromString(string text, string separator)
        {
            var sep = new[] { separator };
            var splitText = text.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            return LoadSkeletonSetFromString(splitText);
        }
    }
}