using System;
using System.Linq;
using NetGore.Editor.Docking;
using NetGore.Editor.Grhs;

namespace DemoGame.Editor
{
    public partial class GrhTreeViewForm : DockContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrhTreeViewForm"/> class.
        /// </summary>
        public GrhTreeViewForm()
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

            gtv.Initialize(GlobalState.Instance.ContentManager);
            gtv.EditGrhDataRequested += gtv_EditGrhDataRequested;
        }

        void gtv_EditGrhDataRequested(GrhTreeView sender, System.Windows.Forms.TreeNode node, NetGore.Graphics.GrhData gd, bool deleteOnCancel)
        {
            if (gd == null)
                return;

            var frm = new EditGrhForm(gd, GlobalState.Instance.MapGrhWalls, (pos, size) => new WallEntity(pos, size), deleteOnCancel);
            frm.Show();
        }

        void gtv_GrhAfterSelect(object sender, GrhTreeViewEventArgs e)
        {
            GlobalState.Instance.Map.GrhToPlace.SetGrh(e.GrhData);
        }
    }
}