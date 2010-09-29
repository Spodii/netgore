using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NetGore.EditorTools.Docking
{
    public abstract class AutoHideStripBase : Control
    {
        readonly DockPanel m_dockPanel;

        readonly PaneCollection m_panesBottom;

        readonly PaneCollection m_panesLeft;

        readonly PaneCollection m_panesRight;
        readonly PaneCollection m_panesTop;
        GraphicsPath m_displayingArea = null;

        protected AutoHideStripBase(DockPanel panel)
        {
            m_dockPanel = panel;
            m_panesTop = new PaneCollection(panel, DockState.DockTopAutoHide);
            m_panesBottom = new PaneCollection(panel, DockState.DockBottomAutoHide);
            m_panesLeft = new PaneCollection(panel, DockState.DockLeftAutoHide);
            m_panesRight = new PaneCollection(panel, DockState.DockRightAutoHide);

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Selectable, false);
        }

        GraphicsPath DisplayingArea
        {
            get
            {
                if (m_displayingArea == null)
                    m_displayingArea = new GraphicsPath();

                return m_displayingArea;
            }
        }

        protected DockPanel DockPanel
        {
            get { return m_dockPanel; }
        }

        protected PaneCollection PanesBottom
        {
            get { return m_panesBottom; }
        }

        protected PaneCollection PanesLeft
        {
            get { return m_panesLeft; }
        }

        protected PaneCollection PanesRight
        {
            get { return m_panesRight; }
        }

        protected PaneCollection PanesTop
        {
            get { return m_panesTop; }
        }

        protected Rectangle RectangleBottomLeft
        {
            get
            {
                var height = MeasureHeight();
                return PanesBottom.Count > 0 && PanesLeft.Count > 0
                           ? new Rectangle(0, Height - height, height, height) : Rectangle.Empty;
            }
        }

        protected Rectangle RectangleBottomRight
        {
            get
            {
                var height = MeasureHeight();
                return PanesBottom.Count > 0 && PanesRight.Count > 0
                           ? new Rectangle(Width - height, Height - height, height, height) : Rectangle.Empty;
            }
        }

        protected Rectangle RectangleTopLeft
        {
            get
            {
                var height = MeasureHeight();
                return PanesTop.Count > 0 && PanesLeft.Count > 0 ? new Rectangle(0, 0, height, height) : Rectangle.Empty;
            }
        }

        protected Rectangle RectangleTopRight
        {
            get
            {
                var height = MeasureHeight();
                return PanesTop.Count > 0 && PanesRight.Count > 0
                           ? new Rectangle(Width - height, 0, height, height) : Rectangle.Empty;
            }
        }

        protected virtual Pane CreatePane(DockPane dockPane)
        {
            return new Pane(dockPane);
        }

        protected virtual Tab CreateTab(IDockContent content)
        {
            return new Tab(content);
        }

        internal int GetNumberOfPanes(DockState dockState)
        {
            return GetPanes(dockState).Count;
        }

        protected PaneCollection GetPanes(DockState dockState)
        {
            if (dockState == DockState.DockTopAutoHide)
                return PanesTop;
            else if (dockState == DockState.DockBottomAutoHide)
                return PanesBottom;
            else if (dockState == DockState.DockLeftAutoHide)
                return PanesLeft;
            else if (dockState == DockState.DockRightAutoHide)
                return PanesRight;
            else
                throw new ArgumentOutOfRangeException("dockState");
        }

        protected internal Rectangle GetTabStripRectangle(DockState dockState)
        {
            var height = MeasureHeight();
            if (dockState == DockState.DockTopAutoHide && PanesTop.Count > 0)
                return new Rectangle(RectangleTopLeft.Width, 0, Width - RectangleTopLeft.Width - RectangleTopRight.Width, height);
            else if (dockState == DockState.DockBottomAutoHide && PanesBottom.Count > 0)
            {
                return new Rectangle(RectangleBottomLeft.Width, Height - height,
                                     Width - RectangleBottomLeft.Width - RectangleBottomRight.Width, height);
            }
            else if (dockState == DockState.DockLeftAutoHide && PanesLeft.Count > 0)
                return new Rectangle(0, RectangleTopLeft.Width, height,
                                     Height - RectangleTopLeft.Height - RectangleBottomLeft.Height);
            else if (dockState == DockState.DockRightAutoHide && PanesRight.Count > 0)
            {
                return new Rectangle(Width - height, RectangleTopRight.Width, height,
                                     Height - RectangleTopRight.Height - RectangleBottomRight.Height);
            }
            else
                return Rectangle.Empty;
        }

        protected abstract IDockContent HitTest(Point point);

        IDockContent HitTest()
        {
            var ptMouse = PointToClient(MousePosition);
            return HitTest(ptMouse);
        }

        protected internal abstract int MeasureHeight();

        protected override void OnLayout(LayoutEventArgs levent)
        {
            RefreshChanges();
            base.OnLayout(levent);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button != MouseButtons.Left)
                return;

            var content = HitTest();
            if (content == null)
                return;

            content.DockHandler.Activate();
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);

            var content = HitTest();
            if (content != null && DockPanel.ActiveAutoHideContent != content)
                DockPanel.ActiveAutoHideContent = content;

            // requires further tracking of mouse hover behavior,
            ResetMouseEventArgs();
        }

        protected virtual void OnRefreshChanges()
        {
        }

        internal void RefreshChanges()
        {
            if (IsDisposed)
                return;

            SetRegion();
            OnRefreshChanges();
        }

        void SetRegion()
        {
            DisplayingArea.Reset();
            DisplayingArea.AddRectangle(RectangleTopLeft);
            DisplayingArea.AddRectangle(RectangleTopRight);
            DisplayingArea.AddRectangle(RectangleBottomLeft);
            DisplayingArea.AddRectangle(RectangleBottomRight);
            DisplayingArea.AddRectangle(GetTabStripRectangle(DockState.DockTopAutoHide));
            DisplayingArea.AddRectangle(GetTabStripRectangle(DockState.DockBottomAutoHide));
            DisplayingArea.AddRectangle(GetTabStripRectangle(DockState.DockLeftAutoHide));
            DisplayingArea.AddRectangle(GetTabStripRectangle(DockState.DockRightAutoHide));
            Region = new Region(DisplayingArea);
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        protected class Pane : IDisposable
        {
            readonly DockPane m_dockPane;

            protected internal Pane(DockPane dockPane)
            {
                m_dockPane = dockPane;
            }

            public TabCollection AutoHideTabs
            {
                get
                {
                    if (DockPane.AutoHideTabs == null)
                        DockPane.AutoHideTabs = new TabCollection(DockPane);
                    return DockPane.AutoHideTabs as TabCollection;
                }
            }

            public DockPane DockPane
            {
                get { return m_dockPane; }
            }

            protected virtual void Dispose(bool disposing)
            {
            }

            ~Pane()
            {
                Dispose(false);
            }

            #region IDisposable Members

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            #endregion
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        protected sealed class PaneCollection : IEnumerable<Pane>
        {
            readonly DockPanel m_dockPanel;

            readonly AutoHideStateCollection m_states;

            internal PaneCollection(DockPanel panel, DockState dockState)
            {
                m_dockPanel = panel;
                m_states = new AutoHideStateCollection();
                States[DockState.DockTopAutoHide].Selected = (dockState == DockState.DockTopAutoHide);
                States[DockState.DockBottomAutoHide].Selected = (dockState == DockState.DockBottomAutoHide);
                States[DockState.DockLeftAutoHide].Selected = (dockState == DockState.DockLeftAutoHide);
                States[DockState.DockRightAutoHide].Selected = (dockState == DockState.DockRightAutoHide);
            }

            public Pane this[int index]
            {
                get
                {
                    var count = 0;
                    foreach (var pane in DockPanel.Panes)
                    {
                        if (!States.ContainsPane(pane))
                            continue;

                        if (count == index)
                        {
                            if (pane.AutoHidePane == null)
                                pane.AutoHidePane = DockPanel.AutoHideStripControl.CreatePane(pane);
                            return pane.AutoHidePane as Pane;
                        }

                        count++;
                    }
                    throw new ArgumentOutOfRangeException("index");
                }
            }

            public int Count
            {
                get
                {
                    var count = 0;
                    foreach (var pane in DockPanel.Panes)
                    {
                        if (States.ContainsPane(pane))
                            count++;
                    }

                    return count;
                }
            }

            public DockPanel DockPanel
            {
                get { return m_dockPanel; }
            }

            AutoHideStateCollection States
            {
                get { return m_states; }
            }

            public bool Contains(Pane pane)
            {
                return (IndexOf(pane) != -1);
            }

            public int IndexOf(Pane pane)
            {
                if (pane == null)
                    return -1;

                var index = 0;
                foreach (var dockPane in DockPanel.Panes)
                {
                    if (!States.ContainsPane(pane.DockPane))
                        continue;

                    if (pane == dockPane.AutoHidePane)
                        return index;

                    index++;
                }
                return -1;
            }

            #region IEnumerable<Pane> Members

            IEnumerator<Pane> IEnumerable<Pane>.GetEnumerator()
            {
                for (var i = 0; i < Count; i++)
                {
                    yield return this[i];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                for (var i = 0; i < Count; i++)
                {
                    yield return this[i];
                }
            }

            #endregion

            class AutoHideState
            {
                public readonly DockState m_dockState;
                public bool m_selected = false;

                public AutoHideState(DockState dockState)
                {
                    m_dockState = dockState;
                }

                public DockState DockState
                {
                    get { return m_dockState; }
                }

                public bool Selected
                {
                    get { return m_selected; }
                    set { m_selected = value; }
                }
            }

            class AutoHideStateCollection
            {
                readonly AutoHideState[] m_states;

                public AutoHideStateCollection()
                {
                    m_states = new AutoHideState[]
                    {
                        new AutoHideState(DockState.DockTopAutoHide), new AutoHideState(DockState.DockBottomAutoHide),
                        new AutoHideState(DockState.DockLeftAutoHide), new AutoHideState(DockState.DockRightAutoHide)
                    };
                }

                public AutoHideState this[DockState dockState]
                {
                    get
                    {
                        for (var i = 0; i < m_states.Length; i++)
                        {
                            if (m_states[i].DockState == dockState)
                                return m_states[i];
                        }
                        throw new ArgumentOutOfRangeException("dockState");
                    }
                }

                public bool ContainsPane(DockPane pane)
                {
                    if (pane.IsHidden)
                        return false;

                    for (var i = 0; i < m_states.Length; i++)
                    {
                        if (m_states[i].DockState == pane.DockState && m_states[i].Selected)
                            return true;
                    }
                    return false;
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        protected class Tab : IDisposable
        {
            readonly IDockContent m_content;

            protected internal Tab(IDockContent content)
            {
                m_content = content;
            }

            public IDockContent Content
            {
                get { return m_content; }
            }

            protected virtual void Dispose(bool disposing)
            {
            }

            ~Tab()
            {
                Dispose(false);
            }

            #region IDisposable Members

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            #endregion
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        protected sealed class TabCollection : IEnumerable<Tab>
        {
            readonly DockPane m_dockPane = null;

            internal TabCollection(DockPane pane)
            {
                m_dockPane = pane;
            }

            public Tab this[int index]
            {
                get
                {
                    var content = DockPane.DisplayingContents[index];
                    if (content == null)
                        throw (new ArgumentOutOfRangeException("index"));
                    if (content.DockHandler.AutoHideTab == null)
                        content.DockHandler.AutoHideTab = (DockPanel.AutoHideStripControl.CreateTab(content));
                    return content.DockHandler.AutoHideTab as Tab;
                }
            }

            public int Count
            {
                get { return DockPane.DisplayingContents.Count; }
            }

            public DockPane DockPane
            {
                get { return m_dockPane; }
            }

            public DockPanel DockPanel
            {
                get { return DockPane.DockPanel; }
            }

            public bool Contains(Tab tab)
            {
                return (IndexOf(tab) != -1);
            }

            public bool Contains(IDockContent content)
            {
                return (IndexOf(content) != -1);
            }

            public int IndexOf(Tab tab)
            {
                if (tab == null)
                    return -1;

                return IndexOf(tab.Content);
            }

            public int IndexOf(IDockContent content)
            {
                return DockPane.DisplayingContents.IndexOf(content);
            }

            #region IEnumerable<Tab> Members

            IEnumerator<Tab> IEnumerable<Tab>.GetEnumerator()
            {
                for (var i = 0; i < Count; i++)
                {
                    yield return this[i];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                for (var i = 0; i < Count; i++)
                {
                    yield return this[i];
                }
            }

            #endregion
        }
    }
}