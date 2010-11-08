using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Windows.Forms;
using NetGore.Editor.Docking.Win32;

namespace NetGore.Editor.Docking
{
    public class FloatWindow : Form, INestedPanesContainer, IDockDragSource
    {
        internal const int WM_CHECKDISPOSE = (int)(Msgs.WM_USER + 1);
        bool m_allowEndUserDocking = true;
        DockPanel m_dockPanel;
        NestedPaneCollection m_nestedPanes;

        protected internal FloatWindow(DockPanel dockPanel, DockPane pane)
        {
            InternalConstruct(dockPanel, pane, false, Rectangle.Empty);
        }

        protected internal FloatWindow(DockPanel dockPanel, DockPane pane, Rectangle bounds)
        {
            InternalConstruct(dockPanel, pane, true, bounds);
        }

        public bool AllowEndUserDocking
        {
            get { return m_allowEndUserDocking; }
            set { m_allowEndUserDocking = value; }
        }

        public DockPanel DockPanel
        {
            get { return m_dockPanel; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DockPanel != null)
                    DockPanel.RemoveFloatWindow(this);
                m_dockPanel = null;
            }
            base.Dispose(disposing);
        }

        void InternalConstruct(DockPanel dockPanel, DockPane pane, bool boundsSpecified, Rectangle bounds)
        {
            if (dockPanel == null)
                throw (new ArgumentNullException(Strings.FloatWindow_Constructor_NullDockPanel));

            m_nestedPanes = new NestedPaneCollection(this);

            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            ShowInTaskbar = false;
            if (dockPanel.RightToLeft != RightToLeft)
                RightToLeft = dockPanel.RightToLeft;
            if (RightToLeftLayout != dockPanel.RightToLeftLayout)
                RightToLeftLayout = dockPanel.RightToLeftLayout;

            SuspendLayout();
            if (boundsSpecified)
            {
                Bounds = bounds;
                StartPosition = FormStartPosition.Manual;
            }
            else
            {
                StartPosition = FormStartPosition.WindowsDefaultLocation;
                Size = dockPanel.DefaultFloatWindowSize;
            }

            m_dockPanel = dockPanel;
            Owner = DockPanel.FindForm();
            DockPanel.AddFloatWindow(this);
            if (pane != null)
                pane.FloatWindow = this;

            ResumeLayout();
        }

        internal bool IsDockStateValid(DockState dockState)
        {
            foreach (var pane in NestedPanes)
            {
                foreach (var content in pane.Contents)
                {
                    if (!DockHelper.IsDockStateValid(dockState, content.DockHandler.DockAreas))
                        return false;
                }
            }

            return true;
        }

        static void MergeNestedPanes(VisibleNestedPaneCollection nestedPanesFrom, NestedPaneCollection nestedPanesTo,
                                     DockPane prevPane, DockAlignment alignment, double proportion)
        {
            if (nestedPanesFrom.Count == 0)
                return;

            var count = nestedPanesFrom.Count;
            var panes = new DockPane[count];
            var prevPanes = new DockPane[count];
            var alignments = new DockAlignment[count];
            var proportions = new double[count];

            for (var i = 0; i < count; i++)
            {
                panes[i] = nestedPanesFrom[i];
                prevPanes[i] = nestedPanesFrom[i].NestedDockingStatus.PreviousPane;
                alignments[i] = nestedPanesFrom[i].NestedDockingStatus.Alignment;
                proportions[i] = nestedPanesFrom[i].NestedDockingStatus.Proportion;
            }

            var pane = panes[0].DockTo(nestedPanesTo.Container, prevPane, alignment, proportion);
            panes[0].DockState = nestedPanesTo.DockState;

            for (var i = 1; i < count; i++)
            {
                for (var j = i; j < count; j++)
                {
                    if (prevPanes[j] == panes[i - 1])
                        prevPanes[j] = pane;
                }
                pane = panes[i].DockTo(nestedPanesTo.Container, prevPanes[i], alignments[i], proportions[i]);
                panes[i].DockState = nestedPanesTo.DockState;
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            DockPanel.FloatWindows.BringWindowToFront(this);
            base.OnActivated(e);
            // Propagate the Activated event to the visible panes content objects
            foreach (var pane in VisibleNestedPanes)
            {
                foreach (var content in pane.Contents)
                {
                    content.OnActivated(e);
                }
            }
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            // Propagate the Deactivate event to the visible panes content objects
            foreach (var pane in VisibleNestedPanes)
            {
                foreach (var content in pane.Contents)
                {
                    content.OnDeactivate(e);
                }
            }
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            VisibleNestedPanes.Refresh();
            RefreshChanges();
            Visible = (VisibleNestedPanes.Count > 0);
            SetText();

            base.OnLayout(levent);
        }

        internal void RefreshChanges()
        {
            if (IsDisposed)
                return;

            if (VisibleNestedPanes.Count == 0)
            {
                ControlBox = true;
                return;
            }

            for (var i = VisibleNestedPanes.Count - 1; i >= 0; i--)
            {
                var contents = VisibleNestedPanes[i].Contents;
                for (var j = contents.Count - 1; j >= 0; j--)
                {
                    var content = contents[j];
                    if (content.DockHandler.DockState != DockState.Float)
                        continue;

                    if (content.DockHandler.CloseButton && content.DockHandler.CloseButtonVisible)
                    {
                        ControlBox = true;
                        return;
                    }
                }
            }
            //Only if there is a ControlBox do we turn it off
            //old code caused a flash of the window.
            if (ControlBox)
                ControlBox = false;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            var rectWorkArea = SystemInformation.VirtualScreen;

            if (y + height > rectWorkArea.Bottom)
                y -= (y + height) - rectWorkArea.Bottom;

            if (y < rectWorkArea.Top)
                y += rectWorkArea.Top - y;

            base.SetBoundsCore(x, y, width, height, specified);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters",
            MessageId = "System.Windows.Forms.Control.set_Text(System.String)")]
        internal void SetText()
        {
            var theOnlyPane = (VisibleNestedPanes.Count == 1) ? VisibleNestedPanes[0] : null;

            if (theOnlyPane == null)
                Text = " ";
                // use " " instead of string.Empty because the whole title bar will disappear when ControlBox is set to false.
            else if (theOnlyPane.ActiveContent == null)
                Text = " ";
            else
                Text = theOnlyPane.ActiveContent.DockHandler.TabText;
        }

        internal void TestDrop(IDockDragSource dragSource, DockOutlineBase dockOutline)
        {
            if (VisibleNestedPanes.Count == 1)
            {
                var pane = VisibleNestedPanes[0];
                if (!dragSource.CanDockTo(pane))
                    return;

                var ptMouse = MousePosition;
                var lParam = Win32Helper.MakeLong(ptMouse.X, ptMouse.Y);
                if (NativeMethods.SendMessage(Handle, (int)Msgs.WM_NCHITTEST, 0, lParam) == (uint)HitTest.HTCAPTION)
                    dockOutline.Show(VisibleNestedPanes[0], -1);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)Msgs.WM_NCLBUTTONDOWN)
            {
                if (IsDisposed)
                    return;

                var result = NativeMethods.SendMessage(Handle, (int)Msgs.WM_NCHITTEST, 0, (uint)m.LParam);
                if (result == 2 && DockPanel.AllowEndUserDocking && AllowEndUserDocking) // HITTEST_CAPTION
                {
                    Activate();
                    m_dockPanel.BeginDrag(this);
                }
                else
                    base.WndProc(ref m);

                return;
            }
            else if (m.Msg == (int)Msgs.WM_NCRBUTTONDOWN)
            {
                var result = NativeMethods.SendMessage(Handle, (int)Msgs.WM_NCHITTEST, 0, (uint)m.LParam);
                if (result == 2) // HITTEST_CAPTION
                {
                    var theOnlyPane = (VisibleNestedPanes.Count == 1) ? VisibleNestedPanes[0] : null;
                    if (theOnlyPane != null && theOnlyPane.ActiveContent != null)
                    {
                        theOnlyPane.ShowTabPageContextMenu(this, PointToClient(MousePosition));
                        return;
                    }
                }

                base.WndProc(ref m);
                return;
            }
            else if (m.Msg == (int)Msgs.WM_CLOSE)
            {
                if (NestedPanes.Count == 0)
                {
                    base.WndProc(ref m);
                    return;
                }

                for (var i = NestedPanes.Count - 1; i >= 0; i--)
                {
                    var contents = NestedPanes[i].Contents;
                    for (var j = contents.Count - 1; j >= 0; j--)
                    {
                        var content = contents[j];
                        if (content.DockHandler.DockState != DockState.Float)
                            continue;

                        if (!content.DockHandler.CloseButton)
                            continue;

                        if (content.DockHandler.HideOnClose)
                            content.DockHandler.Hide();
                        else
                            content.DockHandler.Close();
                    }
                }

                return;
            }
            else if (m.Msg == (int)Msgs.WM_NCLBUTTONDBLCLK)
            {
                var result = NativeMethods.SendMessage(Handle, (int)Msgs.WM_NCHITTEST, 0, (uint)m.LParam);
                if (result != 2) // HITTEST_CAPTION
                {
                    base.WndProc(ref m);
                    return;
                }

                DockPanel.SuspendLayout(true);

                // Restore to panel
                foreach (var pane in NestedPanes)
                {
                    if (pane.DockState != DockState.Float)
                        continue;
                    pane.RestoreToPanel();
                }

                DockPanel.ResumeLayout(true, true);
                return;
            }
            else if (m.Msg == WM_CHECKDISPOSE)
            {
                if (NestedPanes.Count == 0)
                    Dispose();

                return;
            }

            base.WndProc(ref m);
        }

        #region IDockDragSource Members

        Control IDragSource.DragControl
        {
            get { return this; }
        }

        Rectangle IDockDragSource.BeginDrag(Point ptMouse)
        {
            return Bounds;
        }

        bool IDockDragSource.CanDockTo(DockPane pane)
        {
            if (!IsDockStateValid(pane.DockState))
                return false;

            if (pane.FloatWindow == this)
                return false;

            return true;
        }

        public void DockTo(DockPane pane, DockStyle dockStyle, int contentIndex)
        {
            if (dockStyle == DockStyle.Fill)
            {
                for (var i = NestedPanes.Count - 1; i >= 0; i--)
                {
                    var paneFrom = NestedPanes[i];
                    for (var j = paneFrom.Contents.Count - 1; j >= 0; j--)
                    {
                        var c = paneFrom.Contents[j];
                        c.DockHandler.Pane = pane;
                        if (contentIndex != -1)
                            pane.SetContentIndex(c, contentIndex);
                        c.DockHandler.Activate();
                    }
                }
            }
            else
            {
                var alignment = DockAlignment.Left;
                if (dockStyle == DockStyle.Left)
                    alignment = DockAlignment.Left;
                else if (dockStyle == DockStyle.Right)
                    alignment = DockAlignment.Right;
                else if (dockStyle == DockStyle.Top)
                    alignment = DockAlignment.Top;
                else if (dockStyle == DockStyle.Bottom)
                    alignment = DockAlignment.Bottom;

                MergeNestedPanes(VisibleNestedPanes, pane.NestedPanesContainer.NestedPanes, pane, alignment, 0.5);
            }
        }

        public void DockTo(DockPanel panel, DockStyle dockStyle)
        {
            if (panel != DockPanel)
                throw new ArgumentException(Strings.IDockDragSource_DockTo_InvalidPanel, "panel");

            NestedPaneCollection nestedPanesTo = null;

            if (dockStyle == DockStyle.Top)
                nestedPanesTo = DockPanel.DockWindows[DockState.DockTop].NestedPanes;
            else if (dockStyle == DockStyle.Bottom)
                nestedPanesTo = DockPanel.DockWindows[DockState.DockBottom].NestedPanes;
            else if (dockStyle == DockStyle.Left)
                nestedPanesTo = DockPanel.DockWindows[DockState.DockLeft].NestedPanes;
            else if (dockStyle == DockStyle.Right)
                nestedPanesTo = DockPanel.DockWindows[DockState.DockRight].NestedPanes;
            else if (dockStyle == DockStyle.Fill)
                nestedPanesTo = DockPanel.DockWindows[DockState.Document].NestedPanes;

            DockPane prevPane = null;
            for (var i = nestedPanesTo.Count - 1; i >= 0; i--)
            {
                if (nestedPanesTo[i] != VisibleNestedPanes[0])
                    prevPane = nestedPanesTo[i];
            }
            MergeNestedPanes(VisibleNestedPanes, nestedPanesTo, prevPane, DockAlignment.Left, 0.5);
        }

        public void FloatAt(Rectangle floatWindowBounds)
        {
            Bounds = floatWindowBounds;
        }

        bool IDockDragSource.IsDockStateValid(DockState dockState)
        {
            return IsDockStateValid(dockState);
        }

        #endregion

        #region INestedPanesContainer Members

        public virtual Rectangle DisplayingRectangle
        {
            get { return ClientRectangle; }
        }

        public DockState DockState
        {
            get { return DockState.Float; }
        }

        public bool IsFloat
        {
            get { return DockState == DockState.Float; }
        }

        public NestedPaneCollection NestedPanes
        {
            get { return m_nestedPanes; }
        }

        public VisibleNestedPaneCollection VisibleNestedPanes
        {
            get { return NestedPanes.VisibleNestedPanes; }
        }

        #endregion
    }
}