using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Windows.Forms;
using NetGore.Editor.Docking.Win32;

namespace NetGore.Editor.Docking
{
    [ToolboxItem(false)]
    public partial class DockPane : UserControl, IDockDragSource
    {
        public enum AppearanceStyle
        {
            ToolWindow,
            Document
        }

        static readonly object DockStateChangedEvent = new object();
        static readonly object IsActivatedChangedEvent = new object();
        static readonly object IsActiveDocumentPaneChangedEvent = new object();
        IDockContent m_activeContent = null;
        bool m_allowDockDragAndDrop = true;
        IDisposable m_autoHidePane = null;
        object m_autoHideTabs = null;

        DockPaneCaptionBase m_captionControl;
        DockContentCollection m_contents;
        int m_countRefreshStateChange = 0;
        DockContentCollection m_displayingContents;
        DockPanel m_dockPanel;
        DockState m_dockState = DockState.Unknown;
        bool m_isActivated = false;
        bool m_isActiveDocumentPane = false;
        bool m_isFloat;
        bool m_isHidden = true;
        NestedDockingStatus m_nestedDockingStatus;

        DockPaneStripBase m_tabStripControl;

        protected internal DockPane(IDockContent content, DockState visibleState, bool show)
        {
            InternalConstruct(content, visibleState, false, Rectangle.Empty, null, DockAlignment.Right, 0.5, show);
        }

        [SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId = "1#")]
        protected internal DockPane(IDockContent content, FloatWindow floatWindow, bool show)
        {
            if (floatWindow == null)
                throw new ArgumentNullException("floatWindow");

            InternalConstruct(content, DockState.Float, false, Rectangle.Empty,
                              floatWindow.NestedPanes.GetDefaultPreviousPane(this), DockAlignment.Right, 0.5, show);
        }

        protected internal DockPane(IDockContent content, DockPane previousPane, DockAlignment alignment, double proportion,
                                    bool show)
        {
            if (previousPane == null)
                throw (new ArgumentNullException("previousPane"));
            InternalConstruct(content, previousPane.DockState, false, Rectangle.Empty, previousPane, alignment, proportion, show);
        }

        [SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId = "1#")]
        protected internal DockPane(IDockContent content, Rectangle floatWindowBounds, bool show)
        {
            InternalConstruct(content, DockState.Float, true, floatWindowBounds, null, DockAlignment.Right, 0.5, show);
        }

        public event EventHandler DockStateChanged
        {
            add { Events.AddHandler(DockStateChangedEvent, value); }
            remove { Events.RemoveHandler(DockStateChangedEvent, value); }
        }

        public event EventHandler IsActivatedChanged
        {
            add { Events.AddHandler(IsActivatedChangedEvent, value); }
            remove { Events.RemoveHandler(IsActivatedChangedEvent, value); }
        }

        public event EventHandler IsActiveDocumentPaneChanged
        {
            add { Events.AddHandler(IsActiveDocumentPaneChangedEvent, value); }
            remove { Events.RemoveHandler(IsActiveDocumentPaneChangedEvent, value); }
        }

        public virtual IDockContent ActiveContent
        {
            get { return m_activeContent; }
            set
            {
                if (ActiveContent == value)
                    return;

                if (value != null)
                {
                    if (!DisplayingContents.Contains(value))
                        throw (new InvalidOperationException(Strings.DockPane_ActiveContent_InvalidValue));
                }
                else
                {
                    if (DisplayingContents.Count != 0)
                        throw (new InvalidOperationException(Strings.DockPane_ActiveContent_InvalidValue));
                }

                var oldValue = m_activeContent;

                if (DockPanel.ActiveAutoHideContent == oldValue)
                    DockPanel.ActiveAutoHideContent = null;

                m_activeContent = value;

                if (DockPanel.DocumentStyle == DocumentStyle.DockingMdi && DockState == DockState.Document)
                {
                    if (m_activeContent != null)
                        m_activeContent.DockHandler.Form.BringToFront();
                }
                else
                {
                    if (m_activeContent != null)
                        m_activeContent.DockHandler.SetVisible();
                    if (oldValue != null && DisplayingContents.Contains(oldValue))
                        oldValue.DockHandler.SetVisible();
                    if (IsActivated && m_activeContent != null)
                        m_activeContent.DockHandler.Activate();
                }

                if (FloatWindow != null)
                    FloatWindow.SetText();

                if (DockPanel.DocumentStyle == DocumentStyle.DockingMdi && DockState == DockState.Document)
                    RefreshChanges(false); // delayed layout to reduce screen flicker
                else
                    RefreshChanges();

                if (m_activeContent != null)
                    TabStripControl.EnsureTabVisible(m_activeContent);
            }
        }

        public virtual bool AllowDockDragAndDrop
        {
            get { return m_allowDockDragAndDrop; }
            set { m_allowDockDragAndDrop = value; }
        }

        public AppearanceStyle Appearance
        {
            get { return (DockState == DockState.Document) ? AppearanceStyle.Document : AppearanceStyle.ToolWindow; }
        }

        internal IDisposable AutoHidePane
        {
            get { return m_autoHidePane; }
            set { m_autoHidePane = value; }
        }

        internal object AutoHideTabs
        {
            get { return m_autoHideTabs; }
            set { m_autoHideTabs = value; }
        }

        DockPaneCaptionBase CaptionControl
        {
            get { return m_captionControl; }
        }

        Rectangle CaptionRectangle
        {
            get
            {
                if (!HasCaption)
                    return Rectangle.Empty;

                var rectWindow = DisplayingRectangle;
                int x, y, width;
                x = rectWindow.X;
                y = rectWindow.Y;
                width = rectWindow.Width;
                var height = CaptionControl.MeasureHeight();

                return new Rectangle(x, y, width, height);
            }
        }

        public virtual string CaptionText
        {
            get { return ActiveContent == null ? string.Empty : ActiveContent.DockHandler.TabText; }
        }

        internal Rectangle ContentRectangle
        {
            get
            {
                var rectWindow = DisplayingRectangle;
                var rectCaption = CaptionRectangle;
                var rectTabStrip = TabStripRectangle;

                var x = rectWindow.X;

                var y = rectWindow.Y + (rectCaption.IsEmpty ? 0 : rectCaption.Height);
                if (DockState == DockState.Document && DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Top)
                    y += rectTabStrip.Height;

                var width = rectWindow.Width;
                var height = rectWindow.Height - rectCaption.Height - rectTabStrip.Height;

                return new Rectangle(x, y, width, height);
            }
        }

        public DockContentCollection Contents
        {
            get { return m_contents; }
        }

        public DockContentCollection DisplayingContents
        {
            get { return m_displayingContents; }
        }

        internal Rectangle DisplayingRectangle
        {
            get { return ClientRectangle; }
        }

        public DockPanel DockPanel
        {
            get { return m_dockPanel; }
        }

        public DockState DockState
        {
            get { return m_dockState; }
            set { SetDockState(value); }
        }

        public DockWindow DockWindow
        {
            get
            {
                return (m_nestedDockingStatus.NestedPanes == null)
                           ? null : m_nestedDockingStatus.NestedPanes.Container as DockWindow;
            }
            set
            {
                var oldValue = DockWindow;
                if (oldValue == value)
                    return;

                DockTo(value);
            }
        }

        public FloatWindow FloatWindow
        {
            get
            {
                return (m_nestedDockingStatus.NestedPanes == null)
                           ? null : m_nestedDockingStatus.NestedPanes.Container as FloatWindow;
            }
            set
            {
                var oldValue = FloatWindow;
                if (oldValue == value)
                    return;

                DockTo(value);
            }
        }

        bool HasCaption
        {
            get
            {
                if (DockState == DockState.Document || DockState == DockState.Hidden || DockState == DockState.Unknown ||
                    (DockState == DockState.Float && FloatWindow.VisibleNestedPanes.Count <= 1))
                    return false;
                else
                    return true;
            }
        }

        internal bool HasTabPageContextMenu
        {
            get { return TabPageContextMenu != null; }
        }

        public bool IsActivated
        {
            get { return m_isActivated; }
        }

        public bool IsActiveDocumentPane
        {
            get { return m_isActiveDocumentPane; }
        }

        public bool IsAutoHide
        {
            get { return DockHelper.IsDockStateAutoHide(DockState); }
        }

        public bool IsFloat
        {
            get { return m_isFloat; }
        }

        public bool IsHidden
        {
            get { return m_isHidden; }
        }

        bool IsRefreshStateChangeSuspended
        {
            get { return m_countRefreshStateChange != 0; }
        }

        public NestedDockingStatus NestedDockingStatus
        {
            get { return m_nestedDockingStatus; }
        }

        public INestedPanesContainer NestedPanesContainer
        {
            get
            {
                if (NestedDockingStatus.NestedPanes == null)
                    return null;
                else
                    return NestedDockingStatus.NestedPanes.Container;
            }
        }

        object TabPageContextMenu
        {
            get
            {
                var content = ActiveContent;

                if (content == null)
                    return null;

                if (content.DockHandler.TabPageContextMenuStrip != null)
                    return content.DockHandler.TabPageContextMenuStrip;
                else if (content.DockHandler.TabPageContextMenu != null)
                    return content.DockHandler.TabPageContextMenu;
                else
                    return null;
            }
        }

        internal DockPaneStripBase TabStripControl
        {
            get { return m_tabStripControl; }
        }

        internal Rectangle TabStripRectangle
        {
            get
            {
                if (Appearance == AppearanceStyle.ToolWindow)
                    return TabStripRectangle_ToolWindow;
                else
                    return TabStripRectangle_Document;
            }
        }

        Rectangle TabStripRectangle_Document
        {
            get
            {
                if (DisplayingContents.Count == 0)
                    return Rectangle.Empty;

                if (DisplayingContents.Count == 1 && DockPanel.DocumentStyle == DocumentStyle.DockingSdi)
                    return Rectangle.Empty;

                var rectWindow = DisplayingRectangle;
                var x = rectWindow.X;
                var width = rectWindow.Width;
                var height = TabStripControl.MeasureHeight();

                var y = 0;
                if (DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                    y = rectWindow.Height - height;
                else
                    y = rectWindow.Y;

                return new Rectangle(x, y, width, height);
            }
        }

        Rectangle TabStripRectangle_ToolWindow
        {
            get
            {
                if (DisplayingContents.Count <= 1 || IsAutoHide)
                    return Rectangle.Empty;

                var rectWindow = DisplayingRectangle;

                var width = rectWindow.Width;
                var height = TabStripControl.MeasureHeight();
                var x = rectWindow.X;
                var y = rectWindow.Bottom - height;
                var rectCaption = CaptionRectangle;
                if (rectCaption.Contains(x, y))
                    y = rectCaption.Y + rectCaption.Height;

                return new Rectangle(x, y, width, height);
            }
        }

        public void Activate()
        {
            if (DockHelper.IsDockStateAutoHide(DockState) && DockPanel.ActiveAutoHideContent != ActiveContent)
                DockPanel.ActiveAutoHideContent = ActiveContent;
            else if (!IsActivated && ActiveContent != null)
                ActiveContent.DockHandler.Activate();
        }

        internal void AddContent(IDockContent content)
        {
            if (Contents.Contains(content))
                return;

            Contents.Add(content);
        }

        internal void Close()
        {
            Dispose();
        }

        public void CloseActiveContent()
        {
            CloseContent(ActiveContent);
        }

        internal void CloseContent(IDockContent content)
        {
            var dockPanel = DockPanel;
            dockPanel.SuspendLayout(true);

            if (content == null)
                return;

            if (!content.DockHandler.CloseButton)
                return;

            if (content.DockHandler.HideOnClose)
                content.DockHandler.Hide();
            else
                content.DockHandler.Close();

            dockPanel.ResumeLayout(true, true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_dockState = DockState.Unknown;

                if (NestedPanesContainer != null)
                    NestedPanesContainer.NestedPanes.Remove(this);

                if (DockPanel != null)
                {
                    DockPanel.RemovePane(this);
                    m_dockPanel = null;
                }

                Splitter.Dispose();
                if (m_autoHidePane != null)
                    m_autoHidePane.Dispose();
            }
            base.Dispose(disposing);
        }

        public DockPane DockTo(INestedPanesContainer container)
        {
            if (container == null)
                throw new InvalidOperationException(Strings.DockPane_DockTo_NullContainer);

            DockAlignment alignment;
            if (container.DockState == DockState.DockLeft || container.DockState == DockState.DockRight)
                alignment = DockAlignment.Bottom;
            else
                alignment = DockAlignment.Right;

            return DockTo(container, container.NestedPanes.GetDefaultPreviousPane(this), alignment, 0.5);
        }

        public DockPane DockTo(INestedPanesContainer container, DockPane previousPane, DockAlignment alignment, double proportion)
        {
            if (container == null)
                throw new InvalidOperationException(Strings.DockPane_DockTo_NullContainer);

            if (container.IsFloat == IsFloat)
            {
                InternalAddToDockList(container, previousPane, alignment, proportion);
                return this;
            }

            var firstContent = GetFirstContent(container.DockState);
            if (firstContent == null)
                return null;

            DockPane pane;
            DockPanel.DummyContent.DockPanel = DockPanel;
            if (container.IsFloat)
                pane = DockPanel.DockPaneFactory.CreateDockPane(DockPanel.DummyContent, (FloatWindow)container, true);
            else
                pane = DockPanel.DockPaneFactory.CreateDockPane(DockPanel.DummyContent, container.DockState, true);

            pane.DockTo(container, previousPane, alignment, proportion);
            SetVisibleContentsToPane(pane);
            DockPanel.DummyContent.DockPanel = null;

            return pane;
        }

        public DockPane Float()
        {
            DockPanel.SuspendLayout(true);

            var activeContent = ActiveContent;

            var floatPane = GetFloatPaneFromContents();
            if (floatPane == null)
            {
                var firstContent = GetFirstContent(DockState.Float);
                if (firstContent == null)
                {
                    DockPanel.ResumeLayout(true, true);
                    return null;
                }
                floatPane = DockPanel.DockPaneFactory.CreateDockPane(firstContent, DockState.Float, true);
            }
            SetVisibleContentsToPane(floatPane, activeContent);

            DockPanel.ResumeLayout(true, true);
            return floatPane;
        }

        IDockContent GetFirstContent(DockState dockState)
        {
            for (var i = 0; i < DisplayingContents.Count; i++)
            {
                var content = DisplayingContents[i];
                if (content.DockHandler.IsDockStateValid(dockState))
                    return content;
            }
            return null;
        }

        DockPane GetFloatPaneFromContents()
        {
            DockPane floatPane = null;
            for (var i = 0; i < DisplayingContents.Count; i++)
            {
                var content = DisplayingContents[i];
                if (!content.DockHandler.IsDockStateValid(DockState.Float))
                    continue;

                if (floatPane != null && content.DockHandler.FloatPane != floatPane)
                    return null;
                else
                    floatPane = content.DockHandler.FloatPane;
            }

            return floatPane;
        }

        IDockContent GetFocusedContent()
        {
            IDockContent contentFocused = null;
            foreach (var content in Contents)
            {
                if (content.DockHandler.Form.ContainsFocus)
                {
                    contentFocused = content;
                    break;
                }
            }

            return contentFocused;
        }

        HitTestResult GetHitTest(Point ptMouse)
        {
            var ptMouseClient = PointToClient(ptMouse);

            var rectCaption = CaptionRectangle;
            if (rectCaption.Contains(ptMouseClient))
                return new HitTestResult(HitTestArea.Caption, -1);

            var rectContent = ContentRectangle;
            if (rectContent.Contains(ptMouseClient))
                return new HitTestResult(HitTestArea.Content, -1);

            var rectTabStrip = TabStripRectangle;
            if (rectTabStrip.Contains(ptMouseClient))
                return new HitTestResult(HitTestArea.TabStrip, TabStripControl.HitTest(TabStripControl.PointToClient(ptMouse)));

            return new HitTestResult(HitTestArea.None, -1);
        }

        void InternalAddToDockList(INestedPanesContainer container, DockPane prevPane, DockAlignment alignment, double proportion)
        {
            if ((container.DockState == DockState.Float) != IsFloat)
                throw new InvalidOperationException(Strings.DockPane_DockTo_InvalidContainer);

            var count = container.NestedPanes.Count;
            if (container.NestedPanes.Contains(this))
                count --;
            if (prevPane == null && count > 0)
                throw new InvalidOperationException(Strings.DockPane_DockTo_NullPrevPane);

            if (prevPane != null && !container.NestedPanes.Contains(prevPane))
                throw new InvalidOperationException(Strings.DockPane_DockTo_NoPrevPane);

            if (prevPane == this)
                throw new InvalidOperationException(Strings.DockPane_DockTo_SelfPrevPane);

            var oldContainer = NestedPanesContainer;
            var oldDockState = DockState;
            container.NestedPanes.Add(this);
            NestedDockingStatus.SetStatus(container.NestedPanes, prevPane, alignment, proportion);

            if (DockHelper.IsDockWindowState(DockState))
                m_dockState = container.DockState;

            RefreshStateChange(oldContainer, oldDockState);
        }

        void InternalConstruct(IDockContent content, DockState dockState, bool flagBounds, Rectangle floatWindowBounds,
                               DockPane prevPane, DockAlignment alignment, double proportion, bool show)
        {
            if (dockState == DockState.Hidden || dockState == DockState.Unknown)
                throw new ArgumentException(Strings.DockPane_SetDockState_InvalidState);

            if (content == null)
                throw new ArgumentNullException(Strings.DockPane_Constructor_NullContent);

            if (content.DockHandler.DockPanel == null)
                throw new ArgumentException(Strings.DockPane_Constructor_NullDockPanel);

            SuspendLayout();
            SetStyle(ControlStyles.Selectable, false);

            m_isFloat = (dockState == DockState.Float);

            m_contents = new DockContentCollection();
            m_displayingContents = new DockContentCollection(this);
            m_dockPanel = content.DockHandler.DockPanel;
            m_dockPanel.AddPane(this);

            m_splitter = new SplitterControl(this);

            m_nestedDockingStatus = new NestedDockingStatus(this);

            m_captionControl = DockPanel.DockPaneCaptionFactory.CreateDockPaneCaption(this);
            m_tabStripControl = DockPanel.DockPaneStripFactory.CreateDockPaneStrip(this);
            Controls.AddRange(new Control[] { m_captionControl, m_tabStripControl });

            DockPanel.SuspendLayout(true);
            if (flagBounds)
                FloatWindow = DockPanel.FloatWindowFactory.CreateFloatWindow(DockPanel, this, floatWindowBounds);
            else if (prevPane != null)
                DockTo(prevPane.NestedPanesContainer, prevPane, alignment, proportion);

            SetDockState(dockState);
            if (show)
                content.DockHandler.Pane = this;
            else if (IsFloat)
                content.DockHandler.FloatPane = this;
            else
                content.DockHandler.PanelPane = this;

            ResumeLayout();
            DockPanel.ResumeLayout(true, true);
        }

        void InternalSetDockState(DockState value)
        {
            if (m_dockState == value)
                return;

            var oldDockState = m_dockState;
            var oldContainer = NestedPanesContainer;

            m_dockState = value;

            SuspendRefreshStateChange();

            var contentFocused = GetFocusedContent();
            if (contentFocused != null)
                DockPanel.SaveFocus();

            if (!IsFloat)
                DockWindow = DockPanel.DockWindows[DockState];
            else if (FloatWindow == null)
                FloatWindow = DockPanel.FloatWindowFactory.CreateFloatWindow(DockPanel, this);

            if (contentFocused != null)
                DockPanel.ContentFocusManager.Activate(contentFocused);

            ResumeRefreshStateChange(oldContainer, oldDockState);
        }

        public bool IsDockStateValid(DockState dockState)
        {
            foreach (var content in Contents)
            {
                if (!content.DockHandler.IsDockStateValid(dockState))
                    return false;
            }

            return true;
        }

        protected virtual void OnDockStateChanged(EventArgs e)
        {
            var handler = (EventHandler)Events[DockStateChangedEvent];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnIsActivatedChanged(EventArgs e)
        {
            var handler = (EventHandler)Events[IsActivatedChangedEvent];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnIsActiveDocumentPaneChanged(EventArgs e)
        {
            var handler = (EventHandler)Events[IsActiveDocumentPaneChangedEvent];
            if (handler != null)
                handler(this, e);
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            SetIsHidden(DisplayingContents.Count == 0);
            if (!IsHidden)
            {
                CaptionControl.Bounds = CaptionRectangle;
                TabStripControl.Bounds = TabStripRectangle;

                SetContentBounds();

                foreach (var content in Contents)
                {
                    if (DisplayingContents.Contains(content))
                    {
                        if (content.DockHandler.FlagClipWindow && content.DockHandler.Form.Visible)
                            content.DockHandler.FlagClipWindow = false;
                    }
                }
            }

            base.OnLayout(levent);
        }

        internal void RefreshChanges()
        {
            RefreshChanges(true);
        }

        void RefreshChanges(bool performLayout)
        {
            if (IsDisposed)
                return;

            CaptionControl.RefreshChanges();
            TabStripControl.RefreshChanges();
            if (DockState == DockState.Float)
                FloatWindow.RefreshChanges();
            if (DockHelper.IsDockStateAutoHide(DockState) && DockPanel != null)
            {
                DockPanel.RefreshAutoHideStrip();
                DockPanel.PerformLayout();
            }

            if (performLayout)
                PerformLayout();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2002:DoNotLockOnObjectsWithWeakIdentity")]
        void RefreshStateChange(INestedPanesContainer oldContainer, DockState oldDockState)
        {
            lock (this)
            {
                if (IsRefreshStateChangeSuspended)
                    return;

                SuspendRefreshStateChange();
            }

            DockPanel.SuspendLayout(true);

            var contentFocused = GetFocusedContent();
            if (contentFocused != null)
                DockPanel.SaveFocus();
            SetParent();

            if (ActiveContent != null)
                ActiveContent.DockHandler.SetDockState(ActiveContent.DockHandler.IsHidden, DockState,
                                                       ActiveContent.DockHandler.Pane);
            foreach (var content in Contents)
            {
                if (content.DockHandler.Pane == this)
                    content.DockHandler.SetDockState(content.DockHandler.IsHidden, DockState, content.DockHandler.Pane);
            }

            if (oldContainer != null)
            {
                var oldContainerControl = (Control)oldContainer;
                if (oldContainer.DockState == oldDockState && !oldContainerControl.IsDisposed)
                    oldContainerControl.PerformLayout();
            }
            if (DockHelper.IsDockStateAutoHide(oldDockState))
                DockPanel.RefreshActiveAutoHideContent();

            if (NestedPanesContainer.DockState == DockState)
                ((Control)NestedPanesContainer).PerformLayout();
            if (DockHelper.IsDockStateAutoHide(DockState))
                DockPanel.RefreshActiveAutoHideContent();

            if (DockHelper.IsDockStateAutoHide(oldDockState) || DockHelper.IsDockStateAutoHide(DockState))
            {
                DockPanel.RefreshAutoHideStrip();
                DockPanel.PerformLayout();
            }

            ResumeRefreshStateChange();

            if (contentFocused != null)
                contentFocused.DockHandler.Activate();

            DockPanel.ResumeLayout(true, true);

            if (oldDockState != DockState)
                OnDockStateChanged(EventArgs.Empty);
        }

        internal void RemoveContent(IDockContent content)
        {
            if (!Contents.Contains(content))
                return;

            Contents.Remove(content);
        }

        public void RestoreToPanel()
        {
            DockPanel.SuspendLayout(true);

            var activeContent = DockPanel.ActiveContent;

            for (var i = DisplayingContents.Count - 1; i >= 0; i--)
            {
                var content = DisplayingContents[i];
                if (content.DockHandler.CheckDockState(false) != DockState.Unknown)
                    content.DockHandler.IsFloat = false;
            }

            DockPanel.ResumeLayout(true, true);
        }

        void ResumeRefreshStateChange()
        {
            m_countRefreshStateChange --;
            Debug.Assert(m_countRefreshStateChange >= 0);
            DockPanel.ResumeLayout(true, true);
        }

        void ResumeRefreshStateChange(INestedPanesContainer oldContainer, DockState oldDockState)
        {
            ResumeRefreshStateChange();
            RefreshStateChange(oldContainer, oldDockState);
        }

        internal void SetContentBounds()
        {
            var rectContent = ContentRectangle;
            if (DockState == DockState.Document && DockPanel.DocumentStyle == DocumentStyle.DockingMdi)
                rectContent = DockPanel.RectangleToMdiClient(RectangleToScreen(rectContent));

            var rectInactive = new Rectangle(-rectContent.Width, rectContent.Y, rectContent.Width, rectContent.Height);
            foreach (var content in Contents)
            {
                if (content.DockHandler.Pane == this)
                {
                    if (content == ActiveContent)
                        content.DockHandler.Form.Bounds = rectContent;
                    else
                        content.DockHandler.Form.Bounds = rectInactive;
                }
            }
        }

        public void SetContentIndex(IDockContent content, int index)
        {
            var oldIndex = Contents.IndexOf(content);
            if (oldIndex == -1)
                throw (new ArgumentException(Strings.DockPane_SetContentIndex_InvalidContent));

            if (index < 0 || index > Contents.Count - 1)
            {
                if (index != -1)
                    throw (new ArgumentOutOfRangeException(Strings.DockPane_SetContentIndex_InvalidIndex));
            }

            if (oldIndex == index)
                return;
            if (oldIndex == Contents.Count - 1 && index == -1)
                return;

            Contents.Remove(content);
            if (index == -1)
                Contents.Add(content);
            else if (oldIndex < index)
                Contents.AddAt(content, index - 1);
            else
                Contents.AddAt(content, index);

            RefreshChanges();
        }

        public DockPane SetDockState(DockState value)
        {
            if (value == DockState.Unknown || value == DockState.Hidden)
                throw new InvalidOperationException(Strings.DockPane_SetDockState_InvalidState);

            if ((value == DockState.Float) == IsFloat)
            {
                InternalSetDockState(value);
                return this;
            }

            if (DisplayingContents.Count == 0)
                return null;

            IDockContent firstContent = null;
            for (var i = 0; i < DisplayingContents.Count; i++)
            {
                var content = DisplayingContents[i];
                if (content.DockHandler.IsDockStateValid(value))
                {
                    firstContent = content;
                    break;
                }
            }
            if (firstContent == null)
                return null;

            firstContent.DockHandler.DockState = value;
            var pane = firstContent.DockHandler.Pane;
            DockPanel.SuspendLayout(true);
            for (var i = 0; i < DisplayingContents.Count; i++)
            {
                var content = DisplayingContents[i];
                if (content.DockHandler.IsDockStateValid(value))
                    content.DockHandler.Pane = pane;
            }
            DockPanel.ResumeLayout(true, true);
            return pane;
        }

        internal void SetIsActivated(bool value)
        {
            if (m_isActivated == value)
                return;

            m_isActivated = value;
            if (DockState != DockState.Document)
                RefreshChanges(false);
            OnIsActivatedChanged(EventArgs.Empty);
        }

        internal void SetIsActiveDocumentPane(bool value)
        {
            if (m_isActiveDocumentPane == value)
                return;

            m_isActiveDocumentPane = value;
            if (DockState == DockState.Document)
                RefreshChanges();
            OnIsActiveDocumentPaneChanged(EventArgs.Empty);
        }

        void SetIsHidden(bool value)
        {
            if (m_isHidden == value)
                return;

            m_isHidden = value;
            if (DockHelper.IsDockStateAutoHide(DockState))
            {
                DockPanel.RefreshAutoHideStrip();
                DockPanel.PerformLayout();
            }
            else if (NestedPanesContainer != null)
                ((Control)NestedPanesContainer).PerformLayout();
        }

        public void SetNestedDockingProportion(double proportion)
        {
            NestedDockingStatus.SetStatus(NestedDockingStatus.NestedPanes, NestedDockingStatus.PreviousPane,
                                          NestedDockingStatus.Alignment, proportion);
            if (NestedPanesContainer != null)
                ((Control)NestedPanesContainer).PerformLayout();
        }

        void SetParent()
        {
            if (DockState == DockState.Unknown || DockState == DockState.Hidden)
            {
                SetParent(null);
                Splitter.Parent = null;
            }
            else if (DockState == DockState.Float)
            {
                SetParent(FloatWindow);
                Splitter.Parent = FloatWindow;
            }
            else if (DockHelper.IsDockStateAutoHide(DockState))
            {
                SetParent(DockPanel.AutoHideControl);
                Splitter.Parent = null;
            }
            else
            {
                SetParent(DockPanel.DockWindows[DockState]);
                Splitter.Parent = Parent;
            }
        }

        void SetParent(Control value)
        {
            if (Parent == value)
                return;

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // Workaround of .Net Framework bug:
            // Change the parent of a control with focus may result in the first
            // MDI child form get activated. 
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            var contentFocused = GetFocusedContent();
            if (contentFocused != null)
                DockPanel.SaveFocus();

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            Parent = value;

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // Workaround of .Net Framework bug:
            // Change the parent of a control with focus may result in the first
            // MDI child form get activated. 
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (contentFocused != null)
                contentFocused.DockHandler.Activate();
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        void SetVisibleContentsToPane(DockPane pane)
        {
            SetVisibleContentsToPane(pane, ActiveContent);
        }

        void SetVisibleContentsToPane(DockPane pane, IDockContent activeContent)
        {
            for (var i = 0; i < DisplayingContents.Count; i++)
            {
                var content = DisplayingContents[i];
                if (content.DockHandler.IsDockStateValid(pane.DockState))
                {
                    content.DockHandler.Pane = pane;
                    i--;
                }
            }

            if (activeContent.DockHandler.Pane == pane)
                pane.ActiveContent = activeContent;
        }

        public new void Show()
        {
            Activate();
        }

        internal void ShowTabPageContextMenu(Control control, Point position)
        {
            var menu = TabPageContextMenu;

            if (menu == null)
                return;

            var contextMenuStrip = menu as ContextMenuStrip;
            if (contextMenuStrip != null)
            {
                contextMenuStrip.Show(control, position);
                return;
            }

            var contextMenu = menu as ContextMenu;
            if (contextMenu != null)
                contextMenu.Show(this, position);
        }

        void SuspendRefreshStateChange()
        {
            m_countRefreshStateChange ++;
            DockPanel.SuspendLayout(true);
        }

        internal void TestDrop(IDockDragSource dragSource, DockOutlineBase dockOutline)
        {
            if (!dragSource.CanDockTo(this))
                return;

            var ptMouse = MousePosition;

            var hitTestResult = GetHitTest(ptMouse);
            if (hitTestResult.HitArea == HitTestArea.Caption)
                dockOutline.Show(this, -1);
            else if (hitTestResult.HitArea == HitTestArea.TabStrip && hitTestResult.Index != -1)
                dockOutline.Show(this, hitTestResult.Index);
        }

        internal void ValidateActiveContent()
        {
            if (ActiveContent == null)
            {
                if (DisplayingContents.Count != 0)
                    ActiveContent = DisplayingContents[0];
                return;
            }

            if (DisplayingContents.IndexOf(ActiveContent) >= 0)
                return;

            IDockContent prevVisible = null;
            for (var i = Contents.IndexOf(ActiveContent) - 1; i >= 0; i--)
            {
                if (Contents[i].DockHandler.DockState == DockState)
                {
                    prevVisible = Contents[i];
                    break;
                }
            }

            IDockContent nextVisible = null;
            for (var i = Contents.IndexOf(ActiveContent) + 1; i < Contents.Count; i++)
            {
                if (Contents[i].DockHandler.DockState == DockState)
                {
                    nextVisible = Contents[i];
                    break;
                }
            }

            if (prevVisible != null)
                ActiveContent = prevVisible;
            else if (nextVisible != null)
                ActiveContent = nextVisible;
            else
                ActiveContent = null;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)Msgs.WM_MOUSEACTIVATE)
                Activate();

            base.WndProc(ref m);
        }

        #region IDockDragSource Members

        Control IDragSource.DragControl
        {
            get { return this; }
        }

        Rectangle IDockDragSource.BeginDrag(Point ptMouse)
        {
            var location = PointToScreen(new Point(0, 0));
            Size size;

            var floatPane = ActiveContent.DockHandler.FloatPane;
            if (DockState == DockState.Float || floatPane == null || floatPane.FloatWindow.NestedPanes.Count != 1)
                size = DockPanel.DefaultFloatWindowSize;
            else
                size = floatPane.FloatWindow.Size;

            if (ptMouse.X > location.X + size.Width)
                location.X += ptMouse.X - (location.X + size.Width) + Measures.SplitterSize;

            return new Rectangle(location, size);
        }

        bool IDockDragSource.CanDockTo(DockPane pane)
        {
            if (!IsDockStateValid(pane.DockState))
                return false;

            if (pane == this)
                return false;

            return true;
        }

        public void DockTo(DockPane pane, DockStyle dockStyle, int contentIndex)
        {
            if (dockStyle == DockStyle.Fill)
            {
                var activeContent = ActiveContent;
                for (var i = Contents.Count - 1; i >= 0; i--)
                {
                    var c = Contents[i];
                    if (c.DockHandler.DockState == DockState)
                    {
                        c.DockHandler.Pane = pane;
                        if (contentIndex != -1)
                            pane.SetContentIndex(c, contentIndex);
                    }
                }
                pane.ActiveContent = activeContent;
            }
            else
            {
                if (dockStyle == DockStyle.Left)
                    DockTo(pane.NestedPanesContainer, pane, DockAlignment.Left, 0.5);
                else if (dockStyle == DockStyle.Right)
                    DockTo(pane.NestedPanesContainer, pane, DockAlignment.Right, 0.5);
                else if (dockStyle == DockStyle.Top)
                    DockTo(pane.NestedPanesContainer, pane, DockAlignment.Top, 0.5);
                else if (dockStyle == DockStyle.Bottom)
                    DockTo(pane.NestedPanesContainer, pane, DockAlignment.Bottom, 0.5);

                DockState = pane.DockState;
            }
        }

        public void DockTo(DockPanel panel, DockStyle dockStyle)
        {
            if (panel != DockPanel)
                throw new ArgumentException(Strings.IDockDragSource_DockTo_InvalidPanel, "panel");

            if (dockStyle == DockStyle.Top)
                DockState = DockState.DockTop;
            else if (dockStyle == DockStyle.Bottom)
                DockState = DockState.DockBottom;
            else if (dockStyle == DockStyle.Left)
                DockState = DockState.DockLeft;
            else if (dockStyle == DockStyle.Right)
                DockState = DockState.DockRight;
            else if (dockStyle == DockStyle.Fill)
                DockState = DockState.Document;
        }

        public void FloatAt(Rectangle floatWindowBounds)
        {
            if (FloatWindow == null || FloatWindow.NestedPanes.Count != 1)
                FloatWindow = DockPanel.FloatWindowFactory.CreateFloatWindow(DockPanel, this, floatWindowBounds);
            else
                FloatWindow.Bounds = floatWindowBounds;

            DockState = DockState.Float;
        }

        bool IDockDragSource.IsDockStateValid(DockState dockState)
        {
            return IsDockStateValid(dockState);
        }

        #endregion

        enum HitTestArea
        {
            Caption,
            TabStrip,
            Content,
            None
        }

        struct HitTestResult
        {
            public readonly HitTestArea HitArea;
            public readonly int Index;

            public HitTestResult(HitTestArea hitTestArea, int index)
            {
                HitArea = hitTestArea;
                Index = index;
            }
        }
    }
}