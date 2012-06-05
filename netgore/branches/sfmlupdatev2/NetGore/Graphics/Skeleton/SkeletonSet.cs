using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
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
        /// Initializes a new instance of the <see cref="SkeletonSet"/> class.
        /// </summary>
        /// <param name="keyFrames">Array of frames to use for the keyframes.</param>
        public SkeletonSet(SkeletonFrame[] keyFrames)
        {
            _keyFrames = keyFrames;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonSet"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the data from.</param>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to use to load other data.</param>
        public SkeletonSet(IValueReader reader, ContentPaths contentPath)
        {
            Read(reader, contentPath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonSet"/> class.
        /// </summary>
        /// <param name="skeletonSetName">Name of the <see cref="SkeletonSet"/>.</param>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to load from.</param>
        public SkeletonSet(string skeletonSetName, ContentPaths contentPath)
            : this(GenericValueReader.CreateFromFile(GetFilePath(skeletonSetName, contentPath), _rootNodeName), contentPath)
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="GenericValueIOFormat"/> to use for when an instance of this class
        /// writes itself out to a new <see cref="GenericValueWriter"/>. If null, the format to use
        /// will be inherited from <see cref="GenericValueWriter.DefaultFormat"/>.
        /// Default value is null.
        /// </summary>
        public static GenericValueIOFormat? EncodingFormat { get; set; }

        /// <summary>
        /// Gets the keyframes used by the set
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public SkeletonFrame[] KeyFrames
        {
            get { return _keyFrames; }
        }

        /// <summary>
        /// Gets the absolute file path for a SkeletonSet file.
        /// </summary>
        /// <param name="skeletonSetName">The name of the SkeletonSet.</param>
        /// <param name="contentPath">The content path to use.</param>
        /// <returns>The absolute file path for a SkeletonSet file.</returns>
        public static string GetFilePath(string skeletonSetName, ContentPaths contentPath)
        {
            return contentPath.Skeletons.Join(skeletonSetName + FileSuffix);
        }

        /// <summary>
        /// Creates a string to represent the SkeletonSet.
        /// </summary>
        /// <returns>String representing the SkeletonSet.</returns>
        public string GetFramesString()
        {
            var sb = new StringBuilder(512);
            foreach (var frame in _keyFrames)
            {
                sb.Append(frame.FileName);
                sb.Append("/");
                sb.Append(frame.Delay);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the names of all the <see cref="SkeletonSet"/>s that exist in the specified <see cref="ContentPaths"/>.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to search.</param>
        public static IEnumerable<string> GetSetNames(ContentPaths contentPath)
        {
            var allFiles = Directory.GetFiles(contentPath.Skeletons, "*" + FileSuffix, SearchOption.AllDirectories);
            var withoutExtensions = allFiles.Select(Path.GetFileNameWithoutExtension);
            return withoutExtensions;
        }

        public void Read(IValueReader reader, ContentPaths contentPath)
        {
            var loadedFrames = reader.ReadManyNodes(_framesNodeName, x => new SkeletonFrame(x, contentPath));
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
            using (var writer = GenericValueWriter.Create(filePath, _rootNodeName, EncodingFormat))
            {
                Write(writer);
            }
        }
    }
}