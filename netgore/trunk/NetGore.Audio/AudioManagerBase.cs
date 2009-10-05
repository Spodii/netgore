using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace NetGore.Audio
{
    /// <summary>
    /// Base class for a manager for all the game audio.
    /// </summary>
    public abstract class AudioManagerBase : IDisposable
    {
        readonly List<Cue3D> _activeCues = new List<Cue3D>();
        readonly AudioEngine _audioEngine;
        readonly AudioEmitter _emitter;
        readonly Stack<Cue3D> _freeCues = new Stack<Cue3D>();
        readonly Stack<StaticAudioEmitter> _freeStaticAudioEmitters = new Stack<StaticAudioEmitter>();
        readonly AudioListener _listener;
        readonly SoundBank _soundBank;
        readonly WaveBank _waveBank;

        Cue _musicCue;

        /// <summary>
        /// Gets the Cue being used for the currently set music. Can be null.
        /// </summary>
        public Cue MusicCue
        {
            get { return _musicCue; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioManagerBase"/> class.
        /// </summary>
        /// <param name="settingsFile">The path to the settings file.</param>
        /// <param name="wavBankFile">The path to the wav bank file.</param>
        /// <param name="soundBankFile">The path to the sound bank file.</param>
        protected AudioManagerBase(string settingsFile, string wavBankFile, string soundBankFile)
        {
            _audioEngine = new AudioEngine(settingsFile);
            _waveBank = new WaveBank(_audioEngine, wavBankFile);
            _soundBank = new SoundBank(_audioEngine, soundBankFile);

            _listener = new AudioListener { Forward = Vector3.Forward, Up = Vector3.Up, Velocity = Vector3.Zero };
            _emitter = new AudioEmitter { Forward = Vector3.Forward, Up = Vector3.Up, Velocity = Vector3.Zero };
        }

        /// <summary>
        /// Stops and clears the currently set music, or does nothing if no music is set.
        /// </summary>
        public void ClearMusic()
        {
            if (_musicCue == null)
                return;

            _musicCue.Stop(AudioStopOptions.AsAuthored);
            _musicCue = null;
        }

        /// <summary>
        /// Updates the <see cref="AudioManager"/>.
        /// </summary>
        /// <param name="listenerPosition">The world position of the listener. This is generally where the user's
        /// character is.</param>
        protected virtual void DoUpdate(Vector2 listenerPosition)
        {
            int i = 0;

            _listener.Position = new Vector3(listenerPosition, 0f);

            while (i < _activeCues.Count)
            {
                var current = _activeCues[i];

                // Check if the cue has stopped
                if (current.Cue.IsStopped)
                {
                    // Remove the cue from the active list and push it into the free list
                    _activeCues.RemoveAt(i);
                    FreeCue3D(current);
                }
                else
                {
                    // Update the position information and move to the next index
                    UpdateCue3D(current);
                    i++;
                }
            }
        }

        /// <summary>
        /// Frees up a <see cref="Cue3D"/>.
        /// </summary>
        /// <param name="cue3D">The <see cref="Cue3D"/> to free.</param>
        void FreeCue3D(Cue3D cue3D)
        {
            _freeCues.Push(cue3D);

            // Keep track of the emitter if it is a StaticAudioEmitter
            var asSAE = cue3D.Emitter as StaticAudioEmitter;
            if (asSAE != null)
                _freeStaticAudioEmitters.Push(asSAE);
        }

        /// <summary>
        /// Plays a sound.
        /// </summary>
        /// <param name="cueName">The name of the <see cref="Cue"/> to play.</param>
        /// <param name="worldPosition">The world position to play the sound at.</param>
        /// <returns>The <see cref="Cue"/> created to play the sound.</returns>
        public virtual Cue Play(string cueName, Vector2 worldPosition)
        {
            // Get a StaticAudioEmitter
            StaticAudioEmitter emitter;

            if (_freeStaticAudioEmitters.Count > 0)
                emitter = _freeStaticAudioEmitters.Pop();
            else
                emitter = new StaticAudioEmitter();

            // Set up the emitter with the position, then call the other Play
            emitter.Initialize(worldPosition);

            return Play(cueName, emitter);
        }

        /// <summary>
        /// Plays a sound.
        /// </summary>
        /// <param name="cueName">The name of the <see cref="Cue"/> to play.</param>
        /// <param name="emitter">The <see cref="IAudioEmitter"/> that this sound is coming from.</param>
        /// <returns>The <see cref="Cue"/> created to play the sound.</returns>
        public virtual Cue Play(string cueName, IAudioEmitter emitter)
        {
            // Get the Cue
            var cue = _soundBank.GetCue(cueName);
            if (cue == null)
                throw new ArgumentException("No cue with the specified name found.", "cueName");

            Cue3D cue3D;

            // Get the free Cue3D
            if (_freeCues.Count > 0)
                cue3D = _freeCues.Pop();
            else
                cue3D = new Cue3D();

            // Initialize and add to the list of live Cue3Ds
            cue3D.Initialize(cue, emitter);
            _activeCues.Add(cue3D);

            // Update and play
            UpdateCue3D(cue3D);
            cue3D.Cue.Play();

            return cue3D.Cue;
        }

        /// <summary>
        /// Sets the music to play.
        /// </summary>
        /// <param name="cueName">The name of the <see cref="Cue"/> to play.</param>
        /// <exception cref="ArgumentException">No <see cref="Cue"/> found with the given
        /// <paramref name="cueName"/>.</exception>
        public void SetMusic(string cueName)
        {
            // Continue playing if this is already the music set
            if (MusicCue != null && !string.IsNullOrEmpty(cueName) &&
                MusicCue.Name.Equals(cueName, StringComparison.OrdinalIgnoreCase))
                return;

            // Get the Cue
            var cue = _soundBank.GetCue(cueName);
            if (cue == null)
                throw new ArgumentException("No cue with the specified name found.", "cueName");

            // Stop the old music and play the new
            if (_musicCue != null)
                _musicCue.Stop(AudioStopOptions.AsAuthored);

            _musicCue = cue;
            _musicCue.Play();
        }

        /// <summary>
        /// Stops all of the sounds playing except for the music.
        /// </summary>
        public void StopAll()
        {
            StopAll(AudioStopOptions.Immediate);
        }

        /// <summary>
        /// Stops all of the sounds playing except for the music.
        /// </summary>
        /// <param name="stopOptions">Specifies how to stop the sounds.</param>
        public void StopAll(AudioStopOptions stopOptions)
        {
            foreach (var cue3D in _activeCues)
            {
                cue3D.Cue.Stop(stopOptions);
                FreeCue3D(cue3D);
            }

            _activeCues.Clear();
        }

        /// <summary>
        /// Updates the <see cref="AudioManager"/>.
        /// </summary>
        /// <param name="listener">The emitter that is listening to the audio. This is generally where the
        /// user's character is.</param>
        public void Update(IAudioEmitter listener)
        {
            DoUpdate(listener.Position);
        }

        /// <summary>
        /// Updates the <see cref="AudioManager"/>.
        /// </summary>
        /// <param name="listenerPosition">The world position of the listener. This is generally where the user's
        /// character is.</param>
        public void Update(Vector2 listenerPosition)
        {
            DoUpdate(listenerPosition);
        }

        /// <summary>
        /// Updates a <see cref="Cue3D"/>.
        /// </summary>
        /// <param name="cue3D">The <see cref="Cue3D"/> to update.</param>
        protected virtual void UpdateCue3D(Cue3D cue3D)
        {
            _emitter.Position = new Vector3(cue3D.Emitter.Position, 0f);
            cue3D.Cue.Apply3D(_listener, _emitter);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            _audioEngine.Dispose();
            _waveBank.Dispose();
            _soundBank.Dispose();
        }

        #endregion

        /// <summary>
        /// Represents a <see cref="Cue"/> and the <see cref="IAudioEmitter"/> it is coming from.
        /// </summary>
        protected class Cue3D
        {
            Cue _cue;
            IAudioEmitter _emitter;

            /// <summary>
            /// Gets the <see cref="Cue"/> being played.
            /// </summary>
            public Cue Cue
            {
                get { return _cue; }
            }

            /// <summary>
            /// Gets the source emitting the sound.
            /// </summary>
            public IAudioEmitter Emitter
            {
                get { return _emitter; }
            }

            /// <summary>
            /// Initializes the <see cref="Cue3D"/>.
            /// </summary>
            /// <param name="cue">The <see cref="Cue"/> being played.</param>
            /// <param name="emitter">The source emitting the sound.</param>
            public void Initialize(Cue cue, IAudioEmitter emitter)
            {
                if (cue == null)
                    throw new ArgumentNullException("cue");
                if (emitter == null)
                    throw new ArgumentNullException("emitter");

                _cue = cue;
                _emitter = emitter;
            }
        }

        /// <summary>
        /// A <see cref="IAudioEmitter"/> implementation for a stationary source.
        /// </summary>
        class StaticAudioEmitter : IAudioEmitter
        {
            Vector2 _position;

            /// <summary>
            /// Initializes the <see cref="StaticAudioEmitter"/>.
            /// </summary>
            /// <param name="position">The position of the emitter.</param>
            public void Initialize(Vector2 position)
            {
                _position = position;
            }

            #region IAudioEmitter Members

            /// <summary>
            /// Gets the position of the audio emitter.
            /// </summary>
            /// <value></value>
            public Vector2 Position
            {
                get { return _position; }
            }

            #endregion
        }
    }
}
