using System.Linq;
using WeifenLuo.WinFormsUI.Docking;

namespace DemoGame.Editor.Tools
{
    public partial class MapPropertiesToolForm : DockContent
    {
        EditorMap _map;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapPropertiesToolForm"/> class.
        /// </summary>
        public MapPropertiesToolForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the map to display the properties for.
        /// </summary>
        public EditorMap Map
        {
            get { return _map; }
            set
            {
                if (_map == value)
                    return;

                var oldValue = _map;

                _map = value;

                OnMapChanged(oldValue, value);
            }
        }

        static string GetFormText(EditorMap map)
        {
            return "Map properties: " + (map == null ? "(No map loaded)" : map.ToString());
        }

        protected virtual void OnMapChanged(EditorMap oldValue, EditorMap newValue)
        {
            Text = GetFormText(newValue);

            pg.SelectedObject = newValue;
        }
    }
}