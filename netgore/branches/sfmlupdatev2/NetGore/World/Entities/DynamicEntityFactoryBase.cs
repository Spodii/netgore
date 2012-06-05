using System;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.World
{
    /// <summary>
    /// Factory for <see cref="DynamicEntity"/> creation and serialization.
    /// </summary>
    public abstract class DynamicEntityFactoryBase : IDynamicEntityFactory
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The key for holding the DynamicEntity type value.
        /// </summary>
        const string _typeStringKey = "DynamicEntityType";

        readonly TypeFactory _typeCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicEntityFactoryBase"/> class.
        /// </summary>
        protected DynamicEntityFactoryBase()
        {
            var filter = GetTypeFilterCreator();

            _typeCollection = new TypeFactory(filter.GetFilter(), OnLoadTypeHandler);
        }

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
        /// Handles when a new type has been loaded into the <see cref="DynamicEntityFactoryBase"/>.
        /// </summary>
        /// <param name="typeFactory">The type factory.</param>
        /// <param name="e">The <see cref="NetGore.Collections.TypeFactoryLoadedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnLoadTypeHandler(TypeFactory typeFactory, TypeFactoryLoadedEventArgs e)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("Loaded DynamicEntity `{0}` from Type `{1}`.", e.Name, e.LoadedType);
        }

        #region IDynamicEntityFactory Members

        /// <summary>
        /// Reads and constructs a <see cref="DynamicEntity"/> from a stream.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read the <see cref="DynamicEntity"/> from.</param>
        /// <param name="compact">Whether or not the <see cref="DynamicEntity"/> is to be stored in a way that is optimized
        /// for size. The compact format is not guaranteed to remain stable. Because of this, the compact format should
        /// never be used for persistent storage. It is recommended to only use the compact format in network IO.
        /// The <paramref name="compact"/> value must be the same when reading and writing. That is, you cannot write
        /// with <paramref name="compact"/> set to true, then read back with it set to false, or vise versa.</param>
        /// <returns>
        /// The <see cref="DynamicEntity"/> created from the <paramref name="reader"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public DynamicEntity Read(IValueReader reader, bool compact = false)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            var typeName = reader.ReadString(_typeStringKey);

            var dEntity = (DynamicEntity)_typeCollection.GetTypeInstance(typeName);

            dEntity.ReadAll(reader);

            return dEntity;
        }

        /// <summary>
        /// Writes a <see cref="DynamicEntity"/> to a stream.
        /// </summary>
        /// <param name="writer"><see cref="IValueWriter"/> to write the <see cref="DynamicEntity"/> to.</param>
        /// <param name="dEntity"><see cref="DynamicEntity"/> to write to the stream.</param>
        /// <param name="compact">Whether or not the <see cref="DynamicEntity"/> is to be stored in a way that is optimized
        /// for size. The compact format is not guaranteed to remain stable. Because of this, the compact format should
        /// never be used for persistent storage. It is recommended to only use the compact format in network IO.
        /// The <paramref name="compact"/> value must be the same when reading and writing. That is, you cannot write
        /// with <paramref name="compact"/> set to true, then read back with it set to false, or vise versa.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="dEntity"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="dEntity"/> is not of a valid that <see cref="Type"/>
        /// that is supported by this factory.</exception>
        public void Write(IValueWriter writer, DynamicEntity dEntity, bool compact = false)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            if (dEntity == null)
                throw new ArgumentNullException("dEntity");

            var type = dEntity.GetType();
            var typeName = _typeCollection[type];

            // FUTURE: Make use of the "compact" argument to write the typeName as an ID, not a string. Difficulty is finding a reliable way to share the IDs to use.

            if (typeName == null)
            {
                const string errmsg = "Failed to write. The specified DynamicEntity `{0}` is not of a supported type ({1}).";
                throw new ArgumentException(string.Format(errmsg, dEntity, dEntity.GetType()));
            }

            writer.Write(_typeStringKey, typeName);
            dEntity.WriteAll(writer);
        }

        #endregion
    }
}