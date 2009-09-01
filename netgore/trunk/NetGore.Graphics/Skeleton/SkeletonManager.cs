using System;
using System.Collections.Generic;
using System.Linq;

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
        readonly Dictionary<string, SkeletonBodyInfo> _bodyInfo = new Dictionary<string, SkeletonBodyInfo>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Dictionary of SkeletonSets, identified by their partial file name
        /// </summary>
        readonly Dictionary<string, SkeletonSet> _sets = new Dictionary<string, SkeletonSet>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Directory to load from
        /// </summary>
        string _dir;

        /// <summary>
        /// Gets or sets the directory to load from
        /// </summary>
        public string Dir
        {
            get { return _dir; }
            set
            {
                if (!value.EndsWith("/"))
                    _dir = value + "/";
                else
                    _dir = value;
            }
        }

        /// <summary>
        /// SkeletonManager constructor
        /// </summary>
        /// <param name="dir">Directory to load from</param>
        public SkeletonManager(string dir)
        {
            Dir = dir;
        }

        /// <summary>
        /// Loads a SkeletonBodyInfo by the given name or a cached copy of it
        /// </summary>
        /// <param name="name">Name of the SkeletonBodyInfo</param>
        /// <returns>SkeletonBodyInfo by the given name</returns>
        public SkeletonBodyInfo LoadBodyInfo(string name)
        {
            SkeletonBodyInfo ret;

            // Get the value from the dictionary
            if (!_bodyInfo.TryGetValue(name, out ret))
            {
                // Value did not already exist, load it
                ret = new SkeletonBodyInfo(_dir + name + ".skelb");
                _bodyInfo.Add(name, ret);
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