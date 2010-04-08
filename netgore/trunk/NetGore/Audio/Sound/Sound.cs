using System.Collections.Generic;
using System.Linq;
using NetGore.IO;
using SFML.Audio;
using SFML.Graphics;

// TODO: ## Add back in sounds

namespace NetGore.Audio
{
    // TODO: Apply volume fade to 3D sounds based off of distance from emitter and listener

    /// <summary>
    /// A unique sound effect.
    /// </summary>
    public class SoundTrack : ISound
    {
        readonly List<SoundEffectInstance3D> _active3DSounds = new List<SoundEffectInstance3D>();
        readonly AudioManagerBase _audioManager;
        readonly SoundID _index;
        readonly string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundTrack"/> class.
        /// </summary>
        /// <param name="audioManager">The <see cref="AudioManagerBase"/>.</param>
        /// <param name="r">The <see cref="IValueReader"/> to read the values from.</param>
        internal SoundTrack(AudioManagerBase audioManager, IValueReader r)
        {
            _audioManager = audioManager;
  
            _name = r.ReadString(IAudioHelper.FileValueKey);
            _index = new SoundID(r.ReadUShort(IAudioHelper.IndexValueKey));
        }

        #region ISound Members

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
        /// Gets the sound track index.
        /// </summary>
        public SoundID Index
        {
            get { return _index; }
        }

        /// <summary>
        /// Gets if only one instance of this <see cref="IAudio"/> may be playing at a time.
        /// </summary>
        public bool IsSingleInstance
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the name of the <see cref="IAudio"/>.
        /// </summary>
        public string Name
        {
            get { return _name; }
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
        /// Plays a new instance of the sound.
        /// </summary>
        /// <param name="position">The world position of the source of the sound.</param>
        public void Play(Vector2 position)
        {
        }

        /// <summary>
        /// Plays a new instance of the sound.
        /// </summary>
        /// <param name="emitter">The object that is emitting the sound.</param>
        public void Play(IAudioEmitter emitter)
        {
        }

        /// <summary>
        /// Plays the audio track. If <see cref="IsSingleInstance"/> is true, this will play the track if it is not
        /// already playing. Otherwise, this will spawn a new instance of the sound.
        /// </summary>
        public void Play()
        {
        }

        /// <summary>
        /// Stops the audio track. If <see cref="IsSingleInstance"/> is true, this will stop the track. Otherwise,
        /// every instance of the track will be stopped.
        /// </summary>
        public void Stop()
        {
        }

        /// <summary>
        /// Updates the audio.
        /// </summary>
        public void Update()
        {
        }

        /// <summary>
        /// Updates the volume of the audio to match the volume specified by the <see cref="IAudio.AudioManager"/>.
        /// </summary>
        public void UpdateVolume()
        {
        }

        #endregion
    }
}