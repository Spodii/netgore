using System.Drawing;

namespace NetGore.EditorTools.Docking
{
    partial class DockPanel
    {
        SplitterDragHandler m_splitterDragHandler = null;

        internal void BeginDrag(ISplitterDragSource dragSource, Rectangle rectSplitter)
        {
            GetSplitterDragHandler().BeginDrag(dragSource, rectSplitter);
        }

        SplitterDragHandler GetSplitterDragHandler()
        {
            if (m_splitterDragHandler == null)
                m_splitterDragHandler = new SplitterDragHandler(this);
            return m_splitterDragHandler;
        }

        sealed class SplitterDragHandler : DragHandler
        {
            SplitterOutline m_outline;
            Rectangle m_rectSplitter;

            public SplitterDragHandler(DockPanel dockPanel) : base(dockPanel)
            {
            }

            public new ISplitterDragSource DragSource
            {
                get { return base.DragSource as ISplitterDragSource; }
                private set { base.DragSource = value; }
            }

            SplitterOutline Outline
            {
                get { return m_outline; }
                set { m_outline = value; }
            }

            Rectangle RectSplitter
            {
                get { return m_rectSplitter; }
                set { m_rectSplitter = value; }
            }

            public void BeginDrag(ISplitterDragSource dragSource, Rectangle rectSplitter)
            {
                DragSource = dragSource;
                RectSplitter = rectSplitter;

                if (!BeginDrag())
                {
                    DragSource = null;
                    return;
                }

                Outline = new SplitterOutline();
                Outline.Show(rectSplitter);
                DragSource.BeginDrag(rectSplitter);
            }

            int GetMovingOffset(Point ptMouse)
            {
                var rect = GetSplitterOutlineBounds(ptMouse);
                if (DragSource.IsVertical)
                    return rect.X - RectSplitter.X;
                else
                    return rect.Y - RectSplitter.Y;
            }

            Rectangle GetSplitterOutlineBounds(Point ptMouse)
            {
                var rectLimit = DragSource.DragLimitBounds;

                var rect = RectSplitter;
                if (rectLimit.Width <= 0 || rectLimit.Height <= 0)
                    return rect;

                if (DragSource.IsVertical)
                {
                    rect.X += ptMouse.X - StartMousePosition.X;
                    rect.Height = rectLimit.Height;
                }
                else
                {
                    rect.Y += ptMouse.Y - StartMousePosition.Y;
                    rect.Width = rectLimit.Width;
                }

                if (rect.Left < rectLimit.Left)
                    rect.X = rectLimit.X;
                if (rect.Top < rectLimit.Top)
                    rect.Y = rectLimit.Y;
                if (rect.Right > rectLimit.Right)
                    rect.X -= rect.Right - rectLimit.Right;
                if (rect.Bottom > rectLimit.Bottom)
                    rect.Y -= rect.Bottom - rectLimit.Bottom;

                return rect;
            }

            protected override void OnDragging()
            {
                Outline.Show(GetSplitterOutlineBounds(MousePosition));
            }

            protected override void OnEndDrag(bool abort)
            {
                DockPanel.SuspendLayout(true);

                Outline.Close();

                if (!abort)
                    DragSource.MoveSplitter(GetMovingOffset(MousePosition));

                DragSource.EndDrag();
                DockPanel.ResumeLayout(true, true);
            }

            class SplitterOutline
            {
                readonly DragForm m_dragForm;

                public SplitterOutline()
                {
                    m_dragForm = new DragForm();
                    SetDragForm(Rectangle.Empty);
                    DragForm.BackColor = Color.Black;
                    DragForm.Opacity = 0.7;
                    DragForm.Show(false);
                }

                DragForm DragForm
                {
                    get { return m_dragForm; }
                }

                public void Close()
                {
                    DragForm.Close();
                }

                void SetDragForm(Rectangle rect)
                {
                    DragForm.Bounds = rect;
                    if (rect == Rectangle.Empty)
                        DragForm.Region = new Region(Rectangle.Empty);
                    else if (DragForm.Region != null)
                        DragForm.Region = null;
                }

                public void Show(Rectangle rect)
                {
                    SetDragForm(rect);
                }
            }
        }
    }
}