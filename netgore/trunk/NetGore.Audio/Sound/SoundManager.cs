using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using NetGore.IO;

namespace NetGore.Audio
{
    /// <summary>
    /// Manages the sounds.
    /// </summary>
    public sealed class SoundManager : AudioManagerBase<ISound, SoundID>
    {
        static readonly object _instanceLock = new object();
        static SoundManager _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundManager"/> class.
        /// </summary>
        /// <param name="cm">The <see cref="ContentManager"/>.</param>
        SoundManager(ContentManager cm)
            : base(cm, ContentPaths.Build.Data.Join("sounds.xml"), "Sound", "Sounds" + Path.DirectorySeparatorChar)
        {
        }

        /// <summary>
        /// Gets the sound object at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the item to get.</param>
        /// <returns>The item at the given <paramref name="index"/>, or null if the <see cref="index"/> is invalid
        /// or no item exists for the given <see cref="index"/>.</returns>
        public ISound this[SoundID index]
        {
            get { return GetItem(index); }
        }

        /// <summary>
        /// Gets the sound object with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the item to get.</param>
        /// <returns>The item at the given <paramref name="name"/>, or null if the <see cref="name"/> is invalid
        /// or no item exists for the given <see cref="name"/>.</returns>
        public ISound this[string name]
        {
            get { return GetItem(name); }
        }

        /// <summary>
        /// Gets an instance of the <see cref="SoundManager"/> for the given <paramref name="contentManager"/>.
        /// Only the first <see cref="ContentManager"/> passed to this method will be used. Successive calls
        /// can pass a null <see cref="ContentManager"/>, but doing so is not recommended if it can be avoided.
        /// This method is thread-safe, but it is recommended that you store the returned object in a local
        /// member if you want to access it frequently to avoid the overhead of thread synchronization.
        /// </summary>
        /// <param name="contentManager">The <see cref="ContentManager"/>.</param>
        /// <returns>An instance of the <see cref="SoundManager"/> for the given
        /// <paramref name="contentManager"/>.</returns>
        public static SoundManager GetInstance(ContentManager contentManager)
        {
            if (_instance == null)
                _instance = new SoundManager(contentManager);

            return _instance;
        }

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The converted value.</returns>
        protected override int IDToInt(SoundID value)
        {
            return (int)value;
        }

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The converted value.</returns>
        protected override SoundID IntToID(int value)
        {
            return new SoundID(value);
        }

        /// <summary>
        /// When overridden in the derived class, handles creating and reading an object
        /// from the given <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> used to read the object values from.</param>
        /// <returns>Instance of the object created using the <paramref name="reader"/>.</returns>
        protected override ISound ReadHandler(IValueReader reader)
        {
            return new Sound(this, reader);
        }

        /// <summary>
        /// When overridden in the derived class, stops all the playing audio in this manager.
        /// </summary>
        public override void Stop()
        {
            foreach (var item in Items)
            {
                item.Stop();
            }
        }

        /// <summary>
        /// Tries to play an audio track.
        /// </summary>
        /// <param name="id">The audio track to play.</param>
        /// <param name="position">The world position of the sound.</param>
        /// <returns>True if the audio track was successfully played; otherwise false.</returns>
        public bool TryPlay(SoundID id, Vector2 position)
        {
            return TryPlay(this[id], position);
        }

        /// <summary>
        /// Tries to play an audio track.
        /// </summary>
        /// <param name="name">The audio track to play.</param>
        /// <param name="position">The world position of the sound.</param>
        /// <returns>True if the audio track was successfully played; otherwise false.</returns>
        public bool TryPlay(string name, Vector2 position)
        {
            return TryPlay(this[name], position);
        }

        /// <summary>
        /// Tries to play an audio track.
        /// </summary>
        /// <param name="id">The audio track to play.</param>
        /// <param name="emitter">The object emitting the sound.</param>
        /// <returns>True if the audio track was successfully played; otherwise false.</returns>
        public bool TryPlay(SoundID id, IAudioEmitter emitter)
        {
            return TryPlay(this[id], emitter);
        }

        /// <summary>
        /// Tries to play an audio track.
        /// </summary>
        /// <param name="name">The audio track to play.</param>
        /// <param name="emitter">The object emitting the sound.</param>
        /// <returns>True if the audio track was successfully played; otherwise false.</returns>
        public bool TryPlay(string name, IAudioEmitter emitter)
        {
            return TryPlay(this[name], emitter);
        }

        /// <summary>
        /// When overridden in the derived class, tries to play an audio track.
        /// </summary>
        /// <param name="id">The ID of the audio track.</param>
        /// <returns>True if the audio track was successfully played; otherwise false.</returns>
        public override bool TryPlay(SoundID id)
        {
            var item = this[id];
            return TryPlay(item);
        }

        /// <summary>
        /// When overridden in the derived class, tries to play an audio track.
        /// </summary>
        /// <param name="name">The name of the audio track.</param>
        /// <returns>True if the audio track was successfully played; otherwise false.</returns>
        public override bool TryPlay(string name)
        {
            var item = this[name];
            return TryPlay(item);
        }

        /// <summary>
        /// Tries to play an audio track.
        /// </summary>
        /// <param name="sound">The audio track to play.</param>
        /// <param name="emitter">The object emitting the sound.</param>
        /// <returns>True if the audio track was successfully played; otherwise false.</returns>
        static bool TryPlay(ISound sound, IAudioEmitter emitter)
        {
            if (sound == null)
                return false;

            sound.Play(emitter);
            return true;
        }

        /// <summary>
        /// Tries to play an audio track.
        /// </summary>
        /// <param name="sound">The audio track to play.</param>
        /// <param name="position">The position to play the sound.</param>
        /// <returns>True if the audio track was successfully played; otherwise false.</returns>
        static bool TryPlay(ISound sound, Vector2 position)
        {
            if (sound == null)
                return false;

            sound.Play(position);
            return true;
        }

        /// <summary>
        /// Tries to play an audio track.
        /// </summary>
        /// <param name="sound">The audio track to play.</param>
        /// <returns>True if the audio track was successfully played; otherwise false.</returns>
        static bool TryPlay(IAudio sound)
        {
            if (sound == null)
                return false;

            sound.Play();
            return true;
        }
    }
}