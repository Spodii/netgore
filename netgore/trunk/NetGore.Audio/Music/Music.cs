using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using NetGore.IO;

// FUTURE: Make the MusicManager work together with this better to unload all but the current playing music track

namespace NetGore.Audio
{
    /// <summary>
    /// A unique music track.
    /// </summary>
    public class Music : IMusic
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly AudioManagerBase _audioManager;
        readonly MusicID _index;
        readonly Song _instance;
        readonly string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Music"/> class.
        /// </summary>
        /// <param name="audioManager">The <see cref="AudioManagerBase"/>.</param>
        /// <param name="r">The <see cref="IValueReader"/> to read the values from.</param>
        internal Music(AudioManagerBase audioManager, IValueReader r)
        {
            _audioManager = audioManager;
            _name = r.ReadString(IAudioHelper.FileValueKey);
            _index = new MusicID(r.ReadUShort(IAudioHelper.IndexValueKey));
            _instance = _audioManager.ContentManager.Load<Song>(AssetName);
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
            MediaPlayer.Volume = _audioManager.Volume;
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
            // HACK: This is really fucking lame. MediaPlayer gives us MP3 support, but it requires us to have Windows
            // Media Player installed, and it is slow as hell on the first playback (thus the thread call). So it is either
            // use wavs (and whore up the memory usage), use MP3s and deal with this crap, or switch music libraries...
            ThreadPool.QueueUserWorkItem(delegate
                                         {
                                             ((IAudio)this).UpdateVolume();

                                             try
                                             {
                                                 MediaPlayer.Play(_instance);
                                             }
                                             catch (InvalidOperationException ex)
                                             {
                                                 const string errmsg = "Failed to play music. Exception: {0}";
                                                 if (log.IsErrorEnabled)
                                                     log.ErrorFormat(errmsg, ex);
                                             }
                                         });
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
            MediaPlayer.Stop();
        }

        /// <summary>
        /// Pauses the music track if it is playing.
        /// </summary>
        public void Pause()
        {
            MediaPlayer.Pause();
        }

        /// <summary>
        /// Resumes the music track if it was paused.
        /// </summary>
        public void Resume()
        {
            MediaPlayer.Resume();
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

                switch (MediaPlayer.State)
                {
                    case MediaState.Paused:
                        return SoundState.Paused;
                    case MediaState.Playing:
                        return SoundState.Playing;
                    case MediaState.Stopped:
                        return SoundState.Stopped;

                    default:
                        const string errmsg = "Unknown MediaPlayer state `{0}`. How the hell did this happen!?";
                        if (log.IsFatalEnabled)
                            log.FatalFormat(errmsg, MediaPlayer.State);
                        Debug.Fail(string.Format(errmsg, MediaPlayer.State));
                        return SoundState.Stopped;
                }
            }
        }

        #endregion
    }
}