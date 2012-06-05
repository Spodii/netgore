using System.Linq;
using SFML.Graphics;

namespace NetGore.Audio
{
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
}