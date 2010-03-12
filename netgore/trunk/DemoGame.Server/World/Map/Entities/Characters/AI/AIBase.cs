using System;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.AI;

namespace DemoGame.Server
{
    /// <summary>
    /// Base class for all AI modules. This must be inherited by every AI module. Every inheritor
    /// of the AIBase must include static method that named "CreateInstance" that accepts a Character
    /// and returns an AIBase.
    /// </summary>
    public abstract class AIBase : IAI, IGetTime
    {
        static readonly Vector2 _halfScreenSize = GameData.ScreenSize / 2;
        static readonly Random _rand = new Random();

        readonly Character _actor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AIBase"/> class.
        /// </summary>
        /// <param name="actor">Character for this AI module to control.</param>
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
        /// Handles the real updating of the AI.
        /// </summary>
        protected abstract void DoUpdate();

        /// <summary>
        /// Gets the closest Character to this Actor that the Actor is hostile towards (as defined by
        /// the Actor's alliance). Only checks Characters in view of the Actor.
        /// </summary>
        /// <returns>Closest Character this Actor is hostile towards, or null if none in range.</returns>
        protected virtual Character GetClosestHostile()
        {
            var visibleArea = GetVisibleMapArea();
            var center = Actor.Center;

            // Get the characters that we are even hostile towards and are in view
            var possibleChars = Actor.Map.Spatial.GetMany<Character>(visibleArea, x => IsHostileTowards(x) && x != Actor);
            var closest = possibleChars.MinElementOrDefault(x => center.QuickDistance(x.Center));

            return closest;
        }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> describing the visible screen area.
        /// </summary>
        /// <returns>A <see cref="Rectangle"/> describing the visible screen area.</returns>
        protected Rectangle GetVisibleMapArea()
        {
            var center = Actor.Center;
            var min = center - _halfScreenSize;

            int x = (int)Math.Max(0, min.X);
            int y = (int)Math.Max(0, min.Y);
            int w = (int)Math.Max(Actor.Map.Width, min.X + GameData.ScreenSize.X);
            int h = (int)Math.Max(Actor.Map.Height, min.Y + GameData.ScreenSize.Y);

            return new Rectangle(x, y, w, h);
        }

        /// <summary>
        /// Gets if the actor is hostile towards the given <see cref="Character"/>.
        /// </summary>
        /// <param name="character">The character to check if the actor is hostile towards.</param>
        /// <returns>True if the actor is hostile towards the <paramref name="character"/>; otherwise false.</returns>
        protected virtual bool IsHostileTowards(Character character)
        {
            return Actor.Alliance.IsHostile(character.Alliance);
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
            return _rand.Next(min, max);
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
            if (!Actor.IsAlive || Actor.Map == null)
                return;

            DoUpdate();
        }

        #endregion

        #region IGetTime Members

        /// <summary>
        /// Gets the current time in milliseconds.
        /// </summary>
        /// <returns>The current time in milliseconds.</returns>
        public int GetTime()
        {
            if (Actor != null)
                return Actor.GetTime();

            return 0;
        }

        #endregion
    }
}