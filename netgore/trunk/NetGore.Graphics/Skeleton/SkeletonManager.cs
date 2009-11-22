using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// Content manager for skeletons
    /// </summary>
    public class SkeletonManager
    {
        /// <summary>
        /// Dictionary of SkeletonBodyInfos, identified by their partial file name
        /// </summary>
        readonly Dictionary<string, SkeletonBodyInfo> _bodyInfo =
            new Dictionary<string, SkeletonBodyInfo>(StringComparer.OrdinalIgnoreCase);

        readonly string _dir;

        /// <summary>
        /// Dictionary of SkeletonSets, identified by their partial file name
        /// </summary>
        readonly Dictionary<string, SkeletonSet> _sets = new Dictionary<string, SkeletonSet>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// SkeletonManager constructor
        /// </summary>
        /// <param name="dir">Directory to load from</param>
        public SkeletonManager(string dir)
        {
            _dir = dir;
        }

        /// <summary>
        /// Gets or the directory to load from.
        /// </summary>
        public string Dir
        {
            get { return _dir; }
        }

        public SkeletonBodyInfo LoadBodyInfo(string skeletonBodyInfoName, ContentPaths contentPath)
        {
            SkeletonBodyInfo ret;

            // Get the value from the dictionary
            if (!_bodyInfo.TryGetValue(skeletonBodyInfoName, out ret))
            {
                // Value did not already exist, load it
                ret = new SkeletonBodyInfo(skeletonBodyInfoName, contentPath);
                _bodyInfo.Add(skeletonBodyInfoName, ret);
            }

            return ret;
        }

        public SkeletonSet LoadSet(string skeletonSetName, ContentPaths contentPath)
        {
            SkeletonSet ret;

            // Get the value from the dictionary
            if (!_sets.TryGetValue(skeletonSetName, out ret))
            {
                // Value did not already exist, so load it
                ret = new SkeletonSet(skeletonSetName, contentPath);
                _sets.Add(skeletonSetName, ret);
            }

            return ret;
        }
    }
}