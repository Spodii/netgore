using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using NetGore.IO;

// FUTURE: Make the MusicManager work together with this better to unload all but the current playing music track

namespace NetGore.Audio
{
    /// <summary>
    /// A unique music track.
    /// </summary>
    public class Music : IMusic
    {
        const string _fileValueKey = "File";
        const string _indexValueKey = "Index";
        readonly AudioManagerBase _audioManager;
        readonly MusicID _index;
        readonly SoundEffectInstance _instance;
        readonly string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Music"/> class.
        /// </summary>
        /// <param name="audioManager">The <see cref="AudioManagerBase"/>.</param>
        /// <param name="r">The <see cref="IValueReader"/> to read the values from.</param>
        internal Music(AudioManagerBase audioManager, IValueReader r)
        {
            _audioManager = audioManager;
            _name = r.ReadString(_fileValueKey);
            _index = new MusicID(r.ReadUShort(_indexValueKey));

            var sound = _audioManager.ContentManager.Load<SoundEffect>(AssetName);

            _instance = sound.CreateInstance();
            _instance.IsLooped = true;
            _instance.Volume = AudioManager.Volume;
        }

        #region IMusic Members

        /// <summary>
        /// Gets the fully qualified name of the asset used by this <see cref="IAudio"/>. This is the name used
        /// when loading from the <see cref="ContentManager"/>. It cannot be used to reference this
        /// <see cref="IAudio"/> in the underlying <see cref="AudioManagerBase"/>.
        /// </summary>
        public string AssetName
        {
            get { return AudioManager.AssetPrefix + _name; }
        }

        /// <summary>
        /// Gets the <see cref="AudioManagerBase"/> that contains this <see cref="IAudio"/>.
        /// </summary>
        public AudioManagerBase AudioManager
        {
            get { return _audioManager; }
        }

        /// <summary>
        /// Updates the volume of the audio to match the volume specified by the <see cref="IAudio.AudioManager"/>.
        /// </summary>
        void IAudio.UpdateVolume()
        {
            _instance.Volume = AudioManager.Volume;
        }

        /// <summary>
        /// Gets the unique index of the <see cref="IAudio"/>.
        /// </summary>
        /// <returns>The unique index.</returns>
        int IAudio.GetIndex()
        {
            return (int)Index;
        }

        /// <summary>
        /// Gets if only one instance of this <see cref="IAudio"/> may be playing at a time.
        /// </summary>
        public bool IsSingleInstance
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the music track index.
        /// </summary>
        public MusicID Index
        {
            get { return _index; }
        }

        /// <summary>
        /// Gets the name of the <see cref="IAudio"/>.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Plays the audio track. If <see cref="IsSingleInstance"/> is true, this will play the track if it is not
        /// already playing. Otherwise, this will spawn a new instance of the sound.
        /// </summary>
        public void Play()
        {
            _instance.Play();
        }

        /// <summary>
        /// Updates the audio.
        /// </summary>
        void IAudio.Update()
        {
        }

        /// <summary>
        /// Stops the audio track. If <see cref="IsSingleInstance"/> is true, this will stop the track. Otherwise,
        /// every instance of the track will be stopped.
        /// </summary>
        public void Stop()
        {
            _instance.Stop();
        }

        /// <summary>
        /// Pauses the music track if it is playing.
        /// </summary>
        public void Pause()
        {
            _instance.Pause();
        }

        /// <summary>
        /// Resumes the music track if it was paused.
        /// </summary>
        public void Resume()
        {
            _instance.Resume();
        }

        /// <summary>
        /// Gets the current state of the music track.
        /// </summary>
        public SoundState State
        {
            get
            {
                if (_instance == null)
                    return SoundState.Stopped;

                return _instance.State;
            }
        }

        #endregion
    }
}