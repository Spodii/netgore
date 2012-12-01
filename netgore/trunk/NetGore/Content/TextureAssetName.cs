using System;
using System.ComponentModel;
using System.Linq;
using NetGore.IO;

namespace NetGore.Content
{
    /// <summary>
    /// An immutable string that represents the name, or virtual path, to a texture content asset.
    /// </summary>
    [TypeConverter(typeof(TextureAssetNameConverter))]
    public sealed class TextureAssetName : ContentAssetName
    {
        const string _rootDir = ContentPaths.GrhsFolder + PathSeparator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureAssetName"/> class.
        /// </summary>
        /// <param name="textureName">Name of the texture asset.</param>
        public TextureAssetName(string textureName) : base(GetContentAssetName(textureName))
        {
        }

        static string GetContentAssetName(string textureName)
        {
            string assetName = string.Empty;

            // Add the rootDir if it is not already there
            if (!textureName.StartsWith(_rootDir, StringComparison.OrdinalIgnoreCase))
                assetName += _rootDir;

            // Add the textureName, removing any trailing separators if they already exist (since its already on the end of the rootDir)
            assetName += textureName.TrimStart('/', '\\');

#if DEBUG
            // Sanity check to make sure we didn't accidentally get any double-slashes in there (and cleans up from when it did in older code)
            assetName = assetName.Replace("//", "/").Replace("\\\\", "\\");
#endif

            return assetName;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Value))
                return string.Empty;

            if (Value.Length > _rootDir.Length)
                return Value.Substring(_rootDir.Length);

            return Value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="TextureAssetName"/>.
        /// </summary>
        /// <param name="textureName">Name of the texture asset.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TextureAssetName(string textureName)
        {
            return new TextureAssetName(textureName);
        }
    }
}