using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.Editor.Properties;
using NetGore;
using NetGore.Editor.EditorTool;
using NetGore.Graphics;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Editor.Tools
{
    /// <summary>
    /// A <see cref="Tool"/> that displays the walls on the map.
    /// </summary>
    public class MapWallsDrawerTool : Tool
    {
        readonly ToolStripButton _ddMapGrhWalls;
        readonly ToolStripButton _ddWalls;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGridDrawerTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        protected MapWallsDrawerTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
            var s = ToolBarControl.ControlSettings.AsSplitButtonSettings();

            s.ToolTipText = "Toggles the display of the walls on the map";
            s.ClickToEnable = true;

            // Set the Parent property in the MapDrawingExtension
            var exts = ToolSettings.MapDrawingExtensions.OfType<ToolMapDrawingExtension>();
            foreach (var ext in exts)
            {
                ext.Parent = this;
            }

            // Create the drop-down items
            _ddMapGrhWalls = new ToolStripButton("MapGrh walls") { CheckOnClick = true };
            _ddWalls = new ToolStripButton("Walls") { CheckOnClick = true };

            s.DropDownItems.AddRange(new ToolStripItem[] { _ddMapGrhWalls, _ddWalls });
        }

        /// <summary>
        /// Gets or sets if the automatic walls for a <see cref="MapGrh"/> are drawn.
        /// </summary>
        [SyncValue]
        public bool DrawMapGrhWalls
        {
            get { return _ddMapGrhWalls.Checked; }
            set { _ddMapGrhWalls.Checked = value; }
        }

        /// <summary>
        /// Gets or sets if normal walls on the map are drawn.
        /// </summary>
        [SyncValue]
        public bool DrawWalls
        {
            get { return _ddWalls.Checked; }
            set { _ddWalls.Checked = value; }
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Display Walls")
            {
                ToolBarVisibility = ToolBarVisibility.Map,
                MapDrawingExtensions = new IMapDrawingExtension[] { new ToolMapDrawingExtension() },
                OnToolBarByDefault = true,
                ToolBarControlType = ToolBarControlType.SplitButton,
                EnabledImage = Resources.MapWallsDrawerTool_Enabled,
                DisabledImage = Resources.MapWallsDrawerTool_Disabled,
                HelpName = "Map Display Walls Tool",
                HelpWikiPage = "Map display walls tool",
            };
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling resetting the state of this <see cref="Tool"/>.
        /// For simplicity, all default values should be constant, no matter the current state.
        /// </summary>
        protected override void HandleResetState()
        {
            DrawWalls = true;
            DrawMapGrhWalls = true;

            base.HandleResetState();
        }

        /// <summary>
        /// The <see cref="MapDrawingExtension"/> for the <see cref="MapWallsDrawerTool"/>.
        /// </summary>
        class ToolMapDrawingExtension : MapDrawingExtension
        {
            /// <summary>
            /// Gets or sets the <see cref="MapWallsDrawerTool"/>.
            /// </summary>
            public MapWallsDrawerTool Parent { get; set; }

            /// <summary>
            /// When overridden in the derived class, handles drawing to the map after all of the map drawing finishes.
            /// </summary>
            /// <param name="map">The map the drawing is taking place on.</param>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
            /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
            protected override void HandleDrawAfterMap(IDrawableMap map, ISpriteBatch spriteBatch, ICamera2D camera)
            {
                var p = Parent;
                if (p == null)
                    return;

                // Ensure the map is valid
                var clientMap = map as EditorMap;
                if (clientMap == null)
                    return;

                var viewArea = camera.GetViewArea();

                // Draw the walls
                if (p.DrawWalls)
                {
                    var visibleWalls = map.Spatial.GetMany<WallEntityBase>(viewArea);
                    foreach (var wall in visibleWalls)
                    {
                        EntityDrawer.Draw(spriteBatch, camera, wall);
                    }
                }

                // Draw the MapGrh walls
                if (p.DrawMapGrhWalls)
                {
                    var mapGrhWalls = GlobalState.Instance.MapGrhWalls;
                    var toDraw = clientMap.Spatial.GetMany<MapGrh>(viewArea);
                    var tmpWall = new WallEntity(Vector2.Zero, Vector2.Zero);

                    foreach (var mg in toDraw)
                    {
                        var boundWalls = mapGrhWalls[mg.Grh.GrhData];
                        if (boundWalls == null)
                            continue;

                        foreach (var wall in boundWalls)
                        {
                            tmpWall.Size = wall.Size != Vector2.Zero ? wall.Size * mg.Scale : mg.Size;
                            tmpWall.Position = mg.Position + (wall.Position * mg.Scale);
                            tmpWall.IsPlatform = wall.IsPlatform;
                            EntityDrawer.Draw(spriteBatch, camera, tmpWall);
                        }
                    }
                }
            }
        }
    }
}