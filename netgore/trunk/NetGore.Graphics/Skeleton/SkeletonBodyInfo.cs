using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace NetGore.Graphics
{
    /// <summary>
    /// Describes a SkeletonBody
    /// </summary>
    public class SkeletonBodyInfo
    {
        /// <summary>
        /// An array of all the SkeletonBodyItemInfos in this SkeletonBodyInfo
        /// </summary>
        readonly SkeletonBodyItemInfo[] _items;

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

        /// <summary>
        /// SkeletonBodyInfo constructor
        /// </summary>
        /// <param name="filePath">File to load the SkeletonBodyInfo from</param>
        public SkeletonBodyInfo(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                using (XmlReader r = XmlReader.Create(fs))
                {
                    int currBodyIndex = 0;
                    while (r.Read())
                    {
                        if (r.NodeType == XmlNodeType.Element)
                        {
                            if (r.Name == "ItemsCount")
                                _items = new SkeletonBodyItemInfo[r.ReadElementContentAsInt()];
                            else if (r.Name == "BodyItem")
                            {
                                _items[currBodyIndex] = SkeletonBodyItemInfo.Load(r.ReadSubtree());
                                currBodyIndex++;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves the SkeletonBodyInfo to a file
        /// </summary>
        /// <param name="filePath"></param>
        public void Save(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
                using (XmlWriter w = XmlWriter.Create(fs, settings))
                {
                    if (w == null)
                        throw new Exception("Failed to create XmlWriter for saving SkeletonBodyInfo.");

                    w.WriteStartDocument();
                    w.WriteStartElement("BodyInfo");

                    // BodyInfo items
                    w.WriteElementString("ItemsCount", _items.Length.ToString());
                    for (int i = 0; i < _items.Length; i++)
                    {
                        _items[i].Save(w);
                    }

                    w.WriteEndElement();
                    w.WriteEndDocument();
                }
            }
        }
    }
}