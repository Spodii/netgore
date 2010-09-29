using System;
using System.Linq;
using System.Windows.Forms;

namespace DemoGame.Editor.Forms
{
    public partial class SelectedMapObjectsForm : Form
    {
        static readonly SelectedMapObjectsForm _instance;

        /// <summary>
        /// Initializes the <see cref="SelectedMapObjectsForm"/> class.
        /// </summary>
        static SelectedMapObjectsForm()
        {
            _instance = new SelectedMapObjectsForm();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedMapObjectsForm"/> class.
        /// </summary>
        SelectedMapObjectsForm()
        {
            InitializeComponent();
        }

        public static SelectedMapObjectsForm Instance
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