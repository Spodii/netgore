using System;
using System.ComponentModel;
using System.Linq;
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
        const string _generalCategoryName = "Draw Filters";
        const string _mapGrhCategoryName = "Draw Filters - MapGrh";

        const bool _defaultDrawBackground = true;
        const bool _defaultDrawCharacters = true;
        const bool _defaultDrawItems = true;
        const bool _defaultDrawForegroundMapGrhs = true;
        const bool _defaultDrawBackgroundMapGrhs = true;
        static readonly int? _defaultMapGrhMinDepth = null;
        static readonly int? _defaultMapGrhMaxDepth = null;

        readonly Func<IDrawable, bool> _filterFunc;

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
        /// Gets a Func describing this filter.
        /// </summary>
        /// <returns>A Func describing this filter.</returns>
        [Browsable(false)]
        public Func<IDrawable, bool> FilterFunc
        {
            get { return _filterFunc; }
        }

        /// <summary>
        /// The actual filtering method.
        /// </summary>
        /// <param name="drawable">The <see cref="IDrawable"/> to run the filter on.</param>
        /// <returns>True if the <paramref name="drawable"/> should be drawn; otherwise false.</returns>
        public bool Filter(IDrawable drawable)
        {
            // Background layer
            if (!DrawBackground && drawable.MapRenderLayer == MapRenderLayer.Background)
                return false;

            // Character layer
            if (!DrawCharacters && drawable.MapRenderLayer == MapRenderLayer.Chararacter)
                return false;

            // Item layer
            if (!DrawItems && drawable.MapRenderLayer == MapRenderLayer.Item)
                return false;

            // MapGrhs
            if (drawable is MapGrh)
            {
                var mg = (MapGrh)drawable;
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

                if (MapGrhMinDepth.HasValue && mg.LayerDepth < MapGrhMinDepth.Value)
                    return false;

                if (MapGrhMaxDepth.HasValue && mg.LayerDepth > MapGrhMaxDepth.Value)
                    return false;
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