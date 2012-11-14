using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// Provides an interface for sorting <see cref="IDrawable"/>s by their <see cref="MapRenderLayer"/>
    /// and their <see cref="IDrawable.LayerDepth"/>.
    /// </summary>
    public class DrawableSorter
    {
        /// <summary>
        /// The required size for the array of lists each instace of this class has.
        /// </summary>
        static readonly int _requiredArraySize;

        readonly List<IDrawable>[] _layers;

        /// <summary>
        /// Initializes the <see cref="DrawableSorter"/> class.
        /// </summary>
        static DrawableSorter()
        {
            _requiredArraySize = EnumHelper<MapRenderLayer>.MaxValue + 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableSorter"/> class.
        /// </summary>
        public DrawableSorter()
        {
            _layers = new List<IDrawable>[_requiredArraySize];
            for (var i = 0; i < _layers.Length; i++)
            {
                _layers[i] = new List<IDrawable>(32);
            }
        }

        /// <summary>
        /// Gets an IEnumerable of the <see cref="MapRenderLayer"/> and all the <see cref="IDrawable"/>s on that layer.
        /// The <see cref="MapRenderLayer"/>s will return in the order they are defined in the enum, and each item on
        /// each layer will be sorted by the <see cref="IDrawable.LayerDepth"/>. This method is not thread-safe, and must
        /// only be enumerated over one at a time.
        /// </summary>
        /// <param name="drawables">The <see cref="IDrawable"/>s to sort.</param>
        /// <returns>The IEnumerable of the <see cref="MapRenderLayer"/> and all the <see cref="IDrawable"/>s
        /// on that layer.</returns>
        public IEnumerable<KeyValuePair<MapRenderLayer, IEnumerable<IDrawable>>> GetSorted(IEnumerable<IDrawable> drawables)
        {
            // Clear all the layers
            foreach (var layer in _layers)
            {
                layer.Clear();
            }

            // Add all the drawables to their appropriate layer
            foreach (var drawable in drawables)
            {
                _layers[(int)drawable.MapRenderLayer].Add(drawable);
            }

            // Return the results for each layer, making sure to sort them as we return them
            for (var i = 0; i < _layers.Length; i++)
            {
                IEnumerable<IDrawable> sorted;
                if (i == (int)MapRenderLayer.Dynamic)
                {
                    // Sort by Y (bottom), then X (left), then depth (making depth quite useless)
                    sorted = _layers[i]
                        .OrderBy(obj => obj.Position.Y + obj.Size.Y)
                        .ThenBy(obj => obj.Position.X)
                        .ThenBy(obj => obj.LayerDepth);
                }
                else
                {
                    // Sort just by depth
                    sorted = _layers[i].OrderBy(obj => obj.LayerDepth);
                }

                yield return new KeyValuePair<MapRenderLayer, IEnumerable<IDrawable>>((MapRenderLayer)i, sorted);
            }
        }
    }
}