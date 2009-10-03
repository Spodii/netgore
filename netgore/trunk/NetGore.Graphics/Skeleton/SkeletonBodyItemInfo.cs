using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// Describes a SkeletonBodyItem
    /// </summary>
    public class SkeletonBodyItemInfo
    {
        const string _destValueKey = "Destination";
        const string _grhIndexValueKey = "GrhIndex";
        const string _offsetValueKey = "Offset";
        const string _originValueKey = "Origin";
        const string _sourceValueKey = "Source";

        string _destName;
        GrhIndex _grhIndex;
        string _sourceName;

        /// <summary>
        /// Gets the name of the destination node.
        /// </summary>
        public string DestName
        {
            get { return _destName; }
        }

        /// <summary>
        /// Gets the GrhIndex to use.
        /// </summary>
        public GrhIndex GrhIndex
        {
            get { return _grhIndex; }
        }

        /// <summary>
        /// Gets or sets the Grh drawing offset.
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <summary>
        /// Gets or sets the Grh drawing origin.
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// Gets the name of the source node.
        /// </summary>
        public string SourceName
        {
            get { return _sourceName; }
        }

        /// <summary>
        /// SkeletonBodyItemInfo constructor.
        /// </summary>
        /// <param name="grhIndex">GrhIndex of the body item.</param>
        /// <param name="sourceName">Name of the source node.</param>
        /// <param name="destName">Name of the destination node (String.Empty for no destination).</param>
        /// <param name="offset">Grh drawing offset.</param>
        /// <param name="origin">Grh drawing origin.</param>
        public SkeletonBodyItemInfo(GrhIndex grhIndex, string sourceName, string destName, Vector2 offset, Vector2 origin)
        {
            _grhIndex = grhIndex;
            _sourceName = sourceName;
            _destName = destName;
            Offset = offset;
            Origin = origin;
        }

        public SkeletonBodyItemInfo(IValueReader reader)
        {
            Read(reader);
        }

        public void Read(IValueReader reader)
        {
            _grhIndex = reader.ReadGrhIndex(_grhIndexValueKey);
            _sourceName = reader.ReadString(_sourceValueKey);
            _destName = reader.ReadString(_destValueKey);
            Offset = reader.ReadVector2(_offsetValueKey);
            Origin = reader.ReadVector2(_originValueKey);
        }

        public void Write(IValueWriter writer)
        {
            writer.Write(_grhIndexValueKey, GrhIndex);
            writer.Write(_sourceValueKey, SourceName);
            writer.Write(_destValueKey, DestName);
            writer.Write(_offsetValueKey, Offset);
            writer.Write(_originValueKey, Origin);
        }
    }
}