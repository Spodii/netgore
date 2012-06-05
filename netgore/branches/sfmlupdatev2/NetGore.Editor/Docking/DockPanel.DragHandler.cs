using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NetGore.Editor.Docking.Win32;

namespace NetGore.Editor.Docking
{
    partial class DockPanel
    {
        abstract class DragHandler : DragHandlerBase
        {
            readonly DockPanel m_dockPanel;
            IDragSource m_dragSource;

            protected DragHandler(DockPanel dockPanel)
            {
                m_dockPanel = dockPanel;
            }

            public DockPanel DockPanel
            {
                get { return m_dockPanel; }
            }

            protected override sealed Control DragControl
            {
                get { return DragSource == null ? null : DragSource.DragControl; }
            }

            protected IDragSource DragSource
            {
                get { return m_dragSource; }
                set { m_dragSource = value; }
            }

            protected override sealed bool OnPreFilterMessage(ref Message m)
            {
                if ((m.Msg == (int)Msgs.WM_KEYDOWN || m.Msg == (int)Msgs.WM_KEYUP) &&
                    ((int)m.WParam == (int)Keys.ControlKey || (int)m.WParam == (int)Keys.ShiftKey))
                    OnDragging();

                return base.OnPreFilterMessage(ref m);
            }
        }

        /// <summary>
        /// DragHandlerBase is the base class for drag handlers. The derived class should:
        ///   1. Define its public method BeginDrag. From within this public BeginDrag method,
        ///      DragHandlerBase.BeginDrag should be called to initialize the mouse capture
        ///      and message filtering.
        ///   2. Override the OnDragging and OnEndDrag methods.
        /// </summary>
        abstract class DragHandlerBase : NativeWindow, IMessageFilter
        {
            Point m_startMousePosition = Point.Empty;
            protected abstract Control DragControl { get; }

            protected Point StartMousePosition
            {
                get { return m_startMousePosition; }
                private set { m_startMousePosition = value; }
            }

            protected bool BeginDrag()
            {
                // Avoid re-entrance;
                lock (this)
                {
                    if (DragControl == null)
                        return false;

                    StartMousePosition = MousePosition;

                    if (!NativeMethods.DragDetect(DragControl.Handle, StartMousePosition))
                        return false;

                    DragControl.FindForm().Capture = true;
                    AssignHandle(DragControl.FindForm().Handle);
                    Application.AddMessageFilter(this);
                    return true;
                }
            }

            void EndDrag(bool abort)
            {
                ReleaseHandle();
                Application.RemoveMessageFilter(this);
                DragControl.FindForm().Capture = false;

                OnEndDrag(abort);
            }

            protected abstract void OnDragging();

            protected abstract void OnEndDrag(bool abort);

            protected virtual bool OnPreFilterMessage(ref Message m)
            {
                return false;
            }

            protected override sealed void WndProc(ref Message m)
            {
                if (m.Msg == (int)Msgs.WM_CANCELMODE || m.Msg == (int)Msgs.WM_CAPTURECHANGED)
                    EndDrag(true);

                base.WndProc(ref m);
            }

            #region IMessageFilter Members

            bool IMessageFilter.PreFilterMessage(ref Message m)
            {
                if (m.Msg == (int)Msgs.WM_MOUSEMOVE)
                    OnDragging();
                else if (m.Msg == (int)Msgs.WM_LBUTTONUP)
                    EndDrag(false);
                else if (m.Msg == (int)Msgs.WM_CAPTURECHANGED)
                    EndDrag(true);
                else if (m.Msg == (int)Msgs.WM_KEYDOWN && (int)m.WParam == (int)Keys.Escape)
                    EndDrag(true);

                return OnPreFilterMessage(ref m);
            }

            #endregion
        }
    }
}