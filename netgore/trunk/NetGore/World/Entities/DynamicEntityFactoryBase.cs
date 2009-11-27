using System;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Factory for <see cref="DynamicEntity"/> creation and serialization.
    /// </summary>
    public abstract class DynamicEntityFactoryBase : IDynamicEntityFactory
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public const string TypeNameStringKey = "DynamicEntityType";
       
        readonly TypeFactory _typeCollection;

        /// <summary>
        /// Gets the <see cref="TypeFilterCreator"/> to use for creating the <see cref="TypeFactory"/> that loads
        /// all the items in the <see cref="DynamicEntityFactoryBase"/>.
        /// </summary>
        /// <returns>The <see cref="TypeFilterCreator"/> to use.</returns>
        protected virtual TypeFilterCreator GetTypeFilterCreator()
        {
            return new TypeFilterCreator { IsClass = true, IsAbstract = false, Subclass = typeof(DynamicEntity) };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicEntityFactoryBase"/> class.
        /// </summary>
        protected DynamicEntityFactoryBase()
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            var filter = GetTypeFilterCreator();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor

            _typeCollection = new TypeFactory(filter.GetFilter(), OnLoadTypeHandler, false);
        }

        /// <summary>
        /// Handles when a new type has been loaded into the <see cref="DynamicEntityFactoryBase"/>.
        /// </summary>
        /// <param name="typeFactory"><see cref="TypeFactory"/> that the event occured on.</param>
        /// <param name="loadedType"><see cref="Type"/> that was loaded.</param>
        /// <param name="name">Name of the Type.</param>
        protected virtual void OnLoadTypeHandler(TypeFactory typeFactory, Type loadedType, string name)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("Loaded DynamicEntity `{0}` from Type `{1}`.", name, loadedType);
        }

        /// <summary>
        /// Reads and constructs a <see cref="DynamicEntity"/> from a stream.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read the <see cref="DynamicEntity"/> from.</param>
        /// <returns>The <see cref="DynamicEntity"/> created from the <paramref name="reader"/>.</returns>
        public DynamicEntity Read(IValueReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            string typeName = reader.ReadString(TypeNameStringKey);

            DynamicEntity dEntity = (DynamicEntity)_typeCollection.GetTypeInstance(typeName);

            dEntity.ReadAll(reader);

            return dEntity;
        }

        /// <summary>
        /// Writes a <see cref="DynamicEntity"/> to a stream.
        /// </summary>
        /// <param name="writer"><see cref="IValueWriter"/> to write the <see cref="DynamicEntity"/> to.</param>
        /// <param name="dEntity"><see cref="DynamicEntity"/> to write to the stream.</param>
        public void Write(IValueWriter writer, DynamicEntity dEntity)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            if (dEntity == null)
                throw new ArgumentNullException("dEntity");

            Type type = dEntity.GetType();
            string typeName = _typeCollection[type];

            writer.Write(TypeNameStringKey, typeName);
            dEntity.WriteAll(writer);
        }
    }
}