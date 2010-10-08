using System;
using System.Linq;
using System.Windows.Forms;
using NetGore.Editor.Docking;

namespace DemoGame.Editor
{
    public partial class SelectedObjectsForm : DockContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedObjectsForm"/> class.
        /// </summary>
        public SelectedObjectsForm()
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