using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace NetGore.Editor.Docking
{
    public sealed class VisibleNestedPaneCollection : ReadOnlyCollection<DockPane>
    {
        readonly NestedPaneCollection m_nestedPanes;

        internal VisibleNestedPaneCollection(NestedPaneCollection nestedPanes) : base(new List<DockPane>())
        {
            m_nestedPanes = nestedPanes;
        }

        public INestedPanesContainer Container
        {
            get { return NestedPanes.Container; }
        }

        public DockState DockState
        {
            get { return NestedPanes.DockState; }
        }

        public bool IsFloat
        {
            get { return NestedPanes.IsFloat; }
        }

        public NestedPaneCollection NestedPanes
        {
            get { return m_nestedPanes; }
        }

        void CalculateBounds()
        {
            if (Count == 0)
                return;

            this[0].NestedDockingStatus.SetDisplayingBounds(Container.DisplayingRectangle, Container.DisplayingRectangle,
                                                            Rectangle.Empty);

            for (var i = 1; i < Count; i++)
            {
                var pane = this[i];
                var status = pane.NestedDockingStatus;
                var prevPane = status.DisplayingPreviousPane;
                var statusPrev = prevPane.NestedDockingStatus;

                var rect = statusPrev.PaneBounds;

                var rectThis = rect;
                var rectPrev = rect;
                var rectSplitter = rect;
                if (status.DisplayingAlignment == DockAlignment.Left)
                {
                    rectThis.Width = (int)(rect.Width * status.DisplayingProportion) - (Measures.SplitterSize / 2);
                    rectSplitter.X = rectThis.X + rectThis.Width;
                    rectSplitter.Width = Measures.SplitterSize;
                    rectPrev.X = rectSplitter.X + rectSplitter.Width;
                    rectPrev.Width = rect.Width - rectThis.Width - rectSplitter.Width;
                }
                else if (status.DisplayingAlignment == DockAlignment.Right)
                {
                    rectPrev.Width = (rect.Width - (int)(rect.Width * status.DisplayingProportion)) - (Measures.SplitterSize / 2);
                    rectSplitter.X = rectPrev.X + rectPrev.Width;
                    rectSplitter.Width = Measures.SplitterSize;
                    rectThis.X = rectSplitter.X + rectSplitter.Width;
                    rectThis.Width = rect.Width - rectPrev.Width - rectSplitter.Width;
                }
                else if (status.DisplayingAlignment == DockAlignment.Top)
                {
                    rectThis.Height = (int)(rect.Height * status.DisplayingProportion) - (Measures.SplitterSize / 2);
                    rectSplitter.Y = rectThis.Y + rectThis.Height;
                    rectSplitter.Height = Measures.SplitterSize;
                    rectPrev.Y = rectSplitter.Y + rectSplitter.Height;
                    rectPrev.Height = rect.Height - rectThis.Height - rectSplitter.Height;
                }
                else if (status.DisplayingAlignment == DockAlignment.Bottom)
                {
                    rectPrev.Height = (rect.Height - (int)(rect.Height * status.DisplayingProportion)) -
                                      (Measures.SplitterSize / 2);
                    rectSplitter.Y = rectPrev.Y + rectPrev.Height;
                    rectSplitter.Height = Measures.SplitterSize;
                    rectThis.Y = rectSplitter.Y + rectSplitter.Height;
                    rectThis.Height = rect.Height - rectPrev.Height - rectSplitter.Height;
                }
                else
                    rectThis = Rectangle.Empty;

                rectSplitter.Intersect(rect);
                rectThis.Intersect(rect);
                rectPrev.Intersect(rect);
                status.SetDisplayingBounds(rect, rectThis, rectSplitter);
                statusPrev.SetDisplayingBounds(statusPrev.LogicalBounds, rectPrev, statusPrev.SplitterBounds);
            }
        }

        internal void Refresh()
        {
            Items.Clear();
            for (var i = 0; i < NestedPanes.Count; i++)
            {
                var pane = NestedPanes[i];
                var status = pane.NestedDockingStatus;
                status.SetDisplayingStatus(true, status.PreviousPane, status.Alignment, status.Proportion);
                Items.Add(pane);
            }

            foreach (var pane in NestedPanes)
            {
                if (pane.DockState != DockState || pane.IsHidden)
                {
                    pane.Bounds = Rectangle.Empty;
                    pane.SplitterBounds = Rectangle.Empty;
                    Remove(pane);
                }
            }

            CalculateBounds();

            foreach (var pane in this)
            {
                var status = pane.NestedDockingStatus;
                pane.Bounds = status.PaneBounds;
                pane.SplitterBounds = status.SplitterBounds;
                pane.SplitterAlignment = status.Alignment;
            }
        }

        void Remove(DockPane pane)
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
                lastNestedDock.SetDisplayingStatus(true, statusPane.DisplayingPreviousPane, statusPane.DisplayingAlignment,
                                                   statusPane.DisplayingProportion);
                for (var i = indexLastNestedPane - 1; i > IndexOf(lastNestedPane); i--)
                {
                    var status = this[i].NestedDockingStatus;
                    if (status.PreviousPane == pane)
                        status.SetDisplayingStatus(true, lastNestedPane, status.DisplayingAlignment, status.DisplayingProportion);
                }
            }
            else
                Items.Remove(pane);

            statusPane.SetDisplayingStatus(false, null, DockAlignment.Left, 0.5);
        }
    }
}