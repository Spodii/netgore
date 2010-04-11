using System.Linq;

namespace NetGore.Audio
{
    /// <summary>
    /// Contains the information for music.
    /// </summary>
    public class MusicInfo : IMusicInfo
    {
        readonly MusicID _id;
        readonly string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The id.</param>
        public MusicInfo(string name, MusicID id)
        {
            _name = name;
            _id = id;
        }

        #region IMusicInfo Members

        /// <summary>
        /// Gets the unique ID of the music.
        /// </summary>
        public MusicID ID
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets the unique name of the music.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        #endregion
    }
}