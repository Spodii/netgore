using System.Linq;
using DemoGame.Server.Queries;
using NetGore.Content;
using NetGore.Db;
using NetGore.Db.MySql;
using NetGore.EditorTools;
using NetGore.Graphics;
using SFML.Graphics;

namespace DemoGame.Editor
{
    /// <summary>
    /// Describes the global state for the editors. This contains state that is shared across multiple parts of the editor and
    /// can be utilized by any part of the editor. When something is specific to a single control instance, it belongs in that
    /// control instance and not here.
    /// </summary>
    public class GlobalState
    {
        static readonly GlobalState _instance;

        readonly IContentManager _contentManager;
        readonly IDbController _dbController;
        readonly Font _defaultRenderFont;
        readonly MapState _mapState;

        /// <summary>
        /// Initializes the <see cref="GlobalState"/> class.
        /// </summary>
        static GlobalState()
        {
            _instance = new GlobalState();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalState"/> class.
        /// </summary>
        GlobalState()
        {
            _mapState = new MapState();
            _contentManager = NetGore.Content.ContentManager.Create();

            var dbConnSettings = new DbConnectionSettings();
            _dbController =
                dbConnSettings.CreateDbControllerPromptEditWhenInvalid(x => new ServerDbController(x.GetMySqlConnectionString()),
                                                                       x => dbConnSettings.PromptEditFileMessageBox(x));

            _defaultRenderFont = ContentManager.LoadFont("Font/Arial", 16, ContentLevel.Global);
        }

        /// <summary>
        /// Gets the <see cref="IContentManager"/> used by all parts of the editor.
        /// </summary>
        public IContentManager ContentManager
        {
            get { return _contentManager; }
        }

        /// <summary>
        /// Gets the <see cref="IDbController"/> to use to communicate with the database.
        /// </summary>
        public IDbController DbController
        {
            get { return _dbController; }
        }

        /// <summary>
        /// Gets the default <see cref="Font"/> to use for writing to rendered screens.
        /// </summary>
        public Font DefaultRenderFont
        {
            get { return _defaultRenderFont; }
        }

        /// <summary>
        /// Gets the <see cref="GlobalState"/> instance.
        /// </summary>
        public static GlobalState Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the <see cref="MapState"/>.
        /// </summary>
        public MapState Map
        {
            get { return _mapState; }
        }

        /// <summary>
        /// Describes the current state related to editing the maps.
        /// </summary>
        public class MapState
        {
            readonly SelectedObjectsManager<object> _selectedObjsManager = new SelectedObjectsManager<object>();

            /// <summary>
            /// Initializes a new instance of the <see cref="MapState"/> class.
            /// </summary>
            internal MapState()
            {
            }

            /// <summary>
            /// Gets or sets the <see cref="Grh"/> that has been selected to be placed on the map.
            /// </summary>
            public Grh GrhToPlace { get; set; }

            /// <summary>
            /// Gets the <see cref="SelectedObjectsManager{T}"/> that contains the currently selected map objects.
            /// </summary>
            public SelectedObjectsManager<object> SelectedObjsManager
            {
                get { return _selectedObjsManager; }
            }
        }
    }
}