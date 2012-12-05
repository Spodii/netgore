using System;
using System.Linq;
using DemoGame.Editor.Properties;
using DemoGame.Server;
using NetGore.Editor.EditorTool;
using NetGore.Graphics;
using SFML.Graphics;
using NetGore;

namespace DemoGame.Editor.Tools
{
    /// <summary>
    /// A <see cref="Tool"/> that displays the NPC spawning properties.
    /// </summary>
    public class MapEditNPCSpawnsTool : Tool
    {
        MapEditNPCSpawnsToolForm _form;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapEditNPCSpawnsTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        protected MapEditNPCSpawnsTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
            ToolBarControl.ControlSettings.ToolTipText = "Edit the NPC spawns";
            ToolBarControl.ControlSettings.Click -= ControlSettings_Click;
            ToolBarControl.ControlSettings.Click += ControlSettings_Click;
        }

        /// <summary>
        /// Handles the Click event of the ControlSettings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void ControlSettings_Click(object sender, EventArgs e)
        {
            var tb = ToolBar.GetToolBar(ToolBarVisibility.Map);
            if (tb == null)
                return;

            var map = tb.DisplayObject as EditorMap;
            if (map == null)
                return;

            // Display BG properties
            if (_form == null || _form.IsDisposed)
                _form = new MapEditNPCSpawnsToolForm();

            _form.Map = map;
            _form.Show(MainForm.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Float);
        }

        /// <summary>
        /// When overridden in the derived class, handles performing drawing after the GUI for a <see cref="IDrawableMap"/> has been draw.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="map">The <see cref="IDrawableMap"/> being drawn.</param>
        protected override void HandleAfterDrawMapGUI(ISpriteBatch spriteBatch, IDrawableMap map)
        {
            base.HandleAfterDrawMapGUI(spriteBatch, map);

            if (_form == null || !_form.Visible)
                return;

            // Draw spawns
            var spawns = _form.GetMapSpawns();
            if (spawns != null)
            {
                var font = GlobalState.Instance.DefaultRenderFont;
                foreach (var spawn in spawns)
                {
                    EditorEntityDrawer.DrawSpawn(spawn, spriteBatch, map, font);
                }
            }
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Edit NPC Spawns")
            {
                ToolBarVisibility = ToolBarVisibility.Map,
                OnToolBarByDefault = true,
                ToolBarControlType = ToolBarControlType.Button,
                EnabledImage = Resources.MapEditNPCSpawnsTool,
                HelpName = "Map Edit NPC Spawns Tool",
                HelpWikiPage = "Map edit npc spawns tool",
            };
        }
    }
}