using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace NetGore.EditorTools.Docking
{
    public sealed class NestedPaneCollection : ReadOnlyCollection<DockPane>
    {
        readonly INestedPanesContainer m_container;
        readonly VisibleNestedPaneCollection m_visibleNestedPanes;

        internal NestedPaneCollection(INestedPanesContainer container) : base(new List<DockPane>())
        {
            m_container = container;
            m_visibleNestedPanes = new VisibleNestedPaneCollection(this);
        }

        public INestedPanesContainer Container
        {
            get { return m_container; }
        }

        public DockState DockState
        {
            get { return Container.DockState; }
        }

        public bool IsFloat
        {
            get { return DockState == DockState.Float; }
        }

        public VisibleNestedPaneCollection VisibleNestedPanes
        {
            get { return m_visibleNestedPanes; }
        }

        internal void Add(DockPane pane)
        {
            if (pane == null)
                return;

            var oldNestedPanes = (pane.NestedPanesContainer == null) ? null : pane.NestedPanesContainer.NestedPanes;
            if (oldNestedPanes != null)
                oldNestedPanes.InternalRemove(pane);
            Items.Add(pane);
            if (oldNestedPanes != null)
                oldNestedPanes.CheckFloatWindowDispose();
        }

        void CheckFloatWindowDispose()
        {
            if (Count == 0 && Container.DockState == DockState.Float)
            {
                var floatWindow = (FloatWindow)Container;
                if (!floatWindow.Disposing && !floatWindow.IsDisposed)
                    NativeMethods.PostMessage(((FloatWindow)Container).Handle, FloatWindow.WM_CHECKDISPOSE, 0, 0);
            }
        }

        public DockPane GetDefaultPreviousPane(DockPane pane)
        {
            for (var i = Count - 1; i >= 0; i--)
            {
                if (this[i] != pane)
                    return this[i];
            }

            return null;
        }

        void InternalRemove(DockPane pane)
        {
            if (!Contains(pane))
                return;

            var statusPane = pane.NestedDockingStatus;
            DockPane lastNestedPane = null;
            for (var i = Count - 1; i > IndexOf(pane); i--)
            {
                if (this[i].NestedDockingStatus.PreviousPane == pane)
                {
                    lastNestedPane = this[i];
                    break;
                }
            }

            if (lastNestedPane != null)
            {
                var indexLastNestedPane = IndexOf(lastNestedPane);
                Items.Remove(lastNestedPane);
                Items[IndexOf(pane)] = lastNestedPane;
                var lastNestedDock = lastNestedPane.NestedDockingStatus;
                lastNestedDock.SetStatus(this, statusPane.PreviousPane, statusPane.Alignment, statusPane.Proportion);
                for (var i = indexLastNestedPane - 1; i > IndexOf(lastNestedPane); i--)
                {
                    var status = this[i].NestedDockingStatus;
                    if (status.PreviousPane == pane)
                        status.SetStatus(this, lastNestedPane, status.Alignment, status.Proportion);
                }
            }
            else
                Items.Remove(pane);

            statusPane.SetStatus(null, null, DockAlignment.Left, 0.5);
            statusPane.SetDisplayingStatus(false, null, DockAlignment.Left, 0.5);
            statusPane.SetDisplayingBounds(Rectangle.Empty, Rectangle.Empty, Rectangle.Empty);
        }

        internal void Remove(DockPane pane)
        {
            InternalRemove(pane);
            CheckFloatWindowDispose();
        }
    }
}