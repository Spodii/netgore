using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Content;
using NetGore.IO;
using SFML.Audio;
using SFML.Graphics;

namespace NetGore.Audio
{
    /// <summary>
    /// Manages the game sounds.
    /// </summary>
    public class SoundManager : ISoundManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The default <see cref="Sound.Attenuation"/> property value for <see cref="Sound"/>s.
        /// </summary>
        const float _attenuation = 1f;

        /// <summary>
        /// The default <see cref="Sound.MinDistance"/> property value for <see cref="Sound"/>s.
        /// </summary>
        const float _minDistance = 300f;

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
            var max = values.Max(x => x.Value);
            _infos = new ISoundInfo[max + 1];
            _soundBuffers = new SoundBuffer[_infos.Length];

            // Populate both collections
            foreach (var value in values)
            {
                var id = new SoundID(value.Value);
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
                _soundBuffers[(int)soundInfo.ID] = contentManager.LoadSoundBuffer(
                    ContentPaths.SoundsFolder + "/" + soundInfo.Name, ContentLevel.GameScreen);
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
                    log.Info("Could not play sound - was given a null ISoundInfo.");
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
            snd.Attenuation = _attenuation;
            snd.MinDistance = _minDistance;

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
            var i = (int)id;
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
        /// Stops all instances of a sound with the given <see cref="SoundID"/>.
        /// </summary>
        /// <param name="id">The ID of the sounds to stop.</param>
        public void Stop(SoundID id)
        {
            for (var i = 0; i < _soundInstances.Count; i++)
            {
                if (_soundInstances[i].SoundInfo.ID != id)
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
            for (var i = 0; i < _soundInstances.Count; i++)
            {
                if (_soundInstances[i].Sound.RelativeToListener)
                    continue;

                _soundInstances.RemoveAt(i);
                i--;
            }
        }

        /// <summary>
        /// Stops all 3D sounds. Any 2D sounds playing will continue to play.
        /// </summary>
        public void Stop3D()
        {
            for (var i = 0; i < _soundInstances.Count; i++)
            {
                if (!_soundInstances[i].Sound.RelativeToListener)
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
            for (var i = 0; i < _soundInstances.Count; i++)
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
}