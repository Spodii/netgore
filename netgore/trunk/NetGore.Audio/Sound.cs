using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using NetGore.IO;

namespace NetGore.Audio
{
    /// <summary>
    /// A unique sound effect.
    /// </summary>
    public class Sound : ISound
    {
        readonly SoundID _index;
        readonly string _name;
        readonly SoundEffect _soundEffect;
        readonly ContentManager _contentManager;
        readonly List<SoundEffectInstance> _instances = new List<SoundEffectInstance>(1);

        /// <summary>
        /// Gets the unique index of the <see cref="IAudio"/>.
        /// </summary>
        /// <returns>The unique index.</returns>
        int IAudio.GetIndex() { return (int)Index; }

        /// <summary>
        /// Gets if only one instance of this <see cref="IAudio"/> may be playing at a time.
        /// </summary>
        public bool IsSingleInstance
        {
            get { return false; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sound"/> class.
        /// </summary>
        /// <param name="cm">The <see cref="ContentManager"/>.</param>
        /// <param name="r">The <see cref="IValueReader"/> to read the values from.</param>
        /// <param name="getAssetName">Func used to get the fully qualified asset name.</param>
        internal Sound(ContentManager cm, IValueReader r, Func<string, string> getAssetName)
        {
            _contentManager = cm;
            _name = r.ReadString("File");
            _index = new SoundID(r.ReadUShort("Index"));

            var assetName = getAssetName(_name);
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Sound"/> class.
        /// </summary>
        /// <param name="cm">The <see cref="ContentManager"/>.</param>
        /// <param name="index">The index.</param>
        /// <param name="assetName">The name of the asset.</param>
        internal Sound(ContentManager cm, SoundID index, string assetName)
        {
            _contentManager = cm;
            _index = index;
            _name = assetName;

            _soundEffect = cm.Load<SoundEffect>(assetName);
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
            // TODO: ...
            throw new NotImplementedException();
        }

        /// <summary>
        /// Plays a new instance of the sound.
        /// </summary>
        /// <param name="emitter">The object that is emitting the sound.</param>
        public void Play(IAudioEmitter emitter)
        {
            // TODO: ...
            throw new NotImplementedException();
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
            // TODO: ...
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
                    item.Stop();
            }
        }
    }
}