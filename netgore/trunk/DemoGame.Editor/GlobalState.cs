using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
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
        readonly MapGrhWalls _mapGrhWalls;
        readonly MapState _mapState;
        readonly Timer _timer;
        readonly string[] _hotkeyedGrhs = new string[10];
        readonly Dictionary<GrhIndex, GrhData.FileTags> _fileTags = new Dictionary<GrhIndex, GrhData.FileTags>();

        public string[] HotkeyedGrhs { get { return _hotkeyedGrhs; } }

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
            _dbController = dbConnSettings.CreateDbControllerPromptEditWhenInvalid(x => 
                new ServerDbController(x.GetMySqlConnectionString()), dbConnSettings.PromptEditFileMessageBox);

            _defaultRenderFont = ContentManager.LoadFont("Font/Arial", 16, ContentLevel.Global);

            Character.NameFont = DefaultRenderFont;

            GrhInfo.Load(ContentPaths.Dev, ContentManager);

            _mapGrhWalls = new MapGrhWalls();

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
        public event EventHandler<EventArgs<TickCount>> Tick;

        /// <summary>
        /// Gets the dictionary containing the GrhData.FileTags for the associated GrhIndexes.
        /// </summary>
        public IDictionary<GrhIndex, GrhData.FileTags> FileTags { get { return _fileTags; } }

        /// <summary>
        /// Gets the GrhData.FileTags for the given GrhData.
        /// </summary>
        public GrhData.FileTags GetFileTags(GrhData gd)
        {
            return GetFileTags(gd.GrhIndex);
        }

        /// <summary>
        /// Gets the GrhData.FileTags for the given GrhData.
        /// </summary>
        public GrhData.FileTags GetFileTags(GrhIndex grhIndex)
        {
            GrhData.FileTags ret;
            if (!_fileTags.TryGetValue(grhIndex, out ret))
                return null;
            return ret;
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
        public static void Initialize()
        {
            // Calling this will invoke the static constructor, creating the instance, and ultimately setting everything up
        }

        /// <summary>
        /// Cleans up the global state.
        /// </summary>
        public static void Destroy()
        {
            try
            {
                Instance.Dispose();
            }
            catch
            {
            }
        }

        void Dispose()
        {
            try
            {
                if (_dbController != null)
                    _dbController.Dispose();
            }
            catch
            {
            }

            try
            {
                if (_contentManager != null)
                    _contentManager.Dispose();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Sets the GrhToPlace from the hotkey.
        /// </summary>
        /// <param name="hotkeyIndex">The hotkey index.</param>
        public void SetGrhFromHotkey(int hotkeyIndex)
        {
            if (hotkeyIndex >= HotkeyedGrhs.Length || hotkeyIndex < 0)
                return;

            string categorization = HotkeyedGrhs[hotkeyIndex];
            if (string.IsNullOrEmpty(categorization))
                return;

            GrhData grhData = null;
            try
            {
                SpriteCategorization cat = new SpriteCategorization(categorization);
                grhData = GrhInfo.GetData(cat);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
            }

            if (grhData == null)
            {
                // No grh for categorization, so unset it
                HotkeyedGrhs[hotkeyIndex] = null;
                return;
            }

            Map.SetGrhToPlace(grhData.GrhIndex);
        }


        /// <summary>
        /// Finds the next free <see cref="SoundID"/>.
        /// </summary>
        /// <param name="usedIDs">Collection of <see cref="SoundID"/>s already assigned.</param>
        /// <param name="start">The <see cref="SoundID"/> to start at.</param>
        /// <returns>The next free <see cref="SoundID"/>. The returned value will be marked as used in the
        /// <paramref name="usedIDs"/>.</returns>
        static SoundID NextFreeID(HashSet<SoundID> usedIDs, SoundID start)
        {
            while (!usedIDs.Add(start))
            {
                start++;
            }

            return start;
        }


        public void AutoUpdateSounds()
        {
            const string title = "Update sound";
            const string confirmMsg = "The following changes are to be made (+ for add, - for remove):";
            const string upToDateMsg = "The sound is already up-to-date.";
            const string acceptChangesMsg = "Accept these {0} changes and update the sound?";
            const string doneMsg = "Sound successfully updated!";

            var cm = NetGore.Content.ContentManager.Create();
            var sm = AudioManager.GetInstance(cm).SoundManager;

            // Find all the sound files
            var files = Directory.GetFiles(ContentPaths.Build.Sounds);

            // Find the new files (file exists, but SoundInfo does not)
            var newFiles =
                files.Where(f => !sm.SoundInfos.Any(si => StringComparer.OrdinalIgnoreCase.Equals(si.Name, Path.GetFileName(f)))).
                    ToArray();

            // Find the removed files (SoundInfo exists, but file does not)
            var removedFiles =
                sm.SoundInfos.Where(si => !files.Any(f => StringComparer.OrdinalIgnoreCase.Equals(si.Name, Path.GetFileName(f)))).
                    ToArray();

            // Check if there are any changes
            if (newFiles.Length <= 0 && removedFiles.Length <= 0)
            {
                return;
            }

            // Display list of changes

            var sb = new StringBuilder();
            sb.AppendLine(confirmMsg);

            const int maxLines = 25;
            var lines = 0;

            foreach (var f in removedFiles)
            {
                sb.AppendLine(" - " + Path.GetFileName(f.Name) + " [" + f.ID + "]");
                if (++lines > maxLines)
                    break;
            }

            foreach (var f in newFiles)
            {
                sb.AppendLine(" + " + Path.GetFileName(f));
                if (++lines > maxLines)
                    break;
            }

            sb.AppendLine();
            sb.AppendLine(string.Format(acceptChangesMsg, newFiles.Length + removedFiles.Length));

            if (MessageBox.Show(sb.ToString(), title, MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // Update by taking the existing SoundInfos, removing the ones to remove, adding the new ones

            var sis = sm.SoundInfos.ToList();

            foreach (var toRemove in removedFiles)
            {
                sis.Remove(toRemove);
            }

            var usedIDs = new HashSet<SoundID>();
            foreach (var si in sis)
            {
                usedIDs.Add(si.ID);
            }

            var soundIDCounter = new SoundID(1);

            foreach (var toAdd in newFiles)
            {
                var name = Path.GetFileName(toAdd);
                var id = NextFreeID(usedIDs, soundIDCounter);
                sis.Add(new SoundInfo(name, id));
            }

            // Save it all
            sm.ReloadData(sis);
            sm.Save();


            MessageBox.Show(doneMsg, title, MessageBoxButtons.OK);
        }

        public void AutoUpdateMusic()
        {


            const string title = "Update music";
            const string confirmMsg = "The following changes are to be made (+ for add, - for remove):";
            const string upToDateMsg = "The music is already up-to-date.";
            const string acceptChangesMsg = "Accept these {0} changes and update the music?";
            const string doneMsg = "Music successfully updated!";

            var cm = NetGore.Content.ContentManager.Create();
            var mm = AudioManager.GetInstance(cm).MusicManager;

            // Find all the music files
            var files = Directory.GetFiles(ContentPaths.Build.Music);

            // Find the new files (file exists, but MusicInfo does not)
            var newFiles =
                files.Where(f => !mm.MusicInfos.Any(mi => StringComparer.OrdinalIgnoreCase.Equals(mi.Name, Path.GetFileName(f)))).
                    ToArray();

            // Find the removed files (MusicInfo exists, but file does not)
            var removedFiles =
                mm.MusicInfos.Where(mi => !files.Any(f => StringComparer.OrdinalIgnoreCase.Equals(mi.Name, Path.GetFileName(f)))).
                    ToArray();

            // Check if there are any changes
            if (newFiles.Length <= 0 && removedFiles.Length <= 0)
            {
                return;
            }

            // Display list of changes

            var sb = new StringBuilder();
            sb.AppendLine(confirmMsg);

            const int maxLines = 25;
            var lines = 0;

            foreach (var f in removedFiles)
            {
                sb.AppendLine(" - " + Path.GetFileName(f.Name) + " [" + f.ID + "]");
                if (++lines > maxLines)
                    break;
            }

            foreach (var f in newFiles)
            {
                sb.AppendLine(" + " + Path.GetFileName(f));
                if (++lines > maxLines)
                    break;
            }

            sb.AppendLine();
            sb.AppendLine(string.Format(acceptChangesMsg, newFiles.Length + removedFiles.Length));

            if (MessageBox.Show(sb.ToString(), title, MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // Update by taking the existing MusicInfos, removing the ones to remove, adding the new ones

            var mis = mm.MusicInfos.ToList();

            foreach (var toRemove in removedFiles)
            {
                mis.Remove(toRemove);
            }

            var usedIDs = new HashSet<MusicID>();
            foreach (var mi in mis)
            {
                usedIDs.Add(mi.ID);
            }

            var musicIDCounter = new MusicID(1);

            foreach (var toAdd in newFiles)
            {
                var name = Path.GetFileName(toAdd);
                var id = NextFreeID(usedIDs, musicIDCounter);
                mis.Add(new MusicInfo(name, id));
            }

            // Save it all
            mm.ReloadData(mis);
            mm.Save();


            MessageBox.Show(doneMsg, title, MessageBoxButtons.OK);
        }

        /// <summary>
        /// Finds the next free <see cref="MusicID"/>.
        /// </summary>
        /// <param name="usedIDs">Collection of <see cref="MusicID"/>s already assigned.</param>
        /// <param name="start">The <see cref="MusicID"/> to start at.</param>
        /// <returns>The next free <see cref="MusicID"/>. The returned value will be marked as used in the
        /// <paramref name="usedIDs"/>.</returns>
        static MusicID NextFreeID(HashSet<MusicID> usedIDs, MusicID start)
        {
            while (!usedIDs.Add(start))
            {
                start++;
            }

            return start;
        }

        public void AutoUpdateGrhDatas()
        {
            GrhData[] deleted;
            GrhData[] added;
            Dictionary<GrhData, GrhData.FileTags> grhDataFileTags;
            
            AutomaticGrhDataUpdater.Update(ContentManager, ContentPaths.Dev.Grhs, out added, out deleted, out grhDataFileTags);
            if (deleted.Length > 0 || added.Length > 0)
            {
                string msg = string.Format("GrhDatas updated: {0} added, {1} deleted", added.Length, deleted.Length);

                if (deleted.Length > 0)
                {
                    var removedFromMaps = MapHelper.RemoveInvalidGrhDatasFromMaps();
                    if (removedFromMaps.Any(x => x.Value > 0))
                    {
                        msg += Environment.NewLine + Environment.NewLine;
                        msg += "The following maps were altered to remove the deleted GrhIndexes:";
                        foreach (var mapInfo in removedFromMaps.Where(x => x.Value > 0))
                        {
                            msg += Environment.NewLine + string.Format("{0}: {1} MapGrhs", mapInfo.Key, mapInfo.Value);
                        }
                    }
                }
                MessageBox.Show(msg);
            }

            _fileTags.Clear();
            foreach (var kvp in grhDataFileTags)
                _fileTags.Add(kvp.Key.GrhIndex, kvp.Value);

            // Update the bound walls
            RebuildMapGrhWalls(grhDataFileTags);
        }

        void RebuildMapGrhWalls(IEnumerable<KeyValuePair<GrhData, GrhData.FileTags>> grhDataFileTags)
        {
            MapGrhWalls.Clear();

            foreach (var kvp in grhDataFileTags)
            {
                if (kvp.Value.Walls == null || kvp.Value.Walls.Count == 0)
                    continue;

                List<WallEntityBase> wallList = new List<WallEntityBase>();
                GrhData gd = kvp.Key;

                // Create the wall entities
                foreach (var wallInfo in kvp.Value.Walls)
                {
                    GrhData.BoundWallType wallType = wallInfo.Item1;
                    Rectangle? area = wallInfo.Item2;

                    Vector2 position;
                    Vector2 size;

                    if (area.HasValue)
                    {
                        position = new Vector2(area.Value.X, area.Value.Y);
                        size = new Vector2(area.Value.Width, area.Value.Height);
                    }
                    else
                    {
                        position = Vector2.Zero;
                        size = Vector2.Zero;
                    }

                    if (wallType == GrhData.BoundWallType.Solid || wallType == GrhData.BoundWallType.Platform)
                    {
                        WallEntity wall = new WallEntity(position, size) { IsPlatform = wallType == GrhData.BoundWallType.Platform };
                        wallList.Add(wall);
                    }
                }

                // If there are any walls, trim the list and set it
                if (wallList.Count > 0)
                {
                    wallList.TrimExcess();
                    MapGrhWalls[gd] = wallList;
                }
            }
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
                Tick.Raise(this, EventArgsHelper.Create(now));
        }

        /// <summary>
        /// Describes the current state related to editing the maps.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public class MapState
        {
            readonly Grh _grhToPlace = new Grh();
            readonly GlobalState _parent;
            readonly SelectedObjectsManager<object> _selectedObjsManager = new SelectedObjectsManager<object>();
            readonly HashSet<Func<IDrawable, bool>> _mapDrawFilters = new HashSet<Func<IDrawable, bool>>();
            readonly Func<IDrawable, bool> _mapDrawFilter;

            MapRenderLayer _layer = MapRenderLayer.SpriteBackground;
            int _layerDepth = 0;

            /// <summary>
            /// Notifies listeners when the LayerDepth property changes.
            /// </summary>
            public event EventHandler LayerDepthChanged;

            /// <summary>
            /// Notifies listeners when the Layer property changes.
            /// </summary>
            public event EventHandler LayerChanged;

            /// <summary>
            /// Notifies listeners when the GrhToPlace has been changed to a different Grh.
            /// </summary>
            public event EventHandler GrhToPlaceChanged;

            public HashSet<Func<IDrawable, bool>> MapDrawFilters { get { return _mapDrawFilters; } }

            bool MapDrawFilterInternal(IDrawable drawable)
            {
                return MapDrawFilters.All(x => x(drawable));
            }

            public Func<IDrawable, bool> MapDrawFilter { get { return _mapDrawFilter; } }

            /// <summary>
            /// Initializes a new instance of the <see cref="MapState"/> class.
            /// </summary>
            /// <param name="parent">The <see cref="GlobalState"/>.</param>
            internal MapState(GlobalState parent)
            {
                _parent = parent;
                _mapDrawFilter = MapDrawFilterInternal;
            }

            /// <summary>
            /// Changes the GrhToPlace and raises the GrhToPlaceChanged event.
            /// </summary>
            /// <param name="grhIndex"></param>
            public void SetGrhToPlace(GrhIndex grhIndex)
            {
                if (GrhToPlace.GrhData != null && GrhToPlace.GrhData.GrhIndex == grhIndex)
                    return;

                LayerDepth = 0;

                GrhToPlace.SetGrh(grhIndex);

                GrhData.FileTags fileTags = Parent.GetFileTags(grhIndex);
                if (fileTags != null && fileTags.Layer.HasValue)
                    Layer = fileTags.Layer.Value;

                if (GrhToPlaceChanged != null)
                    GrhToPlaceChanged.Raise(this, EventArgs.Empty);
            }

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

            /// <summary>
            /// Gets or sets the current map layer (used by cursors).
            /// </summary>
            public MapRenderLayer Layer
            {
                get { return _layer; }
                set
                {
                    if (_layer == value)
                        return;

                    _layer = value;
                    var e = LayerChanged;
                    if (e != null)
                        e(this, EventArgs.Empty);
                }
            }

            /// <summary>
            /// Gets or sets the layer depth to place new grhs at.
            /// </summary>
            public int LayerDepth
            {
                get { return _layerDepth; }
                set
                {
                    if (_layerDepth == value)
                        return;

                    _layerDepth = value;
                    var e = LayerDepthChanged;
                    if (e != null)
                        e(this, EventArgs.Empty);
                }
            }
        }
    }
}