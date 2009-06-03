using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Extensions;
using log4net;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.Extensions;

namespace DemoGame.Server
{
    /// <summary>
    /// Base class for all AI modules. This must be inherited by every AI module. Every inheritor
    /// of the AIBase must include static method that named "CreateInstance" that accepts a Character
    /// and returns an AIBase.
    /// </summary>
    abstract class AIBase
    {
        /// <summary>
        /// Dictionary of each inheritor of the AIBase. The key is the name of the inheriting class
        /// and the value is an instance of the inheritor, created with its private constructor.
        /// </summary>
        static readonly Dictionary<string, CreateInstanceHandler> _aiModules = new Dictionary<string, CreateInstanceHandler>();

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
        /// Static AIBase constructor.
        /// </summary>
        static AIBase()
        {
            // Get all the types in the current assembly that inherit type AIBase
            var inheritors = Assembly.GetEntryAssembly().GetTypes().Where(type => type.IsSubclassOf(typeof(AIBase)));

            // For each inheritor, get a reference to the static method CreateInstance and add it to the dictionary
            foreach (Type inheritor in inheritors)
            {
                CreateInstanceHandler handler = GetCreateInstanceHandler(inheritor);
                _aiModules.Add(inheritor.Name, handler);
            }
        }

        /// <summary>
        /// AIBase constructor.
        /// </summary>
        /// <param name="actor">Character for this AI module to control.</param>
        protected AIBase(Character actor)
        {
            if (actor == null)
                throw new ArgumentNullException("actor");

            _actor = actor;
        }

        /// <summary>
        /// Creates an AI instance from the AI class's name.
        /// </summary>
        /// <param name="aiName">Name of the AI class.</param>
        /// <param name="actor">Character to bind the AI to.</param>
        /// <returns>AI module object instance.</returns>
        public static AIBase CreateInstance(string aiName, Character actor)
        {
            CreateInstanceHandler handler;

            // Get the AI's instance from its name
            if (!_aiModules.TryGetValue(aiName, out handler))
            {
                const string errmsg = "No AI module with the name `{0}` found.";
                throw new ArgumentException(string.Format(errmsg, aiName));
            }

            // Create and return a new instance of the AI
            return handler(actor);
        }

        /// <summary>
        /// Gets the closest Character to this Actor that the Actor is hostile towards (as defined by
        /// the Actor's alliance). Only checks Characters in view of the Actor.
        /// </summary>
        /// <returns>Closest Character this Actor is hostile towards, or null if none in range.</returns>
        protected Character GetClosestHostile()
        {
            Character closestChar = null;
            float closestDist = 0f;

            foreach (NPC character in Actor.Map.NPCs)
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

                float dist = Vector2.Distance(Actor.Position, character.Position);
                if (closestChar == null || closestDist > dist)
                {
                    closestChar = character;
                    closestDist = dist;
                }
            }

            return closestChar;
        }

        /// <summary>
        /// Gets the CreateInstanceHandler for a given type.
        /// </summary>
        /// <param name="type">Type to get the CreateInstanceHandler for.</param>
        /// <returns>CreateInstanceHandler pointed at the CreateInstance method for the type.</returns>
        static CreateInstanceHandler GetCreateInstanceHandler(Type type)
        {
            CreateInstanceHandler handler =
                (CreateInstanceHandler)
                Delegate.CreateDelegate(typeof(CreateInstanceHandler), type, "CreateInstance", false, false);

            // Ensure that the delegate could be created. Because of how annoying it is trying to debug a static
            // constructor, we handle the error manually instead of letting CreateDelegate throw an exception.
            if (handler == null)
            {
                const string errmsg =
                    "Class `{0}` must implement method with signature: " + "static AIBase CreateInstance(Character)";
                Debug.Fail(string.Format(errmsg, type.Name));
                if (log.IsFatalEnabled)
                    log.FatalFormat(errmsg, type.Name);
                throw new MissingMethodException(type.Name, "CreateInstance");
            }

            return handler;
        }

        /// <summary>
        /// Gets a Rectangle defining the melee hit area for the Actor.
        /// </summary>
        /// <returns>A Rectangle defining the melee hit area for the Actor.</returns>
        Rectangle GetMeleeRect()
        {
            return Actor.BodyInfo.GetHitRect(Actor, Actor.BodyInfo.PunchRect);
        }

        /// <summary>
        /// Checks if a given Entity is in the melee range of the Actor.
        /// </summary>
        /// <param name="entity">Entity to check against.</param>
        /// <returns>True if the entity is in melee range of the Actor, else false.</returns>
        protected bool IsInMeleeRange(Entity entity)
        {
            Rectangle hitRect = GetMeleeRect();
            return entity.CB.Intersect(hitRect);
        }

        /// <summary>
        /// Checks if a given Entity is in view of the Actor.
        /// </summary>
        /// <param name="entity">Entity to check against.</param>
        /// <returns>True if the entity is in view of the Actor, else false.</returns>
        protected bool IsInView(Entity entity)
        {
            Vector2 halfScreenSize = GameData.ScreenSize / 2;

            Vector2 dist = Actor.Position - entity.Position;
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
        /// Called once per frame to update the AI.
        /// </summary>
        public abstract void Update();

        delegate AIBase CreateInstanceHandler(Character actor);
    }
}