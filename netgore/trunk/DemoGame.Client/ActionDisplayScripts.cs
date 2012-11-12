using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Audio;
using NetGore.Content;
using NetGore.Features.ActionDisplays;
using NetGore.Graphics;
using NetGore.Graphics.ParticleEngine;
using NetGore.IO;
using NetGore.World;

namespace DemoGame.Client
{
    /// <summary>
    /// Contains the scripts used for showing the <see cref="ActionDisplay"/>s. Each method marked with an
    /// <see cref="ActionDisplayScriptAttribute"/> should be static and contained the same signature.
    /// </summary>
    [ActionDisplayScriptCollection]
    public static class ActionDisplayScripts
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly ActionDisplayCollection _actionDisplays;

        /// <summary>
        /// A dictionary containing the <see cref="Character"/>s who have active <see cref="ITemporaryMapEffect"/>s out
        /// that are for casting a skill. This allows us to terminate the <see cref="ITemporaryMapEffect"/>s when they
        /// stop casting.
        /// </summary>
        static readonly Dictionary<Character, List<ITemporaryMapEffect>> _activeCastingSkillEffects =
            new Dictionary<Character, List<ITemporaryMapEffect>>();

        static readonly object _activeCastingSkillEffectsSync = new object();
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

        /// <summary>
        /// A generic <see cref="ActionDisplay"/> script used for displaying skills being casted. The created effects
        /// last until <see cref="Character.IsCastingSkill"/> is set to false on the <paramref name="source"/>.
        /// </summary>
        /// <param name="actionDisplay">The <see cref="ActionDisplay"/> being used.</param>
        /// <param name="map">The map that the entities are on.</param>
        /// <param name="source">The <see cref="Entity"/> that this action came from (the invoker of the action).</param>
        /// <param name="target">Unused by this script.</param>
        [ActionDisplayScript("CastingSkill")]
        public static void AD_CastingSkill(ActionDisplay actionDisplay, IMap map, Entity source, Entity target)
        {
            // The maximum life the effects will have. This way, if the entity gets stuck with IsCastingSkill set to true,
            // the effects will at least go away eventually. Make sure that this is not greater than the longest possible
            // time to cast a skill.
            const int maxEffectLife = 1000 * 20;

            var drawableMap = map as IDrawableMap;
            var sourceAsCharacter = source as Character;

            var castingEffects = new List<ITemporaryMapEffect>();

            // Make sure we have a valid source
            if (sourceAsCharacter == null)
            {
                const string errmsg = "AD_CastingSkill requires a Character as the source, but the source ({0}) is type `{1}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, source, source.GetType());
                Debug.Fail(string.Format(errmsg, source, source.GetType()));
                return;
            }

            // Play the sound
            PlaySoundSimple(actionDisplay, source);

            // Check if we can properly display the effect
            if (drawableMap != null)
            {
                // Show the graphic going from the source to target
                if (actionDisplay.GrhIndex != GrhIndex.Invalid)
                {
                    var gd = GrhInfo.GetData(actionDisplay.GrhIndex);
                    if (gd != null)
                    {
                        // Make the effect loop indefinitely
                        var grh = new Grh(gd, AnimType.Loop, TickCount.Now);
                        var effect = new MapGrhEffectTimed(grh, source.Center, maxEffectLife) { MapRenderLayer = MapRenderLayer.SpriteForeground };
                        drawableMap.AddTemporaryMapEffect(effect);
                        castingEffects.Add(effect);
                    }
                }

                // Show the particle effect
                var pe = ParticleEffectManager.Instance.TryCreateEffect(actionDisplay.ParticleEffect);
                if (pe != null)
                {
                    // Effect that just takes place on the caster
                    pe.Position = source.Center;
                    pe.Life = maxEffectLife;
                    var effect = new TemporaryMapParticleEffect(pe) { MapRenderLayer = MapRenderLayer.SpriteForeground };
                    drawableMap.AddTemporaryMapEffect(effect);
                    castingEffects.Add(effect);
                }
            }

            // Make sure we have at least one valid effect. If not, there is nothing more to do.
            if (castingEffects.Count <= 0)
                return;

            // Add the list of effects to our local dictionary
            lock (_activeCastingSkillEffectsSync)
            {
                // Make sure they don't already have effects in the dictionary
                RemoveFromActiveCastingSkillEffects(sourceAsCharacter);

                // Add to the dictionary
                _activeCastingSkillEffects.Add(sourceAsCharacter, castingEffects);
            }

            // Attach the listener for the IsCastingSkill
            sourceAsCharacter.IsCastingSkillChanged += AD_CastingSkill_Character_IsCastingSkillChanged;

            // If the source already finished casting the skill, destroy them now since we probably missed the event
            if (!sourceAsCharacter.IsCastingSkill)
                RemoveFromActiveCastingSkillEffects(sourceAsCharacter);
        }

        /// <summary>
        /// An event callback for <see cref="Character.IsCastingSkillChanged"/> for the
        /// the <see cref="AD_CastingSkill"/> <see cref="ActionDisplay"/> script.
        /// </summary>
        /// <param name="sender">The <see cref="Character"/> who's <see cref="Character.IsCastingSkillChanged"/> event
        /// was invoked.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        static void AD_CastingSkill_Character_IsCastingSkillChanged(Character sender, EventArgs e)
        {
            Debug.Assert(sender.IsCastingSkill == false);

            // Remove the event hook
            sender.IsCastingSkillChanged -= AD_CastingSkill_Character_IsCastingSkillChanged;

            // Remove their active effects for casting skills
            lock (_activeCastingSkillEffectsSync)
            {
                RemoveFromActiveCastingSkillEffects(sender);
            }
        }

        /// <summary>
        /// A basic <see cref="ActionDisplay"/> script for melee attacks.
        /// </summary>
        /// <param name="actionDisplay">The <see cref="ActionDisplay"/> being used.</param>
        /// <param name="map">The map that the entities are on.</param>
        /// <param name="source">The <see cref="Entity"/> that this action came from (the invoker of the action).</param>
        /// <param name="target">The <see cref="Entity"/> that this action is targeting. It is possible that this will be
        /// equal to the <paramref name="source"/> or be null.</param>
        [ActionDisplayScript("Melee")]
        public static void AD_Melee(ActionDisplay actionDisplay, IMap map, Entity source, Entity target)
        {
            var drawableMap = map as IDrawableMap;
            var sourceAsCharacter = source as Character;

            // Play the sound
            PlaySoundSimple(actionDisplay, source);

            // Show the attack animation on the attacker
            if (sourceAsCharacter != null)
                sourceAsCharacter.Attack();

            // Check if we can properly display the effect
            if (drawableMap != null && target != null && source != target)
            {
                // Show the graphic going from the attacker to attacked
                if (actionDisplay.GrhIndex != GrhIndex.Invalid)
                {
                    var gd = GrhInfo.GetData(actionDisplay.GrhIndex);
                    if (gd != null)
                    {
                        var grh = new Grh(gd, AnimType.Loop, TickCount.Now);
                        var effect = new MapGrhEffectLoopOnce(grh, source.Center) { MapRenderLayer = MapRenderLayer.SpriteForeground };
                        drawableMap.AddTemporaryMapEffect(effect);
                    }
                }

                // Show the particle effect
                var pe = ParticleEffectManager.Instance.TryCreateEffect(actionDisplay.ParticleEffect);
                if (pe != null)
                {
                    // Effect that just takes place on the target and dies very quickly
                    pe.Position = target.Center;
                    pe.Life = 100;
                    var effect = new TemporaryMapParticleEffect(pe) { MapRenderLayer = MapRenderLayer.SpriteForeground };
                    drawableMap.AddTemporaryMapEffect(effect);
                }
            }
        }

        /// <summary>
        /// A basic <see cref="ActionDisplay"/> script for projectiles.
        /// </summary>
        /// <param name="actionDisplay">The <see cref="ActionDisplay"/> being used.</param>
        /// <param name="map">The map that the entities are on.</param>
        /// <param name="source">The <see cref="Entity"/> that this action came from (the invoker of the action).</param>
        /// <param name="target">The <see cref="Entity"/> that this action is targeting. It is possible that this will be
        /// equal to the <paramref name="source"/> or be null.</param>
        [ActionDisplayScript("Projectile")]
        public static void AD_Projectile(ActionDisplay actionDisplay, IMap map, Entity source, Entity target)
        {
            var drawableMap = map as IDrawableMap;
            var sourceAsCharacter = source as Character;

            // Play the sound
            PlaySoundSimple(actionDisplay, source);

            // Show the attack animation on the attacker
            if (sourceAsCharacter != null)
                sourceAsCharacter.Attack();

            // Check if we can properly display the effect
            if (drawableMap != null && target != null && source != target)
            {
                // Show the graphic going from the attacker to attacked
                if (actionDisplay.GrhIndex != GrhIndex.Invalid)
                {
                    var gd = GrhInfo.GetData(actionDisplay.GrhIndex);
                    if (gd != null)
                    {
                        var grh = new Grh(gd, AnimType.Loop, TickCount.Now);
                        var effect = new MapGrhEffectSeekPosition(grh, source.Center, target.Center, 750f) { MapRenderLayer = MapRenderLayer.SpriteForeground };
                        drawableMap.AddTemporaryMapEffect(effect);
                    }
                }

                // Show the particle effect
                var pe = ParticleEffectManager.Instance.TryCreateEffect(actionDisplay.ParticleEffect);
                if (pe != null)
                {
                    // Effect that just takes place on the target and dies very quickly
                    pe.Position = target.Center;
                    pe.Life = 100;
                    var effect = new TemporaryMapParticleEffect(pe) { MapRenderLayer = MapRenderLayer.SpriteForeground };
                    drawableMap.AddTemporaryMapEffect(effect);
                }
            }
        }

        /// <summary>
        /// A basic <see cref="ActionDisplay"/> script for when a skill has been used.
        /// </summary>
        /// <param name="actionDisplay">The <see cref="ActionDisplay"/> being used.</param>
        /// <param name="map">The map that the entities are on.</param>
        /// <param name="source">The <see cref="Entity"/> that this action came from (the invoker of the action).</param>
        /// <param name="target">The <see cref="Entity"/> that this action is targeting. It is possible that this will be
        /// equal to the <paramref name="source"/> or be null.</param>
        [ActionDisplayScript("SkillCasted")]
        public static void AD_SkillCasted(ActionDisplay actionDisplay, IMap map, Entity source, Entity target)
        {
            var drawableMap = map as IDrawableMap;

            // Play the sound
            PlaySoundSimple(actionDisplay, source);

            // Check if we can properly display the effect
            if (drawableMap != null && source != null)
            {
                if (actionDisplay.GrhIndex != GrhIndex.Invalid)
                {
                    if (target != null && target != source)
                    {
                        // Show the graphic going from the source to target
                        var gd = GrhInfo.GetData(actionDisplay.GrhIndex);
                        if (gd != null)
                        {
                            var grh = new Grh(gd, AnimType.Loop, TickCount.Now);
                            var effect = new MapGrhEffectSeekPosition(grh, source.Center, target.Center, 750f) { MapRenderLayer = MapRenderLayer.SpriteForeground };
                            drawableMap.AddTemporaryMapEffect(effect);
                        }
                    }
                    else
                    {
                        // Show the graphic at the source
                        var gd = GrhInfo.GetData(actionDisplay.GrhIndex);
                        if (gd != null)
                        {
                            var grh = new Grh(gd, AnimType.Loop, TickCount.Now);
                            var effect = new MapGrhEffectLoopOnce(grh, source.Center) { MapRenderLayer = MapRenderLayer.SpriteForeground };
                            drawableMap.AddTemporaryMapEffect(effect);
                        }
                    }
                }

                // Show the particle effect
                var pe = ParticleEffectManager.Instance.TryCreateEffect(actionDisplay.ParticleEffect);
                if (pe != null)
                {
                    pe.Position = source.Center;
                    ITemporaryMapEffect effect;

                    if (target != null && target != source)
                    {
                        // Effect that seeks out the position of the target
                        effect = new TemporaryMapParticleEffectSeekPosition(pe, target.Center, 250f) { MapRenderLayer = MapRenderLayer.SpriteForeground };
                    }
                    else
                    {
                        // Effect that takes place at the source
                        effect = new TemporaryMapParticleEffect(pe) { MapRenderLayer = MapRenderLayer.SpriteForeground };
                    }

                    // Add the effect to the map
                    if (effect != null)
                        drawableMap.AddTemporaryMapEffect(effect);
                }
            }
        }

        /// <summary>
        /// Plays the sound for an <see cref="ActionDisplay"/> in a very basic way.
        /// This is intended to be called by the scripts for playing sound instead of handling sound manually.
        /// </summary>
        /// <param name="actionDisplay">The <see cref="ActionDisplay"/>.</param>
        /// <param name="source">The source of the sound.</param>
        static void PlaySoundSimple(ActionDisplay actionDisplay, ISpatial source)
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

        /// <summary>
        /// Removes the <see cref="ITemporaryMapEffect"/>s for the given <see cref="Character"/> from the
        /// <see cref="_activeCastingSkillEffects"/> dictionary. Acquire the <see cref="_activeCastingSkillEffectsSync"/>
        /// lock before calling this!
        /// </summary>
        /// <param name="key">The <see cref="Character"/> to remove the effects for.</param>
        static void RemoveFromActiveCastingSkillEffects(Character key)
        {
            List<ITemporaryMapEffect> value;
            if (!_activeCastingSkillEffects.TryGetValue(key, out value))
            {
                // They were not in the dictionary
                return;
            }

            // Remove from the dictionary
            _activeCastingSkillEffects.Remove(key);

            // Kill each effect
            foreach (var fx in value)
            {
                fx.Kill(false);
            }
        }
    }
}