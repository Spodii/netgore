using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Content;
using NetGore.IO;
using SFML;
using SFML.Audio;
using SFML.Graphics;

namespace NetGore.Audio
{
    /// <summary>
    /// Interface for an object that manages the game music.
    /// </summary>
    public interface IMusicManager
    {
        /// <summary>
        /// Gets or sets if music will loop. Default is true.
        /// </summary>
        bool Loop { get; set; }

        /// <summary>
        /// Gets the <see cref="IMusicInfo"/>s for all music tracks.
        /// </summary>
        IEnumerable<IMusicInfo> MusicInfos { get; }

        /// <summary>
        /// Gets the <see cref="IMusicInfo"/> for the music track currently playing. Will be null if no music
        /// is playing.
        /// </summary>
        IMusicInfo Playing { get; }

        /// <summary>
        /// Gets or sets the global volume of all music. This value must be in a range of 0 to 100, where 0 is
        /// silence and 100 is the full volume. If a value is specified that does not fall into this range, it will be
        /// altered to fit this range. Default is 100.
        /// </summary>
        float Volume { get; set; }

        /// <summary>
        /// Gets the <see cref="IMusicInfo"/> for a music track.
        /// </summary>
        /// <param name="id">The id of the <see cref="IMusicInfo"/> to get.</param>
        /// <returns>The <see cref="IMusicInfo"/> for the given <paramref name="id"/>, or null if the value
        /// was invalid.</returns>
        IMusicInfo GetMusicInfo(MusicID id);

        /// <summary>
        /// Gets the <see cref="IMusicInfo"/> for a music track.
        /// </summary>
        /// <param name="name">The name of the <see cref="IMusicInfo"/> to get.</param>
        /// <returns>The <see cref="IMusicInfo"/> for the given <paramref name="name"/>, or null if the value
        /// was invalid.</returns>
        IMusicInfo GetMusicInfo(string name);

        /// <summary>
        /// Pauses the currently playing music, if any music is playing.
        /// </summary>
        void Pause();

        /// <summary>
        /// Plays a music track by the given <see cref="MusicID"/>.
        /// </summary>
        /// <param name="id">The ID of the music to play.</param>
        /// <returns>
        /// True if the music played successfully; otherwise false.
        /// </returns>
        bool Play(MusicID id);

        /// <summary>
        /// Resumes the currently paused music, if there is any paused music.
        /// </summary>
        void Resume();

        /// <summary>
        /// Stops the currently playing music.
        /// </summary>
        void Stop();

        /// <summary>
        /// Updates the <see cref="IMusicManager"/>.
        /// </summary>
        void Update();
    }

    /// <summary>
    /// Extension methods for the <see cref="IMusicManager"/>. 
    /// </summary>
    public static class IMusicManagerExtensions
    {
        /// <summary>
        /// Plays a music track.
        /// </summary>
        /// <param name="musicManager">The music manager.</param>
        /// <param name="name">The name of the music to play.</param>
        /// <returns>True if the music track was successfully played; otherwise false.</returns>
        public static bool Play(this IMusicManager musicManager, string name)
        {
            var info = musicManager.GetMusicInfo(name);
            if (info == null)
                return false;

            return musicManager.Play(info.ID);
        }

        /// <summary>
        /// Plays a music track.
        /// </summary>
        /// <param name="musicManager">The music manager.</param>
        /// <param name="info">The <see cref="IMusicInfo"/> to play.</param>
        /// <returns>
        /// True if the music played successfully; otherwise false.
        /// </returns>
        public static bool Play(this IMusicManager musicManager, IMusicInfo info)
        {
            return musicManager.Play(info.ID);
        }
    }

    /// <summary>
    /// Extension methods for the <see cref="ISoundManager"/>. 
    /// </summary>
    public static class ISoundManagerExtensions
    {
        /// <summary>
        /// Plays a sound.
        /// </summary>
        /// <param name="soundManager">The sound manager.</param>
        /// <param name="name">The name of the sound to play.</param>
        /// <returns>True if the sound was successfully played; otherwise false.</returns>
        public static bool Play(this ISoundManager soundManager, string name)
        {
            var info = soundManager.GetSoundInfo(name);
            if (info == null)
                return false;

            return soundManager.Play(info.ID);
        }

        /// <summary>
        /// Plays a sound.
        /// </summary>
        /// <param name="soundManager">The sound manager.</param>
        /// <param name="info">The <see cref="ISoundInfo"/> to play.</param>
        /// <returns>
        /// True if the sound played successfully; otherwise false.
        /// </returns>
        public static bool Play(this ISoundManager soundManager, ISoundInfo info)
        {
            return soundManager.Play(info.ID);
        }

        /// <summary>
        /// Plays a sound.
        /// </summary>
        /// <param name="soundManager">The sound manager.</param>
        /// <param name="info">The <see cref="ISoundInfo"/> to play.</param>
        /// <param name="source">The source.</param>
        /// <returns>
        /// True if the sound played successfully; otherwise false.
        /// </returns>
        public static bool Play(this ISoundManager soundManager, ISoundInfo info, Vector2 source)
        {
            return soundManager.Play(info.ID, source);
        }

        /// <summary>
        /// Plays a sound.
        /// </summary>
        /// <param name="soundManager">The sound manager.</param>
        /// <param name="info">The <see cref="ISoundInfo"/> to play.</param>
        /// <param name="source">The source.</param>
        /// <returns>
        /// True if the sound played successfully; otherwise false.
        /// </returns>
        public static bool Play(this ISoundManager soundManager, ISoundInfo info, IAudioEmitter source)
        {
            return soundManager.Play(info.ID, source);
        }

        /// <summary>
        /// Plays a sound.
        /// </summary>
        /// <param name="soundManager">The sound manager.</param>
        /// <param name="name">The name of the sound to play.</param>
        /// <param name="source">The source of the sound.</param>
        /// <returns>
        /// True if the sound was successfully played; otherwise false.
        /// </returns>
        public static bool Play(this ISoundManager soundManager, string name, Vector2 source)
        {
            var info = soundManager.GetSoundInfo(name);
            if (info == null)
                return false;

            return soundManager.Play(info.ID, source);
        }

        /// <summary>
        /// Plays a sound.
        /// </summary>
        /// <param name="soundManager">The sound manager.</param>
        /// <param name="name">The name of the sound to play.</param>
        /// <param name="source">The source of the sound.</param>
        /// <returns>
        /// True if the sound was successfully played; otherwise false.
        /// </returns>
        public static bool Play(this ISoundManager soundManager, string name, IAudioEmitter source)
        {
            var info = soundManager.GetSoundInfo(name);
            if (info == null)
                return false;

            return soundManager.Play(info.ID, source);
        }
    }

    /// <summary>
    /// Interface for an object that manages game sounds.
    /// </summary>
    public interface ISoundManager
    {
        /// <summary>
        /// Gets the <see cref="ISoundInfo"/>s for all sounds.
        /// </summary>
        IEnumerable<ISoundInfo> SoundInfos { get; }

        /// <summary>
        /// Gets or sets the global volume of all sounds. This value must be in a range of 0 to 100, where 0 is
        /// silence and 100 is the full volume. If a value is specified that does not fall into this range, it will be
        /// altered to fit this range. Default is 100.
        /// </summary>
        float Volume { get; set; }

        /// <summary>
        /// Gets the <see cref="ISoundInfo"/> for a sound.
        /// </summary>
        /// <param name="id">The id of the <see cref="ISoundInfo"/> to get.</param>
        /// <returns>The <see cref="ISoundInfo"/> for the given <paramref name="id"/>, or null if the value
        /// was invalid.</returns>
        ISoundInfo GetSoundInfo(SoundID id);

        /// <summary>
        /// Gets the <see cref="ISoundInfo"/> for a sound.
        /// </summary>
        /// <param name="name">The name of the <see cref="ISoundInfo"/> to get.</param>
        /// <returns>The <see cref="ISoundInfo"/> for the given <paramref name="name"/>, or null if the value
        /// was invalid.</returns>
        ISoundInfo GetSoundInfo(string name);

        /// <summary>
        /// Plays a sound by the given <see cref="SoundID"/>.
        /// </summary>
        /// <param name="id">The ID of the sound to play.</param>
        /// <returns>
        /// True if the sound played successfully; otherwise false.
        /// </returns>
        bool Play(SoundID id);

        /// <summary>
        /// Plays a sound by the given <see cref="SoundID"/>.
        /// </summary>
        /// <param name="id">The ID of the sound to play.</param>
        /// <param name="source">The world position that the sound is coming from.</param>
        /// <returns>
        /// True if the sound played successfully; otherwise false.
        /// </returns>
        bool Play(SoundID id, Vector2 source);

        /// <summary>
        /// Plays a sound by the given <see cref="SoundID"/>.
        /// </summary>
        /// <param name="id">The ID of the sound to play.</param>
        /// <param name="source">The <see cref="IAudioEmitter"/> that the sound is coming from.</param>
        /// <returns>
        /// True if the sound played successfully; otherwise false.
        /// </returns>
        bool Play(SoundID id, IAudioEmitter source);

        /// <summary>
        /// Stops all sounds.
        /// </summary>
        void Stop();

        /// <summary>
        /// Stops all 3D sounds. Any 2D sounds playing will continue to play.
        /// </summary>
        void Stop3D();

        /// <summary>
        /// Stops all 2D sounds. Any 3D sounds playing will continue to play.
        /// </summary>
        void Stop2D();

        /// <summary>
        /// Stops all instances of a sound with the given <see cref="SoundID"/>.
        /// </summary>
        /// <param name="id">The ID of the sounds to stop.</param>
        void Stop(SoundID id);

        /// <summary>
        /// Updates the <see cref="ISoundManager"/>.
        /// </summary>
        void Update();
    }

    /// <summary>
    /// Interface for an object that manages all of the audio.
    /// </summary>
    public interface IAudioManager
    {
        /// <summary>
        /// Gets or sets the global volume of all audio. This value must be in a range of 0 to 100, where 0 is
        /// silence and 100 is the full volume. If a value is specified that does not fall into this range, it will be
        /// altered to fit this range. Default is 100.
        /// </summary>
        float GlobalVolume { get; set; }

        /// <summary>
        /// Gets or sets the world position of the audio listener. This is almost always the user's character position.
        /// Only valid for when using 3D audio.
        /// </summary>
        Vector2 ListenerPosition { get; set; }

        /// <summary>
        /// Gets the <see cref="IMusicManager"/> instance.
        /// </summary>
        IMusicManager MusicManager { get; }

        /// <summary>
        /// Gets the <see cref="ISoundManager"/> instance.
        /// </summary>
        ISoundManager SoundManager { get; }

        /// <summary>
        /// Stops all forms of audio.
        /// </summary>
        void Stop();

        /// <summary>
        /// Updates all the audio.
        /// </summary>
        void Update();
    }

    /// <summary>
    /// Manages the game sounds.
    /// </summary>
    public class SoundManager : ISoundManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// How frequently, in milliseconds, the sounds are updated.
        /// </summary>
        const int _updateRate = 1000;

        readonly ISoundInfo[] _infos;
        readonly Dictionary<string, ISoundInfo> _infosByName = new Dictionary<string, ISoundInfo>(StringComparer.OrdinalIgnoreCase);
        readonly SoundBuffer[] _soundBuffers;
        readonly List<SoundInstance> _soundInstances = new List<SoundInstance>();

        /// <summary>
        /// The time at which sounds will next be updated.
        /// </summary>
        int _nextUpdateTime = int.MinValue;

        float _volume = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundManager"/> class.
        /// </summary>
        public SoundManager(IContentManager contentManager)
        {
            // Load the values from file
            var values = AudioManager.LoadValues("sounds", "Sound");

            // Create the _infos and _soundBuffers arrays large enough to hold all values
            int max = values.Max(x => x.Value);
            _infos = new ISoundInfo[max + 1];
            _soundBuffers = new SoundBuffer[_infos.Length];

            // Populate both collections
            foreach (var value in values)
            {
                SoundID id = new SoundID(value.Value);
                var soundInfo = new SoundInfo(value.Key, id);

                // Ensure no duplicates
                if (_infos[(int)id] != null)
                    throw new DuplicateKeyException(string.Format("Two or more SoundInfos found with the ID `{0}`!", soundInfo.ID));

                if (_infosByName.ContainsKey(soundInfo.Name))
                    throw new DuplicateKeyException(string.Format("Two or more SoundInfos found with the name `{0}`!",
                                                                  soundInfo.Name));

                // Add
                _infosByName.Add(soundInfo.Name, soundInfo);
                _infos[(int)soundInfo.ID] = soundInfo;
                _soundBuffers[(int)soundInfo.ID] = contentManager.LoadSoundBuffer(ContentPaths.SoundsFolder + "/" + soundInfo.Name,
                                                                                  ContentLevel.GameScreen);
            }
        }

        /// <summary>
        /// Gets the <see cref="SoundBuffer"/> for the given <see cref="SoundID"/>.
        /// </summary>
        /// <param name="id">The <see cref="SoundID"/> for the <see cref="SoundBuffer"/> to get.</param>
        /// <returns>The <see cref="SoundBuffer"/> for the <paramref name="id"/>, or null if the <see cref="SoundBuffer"/>
        /// failed to load or the <paramref name="id"/> is invalid.</returns>
        protected SoundBuffer GetSoundBuffer(SoundID id)
        {
            var i = (int)id;
            if (i < 0 || i >= _soundBuffers.Length)
                return null;

            return _soundBuffers[i];
        }

        /// <summary>
        /// Attempts to create a <see cref="Sound"/> instance.
        /// </summary>
        /// <param name="id">The <see cref="SoundID"/>.</param>
        /// <param name="info">When this method returns a non-null object, contains the <see cref="ISoundInfo"/> for the
        /// <paramref name="id"/>.</param>
        /// <returns>The <see cref="Sound"/> instance, or null if it failed to be created.</returns>
        Sound InternalCreateSound(SoundID id, out ISoundInfo info)
        {
            // Get the sound info
            info = GetSoundInfo(id);
            return InternalCreateSound(info);
        }

        /// <summary>
        /// Attempts to create a <see cref="Sound"/> instance.
        /// </summary>
        /// <param name="info">The <see cref="ISoundInfo"/> describing the sound to create.</param>
        /// <returns>The <see cref="Sound"/> instance, or null if it failed to be created.</returns>
        Sound InternalCreateSound(ISoundInfo info)
        {
            // Check for a valid ISoundInfo
            if (info == null)
            {
                if (log.IsInfoEnabled)
                    log.InfoFormat("Could not play sound ID `{0}` - no sound with that ID exists.");
                return null;
            }

            // Get the buffer
            var buffer = GetSoundBuffer(info.ID);
            if (buffer == null)
            {
                const string errmsg =
                    "Failed to play sound `{0}` - could not load the sound buffer. The sound file may not exist or be corrupt.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, info);
                return null;
            }

            // Create the sound instance
            Sound snd = null;
            try
            {
                snd = new Sound(buffer);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to create sound `{0}`: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, info, ex);

                try
                {
                    if (snd != null)
                        snd.Dispose();
                }
                catch (Exception ex2)
                {
                    const string errmsg2 = "Failed to dispose sound `{0}` that failed to be created: {1}";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg2, info, ex2);
                }

                return null;
            }

            snd.Volume = Volume;
            snd.Loop = false;

            return snd;
        }

        #region ISoundManager Members

        /// <summary>
        /// Gets the <see cref="ISoundInfo"/>s for all sounds.
        /// </summary>
        public IEnumerable<ISoundInfo> SoundInfos
        {
            get { return _infosByName.Values; }
        }

        /// <summary>
        /// Gets or sets the global volume of all sounds. This value must be in a range of 0 to 100, where 0 is
        /// silence and 100 is the full volume. If a value is specified that does not fall into this range, it will be
        /// altered to fit this range. Default is 100.
        /// </summary>
        public float Volume
        {
            get { return _volume; }
            set
            {
                // Keep the value in a valid range
                if (value < 0)
                    value = 0;
                else if (value > 100)
                    value = 100;

                if (_volume == value)
                    return;

                // Set the new value
                _volume = value;

                // Apply the new value to all the sound instances
                foreach (var si in _soundInstances)
                {
                    si.Sound.Volume = value;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="ISoundInfo"/> for a sound.
        /// </summary>
        /// <param name="id">The id of the <see cref="ISoundInfo"/> to get.</param>
        /// <returns>The <see cref="ISoundInfo"/> for the given <paramref name="id"/>, or null if the value
        /// was invalid.</returns>
        public ISoundInfo GetSoundInfo(SoundID id)
        {
            int i = (int)id;
            if (i < 0 || i >= _infos.Length)
                return null;

            return _infos[i];
        }

        /// <summary>
        /// Gets the <see cref="ISoundInfo"/> for a sound.
        /// </summary>
        /// <param name="name">The name of the <see cref="ISoundInfo"/> to get.</param>
        /// <returns>The <see cref="ISoundInfo"/> for the given <paramref name="name"/>, or null if the value
        /// was invalid.</returns>
        public ISoundInfo GetSoundInfo(string name)
        {
            ISoundInfo ret;
            if (!_infosByName.TryGetValue(name, out ret))
                return null;

            return ret;
        }

        /// <summary>
        /// Plays a sound by the given <see cref="SoundID"/>.
        /// </summary>
        /// <param name="id">The ID of the sound to play.</param>
        /// <returns>
        /// True if the sound played successfully; otherwise false.
        /// </returns>
        public bool Play(SoundID id)
        {
            // Create the sound instance
            ISoundInfo info;
            var snd = InternalCreateSound(id, out info);
            if (snd == null)
                return false;

            // Set the sound up and start playing it
            SoundInstance si;
            try
            {
                snd.RelativeToListener = false;
                si = new SoundInstance(info, snd, null);
                snd.Play();
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to play sound `{0}`: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, info, ex);

                try
                {
                    snd.Dispose();
                }
                catch (Exception ex2)
                {
                    const string errmsg2 = "Failed to dispose sound that failed to play `{0}`: {1}";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg2, info, ex2);
                }

                return false;
            }

            // Add to the list of active sounds
            _soundInstances.Add(si);

            return true;
        }

        /// <summary>
        /// Plays a sound by the given <see cref="SoundID"/>.
        /// </summary>
        /// <param name="id">The ID of the sound to play.</param>
        /// <param name="source">The world position that the sound is coming from.</param>
        /// <returns>
        /// True if the sound played successfully; otherwise false.
        /// </returns>
        public bool Play(SoundID id, Vector2 source)
        {
            // Create the sound instance
            ISoundInfo info;
            var snd = InternalCreateSound(id, out info);
            if (snd == null)
                return false;

            // Set the sound up and start playing it
            SoundInstance si;
            try
            {
                snd.Position = new Vector3(source, 0f);
                snd.RelativeToListener = true;
                si = new SoundInstance(info, snd, null);
                snd.Play();
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to play sound `{0}`: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, info, ex);

                try
                {
                    snd.Dispose();
                }
                catch (Exception ex2)
                {
                    const string errmsg2 = "Failed to dispose sound that failed to play `{0}`: {1}";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg2, info, ex2);
                }

                return false;
            }

            // Add to the list of active sounds
            _soundInstances.Add(si);

            return true;
        }

        /// <summary>
        /// Plays a sound by the given <see cref="SoundID"/>.
        /// </summary>
        /// <param name="id">The ID of the sound to play.</param>
        /// <param name="source">The <see cref="IAudioEmitter"/> that the sound is coming from.</param>
        /// <returns>
        /// True if the sound played successfully; otherwise false.
        /// </returns>
        public bool Play(SoundID id, IAudioEmitter source)
        {
            // Create the sound instance
            ISoundInfo info;
            var snd = InternalCreateSound(id, out info);
            if (snd == null)
                return false;

            // Set the sound up and start playing it
            SoundInstance si;
            try
            {
                snd.Position = new Vector3(source.Position, 0f);
                snd.RelativeToListener = true;
                si = new SoundInstance(info, snd, source);
                snd.Play();
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to play sound `{0}`: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, info, ex);

                try
                {
                    snd.Dispose();
                }
                catch (Exception ex2)
                {
                    const string errmsg2 = "Failed to dispose sound that failed to play `{0}`: {1}";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg2, info, ex2);
                }

                return false;
            }

            // Add to the list of active sounds
            _soundInstances.Add(si);

            return true;
        }

        /// <summary>
        /// Stops all sounds.
        /// </summary>
        public void Stop()
        {
            // Dispose all the sounds
            foreach (var si in _soundInstances)
            {
                try
                {
                    si.Sound.Dispose();
                }
                catch (Exception ex)
                {
                    const string errmsg = "Failed to dispose sound `{0}`: {1}";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, si, ex);
                }
            }

            // Clear the list
            _soundInstances.Clear();
        }

        /// <summary>
        /// Stops all 3D sounds. Any 2D sounds playing will continue to play.
        /// </summary>
        public void Stop3D()
        {
            for (int i = 0; i < _soundInstances.Count; i++)
            {
                if (!_soundInstances[i].Sound.RelativeToListener)
                    continue;

                _soundInstances.RemoveAt(i);
                i--;
            }
        }

        /// <summary>
        /// Stops all 2D sounds. Any 3D sounds playing will continue to play.
        /// </summary>
        public void Stop2D()
        {
            for (int i = 0; i < _soundInstances.Count; i++)
            {
                if (_soundInstances[i].Sound.RelativeToListener)
                    continue;

                _soundInstances.RemoveAt(i);
                i--;
            }
        }

        /// <summary>
        /// Stops all instances of a sound with the given <see cref="SoundID"/>.
        /// </summary>
        /// <param name="id">The ID of the sounds to stop.</param>
        public void Stop(SoundID id)
        {
            for (int i = 0; i < _soundInstances.Count; i++)
            {
                if (_soundInstances[i].SoundInfo.ID != id)
                    continue;

                _soundInstances.RemoveAt(i);
                i--;
            }
        }

        /// <summary>
        /// Updates all of the sounds.
        /// </summary>
        public void Update()
        {
            var time = Environment.TickCount;
            if (_nextUpdateTime > time)
                return;

            _nextUpdateTime = time + _updateRate;

            // Loop through all the sounds
            for (int i = 0; i < _soundInstances.Count; i++)
            {
                var curr = _soundInstances[i];

                if (curr.Sound.Status == SoundStatus.Playing)
                {
                    // If the sound is playing, ensure the position is up-to-date
                    if (curr.AudioEmitter != null)
                        curr.Sound.Position = new Vector3(curr.AudioEmitter.Position, 0f);
                }
                else
                {
                    // For stopped sounds, remove them from the list
                    try
                    {
                        curr.Sound.Dispose();
                    }
                    catch (Exception ex)
                    {
                        const string errmsg = "Failed to dispose sound `{0}`: {1}";
                        if (log.IsWarnEnabled)
                            log.WarnFormat(errmsg, curr.SoundInfo, ex);
                    }

                    _soundInstances.RemoveAt(i);
                }
            }
        }

        #endregion

        /// <summary>
        /// Describes a live, playing sound.
        /// </summary>
        struct SoundInstance
        {
            readonly IAudioEmitter _audioEmitter;
            readonly Sound _sound;
            readonly ISoundInfo _soundInfo;

            /// <summary>
            /// Initializes a new instance of the <see cref="SoundInstance"/> struct.
            /// </summary>
            /// <param name="soundInfo">The sound info.</param>
            /// <param name="sound">The sound.</param>
            /// <param name="audioEmitter">The audio emitter.</param>
            public SoundInstance(ISoundInfo soundInfo, Sound sound, IAudioEmitter audioEmitter)
            {
                _sound = sound;
                _soundInfo = soundInfo;
                _audioEmitter = audioEmitter;
            }

            /// <summary>
            /// Gets the <see cref="IAudioEmitter"/> that this sound is attached to position-wise. Can be null
            /// for a sound that does not move.
            /// </summary>
            public IAudioEmitter AudioEmitter
            {
                get { return _audioEmitter; }
            }

            /// <summary>
            /// Gets the <see cref="Sound"/>.
            /// </summary>
            public Sound Sound
            {
                get { return _sound; }
            }

            /// <summary>
            /// Gets the <see cref="ISoundInfo"/>.
            /// </summary>
            public ISoundInfo SoundInfo
            {
                get { return _soundInfo; }
            }
        }
    }

    /// <summary>
    /// Manages the game music.
    /// </summary>
    public class MusicManager : IMusicManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly IMusicInfo[] _infos;
        readonly Dictionary<string, IMusicInfo> _infosByName = new Dictionary<string, IMusicInfo>(StringComparer.OrdinalIgnoreCase);
        
        bool _loop = true;
        Music _playing;
        IMusicInfo _playingInfo;
        float _volume = 100;

        public MusicManager()
        {
            // Load the values from file
            var values = AudioManager.LoadValues("music", "Music");

            // Create the _infos array large enough to hold all values
            int max = values.Max(x => x.Value);
            _infos = new IMusicInfo[max + 1];

            // Populate both collections
            foreach (var value in values)
            {
                MusicID id = new MusicID(value.Value);
                var musicInfo = new MusicInfo(value.Key, id);

                // Ensure no duplicates
                if (_infos[(int)id] != null)
                    throw new DuplicateKeyException(string.Format("Two or more MusicInfos found with the ID `{0}`!", musicInfo.ID));

                if (_infosByName.ContainsKey(musicInfo.Name))
                    throw new DuplicateKeyException(string.Format("Two or more MusicInfos found with the name `{0}`!",
                                                                  musicInfo.Name));

                // Add
                _infosByName.Add(musicInfo.Name, musicInfo);
                _infos[(int)musicInfo.ID] = musicInfo;
            }
        }

        /// <summary>
        /// Gets the file path for a music file.
        /// </summary>
        /// <param name="musicInfo">The <see cref="IMusicInfo"/> to get the file path for.</param>
        /// <returns>The file path for the <paramref name="musicInfo"/>.</returns>
        protected static string GetFilePath(IMusicInfo musicInfo)
        {
            return ContentPaths.Build.Music.Join(musicInfo.Name) + ".xnb";
        }

        #region IMusicManager Members

        /// <summary>
        /// Gets or sets if music will loop. Default is true.
        /// </summary>
        public bool Loop
        {
            get { return _loop; }
            set
            {
                if (_loop == value)
                    return;

                // Store the new value
                _loop = value;

                // If music is currently playing, update the loop value on it
                if (_playing != null)
                    _playing.Loop = _loop;
            }
        }

        /// <summary>
        /// Gets the <see cref="IMusicInfo"/>s for all music tracks.
        /// </summary>
        public IEnumerable<IMusicInfo> MusicInfos
        {
            get { return _infosByName.Values; }
        }

        /// <summary>
        /// Gets the <see cref="IMusicInfo"/> for the music track currently playing. Will be null if no music
        /// is playing.
        /// </summary>
        public IMusicInfo Playing
        {
            get { return _playingInfo; }
        }

        /// <summary>
        /// Gets or sets the global volume of all music. This value must be in a range of 0 to 100, where 0 is
        /// silence and 100 is the full volume. If a value is specified that does not fall into this range, it will be
        /// altered to fit this range. Default is 100.
        /// </summary>
        public float Volume
        {
            get { return _volume; }
            set
            {
                // Keep in a valid range
                if (value < 0)
                    value = 0;
                else if (value > 100)
                    value = 100;

                if (_volume == value)
                    return;

                // Store the new value
                _volume = value;

                // If music is currently playing, update the volume value on it
                if (_playing != null)
                    _playing.Volume = _volume;
            }
        }

        /// <summary>
        /// Gets the <see cref="IMusicInfo"/> for a music track.
        /// </summary>
        /// <param name="id">The id of the <see cref="IMusicInfo"/> to get.</param>
        /// <returns>The <see cref="IMusicInfo"/> for the given <paramref name="id"/>, or null if the value
        /// was invalid.</returns>
        public IMusicInfo GetMusicInfo(MusicID id)
        {
            var i = (int)id;
            if (i < 0 || i >= _infos.Length)
                return null;

            return _infos[i];
        }

        /// <summary>
        /// Gets the <see cref="IMusicInfo"/> for a music track.
        /// </summary>
        /// <param name="name">The name of the <see cref="IMusicInfo"/> to get.</param>
        /// <returns>The <see cref="IMusicInfo"/> for the given <paramref name="name"/>, or null if the value
        /// was invalid.</returns>
        public IMusicInfo GetMusicInfo(string name)
        {
            IMusicInfo ret;
            if (!_infosByName.TryGetValue(name, out ret))
                return null;

            return ret;
        }

        /// <summary>
        /// Pauses the currently playing music, if any music is playing.
        /// </summary>
        public void Pause()
        {
            if (_playing != null)
                _playing.Pause();
        }

        /// <summary>
        /// Plays a music track by the given <see cref="MusicID"/>.
        /// </summary>
        /// <param name="id">The ID of the music to play.</param>
        /// <returns>
        /// True if the music played successfully; otherwise false.
        /// </returns>
        public bool Play(MusicID id)
        {
            // If the music is already playing, continue to play it
            if (_playingInfo != null && _playingInfo.ID == id)
            {
                _playing.Play();
                return true;
            }

            // Get the info for the music to play
            var info = GetMusicInfo(id);
            if (info == null)
                return false;

            // Stop the old music
            Stop();

            // Start the new music
            _playingInfo = info;

            var file = GetFilePath(info);
            try
            {
                _playing = new Music(file);
            }
            catch (LoadingFailedException ex)
            {
                const string errmsg = "Failed to load music `{0}`: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, info, ex);
                Debug.Fail(string.Format(errmsg, info, ex));

                _playing = null;
                _playingInfo = null;

                return false;
            }

            // Set the values for the music and start playing it
            _playing.Volume = Volume;
            _playing.Loop = Loop;
            _playing.RelativeToListener = false;
            _playing.Play();

            return true;
        }

        /// <summary>
        /// Resumes the currently paused music, if there is any paused music.
        /// </summary>
        public void Resume()
        {
            if (_playing != null && (_playing.Status == SoundStatus.Paused || _playing.Status == SoundStatus.Stopped))
                _playing.Play();
        }

        /// <summary>
        /// Stops the currently playing music.
        /// </summary>
        public void Stop()
        {
            if (_playing == null)
                return;

            try
            {
                _playing.Dispose();
            }
            catch (InvalidOperationException)
            {
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to dispose music `{0}` [`{1}`]: {2}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, _playing, _playingInfo, ex);
                Debug.Fail(string.Format(errmsg, _playing, _playingInfo, ex));
            }
            finally
            {
                _playing = null;
                _playingInfo = null;
            }
        }

        /// <summary>
        /// Updates the <see cref="IMusicManager"/>.
        /// </summary>
        void IMusicManager.Update()
        {
            // Not needed by this implementation
        }

        #endregion
    }

    /// <summary>
    /// Interface containing information on sounds.
    /// </summary>
    public interface ISoundInfo
    {
        /// <summary>
        /// Gets the unique ID of the sound.
        /// </summary>
        SoundID ID { get; }

        /// <summary>
        /// Gets the unique name of the sound.
        /// </summary>
        string Name { get; }
    }

    /// <summary>
    /// Interface containing information on music.
    /// </summary>
    public interface IMusicInfo
    {
        /// <summary>
        /// Gets the unique ID of the music.
        /// </summary>
        MusicID ID { get; }

        /// <summary>
        /// Gets the unique name of the music.
        /// </summary>
        string Name { get; }
    }

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

    /// <summary>
    /// Manages all of the game audio.
    /// </summary>
    public class AudioManager : IAudioManager
    {
        static readonly object _instanceSync = new object();
        static AudioManager _instance;

        readonly IMusicManager _musicManager;
        readonly ISoundManager _soundManager;

        /// <summary>
        /// Gets the <see cref="IAudioManager"/> instance.
        /// </summary>
        /// <param name="contentManager">The <see cref="IContentManager"/> to use to load the audio content. Although it is
        /// recommended to always use a non-null value to ensure an <see cref="IAudioManager"/> instance
        /// can be returned, this value can be null if it has already been called before while passing a non-null
        /// value.</param>
        /// <returns>The <see cref="IAudioManager"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="contentManager"/> is null and a valid
        /// <see cref="IContentManager"/> has not been passed to this method before.</exception>
        public static IAudioManager GetInstance(IContentManager contentManager)
        {
            lock (_instanceSync)
            {
                if (_instance == null)
                {
                    if (contentManager == null || contentManager.IsDisposed)
                        throw new ArgumentNullException("contentManager", "The contentManager parameter cannot be null or disposed on the first call to this method.");

                    _instance = new AudioManager(contentManager);
                }
            }

            return _instance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioManager"/> class.
        /// </summary>
        /// <param name="contentManager">The <see cref="IContentManager"/> to use to load the audio content.</param>
        AudioManager(IContentManager contentManager)
        {
            // Create the music and sound managers

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            _soundManager = CreateSoundManager(contentManager);
            if (_soundManager == null)
                throw new Exception("SoundManager is null.");

            _musicManager = CreateMusicManager(contentManager);
            if (_soundManager == null)
                throw new Exception("MusicManager is null.");
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Creates the <see cref="IMusicManager"/> to use in this <see cref="AudioManager"/>.
        /// </summary>
        /// <param name="contentManager">The <see cref="IContentManager"/> that can be used when creating
        /// the <see cref="IMusicManager"/>.</param>
        /// <returns>The <see cref="IMusicManager"/> instance. Cannot be null.</returns>
        protected virtual IMusicManager CreateMusicManager(IContentManager contentManager)
        {
            return new MusicManager();
        }

        /// <summary>
        /// Creates the <see cref="ISoundManager"/> to use in this <see cref="AudioManager"/>.
        /// </summary>
        /// <param name="contentManager">The <see cref="IContentManager"/> that can be used when creating
        /// the <see cref="ISoundManager"/>.</param>
        /// <returns>The <see cref="ISoundManager"/> instance. Cannot be null.</returns>
        protected virtual ISoundManager CreateSoundManager(IContentManager contentManager)
        {
            return new SoundManager(contentManager);
        }

        internal static IEnumerable<KeyValuePair<string, int>> LoadValues(string fileName, string rootNode)
        {
            IValueReader r = new XmlValueReader(ContentPaths.Build.Data.Join(fileName + ".xml"), rootNode);
            var ret = r.ReadManyNodes<KeyValuePair<string, int>>("Items", ReadValue);
            return ret;
        }

        internal static KeyValuePair<string, int> ReadValue(IValueReader r)
        {
            string file = r.ReadString("File");
            int index = r.ReadInt("Index");

            return new KeyValuePair<string, int>(file, index);
        }

        #region IAudioManager Members

        /// <summary>
        /// Gets or sets the global volume of all audio. This value must be in a range of 0 to 100, where 0 is
        /// silence and 100 is the full volume. If a value is specified that does not fall into this range, it will be
        /// altered to fit this range. Default is 100.
        /// </summary>
        public float GlobalVolume
        {
            get { return Listener.GlobalVolume; }
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > 100)
                    value = 100;

                Listener.GlobalVolume = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="IMusicManager"/> instance.
        /// </summary>
        public IMusicManager MusicManager
        {
            get { return _musicManager; }
        }

        /// <summary>
        /// Gets the <see cref="ISoundManager"/> instance.
        /// </summary>
        public ISoundManager SoundManager
        {
            get { return _soundManager; }
        }

        /// <summary>
        /// Stops all forms of audio.
        /// </summary>
        public void Stop()
        {
            MusicManager.Stop();
            SoundManager.Stop();
        }

        /// <summary>
        /// Gets or sets the world position of the audio listener. This is almost always the user's character position.
        /// Only valid for when using 3D audio.
        /// </summary>
        public Vector2 ListenerPosition
        {
            get { return new Vector2(Listener.Position.X, Listener.Position.Y); }
            set
            {
                Listener.Position = new Vector3(value, 0);
            }
        }

        /// <summary>
        /// Updates all the audio.
        /// </summary>
        public void Update()
        {
            MusicManager.Update();
            SoundManager.Update();
        }

        #endregion
    }
}