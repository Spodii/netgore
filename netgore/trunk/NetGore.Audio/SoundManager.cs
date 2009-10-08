using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using NetGore.IO;

namespace NetGore.Audio
{
    /// <summary>
    /// Manages the sounds.
    /// </summary>
    public sealed class SoundManager : AudioManagerBase<ISound, SoundID>
    {
        static readonly object _instanceLock = new object();
        static SoundManager _instance;

        /// <summary>
        /// Gets the sound object at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the item to get.</param>
        /// <returns>The item at the given <paramref name="index"/>, or null if the <see cref="index"/> is invalid
        /// or no item exists for the given <see cref="index"/>.</returns>
        public ISound this[SoundID index]
        {
            get { return GetItem(index); }
        }

        /// <summary>
        /// Gets the sound object with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the item to get.</param>
        /// <returns>The item at the given <paramref name="name"/>, or null if the <see cref="name"/> is invalid
        /// or no item exists for the given <see cref="name"/>.</returns>
        public ISound this[string name]
        {
            get { return GetItem(name); }
        }

        /// <summary>
        /// When overridden in the derived class, stops all the playing audio in this manager.
        /// </summary>
        public override void Stop()
        {
            foreach (var item in GetAudio)
                item.Stop();
        }

        /// <summary>
        /// When overridden in the derived class, gets the name of the items node in the data file.
        /// </summary>
        protected override string ItemsNodeName
        {
            get { return "Sounds"; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the name of the root node in the data file.
        /// </summary>
        protected override string RootNodeName
        {
            get { return "Sound"; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundManager"/> class.
        /// </summary>
        /// <param name="cm">The <see cref="ContentManager"/>.</param>
        SoundManager(ContentManager cm) : base(cm, ContentPaths.Build.Data.Join("sounds.xml"))
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets the fully qualified content path for the asset with the
        /// given name.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <returns>The fully qualified content path for the asset with the given name.</returns>
        protected override string GetContentPath(string assetName)
        {
            return "Sounds" + Path.DirectorySeparatorChar + assetName;
        }

        /// <summary>
        /// Gets an instance of the <see cref="SoundManager"/> for the given <paramref name="contentManager"/>.
        /// </summary>
        /// <param name="contentManager">The <see cref="ContentManager"/>.</param>
        /// <returns>An instance of the <see cref="SoundManager"/> for the given
        /// <paramref name="contentManager"/>.</returns>
        public static SoundManager GetInstance(ContentManager contentManager)
        {
            if (_instance == null)
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                        _instance = new SoundManager(contentManager);
                }
            }

            return _instance;
        }

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The converted value.</returns>
        protected override int IndexToInt(SoundID value)
        {
            return (int)value;
        }

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The converted value.</returns>
        protected override SoundID IntToIndex(int value)
        {
            return new SoundID(value);
        }

        /// <summary>
        /// When overridden in the derived class, handles creating and reading an object
        /// from the given <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> used to read the object values from.</param>
        /// <returns>Instance of the object created using the <paramref name="reader"/>.</returns>
        protected override ISound ReadHandler(IValueReader reader)
        {
            return new Sound(ContentManager, reader, GetContentPath);
        }
    }
}