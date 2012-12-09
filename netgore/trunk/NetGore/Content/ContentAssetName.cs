using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using log4net;
using NetGore.IO;

namespace NetGore.Content
{
    /// <summary>
    /// An immutable string that represents the name, or virtual path, to a content asset.
    /// </summary>
    [TypeConverter(typeof(ContentAssetNameConverter))]
    public class ContentAssetName : IEquatable<ContentAssetName>, IComparable<ContentAssetName>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The string used to separate the directories.
        /// </summary>
        public const string PathSeparator = "/";

        readonly string _assetName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentAssetName"/> class.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        public ContentAssetName(string assetName)
        {
            _assetName = Sanitize(assetName);
        }

        /// <summary>
        /// Gets the complete name of the asset. This differs from <see cref="ToString"/> as this will always
        /// be the complete name of the asset, while <see cref="ToString"/> may only be a partial path in
        /// derived classes.
        /// </summary>
        public string Value
        {
            get { return _assetName; }
        }

        /// <summary>
        /// Checks if the content with the given name exists.
        /// </summary>
        /// <returns>True if the content exists in the build; otherwise false.</returns>
        public bool ContentExists()
        {
            var filePath = ContentPaths.Build.Root.Join(GetFileName());
            return File.Exists(filePath);
        }

        /// <summary>
        /// Checks if this object is equal to another object.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns>True if the two are equal; otherwise false.</returns>
        public override bool Equals(object obj)
        {
            return obj is ContentAssetName && this == (ContentAssetName)obj;
        }

        /// <summary>
        /// Creates a <see cref="ContentAssetName"/> from an absolute file path.
        /// </summary>
        /// <param name="filePath">The absolute file path to the asset.</param>
        /// <param name="contentRoot">The root path to the content directory.</param>
        /// <returns>The <see cref="ContentAssetName"/> from the <paramref name="filePath"/>.</returns>
        public static ContentAssetName FromAbsoluteFilePath(string filePath, string contentRoot)
        {
            var start = contentRoot.Length;
            if (!contentRoot.EndsWith("/") && !contentRoot.EndsWith("\\") &&
                !contentRoot.EndsWith(Path.DirectorySeparatorChar.ToString()))
                ++start;

            var len = filePath.Length - start;
            if (ContentPaths.ContentFileSuffix.Length > 0 &&
                filePath.EndsWith(ContentPaths.ContentFileSuffix, StringComparison.OrdinalIgnoreCase))
                len -= ContentPaths.ContentFileSuffix.Length;

            var substr = filePath.Substring(start, len);
            return new ContentAssetName(substr);
        }

        /// <summary>
        /// Gets the absolute file path for the content asset.
        /// </summary>
        /// <param name="rootPath">The root content path.</param>
        /// <returns>The absolute file path for the content asset.</returns>
        /// <exception cref="ArgumentException">Either zero or more than one files matching this <see cref="ContentAssetName"/>
        /// were found in the <paramref name="rootPath"/>.</exception>
        public string GetAbsoluteFilePath(ContentPaths rootPath)
        {
            var sb = new StringBuilder();

            // Get the root path
            string rootPathStr = rootPath.Root.ToString();
            sb.Append(rootPathStr);

            // Get the relative file path
            string relFilePathStr = GetFileName();

            // Get all of the files in the directory that this file lives in
            string dirPath = Path.GetDirectoryName(Path.Combine(rootPathStr, relFilePathStr));
            string[] dirFiles = Directory.GetFiles(dirPath, "*.*", SearchOption.TopDirectoryOnly);

            // Take all the files in the directory, filter out the markup and suffix, and find which file names match ours
            string targetFileName = Path.GetFileName(relFilePathStr);
            targetFileName = RemoveFileTagsFromFileName(targetFileName);

            string[] contentFilePaths = dirFiles.Where(x =>
            {
                string xFileName = Path.GetFileNameWithoutExtension(x);
                xFileName = RemoveFileTagsFromFileName(xFileName);
                return StringComparer.OrdinalIgnoreCase.Equals(xFileName, targetFileName);
            })
            .ToArray();

            // Verify we found exactly 1 match
            if (contentFilePaths.Length == 0)
            {
                throw new ArgumentException(string.Format(
                    "Could not find a file named (without the [] markup and suffix) `{0}` in path `{1}`.",
                    targetFileName, dirPath));
            }
            if (contentFilePaths.Length > 1)
            {
                throw new ArgumentException(string.Format(
                    "Found multiple potential matches for the file named (without the [] markup and suffix) `{0}` in path `{1}`. Was expecting just one.",
                    targetFileName, dirPath));
            }

            return contentFilePaths[0];
        }

        /// <summary>
        /// Removes the file tags from a file name.
        /// </summary>
        /// <param name="fileName">The file name only (no directories, no suffix).</param>
        static string RemoveFileTagsFromFileName(string fileName)
        {
            int splitIndex = fileName.IndexOf('[');
            if (splitIndex > 0)
                fileName = fileName.Substring(0, splitIndex);
            return fileName;
        }

        /// <summary>
        /// Gets the relative file path and name for the content asset. This must still be prefixed by a path created
        /// with the <see cref="ContentPaths"/> to generate an absolute path.
        /// </summary>
        /// <returns>The relative file path and name for the content asset..</returns>
        public string GetFileName()
        {
            return _assetName + ContentPaths.ContentFileSuffix;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures
        /// like a hash table. </returns>
        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
        }

        /// <summary>
        /// Recycles the actual content file. The file located in the <see cref="ContentPaths.Dev"/> path (if exists) will be moved
        /// to the <see cref="ContentPaths.Recycled"/>. Other instances of the file will just be deleted from the system.
        /// </summary>
        public void RecycleFile()
        {
            if (log.IsInfoEnabled)
                log.InfoFormat("Recycling `{0}`.", this);

            // Delete file in build path
            var buildPath = GetAbsoluteFilePath(ContentPaths.Build);
            TryDeleteFile(buildPath);

            // Check for recycle folder
            var recycled = ContentPaths.Recycled;
            if (recycled == null)
            {
                const string errmsg = "Cannot recycle `{0}` - ContentPaths.Recycled returned null.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
                return;
            }

            // Copy from dev to recycling, then delete from dev
            var srcPath = GetAbsoluteFilePath(ContentPaths.Dev);
            if (File.Exists(srcPath))
            {
                // Get the relative file path by taking the source path and chopping off the ContentPath's root directory prefix
                var relPath = srcPath.Substring(ContentPaths.Dev.Root.ToString().Length);

                // Get the destination path by joining the relative path with the recycled path
                var destPath = ContentPaths.Recycled.Join(relPath);

                // Create directory if it doesn't already exist
                var destPathDir = Path.GetDirectoryName(destPath);
                if (destPathDir != null && !Directory.Exists(destPathDir))
                    Directory.CreateDirectory(destPathDir);

                // Copy file
                File.Copy(srcPath, destPath, true);

                // Ensure the file was copied over before deleting
                if (File.Exists(destPath))
                {
                    TryDeleteFile(srcPath);
                }
                else
                {
                    // Failed to copy over?
                    const string errmsg = "File.Copy() from `{0}` to `{1}` seems to have failed - File.Exists() returned false.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, srcPath, destPath);
                    Debug.Fail(string.Format(errmsg, srcPath, destPath));
                }
            }
        }

        /// <summary>
        /// Sanitizes the asset name. This will fix aspects of the asset name that can be fixed without
        /// making too large of assumptions.
        /// </summary>
        /// <param name="assetName">The asset name.</param>
        /// <returns>The sanitized asset name.</returns>
        public static string Sanitize(string assetName)
        {
            var suffixLen = ContentPaths.ContentFileSuffix.Length;

            // Replace \\ with the proper character
            assetName = assetName.Replace("\\", PathSeparator);

            // Remove any prefixed or suffixed path separators
            if (assetName.StartsWith(PathSeparator))
                assetName = assetName.Substring(1);

            if (assetName.EndsWith(PathSeparator))
                assetName = assetName.Substring(0, assetName.Length - 1);

            if (assetName.Length > suffixLen &&
                (ContentPaths.ContentFileSuffix.Length == 0 ||
                 assetName.EndsWith(ContentPaths.ContentFileSuffix, StringComparison.OrdinalIgnoreCase)))
                assetName = assetName.Substring(0, assetName.Length - suffixLen);

            return assetName;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return _assetName;
        }

        /// <summary>
        /// Attempts to delete a file. No exceptions are ever thrown.
        /// </summary>
        /// <param name="filePath">The path to the file to delete.</param>
        /// <returns>True if the file at the <paramref name="filePath"/> was deleted; otherwise false.</returns>
        static bool TryDeleteFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return false;

                File.Delete(filePath);

                return true;
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to delete file at `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, filePath, ex);

                return false;
            }
        }

        #region IComparable<ContentAssetName> Members

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(ContentAssetName other)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(Value, other.Value);
        }

        #endregion

        #region IEquatable<ContentAssetName> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ContentAssetName other)
        {
            return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ContentAssetName a, ContentAssetName b)
        {
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return ReferenceEquals(a, b);

            return a.Equals(b);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ContentAssetName a, ContentAssetName b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="ContentAssetName"/>.
        /// </summary>
        /// <param name="assetName">The asset name.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ContentAssetName(string assetName)
        {
            return new ContentAssetName(assetName);
        }
    }
}