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
using NetGore.Editor;
using NetGore.Editor.EditorTool;
using NetGore.Editor.Grhs;
using NetGore.Graphics;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Editor
{
    // TODO: !! Need to save the GlobalState values... where applicable

    /// <summary>
    /// Describes the global state for the editors. This contains state that is shared across multiple parts of the editor and
    /// can be utilized by any part of the editor. When something is specific to a single control instance, it belongs in that
    /// control instance and not here.
    /// </summary>
    public class GlobalState
    {
        /// <summary>
        /// Delegate for handling an event for when a property changes in the <see cref="GlobalState"/>.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="sender">The <see cref="GlobalState"/> the event took place in.</param>
        /// <param name="oldValue">The old property value.</param>
        /// <param name="newValue">The new property value.</param>
        public delegate void PropertyChangedEventHandler<T>(GlobalState sender, T oldValue, T newValue);

        /// <summary>
        /// Delegate for handling the <see cref="GlobalState.Tick"/> event.
        /// </summary>
        /// <param name="currentTime">The current <see cref="TickCount"/>.</param>
        public delegate void TickEventHandler(TickCount currentTime);

        static readonly GlobalState _instance;

        readonly IContentManager _contentManager;
        readonly IDbController _dbController;
        readonly Font _defaultRenderFont;
        readonly MapGrhWalls _mapGrhWalls;
        readonly MapState _mapState;
        readonly Timer _timer;

        /// <summary>
        /// Initializes the <see cref="GlobalState"/> class.
        /// </summary>
        static GlobalState()
        {
            Input.Initialize();

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
            _mapState = new MapState(this);

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
        /// Gets the <see cref="IDynamicEntityFactory"/> instance to use.
        /// </summary>
        public IDynamicEntityFactory DynamicEntityFactory
        {
            get { return EditorDynamicEntityFactory.Instance; }
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
        /// Gets the <see cref="MapGrhWalls"/> instance.
        /// </summary>
        public MapGrhWalls MapGrhWalls
        {
            get { return _mapGrhWalls; }
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
            /// <summary>
            /// Delegate for handling an event for when a property changes in the <see cref="MapState"/>.
            /// </summary>
            /// <typeparam name="T">The type of the property.</typeparam>
            /// <param name="sender">The <see cref="MapState"/> the event took place in.</param>
            /// <param name="oldValue">The old property value.</param>
            /// <param name="newValue">The new property value.</param>
            public delegate void PropertyChangedEventHandler<T>(MapState sender, T oldValue, T newValue);

            readonly Grh _grhToPlace = new Grh();
            readonly GlobalState _parent;
            readonly SelectedObjectsManager<object> _selectedObjsManager = new SelectedObjectsManager<object>();

            Vector2 _gridSize = new Vector2(32);
            bool _mapGrhDefaultBackground = true;
            int _mapGrhDefaultDepth = 0;

            /// <summary>
            /// Initializes a new instance of the <see cref="MapState"/> class.
            /// </summary>
            /// <param name="parent">The <see cref="GlobalState"/>.</param>
            internal MapState(GlobalState parent)
            {
                _parent = parent;
            }

            /// <summary>
            /// Notifies listeners when the <see cref="MapState.MapGrhDefaultBackground"/> property has changed.
            /// </summary>
            public event PropertyChangedEventHandler<bool> MapGrhDefaultBackgroundChanged;

            /// <summary>
            /// Notifies listeners when the <see cref="MapState.MapGrhDefaultDepth"/> property has changed.
            /// </summary>
            public event PropertyChangedEventHandler<int> MapGrhDefaultDepthChanged;

            /// <summary>
            /// Gets the <see cref="Grh"/> that has been selected to be placed on the map. When placing the <see cref="Grh"/>,
            /// create a deep copy.
            /// This property will never be null, but the <see cref="GrhData"/> can be unset.
            /// </summary>
            public Grh GrhToPlace
            {
                get { return _grhToPlace; }
            }

            /// <summary>
            /// Gets or sets the size of the map grid in pixels. Must be greater than or equal to 1.
            /// </summary>
            /// <exception cref="ArgumentOutOfRangeException">The <paramref name="value"/>'s X or Y field are less than one.</exception>
            public Vector2 GridSize
            {
                get { return _gridSize; }
                set
                {
                    if (_gridSize.X <= float.Epsilon || _gridSize.Y <= float.Epsilon)
                        throw new ArgumentOutOfRangeException("value",
                                                              "The X and Y fields of the value must be greater than or equal to 1.");

                    _gridSize = value;
                }
            }

            /// <summary>
            /// Gets or sets if <see cref="MapGrh"/>s are placed on the background by default. If false, they are placed
            /// on the foreground by default.
            /// </summary>
            public bool MapGrhDefaultBackground
            {
                get { return _mapGrhDefaultBackground; }
                set
                {
                    if (_mapGrhDefaultBackground == value)
                        return;

                    var oldValue = _mapGrhDefaultBackground;
                    _mapGrhDefaultBackground = value;

                    if (MapGrhDefaultBackgroundChanged != null)
                        MapGrhDefaultBackgroundChanged(this, oldValue, value);
                }
            }

            /// <summary>
            /// Gets or sets the default depth for a <see cref="MapGrh"/>.
            /// </summary>
            public int MapGrhDefaultDepth
            {
                get { return _mapGrhDefaultDepth; }
                set
                {
                    if (value < short.MinValue)
                        value = short.MinValue;

                    if (value > short.MaxValue)
                        value = short.MaxValue;

                    if (_mapGrhDefaultDepth == value)
                        return;

                    var oldValue = _mapGrhDefaultDepth;
                    _mapGrhDefaultDepth = value;

                    if (MapGrhDefaultDepthChanged != null)
                        MapGrhDefaultDepthChanged(this, oldValue, value);
                }
            }

            /// <summary>
            /// Gets the parent <see cref="GlobalState"/>.
            /// </summary>
            public GlobalState Parent
            {
                get { return _parent; }
            }

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