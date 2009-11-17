using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.IO
{
    /// <summary>
    /// An immutable string that represents the name, or virtual path, to a content asset.
    /// </summary>
    public sealed class ContentAssetName 
    {
        readonly string _assetName;

        /// <summary>
        /// The string used to separate the directories.
        /// </summary>
        public const string PathSeparator = "/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentAssetName"/> class.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        public ContentAssetName(string assetName)
        {
            _assetName = Sanitize(assetName);
        }

        /// <summary>
        /// Sanitizes the asset name. This will fix aspects of the asset name that can be fixed without
        /// making too large of assumptions.
        /// </summary>
        /// <param name="assetName">The asset name.</param>
        /// <returns>The sanitized asset name.</returns>
        public static string Sanitize(string assetName)
        {
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
