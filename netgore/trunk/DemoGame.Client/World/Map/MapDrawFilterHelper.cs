using System;
using System.ComponentModel;
using System.Linq;
using NetGore;
using NetGore.Graphics;
using NetGore.IO;

namespace DemoGame.Client
{
    /// <summary>
    /// Provides assistance in creating a draw filter for the <see cref="Map"/>.
    /// </summary>
    public class MapDrawFilterHelper : IPersistable
    {
        const bool _defaultDrawBackground = true;
        const bool _defaultDrawCharacters = true;
        const bool _defaultDrawItems = true;
        const bool _defaultDrawMapGrhs = true;

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
            DrawMapGrhs = _defaultDrawMapGrhs;
        }

        /// <summary>
        /// Gets or sets if the background is drawn.
        /// The default value is true.
        /// </summary>
        [SyncValue]
        [DefaultValue(_defaultDrawBackground)]
        public bool DrawBackground { get; set; }

        /// <summary>
        /// Gets or sets if the <see cref="Character"/>s are drawn.
        /// The default value is true.
        /// </summary>
        [SyncValue]
        [DefaultValue(_defaultDrawCharacters)]
        public bool DrawCharacters { get; set; }

        /// <summary>
        /// Gets or sets if the <see cref="ItemEntityBase"/>s are drawn.
        /// The default value is true.
        /// </summary>
        [SyncValue]
        [DefaultValue(_defaultDrawItems)]
        public bool DrawItems { get; set; }

        /// <summary>
        /// Gets or sets if the <see cref="MapGrh"/>s are drawn.
        /// The default value is true.
        /// </summary>
        [SyncValue]
        [DefaultValue(_defaultDrawMapGrhs)]
        public bool DrawMapGrhs { get; set; }

        /// <summary>
        /// The actual filtering method.
        /// </summary>
        /// <param name="drawable">The <see cref="IDrawable"/> to run the filter on.</param>
        /// <returns>True if the <paramref name="drawable"/> should be drawn; otherwise false.</returns>
        public bool Filter(IDrawable drawable)
        {
            if (!DrawBackground && drawable.MapRenderLayer == MapRenderLayer.Background)
                return false;

            if (!DrawCharacters && drawable.MapRenderLayer == MapRenderLayer.Chararacter)
                return false;

            if (!DrawItems && drawable.MapRenderLayer == MapRenderLayer.Item)
                return false;

            if (!DrawMapGrhs && drawable is MapGrh)
                return false;

            return true;
        }

        /// <summary>
        /// Gets a Func describing this filter.
        /// </summary>
        /// <returns>A Func describing this filter.</returns>
        public Func<IDrawable, bool> FilterFunc { get { return _filterFunc; } }

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