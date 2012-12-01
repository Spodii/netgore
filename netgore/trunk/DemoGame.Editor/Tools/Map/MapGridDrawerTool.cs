using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Editor.Properties;
using NetGore;
using NetGore.Editor.EditorTool;
using NetGore.Editor.WinForms;
using NetGore.Graphics;
using SFML.Graphics;

namespace DemoGame.Editor.Tools
{
    /// <summary>
    /// A <see cref="Tool"/> that displays the <see cref="ScreenGrid"/> for an <see cref="IDrawableMap"/>.
    /// </summary>
    public class MapGridDrawerTool : Tool
    {
        readonly ToolStripButton _mnuSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGridDrawerTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        protected MapGridDrawerTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
            ToolBarControl.ControlSettings.ToolTipText = "Toggles the display of the map grid";

            var s = ToolBarControl.ControlSettings.AsSplitButtonSettings();
            s.ClickToEnable = true;

            // Listen for when the settings properties change
            EditorSettings.Default.PropertyChanged -= EditorSettings_PropertyChanged;
            EditorSettings.Default.PropertyChanged += EditorSettings_PropertyChanged;

            // Add menu items
            _mnuSize = new ToolStripButton(GetMnuGridSizeText()) { ToolTipText = "Change the size of the grid" };
            _mnuSize.Click += mnuSize_Click;
            s.DropDownItems.Add(_mnuSize);
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Display Grid")
            {
                ToolBarVisibility = ToolBarVisibility.Map,
                MapDrawingExtensions = new IMapDrawingExtension[] { new MapGridDrawingExtension() },
                OnToolBarByDefault = true,
                ToolBarControlType = ToolBarControlType.SplitButton,
                DisabledImage = Resources.MapGridDrawerTool_Disabled,
                EnabledImage = Resources.MapGridDrawerTool_Enabled,
                HelpName = "Map Display Grid Tool",
                HelpWikiPage = "Map display grid tool",
            };
        }

        /// <summary>
        /// Handles the PropertyChanged event of the <see cref="EditorSettings"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        void EditorSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            const string gridSizePropName = "GridSize";

            // Update text when its the grid size that changed
            EditorSettings.Default.AssertPropertyExists(gridSizePropName);

            if (StringComparer.Ordinal.Equals(gridSizePropName, e.PropertyName))
                _mnuSize.Text = GetMnuGridSizeText();
        }

        /// <summary>
        /// Gets the text for the <see cref="_mnuSize"/> menu item.
        /// </summary>
        /// <returns>The text for the mnuSize menu item.</returns>
        static string GetMnuGridSizeText()
        {
            const string format = "Grid Size ({0}x{1})";
            var size = EditorSettings.Default.GridSize;
            return string.Format(format, size.X, size.Y);
        }

        /// <summary>
        /// Handles the Click event of the <see cref="_mnuSize"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void mnuSize_Click(object sender, EventArgs e)
        {
            // Show the form for changing the size
            const string title = "Enter new grid size";
            const string msg = "Enter the new size for the grid (ex: 32x32)";
            const string invalidInputMsg = "The string you entered was not in the correct format.";

            var size = EditorSettings.Default.GridSize;
            var result = InputBox.Show(title, msg, string.Format("{0}x{1}", size.X, size.Y));

            // Parse the returned value
            int x;
            int y;
            while (true)
            {
                // Check if aborted
                if (string.IsNullOrEmpty(result))
                    return;

                // Split by the delimiter, and try parsing the values
                var split = result.Split('x', ',', '*');
                if (split.Length != 2 || !int.TryParse(split[0], out x) || !int.TryParse(split[1], out y) || x < 1 || y < 1)
                {
                    // Failed to parse properly, so display the dialog again
                    result = InputBox.Show(title, invalidInputMsg + Environment.NewLine + msg,
                        string.Format("{0}x{1}", size.X, size.Y));
                }
                else
                {
                    // Parsed successfully
                    break;
                }
            }

            // Update settings
            EditorSettings.Default.GridSize = new Vector2(x, y);
            EditorSettings.Default.Save();
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
                _grid.Size = EditorSettings.Default.GridSize;
                _grid.Draw(spriteBatch, camera);
            }
        }
    }
}