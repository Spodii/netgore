using System;
using System.Linq;
using NetGore.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Provides assistance in creating a draw filter for the <see cref="Map"/>.
    /// </summary>
    public class MapDrawFilterHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapDrawFilterHelper"/> class.
        /// </summary>
        public MapDrawFilterHelper()
        {
            DrawBackground = true;
            DrawCharacters = true;
            DrawItems = true;
            DrawMapGrhs = true;
        }

        /// <summary>
        /// Gets or sets if the background is drawn.
        /// </summary>
        public bool DrawBackground { get; set; }

        /// <summary>
        /// Gets or sets if the <see cref="Character"/>s are drawn.
        /// </summary>
        public bool DrawCharacters { get; set; }

        /// <summary>
        /// Gets or sets if the <see cref="ItemEntityBase"/>s are drawn.
        /// </summary>
        public bool DrawItems { get; set; }

        /// <summary>
        /// Gets or sets if the <see cref="MapGrh"/>s are drawn.
        /// </summary>
        public bool DrawMapGrhs { get; set; }

        /// <summary>
        /// The actual filtering method.
        /// </summary>
        /// <param name="drawable">The <see cref="IDrawable"/> to run the filter on.</param>
        /// <returns>True if the <paramref name="drawable"/> should be drawn; otherwise false.</returns>
        bool Filter(IDrawable drawable)
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
        /// Gets a Func describing this filter for the current settings.
        /// </summary>
        /// <returns>A Func describing this filter for the current settings.</returns>
        public Func<IDrawable, bool> GetFilter()
        {
            return Filter;
        }
    }
}