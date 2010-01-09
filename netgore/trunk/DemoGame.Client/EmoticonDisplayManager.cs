using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Collections;
using NetGore.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Handles displaying <see cref="Emoticon"/>s.
    /// </summary>
    public class EmoticonDisplayManager
    {
        static readonly EmoticonInfoManager _emoticonInfoManager;
        static readonly EmoticonDisplayManager _instance;

        /// <summary>
        /// A dictionary of the active emoticons.
        /// </summary>
        readonly Dictionary<ISpatial, EmoticonDisplayInfo> _activeEmoticons = new Dictionary<ISpatial, EmoticonDisplayInfo>();

        /// <summary>
        /// Object pool used to spawn <see cref="EmoticonDisplayInfo"/> instances.
        /// </summary>
        readonly ObjectPool<EmoticonDisplayInfo> _objectPool = new ObjectPool<EmoticonDisplayInfo>(
            x => new EmoticonDisplayInfo(), false);

        /// <summary>
        /// Initializes the <see cref="EmoticonDisplayManager"/> class.
        /// </summary>
        static EmoticonDisplayManager()
        {
            _instance = new EmoticonDisplayManager();
            _emoticonInfoManager = EmoticonInfoManager.Instance;

            Debug.Assert(_emoticonInfoManager != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmoticonDisplayManager"/> class.
        /// </summary>
        EmoticonDisplayManager()
        {
        }

        /// <summary>
        /// Gets the <see cref="EmoticonDisplayManager"/> instance.
        /// </summary>
        public static EmoticonDisplayManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Adds an <see cref="Emoticon"/> display.
        /// </summary>
        /// <param name="entity">The entity that emoted.</param>
        /// <param name="emoticon">The emoticon to display.</param>
        /// <param name="currentTime">The current game time.</param>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> is null.</exception>
        public void Add(ISpatial entity, Emoticon emoticon, int currentTime)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var emoticonInfo = _emoticonInfoManager.GetAttribute(emoticon);
            EmoticonDisplayInfo obj;
            bool keyExists;

            // If an emoticon already exists, reuse that. Otherwise, get a new one from the pool.
            if (_activeEmoticons.ContainsKey(entity))
            {
                obj = _activeEmoticons[entity];
                keyExists = true;
            }
            else
            {
                obj = _objectPool.Acquire();
                keyExists = false;
            }

            // Initialize
            obj.Initialize(emoticonInfo.GrhIndex, currentTime);

            // Add to the dictionary of active emoticons
            if (keyExists)
                _activeEmoticons[entity] = obj;
            else
                _activeEmoticons.Add(entity, obj);
        }

        /// <summary>
        /// Draws the emoticons in the <see cref="EmoticonDisplayManager"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw to.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var kvp in _activeEmoticons)
            {
                var drawPos = GetDrawPosition(kvp.Key);
                kvp.Value.Draw(spriteBatch, drawPos);
            }
        }

        /// <summary>
        /// Gets the position to draw the emoticon at.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to get the draw position for.</param>
        /// <returns>The position to draw the emoticon at.</returns>
        static Vector2 GetDrawPosition(ISpatial spatial)
        {
            return (spatial.Position + (spatial.Size / 2f)) - new Vector2(6);
        }

        /// <summary>
        /// Updates the emoticons in the <see cref="EmoticonDisplayManager"/>.
        /// </summary>
        /// <param name="currentTime">The current game time.</param>
        public void Update(int currentTime)
        {
            int removeCount = 0;

            // Update all the living emoticons
            foreach (var value in _activeEmoticons.Values)
            {
                value.Update(currentTime);
                if (!value.IsAlive)
                    removeCount++;
            }

            // If we counted any dead emoticons, remove them
            if (removeCount > 0)
            {
                var toRemove = _activeEmoticons.Where(x => !x.Value.IsAlive).ToImmutable();
                foreach (var kvp in toRemove)
                {
                    _activeEmoticons.Remove(kvp.Key);
                    _objectPool.Free(kvp.Value);
                }
            }
        }

        /// <summary>
        /// Contains the information for a single display of an <see cref="Emoticon"/>.
        /// </summary>
        class EmoticonDisplayInfo : IPoolable
        {
            /// <summary>
            /// How long a stationary emoticon will live for, in milliseconds.
            /// </summary>
            const int _stationaryEmoticonLife = 1600;

            readonly Grh _grh = new Grh();

            int _initializeTime;

            bool _isAlive;

            /// <summary>
            /// Gets if the <see cref="EmoticonDisplayInfo"/> is still alive. If false, the emoticon will no longer
            /// display.
            /// </summary>
            public bool IsAlive
            {
                get { return _isAlive; }
            }

            /// <summary>
            /// Gets if this object is in a valid state to update and draw.
            /// </summary>
            bool IsValidStateToUse
            {
                get { return _grh != null && _grh.GrhData != null && IsAlive; }
            }

            /// <summary>
            /// Draws the emoticon.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw to.</param>
            /// <param name="position">The world position to draw the emoticon.</param>
            public void Draw(SpriteBatch spriteBatch, Vector2 position)
            {
                // Check for a valid state
                if (!IsValidStateToUse)
                    return;

                _grh.Draw(spriteBatch, position);
            }

            /// <summary>
            /// Sets up the <see cref="EmoticonDisplayInfo"/>.
            /// </summary>
            /// <param name="grhIndex">The <see cref="GrhIndex"/> of the emoticon.</param>
            /// <param name="currentTime">The current game time.</param>
            public void Initialize(GrhIndex grhIndex, int currentTime)
            {
                _grh.SetGrh(grhIndex, AnimType.LoopOnce, currentTime);
                _initializeTime = currentTime;
                _isAlive = true;
            }

            /// <summary>
            /// Updates the emoticon.
            /// </summary>
            /// <param name="currentTime">The current game time.</param>
            public void Update(int currentTime)
            {
                // Check for a valid state
                if (!IsValidStateToUse)
                    return;

                // Update the sprite
                _grh.Update(currentTime);

                // Check if the emoticon is still alive
                if (_grh.GrhData.FramesCount <= 1)
                {
                    // For stationary, check that the target amount of time has elapsed
                    if (currentTime > _initializeTime + _stationaryEmoticonLife)
                        _isAlive = false;
                }
                else
                {
                    // If animated, check if the animation is still going
                    _isAlive = (_grh.AnimType == AnimType.LoopOnce);
                }
            }

            #region IPoolable Members

            /// <summary>
            /// Gets or sets the index of the object in the pool. This value should never be used by anything
            /// other than the pool that owns this object.
            /// </summary>
            int IPoolable.PoolIndex { get; set; }

            #endregion
        }
    }
}