using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;
using NetGore.EditorTools.Docking.Win32;

namespace NetGore.EditorTools.Docking
{
    public abstract class DockPaneCaptionBase : Control
    {
        readonly DockPane m_dockPane;

        protected internal DockPaneCaptionBase(DockPane pane)
        {
            m_dockPane = pane;

            SetStyle(
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.Selectable, false);
        }

        protected DockPane.AppearanceStyle Appearance
        {
            get { return DockPane.Appearance; }
        }

        protected DockPane DockPane
        {
            get { return m_dockPane; }
        }

        protected bool HasTabPageContextMenu
        {
            get { return DockPane.HasTabPageContextMenu; }
        }

        protected internal abstract int MeasureHeight();

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left && DockPane.DockPanel.AllowEndUserDocking && DockPane.AllowDockDragAndDrop &&
                !DockHelper.IsDockStateAutoHide(DockPane.DockState) && DockPane.ActiveContent != null)
                DockPane.DockPanel.BeginDrag(DockPane);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Right)
                ShowTabPageContextMenu(new Point(e.X, e.Y));
        }

        protected virtual void OnRefreshChanges()
        {
        }

        protected virtual void OnRightToLeftLayoutChanged()
        {
        }

        internal void RefreshChanges()
        {
            if (IsDisposed)
                return;

            OnRefreshChanges();
        }

        protected void ShowTabPageContextMenu(Point position)
        {
            DockPane.ShowTabPageContextMenu(this, position);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)Msgs.WM_LBUTTONDBLCLK)
            {
                if (DockHelper.IsDockStateAutoHide(DockPane.DockState))
                {
                    DockPane.DockPanel.ActiveAutoHideContent = null;
                    return;
                }

                if (DockPane.IsFloat)
                    DockPane.RestoreToPanel();
                else
                    DockPane.Float();
            }
            base.WndProc(ref m);
        }
    }
}