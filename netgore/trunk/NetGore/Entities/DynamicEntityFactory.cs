using System;
using System.Linq;
using System.Reflection;
using System.Xml;
using log4net;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Factory for DynamicEntity creation and serialization.
    /// </summary>
    public static class DynamicEntityFactory
    {
        /// <summary>
        /// The name given to each DynamicEntity instance when using an IValueWriter that supports nodes.
        /// </summary>
        public const string NodeName = "DynamicEntity";

        static readonly FactoryTypeCollection _typeCollection;
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Static DynamicEntityFactory constructor.
        /// </summary>
        static DynamicEntityFactory()
        {
            // NOTE: It would be nice if the constructors were required on the Client, but still not the Server (since the server doesn't need to construct all DynamicEntities)
            var filter = FactoryTypeCollection.CreateFilter(typeof(DynamicEntity));
            _typeCollection = new FactoryTypeCollection(filter, OnLoadTypeHandler, false);
            _typeCollection.BeginLoading();
        }

        /// <summary>
        /// Handles when a new type has been loaded into a FactoryTypeCollection.
        /// </summary>
        /// <param name="factoryTypeCollection">FactoryTypeCollection that the event occured on.</param>
        /// <param name="loadedType">Type that was loaded.</param>
        /// <param name="name">Name of the Type.</param>
        static void OnLoadTypeHandler(FactoryTypeCollection factoryTypeCollection, Type loadedType, string name)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat("Loaded DynamicEntity `{0}` from Type `{1}`.", name, loadedType);
        }

        /// <summary>
        /// Reads and constructs a DynamicEntity from a stream.
        /// </summary>
        /// <param name="reader">BitStream to read the DynamicEntity from.</param>
        /// <returns>The DynamicEntity created from the <paramref name="reader"/>.</returns>
        public static DynamicEntity Read(BitStream reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            string typeName = reader.ReadString();

            DynamicEntity dEntity = (DynamicEntity)_typeCollection.GetTypeInstance(typeName);
            dEntity.ReadAll(new BitStreamValueReader(reader));

            return dEntity;
        }

        /// <summary>
        /// Reads and constructs a DynamicEntity from a stream.
        /// </summary>
        /// <param name="reader">XmlReader to read the DynamicEntity from.</param>
        /// <returns>The DynamicEntity created from the <paramref name="reader"/>.</returns>
        public static DynamicEntity Read(XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            string nodeName = reader.Name;
            if (nodeName != NodeName)
                throw new ArgumentException("XmlReader was not at a valid location", "reader");

            reader.MoveToAttribute("Type");
            string typeName = reader.ReadContentAsString();

            DynamicEntity dEntity = (DynamicEntity)_typeCollection.GetTypeInstance(typeName);
            dEntity.ReadAll(new XmlValueReader(reader, NodeName));
            return dEntity;
        }

        /// <summary>
        /// Writes a DynamicEntity to a stream.
        /// </summary>
        /// <param name="writer">BitStream to write the DynamicEntity to.</param>
        /// <param name="dEntity">DynamicEntity to write to the stream.</param>
        public static void Write(BitStream writer, DynamicEntity dEntity)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            if (dEntity == null)
                throw new ArgumentNullException("dEntity");

            Type type = dEntity.GetType();
            string typeName = _typeCollection[type];

            using (BitStreamValueWriter valueWriter = new BitStreamValueWriter(writer))
            {
                valueWriter.Write(null, typeName);
                dEntity.WriteAll(valueWriter);
            }
        }

        /// <summary>
        /// Writes a DynamicEntity to a stream.
        /// </summary>
        /// <param name="writer">XmlWriter to write the DynamicEntity to.</param>
        /// <param name="dEntity">DynamicEntity to write to the stream.</param>
        public static void Write(XmlWriter writer, DynamicEntity dEntity)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            if (dEntity == null)
                throw new ArgumentNullException("dEntity");

            Type type = dEntity.GetType();
            string typeName = _typeCollection[type];

            using (XmlValueWriter valueWriter = new XmlValueWriter(writer, NodeName, "Type", typeName))
            {
                dEntity.WriteAll(valueWriter);
            }
        }
    }
}