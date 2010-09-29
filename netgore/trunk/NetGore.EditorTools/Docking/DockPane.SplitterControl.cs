using System.Drawing;
using System.Windows.Forms;

namespace NetGore.EditorTools.Docking
{
    partial class DockPane
    {
        SplitterControl m_splitter;

        SplitterControl Splitter
        {
            get { return m_splitter; }
        }

        internal DockAlignment SplitterAlignment
        {
            set { Splitter.Alignment = value; }
        }

        internal Rectangle SplitterBounds
        {
            set { Splitter.Bounds = value; }
        }

        class SplitterControl : Control, ISplitterDragSource
        {
            readonly DockPane m_pane;

            DockAlignment m_alignment;

            public SplitterControl(DockPane pane)
            {
                SetStyle(ControlStyles.Selectable, false);
                m_pane = pane;
            }

            public DockAlignment Alignment
            {
                get { return m_alignment; }
                set
                {
                    m_alignment = value;
                    if (m_alignment == DockAlignment.Left || m_alignment == DockAlignment.Right)
                        Cursor = Cursors.VSplit;
                    else if (m_alignment == DockAlignment.Top || m_alignment == DockAlignment.Bottom)
                        Cursor = Cursors.HSplit;
                    else
                        Cursor = Cursors.Default;

                    if (DockPane.DockState == DockState.Document)
                        Invalidate();
                }
            }

            public DockPane DockPane
            {
                get { return m_pane; }
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                base.OnMouseDown(e);

                if (e.Button != MouseButtons.Left)
                    return;

                DockPane.DockPanel.BeginDrag(this, Parent.RectangleToScreen(Bounds));
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                if (DockPane.DockState != DockState.Document)
                    return;

                var g = e.Graphics;
                var rect = ClientRectangle;
                if (Alignment == DockAlignment.Top || Alignment == DockAlignment.Bottom)
                    g.DrawLine(SystemPens.ControlDark, rect.Left, rect.Bottom - 1, rect.Right, rect.Bottom - 1);
                else if (Alignment == DockAlignment.Left || Alignment == DockAlignment.Right)
                    g.DrawLine(SystemPens.ControlDarkDark, rect.Right - 1, rect.Top, rect.Right - 1, rect.Bottom);
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
                    var status = DockPane.NestedDockingStatus;
                    var rectLimit = Parent.RectangleToScreen(status.LogicalBounds);
                    if (((ISplitterDragSource)this).IsVertical)
                    {
                        rectLimit.X += MeasurePane.MinSize;
                        rectLimit.Width -= 2 * MeasurePane.MinSize;
                    }
                    else
                    {
                        rectLimit.Y += MeasurePane.MinSize;
                        rectLimit.Height -= 2 * MeasurePane.MinSize;
                    }

                    return rectLimit;
                }
            }

            bool ISplitterDragSource.IsVertical
            {
                get
                {
                    var status = DockPane.NestedDockingStatus;
                    return (status.DisplayingAlignment == DockAlignment.Left || status.DisplayingAlignment == DockAlignment.Right);
                }
            }

            void ISplitterDragSource.BeginDrag(Rectangle rectSplitter)
            {
            }

            void ISplitterDragSource.EndDrag()
            {
            }

            void ISplitterDragSource.MoveSplitter(int offset)
            {
                var status = DockPane.NestedDockingStatus;
                var proportion = status.Proportion;
                if (status.LogicalBounds.Width <= 0 || status.LogicalBounds.Height <= 0)
                    return;
                else if (status.DisplayingAlignment == DockAlignment.Left)
                    proportion += (offset) / (double)status.LogicalBounds.Width;
                else if (status.DisplayingAlignment == DockAlignment.Right)
                    proportion -= (offset) / (double)status.LogicalBounds.Width;
                else if (status.DisplayingAlignment == DockAlignment.Top)
                    proportion += (offset) / (double)status.LogicalBounds.Height;
                else
                    proportion -= (offset) / (double)status.LogicalBounds.Height;

                DockPane.SetNestedDockingProportion(proportion);
            }

            #endregion
        }
    }
}