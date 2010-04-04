using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using NetGore.IO;

namespace NetGore.Audio
{
    // TODO: Apply volume fade to 3D sounds based off of distance from emitter and listener

    /// <summary>
    /// A unique sound effect.
    /// </summary>
    public class Sound : ISound
    {
        readonly List<SoundEffectInstance3D> _active3DSounds = new List<SoundEffectInstance3D>();
        readonly AudioManagerBase _audioManager;
        readonly AudioEmitter _emitter = new AudioEmitter { Forward = Vector3.Forward, Up = Vector3.Up };
        readonly SoundID _index;
        readonly List<SoundEffectInstance> _instances = new List<SoundEffectInstance>(1);
        readonly AudioListener _listener = new AudioListener { Forward = Vector3.Forward, Up = Vector3.Up };
        readonly string _name;

        SoundEffect _soundEffect;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sound"/> class.
        /// </summary>
        /// <param name="audioManager">The <see cref="AudioManagerBase"/>.</param>
        /// <param name="r">The <see cref="IValueReader"/> to read the values from.</param>
        internal Sound(AudioManagerBase audioManager, IValueReader r)
        {
            _audioManager = audioManager;

            _name = r.ReadString(IAudioHelper.FileValueKey);
            _index = new SoundID(r.ReadUShort(IAudioHelper.IndexValueKey));
        }

        /// <summary>
        /// Gets an available <see cref="SoundEffectInstance"/>.
        /// </summary>
        /// <returns>An available <see cref="SoundEffectInstance"/>.</returns>
        SoundEffectInstance GetFreeInstance()
        {
            SoundEffectInstance instance;

            lock (_instances)
            {
                // Return the first instance that is not playing
                for (int i = 0; i < _instances.Count; i++)
                {
                    if (_instances[i].State != SoundState.Playing)
                        return _instances[i];
                }

                // No existing non-playing instance was found, so make sure we have a valid SoundEffect
                // instance and create and return a new SoundEffectInstance
                if (_soundEffect == null || _soundEffect.IsDisposed)
                    _soundEffect = _audioManager.ContentManager.Load<SoundEffect>(AssetName, ContentLevel.GameScreen);

                instance = _soundEffect.CreateInstance();
                _instances.Add(instance);
            }

            instance.IsLooped = false;
            instance.Volume = AudioManager.Volume;
            return instance;
        }

        #region ISound Members

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
        /// Gets or sets the object that is listening to the sounds. If null, 3D sounds will not be able
        /// to be updated.
        /// </summary>
        public IAudioEmitter Listener { get; set; }

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
            var sfx = GetFreeInstance();
            var emitter = StaticAudioEmitter.Create(position);
            var sfx3D = SoundEffectInstance3D.Create(sfx, emitter);
            _active3DSounds.Add(sfx3D);
        }

        /// <summary>
        /// Plays a new instance of the sound.
        /// </summary>
        /// <param name="emitter">The object that is emitting the sound.</param>
        public void Play(IAudioEmitter emitter)
        {
            var sfx = GetFreeInstance();
            var sfx3D = SoundEffectInstance3D.Create(sfx, emitter);
            _active3DSounds.Add(sfx3D);
        }

        /// <summary>
        /// Plays the audio track. If <see cref="IsSingleInstance"/> is true, this will play the track if it is not
        /// already playing. Otherwise, this will spawn a new instance of the sound.
        /// </summary>
        public void Play()
        {
            var instance = GetFreeInstance();
            instance.Play();
        }

        /// <summary>
        /// Stops the audio track. If <see cref="IsSingleInstance"/> is true, this will stop the track. Otherwise,
        /// every instance of the track will be stopped.
        /// </summary>
        public void Stop()
        {
            lock (_instances)
            {
                foreach (var item in _instances)
                {
                    item.Stop();
                }
            }
        }

        /// <summary>
        /// Updates the audio.
        /// </summary>
        public void Update()
        {
            var l = Listener;
            if (l != null)
                _listener.Position = new Vector3(l.Position, 0f);

            int i = 0;

            // Update all of the 3d sounds
            while (i < _active3DSounds.Count)
            {
                var current = _active3DSounds[i];

                if (current.Sound.State == SoundState.Playing)
                {
                    // Update the playing sound
                    _emitter.Position = new Vector3(current.Emitter.Position, 0f);
                    current.Sound.Apply3D(_listener, _emitter);
                    i++;
                }
                else
                {
                    // Remove the dead sound
                    _active3DSounds.RemoveAt(i);
                    StaticAudioEmitter.TryReturn(current.Emitter);
                }
            }
        }

        /// <summary>
        /// Updates the volume of the audio to match the volume specified by the <see cref="IAudio.AudioManager"/>.
        /// </summary>
        public void UpdateVolume()
        {
            var v = AudioManager.Volume;

            lock (_instances)
            {
                for (int i = 0; i < _instances.Count; i++)
                {
                    if (_instances[i].State != SoundState.Playing)
                        _instances[i].Volume = v;
                }
            }
        }

        #endregion
    }
}