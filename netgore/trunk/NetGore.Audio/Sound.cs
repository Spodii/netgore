using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using NetGore.IO;

// TODO: Apply volume fade to 3D sounds based off of distance from emitter and listener

namespace NetGore.Audio
{
    /// <summary>
    /// A unique sound effect.
    /// </summary>
    public class Sound : ISound
    {
        readonly SoundID _index;
        readonly List<SoundEffectInstance> _instances = new List<SoundEffectInstance>(1);
        readonly string _name;
        readonly SoundEffect _soundEffect;
        readonly List<SoundEffectInstance3D> _active3DSounds = new List<SoundEffectInstance3D>();
        readonly AudioListener _listener = new AudioListener { Forward = Vector3.Forward, Up = Vector3.Up };
        readonly AudioEmitter _emitter = new AudioEmitter { Forward = Vector3.Forward, Up = Vector3.Up };

        /// <summary>
        /// Initializes a new instance of the <see cref="Sound"/> class.
        /// </summary>
        /// <param name="cm">The <see cref="ContentManager"/>.</param>
        /// <param name="r">The <see cref="IValueReader"/> to read the values from.</param>
        /// <param name="getAssetName">Func used to get the fully qualified asset name.</param>
        internal Sound(ContentManager cm, IValueReader r, Func<string, string> getAssetName)
        {
            _name = r.ReadString("File");
            _index = new SoundID(r.ReadUShort("Index"));

            var assetName = getAssetName(_name);
            _soundEffect = cm.Load<SoundEffect>(assetName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sound"/> class.
        /// </summary>
        /// <param name="cm">The <see cref="ContentManager"/>.</param>
        /// <param name="index">The index.</param>
        /// <param name="assetName">The name of the asset.</param>
        internal Sound(ContentManager cm, SoundID index, string assetName)
        {
            _index = index;
            _name = assetName;

            _soundEffect = cm.Load<SoundEffect>(assetName);
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
                for (int i = 0; i < _instances.Count; i++)
                {
                    if (_instances[i].State != SoundState.Playing)
                        return _instances[i];
                }

                instance = _soundEffect.CreateInstance();
                _instances.Add(instance);
            }

            instance.IsLooped = false;
            return instance;
        }

        #region ISound Members

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
            get { return false; }
        }

        /// <summary>
        /// Gets the sound track index.
        /// </summary>
        /// <value></value>
        public SoundID Index
        {
            get { return _index; }
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
        /// Gets or sets the object that is listening to the sounds. If null, 3D sounds will not be able
        /// to be updated.
        /// </summary>
        public IAudioEmitter Listener
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the <see cref="IAudio"/>.
        /// </summary>
        /// <value></value>
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
            var instance = GetFreeInstance();
            instance.Play();
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

        #endregion
    }
}