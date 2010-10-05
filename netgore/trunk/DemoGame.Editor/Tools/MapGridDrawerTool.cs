﻿using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.Editor.Properties;
using NetGore.Editor;
using NetGore.Editor.EditorTool;
using NetGore.Graphics;

namespace DemoGame.Editor
{
    /// <summary>
    /// A <see cref="Tool"/> that displays the <see cref="ScreenGrid"/> for an <see cref="IDrawableMap"/>.
    /// </summary>
    public class MapGridDrawerTool : Tool
    {
        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Grid Drawer")
            {
                ToolBarVisibility = ToolBarVisibility.Map,
                MapDrawingExtensions = new IMapDrawingExtension[] { new MapGridDrawingExtension() },
                OnToolBarByDefault = true,
                ToolBarControlType = ToolBarControlType.Button,
                DisabledImage = Resources.MapGridDrawerTool_Disabled,
                EnabledImage = Resources.MapGridDrawerTool_Enabled,
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGridDrawerTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        protected MapGridDrawerTool(ToolManager toolManager)
            : base(toolManager, CreateSettings())
        {
            ToolBarControl.ControlSettings.ToolTipText = "Toggles the display of the map grid";
            ToolBarControl.ControlSettings.AsButtonSettings().ClickToEnable = true;
        }

        class MapGridDrawingExtension : MapDrawingExtension
        {
            readonly ScreenGrid _grid = new ScreenGrid();

            /// <summary>
            /// When overridden in the derived class, handles drawing to the map after all of the map drawing finishes.
            /// </summary>
            /// <param name="map">The map the drawing is taking place on.</param>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
            /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
            protected override void HandleDrawAfterMap(IDrawableMap map, ISpriteBatch spriteBatch, ICamera2D camera)
            {
                _grid.Draw(spriteBatch, camera);
            }
        }
    }
}