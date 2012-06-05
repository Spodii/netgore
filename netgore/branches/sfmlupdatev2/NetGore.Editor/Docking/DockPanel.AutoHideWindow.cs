using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor.Docking
{
    partial class DockPanel
    {
        internal Control AutoHideControl
        {
            get { return m_autoHideWindow; }
        }

        AutoHideWindowControl AutoHideWindow
        {
            get { return m_autoHideWindow; }
        }

        internal Rectangle AutoHideWindowRectangle
        {
            get
            {
                var state = AutoHideWindow.DockState;
                var rectDockArea = DockArea;
                if (ActiveAutoHideContent == null)
                    return Rectangle.Empty;

                if (Parent == null)
                    return Rectangle.Empty;

                var rect = Rectangle.Empty;
                var autoHideSize = ActiveAutoHideContent.DockHandler.AutoHidePortion;
                if (state == DockState.DockLeftAutoHide)
                {
                    if (autoHideSize < 1)
                        autoHideSize = rectDockArea.Width * autoHideSize;
                    if (autoHideSize > rectDockArea.Width - MeasurePane.MinSize)
                        autoHideSize = rectDockArea.Width - MeasurePane.MinSize;
                    rect.X = rectDockArea.X;
                    rect.Y = rectDockArea.Y;
                    rect.Width = (int)autoHideSize;
                    rect.Height = rectDockArea.Height;
                }
                else if (state == DockState.DockRightAutoHide)
                {
                    if (autoHideSize < 1)
                        autoHideSize = rectDockArea.Width * autoHideSize;
                    if (autoHideSize > rectDockArea.Width - MeasurePane.MinSize)
                        autoHideSize = rectDockArea.Width - MeasurePane.MinSize;
                    rect.X = rectDockArea.X + rectDockArea.Width - (int)autoHideSize;
                    rect.Y = rectDockArea.Y;
                    rect.Width = (int)autoHideSize;
                    rect.Height = rectDockArea.Height;
                }
                else if (state == DockState.DockTopAutoHide)
                {
                    if (autoHideSize < 1)
                        autoHideSize = rectDockArea.Height * autoHideSize;
                    if (autoHideSize > rectDockArea.Height - MeasurePane.MinSize)
                        autoHideSize = rectDockArea.Height - MeasurePane.MinSize;
                    rect.X = rectDockArea.X;
                    rect.Y = rectDockArea.Y;
                    rect.Width = rectDockArea.Width;
                    rect.Height = (int)autoHideSize;
                }
                else if (state == DockState.DockBottomAutoHide)
                {
                    if (autoHideSize < 1)
                        autoHideSize = rectDockArea.Height * autoHideSize;
                    if (autoHideSize > rectDockArea.Height - MeasurePane.MinSize)
                        autoHideSize = rectDockArea.Height - MeasurePane.MinSize;
                    rect.X = rectDockArea.X;
                    rect.Y = rectDockArea.Y + rectDockArea.Height - (int)autoHideSize;
                    rect.Width = rectDockArea.Width;
                    rect.Height = (int)autoHideSize;
                }

                return rect;
            }
        }

        internal Rectangle GetAutoHideWindowBounds(Rectangle rectAutoHideWindow)
        {
            if (DocumentStyle == DocumentStyle.SystemMdi || DocumentStyle == DocumentStyle.DockingMdi)
                return (Parent == null) ? Rectangle.Empty : Parent.RectangleToClient(RectangleToScreen(rectAutoHideWindow));
            else
                return rectAutoHideWindow;
        }

        internal void RefreshActiveAutoHideContent()
        {
            AutoHideWindow.RefreshActiveContent();
        }

        internal void RefreshAutoHideStrip()
        {
            AutoHideStripControl.RefreshChanges();
        }

        class AutoHideWindowControl : Panel, ISplitterDragSource
        {
            readonly DockPanel m_dockPanel = null;
            readonly SplitterControl m_splitter;
            readonly Timer m_timerMouseTrack;

            IDockContent m_activeContent = null;
            DockPane m_activePane = null;
            bool m_flagAnimate = true;
            bool m_flagDragging = false;

            public AutoHideWindowControl(DockPanel dockPanel)
            {
                m_dockPanel = dockPanel;

                m_timerMouseTrack = new Timer();
                m_timerMouseTrack.Tick += TimerMouseTrack_Tick;

                Visible = false;
                m_splitter = new SplitterControl(this);
                Controls.Add(m_splitter);
            }

            public IDockContent ActiveContent
            {
                get { return m_activeContent; }
                set
                {
                    if (value == m_activeContent)
                        return;

                    if (value != null)
                    {
                        if (!DockHelper.IsDockStateAutoHide(value.DockHandler.DockState) ||
                            value.DockHandler.DockPanel != DockPanel)
                            throw (new InvalidOperationException(Strings.DockPanel_ActiveAutoHideContent_InvalidValue));
                    }

                    DockPanel.SuspendLayout();

                    if (m_activeContent != null)
                    {
                        if (m_activeContent.DockHandler.Form.ContainsFocus)
                            DockPanel.ContentFocusManager.GiveUpFocus(m_activeContent);
                        AnimateWindow(false);
                    }

                    m_activeContent = value;
                    SetActivePane();
                    if (ActivePane != null)
                        ActivePane.ActiveContent = m_activeContent;

                    if (m_activeContent != null)
                        AnimateWindow(true);

                    DockPanel.ResumeLayout();
                    DockPanel.RefreshAutoHideStrip();

                    SetTimerMouseTrack();
                }
            }

            public DockPane ActivePane
            {
                get { return m_activePane; }
            }

            protected virtual Rectangle DisplayingRectangle
            {
                get
                {
                    var rect = ClientRectangle;

                    // exclude the border and the splitter
                    if (DockState == DockState.DockBottomAutoHide)
                    {
                        rect.Y += 2 + Measures.SplitterSize;
                        rect.Height -= 2 + Measures.SplitterSize;
                    }
                    else if (DockState == DockState.DockRightAutoHide)
                    {
                        rect.X += 2 + Measures.SplitterSize;
                        rect.Width -= 2 + Measures.SplitterSize;
                    }
                    else if (DockState == DockState.DockTopAutoHide)
                        rect.Height -= 2 + Measures.SplitterSize;
                    else if (DockState == DockState.DockLeftAutoHide)
                        rect.Width -= 2 + Measures.SplitterSize;

                    return rect;
                }
            }

            public DockPanel DockPanel
            {
                get { return m_dockPanel; }
            }

            public DockState DockState
            {
                get { return ActiveContent == null ? DockState.Unknown : ActiveContent.DockHandler.DockState; }
            }

            bool FlagAnimate
            {
                get { return m_flagAnimate; }
                set { m_flagAnimate = value; }
            }

            internal bool FlagDragging
            {
                get { return m_flagDragging; }
                set
                {
                    if (m_flagDragging == value)
                        return;

                    m_flagDragging = value;
                    SetTimerMouseTrack();
                }
            }

            void AnimateWindow(bool show)
            {
                if (!FlagAnimate && Visible != show)
                {
                    Visible = show;
                    return;
                }

                Parent.SuspendLayout();

                var rectSource = GetRectangle(!show);
                var rectTarget = GetRectangle(show);
                int dxLoc, dyLoc;
                int dWidth, dHeight;
                dxLoc = dyLoc = dWidth = dHeight = 0;
                if (DockState == DockState.DockTopAutoHide)
                    dHeight = show ? 1 : -1;
                else if (DockState == DockState.DockLeftAutoHide)
                    dWidth = show ? 1 : -1;
                else if (DockState == DockState.DockRightAutoHide)
                {
                    dxLoc = show ? -1 : 1;
                    dWidth = show ? 1 : -1;
                }
                else if (DockState == DockState.DockBottomAutoHide)
                {
                    dyLoc = (show ? -1 : 1);
                    dHeight = (show ? 1 : -1);
                }

                if (show)
                {
                    Bounds =
                        DockPanel.GetAutoHideWindowBounds(new Rectangle(-rectTarget.Width, -rectTarget.Height, rectTarget.Width,
                            rectTarget.Height));
                    if (Visible == false)
                        Visible = true;
                    PerformLayout();
                }

                SuspendLayout();

                LayoutAnimateWindow(rectSource);
                if (Visible == false)
                    Visible = true;

                var speedFactor = 1;
                var totalPixels = (rectSource.Width != rectTarget.Width)
                                      ? Math.Abs(rectSource.Width - rectTarget.Width)
                                      : Math.Abs(rectSource.Height - rectTarget.Height);
                var remainPixels = totalPixels;
                var startingTime = DateTime.Now;
                while (rectSource != rectTarget)
                {
                    var startPerMove = DateTime.Now;

                    rectSource.X += dxLoc * speedFactor;
                    rectSource.Y += dyLoc * speedFactor;
                    rectSource.Width += dWidth * speedFactor;
                    rectSource.Height += dHeight * speedFactor;
                    if (Math.Sign(rectTarget.X - rectSource.X) != Math.Sign(dxLoc))
                        rectSource.X = rectTarget.X;
                    if (Math.Sign(rectTarget.Y - rectSource.Y) != Math.Sign(dyLoc))
                        rectSource.Y = rectTarget.Y;
                    if (Math.Sign(rectTarget.Width - rectSource.Width) != Math.Sign(dWidth))
                        rectSource.Width = rectTarget.Width;
                    if (Math.Sign(rectTarget.Height - rectSource.Height) != Math.Sign(dHeight))
                        rectSource.Height = rectTarget.Height;

                    LayoutAnimateWindow(rectSource);
                    if (Parent != null)
                        Parent.Update();

                    remainPixels -= speedFactor;

                    while (true)
                    {
                        var time = new TimeSpan(0, 0, 0, 0, ANIMATE_TIME);
                        var elapsedPerMove = DateTime.Now - startPerMove;
                        var elapsedTime = DateTime.Now - startingTime;
                        if (((int)((time - elapsedTime).TotalMilliseconds)) <= 0)
                        {
                            speedFactor = remainPixels;
                            break;
                        }
                        else
                            speedFactor = remainPixels * (int)elapsedPerMove.TotalMilliseconds /
                                          (int)((time - elapsedTime).TotalMilliseconds);
                        if (speedFactor >= 1)
                            break;
                    }
                }
                ResumeLayout();
                Parent.ResumeLayout();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                    m_timerMouseTrack.Dispose();
                base.Dispose(disposing);
            }

            Rectangle GetRectangle(bool show)
            {
                if (DockState == DockState.Unknown)
                    return Rectangle.Empty;

                var rect = DockPanel.AutoHideWindowRectangle;

                if (show)
                    return rect;

                if (DockState == DockState.DockLeftAutoHide)
                    rect.Width = 0;
                else if (DockState == DockState.DockRightAutoHide)
                {
                    rect.X += rect.Width;
                    rect.Width = 0;
                }
                else if (DockState == DockState.DockTopAutoHide)
                    rect.Height = 0;
                else
                {
                    rect.Y += rect.Height;
                    rect.Height = 0;
                }

                return rect;
            }

            void LayoutAnimateWindow(Rectangle rect)
            {
                Bounds = DockPanel.GetAutoHideWindowBounds(rect);

                var rectClient = ClientRectangle;

                if (DockState == DockState.DockLeftAutoHide)
                    ActivePane.Location = new Point(rectClient.Right - 2 - Measures.SplitterSize - ActivePane.Width,
                        ActivePane.Location.Y);
                else if (DockState == DockState.DockTopAutoHide)
                    ActivePane.Location = new Point(ActivePane.Location.X,
                        rectClient.Bottom - 2 - Measures.SplitterSize - ActivePane.Height);
            }

            protected override void OnLayout(LayoutEventArgs levent)
            {
                DockPadding.All = 0;
                if (DockState == DockState.DockLeftAutoHide)
                {
                    DockPadding.Right = 2;
                    m_splitter.Dock = DockStyle.Right;
                }
                else if (DockState == DockState.DockRightAutoHide)
                {
                    DockPadding.Left = 2;
                    m_splitter.Dock = DockStyle.Left;
                }
                else if (DockState == DockState.DockTopAutoHide)
                {
                    DockPadding.Bottom = 2;
                    m_splitter.Dock = DockStyle.Bottom;
                }
                else if (DockState == DockState.DockBottomAutoHide)
                {
                    DockPadding.Top = 2;
                    m_splitter.Dock = DockStyle.Top;
                }

                var rectDisplaying = DisplayingRectangle;
                var rectHidden = new Rectangle(-rectDisplaying.Width, rectDisplaying.Y, rectDisplaying.Width,
                    rectDisplaying.Height);
                foreach (Control c in Controls)
                {
                    var pane = c as DockPane;
                    if (pane == null)
                        continue;

                    if (pane == ActivePane)
                        pane.Bounds = rectDisplaying;
                    else
                        pane.Bounds = rectHidden;
                }

                base.OnLayout(levent);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                // Draw the border
                var g = e.Graphics;

                if (DockState == DockState.DockBottomAutoHide)
                    g.DrawLine(SystemPens.ControlLightLight, 0, 1, ClientRectangle.Right, 1);
                else if (DockState == DockState.DockRightAutoHide)
                    g.DrawLine(SystemPens.ControlLightLight, 1, 0, 1, ClientRectangle.Bottom);
                else if (DockState == DockState.DockTopAutoHide)
                {
                    g.DrawLine(SystemPens.ControlDark, 0, ClientRectangle.Height - 2, ClientRectangle.Right,
                        ClientRectangle.Height - 2);
                    g.DrawLine(SystemPens.ControlDarkDark, 0, ClientRectangle.Height - 1, ClientRectangle.Right,
                        ClientRectangle.Height - 1);
                }
                else if (DockState == DockState.DockLeftAutoHide)
                {
                    g.DrawLine(SystemPens.ControlDark, ClientRectangle.Width - 2, 0, ClientRectangle.Width - 2,
                        ClientRectangle.Bottom);
                    g.DrawLine(SystemPens.ControlDarkDark, ClientRectangle.Width - 1, 0, ClientRectangle.Width - 1,
                        ClientRectangle.Bottom);
                }

                base.OnPaint(e);
            }

            public void RefreshActiveContent()
            {
                if (ActiveContent == null)
                    return;

                if (!DockHelper.IsDockStateAutoHide(ActiveContent.DockHandler.DockState))
                {
                    FlagAnimate = false;
                    ActiveContent = null;
                    FlagAnimate = true;
                }
            }

            public void RefreshActivePane()
            {
                SetTimerMouseTrack();
            }

            void SetActivePane()
            {
                var value = (ActiveContent == null ? null : ActiveContent.DockHandler.Pane);

                if (value == m_activePane)
                    return;

                m_activePane = value;
            }

            void SetTimerMouseTrack()
            {
                if (ActivePane == null || ActivePane.IsActivated || FlagDragging)
                {
                    m_timerMouseTrack.Enabled = false;
                    return;
                }

                // start the timer
                var hovertime = SystemInformation.MouseHoverTime;

                // assign a default value 400 in case of setting Timer.Interval invalid value exception
                if (hovertime <= 0)
                    hovertime = 400;

                m_timerMouseTrack.Interval = 2 * hovertime;
                m_timerMouseTrack.Enabled = true;
            }

            void TimerMouseTrack_Tick(object sender, EventArgs e)
            {
                if (IsDisposed)
                    return;

                if (ActivePane == null || ActivePane.IsActivated)
                {
                    m_timerMouseTrack.Enabled = false;
                    return;
                }

                var pane = ActivePane;
                var ptMouseInAutoHideWindow = PointToClient(MousePosition);
                var ptMouseInDockPanel = DockPanel.PointToClient(MousePosition);

                var rectTabStrip = DockPanel.GetTabStripRectangle(pane.DockState);

                if (!ClientRectangle.Contains(ptMouseInAutoHideWindow) && !rectTabStrip.Contains(ptMouseInDockPanel))
                {
                    ActiveContent = null;
                    m_timerMouseTrack.Enabled = false;
                }
            }

            #region ISplitterDragSource Members

            Control IDragSource.DragControl
            {
                get { return this; }
            }

            Rectangle ISplitterDragSource.DragLimitBounds
            {
                get
                {
                    var rectLimit = DockPanel.DockArea;

                    if ((this as ISplitterDragSource).IsVertical)
                    {
                        rectLimit.X += MeasurePane.MinSize;
                        rectLimit.Width -= 2 * MeasurePane.MinSize;
                    }
                    else
                    {
                        rectLimit.Y += MeasurePane.MinSize;
                        rectLimit.Height -= 2 * MeasurePane.MinSize;
                    }

                    return DockPanel.RectangleToScreen(rectLimit);
                }
            }

            bool ISplitterDragSource.IsVertical
            {
                get { return (DockState == DockState.DockLeftAutoHide || DockState == DockState.DockRightAutoHide); }
            }

            void ISplitterDragSource.BeginDrag(Rectangle rectSplitter)
            {
                FlagDragging = true;
            }

            void ISplitterDragSource.EndDrag()
            {
                FlagDragging = false;
            }

            void ISplitterDragSource.MoveSplitter(int offset)
            {
                var rectDockArea = DockPanel.DockArea;
                var content = ActiveContent;
                if (DockState == DockState.DockLeftAutoHide && rectDockArea.Width > 0)
                {
                    if (content.DockHandler.AutoHidePortion < 1)
                        content.DockHandler.AutoHidePortion += (offset) / (double)rectDockArea.Width;
                    else
                        content.DockHandler.AutoHidePortion = Width + offset;
                }
                else if (DockState == DockState.DockRightAutoHide && rectDockArea.Width > 0)
                {
                    if (content.DockHandler.AutoHidePortion < 1)
                        content.DockHandler.AutoHidePortion -= (offset) / (double)rectDockArea.Width;
                    else
                        content.DockHandler.AutoHidePortion = Width - offset;
                }
                else if (DockState == DockState.DockBottomAutoHide && rectDockArea.Height > 0)
                {
                    if (content.DockHandler.AutoHidePortion < 1)
                        content.DockHandler.AutoHidePortion -= (offset) / (double)rectDockArea.Height;
                    else
                        content.DockHandler.AutoHidePortion = Height - offset;
                }
                else if (DockState == DockState.DockTopAutoHide && rectDockArea.Height > 0)
                {
                    if (content.DockHandler.AutoHidePortion < 1)
                        content.DockHandler.AutoHidePortion += (offset) / (double)rectDockArea.Height;
                    else
                        content.DockHandler.AutoHidePortion = Height + offset;
                }
            }

            #endregion

            class SplitterControl : SplitterBase
            {
                readonly AutoHideWindowControl m_autoHideWindow;

                public SplitterControl(AutoHideWindowControl autoHideWindow)
                {
                    m_autoHideWindow = autoHideWindow;
                }

                AutoHideWindowControl AutoHideWindow
                {
                    get { return m_autoHideWindow; }
                }

                protected override int SplitterSize
                {
                    get { return Measures.SplitterSize; }
                }

                protected override void StartDrag()
                {
                    AutoHideWindow.DockPanel.BeginDrag(AutoHideWindow, AutoHideWindow.RectangleToScreen(Bounds));
                }
            }

            #region consts

            const int ANIMATE_TIME = 100; // in mini-seconds

            #endregion
        }
    }
}