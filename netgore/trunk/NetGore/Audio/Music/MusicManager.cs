using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Content;
using NetGore.IO;
using SFML;
using SFML.Audio;

namespace NetGore.Audio
{
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
            return ContentPaths.Build.Music.Join(musicInfo.Name + ContentPaths.ContentFileSuffix);
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
}