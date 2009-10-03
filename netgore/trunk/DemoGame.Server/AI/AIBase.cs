using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame;
using log4net;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.Server
{
    /// <summary>
    /// Base class for all AI modules. This must be inherited by every AI module. Every inheritor
    /// of the AIBase must include static method that named "CreateInstance" that accepts a Character
    /// and returns an AIBase.
    /// </summary>
    public abstract class AIBase
    {
        static readonly Random _rand = new Random();

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Character _actor;

        /// <summary>
        /// Gets the Character that this AI module controls.
        /// </summary>
        public Character Actor
        {
            get { return _actor; }
        }

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
        /// Gets the closest Character to this Actor that the Actor is hostile towards (as defined by
        /// the Actor's alliance). Only checks Characters in view of the Actor.
        /// </summary>
        /// <returns>Closest Character this Actor is hostile towards, or null if none in range.</returns>
        protected Character GetClosestHostile()
        {
            Character closestChar = null;
            var closestDist = 0f;

            foreach (var character in Actor.Map.NPCs)
            {
                if (character == null)
                {
                    const string errmsg =
                        "Shouldn't even run into a null Character from Actor.Map.Characters since the underlying " +
                        "type (DArray or List) should not return removed elements when enumerating.";
                    if (log.IsErrorEnabled)
                        log.Error(errmsg);
                    Debug.Fail(errmsg);
                    continue;
                }

                if (!Actor.Alliance.IsHostile(character.Alliance) || !IsInView(character))
                    continue;

                var dist = Vector2.Distance(Actor.Position, character.Position);
                if (closestChar == null || closestDist > dist)
                {
                    closestChar = character;
                    closestDist = dist;
                }
            }

            return closestChar;
        }

        /// <summary>
        /// Gets a Rectangle defining the melee hit area for the Actor.
        /// </summary>
        /// <returns>A Rectangle defining the melee hit area for the Actor.</returns>
        protected Rectangle GetMeleeRect()
        {
            return BodyInfo.GetHitRect(Actor, Actor.BodyInfo.PunchRect);
        }

        /// <summary>
        /// Checks if a given Entity is in the melee range of the Actor.
        /// </summary>
        /// <param name="entity">Entity to check against.</param>
        /// <returns>True if the entity is in melee range of the Actor, else false.</returns>
        protected bool IsInMeleeRange(Entity entity)
        {
            var hitRect = GetMeleeRect();
            return entity.CB.Intersect(hitRect);
        }

        /// <summary>
        /// Checks if a given Entity is in view of the Actor.
        /// </summary>
        /// <param name="entity">Entity to check against.</param>
        /// <returns>True if the entity is in view of the Actor, else false.</returns>
        protected bool IsInView(Entity entity)
        {
            var halfScreenSize = GameData.ScreenSize / 2;

            var dist = Actor.Position - entity.Position;
            dist = dist.Abs();

            return dist.IsLessThan(halfScreenSize);
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

        /// <summary>
        /// When overridden in the derived class, updates the AI. This is called at most once per frame, and only
        /// called whe the <see cref="Actor"/> is alive and active.
        /// </summary>
        public abstract void Update();
    }
}