using System.IO;
using System.Linq;
using System.Xml;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// Describes a SkeletonBody.
    /// </summary>
    public class SkeletonBodyInfo
    {
        /// <summary>
        /// The file suffix used for the SkeletonBody.
        /// </summary>
        public const string FileSuffix = ".skelb";

        const string _itemsNodeName = "BodyItems";
        const string _rootNodeName = "SkeletonBody";

        /// <summary>
        /// An array of all the SkeletonBodyItemInfos in this SkeletonBodyInfo
        /// </summary>
        SkeletonBodyItemInfo[] _items;

        /// <summary>
        /// Gets an array of all the SkeletonBodyItemInfos in this SkeletonBodyInfo
        /// </summary>
        internal SkeletonBodyItemInfo[] Items
        {
            get { return _items; }
        }

        /// <summary>
        /// SkeletonBodyInfo constructor
        /// </summary>
        /// <param name="items">SkeletonBodyItemInfos to create the SkeletonBodyInfo from</param>
        public SkeletonBodyInfo(SkeletonBodyItemInfo[] items)
        {
            _items = items;
        }

        public SkeletonBodyInfo(string filePath) : this(new XmlValueReader(filePath, _rootNodeName))
        {
        }

        public SkeletonBodyInfo(IValueReader reader)
        {
            Read(reader);
        }

        public void Read(IValueReader reader)
        {
            var loadedItems = reader.ReadManyNodes(_itemsNodeName, x => new SkeletonBodyItemInfo(x));
            _items = loadedItems;
        }

        public void Write(IValueWriter writer)
        {
            writer.WriteManyNodes(_itemsNodeName, Items, ((w, item) => item.Write(w)));
        }

        public void Write(string filePath)
        {
            using (XmlValueWriter writer = new XmlValueWriter(filePath, _rootNodeName))
            {
                Write(writer);
            }
        }
    }
}