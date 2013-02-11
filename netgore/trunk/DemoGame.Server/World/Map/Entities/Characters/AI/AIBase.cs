using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.AI;
using log4net;
using NetGore;
using NetGore.AI;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Server
{
    /// <summary>
    /// Base class for all AI modules. This must be inherited by every AI module. Every inheritor
    /// of the AIBase must include static method that named "CreateInstance" that accepts a Character
    /// and returns an AIBase.
    /// </summary>
    public abstract class AIBase : IAI, IGetTime
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// How frequently, in milliseconds, the <see cref="_explicitHostiles"/> collection will be checked for
        /// expired values. A greater value will decrease overhead but result in less accurate expirations.
        /// </summary>
        const int _expireExplicitHostilesRate = 2000;

        static readonly Vector2 _halfScreenSize = GameData.ScreenSize / 2;

        readonly Character _actor;

        /// <summary>
        /// Collection of <see cref="Character"/>s that the <see cref="Actor"/> has explicitly set as being
        /// hostile towards. The key is the <see cref="Character"/> the <see cref="Actor"/> is hostile towards,
        /// and the value is the time at which this hostility will expire.
        /// </summary>
        readonly Dictionary<Character, TickCount> _explicitHostiles = new Dictionary<Character, TickCount>();

        /// <summary>
        /// The time that the <see cref="_explicitHostiles"/> collection will be checked to remove old values.
        /// </summary>
        TickCount _expireExplicitHostilesTime = TickCount.MinValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="AIBase"/> class.
        /// </summary>
        /// <param name="actor">Character for this AI module to control.</param>
        /// <exception cref="ArgumentNullException"><paramref name="actor" /> is <c>null</c>.</exception>
        protected AIBase(Character actor)
        {
            if (actor == null)
                throw new ArgumentNullException("actor");

            _actor = actor;
        }

        /// <summary>
        /// Gets the Character that this AI module controls.
        /// </summary>
        public Character Actor
        {
            get { return _actor; }
        }

        /// <summary>
        /// Gets if the actor can attack the given <see cref="Character"/>.
        /// </summary>
        /// <param name="character">The <see cref="Character"/> to check if the actor can attack.</param>
        /// <returns>True if the actor can attack the <paramref name="character"/>; otherwise false.</returns>
        public virtual bool CanAttack(Character character)
        {
            return Actor.Alliance.CanAttack(character.Alliance);
        }

        /// <summary>
        /// Handles the real updating of the AI.
        /// </summary>
        protected abstract void DoUpdate();

        /// <summary>
        /// Gets the closest Character to this Actor that the Actor is hostile towards (as defined by
        /// the Actor's alliance). Only checks Characters in view of the Actor.
        /// </summary>
        /// <returns>Closest Character this Actor is hostile towards, or null if none in range.</returns>
        public virtual Character GetClosestHostile()
        {
            var visibleArea = GetVisibleMapArea();
            var center = Actor.Center;

            // Get the characters that we are even hostile towards and are in view
            // Invisible players cannot be seen or be targeted
            var possibleChars = Actor.Map.Spatial.GetMany<Character>(visibleArea,
                x => IsValidTarget(x) && IsHostileTowards(x) && x != Actor && !x.Invisible);
            var closest = possibleChars.MinElementOrDefault(x => center.QuickDistance(x.Center));

            return closest;
        }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> describing the visible screen area.
        /// </summary>
        /// <returns>A <see cref="Rectangle"/> describing the visible screen area.</returns>
        public Rectangle GetVisibleMapArea()
        {
            // How large the NPC's view area is in whole
            Vector2 viewSize = GameData.ScreenSize;

            // Get the NPC's area, then inflate it in all directions by half the view area
            Rectangle viewArea = Actor.ToRectangle();
            viewArea.Inflate(viewSize / 2f);

            // Push the view area inside of the map, giving them the same view logic as a user
            if (viewArea.X < 0) viewArea.X = 0;
            if (viewArea.Y < 0) viewArea.Y = 0;
            if (viewArea.Right > Actor.Map.Width) viewArea.X = (int)Actor.Map.Width - viewArea.Width;
            if (viewArea.Bottom > Actor.Map.Height) viewArea.Y = (int)Actor.Map.Height - viewArea.Height;

            return viewArea;
        }

        /// <summary>
        /// Gets if the actor is hostile towards the given <see cref="Character"/>.
        /// </summary>
        /// <param name="character">The character to check if the actor is hostile towards.</param>
        /// <returns>True if the actor is hostile towards the <paramref name="character"/>; otherwise false.</returns>
        public virtual bool IsHostileTowards(Character character)
        {
            var ret = Actor.Alliance.IsHostile(character.Alliance);
            if (!ret)
            {
                if (_explicitHostiles.ContainsKey(character))
                    ret = true;
            }

            return ret;
        }

        /// <summary>
        /// Checks if a given Entity is in the melee range of the Actor.
        /// </summary>
        /// <param name="entity">Entity to check against.</param>
        /// <returns>True if the entity is in melee range of the Actor, else false.</returns>
        protected virtual bool IsInMeleeRange(Entity entity)
        {
            var hitRect = GameData.GetMeleeAttackArea(Actor, Actor.Weapon.Range);
            return entity.Intersects(hitRect);
        }

        /// <summary>
        /// Checks if a given Entity is in view of the Actor.
        /// </summary>
        /// <param name="entity">Entity to check against.</param>
        /// <returns>True if the entity is in view of the Actor, else false.</returns>
        protected virtual bool IsInView(Entity entity)
        {
            var dist = Actor.Center - entity.Center;
            dist = dist.Abs();

            return dist.IsLessThan(_halfScreenSize);
        }

        /// <summary>
        /// Checks if the <paramref name="target"/> is valid.
        /// </summary>
        /// <param name="target">The target <see cref="Character"/>.</param>
        /// <returns>True if the <paramref name="target"/> is valid; otherwise false.</returns>
        protected virtual bool IsValidTarget(Character target)
        {
            if (target == null)
                return false;

            if (!target.IsAlive)
                return false;

            if (target.Map != Actor.Map)
                return false;

            if (target.IsDisposed)
                return false;

            return true;
        }

        /// <summary>
        /// Gets a random number in a given range.
        /// </summary>
        /// <param name="min">Minimum random value.</param>
        /// <param name="max">Maximum random value.</param>
        /// <returns>Random number in the given range.</returns>
        protected static int Rand(int min, int max)
        {
            return RandomHelper.NextInt(min, max);
        }

        /// <summary>
        /// Removes the explicit hostility towards a specific <see cref="Character"/>. Only applies to
        /// hostility set through <see cref="SetHostileTowards"/> and has no affect on hostility created through
        /// the <see cref="Alliance"/>.
        /// </summary>
        /// <param name="target">The <see cref="Character"/> to remove the explicit hostility on.</param>
        /// <returns>True if the explicit hostility towards the <paramref name="target"/> was removed; false if the
        /// hostility towards the <paramref name="target"/> is implicitly defined through the <see cref="Alliance"/>
        /// settings or there was no explicit hostility set on the <paramref name="target"/>.</returns>
        public bool RemoveHostileTowards(Character target)
        {
            if (target == null)
            {
                Debug.Fail("target parameter should not be null.");
                return false;
            }

            return _explicitHostiles.Remove(target);
        }

        /// <summary>
        /// Explicitly sets the AI to be hostile towards a specific <see cref="Character"/> for a set amount
        /// of time. Has no affect if the target <see cref="Character"/>'s <see cref="Alliance"/> already makes
        /// the <see cref="AIBase.Actor"/> hostile towards them, or if they cannot be attacked by the
        /// <see cref="AIBase.Actor"/>.
        /// </summary>
        /// <param name="target">The <see cref="Character"/> to set as hostile towards.</param>
        /// <param name="timeout">How long, in milliseconds, this hostility will last. It is recommended that
        /// this value remains relatively low (5-10 minutes max). Must be greater than 0.</param>
        /// <param name="increaseTimeOnly">If true, the given <paramref name="timeout"/> will not be
        /// used if the <paramref name="target"/> is already marked as being hostile towards, and the
        /// existing timeout is greater than the given <paramref name="timeout"/>. That is, the longer
        /// of the two timeouts will be used.</param>
        public void SetHostileTowards(Character target, TickCount timeout, bool increaseTimeOnly)
        {
            if (target == null)
            {
                Debug.Fail("target parameter should not be null.");
                return;
            }

            if (timeout < 0)
            {
                Debug.Fail("timeout parameter should be greater than 0.");
                return;
            }

            // Ignore characters that are already marked as hostile by the alliance, along with those
            // who cannot be attacked
            if (Actor.Alliance.IsHostile(target.Alliance) || !Actor.Alliance.CanAttack(target.Alliance))
                return;

            // Get the absolute timeout time
            var timeoutTime = GetTime() + timeout;

            if (_explicitHostiles.ContainsKey(target))
            {
                // Target already exists in the list, so update the time
                try
                {
                    if (!increaseTimeOnly)
                        _explicitHostiles[target] = timeoutTime;
                    else
                        _explicitHostiles[target] = Math.Max(timeoutTime, _explicitHostiles[target]);
                }
                catch (KeyNotFoundException ex)
                {
                    const string errmsg = "Possible thread-safety issue in AI's ExplicitHostile. Exception: {0}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, ex);
                    Debug.Fail(string.Format(errmsg, ex));

                    // Retry
                    SetHostileTowards(target, timeoutTime, increaseTimeOnly);
                }
            }
            else
            {
                // Target not in the list yet, so add them
                try
                {
                    _explicitHostiles.Add(target, timeoutTime);
                }
                catch (ArgumentException ex)
                {
                    const string errmsg = "Possible thread-safety issue in AI's ExplicitHostile. Exception: {0}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, ex);
                    Debug.Fail(string.Format(errmsg, ex));

                    // Retry
                    SetHostileTowards(target, timeoutTime, increaseTimeOnly);
                }
            }
        }

        #region IAI Members

        /// <summary>
        /// Gets the <see cref="DynamicEntity"/> that this AI is for.
        /// </summary>
        DynamicEntity IAI.Actor
        {
            get { return Actor; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the ID of this AI.
        /// </summary>
        public abstract AIID ID { get; }

        /// <summary>
        /// When overridden in the derived class, updates the AI. This is called at most once per frame, and only
        /// called whe the <see cref="Actor"/> is alive and active.
        /// </summary>
        public void Update()
        {
            // Ensure a valid actor
            if (!Actor.IsAlive || Actor.Map == null)
                return;

            // Check if AI is enabled
            if (AISettings.AIDisabled)
            {
                if (Actor.IsMoving)
                    Actor.StopMoving();
                _explicitHostiles.Clear();
                return;
            }

            // Check to update the explicit hostiles
            if (_explicitHostiles.Count > 0)
            {
                var time = GetTime();
                if (_expireExplicitHostilesTime < time)
                {
                    _expireExplicitHostilesTime = time + _expireExplicitHostilesRate;
                    _explicitHostiles.RemoveMany(x => x.Value < time || x.Key.IsDisposed);
                }
            }

            // Custom update
            DoUpdate();
        }

        #endregion

        #region IGetTime Members

        /// <summary>
        /// Gets the current time in milliseconds.
        /// </summary>
        /// <returns>The current time in milliseconds.</returns>
        public TickCount GetTime()
        {
            if (Actor != null)
                return Actor.GetTime();

            return 0;
        }

        #endregion
    }
}