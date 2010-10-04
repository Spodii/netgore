using System;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.Server.Queries;
using NetGore;
using NetGore.Audio;
using NetGore.Content;
using NetGore.Db;
using NetGore.Db.MySql;
using NetGore.EditorTools;
using NetGore.EditorTools.EditorTool;
using NetGore.EditorTools.Grhs;
using NetGore.Graphics;
using NetGore.IO;
using SFML.Graphics;
using CustomUITypeEditors = DemoGame.EditorTools.CustomUITypeEditors;

namespace DemoGame.Editor
{
    /// <summary>
    /// Describes the global state for the editors. This contains state that is shared across multiple parts of the editor and
    /// can be utilized by any part of the editor. When something is specific to a single control instance, it belongs in that
    /// control instance and not here.
    /// </summary>
    public class GlobalState
    {
        /// <summary>
        /// Delegate for handling the <see cref="GlobalState.Tick"/> event.
        /// </summary>
        /// <param name="currentTime">The current <see cref="TickCount"/>.</param>
        public delegate void TickEventHandler(TickCount currentTime);

        static readonly GlobalState _instance;

        readonly IContentManager _contentManager;
        readonly IDbController _dbController;
        readonly Font _defaultRenderFont;
        readonly MapState _mapState;
        readonly Timer _timer;
        readonly MapGrhWalls _mapGrhWalls;

        /// <summary>
        /// Gets the <see cref="MapGrhWalls"/> instance.
        /// </summary>
        public MapGrhWalls MapGrhWalls { get { return _mapGrhWalls; } }

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
            ThreadAsserts.IsMainThread();

            // Load all sorts of stuff
            _contentManager = NetGore.Content.ContentManager.Create();

            var dbConnSettings = new DbConnectionSettings();
            _dbController =
                dbConnSettings.CreateDbControllerPromptEditWhenInvalid(x => new ServerDbController(x.GetMySqlConnectionString()),
                                                                       x => dbConnSettings.PromptEditFileMessageBox(x));

            _defaultRenderFont = ContentManager.LoadFont("Font/Arial", 16, ContentLevel.Global);

            Character.NameFont = DefaultRenderFont;

            GrhInfo.Load(ContentPaths.Dev, ContentManager);
            AutomaticGrhDataSizeUpdater.Instance.UpdateSizes();

            _mapGrhWalls = new MapGrhWalls(ContentPaths.Dev, x => new WallEntity(x));

            // Load the child classes
            _mapState = new MapState();

            // Grab the audio manager instances, which will ensure that they are property initialized
            // before something that can't pass it an ContentManager tries to get an instance
            AudioManager.GetInstance(ContentManager);

            // Set the custom UITypeEditors
            CustomUITypeEditors.AddEditors(DbController);

            // Set up the timer
            _timer = new Timer { Interval = 1000 / 60 };
            _timer.Tick += _timer_Tick;
        }

        /// <summary>
        /// An event that is raised once every time updates and draws should take place.
        /// </summary>
        public event TickEventHandler Tick;

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
        /// Gets or sets if the <see cref="GlobalState.Tick"/> event will be trigger.
        /// </summary>
        public bool IsTickEnabled
        {
            get { return _timer.Enabled; }
            set { _timer.Enabled = value; }
        }

        /// <summary>
        /// Gets the <see cref="MapState"/>.
        /// </summary>
        public MapState Map
        {
            get { return _mapState; }
        }

        /// <summary>
        /// Ensures the <see cref="GlobalState"/> is initailized.
        /// </summary>
        public static void Initailize()
        {
            // Calling this will invoke the static constructor, creating the instance, and ultimately setting everything up
        }

        /// <summary>
        /// Handles the Tick event of the _timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void _timer_Tick(object sender, EventArgs e)
        {
            ThreadAsserts.IsMainThread();

            var now = TickCount.Now;

            // Some manual update calls
            if (ToolManager.Instance != null)
                ToolManager.Instance.Update(now);

            // Raise event
            if (Tick != null)
                Tick(now);
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