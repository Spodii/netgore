using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;
using NetGore.Graphics;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Features.Emoticons
{
    /// <summary>
    /// Handles displaying the individual emoticon instances.
    /// </summary>
    /// <typeparam name="TKey">The emoticon key.</typeparam>
    /// <typeparam name="TValue">The emoticon information.</typeparam>
    public class EmoticonDisplayManager<TKey, TValue> where TValue : EmoticonInfo<TKey>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// A dictionary of the active emoticons.
        /// </summary>
        readonly Dictionary<ISpatial, EmoticonDisplayInfo> _activeEmoticons = new Dictionary<ISpatial, EmoticonDisplayInfo>();

        readonly EmoticonInfoManagerBase<TKey, TValue> _emoticonInfoManager;

        /// <summary>
        /// Object pool used to spawn <see cref="EmoticonDisplayInfo"/> instances.
        /// </summary>
        readonly ObjectPool<EmoticonDisplayInfo> _objectPool = new ObjectPool<EmoticonDisplayInfo>(
            x => new EmoticonDisplayInfo(), false);

        Func<ISpatial, Vector2> _getDrawPositionHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmoticonDisplayManager{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="emoticonInfoManager">The <see cref="EmoticonInfoManagerBase{TKey, TValue}"/> to use to look up the information
        /// for emoticons.</param>
        public EmoticonDisplayManager(EmoticonInfoManagerBase<TKey, TValue> emoticonInfoManager)
        {
            _emoticonInfoManager = emoticonInfoManager;
        }

        /// <summary>
        /// Gets or sets the <see cref="Func{T,U}"/> used to get the world position to draw an emoticon at for the given
        /// <see cref="ISpatial"/>. If set to null, the default handler will be used.
        /// </summary>
        public Func<ISpatial, Vector2> GetDrawPositionHandler
        {
            get { return _getDrawPositionHandler; }
            set
            {
                if (value == null)
                    value = DefaultGetDrawPosition;

                if (_getDrawPositionHandler == value)
                    return;

                _getDrawPositionHandler = value;
            }
        }

        /// <summary>
        /// Adds an emoticon display.
        /// </summary>
        /// <param name="entity">The entity that emoted.</param>
        /// <param name="emoticon">The emoticon to display.</param>
        /// <param name="currentTime">The current game time.</param>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="emoticon"/> is invalid.</exception>
        public void Add(ISpatial entity, TKey emoticon, TickCount currentTime)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var emoticonInfo = _emoticonInfoManager[emoticon];
            if (emoticonInfo == null)
            {
                const string errmsg = "No EmoticonInfo associated with the emoticon `{0}`.";
                var err = string.Format(errmsg, emoticon);
                if (log.IsErrorEnabled)
                    log.Error(err);
                Debug.Fail(err);
                throw new ArgumentException(err, "emoticon");
            }

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
        /// Gets the position to draw the emoticon at.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to get the draw position for.</param>
        /// <returns>The position to draw the emoticon at.</returns>
        static Vector2 DefaultGetDrawPosition(ISpatial spatial)
        {
            return (spatial.Position + (spatial.Size / 2f)) - new Vector2(6);
        }

        /// <summary>
        /// Draws all of the emoticons in this collection.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        public void Draw(ISpriteBatch spriteBatch)
        {
            foreach (var kvp in _activeEmoticons)
            {
                Vector2 drawPos;
                if (GetDrawPositionHandler != null)
                    drawPos = GetDrawPositionHandler(kvp.Key);
                else
                    drawPos = DefaultGetDrawPosition(kvp.Key);

                kvp.Value.Draw(spriteBatch, drawPos);
            }
        }

        /// <summary>
        /// Updates all of the emoticons in this collection.
        /// </summary>
        /// <param name="currentTime">The current game time.</param>
        public void Update(TickCount currentTime)
        {
            var removeCount = 0;

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
        /// Contains the information for a single display of an emoticon.
        /// </summary>
        class EmoticonDisplayInfo : IPoolable
        {
            /// <summary>
            /// How long a stationary emoticon will live for, in milliseconds.
            /// </summary>
            const int _stationaryEmoticonLife = 1600;

            readonly Grh _grh = new Grh();

            TickCount _initializeTime;
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
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
            /// <param name="position">The world position to draw the emoticon.</param>
            public void Draw(ISpriteBatch spriteBatch, Vector2 position)
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
            public void Initialize(GrhIndex grhIndex, TickCount currentTime)
            {
                _grh.SetGrh(grhIndex, AnimType.LoopOnce, currentTime);
                _initializeTime = currentTime;
                _isAlive = true;
            }

            /// <summary>
            /// Updates the emoticon.
            /// </summary>
            /// <param name="currentTime">The current game time.</param>
            public void Update(TickCount currentTime)
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