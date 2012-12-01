using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// A collection for <see cref="IMapDrawingExtension"/>s.
    /// </summary>
    public class MapDrawingExtensionCollection : ICollection<IMapDrawingExtension>
    {
        readonly List<IMapDrawingExtension> _extensions = new List<IMapDrawingExtension>();
        readonly TypedEventHandler<IDrawableMap, DrawableMapDrawEventArgs> _handleBeginDrawMap;
        readonly TypedEventHandler<IDrawableMap, DrawableMapDrawLayerEventArgs> _handleBeginDrawMapLayer;
        readonly TypedEventHandler<IDrawableMap, DrawableMapDrawEventArgs> _handleEndDrawMap;
        readonly TypedEventHandler<IDrawableMap, DrawableMapDrawLayerEventArgs> _handleEndDrawMapLayer;

        IDrawableMap _map;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapDrawingExtensionCollection"/> class.
        /// </summary>
        public MapDrawingExtensionCollection()
        {
            _handleBeginDrawMapLayer = BeginDrawMapLayerCallback;
            _handleEndDrawMapLayer = EndDrawMapLayerCallback;
            _handleBeginDrawMap = BeginDrawMapCallback;
            _handleEndDrawMap = EndDrawMapCallback;
        }

        /// <summary>
        /// Gets or sets the current <see cref="IDrawableMap"/>. Can be null.
        /// </summary>
        public virtual IDrawableMap Map
        {
            get { return _map; }
            set
            {
                if (_map == value)
                    return;

                // Remove the event hooks from the old map
                if (Map != null)
                {
                    Map.BeginDrawMap -= _handleBeginDrawMap;
                    Map.EndDrawMap -= _handleEndDrawMap;
                    Map.BeginDrawMapLayer -= _handleBeginDrawMapLayer;
                    Map.EndDrawMapLayer -= _handleEndDrawMapLayer;
                }

                // Set the new map
                _map = value;

                // Set the event hooks on the new map
                if (Map != null)
                {
                    Map.BeginDrawMap -= _handleBeginDrawMap;
                    Map.BeginDrawMap += _handleBeginDrawMap;

                    Map.EndDrawMap -= _handleEndDrawMap;
                    Map.EndDrawMap += _handleEndDrawMap;

                    Map.BeginDrawMapLayer -= _handleBeginDrawMapLayer;
                    Map.BeginDrawMapLayer += _handleBeginDrawMapLayer;

                    Map.EndDrawMapLayer -= _handleEndDrawMapLayer;
                    Map.EndDrawMapLayer += _handleEndDrawMapLayer;
                }
            }
        }

        /// <summary>
        /// Adds items to this collection.
        /// </summary>
        /// <param name="extensions">The items to add.</param>
        public virtual void Add(IEnumerable<IMapDrawingExtension> extensions)
        {
            foreach (var item in extensions)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Handles the <see cref="IDrawableMap.BeginDrawMap"/> event from the current map.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NetGore.Graphics.DrawableMapDrawEventArgs"/> instance containing the event data.</param>
        void BeginDrawMapCallback(IDrawableMap sender, DrawableMapDrawEventArgs e)
        {
            Debug.Assert(Map == sender, "How did we get an event from the wrong map?");
            Debug.Assert(e.SpriteBatch != null && !e.SpriteBatch.IsDisposed);

            foreach (var extension in _extensions)
            {
                extension.DrawBeforeMap(sender, e);
            }
        }

        /// <summary>
        /// Handles the <see cref="IDrawableMap.BeginDrawMapLayer"/> event from the current map.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NetGore.Graphics.DrawableMapDrawLayerEventArgs"/> instance containing the event data.</param>
        void BeginDrawMapLayerCallback(IDrawableMap sender, DrawableMapDrawLayerEventArgs e)
        {
            Debug.Assert(Map == sender, "How did we get an event from the wrong map?");
            Debug.Assert(e.SpriteBatch != null && !e.SpriteBatch.IsDisposed);

            foreach (var extension in _extensions)
            {
                extension.DrawBeforeLayer(sender, e);
            }
        }

        /// <summary>
        /// Handles the <see cref="IDrawableMap.EndDrawMap"/> event from the current map.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NetGore.Graphics.DrawableMapDrawEventArgs"/> instance containing the event data.</param>
        void EndDrawMapCallback(IDrawableMap sender, DrawableMapDrawEventArgs e)
        {
            Debug.Assert(Map == sender, "How did we get an event from the wrong map?");
            Debug.Assert(e.SpriteBatch != null && !e.SpriteBatch.IsDisposed);

            foreach (var extension in _extensions)
            {
                extension.DrawAfterMap(sender, e);
            }
        }

        /// <summary>
        /// Handles the <see cref="IDrawableMap.EndDrawMapLayer"/> event from the current map.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="e">The <see cref="NetGore.Graphics.DrawableMapDrawLayerEventArgs"/> instance containing the event data.</param>
        void EndDrawMapLayerCallback(IDrawableMap map, DrawableMapDrawLayerEventArgs e)
        {
            Debug.Assert(Map == map, "How did we get an event from the wrong map?");
            Debug.Assert(e.SpriteBatch != null && !e.SpriteBatch.IsDisposed);

            foreach (var extension in _extensions)
            {
                extension.DrawAfterLayer(map, e);
            }
        }

        /// <summary>
        /// Removes items from this collection.
        /// </summary>
        /// <param name="extensions">The items to remove.</param>
        /// <returns>The number of items that were successfully removed from the collection. If all items were removed successfully, this
        /// value will be equal to the number of items in the <paramref name="extensions"/>.</returns>
        public virtual int Remove(IEnumerable<IMapDrawingExtension> extensions)
        {
            var c = 0;

            foreach (var item in extensions)
            {
                if (Remove(item))
                    c++;
            }

            return c;
        }

        #region ICollection<IMapDrawingExtension> Members

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get { return _extensions.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        bool ICollection<IMapDrawingExtension>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Adds an item to this collection.
        /// </summary>
        /// <param name="extension">The item to add.</param>
        public void Add(IMapDrawingExtension extension)
        {
            if (_extensions.Contains(extension))
                return;

            _extensions.Add(extension);
        }

        /// <summary>
        /// Clears all items in the collection.
        /// </summary>
        public virtual void Clear()
        {
            _extensions.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise,
        /// false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(IMapDrawingExtension item)
        {
            return _extensions.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an
        /// <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the
        /// elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/>
        /// must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/>
        /// at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/>
        /// is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/>
        /// is less than 0.</exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is
        /// multidimensional.-or-<paramref name="arrayIndex"/> is equal to or greater than the length of
        /// <paramref name="array"/>.-or-The number of elements in the source
        /// <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from
        /// <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type
        /// <see cref="IMapDrawingExtension"/> cannot be cast automatically to the type of the destination
        /// <paramref name="array"/>.</exception>
        void ICollection<IMapDrawingExtension>.CopyTo(IMapDrawingExtension[] array, int arrayIndex)
        {
            _extensions.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<IMapDrawingExtension> GetEnumerator()
        {
            return _extensions.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Removes the specified item from the collection.
        /// </summary>
        /// <param name="extension">The item to remove.</param>
        /// <returns>True if the item was successfully removed; otherwise false.</returns>
        public virtual bool Remove(IMapDrawingExtension extension)
        {
            return _extensions.Remove(extension);
        }

        #endregion
    }
}