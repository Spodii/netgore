using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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
    /// Manages all of the game audio.
    /// </summary>
    public class AudioManager : IAudioManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly object _instanceSync = new object();
        static readonly float _listenerDepth = EngineSettings.Instance.SoundListenerDepth;

        static AudioManager _instance;

        readonly IMusicManager _musicManager;
        readonly ISoundManager _soundManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioManager"/> class.
        /// </summary>
        /// <param name="contentManager">The <see cref="IContentManager"/> to use to load the audio content.</param>
        /// <exception cref="AudioException"><see cref="SoundManager"/> is somehow null.</exception>
        /// <exception cref="AudioException"><see cref="MusicManager"/> is somehow null.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "MusicManager")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "SoundManager")]
        AudioManager(IContentManager contentManager)
        {
            // Create the music and sound managers

            _soundManager = CreateSoundManager(contentManager);
            if (_soundManager == null)
                throw new AudioException("SoundManager is somehow null.");

            _musicManager = CreateMusicManager(contentManager);
            if (_soundManager == null)
                throw new AudioException("MusicManager is somehow null.");
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
                    {
                        throw new ArgumentNullException("contentManager",
                            "The contentManager parameter cannot be null or disposed on the first call to this method.");
                    }

                    _instance = new AudioManager(contentManager);
                }
            }

            return _instance;
        }

        /// <summary>
        /// Loads audio values from file.
        /// </summary>
        /// <param name="fileName">The name of the file to load from.</param>
        /// <param name="rootNode">The name of the root node.</param>
        /// <returns>The loaded values.</returns>
        internal static IEnumerable<KeyValuePair<string, int>> LoadValues(string fileName, string rootNode)
        {
            var r = GenericValueReader.CreateFromFile(ContentPaths.Build.Data.Join(fileName + EngineSettings.DataFileSuffix),
                rootNode);
            var ret = r.ReadManyNodes("Items", ReadValue);
            return ret;
        }

        /// <summary>
        /// Reads an audio value from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="r">The <see cref="IValueReader"/> to read from.</param>
        /// <returns>The read audio value.</returns>
        static KeyValuePair<string, int> ReadValue(IValueReader r)
        {
            var file = r.ReadString("File");
            var index = r.ReadInt("Index");

            return new KeyValuePair<string, int>(file, index);
        }

        /// <summary>
        /// Writes the audio values to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="w">The <see cref="IValueWriter"/> to write to.</param>
        /// <param name="value">The value to write.</param>
        static void WriteValue(IValueWriter w, KeyValuePair<string, int> value)
        {
            w.Write("File", value.Key);
            w.Write("Index", value.Value);
        }

        /// <summary>
        /// Writes the audio values to file.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="rootNode">The name of the root node.</param>
        /// <param name="values">The values to write.</param>
        internal static void WriteValues(string fileName, string rootNode, IEnumerable<KeyValuePair<string, int>> values)
        {
            // Save to Dev
            var devPath = ContentPaths.Dev.Data.Join(fileName + EngineSettings.DataFileSuffix);
            using (var w = GenericValueWriter.Create(devPath, rootNode))
            {
                w.WriteManyNodes("Items", values, WriteValue);
            }

            // Copy from Dev to Build
            var buildPath = ContentPaths.Build.Data.Join(fileName + EngineSettings.DataFileSuffix);
            try
            {
                File.Copy(devPath, buildPath, true);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to copy file to ContentPaths.Build (`{0}` to `{1}`). Exception: {2}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, devPath, buildPath, ex);
                Debug.Fail(string.Format(errmsg, devPath, buildPath, ex));
            }
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
        /// Gets or sets the world position of the audio listener. This is almost always the user's character position.
        /// Only valid for when using 3D audio.
        /// </summary>
        public Vector2 ListenerPosition
        {
            get { return new Vector2(Listener.Position.X, Listener.Position.Y); }
            set
            {
                Listener.Position = new Vector3(value, _listenerDepth);
                Listener.Direction = new Vector3(0, 0, -1.0f);
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