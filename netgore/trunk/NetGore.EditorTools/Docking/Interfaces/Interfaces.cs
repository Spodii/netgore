using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.EditorTools.Docking
{
    public interface IDockContent
    {
        DockContentHandler DockHandler { get; }

        void OnActivated(EventArgs e);

        void OnDeactivate(EventArgs e);
    }

    public interface INestedPanesContainer
    {
        Rectangle DisplayingRectangle { get; }
        DockState DockState { get; }
        bool IsFloat { get; }
        NestedPaneCollection NestedPanes { get; }
        VisibleNestedPaneCollection VisibleNestedPanes { get; }
    }

    interface IDragSource
    {
        Control DragControl { get; }
    }

    interface IDockDragSource : IDragSource
    {
        Rectangle BeginDrag(Point ptMouse);

        bool CanDockTo(DockPane pane);

        void DockTo(DockPane pane, DockStyle dockStyle, int contentIndex);

        void DockTo(DockPanel panel, DockStyle dockStyle);

        void FloatAt(Rectangle floatWindowBounds);

        bool IsDockStateValid(DockState dockState);
    }

    interface ISplitterDragSource : IDragSource
    {
        Rectangle DragLimitBounds { get; }
        bool IsVertical { get; }

        void BeginDrag(Rectangle rectSplitter);

        void EndDrag();

        void MoveSplitter(int offset);
    }
}