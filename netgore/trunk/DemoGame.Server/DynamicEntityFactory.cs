using System.Linq;
using NetGore;
using NetGore.Collections;
using NetGore.World;

namespace DemoGame.Server
{
    public class DynamicEntityFactory : DynamicEntityFactoryBase
    {
        static readonly IDynamicEntityFactory _instance;

        /// <summary>
        /// Initializes the <see cref="DynamicEntityFactory"/> class.
        /// </summary>
        static DynamicEntityFactory()
        {
            _instance = new DynamicEntityFactory();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicEntityFactory"/> class.
        /// </summary>
        DynamicEntityFactory()
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
            filter.RequireConstructor = false;
            return filter;
        }
    }
}