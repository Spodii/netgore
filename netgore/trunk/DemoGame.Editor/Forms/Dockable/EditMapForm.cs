using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.Editor.Properties;
using NetGore;
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
        /// Notifies listeners when a <see cref="EditMapForm"/> is loaded.
        /// </summary>
        public static event EventHandler FormLoaded;

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
        /// Handles the corresponding event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ValueChangedEventArgs{EditorMap}"/> instance containing the event data.</param>
        void MapScreenControl_MapChanged(MapScreenControl sender, ValueChangedEventArgs<EditorMap> e)
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
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            const string confirmMsg =
                @"Do you wish to save the map ({0}) before closing?
If you do not save, all changes will be lost.";

            // Save?
            var map = mapScreen.Map;
            if (map != null && MapHelper.DiffersFromSaved(map))
            {
                var result = MessageBox.Show(string.Format(confirmMsg, map), "Save before closing?", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (result == DialogResult.Yes)
                {
                    MapHelper.SaveMap(map);
                }
            }

            base.OnClosing(e);
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

            MapScreenControl.MapChanged -= MapScreenControl_MapChanged;
            MapScreenControl.MapChanged += MapScreenControl_MapChanged;

            MapScreenControl.Disposed -= MapScreenControl_Disposed;
            MapScreenControl.Disposed += MapScreenControl_Disposed;
            
            if (FormLoaded != null)
                FormLoaded.Raise(this, e);
        }

        /// <summary>
        /// Handles the Disposed event of the MapScreenControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void MapScreenControl_Disposed(object sender, EventArgs e)
        {
            // When the MapScreenControl disposes, also close down the form
            Close();
        }

        private void mapScreen_MouseUp(object sender, MouseEventArgs e)
        {
            // Take an undo snapshot when the mouse button is pressed
            MapScreenControl.Map.UndoManager.Snapshot();
        }

        private void mapScreen_MouseDown(object sender, MouseEventArgs e)
        {
            // Take an undo snapshot when the mouse button is released
            MapScreenControl.Map.UndoManager.Snapshot();
        }

        private void mapScreen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.Z)
                {
                    MapScreenControl.Map.UndoManager.Undo();
                }
                else if (e.KeyCode == Keys.Y)
                {
                    MapScreenControl.Map.UndoManager.Redo();
                }
            }
        }

        private void mapScreen_Enter(object sender, EventArgs e)
        {
            if (MapScreenControl != null && MapScreenControl.Map != null && MapScreenControl.Map.ID > 0)
            {
                var settings = EditorSettings.Default;
                settings.InitialMapId = (int)MapScreenControl.Map.ID;
                settings.Save();
            }
        }
    }
}