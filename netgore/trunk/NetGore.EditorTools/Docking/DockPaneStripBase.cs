using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Permissions;
using System.Windows.Forms;
using NetGore.EditorTools.Docking.Win32;

namespace NetGore.EditorTools.Docking
{
    public abstract class DockPaneStripBase : Control
    {
        readonly DockPane m_dockPane;

        TabCollection m_tabs = null;

        protected DockPaneStripBase(DockPane pane)
        {
            m_dockPane = pane;

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Selectable, false);
            AllowDrop = true;
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

        protected TabCollection Tabs
        {
            get
            {
                if (m_tabs == null)
                    m_tabs = new TabCollection(DockPane);

                return m_tabs;
            }
        }

        protected internal virtual Tab CreateTab(IDockContent content)
        {
            return new Tab(content);
        }

        protected internal abstract void EnsureTabVisible(IDockContent content);

        protected internal abstract GraphicsPath GetOutline(int index);

        protected int HitTest()
        {
            return HitTest(PointToClient(MousePosition));
        }

        protected internal abstract int HitTest(Point point);

        protected internal abstract int MeasureHeight();

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            base.OnDragOver(drgevent);

            var index = HitTest();
            if (index != -1)
            {
                var content = Tabs[index].Content;
                if (DockPane.ActiveContent != content)
                    DockPane.ActiveContent = content;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            var index = HitTest();
            if (index != -1)
            {
                var content = Tabs[index].Content;
                if (DockPane.ActiveContent != content)
                    DockPane.ActiveContent = content;
            }

            if (e.Button == MouseButtons.Left)
            {
                if (DockPane.DockPanel.AllowEndUserDocking && DockPane.AllowDockDragAndDrop &&
                    DockPane.ActiveContent.DockHandler.AllowEndUserDocking)
                    DockPane.DockPanel.BeginDrag(DockPane.ActiveContent.DockHandler);
            }
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
                base.WndProc(ref m);

                var index = HitTest();
                if (DockPane.DockPanel.AllowEndUserDocking && index != -1)
                {
                    var content = Tabs[index].Content;
                    if (content.DockHandler.CheckDockState(!content.DockHandler.IsFloat) != DockState.Unknown)
                        content.DockHandler.IsFloat = !content.DockHandler.IsFloat;
                }

                return;
            }

            base.WndProc(ref m);
            return;
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        protected internal class Tab : IDisposable
        {
            readonly IDockContent m_content;

            public Tab(IDockContent content)
            {
                m_content = content;
            }

            public IDockContent Content
            {
                get { return m_content; }
            }

            public Form ContentForm
            {
                get { return m_content as Form; }
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
            readonly DockPane m_dockPane;

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
                    return content.DockHandler.GetTab(DockPane.TabStripControl);
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

                return DockPane.DisplayingContents.IndexOf(tab.Content);
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