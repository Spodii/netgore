using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using NetGore.Editor.Grhs;
using NetGore.Graphics;

namespace NetGore.Editor.UI
{
    /// <summary>
    /// Editor form for the <see cref="GrhEditor"/>.
    /// </summary>
    public partial class GrhUITypeEditorForm : Form
    {
        GrhIndex _selected = GrhIndex.Invalid;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="selected">The <see cref="GrhData"/> to select by default. Multiple values are supported.
        /// Can be null.</param>
        public GrhUITypeEditorForm(object selected)
        {
            if (selected == null)
                return;

            if (selected is GrhIndex)
            {
                var s = (GrhIndex)selected;
                _selected = s;
            }
            else if (selected is Grh)
            {
                var s = (Grh)selected;
                if (s.GrhData != null)
                    _selected = s.GrhData.GrhIndex;
            }
            else if (selected is GrhData)
            {
                var s = (GrhData)selected;
                _selected = s.GrhIndex;
            }

            InitializeComponent();
        }

        /// <summary>
        /// Gets the selected <see cref="GrhIndex"/>.
        /// </summary>
        public GrhIndex SelectedValue
        {
            get { return _selected; }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // If we were given an invalid default value, just use whatever the first valid one we can find is
            if (_selected == GrhIndex.Invalid)
            {
                var gd = GrhInfo.GrhDatas.FirstOrDefault();
                if (gd != null)
                    _selected = gd.GrhIndex;
            }
            
            // Load the GrhTreeView
            gtv.InitializeCompact();
            gtv.CollapseAll();
            gtv.SelectedNode = gtv.FindGrhDataNode(GrhInfo.GetData(_selected));
        }

        /// <summary>
        /// Handles the GrhMouseClick event of the gtv control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GrhTreeNodeMouseClickEventArgs"/> instance
        /// containing the event data.</param>
        void gtv_GrhMouseClick(object sender, GrhTreeNodeMouseClickEventArgs e)
        {
            if (e == null || e.GrhData == null)
                return;

            _selected = e.GrhData.GrhIndex;
            Debug.Assert(_selected != GrhIndex.Invalid);
        }

        /// <summary>
        /// Handles the GrhMouseDoubleClick event of the gtv control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GrhTreeNodeMouseClickEventArgs"/> instance containing
        /// the event data.</param>
        void gtv_GrhMouseDoubleClick(object sender, GrhTreeNodeMouseClickEventArgs e)
        {
            if (e == null || e.GrhData == null)
                return;

            _selected = e.GrhData.GrhIndex;
            Debug.Assert(_selected != GrhIndex.Invalid);

            DialogResult = DialogResult.OK;
        }
    }
}