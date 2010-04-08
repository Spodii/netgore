using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using NetGore.IO;
using SFML.Audio;

// FUTURE: Make the MusicManager work together with this better to unload all but the current playing music track

namespace NetGore.Audio
{
    /// <summary>
    /// A unique music track.
    /// </summary>
    public class MusicTrack : IMusic
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly AudioManagerBase _audioManager;
        readonly MusicID _index;
        readonly string _name;

        SFML.Audio.Music _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicTrack"/> class.
        /// </summary>
        /// <param name="audioManager">The <see cref="AudioManagerBase"/>.</param>
        /// <param name="r">The <see cref="IValueReader"/> to read the values from.</param>
        internal MusicTrack(AudioManagerBase audioManager, IValueReader r)
        {
            _audioManager = audioManager;
            _name = r.ReadString(IAudioHelper.FileValueKey);
            _index = new MusicID(r.ReadUShort(IAudioHelper.IndexValueKey));
        }

        #region IMusic Members

        /// <summary>
        /// Gets the fully qualified name of the asset used by this <see cref="IAudio"/>. This is the name used
        /// when loading from the <see cref="IContentManager"/>. It cannot be used to reference this
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
        /// Gets the music track index.
        /// </summary>
        public MusicID Index
        {
            get { return _index; }
        }

        /// <summary>
        /// Gets if only one instance of this <see cref="IAudio"/> may be playing at a time.
        /// </summary>
        public bool IsSingleInstance
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the name of the <see cref="IAudio"/>.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the current state of the music track.
        /// </summary>
        public SoundStatus State
        {
            get
            {
                if (_instance == null)
                    return  SoundStatus.Stopped;

                return _instance.Status;
            }
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
        /// Pauses the music track if it is playing.
        /// </summary>
        public void Pause()
        {
            if (_instance != null)
            _instance.Pause();
        }

        /// <summary>
        /// Plays the audio track. If <see cref="IsSingleInstance"/> is true, this will play the track if it is not
        /// already playing. Otherwise, this will spawn a new instance of the sound.
        /// </summary>
        public void Play()
        {
            // TODO: ## Music
            /*
            ((IAudio)this).UpdateVolume();

            // Ensure the instance is valid
            if (_instance == null)
                _instance = _audioManager.ContentManager.LoadMusic(AssetName, ContentLevel.GameScreen);

            if (_instance != null)
            _instance.Play();
            */
        }

        /// <summary>
        /// Resumes the music track if it was paused.
        /// </summary>
        public void Resume()
        {
            if (_instance != null)
            _instance.Play();
        }

        /// <summary>
        /// Stops the audio track. If <see cref="IsSingleInstance"/> is true, this will stop the track. Otherwise,
        /// every instance of the track will be stopped.
        /// </summary>
        public void Stop()
        {
            if (_instance != null)
            _instance.Stop();
        }

        /// <summary>
        /// Updates the audio.
        /// </summary>
        void IAudio.Update()
        {
        }

        /// <summary>
        /// Updates the volume of the audio to match the volume specified by the <see cref="IAudio.AudioManager"/>.
        /// </summary>
        void IAudio.UpdateVolume()
        {
            if (_instance != null)
            _instance.Volume = _audioManager.Volume;
        }

        #endregion
    }
}