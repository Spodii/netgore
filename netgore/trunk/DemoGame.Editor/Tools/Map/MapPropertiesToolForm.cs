using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DemoGame.Client;
using NetGore.Editor.Docking;

namespace DemoGame.Editor.Tools
{
    public partial class MapPropertiesToolForm : DockContent
    {
        Map _map;

        /// <summary>
        /// Gets or sets the map to display the properties for.
        /// </summary>
        public Map Map
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

        protected virtual void OnMapChanged(Map oldValue, Map newValue)
        {
            Text = GetFormText(newValue);

            pg.SelectedObject = newValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapPropertiesToolForm"/> class.
        /// </summary>
        public MapPropertiesToolForm()
        {
            InitializeComponent();
        }

        static string GetFormText(Map map)
        {
            return "Map properties: " + (map == null ? "(No map loaded)" : map.ToString());
        }
    }
}
