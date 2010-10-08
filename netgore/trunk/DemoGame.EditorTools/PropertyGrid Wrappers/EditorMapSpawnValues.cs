using System.ComponentModel;
using System.Linq;
using DemoGame.Server;

namespace DemoGame.Editor
{
    /// <summary>
    /// Wrapper for the <see cref="MapSpawnValues"/> for using in a property grid.
    /// </summary>
    public class EditorMapSpawnValues
    {
        readonly MapBase _map;
        readonly MapSpawnValues _msv;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorMapSpawnValues"/> class.
        /// </summary>
        /// <param name="msv">The <see cref="MapSpawnValues"/>.</param>
        /// <param name="map">The map.</param>
        public EditorMapSpawnValues(MapSpawnValues msv, MapBase map)
        {
            _msv = msv;
            _map = map;
        }

        /// <summary>
        /// Gets the area on the map the spawning will take place at.
        /// </summary>
        [Browsable(true)]
        [DisplayName("Area")]
        [Description("The area on the map the spawning will take place at.")]
        public MapSpawnRect Area
        {
            get { return _msv.SpawnArea; }
            set { _msv.SetSpawnArea(_map, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="CharacterTemplateID"/> of the <see cref="CharacterTemplate"/> to spawn.
        /// </summary>
        [Browsable(true)]
        [DisplayName("Character")]
        [Description("The ID of the character template to spawn.")]
        public CharacterTemplateID CharacterTemplateID
        {
            get { return _msv.CharacterTemplateID; }
            set { _msv.CharacterTemplateID = value; }
        }

        /// <summary>
        /// Gets the map.
        /// </summary>
        [Browsable(false)]
        public MapBase Map
        {
            get { return _map; }
        }

        /// <summary>
        /// Gets the underlying <see cref="MapSpawnValues"/>.
        /// </summary>
        [Browsable(false)]
        public MapSpawnValues MapSpawnValues
        {
            get { return _msv; }
        }

        /// <summary>
        /// Gets or sets the maximum number of Characters that will be spawned at once.
        /// </summary>
        [Browsable(true)]
        [DisplayName("Amount")]
        [Description("The maximum number of Characters that will be spawned at once.")]
        public byte SpawnAmount
        {
            get { return _msv.SpawnAmount; }
            set { _msv.SpawnAmount = value; }
        }
    }
}