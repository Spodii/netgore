using System;
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
        readonly ContentManager _contentManager;
        readonly MusicID _index;
        readonly SoundEffectInstance _instance;
        readonly string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Music"/> class.
        /// </summary>
        /// <param name="cm">The <see cref="ContentManager"/>.</param>
        /// <param name="r">The <see cref="IValueReader"/> to read the values from.</param>
        /// <param name="getAssetName">Func used to get the fully qualified asset name.</param>
        internal Music(ContentManager cm, IValueReader r, Func<string, string> getAssetName)
        {
            _contentManager = cm;
            _name = r.ReadString("File");
            _index = new MusicID(r.ReadUShort("Index"));

            var assetName = getAssetName(_name);
            var sound = cm.Load<SoundEffect>(assetName);

            _instance = sound.CreateInstance();
            _instance.IsLooped = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Music"/> class.
        /// </summary>
        /// <param name="cm">The <see cref="ContentManager"/>.</param>
        /// <param name="index">The index.</param>
        /// <param name="assetName">The name of the asset.</param>
        internal Music(ContentManager cm, MusicID index, string assetName)
        {
            _contentManager = cm;
            _index = index;
            _name = assetName;

            var sound = cm.Load<SoundEffect>(assetName);

            _instance = sound.CreateInstance();
            _instance.IsLooped = true;
        }

        #region IMusic Members

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
        /// <value></value>
        public MusicID Index
        {
            get { return _index; }
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