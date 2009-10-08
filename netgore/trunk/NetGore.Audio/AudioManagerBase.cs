using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Audio
{
    public abstract class AudioManagerBase : IDisposable
    {
        readonly ContentManager _contentManager;
        bool _isDisposed = false;

        /// <summary>
        /// Gets the <see cref="ContentManager"/> used to load the audio tracks in this
        /// <see cref="AudioManagerBase&lt;T, TIndex&gt;"/>.
        /// </summary>
        public ContentManager ContentManager
        {
            get { return _contentManager; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioManagerBase"/> class.
        /// </summary>
        /// <param name="cm">The <see cref="ContentManager"/> used to load the audio tracks.</param>
        protected AudioManagerBase(ContentManager cm)
        {
            _contentManager = cm;
        }

        /// <summary>
        /// When overridden in the derived class, gets the fully qualified content path for the asset with the
        /// given name.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <returns>The fully qualified content path for the asset with the given name.</returns>
        protected abstract string GetContentPath(string assetName);

        /// <summary>
        /// Allows for additional disposing to be done by derived classes. This disposing takes place before the
        /// base class is disposed.
        /// </summary>
        protected virtual void InternalDispose()
        {
        }

        /// <summary>
        /// When overridden in the derived class, stops all the playing audio in this manager.
        /// </summary>
        public abstract void Stop();

        #region IDisposable Members

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

        #endregion
    }

    /// <summary>
    /// Base class for a manager of audio tracks.
    /// </summary>
    /// <typeparam name="T">The Type of audio track.</typeparam>
    /// <typeparam name="TIndex">The Type of index.</typeparam>
    public abstract class AudioManagerBase<T, TIndex> : AudioManagerBase where T : class, IAudio
    {
        const string _itemsNodeName = "Items";
        readonly DArray<T> _items = new DArray<T>(false);
        readonly Dictionary<string, T> _itemsByName = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets an IEnumerable of all the audio items in this manager.
        /// </summary>
        public IEnumerable<T> GetAudio
        {
            get { return _items; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioManagerBase&lt;T, TIndex&gt;"/> class.
        /// </summary>
        /// <param name="cm">The <see cref="ContentManager"/> used to load the audio tracks.</param>
        /// <param name="dataFilePath">The file path to the audio data to load.</param>
        /// <param name="rootNodeName">The name of the root node in the data file being loaded.</param>
        protected AudioManagerBase(ContentManager cm, string dataFilePath, string rootNodeName) : base(cm)
        {
            IValueReader r = new XmlValueReader(dataFilePath, rootNodeName);
            Load(r, cm);
        }

        /// <summary>
        /// Gets the audio item at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the item to get.</param>
        /// <returns>The item at the given <paramref name="index"/>, or null if the <see cref="index"/> is invalid
        /// or no item exists for the given <see cref="index"/>.</returns>
        protected T GetItem(TIndex index)
        {
            var intIndex = IndexToInt(index);
            return GetItem(intIndex);
        }

        /// <summary>
        /// Gets the audio item with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the item to get.</param>
        /// <returns>The item at the given <paramref name="name"/>, or null if the <see cref="name"/> is invalid
        /// or no item exists for the given <see cref="name"/>.</returns>
        protected T GetItem(string name)
        {
            T item;
            if (!_itemsByName.TryGetValue(name, out item))
                return null;

            return item;
        }

        /// <summary>
        /// Gets the audio item at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the item to get.</param>
        /// <returns>The item at the given <paramref name="index"/>, or null if the <see cref="index"/> is invalid
        /// or no item exists for the given <see cref="index"/>.</returns>
        protected T GetItem(int index)
        {
            if (!_items.CanGet(index))
                return null;

            return _items[index];
        }

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
        /// Loads the audio track data.
        /// </summary>
        /// <param name="reader">IValueReader to read the data from.</param>
        /// <param name="cm">The <see cref="ContentManager"/> to use.</param>
        void Load(IValueReader reader, ContentManager cm)
        {
            var items = reader.ReadManyNodes(_itemsNodeName, r => ReadHandler(r, cm));
            foreach (var item in items)
            {
                int index = item.GetIndex();
                string name = item.Name;

                // TODO: Better error handling
                Debug.Assert(GetItem(index) == null);
                _items.Insert(index, item);

                Debug.Assert(GetItem(name) == null);
                _itemsByName.Add(name, item);
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles creating and reading an object of type <typeparamref name="T"/>
        /// from the given <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> used to read the object values from.</param>
        /// <param name="cm">The <see cref="ContentManager"/> to use.</param>
        /// <returns>Instance of the object created using the <paramref name="reader"/>.</returns>
        protected abstract T ReadHandler(IValueReader reader, ContentManager cm);
    }
}