using System.Diagnostics;
using System.Linq;
using NetGore;
using NetGore.Audio;
using NetGore.Content;
using NetGore.Features.ActionDisplays;
using NetGore.Graphics;
using NetGore.IO;

namespace DemoGame.Client
{
    /// <summary>
    /// Contains the scripts used for showing the <see cref="ActionDisplay"/>s. Each method marked with an
    /// <see cref="ActionDisplayScriptAttribute"/> should be static and contained the same signature.
    /// </summary>
    [ActionDisplayScriptCollection]
    public static class ActionDisplayScripts
    {
        static readonly ActionDisplayCollection _actionDisplays;

        static readonly IAudioManager _audioManager;
        static readonly IContentManager _contentManager;

        /// <summary>
        /// Initializes the <see cref="ActionDisplayScripts"/> class.
        /// </summary>
        static ActionDisplayScripts()
        {
            _contentManager = ContentManager.Create();
            _audioManager = AudioManager.GetInstance(_contentManager);

            _actionDisplays = ActionDisplayCollection.Read(ContentPaths.Build);
        }

        /// <summary>
        /// Gets the <see cref="ActionDisplayCollection"/> instance.
        /// </summary>
        public static ActionDisplayCollection ActionDisplays
        {
            get { return _actionDisplays; }
        }

        [ActionDisplayScript("Projectile")]
        public static void AD_Projectile(ActionDisplay actionDisplay, IMap map, Entity attacker, Entity attacked)
        {
            var drawableMap = map as IDrawableMap;
            var attackerAsCharacter = attacker as Character;

            // Play the sound
            PlaySoundSimple(actionDisplay, attacker);

            // Show the attack animation on the attacker
            if (attackerAsCharacter != null)
                attackerAsCharacter.Attack();

            // Show the graphic going from the attacker to attacked
            if (drawableMap != null && attacked != null && attacker != attacked && actionDisplay.GrhIndex != GrhIndex.Invalid)
            {
                var gd = GrhInfo.GetData(actionDisplay.GrhIndex);
                if (gd != null)
                {
                    var effect = new MapGrhEffectSeekPosition(new Grh(gd, AnimType.Loop, TickCount.Now), attacker.Center, true,
                                                             attacked.Center, 25f);

                    drawableMap.AddTemporaryMapEffect(effect);
                }
            }

            // TODO: !! Display particle effect
        }

        /// <summary>
        /// Plays the sound for an <see cref="ActionDisplay"/> in a very basic way.
        /// This is intended to be called by the scripts for playing sound instead of handling sound manually.
        /// </summary>
        /// <param name="actionDisplay">The <see cref="ActionDisplay"/>.</param>
        /// <param name="source">The source of the sound.</param>
        static void PlaySoundSimple(ActionDisplay actionDisplay, Entity source)
        {
            // Check for valid parameters
            if (actionDisplay == null)
            {
                Debug.Fail("actionDisplay must not be null.");
                return;
            }
            if (source == null)
            {
                Debug.Fail("source must not be null.");
                return;
            }

            // Make sure there is a valid sound
            if (!actionDisplay.Sound.HasValue)
                return;

            // When possible, attach the sound to the source. Otherwise, just play it where the source is currently at
            var attackerAsAudioEmitter = source as IAudioEmitter;
            if (attackerAsAudioEmitter != null)
                _audioManager.SoundManager.Play(actionDisplay.Sound.Value, attackerAsAudioEmitter);
            else
                _audioManager.SoundManager.Play(actionDisplay.Sound.Value, source.Center);
        }
    }
}