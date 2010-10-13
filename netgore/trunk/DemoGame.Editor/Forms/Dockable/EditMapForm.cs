using System;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using ToolBar = NetGore.Editor.EditorTool.ToolBar;

namespace DemoGame.Editor
{
    /// <summary>
    /// A form that displays a <see cref="Map"/> and provides interactive editing of it.
    /// </summary>
    public sealed partial class EditMapForm : ToolTargetFormBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditMapForm"/> class.
        /// </summary>
        public EditMapForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the <see cref="MapScreenControl"/> for this EditMapForm.
        /// </summary>
        public MapScreenControl MapScreenControl
        {
            get { return mapScreen; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the object that represents the focus of this <see cref="ToolTargetFormBase"/>
        /// and what the <see cref="NetGore.Editor.EditorTool.ToolBar"/> is being displayed for.
        /// </summary>
        /// <returns>
        /// The object that represents what the <see cref="NetGore.Editor.EditorTool.ToolBar"/> is being displayed for.
        /// </returns>
        protected override object GetToolBarObject()
        {
            return MapScreenControl.Map;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            const string confirmMsg = @"Do you wish to save the map ({0}) before closing?
If you do not save, all chances will be lost.";

            // Save?
            var map = mapScreen.Map;
            if (map != null)
            {
                if (MessageBox.Show(string.Format(confirmMsg, map), "Save before closing?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MapHelper.SaveMap(map);
                }
            }

            base.OnClosing(e);
        }

        void MapScreenControl_MapChanged(MapScreenControl sender, Map oldValue, Map newValue)
        {
            // When the map changes, and it was our map that made the ToolBar visibile, then update the reference on the ToolBar

            var tb = ToolBar.GetToolBar(ToolBarVisibility);
            if (tb == null)
                return;

            var dispObj = GetToolBarObject();
            if (dispObj != null && tb.DisplayObject == dispObj)
                tb.DisplayObject = dispObj;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode)
                return;

            MapScreenControl.MapChanged += MapScreenControl_MapChanged;
        }
    }
}