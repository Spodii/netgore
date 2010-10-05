using System;
using System.Linq;
using System.Windows.Forms;
using NetGore.Editor.Docking;

namespace DemoGame.Editor
{
    public partial class SelectedObjectsForm : DockContent
    {
        static readonly SelectedObjectsForm _instance;

        /// <summary>
        /// Initializes the <see cref="SelectedObjectsForm"/> class.
        /// </summary>
        static SelectedObjectsForm()
        {
            _instance = new SelectedObjectsForm();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedObjectsForm"/> class.
        /// </summary>
        SelectedObjectsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the <see cref="SelectedObjectsForm"/> instance.
        /// </summary>
        public static SelectedObjectsForm Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var som = GlobalState.Instance.Map.SelectedObjsManager;
            som.SelectedListBox = lstItems;
            som.PropertyGrid = pgSelected;
        }
    }
}