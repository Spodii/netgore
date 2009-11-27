using System.Linq;
using System.Reflection;
using DemoGame.Client;
using NetGore;
using NetGore.Collections;

namespace DemoGame.MapEditor
{
    class MapEditorDynamicEntityFactory : DynamicEntityFactoryBase
    {
        static readonly IDynamicEntityFactory _instance;

        /// <summary>
        /// Initializes the <see cref="MapEditorDynamicEntityFactory"/> class.
        /// </summary>
        static MapEditorDynamicEntityFactory()
        {
            _instance = new MapEditorDynamicEntityFactory();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapEditorDynamicEntityFactory"/> class.
        /// </summary>
        MapEditorDynamicEntityFactory()
        {
        }

        /// <summary>
        /// Gets the <see cref="DynamicEntityFactory"/> instance.
        /// </summary>
        public static IDynamicEntityFactory Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the <see cref="TypeFilterCreator"/> to use for creating the <see cref="TypeFactory"/> that loads
        /// all the items in the <see cref="DynamicEntityFactoryBase"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="TypeFilterCreator"/> to use.
        /// </returns>
        protected override TypeFilterCreator GetTypeFilterCreator()
        {
            var filter = base.GetTypeFilterCreator();
            filter.RequireConstructor = true;

            // Force Types that are NOT from the Server
            var serverAssembly = Assembly.GetAssembly(typeof(Server.Server));
            filter.CustomFilter = (x => x.Assembly != serverAssembly);

            return filter;
        }
    }
}