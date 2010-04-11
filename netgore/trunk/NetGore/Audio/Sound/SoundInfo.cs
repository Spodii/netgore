using System.Linq;

namespace NetGore.Audio
{
    /// <summary>
    /// Contains the information for sounds.
    /// </summary>
    public class SoundInfo : ISoundInfo
    {
        readonly SoundID _id;
        readonly string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The id.</param>
        public SoundInfo(string name, SoundID id)
        {
            _name = name;
            _id = id;
        }

        #region ISoundInfo Members

        /// <summary>
        /// Gets the unique ID of the sound.
        /// </summary>
        public SoundID ID
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets the unique name of the sound.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        #endregion
    }
}