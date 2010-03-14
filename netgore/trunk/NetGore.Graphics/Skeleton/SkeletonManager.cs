using System;
using System.Linq;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// Contains all of the information for <see cref="Skeleton"/>s.
    /// </summary>
    public class SkeletonManager
    {
        static readonly ICache<ContentPaths, SkeletonManager> _skeletonManagerCache;

        readonly ICache<string, SkeletonBodyInfo> _bodyInfoCache;
        readonly ContentPaths _contentPath;
        readonly ICache<string, SkeletonSet> _setCache;

        /// <summary>
        /// Initializes the <see cref="SkeletonManager"/> class.
        /// </summary>
        static SkeletonManager()
        {
            _skeletonManagerCache = new ThreadSafeHashCache<ContentPaths, SkeletonManager>(x => new SkeletonManager(x));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonManager"/> class.
        /// </summary>
        /// <param name="contentPath">The content path.</param>
        SkeletonManager(ContentPaths contentPath)
        {
            _contentPath = contentPath;

            var keyComparer = StringComparer.OrdinalIgnoreCase;
            _bodyInfoCache = new HashCache<string, SkeletonBodyInfo>(x => new SkeletonBodyInfo(x, ContentPath), keyComparer);
            _setCache = new HashCache<string, SkeletonSet>(x => new SkeletonSet(x, ContentPath), keyComparer);
        }

        /// <summary>
        /// Gets the root <see cref="ContentPaths"/> to use for loading data for the skeletons.
        /// </summary>
        public ContentPaths ContentPath
        {
            get { return _contentPath; }
        }

        /// <summary>
        /// Creates a <see cref="SkeletonManager"/> for the specified <see cref="ContentPaths"/>.
        /// </summary>
        /// <param name="contentPath">The content path.</param>
        /// <returns>The <see cref="SkeletonManager"/> for the specified <see cref="ContentPaths"/>.</returns>
        public static SkeletonManager Create(ContentPaths contentPath)
        {
            return _skeletonManagerCache[contentPath];
        }

        /// <summary>
        /// Gets a <see cref="SkeletonBodyInfo"/>.
        /// </summary>
        /// <param name="skeletonBodyInfoName">The name of the <see cref="SkeletonBodyInfo"/>.</param>
        /// <returns>The <see cref="SkeletonBodyInfo"/> with the given <paramref name="skeletonBodyInfoName"/>, or null
        /// if the the <paramref name="skeletonBodyInfoName"/> is invalid.</returns>
        public SkeletonBodyInfo GetBodyInfo(string skeletonBodyInfoName)
        {
            return _bodyInfoCache[skeletonBodyInfoName];
        }

        /// <summary>
        /// Gets a <see cref="SkeletonSet"/>.
        /// </summary>
        /// <param name="skeletonSetName">The name of the <see cref="SkeletonSet"/>.</param>
        /// <returns>The <see cref="SkeletonSet"/> with the given <paramref name="skeletonSetName"/>, or null
        /// if the the <paramref name="skeletonSetName"/> is invalid.</returns>
        public SkeletonSet GetSet(string skeletonSetName)
        {
            return _setCache[skeletonSetName];
        }
    }
}