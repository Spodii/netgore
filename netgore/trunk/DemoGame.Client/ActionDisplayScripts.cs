using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore;
using NetGore.Audio;
using NetGore.Content;
using NetGore.Features.DisplayAction;
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

        /// <summary>
        /// Gets the <see cref="ActionDisplayCollection"/> instance.
        /// </summary>
        public static ActionDisplayCollection ActionDisplays { get { return _actionDisplays; } }

        static readonly IContentManager _contentManager;
        static readonly IAudioManager _audioManager;

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
        /// Plays the sound for an <see cref="ActionDisplay"/> in a very basic way.
        /// This is intended to be called by the scripts for playing sound instead of handling sound manually.
        /// </summary>
        /// <param name="actionDisplay">The <see cref="ActionDisplay"/>.</param>
        /// <param name="source">The source of the sound.</param>
        static void PlaySoundSimple(ActionDisplay actionDisplay, Entity source)
        {
            if (actionDisplay.Sound.HasValue)
            {
                var attackerAsAudioEmitter = source as IAudioEmitter;

                if (attackerAsAudioEmitter != null)
                    _audioManager.SoundManager.Play(actionDisplay.Sound.Value, attackerAsAudioEmitter);
            }
        }

        [ActionDisplayScript("Projectile")]
        public static void Basic(ActionDisplay actionDisplay, IMap map, Entity attacker, Entity attacked)
        {
            var drawableMap = map as IDrawableMap;

            // TODO: !! Send the ActionDisplayID and attacked character ID when attacking

            // Play the sound
            PlaySoundSimple(actionDisplay, attacker);

            // Show the graphic going from the attacker to attacked
            if (drawableMap != null && attacked != null && attacker != attacked && actionDisplay.GrhIndex != GrhIndex.Invalid)
            {
                var gd = GrhInfo.GetData(actionDisplay.GrhIndex);
                if (gd != null)
                {
                var effect = new MapGrhEffectSeekSpatial(new Grh(gd, AnimType.Loop, TickCount.Now), attacker.Center,
                    true, attacked, 1f);

                drawableMap.AddTemporaryMapEffect(effect);
                }
            }

            // TODO: !! Display particle effect
        }
    }
}
