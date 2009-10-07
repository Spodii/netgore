using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Audio
{
    /// <summary>
    /// Base class for a manager of audio tracks.
    /// </summary>
    /// <typeparam name="T">The Type of audio track.</typeparam>
    /// <typeparam name="TIndex">The Type of index.</typeparam>
    public abstract class AudioManagerBase<T, TIndex> : IDisposable, IEnumerable<T> where T : class, IAudio
    {
        readonly DArray<T> _items = new DArray<T>(false);
        readonly Dictionary<string, T> _itemsByName = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
        readonly ContentManager _contentManager;

        /// <summary>
        /// Gets the <see cref="ContentManager"/> used to load the audio tracks in this
        /// <see cref="AudioManagerBase&lt;T, TIndex&gt;"/>.
        /// </summary>
        public ContentManager ContentManager { get { return _contentManager; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioManagerBase&lt;T, TIndex&gt;"/> class.
        /// </summary>
        /// <param name="cm">The <see cref="ContentManager"/> used to load the audio tracks.</param>
        /// <param name="dataFilePath">The file path to the audio data to load.</param>
        protected AudioManagerBase(ContentManager cm, string dataFilePath)
        {
            _contentManager = cm;

// ReSharper disable DoNotCallOverridableMethodsInConstructor
            IValueReader r = new XmlValueReader(dataFilePath, RootNodeName);
// ReSharper restore DoNotCallOverridableMethodsInConstructor
            Load(r);
        }

        /// <summary>
        /// When overridden in the derived class, gets the fully qualified content path for the asset with the
        /// given name.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <returns>The fully qualified content path for the asset with the given name.</returns>
        protected abstract string GetContentPath(string assetName);

        /// <summary>
        /// When overridden in the derived class, gets the name of the root node in the data file.
        /// </summary>
        protected abstract string RootNodeName { get; }

        /// <summary>
        /// When overridden in the derived class, gets the name of the items node in the data file.
        /// </summary>
        protected abstract string ItemsNodeName { get;}

        /// <summary>
        /// When overridden in the derived class, handles creating and reading an object of type <typeparamref name="T"/>
        /// from the given <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> used to read the object values from.</param>
        /// <returns>Instance of the object created using the <paramref name="reader"/>.</returns>
        protected abstract T ReadHandler(IValueReader reader);

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The converted value.</returns>
        protected abstract int IndexToInt(TIndex value);

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The converted value.</returns>
        protected abstract TIndex IntToIndex(int value);

        /// <summary>
        /// Gets the audio item at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the item to get.</param>
        /// <returns>The item at the given <paramref name="index"/>, or null if the <see cref="index"/> is invalid
        /// or no item exists for the given <see cref="index"/>.</returns>
        public T this[TIndex index]
        {
            get
            {
                var intIndex = IndexToInt(index);
                return this[intIndex];
            }
        }

        /// <summary>
        /// Gets the audio item with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the item to get.</param>
        /// <returns>The item at the given <paramref name="name"/>, or null if the <see cref="name"/> is invalid
        /// or no item exists for the given <see cref="name"/>.</returns>
        public T this[string name]
        {
            get
            {
                T item;
                if (!_itemsByName.TryGetValue(name, out item))
                    return null;

                return item;
            }
        }

        /// <summary>
        /// Gets the audio item at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the item to get.</param>
        /// <returns>The item at the given <paramref name="index"/>, or null if the <see cref="index"/> is invalid
        /// or no item exists for the given <see cref="index"/>.</returns>
        protected T this[int index]
        {
            get
            {
                if (!_items.CanGet(index))
                    return null;

                return _items[index];
            }
        }

        /// <summary>
        /// Loads the audio track data.
        /// </summary>
        /// <param name="reader">IValueReader to read the data from.</param>
        void Load(IValueReader reader)
        {
            var items = reader.ReadManyNodes<T>(ItemsNodeName, ReadHandler);
            foreach (var item in items)
            {
                int index = item.GetIndex();
                string name = item.Name;

                // TODO: Better error checking
                Debug.Assert(this[index] == null);
                _items.Insert(index, item);

                Debug.Assert(this[name] == null);
                _itemsByName.Add(name, item);
            }
        }

        /// <summary>
        /// Allows for additional disposing to be done by derived classes. This disposing takes place before the
        /// base class is disposed.
        /// </summary>
        protected virtual void InternalDispose()
        {
        }

        bool _isDisposed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            InternalDispose();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_items).GetEnumerator();
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
    }
}
