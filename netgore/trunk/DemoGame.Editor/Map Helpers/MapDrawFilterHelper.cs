using System;
using System.ComponentModel;
using System.Linq;
using DemoGame.Client;
using DemoGame.Editor.Properties;
using NetGore;
using NetGore.Graphics;
using NetGore.IO;

namespace DemoGame.Editor
{
    /// <summary>
    /// Provides assistance in creating a draw filter for the <see cref="Map"/>.
    /// </summary>
    public class MapDrawFilterHelper : IPersistable
    {
        const bool _defaultDrawBackground = true;
        const bool _defaultDrawBackgroundMapGrhs = true;
        const bool _defaultDrawCharacters = true;
        const bool _defaultDrawForegroundMapGrhs = true;
        const bool _defaultDrawItems = true;
        const bool _defaultMapGrhsOnDefaultDepthOnly = false;
        const bool _defaultMapGrhsOnDefaultLayerOnly = false;
        const string _generalCategoryName = "Draw Filters";
        const string _mapGrhCategoryName = "Draw Filters - MapGrh";

        static readonly int? _defaultMapGrhMaxDepth = null;
        static readonly int? _defaultMapGrhMinDepth = null;

        readonly Func<IDrawable, bool> _filterFunc;

        short _mapGrh_DefaultDepth_Cache;
        bool _mapGrh_DefaultIsForeground_Cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapDrawFilterHelper"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the state values from.</param>
        public MapDrawFilterHelper(IValueReader reader)
        {
            _filterFunc = Filter;

            ReadState(reader);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapDrawFilterHelper"/> class.
        /// </summary>
        public MapDrawFilterHelper()
        {
            _filterFunc = Filter;

            DrawBackground = _defaultDrawBackground;
            DrawCharacters = _defaultDrawCharacters;
            DrawItems = _defaultDrawItems;
            DrawForegroundMapGrhs = _defaultDrawForegroundMapGrhs;
            DrawBackgroundMapGrhs = _defaultDrawBackgroundMapGrhs;

            MapGrhMinDepth = _defaultMapGrhMinDepth;
            MapGrhMaxDepth = _defaultMapGrhMaxDepth;
            MapGrhsOnDefaultLayerOnly = _defaultMapGrhsOnDefaultLayerOnly;
            MapGrhsOnDefaultDepthOnly = _defaultMapGrhsOnDefaultDepthOnly;

            _mapGrh_DefaultDepth_Cache = EditorSettings.Default.MapGrh_DefaultDepth;
            _mapGrh_DefaultIsForeground_Cache = EditorSettings.Default.MapGrh_DefaultIsForeground;
            EditorSettings.Default.PropertyChanged += EditorSettings_PropertyChanged;
        }

        /// <summary>
        /// Gets or sets if the background is drawn.
        /// The default value is true.
        /// </summary>
        [SyncValue]
        [Category(_generalCategoryName)]
        [DefaultValue(_defaultDrawBackground)]
        [Browsable(true)]
        [Description("If the map background is drawn.")]
        public bool DrawBackground { get; set; }

        /// <summary>
        /// Gets or sets if the background <see cref="MapGrh"/>s are drawn.
        /// The default value is true.
        /// </summary>
        [SyncValue]
        [Category(_mapGrhCategoryName)]
        [DefaultValue(_defaultDrawBackgroundMapGrhs)]
        [Browsable(true)]
        [Description("If the background MapGrhs are drawn.")]
        public bool DrawBackgroundMapGrhs { get; set; }

        /// <summary>
        /// Gets or sets if the <see cref="Character"/>s are drawn.
        /// The default value is true.
        /// </summary>
        [SyncValue]
        [Category(_generalCategoryName)]
        [DefaultValue(_defaultDrawCharacters)]
        [Browsable(true)]
        [Description("If characters on the map are drawn.")]
        public bool DrawCharacters { get; set; }

        /// <summary>
        /// Gets or sets if the foreground <see cref="MapGrh"/>s are drawn.
        /// The default value is true.
        /// </summary>
        [SyncValue]
        [Category(_mapGrhCategoryName)]
        [DefaultValue(_defaultDrawForegroundMapGrhs)]
        [Browsable(true)]
        [Description("If the foreground MapGrhs are drawn.")]
        public bool DrawForegroundMapGrhs { get; set; }

        /// <summary>
        /// Gets or sets if the <see cref="ItemEntityBase"/>s are drawn.
        /// The default value is true.
        /// </summary>
        [SyncValue]
        [Category(_generalCategoryName)]
        [DefaultValue(_defaultDrawItems)]
        [Browsable(true)]
        [Description("If items on the map are drawn.")]
        public bool DrawItems { get; set; }

        /// <summary>
        /// Gets a Func describing this filter.
        /// </summary>
        /// <returns>A Func describing this filter.</returns>
        [Browsable(false)]
        public Func<IDrawable, bool> FilterFunc
        {
            get { return _filterFunc; }
        }

        /// <summary>
        /// Gets or sets the maximum layer depth of the <see cref="MapGrh"/>s to draw.
        /// Default value is null.
        /// </summary>
        [SyncValue]
        [Category(_mapGrhCategoryName)]
        [DefaultValue(null)]
        [Browsable(true)]
        [Description("The maximum layer depth of the MapGrhs to draw.")]
        public int? MapGrhMaxDepth { get; set; }

        /// <summary>
        /// Gets or sets the minimum layer depth of the <see cref="MapGrh"/>s to draw.
        /// Default value is null.
        /// </summary>
        [SyncValue]
        [Category(_mapGrhCategoryName)]
        [DefaultValue(null)]
        [Browsable(true)]
        [Description("The minimum layer depth of the MapGrhs to draw.")]
        public int? MapGrhMinDepth { get; set; }

        /// <summary>
        /// Gets or sets if <see cref="MapGrh"/>s only on the default layer depth will be drawn.
        /// Default value is false.
        /// </summary>
        [SyncValue]
        [Category(_mapGrhCategoryName)]
        [DefaultValue(_defaultMapGrhsOnDefaultDepthOnly)]
        [Browsable(true)]
        [Description("When true, only the MapGrhs on the default layer depth will be drawn.")]
        public bool MapGrhsOnDefaultDepthOnly { get; set; }

        /// <summary>
        /// Gets or sets if <see cref="MapGrh"/>s only on the default layer will be drawn.
        /// Default value is false.
        /// </summary>
        [SyncValue]
        [Category(_mapGrhCategoryName)]
        [DefaultValue(_defaultMapGrhsOnDefaultLayerOnly)]
        [Browsable(true)]
        [Description("When true, only the MapGrhs on the default layer will be drawn.")]
        public bool MapGrhsOnDefaultLayerOnly { get; set; }

        /// <summary>
        /// Handles the PropertyChanged event of the EditorSettings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        void EditorSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            const string defaultDepthName = "MapGrh_DefaultDepth";
            const string defaultIsForegroundName = "MapGrh_DefaultIsForeground";

            EditorSettings.Default.AssertPropertyExists(defaultDepthName);
            EditorSettings.Default.AssertPropertyExists(defaultIsForegroundName);

            if (StringComparer.Ordinal.Equals(defaultDepthName, e.PropertyName))
                _mapGrh_DefaultDepth_Cache = EditorSettings.Default.MapGrh_DefaultDepth;
            else if (StringComparer.Ordinal.Equals(defaultIsForegroundName, e.PropertyName))
                _mapGrh_DefaultIsForeground_Cache = EditorSettings.Default.MapGrh_DefaultIsForeground;
        }

        /// <summary>
        /// The actual filtering method.
        /// </summary>
        /// <param name="drawable">The <see cref="IDrawable"/> to run the filter on.</param>
        /// <returns>True if the <paramref name="drawable"/> should be drawn; otherwise false.</returns>
        public bool Filter(IDrawable drawable)
        {
            if (drawable is MapGrh)
            {
                // MapGrhs
                var mg = (MapGrh)drawable;

                // Layer
                if (MapGrhsOnDefaultLayerOnly)
                {
                    if (mg.IsForeground != _mapGrh_DefaultIsForeground_Cache)
                        return false;
                }
                else
                {
                    if (mg.IsForeground)
                    {
                        if (!DrawForegroundMapGrhs)
                            return false;
                    }
                    else
                    {
                        if (!DrawBackgroundMapGrhs)
                            return false;
                    }
                }

                // Depth
                if (MapGrhsOnDefaultDepthOnly)
                {
                    if (mg.LayerDepth != _mapGrh_DefaultDepth_Cache)
                        return false;
                }
                else
                {
                    if (MapGrhMinDepth.HasValue && mg.LayerDepth < MapGrhMinDepth.Value)
                        return false;

                    if (MapGrhMaxDepth.HasValue && mg.LayerDepth > MapGrhMaxDepth.Value)
                        return false;
                }
            }
            else
            {
                switch (drawable.MapRenderLayer)
                {
                    case MapRenderLayer.Background:
                        // Background layer
                        if (!DrawBackground)
                            return false;
                        break;

                    case MapRenderLayer.Chararacter:
                        // Character layer
                        if (!DrawCharacters)
                            return false;
                        break;

                    case MapRenderLayer.Item:
                        // Item layer
                        if (!DrawItems)
                            return false;
                        break;
                }
            }

            return true;
        }

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);
        }

        #endregion
    }
}