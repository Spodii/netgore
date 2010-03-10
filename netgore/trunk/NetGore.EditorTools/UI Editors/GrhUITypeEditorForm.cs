using System;
using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Editor form for the <see cref="GrhEditor"/>.
    /// </summary>
    public partial class GrhUITypeEditorForm : Form
    {
        readonly Grh _grh;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="grh">The GRH.</param>
        public GrhUITypeEditorForm(Grh grh)
        {
            _grh = grh;

            InitializeComponent();
        }

        /// <summary>
        /// Gets the <see cref="Grh"/> that is being edited.
        /// </summary>
        public Grh Grh
        {
            get { return _grh; }
        }

        /// <summary>
        /// Handles the Load event of the <see cref="GrhUITypeEditorForm"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void GrhUITypeEditorForm_Load(object sender, EventArgs e)
        {
            gtv.InitializeCompact();
            gtv.CollapseAll();
            gtv.SelectedNode = gtv.FindGrhDataNode(Grh.GrhData);
        }

        /// <summary>
        /// Handles the GrhMouseDoubleClick event of the gtv control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NetGore.EditorTools.GrhTreeNodeMouseClickEventArgs"/> instance containing
        /// the event data.</param>
        void gtv_GrhMouseDoubleClick(object sender, GrhTreeNodeMouseClickEventArgs e)
        {
            if (e == null || e.GrhData == null)
                return;

            _grh.SetGrh(e.GrhData, AnimType.Loop, _grh.LastUpdated);

            DialogResult = DialogResult.OK;
        }
    }
}