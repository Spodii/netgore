using System;
using System.Diagnostics;
using System.Linq;
using DemoGame.Editor.Tools;
using WeifenLuo.WinFormsUI.Docking;
using NetGore.Editor.EditorTool;
using NetGore.Editor.Grhs;
using NetGore;
using NetGore.Graphics;

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

            GlobalState.Instance.Map.GrhToPlaceChanged -= Map_GrhToPlaceChanged;
            GlobalState.Instance.Map.GrhToPlaceChanged += Map_GrhToPlaceChanged;

#pragma warning disable 162
            if (GrhTreeView.EnableGrhEditor)
            {
                gtv.EditGrhDataRequested -= gtv_EditGrhDataRequested;
                gtv.EditGrhDataRequested += gtv_EditGrhDataRequested;
            }
#pragma warning restore 162
        }

        void Map_GrhToPlaceChanged(object sender, EventArgs e)
        {
            try
            {
                Grh grh = GlobalState.Instance.Map.GrhToPlace;
                if (grh != null && grh.GrhData != null)
                {
                    GrhTreeViewNode newNode = gtv.FindGrhDataNode(grh.GrhData);
                    if (gtv.SelectedNode != newNode)
                    {
                        gtv.SelectedNode = newNode;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
            }
        }

        static void gtv_EditGrhDataRequested(GrhTreeView sender, GrhTreeViewEditGrhDataEventArgs e)
        {
            if (e.GrhData == null)
                return;

            var frm = new EditGrhForm(e.GrhData, GlobalState.Instance.MapGrhWalls, (pos, size) => new WallEntity(pos, size), e.DeleteOnCancel);
            frm.Show();
        }

        void gtv_GrhAfterSelect(object sender, GrhTreeViewEventArgs e)
        {
            GlobalState.Instance.Map.SetGrhToPlace(e.GrhData.GrhIndex);

            var toolManager = ToolManager.Instance;

            var pencilTool = toolManager.TryGetTool<MapGrhPencilTool>();
            if (pencilTool != null && pencilTool.IsOnToolBar)
            {
                var fillTool = toolManager.TryGetTool<MapGrhFillTool>();
                if (fillTool == null || !fillTool.IsEnabled)
                {
                    pencilTool.IsEnabled = true;
                }
            }
        }

        private void filterTxt_TextChanged(object sender, EventArgs e)
        {
            gtv.Filter = filterTxt.Text;
        }

        private void gtv_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (!e.Shift && !e.Alt)
            {
                int? num = e.KeyCode.GetNumericKeyAsValue();
                if (num.HasValue && num.Value > 0 && num.Value < 10)
                {
                    if (e.Control)
                    {
                        // Set hotkey
                        GrhTreeViewNode node = gtv.SelectedNode as GrhTreeViewNode;
                        if (node != null && node.GrhData != null)
                        {
                            GlobalState.Instance.HotkeyedGrhs[num.Value] = node.GrhData.Categorization.ToString();
                        }
                    }
                    else
                    {
                        // Use hotkey
                        GlobalState.Instance.SetGrhFromHotkey(num.Value);
                    }
                }
            }
        }
    }
}