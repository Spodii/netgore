using System;
using System.Diagnostics;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;

namespace DemoGame.Client
{
    public class World : WorldBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Camera2D _camera;

        /// <summary>
        /// Interface used to get the current time
        /// </summary>
        readonly IGetTime _getTime;

        /// <summary>
        /// Map currently being used
        /// </summary>
        Map _map;

        MapEntityIndex _usercharIndex;

        /// <summary>
        /// Gets the camera used for the active view.
        /// </summary>
        public Camera2D Camera
        {
            get { return _camera; }
        }

        /// <summary>
        /// Gets or sets the map currently being used.
        /// </summary>
        public Map Map
        {
            get { return _map; }
            set
            {
                _map = value;
                _camera.Map = _map;
            }
        }

        /// <summary>
        /// Gets the user's character given the UserCharIndex
        /// </summary>
        public Character UserChar
        {
            get
            {
                if (_map == null || !UserCharIndexSet)
                    return null;

                return _map.GetDynamicEntity<Character>(UserCharIndex);
            }
        }

        /// <summary>
        /// Gets or sets the MapEntityIndex of the Character that belongs to the client's character.
        /// </summary>
        public MapEntityIndex UserCharIndex
        {
            get { return _usercharIndex; }
            set
            {
                _usercharIndex = value;
                UserCharIndexSet = true;
            }
        }

        /// <summary>
        /// Gets or sets if the UserCharIndex is set.
        /// </summary>
        public bool UserCharIndexSet { get; set; }

        /// <summary>
        /// World constructor
        /// </summary>
        /// <param name="getTime">Interface to get the current time</param>
        /// <param name="camera">Primary world view camera</param>
        public World(IGetTime getTime, Camera2D camera)
        {
            _getTime = getTime;
            _camera = camera;
        }

        /// <summary>
        /// Draws the world
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        public void Draw(SpriteBatch sb)
        {
            _map.Draw(sb, Camera);
        }

        /// <summary>
        /// Gets the current time
        /// </summary>
        /// <returns>Current time</returns>
        public override int GetTime()
        {
            return _getTime.GetTime();
        }

        public void SetMap(Map newMap)
        {
            if (newMap == null)
                throw new ArgumentNullException("newMap");

            // Check that the map actually needs to change
            if (Map == newMap)
            {
                const string errmsg = "Requested to change map to the map we are already on.";
                Debug.Fail(errmsg);
                if (log.IsWarnEnabled)
                    log.Warn(errmsg);
                return;
            }

            // Invalidate the user's character index until it is reset
            UserCharIndexSet = false;

            // Dispose of the old map
            if (Map != null)
                Map.Dispose();

            // Set the new map
            Map = newMap;
        }

        protected override void UpdateMaps(int deltaTime)
        {
            // Update the map
            if (_map != null)
                _map.Update(deltaTime);
        }
    }
}