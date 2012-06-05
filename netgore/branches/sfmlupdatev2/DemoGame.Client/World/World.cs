using System;
using System.Linq;
using NetGore;
using NetGore.Features.Emoticons;
using NetGore.Features.Quests;
using NetGore.Graphics;
using NetGore.World;

namespace DemoGame.Client
{
    public class World : WorldBase
    {
        static readonly EmoticonDisplayManager _emoticonDisplayManager = EmoticonDisplayManager.Instance;

        readonly ICamera2D _camera;
        readonly IGetTime _getTime;
        readonly MapDrawingExtensionCollection _mapDrawingExtensions = new MapDrawingExtensionCollection();
        readonly UserInfo _userInfo;

        Map _map;
        MapEntityIndex _usercharIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="World"/> class.
        /// </summary>
        /// <param name="getTime">Interface to get the current time.</param>
        /// <param name="camera">Primary world view camera.</param>
        /// <param name="userInfo">The user info. Can be null.</param>
        public World(IGetTime getTime, ICamera2D camera, UserInfo userInfo)
        {
            _userInfo = userInfo;
            _getTime = getTime;
            _camera = camera;

            MapDrawingExtensions.Add(new EmoticonMapDrawingExtension<Emoticon, EmoticonInfo<Emoticon>>(_emoticonDisplayManager));

            if (userInfo != null)
            {
                Func<QuestID, bool> questStartReqs = x => UserInfo.HasStartQuestRequirements.HasRequirements(x) ?? false;
                Func<QuestID, bool> questFinishReqs =
                    x =>
                    UserInfo.QuestInfo.ActiveQuests.Contains(x) &&
                    (UserInfo.HasFinishQuestRequirements.HasRequirements(x) ?? false);

                var e = new QuestMapDrawingExtension<Character>(userInfo.QuestInfo, questStartReqs, questFinishReqs,
                    m => m.Spatial.GetMany<Character>(m.Camera.GetViewArea(), c => !c.ProvidedQuests.IsEmpty()),
                    c => c.ProvidedQuests)
                {
                    QuestAvailableCanStartIndicator = new Grh(GrhInfo.GetData("Quest", "can start")),
                    QuestStartedIndicator = new Grh(GrhInfo.GetData("Quest", "started")),
                    QuestAvailableCannotStartIndicator = new Grh(GrhInfo.GetData("Quest", "cannot start")),
                    QuestTurnInIndicator = new Grh(GrhInfo.GetData("Quest", "turnin"))
                };

                MapDrawingExtensions.Add(e);
            }
        }

        /// <summary>
        /// Notifies listeners when the map has changed.
        /// </summary>
        public event TypedEventHandler<World, ValueChangedEventArgs<Map>> MapChanged;

        /// <summary>
        /// Gets the camera used for the active view.
        /// </summary>
        public ICamera2D Camera
        {
            get { return _camera; }
        }

        /// <summary>
        /// Gets or sets if the <see cref="UserCharIndex"/> is set.
        /// </summary>
        public bool IsUserCharIndexSet { get; set; }

        /// <summary>
        /// Gets or sets the map currently being used.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        public Map Map
        {
            get { return _map; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                // Check that the map actually needs to change
                if (Map == value)
                    return;

                var oldMap = Map;

                // Invalidate the user's character index until it is reset
                IsUserCharIndexSet = false;

                // Dispose of the old map
                if (Map != null)
                    Map.Dispose();

                // Set the map
                _map = value;
                _camera.Map = Map;

                MapDrawingExtensions.Map = Map;

                // Add the map event hooks to the new map
                if (MapChanged != null)
                    MapChanged.Raise(this, ValueChangedEventArgs.Create(oldMap, Map));
            }
        }

        public MapDrawingExtensionCollection MapDrawingExtensions
        {
            get { return _mapDrawingExtensions; }
        }

        /// <summary>
        /// Gets the user's character, or null if the user's character is not yet set.
        /// </summary>
        public Character UserChar
        {
            get
            {
                if (Map == null || !IsUserCharIndexSet)
                    return null;

                return Map.GetDynamicEntity<Character>(UserCharIndex);
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
                IsUserCharIndexSet = true;
            }
        }

        /// <summary>
        /// Gets the <see cref="UserInfo"/>. Can be null.
        /// </summary>
        public UserInfo UserInfo
        {
            get { return _userInfo; }
        }

        /// <summary>
        /// Draws the world.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        public void Draw(ISpriteBatch sb)
        {
            _map.Draw(sb);
        }

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Current time.</returns>
        public override TickCount GetTime()
        {
            return _getTime.GetTime();
        }

        /// <summary>
        /// When overridden in the derived class, handles updating all of the Maps in this World.
        /// </summary>
        /// <param name="deltaTime">Delta time to use for updating the maps.</param>
        protected override void UpdateMaps(int deltaTime)
        {
            // Update the map
            if (Map != null)
                Map.Update(deltaTime);
        }
    }
}