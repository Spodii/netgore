using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;
using log4net;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore
{
    public static class DynamicEntityFactory
    {
        /// <summary>
        /// The name given to each DynamicEntity instance when using an IValueWriter that supports nodes.
        /// </summary>
        public const string NodeName = "DynamicEntity";

        static readonly FactoryTypeCollection _typeCollection;

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static DynamicEntityFactory()
        {
            var types = FactoryTypeCollection.FindTypesThatInherit(typeof(DynamicEntity), true);
            _typeCollection = new FactoryTypeCollection(types);

            if (log.IsInfoEnabled)
            {
                foreach (Type type in types)
                {
                    log.InfoFormat("Found DynamicEntity type `{0}`.", _typeCollection[type]);
                }
            }
        }

        public static DynamicEntity Read(BitStream reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            string typeName = reader.ReadString();

            DynamicEntity dEntity = (DynamicEntity)_typeCollection.GetTypeInstance(typeName);
            dEntity.ReadAll(new BitStreamValueReader(reader));

            return dEntity;
        }

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