using System;
using System.Linq;
using NetGore.Editor.Docking;

namespace DemoGame.Editor
{
    public partial class SelectedMapObjectsForm : DockContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedMapObjectsForm"/> class.
        /// </summary>
        public SelectedMapObjectsForm()
        {
            InitializeComponent();
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

            var som = GlobalState.Instance.Map.SelectedObjsManager;
            som.SelectedListBox = lstItems;
            som.PropertyGrid = pgSelected;
        }
    }
}