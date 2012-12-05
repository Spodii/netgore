using System;
using System.Linq;
using WeifenLuo.WinFormsUI.Docking;

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

            lstItemsUpdateVisibility();
        }

        void lstItemsUpdateVisibility()
        {
            if (lstItems.Items.Count <= 1)
                sc.Panel2Collapsed = true;
            else
                sc.Panel2Collapsed = false;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the <see cref="lstItems"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lstItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstItemsUpdateVisibility();
        }

        /// <summary>
        /// Handles the Tick event of the <see cref="tmrUpdateLstItemsVisibility"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void tmrUpdateLstItemsVisibility_Tick(object sender, EventArgs e)
        {
            lstItemsUpdateVisibility();
        }
    }
}