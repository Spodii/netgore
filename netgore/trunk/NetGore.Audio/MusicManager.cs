using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using NetGore.IO;

namespace NetGore.Audio
{
    /// <summary>
    /// Manages the music.
    /// </summary>
    public sealed class MusicManager : AudioManagerBase<IMusic, MusicID>
    {
        static readonly object _instanceLock = new object();
        static MusicManager _instance;

        /// <summary>
        /// Gets an instance of the <see cref="MusicManager"/> for the given <paramref name="contentManager"/>.
        /// </summary>
        /// <param name="contentManager">The <see cref="ContentManager"/>.</param>
        /// <returns>An instance of the <see cref="MusicManager"/> for the given
        /// <paramref name="contentManager"/>.</returns>
        public static MusicManager GetInstance(ContentManager contentManager)
        {
            if (_instance == null)
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                        _instance = new MusicManager(contentManager);
                }
            }

            return _instance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicManager"/> class.
        /// </summary>
        /// <param name="cm">The <see cref="ContentManager"/>.</param>
        MusicManager(ContentManager cm) : base(cm, ContentPaths.Build.Data.Join("music.xml"))
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets the fully qualified content path for the asset with the
        /// given name.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <returns>
        /// The fully qualified content path for the asset with the given name.
        /// </returns>
        protected override string GetContentPath(string assetName)
        {
            return "Music" + Path.DirectorySeparatorChar + assetName;
        }

        /// <summary>
        /// When overridden in the derived class, gets the name of the root node in the data file.
        /// </summary>
        protected override string RootNodeName
        {
            get { return "Music"; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the name of the items node in the data file.
        /// </summary>
        protected override string ItemsNodeName
        {
            get { return "Tracks"; }
        }

        /// <summary>
        /// When overridden in the derived class, handles creating and reading an object 
        /// from the given <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> used to read the object values from.</param>
        /// <returns>Instance of the object created using the <paramref name="reader"/>.</returns>
        protected override IMusic ReadHandler(IValueReader reader)
        {
            return new Music(ContentManager, reader, GetContentPath);
        }

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The converted value.</returns>
        protected override int IndexToInt(MusicID value)
        {
            return (int)value;
        }

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The converted value.</returns>
        protected override MusicID IntToIndex(int value)
        {
            return new MusicID(value);
        }
    }
}