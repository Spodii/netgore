using System;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// An immutable string that represents the name, or virtual path, to a content asset.
    /// </summary>
    [TypeConverter(typeof(ContentAssetNameConverter))]
    public class ContentAssetName
    {
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
        /// Creates a <see cref="ContentAssetName"/> from an absolute file path.
        /// </summary>
        /// <param name="filePath">The absolute file path to the asset.</param>
        /// <param name="contentRoot">The root path to the content directory.</param>
        /// <returns>The <see cref="ContentAssetName"/> from the <paramref name="filePath"/>.</returns>
        public static ContentAssetName FromAbsoluteFilePath(string filePath, string contentRoot)
        {
            int start = contentRoot.Length;
            if (!contentRoot.EndsWith("/") && !contentRoot.EndsWith("\\") &&
                !contentRoot.EndsWith(Path.DirectorySeparatorChar.ToString()))
                ++start;

            int len = filePath.Length - start;
            if (filePath.EndsWith("." + ContentPaths.CompiledContentSuffix, StringComparison.OrdinalIgnoreCase))
                len -= 1 + ContentPaths.CompiledContentSuffix.Length;

            var substr = filePath.Substring(start, len);
            return new ContentAssetName(substr);
        }

        /// <summary>
        /// Gets the relative file path and name for the content asset. This must still be prefixed by a path created
        /// with the <see cref="ContentPaths"/> to generate an absolute path.
        /// </summary>
        /// <returns>The relative file path and name for the content asset..</returns>
        public string GetFileName()
        {
            // NOTE: !! Create unit tests

            return _assetName + "." + ContentPaths.CompiledContentSuffix;
        }

        /// <summary>
        /// Sanitizes the asset name. This will fix aspects of the asset name that can be fixed without
        /// making too large of assumptions.
        /// </summary>
        /// <param name="assetName">The asset name.</param>
        /// <returns>The sanitized asset name.</returns>
        public static string Sanitize(string assetName)
        {
            // NOTE: !! Create unit tests
            // Replace \\ with the proper character
            assetName = assetName.Replace("\\", PathSeparator);

            // Remove any prefixed or suffixed path separators
            if (assetName.StartsWith(PathSeparator))
                assetName = assetName.Substring(1);

            if (assetName.EndsWith(PathSeparator))
                assetName = assetName.Substring(0, assetName.Length - 1);

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
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="NetGore.IO.ContentAssetName"/>.
        /// </summary>
        /// <param name="assetName">The asset name.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ContentAssetName(string assetName)
        {
            return new ContentAssetName(assetName);
        }
    }
}