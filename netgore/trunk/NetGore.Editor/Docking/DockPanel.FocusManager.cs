using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using NetGore.Editor.Docking.Win32;

namespace NetGore.Editor.Docking
{
    partial class DockPanel
    {
        static readonly object ActiveContentChangedEvent = new object();
        static readonly object ActiveDocumentChangedEvent = new object();
        static readonly object ActivePaneChangedEvent = new object();

        [LocalizedCategory("Category_PropertyChanged")]
        [LocalizedDescription("DockPanel_ActiveContentChanged_Description")]
        public event EventHandler ActiveContentChanged
        {
            add { Events.AddHandler(ActiveContentChangedEvent, value); }
            remove { Events.RemoveHandler(ActiveContentChangedEvent, value); }
        }

        [LocalizedCategory("Category_PropertyChanged")]
        [LocalizedDescription("DockPanel_ActiveDocumentChanged_Description")]
        public event EventHandler ActiveDocumentChanged
        {
            add { Events.AddHandler(ActiveDocumentChangedEvent, value); }
            remove { Events.RemoveHandler(ActiveDocumentChangedEvent, value); }
        }

        [LocalizedCategory("Category_PropertyChanged")]
        [LocalizedDescription("DockPanel_ActivePaneChanged_Description")]
        public event EventHandler ActivePaneChanged
        {
            add { Events.AddHandler(ActivePaneChangedEvent, value); }
            remove { Events.RemoveHandler(ActivePaneChangedEvent, value); }
        }

        [Browsable(false)]
        public IDockContent ActiveContent
        {
            get { return FocusManager.ActiveContent; }
        }

        [Browsable(false)]
        public IDockContent ActiveDocument
        {
            get { return FocusManager.ActiveDocument; }
        }

        [Browsable(false)]
        public DockPane ActiveDocumentPane
        {
            get { return FocusManager.ActiveDocumentPane; }
        }

        [Browsable(false)]
        public DockPane ActivePane
        {
            get { return FocusManager.ActivePane; }
        }

        internal IContentFocusManager ContentFocusManager
        {
            get { return m_focusManager; }
        }

        IFocusManager FocusManager
        {
            get { return m_focusManager; }
        }

        protected void OnActiveContentChanged(EventArgs e)
        {
            var handler = (EventHandler)Events[ActiveContentChangedEvent];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnActiveDocumentChanged(EventArgs e)
        {
            var handler = (EventHandler)Events[ActiveDocumentChangedEvent];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnActivePaneChanged(EventArgs e)
        {
            var handler = (EventHandler)Events[ActivePaneChangedEvent];
            if (handler != null)
                handler(this, e);
        }

        internal void SaveFocus()
        {
            DummyControl.Focus();
        }

        class FocusManagerImpl : Component, IContentFocusManager, IFocusManager
        {
            readonly DockPanel m_dockPanel;
            readonly LocalWindowsHook.HookEventHandler m_hookEventHandler;

            readonly List<IDockContent> m_listContent = new List<IDockContent>();
            readonly LocalWindowsHook m_localWindowsHook;
            IDockContent m_activeContent = null;
            IDockContent m_activeDocument = null;
            DockPane m_activeDocumentPane = null;
            DockPane m_activePane = null;
            IDockContent m_contentActivating = null;
            int m_countSuspendFocusTracking = 0;
            bool m_disposed = false;
            bool m_inRefreshActiveWindow = false;

            IDockContent m_lastActiveContent = null;

            public FocusManagerImpl(DockPanel dockPanel)
            {
                m_dockPanel = dockPanel;
                m_localWindowsHook = new LocalWindowsHook(HookType.WH_CALLWNDPROCRET);
                m_hookEventHandler = new LocalWindowsHook.HookEventHandler(HookEventHandler);
                m_localWindowsHook.HookInvoked += m_hookEventHandler;
                m_localWindowsHook.Install();
            }

            IDockContent ContentActivating
            {
                get { return m_contentActivating; }
                set { m_contentActivating = value; }
            }

            public DockPanel DockPanel
            {
                get { return m_dockPanel; }
            }

            bool InRefreshActiveWindow
            {
                get { return m_inRefreshActiveWindow; }
            }

            IDockContent LastActiveContent
            {
                get { return m_lastActiveContent; }
                set { m_lastActiveContent = value; }
            }

            List<IDockContent> ListContent
            {
                get { return m_listContent; }
            }

            void AddLastToActiveList(IDockContent content)
            {
                var last = LastActiveContent;
                if (last == content)
                    return;

                var handler = content.DockHandler;

                if (IsInActiveList(content))
                    RemoveFromActiveList(content);

                handler.PreviousActive = last;
                handler.NextActive = null;
                LastActiveContent = content;
                if (last != null)
                    last.DockHandler.NextActive = LastActiveContent;
            }

            static bool ContentContains(IDockContent content, IntPtr hWnd)
            {
                var control = FromChildHandle(hWnd);
                for (var parent = control; parent != null; parent = parent.Parent)
                {
                    if (parent == content.DockHandler.Form)
                        return true;
                }

                return false;
            }

            protected override void Dispose(bool disposing)
            {
                lock (this)
                {
                    try
                    {
                        if (!m_disposed && disposing)
                        {
                            m_localWindowsHook.Dispose();
                            m_disposed = true;
                        }
                    }
                    finally
                    {
                        base.Dispose(disposing);
                    }
                }
            }

            DockPane GetPaneFromHandle(IntPtr hWnd)
            {
                var control = FromChildHandle(hWnd);

                IDockContent content = null;
                DockPane pane = null;
                for (; control != null; control = control.Parent)
                {
                    content = control as IDockContent;
                    if (content != null)
                        content.DockHandler.ActiveWindowHandle = hWnd;

                    if (content != null && content.DockHandler.DockPanel == DockPanel)
                        return content.DockHandler.Pane;

                    pane = control as DockPane;
                    if (pane != null && pane.DockPanel == DockPanel)
                        break;
                }

                return pane;
            }

            void HookEventHandler(object sender, HookEventArgs e)
            {
                var msg = (Msgs)Marshal.ReadInt32(e.lParam, IntPtr.Size * 3);

                if (msg == Msgs.WM_KILLFOCUS)
                {
                    var wParam = Marshal.ReadIntPtr(e.lParam, IntPtr.Size * 2);
                    var pane = GetPaneFromHandle(wParam);
                    if (pane == null)
                        RefreshActiveWindow();
                }
                else if (msg == Msgs.WM_SETFOCUS)
                    RefreshActiveWindow();
            }

            bool IsInActiveList(IDockContent content)
            {
                return !(content.DockHandler.NextActive == null && LastActiveContent != content);
            }

            void RefreshActiveWindow()
            {
                SuspendFocusTracking();
                m_inRefreshActiveWindow = true;

                var oldActivePane = ActivePane;
                var oldActiveContent = ActiveContent;
                var oldActiveDocument = ActiveDocument;

                SetActivePane();
                SetActiveContent();
                SetActiveDocumentPane();
                SetActiveDocument();
                DockPanel.AutoHideWindow.RefreshActivePane();

                ResumeFocusTracking();
                m_inRefreshActiveWindow = false;

                if (oldActiveContent != ActiveContent)
                    DockPanel.OnActiveContentChanged(EventArgs.Empty);
                if (oldActiveDocument != ActiveDocument)
                    DockPanel.OnActiveDocumentChanged(EventArgs.Empty);
                if (oldActivePane != ActivePane)
                    DockPanel.OnActivePaneChanged(EventArgs.Empty);
            }

            void RemoveFromActiveList(IDockContent content)
            {
                if (LastActiveContent == content)
                    LastActiveContent = content.DockHandler.PreviousActive;

                var prev = content.DockHandler.PreviousActive;
                var next = content.DockHandler.NextActive;
                if (prev != null)
                    prev.DockHandler.NextActive = next;
                if (next != null)
                    next.DockHandler.PreviousActive = prev;

                content.DockHandler.PreviousActive = null;
                content.DockHandler.NextActive = null;
            }

            internal void SetActiveContent()
            {
                var value = ActivePane == null ? null : ActivePane.ActiveContent;

                if (m_activeContent == value)
                    return;

                if (m_activeContent != null)
                    m_activeContent.DockHandler.IsActivated = false;

                m_activeContent = value;

                if (m_activeContent != null)
                {
                    m_activeContent.DockHandler.IsActivated = true;
                    if (!DockHelper.IsDockStateAutoHide((m_activeContent.DockHandler.DockState)))
                        AddLastToActiveList(m_activeContent);
                }
            }

            void SetActiveDocument()
            {
                var value = ActiveDocumentPane == null ? null : ActiveDocumentPane.ActiveContent;

                if (m_activeDocument == value)
                    return;

                m_activeDocument = value;
            }

            void SetActiveDocumentPane()
            {
                DockPane value = null;

                if (ActivePane != null && ActivePane.DockState == DockState.Document)
                    value = ActivePane;

                if (value == null && DockPanel.DockWindows != null)
                {
                    if (ActiveDocumentPane == null)
                        value = DockPanel.DockWindows[DockState.Document].DefaultPane;
                    else if (ActiveDocumentPane.DockPanel != DockPanel || ActiveDocumentPane.DockState != DockState.Document)
                        value = DockPanel.DockWindows[DockState.Document].DefaultPane;
                    else
                        value = ActiveDocumentPane;
                }

                if (m_activeDocumentPane == value)
                    return;

                if (m_activeDocumentPane != null)
                    m_activeDocumentPane.SetIsActiveDocumentPane(false);

                m_activeDocumentPane = value;

                if (m_activeDocumentPane != null)
                    m_activeDocumentPane.SetIsActiveDocumentPane(true);
            }

            void SetActivePane()
            {
                var value = GetPaneFromHandle(NativeMethods.GetFocus());
                if (m_activePane == value)
                    return;

                if (m_activePane != null)
                    m_activePane.SetIsActivated(false);

                m_activePane = value;

                if (m_activePane != null)
                    m_activePane.SetIsActivated(true);
            }

            #region IContentFocusManager Members

            public void Activate(IDockContent content)
            {
                if (IsFocusTrackingSuspended)
                {
                    ContentActivating = content;
                    return;
                }

                if (content == null)
                    return;
                var handler = content.DockHandler;
                if (handler.Form.IsDisposed)
                    return; // Should not reach here, but better than throwing an exception
                if (ContentContains(content, handler.ActiveWindowHandle))
                    NativeMethods.SetFocus(handler.ActiveWindowHandle);
                if (!handler.Form.ContainsFocus)
                {
                    if (!handler.Form.SelectNextControl(handler.Form.ActiveControl, true, true, true, true))
                        // Since DockContent Form is not selectalbe, use Win32 SetFocus instead
                        NativeMethods.SetFocus(handler.Form.Handle);
                }
            }

            public void AddToList(IDockContent content)
            {
                if (ListContent.Contains(content) || IsInActiveList(content))
                    return;

                ListContent.Add(content);
            }

            public void GiveUpFocus(IDockContent content)
            {
                var handler = content.DockHandler;
                if (!handler.Form.ContainsFocus)
                    return;

                if (IsFocusTrackingSuspended)
                    DockPanel.DummyControl.Focus();

                if (LastActiveContent == content)
                {
                    var prev = handler.PreviousActive;
                    if (prev != null)
                        Activate(prev);
                    else if (ListContent.Count > 0)
                        Activate(ListContent[ListContent.Count - 1]);
                }
                else if (LastActiveContent != null)
                    Activate(LastActiveContent);
                else if (ListContent.Count > 0)
                    Activate(ListContent[ListContent.Count - 1]);
            }

            public void RemoveFromList(IDockContent content)
            {
                if (IsInActiveList(content))
                    RemoveFromActiveList(content);
                if (ListContent.Contains(content))
                    ListContent.Remove(content);
            }

            #endregion

            #region IFocusManager Members

            public IDockContent ActiveContent
            {
                get { return m_activeContent; }
            }

            public IDockContent ActiveDocument
            {
                get { return m_activeDocument; }
            }

            public DockPane ActiveDocumentPane
            {
                get { return m_activeDocumentPane; }
            }

            public DockPane ActivePane
            {
                get { return m_activePane; }
            }

            public bool IsFocusTrackingSuspended
            {
                get { return m_countSuspendFocusTracking != 0; }
            }

            public void ResumeFocusTracking()
            {
                if (m_countSuspendFocusTracking > 0)
                    m_countSuspendFocusTracking--;

                if (m_countSuspendFocusTracking == 0)
                {
                    if (ContentActivating != null)
                    {
                        Activate(ContentActivating);
                        ContentActivating = null;
                    }
                    m_localWindowsHook.HookInvoked += m_hookEventHandler;
                    if (!InRefreshActiveWindow)
                        RefreshActiveWindow();
                }
            }

            public void SuspendFocusTracking()
            {
                m_countSuspendFocusTracking++;
                m_localWindowsHook.HookInvoked -= m_hookEventHandler;
            }

            #endregion

            class HookEventArgs : EventArgs
            {
                [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
                public int HookCode;

                public IntPtr lParam;

                [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
                public IntPtr wParam;
            }

            class LocalWindowsHook : IDisposable
            {
                // Internal properties
                public delegate void HookEventHandler(object sender, HookEventArgs e);

                readonly NativeMethods.HookProc m_filterFunc = null;
                readonly HookType m_hookType;
                IntPtr m_hHook = IntPtr.Zero;

                // Event delegate

                public LocalWindowsHook(HookType hook)
                {
                    m_hookType = hook;
                    m_filterFunc = new NativeMethods.HookProc(CoreHookProc);
                }

                public event HookEventHandler HookInvoked;

                // Default filter function
                public IntPtr CoreHookProc(int code, IntPtr wParam, IntPtr lParam)
                {
                    if (code < 0)
                        return NativeMethods.CallNextHookEx(m_hHook, code, wParam, lParam);

                    // Let clients determine what to do
                    var e = new HookEventArgs();
                    e.HookCode = code;
                    e.wParam = wParam;
                    e.lParam = lParam;
                    OnHookInvoked(e);

                    // Yield to the next hook in the chain
                    return NativeMethods.CallNextHookEx(m_hHook, code, wParam, lParam);
                }

                protected virtual void Dispose(bool disposing)
                {
                    Uninstall();
                }

                ~LocalWindowsHook()
                {
                    Dispose(false);
                }

                // Install the hook
                public void Install()
                {
                    if (m_hHook != IntPtr.Zero)
                        Uninstall();

                    var threadId = NativeMethods.GetCurrentThreadId();
                    m_hHook = NativeMethods.SetWindowsHookEx(m_hookType, m_filterFunc, IntPtr.Zero, threadId);
                }

                protected void OnHookInvoked(HookEventArgs e)
                {
                    if (HookInvoked != null)
                        HookInvoked(this, e);
                }

                // Uninstall the hook
                public void Uninstall()
                {
                    if (m_hHook != IntPtr.Zero)
                    {
                        NativeMethods.UnhookWindowsHookEx(m_hHook);
                        m_hHook = IntPtr.Zero;
                    }
                }

                #region IDisposable Members

                public void Dispose()
                {
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }

                #endregion
            }
        }

        interface IFocusManager
        {
            IDockContent ActiveContent { get; }
            IDockContent ActiveDocument { get; }
            DockPane ActiveDocumentPane { get; }
            DockPane ActivePane { get; }
            bool IsFocusTrackingSuspended { get; }

            void ResumeFocusTracking();

            void SuspendFocusTracking();
        }
    }
}