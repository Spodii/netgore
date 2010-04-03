using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Audio
{
    /// <summary>
    /// Base class for a manager of audio tracks.
    /// </summary>
    public abstract class AudioManagerBase : IDisposable
    {
        readonly string _assetPrefix;
        readonly IContentManager _contentManager;
        bool _isDisposed = false;
        float _volume = 1.0f;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioManagerBase"/> class.
        /// </summary>
        /// <param name="cm">The <see cref="IContentManager"/> used to load the audio tracks.</param>
        /// <param name="assetPrefix">The prefix to give to assets used by this <see cref="AudioManagerBase"/>
        /// when loading them.</param>
        protected AudioManagerBase(IContentManager cm, string assetPrefix)
        {
            _contentManager = cm;
            _assetPrefix = assetPrefix;
        }

        /// <summary>
        /// Gets the prefix to give to assets used by this <see cref="AudioManagerBase"/> when loading them.
        /// </summary>
        public string AssetPrefix
        {
            get { return _assetPrefix; }
        }

        /// <summary>
        /// Gets the <see cref="IContentManager"/> used to load the audio tracks in this <see cref="AudioManagerBase"/>.
        /// </summary>
        public IContentManager ContentManager
        {
            get { return _contentManager; }
        }

        /// <summary>
        /// Gets or sets the master volume of all audio. This value must be in a range of 0.0f to 1.0f, where 0.0f is
        /// silence and 1.0f is the full volume. If a value is specified that does not fall into this range, it will be
        /// altered to fit this range.
        /// </summary>
        public static float MasterVolume
        {
            get { return SoundEffect.MasterVolume; }
            set
            {
                float v = value;

                if (v > 1.0f)
                    v = 1.0f;
                else if (v < 0.0f)
                    v = 0.0f;

                SoundEffect.MasterVolume = v;
            }
        }

        /// <summary>
        /// Gets or sets the volume of all tracks in this <see cref="AudioManagerBase"/>. This value is in a range
        /// between 0.0f (silence) and 1.0f (full volume).
        /// </summary>
        public float Volume
        {
            get { return _volume; }
            set
            {
                float v = value;

                if (v > 1.0f)
                    v = 1.0f;
                else if (v < 0.0f)
                    v = 0.0f;

                if (v == _volume)
                    return;

                _volume = value;
                ReapplyVolume();
            }
        }

        /// <summary>
        /// Allows for additional disposing to be done by derived classes. This disposing takes place before the
        /// base class is disposed.
        /// </summary>
        protected virtual void InternalDispose()
        {
        }

        /// <summary>
        /// When overridden in the derived class, re-applies the <see cref="Volume"/> property value to all the
        /// audio tracks in this manager.
        /// </summary>
        protected internal abstract void ReapplyVolume();

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
    /// <typeparam name="TID">The Type of index.</typeparam>
    public abstract class AudioManagerBase<T, TID> : AudioManagerBase where T : class, IAudio
    {
        const string _itemsNodeName = "Items";

        readonly DArray<T> _items = new DArray<T>(false);
        readonly Dictionary<string, T> _itemsByName = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioManagerBase{T, TIndex}"/> class.
        /// </summary>
        /// <param name="cm">The <see cref="IContentManager"/> used to load the audio tracks.</param>
        /// <param name="dataFilePath">The file path to the audio data to load.</param>
        /// <param name="rootNodeName">The name of the root node in the data file being loaded.</param>
        /// <param name="assetPrefix">The prefix to give to assets used by this <see cref="AudioManagerBase"/>
        /// when loading them.</param>
        protected AudioManagerBase(IContentManager cm, string dataFilePath, string rootNodeName, string assetPrefix)
            : base(cm, assetPrefix)
        {
            IValueReader r = new XmlValueReader(dataFilePath, rootNodeName);
            Load(r);
        }

        /// <summary>
        /// Gets all of the audio items in this collection.
        /// </summary>
        public IEnumerable<T> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets the audio item at the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The index of the item to get.</param>
        /// <returns>The item at the given <paramref name="id"/>, or null if the <see cref="id"/> is invalid
        /// or no item exists for the given <see cref="id"/>.</returns>
        public T GetItem(TID id)
        {
            var intID = IDToInt(id);
            return GetItem(intID);
        }

        /// <summary>
        /// Gets the audio item with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the item to get.</param>
        /// <returns>The item at the given <paramref name="name"/>, or null if the <see cref="name"/> is invalid
        /// or no item exists for the given <see cref="name"/>.</returns>
        public T GetItem(string name)
        {
            T item;
            if (!_itemsByName.TryGetValue(name, out item))
                return null;

            return item;
        }

        /// <summary>
        /// Gets the audio item at the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The index of the item to get.</param>
        /// <returns>The item at the given <paramref name="id"/>, or null if the <see cref="id"/> is invalid
        /// or no item exists for the given <see cref="id"/>.</returns>
        protected T GetItem(int id)
        {
            if (!_items.CanGet(id))
                return null;

            return _items[id];
        }

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The converted value.</returns>
        protected abstract int IDToInt(TID value);

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The converted value.</returns>
        protected abstract TID IntToID(int value);

        /// <summary>
        /// Loads the audio track data.
        /// </summary>
        /// <param name="reader">IValueReader to read the data from.</param>
        void Load(IValueReader reader)
        {
            var items = reader.ReadManyNodes<T>(_itemsNodeName, ReadHandler);
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
        /// <returns>Instance of the object created using the <paramref name="reader"/>.</returns>
        protected abstract T ReadHandler(IValueReader reader);

        /// <summary>
        /// When overridden in the derived class, reapplies the Volume property value to all the
        /// audio tracks in this manager.
        /// </summary>
        protected internal override void ReapplyVolume()
        {
            foreach (var item in _items)
            {
                item.UpdateVolume();
            }
        }

        /// <summary>
        /// When overridden in the derived class, tries to play an audio track.
        /// </summary>
        /// <param name="id">The ID of the audio track.</param>
        /// <returns>True if the audio track was successfully played; otherwise false.</returns>
        public abstract bool TryPlay(TID id);

        /// <summary>
        /// When overridden in the derived class, tries to play an audio track.
        /// </summary>
        /// <param name="name">The name of the audio track.</param>
        /// <returns>True if the audio track was successfully played; otherwise false.</returns>
        public abstract bool TryPlay(string name);
    }
}