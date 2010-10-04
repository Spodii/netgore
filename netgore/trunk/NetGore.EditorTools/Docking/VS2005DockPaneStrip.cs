using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.EditorTools.Docking
{
    class VS2005DockPaneStrip : DockPaneStripBase
    {
        static Bitmap _imageButtonClose;

        static Bitmap _imageButtonWindowList;

        static Bitmap _imageButtonWindowListOverflow;

        readonly IContainer m_components;
        readonly ContextMenuStrip m_selectMenu;
        readonly ToolTip m_toolTip;

        Font m_boldFont;
        InertButton m_buttonClose;
        InertButton m_buttonWindowList;
        bool m_documentTabsOverflow = false;

        int m_endDisplayingTab = 0;

        int m_firstDisplayingTab = 0;
        Font m_font;
        int m_startDisplayingTab = 0;

        public VS2005DockPaneStrip(DockPane pane) : base(pane)
        {
            SetStyle(
                ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);

            SuspendLayout();

            m_components = new Container();
            m_toolTip = new ToolTip(Components);
            m_selectMenu = new ContextMenuStrip(Components);

            ResumeLayout();
        }

        Font BoldFont
        {
            get
            {
                if (IsDisposed)
                    return null;

                if (m_boldFont == null)
                {
                    m_font = TextFont;
                    m_boldFont = new Font(TextFont, FontStyle.Bold);
                }
                else if (m_font != TextFont)
                {
                    m_boldFont.Dispose();
                    m_font = TextFont;
                    m_boldFont = new Font(TextFont, FontStyle.Bold);
                }

                return m_boldFont;
            }
        }

        InertButton ButtonClose
        {
            get
            {
                if (m_buttonClose == null)
                {
                    m_buttonClose = new InertButton(ImageButtonClose, ImageButtonClose);
                    m_toolTip.SetToolTip(m_buttonClose, ToolTipClose);
                    m_buttonClose.Click += Close_Click;
                    Controls.Add(m_buttonClose);
                }

                return m_buttonClose;
            }
        }

        InertButton ButtonWindowList
        {
            get
            {
                if (m_buttonWindowList == null)
                {
                    m_buttonWindowList = new InertButton(ImageButtonWindowList, ImageButtonWindowListOverflow);
                    m_toolTip.SetToolTip(m_buttonWindowList, ToolTipSelect);
                    m_buttonWindowList.Click += WindowList_Click;
                    Controls.Add(m_buttonWindowList);
                }

                return m_buttonWindowList;
            }
        }

        IContainer Components
        {
            get { return m_components; }
        }

        bool DocumentTabsOverflow
        {
            set
            {
                if (m_documentTabsOverflow == value)
                    return;

                m_documentTabsOverflow = value;
                if (value)
                    ButtonWindowList.ImageCategory = 1;
                else
                    ButtonWindowList.ImageCategory = 0;
            }
        }

        int EndDisplayingTab
        {
            get { return m_endDisplayingTab; }
            set { m_endDisplayingTab = value; }
        }

        int FirstDisplayingTab
        {
            get { return m_firstDisplayingTab; }
            set { m_firstDisplayingTab = value; }
        }

        static GraphicsPath GraphicsPath
        {
            get { return VS2005AutoHideStrip.GraphicsPath; }
        }

        static Bitmap ImageButtonClose
        {
            get
            {
                if (_imageButtonClose == null)
                    _imageButtonClose = Resources.DockPane_Close;

                return _imageButtonClose;
            }
        }

        static Bitmap ImageButtonWindowList
        {
            get
            {
                if (_imageButtonWindowList == null)
                    _imageButtonWindowList = Resources.DockPane_Option;

                return _imageButtonWindowList;
            }
        }

        static Bitmap ImageButtonWindowListOverflow
        {
            get
            {
                if (_imageButtonWindowListOverflow == null)
                    _imageButtonWindowListOverflow = Resources.DockPane_OptionOverflow;

                return _imageButtonWindowListOverflow;
            }
        }

        ContextMenuStrip SelectMenu
        {
            get { return m_selectMenu; }
        }

        int StartDisplayingTab
        {
            get { return m_startDisplayingTab; }
            set
            {
                m_startDisplayingTab = value;
                Invalidate();
            }
        }

        Rectangle TabStripRectangle
        {
            get
            {
                if (Appearance == DockPane.AppearanceStyle.Document)
                    return TabStripRectangle_Document;
                else
                    return TabStripRectangle_ToolWindow;
            }
        }

        Rectangle TabStripRectangle_Document
        {
            get
            {
                var rect = ClientRectangle;
                return new Rectangle(rect.X, rect.Top + DocumentStripGapTop, rect.Width,
                                     rect.Height - DocumentStripGapTop - ToolWindowStripGapBottom);
            }
        }

        Rectangle TabStripRectangle_ToolWindow
        {
            get
            {
                var rect = ClientRectangle;
                return new Rectangle(rect.X, rect.Top + ToolWindowStripGapTop, rect.Width,
                                     rect.Height - ToolWindowStripGapTop - ToolWindowStripGapBottom);
            }
        }

        Rectangle TabsRectangle
        {
            get
            {
                if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                    return TabStripRectangle;

                var rectWindow = TabStripRectangle;
                var x = rectWindow.X;
                var y = rectWindow.Y;
                var width = rectWindow.Width;
                var height = rectWindow.Height;

                x += DocumentTabGapLeft;
                width -= DocumentTabGapLeft + DocumentTabGapRight + DocumentButtonGapRight + ButtonClose.Width +
                         ButtonWindowList.Width + 2 * DocumentButtonGapBetween;

                return new Rectangle(x, y, width, height);
            }
        }

        static Font TextFont
        {
            get { return SystemInformation.MenuFont; }
        }

        bool CalculateDocumentTab(Rectangle rectTabStrip, ref int x, int index)
        {
            var overflow = false;

            var tab = Tabs[index] as TabVS2005;
            tab.MaxWidth = GetMaxTabWidth(index);
            var width = Math.Min(tab.MaxWidth, DocumentTabMaxWidth);
            if (x + width < rectTabStrip.Right || index == StartDisplayingTab)
            {
                tab.TabX = x;
                tab.TabWidth = width;
                EndDisplayingTab = index;
            }
            else
            {
                tab.TabX = 0;
                tab.TabWidth = 0;
                overflow = true;
            }
            x += width;

            return overflow;
        }

        void CalculateTabs()
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                CalculateTabs_ToolWindow();
            else
                CalculateTabs_Document();
        }

        /// <summary>
        /// Calculate which tabs are displayed and in what order.
        /// </summary>
        void CalculateTabs_Document()
        {
            if (m_startDisplayingTab >= Tabs.Count)
                m_startDisplayingTab = 0;

            var rectTabStrip = TabsRectangle;

            var x = rectTabStrip.X + rectTabStrip.Height / 2;
            var overflow = false;

            // Originally all new documents that were considered overflow
            // (not enough pane strip space to show all tabs) were added to
            // the far left (assuming not right to left) and the tabs on the
            // right were dropped from view. If StartDisplayingTab is not 0
            // then we are dealing with making sure a specific tab is kept in focus.
            if (m_startDisplayingTab > 0)
            {
                var tempX = x;
                var tab = Tabs[m_startDisplayingTab] as TabVS2005;
                tab.MaxWidth = GetMaxTabWidth(m_startDisplayingTab);

                // Add the active tab and tabs to the left
                for (var i = StartDisplayingTab; i >= 0; i--)
                {
                    CalculateDocumentTab(rectTabStrip, ref tempX, i);
                }

                // Store which tab is the first one displayed so that it
                // will be drawn correctly (without part of the tab cut off)
                FirstDisplayingTab = EndDisplayingTab;

                tempX = x; // Reset X location because we are starting over

                // Start with the first tab displayed - name is a little misleading.
                // Loop through each tab and set its location. If there is not enough
                // room for all of them overflow will be returned.
                for (var i = EndDisplayingTab; i < Tabs.Count; i++)
                {
                    overflow = CalculateDocumentTab(rectTabStrip, ref tempX, i);
                }

                // If not all tabs are shown then we have an overflow.
                if (FirstDisplayingTab != 0)
                    overflow = true;
            }
            else
            {
                for (var i = StartDisplayingTab; i < Tabs.Count; i++)
                {
                    overflow = CalculateDocumentTab(rectTabStrip, ref x, i);
                }
                for (var i = 0; i < StartDisplayingTab; i++)
                {
                    overflow = CalculateDocumentTab(rectTabStrip, ref x, i);
                }

                FirstDisplayingTab = StartDisplayingTab;
            }

            if (!overflow)
            {
                m_startDisplayingTab = 0;
                FirstDisplayingTab = 0;
                x = rectTabStrip.X + rectTabStrip.Height / 2;
                foreach (TabVS2005 tab in Tabs)
                {
                    tab.TabX = x;
                    x += tab.TabWidth;
                }
            }
            DocumentTabsOverflow = overflow;
        }

        void CalculateTabs_ToolWindow()
        {
            if (Tabs.Count <= 1 || DockPane.IsAutoHide)
                return;

            var rectTabStrip = TabStripRectangle;

            // Calculate tab widths
            var countTabs = Tabs.Count;
            foreach (TabVS2005 tab in Tabs)
            {
                tab.MaxWidth = GetMaxTabWidth(Tabs.IndexOf(tab));
                tab.Flag = false;
            }

            // Set tab whose max width less than average width
            bool anyWidthWithinAverage;
            var totalWidth = rectTabStrip.Width - ToolWindowStripGapLeft - ToolWindowStripGapRight;
            var totalAllocatedWidth = 0;
            var averageWidth = totalWidth / countTabs;
            var remainedTabs = countTabs;
            for (anyWidthWithinAverage = true; anyWidthWithinAverage && remainedTabs > 0;)
            {
                anyWidthWithinAverage = false;
                foreach (TabVS2005 tab in Tabs)
                {
                    if (tab.Flag)
                        continue;

                    if (tab.MaxWidth <= averageWidth)
                    {
                        tab.Flag = true;
                        tab.TabWidth = tab.MaxWidth;
                        totalAllocatedWidth += tab.TabWidth;
                        anyWidthWithinAverage = true;
                        remainedTabs--;
                    }
                }
                if (remainedTabs != 0)
                    averageWidth = (totalWidth - totalAllocatedWidth) / remainedTabs;
            }

            // If any tab width not set yet, set it to the average width
            if (remainedTabs > 0)
            {
                var roundUpWidth = (totalWidth - totalAllocatedWidth) - (averageWidth * remainedTabs);
                foreach (TabVS2005 tab in Tabs)
                {
                    if (tab.Flag)
                        continue;

                    tab.Flag = true;
                    if (roundUpWidth > 0)
                    {
                        tab.TabWidth = averageWidth + 1;
                        roundUpWidth --;
                    }
                    else
                        tab.TabWidth = averageWidth;
                }
            }

            // Set the X position of the tabs
            var x = rectTabStrip.X + ToolWindowStripGapLeft;
            foreach (TabVS2005 tab in Tabs)
            {
                tab.TabX = x;
                x += tab.TabWidth;
            }
        }

        void Close_Click(object sender, EventArgs e)
        {
            DockPane.CloseActiveContent();
        }

        void ContextMenuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (item != null)
            {
                var content = (IDockContent)item.Tag;
                DockPane.ActiveContent = content;
            }
        }

        protected internal override Tab CreateTab(IDockContent content)
        {
            return new TabVS2005(content);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Components.Dispose();
                if (m_boldFont != null)
                {
                    m_boldFont.Dispose();
                    m_boldFont = null;
                }
            }
            base.Dispose(disposing);
        }

        void DrawTab(System.Drawing.Graphics g, TabVS2005 tab, Rectangle rect)
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                DrawTab_ToolWindow(g, tab, rect);
            else
                DrawTab_Document(g, tab, rect);
        }

        void DrawTabStrip(System.Drawing.Graphics g)
        {
            if (Appearance == DockPane.AppearanceStyle.Document)
                DrawTabStrip_Document(g);
            else
                DrawTabStrip_ToolWindow(g);
        }

        void DrawTabStrip_Document(System.Drawing.Graphics g)
        {
            var count = Tabs.Count;
            if (count == 0)
                return;

            var rectTabStrip = TabStripRectangle;

            // Draw the tabs
            var rectTabOnly = TabsRectangle;
            Rectangle rectTab;
            TabVS2005 tabActive = null;
            g.SetClip(DrawHelper.RtlTransform(this, rectTabOnly));
            for (var i = 0; i < count; i++)
            {
                rectTab = GetTabRectangle(i);
                if (Tabs[i].Content == DockPane.ActiveContent)
                {
                    tabActive = Tabs[i] as TabVS2005;
                    continue;
                }
                if (rectTab.IntersectsWith(rectTabOnly))
                    DrawTab(g, Tabs[i] as TabVS2005, rectTab);
            }

            g.SetClip(rectTabStrip);

            if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                g.DrawLine(PenDocumentTabActiveBorder, rectTabStrip.Left, rectTabStrip.Top + 1, rectTabStrip.Right,
                           rectTabStrip.Top + 1);
            else
                g.DrawLine(PenDocumentTabActiveBorder, rectTabStrip.Left, rectTabStrip.Bottom - 1, rectTabStrip.Right,
                           rectTabStrip.Bottom - 1);

            g.SetClip(DrawHelper.RtlTransform(this, rectTabOnly));
            if (tabActive != null)
            {
                rectTab = GetTabRectangle(Tabs.IndexOf(tabActive));
                if (rectTab.IntersectsWith(rectTabOnly))
                    DrawTab(g, tabActive, rectTab);
            }
        }

        void DrawTabStrip_ToolWindow(System.Drawing.Graphics g)
        {
            var rectTabStrip = TabStripRectangle;

            g.DrawLine(PenToolWindowTabBorder, rectTabStrip.Left, rectTabStrip.Top, rectTabStrip.Right, rectTabStrip.Top);

            for (var i = 0; i < Tabs.Count; i++)
            {
                DrawTab(g, Tabs[i] as TabVS2005, GetTabRectangle(i));
            }
        }

        void DrawTab_Document(System.Drawing.Graphics g, TabVS2005 tab, Rectangle rect)
        {
            if (tab.TabWidth == 0)
                return;

            var rectIcon = new Rectangle(rect.X + DocumentIconGapLeft,
                                         rect.Y + rect.Height - 1 - DocumentIconGapBottom - DocumentIconHeight, DocumentIconWidth,
                                         DocumentIconHeight);
            var rectText = rectIcon;
            if (DockPane.DockPanel.ShowDocumentIcon)
            {
                rectText.X += rectIcon.Width + DocumentIconGapRight;
                rectText.Y = rect.Y;
                rectText.Width = rect.Width - rectIcon.Width - DocumentIconGapLeft - DocumentIconGapRight - DocumentTextGapRight;
                rectText.Height = rect.Height;
            }
            else
                rectText.Width = rect.Width - DocumentIconGapLeft - DocumentTextGapRight;

            var rectTab = DrawHelper.RtlTransform(this, rect);
            var rectBack = DrawHelper.RtlTransform(this, rect);
            rectBack.Width += rect.X;
            rectBack.X = 0;

            rectText = DrawHelper.RtlTransform(this, rectText);
            rectIcon = DrawHelper.RtlTransform(this, rectIcon);
            var path = GetTabOutline(tab, true, false);
            if (DockPane.ActiveContent == tab.Content)
            {
                var startColor = DockPane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.StartColor;
                var endColor = DockPane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.EndColor;
                var gradientMode = DockPane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.LinearGradientMode;
                g.FillPath(new LinearGradientBrush(rectBack, startColor, endColor, gradientMode), path);
                g.DrawPath(PenDocumentTabActiveBorder, path);

                var textColor = DockPane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.TextColor;
                if (DockPane.IsActiveDocumentPane)
                    TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, BoldFont, rectText, textColor, DocumentTextFormat);
                else
                    TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText, textColor, DocumentTextFormat);
            }
            else
            {
                var startColor = DockPane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.StartColor;
                var endColor = DockPane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.EndColor;
                var gradientMode =
                    DockPane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.LinearGradientMode;
                g.FillPath(new LinearGradientBrush(rectBack, startColor, endColor, gradientMode), path);
                g.DrawPath(PenDocumentTabInactiveBorder, path);

                var textColor = DockPane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.TextColor;
                TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText, textColor, DocumentTextFormat);
            }

            if (rectTab.Contains(rectIcon) && DockPane.DockPanel.ShowDocumentIcon)
                g.DrawIcon(tab.Content.DockHandler.Icon, rectIcon);
        }

        void DrawTab_ToolWindow(System.Drawing.Graphics g, TabVS2005 tab, Rectangle rect)
        {
            var rectIcon = new Rectangle(rect.X + ToolWindowImageGapLeft,
                                         rect.Y + rect.Height - 1 - ToolWindowImageGapBottom - ToolWindowImageHeight,
                                         ToolWindowImageWidth, ToolWindowImageHeight);
            var rectText = rectIcon;
            rectText.X += rectIcon.Width + ToolWindowImageGapRight;
            rectText.Width = rect.Width - rectIcon.Width - ToolWindowImageGapLeft - ToolWindowImageGapRight -
                             ToolWindowTextGapRight;

            var rectTab = DrawHelper.RtlTransform(this, rect);
            rectText = DrawHelper.RtlTransform(this, rectText);
            rectIcon = DrawHelper.RtlTransform(this, rectIcon);
            var path = GetTabOutline(tab, true, false);
            if (DockPane.ActiveContent == tab.Content)
            {
                var startColor = DockPane.DockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.StartColor;
                var endColor = DockPane.DockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.EndColor;
                var gradientMode =
                    DockPane.DockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.LinearGradientMode;
                g.FillPath(new LinearGradientBrush(rectTab, startColor, endColor, gradientMode), path);
                g.DrawPath(PenToolWindowTabBorder, path);

                var textColor = DockPane.DockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.TextColor;
                TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText, textColor, ToolWindowTextFormat);
            }
            else
            {
                var startColor = DockPane.DockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.StartColor;
                var endColor = DockPane.DockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.EndColor;
                var gradientMode =
                    DockPane.DockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.LinearGradientMode;
                g.FillPath(new LinearGradientBrush(rectTab, startColor, endColor, gradientMode), path);

                if (Tabs.IndexOf(DockPane.ActiveContent) != Tabs.IndexOf(tab) + 1)
                {
                    var pt1 = new Point(rect.Right, rect.Top + ToolWindowTabSeperatorGapTop);
                    var pt2 = new Point(rect.Right, rect.Bottom - ToolWindowTabSeperatorGapBottom);
                    g.DrawLine(PenToolWindowTabBorder, DrawHelper.RtlTransform(this, pt1), DrawHelper.RtlTransform(this, pt2));
                }

                var textColor = DockPane.DockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.TextColor;
                TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText, textColor, ToolWindowTextFormat);
            }

            if (rectTab.Contains(rectIcon))
                g.DrawIcon(tab.Content.DockHandler.Icon, rectIcon);
        }

        bool EnsureDocumentTabVisible(IDockContent content, bool repaint)
        {
            var index = Tabs.IndexOf(content);
            var tab = Tabs[index] as TabVS2005;
            if (tab.TabWidth != 0)
                return false;

            StartDisplayingTab = index;
            if (repaint)
                Invalidate();

            return true;
        }

        protected internal override void EnsureTabVisible(IDockContent content)
        {
            if (Appearance != DockPane.AppearanceStyle.Document || !Tabs.Contains(content))
                return;

            CalculateTabs();
            EnsureDocumentTabVisible(content, true);
        }

        int GetMaxTabWidth(int index)
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                return GetMaxTabWidth_ToolWindow(index);
            else
                return GetMaxTabWidth_Document(index);
        }

        int GetMaxTabWidth_Document(int index)
        {
            var content = Tabs[index].Content;

            var height = GetTabRectangle_Document(index).Height;

            var sizeText = TextRenderer.MeasureText(content.DockHandler.TabText, BoldFont, new Size(DocumentTabMaxWidth, height),
                                                    DocumentTextFormat);

            if (DockPane.DockPanel.ShowDocumentIcon)
                return sizeText.Width + DocumentIconWidth + DocumentIconGapLeft + DocumentIconGapRight + DocumentTextGapRight;
            else
                return sizeText.Width + DocumentIconGapLeft + DocumentTextGapRight;
        }

        int GetMaxTabWidth_ToolWindow(int index)
        {
            var content = Tabs[index].Content;
            var sizeString = TextRenderer.MeasureText(content.DockHandler.TabText, TextFont);
            return ToolWindowImageWidth + sizeString.Width + ToolWindowImageGapLeft + ToolWindowImageGapRight +
                   ToolWindowTextGapRight;
        }

        protected internal override GraphicsPath GetOutline(int index)
        {
            if (Appearance == DockPane.AppearanceStyle.Document)
                return GetOutline_Document(index);
            else
                return GetOutline_ToolWindow(index);
        }

        GraphicsPath GetOutline_Document(int index)
        {
            var rectTab = GetTabRectangle(index);
            rectTab.X -= rectTab.Height / 2;
            rectTab.Intersect(TabsRectangle);
            rectTab = RectangleToScreen(DrawHelper.RtlTransform(this, rectTab));
            var rectPaneClient = DockPane.RectangleToScreen(DockPane.ClientRectangle);

            var path = new GraphicsPath();
            var pathTab = GetTabOutline_Document(Tabs[index], true, true, true);
            path.AddPath(pathTab, true);

            if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
            {
                path.AddLine(rectTab.Right, rectTab.Top, rectPaneClient.Right, rectTab.Top);
                path.AddLine(rectPaneClient.Right, rectTab.Top, rectPaneClient.Right, rectPaneClient.Top);
                path.AddLine(rectPaneClient.Right, rectPaneClient.Top, rectPaneClient.Left, rectPaneClient.Top);
                path.AddLine(rectPaneClient.Left, rectPaneClient.Top, rectPaneClient.Left, rectTab.Top);
                path.AddLine(rectPaneClient.Left, rectTab.Top, rectTab.Right, rectTab.Top);
            }
            else
            {
                path.AddLine(rectTab.Right, rectTab.Bottom, rectPaneClient.Right, rectTab.Bottom);
                path.AddLine(rectPaneClient.Right, rectTab.Bottom, rectPaneClient.Right, rectPaneClient.Bottom);
                path.AddLine(rectPaneClient.Right, rectPaneClient.Bottom, rectPaneClient.Left, rectPaneClient.Bottom);
                path.AddLine(rectPaneClient.Left, rectPaneClient.Bottom, rectPaneClient.Left, rectTab.Bottom);
                path.AddLine(rectPaneClient.Left, rectTab.Bottom, rectTab.Right, rectTab.Bottom);
            }
            return path;
        }

        GraphicsPath GetOutline_ToolWindow(int index)
        {
            var rectTab = GetTabRectangle(index);
            rectTab.Intersect(TabsRectangle);
            rectTab = RectangleToScreen(DrawHelper.RtlTransform(this, rectTab));

            var rectPaneClient = DockPane.RectangleToScreen(DockPane.ClientRectangle);

            var path = new GraphicsPath();
            var pathTab = GetTabOutline(Tabs[index], true, true);
            path.AddPath(pathTab, true);
            path.AddLine(rectTab.Left, rectTab.Top, rectPaneClient.Left, rectTab.Top);
            path.AddLine(rectPaneClient.Left, rectTab.Top, rectPaneClient.Left, rectPaneClient.Top);
            path.AddLine(rectPaneClient.Left, rectPaneClient.Top, rectPaneClient.Right, rectPaneClient.Top);
            path.AddLine(rectPaneClient.Right, rectPaneClient.Top, rectPaneClient.Right, rectTab.Top);
            path.AddLine(rectPaneClient.Right, rectTab.Top, rectTab.Right, rectTab.Top);
            return path;
        }

        GraphicsPath GetTabOutline(Tab tab, bool rtlTransform, bool toScreen)
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                return GetTabOutline_ToolWindow(tab, rtlTransform, toScreen);
            else
                return GetTabOutline_Document(tab, rtlTransform, toScreen, false);
        }

        GraphicsPath GetTabOutline_Document(Tab tab, bool rtlTransform, bool toScreen, bool full)
        {
            const int curveSize = 6;

            GraphicsPath.Reset();
            var rect = GetTabRectangle(Tabs.IndexOf(tab));
            if (rtlTransform)
                rect = DrawHelper.RtlTransform(this, rect);
            if (toScreen)
                rect = RectangleToScreen(rect);

            // Draws the full angle piece for active content (or first tab)
            if (tab.Content == DockPane.ActiveContent || full || Tabs.IndexOf(tab) == FirstDisplayingTab)
            {
                if (RightToLeft == RightToLeft.Yes)
                {
                    if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                    {
                        // For some reason the next line draws a line that is not hidden like it is when drawing the tab strip on top.
                        // It is not needed so it has been commented out.
                        //GraphicsPath.AddLine(rect.Right, rect.Bottom, rect.Right + rect.Height / 2, rect.Bottom);
                        GraphicsPath.AddLine(rect.Right + rect.Height / 2, rect.Top, rect.Right - rect.Height / 2 + curveSize / 2,
                                             rect.Bottom - curveSize / 2);
                    }
                    else
                    {
                        GraphicsPath.AddLine(rect.Right, rect.Bottom, rect.Right + rect.Height / 2, rect.Bottom);
                        GraphicsPath.AddLine(rect.Right + rect.Height / 2, rect.Bottom,
                                             rect.Right - rect.Height / 2 + curveSize / 2, rect.Top + curveSize / 2);
                    }
                }
                else
                {
                    if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                    {
                        // For some reason the next line draws a line that is not hidden like it is when drawing the tab strip on top.
                        // It is not needed so it has been commented out.
                        //GraphicsPath.AddLine(rect.Left, rect.Top, rect.Left - rect.Height / 2, rect.Top);
                        GraphicsPath.AddLine(rect.Left - rect.Height / 2, rect.Top, rect.Left + rect.Height / 2 - curveSize / 2,
                                             rect.Bottom - curveSize / 2);
                    }
                    else
                    {
                        GraphicsPath.AddLine(rect.Left, rect.Bottom, rect.Left - rect.Height / 2, rect.Bottom);
                        GraphicsPath.AddLine(rect.Left - rect.Height / 2, rect.Bottom, rect.Left + rect.Height / 2 - curveSize / 2,
                                             rect.Top + curveSize / 2);
                    }
                }
            }
                // Draws the partial angle for non-active content
            else
            {
                if (RightToLeft == RightToLeft.Yes)
                {
                    if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                    {
                        GraphicsPath.AddLine(rect.Right, rect.Top, rect.Right, rect.Top + rect.Height / 2);
                        GraphicsPath.AddLine(rect.Right, rect.Top + rect.Height / 2, rect.Right - rect.Height / 2 + curveSize / 2,
                                             rect.Bottom - curveSize / 2);
                    }
                    else
                    {
                        GraphicsPath.AddLine(rect.Right, rect.Bottom, rect.Right, rect.Bottom - rect.Height / 2);
                        GraphicsPath.AddLine(rect.Right, rect.Bottom - rect.Height / 2,
                                             rect.Right - rect.Height / 2 + curveSize / 2, rect.Top + curveSize / 2);
                    }
                }
                else
                {
                    if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                    {
                        GraphicsPath.AddLine(rect.Left, rect.Top, rect.Left, rect.Top + rect.Height / 2);
                        GraphicsPath.AddLine(rect.Left, rect.Top + rect.Height / 2, rect.Left + rect.Height / 2 - curveSize / 2,
                                             rect.Bottom - curveSize / 2);
                    }
                    else
                    {
                        GraphicsPath.AddLine(rect.Left, rect.Bottom, rect.Left, rect.Bottom - rect.Height / 2);
                        GraphicsPath.AddLine(rect.Left, rect.Bottom - rect.Height / 2, rect.Left + rect.Height / 2 - curveSize / 2,
                                             rect.Top + curveSize / 2);
                    }
                }
            }

            if (RightToLeft == RightToLeft.Yes)
            {
                if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                {
                    // Draws the bottom horizontal line (short side)
                    GraphicsPath.AddLine(rect.Right - rect.Height / 2 - curveSize / 2, rect.Bottom, rect.Left + curveSize / 2,
                                         rect.Bottom);

                    // Drawing the rounded corner is not necessary. The path is automatically connected
                    //GraphicsPath.AddArc(new Rectangle(rect.Left, rect.Top, curveSize, curveSize), 180, 90);
                }
                else
                {
                    // Draws the bottom horizontal line (short side)
                    GraphicsPath.AddLine(rect.Right - rect.Height / 2 - curveSize / 2, rect.Top, rect.Left + curveSize / 2,
                                         rect.Top);
                    GraphicsPath.AddArc(new Rectangle(rect.Left, rect.Top, curveSize, curveSize), 180, 90);
                }
            }
            else
            {
                if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                {
                    // Draws the bottom horizontal line (short side)
                    GraphicsPath.AddLine(rect.Left + rect.Height / 2 + curveSize / 2, rect.Bottom, rect.Right - curveSize / 2,
                                         rect.Bottom);

                    // Drawing the rounded corner is not necessary. The path is automatically connected
                    //GraphicsPath.AddArc(new Rectangle(rect.Right - curveSize, rect.Bottom, curveSize, curveSize), 90, -90);
                }
                else
                {
                    // Draws the top horizontal line (short side)
                    GraphicsPath.AddLine(rect.Left + rect.Height / 2 + curveSize / 2, rect.Top, rect.Right - curveSize / 2,
                                         rect.Top);

                    // Draws the rounded corner oppposite the angled side
                    GraphicsPath.AddArc(new Rectangle(rect.Right - curveSize, rect.Top, curveSize, curveSize), -90, 90);
                }
            }

            if (Tabs.IndexOf(tab) != EndDisplayingTab &&
                (Tabs.IndexOf(tab) != Tabs.Count - 1 && Tabs[Tabs.IndexOf(tab) + 1].Content == DockPane.ActiveContent) && !full)
            {
                if (RightToLeft == RightToLeft.Yes)
                {
                    if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                    {
                        GraphicsPath.AddLine(rect.Left, rect.Bottom - curveSize / 2, rect.Left, rect.Bottom - rect.Height / 2);
                        GraphicsPath.AddLine(rect.Left, rect.Bottom - rect.Height / 2, rect.Left + rect.Height / 2, rect.Top);
                    }
                    else
                    {
                        GraphicsPath.AddLine(rect.Left, rect.Top + curveSize / 2, rect.Left, rect.Top + rect.Height / 2);
                        GraphicsPath.AddLine(rect.Left, rect.Top + rect.Height / 2, rect.Left + rect.Height / 2, rect.Bottom);
                    }
                }
                else
                {
                    if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                    {
                        GraphicsPath.AddLine(rect.Right, rect.Bottom - curveSize / 2, rect.Right, rect.Bottom - rect.Height / 2);
                        GraphicsPath.AddLine(rect.Right, rect.Bottom - rect.Height / 2, rect.Right - rect.Height / 2, rect.Top);
                    }
                    else
                    {
                        GraphicsPath.AddLine(rect.Right, rect.Top + curveSize / 2, rect.Right, rect.Top + rect.Height / 2);
                        GraphicsPath.AddLine(rect.Right, rect.Top + rect.Height / 2, rect.Right - rect.Height / 2, rect.Bottom);
                    }
                }
            }
            else
            {
                // Draw the vertical line opposite the angled side
                if (RightToLeft == RightToLeft.Yes)
                {
                    if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                        GraphicsPath.AddLine(rect.Left, rect.Bottom - curveSize / 2, rect.Left, rect.Top);
                    else
                        GraphicsPath.AddLine(rect.Left, rect.Top + curveSize / 2, rect.Left, rect.Bottom);
                }
                else
                {
                    if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                        GraphicsPath.AddLine(rect.Right, rect.Bottom - curveSize / 2, rect.Right, rect.Top);
                    else
                        GraphicsPath.AddLine(rect.Right, rect.Top + curveSize / 2, rect.Right, rect.Bottom);
                }
            }

            return GraphicsPath;
        }

        GraphicsPath GetTabOutline_ToolWindow(Tab tab, bool rtlTransform, bool toScreen)
        {
            var rect = GetTabRectangle(Tabs.IndexOf(tab));
            if (rtlTransform)
                rect = DrawHelper.RtlTransform(this, rect);
            if (toScreen)
                rect = RectangleToScreen(rect);

            DrawHelper.GetRoundedCornerTab(GraphicsPath, rect, false);
            return GraphicsPath;
        }

        Rectangle GetTabRectangle(int index)
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                return GetTabRectangle_ToolWindow(index);
            else
                return GetTabRectangle_Document(index);
        }

        Rectangle GetTabRectangle_Document(int index)
        {
            var rectTabStrip = TabStripRectangle;
            var tab = (TabVS2005)Tabs[index];

            var rect = new Rectangle();
            rect.X = tab.TabX;
            rect.Width = tab.TabWidth;
            rect.Height = rectTabStrip.Height - DocumentTabGapTop;

            if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                rect.Y = rectTabStrip.Y + DocumentStripGapBottom;
            else
                rect.Y = rectTabStrip.Y + DocumentTabGapTop;

            return rect;
        }

        Rectangle GetTabRectangle_ToolWindow(int index)
        {
            var rectTabStrip = TabStripRectangle;

            var tab = (TabVS2005)(Tabs[index]);
            return new Rectangle(tab.TabX, rectTabStrip.Y, tab.TabWidth, rectTabStrip.Height);
        }

        protected internal override int HitTest(Point ptMouse)
        {
            var rectTabStrip = TabsRectangle;
            if (!rectTabStrip.Contains(ptMouse))
                return -1;

            foreach (var tab in Tabs)
            {
                var path = GetTabOutline(tab, true, false);
                if (path.IsVisible(ptMouse))
                    return Tabs.IndexOf(tab);
            }
            return -1;
        }

        protected internal override int MeasureHeight()
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                return MeasureHeight_ToolWindow();
            else
                return MeasureHeight_Document();
        }

        int MeasureHeight_Document()
        {
            var height =
                Math.Max(TextFont.Height + DocumentTabGapTop, ButtonClose.Height + DocumentButtonGapTop + DocumentButtonGapBottom) +
                DocumentStripGapBottom + DocumentStripGapTop;

            return height;
        }

        int MeasureHeight_ToolWindow()
        {
            if (DockPane.IsAutoHide || Tabs.Count <= 1)
                return 0;

            var height = Math.Max(TextFont.Height, ToolWindowImageHeight + ToolWindowImageGapTop + ToolWindowImageGapBottom) +
                         ToolWindowStripGapTop + ToolWindowStripGapBottom;

            return height;
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            if (Appearance != DockPane.AppearanceStyle.Document)
            {
                base.OnLayout(levent);
                return;
            }

            var rectTabStrip = TabStripRectangle;

            // Set position and size of the buttons
            var buttonWidth = ButtonClose.Image.Width;
            var buttonHeight = ButtonClose.Image.Height;
            var height = rectTabStrip.Height - DocumentButtonGapTop - DocumentButtonGapBottom;
            if (buttonHeight < height)
            {
                buttonWidth = buttonWidth * (height / buttonHeight);
                buttonHeight = height;
            }
            var buttonSize = new Size(buttonWidth, buttonHeight);

            var x = rectTabStrip.X + rectTabStrip.Width - DocumentTabGapLeft - DocumentButtonGapRight - buttonWidth;
            var y = rectTabStrip.Y + DocumentButtonGapTop;
            var point = new Point(x, y);
            ButtonClose.Bounds = DrawHelper.RtlTransform(this, new Rectangle(point, buttonSize));

            // If the close button is not visible draw the window list button overtop.
            // Otherwise it is drawn to the left of the close button.
            if (ButtonClose.Visible)
                point.Offset(-(DocumentButtonGapBetween + buttonWidth), 0);

            ButtonWindowList.Bounds = DrawHelper.RtlTransform(this, new Rectangle(point, buttonSize));

            OnRefreshChanges();

            base.OnLayout(levent);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            var index = HitTest(PointToClient(MousePosition));
            var toolTip = string.Empty;

            base.OnMouseHover(e);

            if (index != -1)
            {
                var tab = Tabs[index] as TabVS2005;
                if (!String.IsNullOrEmpty(tab.Content.DockHandler.ToolTipText))
                    toolTip = tab.Content.DockHandler.ToolTipText;
                else if (tab.MaxWidth > tab.TabWidth)
                    toolTip = tab.Content.DockHandler.TabText;
            }

            if (m_toolTip.GetToolTip(this) != toolTip)
            {
                m_toolTip.Active = false;
                m_toolTip.SetToolTip(this, toolTip);
                m_toolTip.Active = true;
            }

            // requires further tracking of mouse hover behavior,
            ResetMouseEventArgs();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = TabsRectangle;

            if (Appearance == DockPane.AppearanceStyle.Document)
            {
                rect.X -= DocumentTabGapLeft;

                // Add these values back in so that the DockStrip color is drawn
                // beneath the close button and window list button.
                rect.Width += DocumentTabGapLeft + DocumentTabGapRight + DocumentButtonGapRight + ButtonClose.Width +
                              ButtonWindowList.Width;

                // It is possible depending on the DockPanel DocumentStyle to have
                // a Document without a DockStrip.
                if (rect.Width > 0 && rect.Height > 0)
                {
                    var startColor = DockPane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.StartColor;
                    var endColor = DockPane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.EndColor;
                    var gradientMode =
                        DockPane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.LinearGradientMode;
                    using (var brush = new LinearGradientBrush(rect, startColor, endColor, gradientMode))
                    {
                        e.Graphics.FillRectangle(brush, rect);
                    }
                }
            }
            else
            {
                var startColor = DockPane.DockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.DockStripGradient.StartColor;
                var endColor = DockPane.DockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.DockStripGradient.EndColor;
                var gradientMode =
                    DockPane.DockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.DockStripGradient.LinearGradientMode;
                using (var brush = new LinearGradientBrush(rect, startColor, endColor, gradientMode))
                {
                    e.Graphics.FillRectangle(brush, rect);
                }
            }
            base.OnPaint(e);
            CalculateTabs();
            if (Appearance == DockPane.AppearanceStyle.Document && DockPane.ActiveContent != null)
            {
                if (EnsureDocumentTabVisible(DockPane.ActiveContent, false))
                    CalculateTabs();
            }

            DrawTabStrip(e.Graphics);
        }

        protected override void OnRefreshChanges()
        {
            SetInertButtons();
            Invalidate();
        }

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);
            PerformLayout();
        }

        void SetInertButtons()
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
            {
                if (m_buttonClose != null)
                    m_buttonClose.Left = -m_buttonClose.Width;

                if (m_buttonWindowList != null)
                    m_buttonWindowList.Left = -m_buttonWindowList.Width;
            }
            else
            {
                var showCloseButton = DockPane.ActiveContent == null ? true : DockPane.ActiveContent.DockHandler.CloseButton;
                ButtonClose.Enabled = showCloseButton;
                ButtonClose.Visible = DockPane.ActiveContent == null
                                          ? true : DockPane.ActiveContent.DockHandler.CloseButtonVisible;
                ButtonClose.RefreshChanges();
                ButtonWindowList.RefreshChanges();
            }
        }

        void WindowList_Click(object sender, EventArgs e)
        {
            const int x = 0;
            var y = ButtonWindowList.Location.Y + ButtonWindowList.Height;

            SelectMenu.Items.Clear();
            foreach (TabVS2005 tab in Tabs)
            {
                var content = tab.Content;
                var item = SelectMenu.Items.Add(content.DockHandler.TabText, content.DockHandler.Icon.ToBitmap());
                item.Tag = tab.Content;
                item.Click += ContextMenuItem_Click;
            }
            SelectMenu.Show(ButtonWindowList, x, y);
        }

        #region Customizable Properties

        static string _toolTipClose;

        static string _toolTipSelect;

        static int DocumentButtonGapBetween
        {
            get { return _DocumentButtonGapBetween; }
        }

        static int DocumentButtonGapBottom
        {
            get { return _DocumentButtonGapBottom; }
        }

        static int DocumentButtonGapRight
        {
            get { return _DocumentButtonGapRight; }
        }

        static int DocumentButtonGapTop
        {
            get { return _DocumentButtonGapTop; }
        }

        static int DocumentIconGapBottom
        {
            get { return _DocumentIconGapBottom; }
        }

        static int DocumentIconGapLeft
        {
            get { return _DocumentIconGapLeft; }
        }

        static int DocumentIconGapRight
        {
            get { return _DocumentIconGapRight; }
        }

        static int DocumentIconHeight
        {
            get { return _DocumentIconHeight; }
        }

        static int DocumentIconWidth
        {
            get { return _DocumentIconWidth; }
        }

        static int DocumentStripGapBottom
        {
            get { return _DocumentStripGapBottom; }
        }

        static int DocumentStripGapTop
        {
            get { return _DocumentStripGapTop; }
        }

        static int DocumentTabGapLeft
        {
            get { return _DocumentTabGapLeft; }
        }

        static int DocumentTabGapRight
        {
            get { return _DocumentTabGapRight; }
        }

        static int DocumentTabGapTop
        {
            get { return _DocumentTabGapTop; }
        }

        static int DocumentTabMaxWidth
        {
            get { return _DocumentTabMaxWidth; }
        }

        TextFormatFlags DocumentTextFormat
        {
            get
            {
                const TextFormatFlags textFormat =
                    TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter |
                    TextFormatFlags.HorizontalCenter;
                if (RightToLeft == RightToLeft.Yes)
                    return textFormat | TextFormatFlags.RightToLeft;
                else
                    return textFormat;
            }
        }

        static int DocumentTextGapRight
        {
            get { return _DocumentTextGapRight; }
        }

        static Pen PenDocumentTabActiveBorder
        {
            get { return SystemPens.ControlDarkDark; }
        }

        static Pen PenDocumentTabInactiveBorder
        {
            get { return SystemPens.GrayText; }
        }

        static Pen PenToolWindowTabBorder
        {
            get { return SystemPens.GrayText; }
        }

        static string ToolTipClose
        {
            get
            {
                if (_toolTipClose == null)
                    _toolTipClose = Strings.DockPaneStrip_ToolTipClose;
                return _toolTipClose;
            }
        }

        static string ToolTipSelect
        {
            get
            {
                if (_toolTipSelect == null)
                    _toolTipSelect = Strings.DockPaneStrip_ToolTipWindowList;
                return _toolTipSelect;
            }
        }

        static int ToolWindowImageGapBottom
        {
            get { return _ToolWindowImageGapBottom; }
        }

        static int ToolWindowImageGapLeft
        {
            get { return _ToolWindowImageGapLeft; }
        }

        static int ToolWindowImageGapRight
        {
            get { return _ToolWindowImageGapRight; }
        }

        static int ToolWindowImageGapTop
        {
            get { return _ToolWindowImageGapTop; }
        }

        static int ToolWindowImageHeight
        {
            get { return _ToolWindowImageHeight; }
        }

        static int ToolWindowImageWidth
        {
            get { return _ToolWindowImageWidth; }
        }

        static int ToolWindowStripGapBottom
        {
            get { return _ToolWindowStripGapBottom; }
        }

        static int ToolWindowStripGapLeft
        {
            get { return _ToolWindowStripGapLeft; }
        }

        static int ToolWindowStripGapRight
        {
            get { return _ToolWindowStripGapRight; }
        }

        static int ToolWindowStripGapTop
        {
            get { return _ToolWindowStripGapTop; }
        }

        static int ToolWindowTabSeperatorGapBottom
        {
            get { return _ToolWindowTabSeperatorGapBottom; }
        }

        static int ToolWindowTabSeperatorGapTop
        {
            get { return _ToolWindowTabSeperatorGapTop; }
        }

        TextFormatFlags ToolWindowTextFormat
        {
            get
            {
                const TextFormatFlags textFormat =
                    TextFormatFlags.EndEllipsis | TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine |
                    TextFormatFlags.VerticalCenter;
                if (RightToLeft == RightToLeft.Yes)
                    return textFormat | TextFormatFlags.RightToLeft | TextFormatFlags.Right;
                else
                    return textFormat;
            }
        }

        static int ToolWindowTextGapRight
        {
            get { return _ToolWindowTextGapRight; }
        }

        #endregion

        sealed class InertButton : InertButtonBase
        {
            readonly Bitmap m_image0;
            readonly Bitmap m_image1;

            int m_imageCategory = 0;

            public InertButton(Bitmap image0, Bitmap image1)
            {
                m_image0 = image0;
                m_image1 = image1;
            }

            public override Bitmap Image
            {
                get { return ImageCategory == 0 ? m_image0 : m_image1; }
            }

            public int ImageCategory
            {
                get { return m_imageCategory; }
                set
                {
                    if (m_imageCategory == value)
                        return;

                    m_imageCategory = value;
                    Invalidate();
                }
            }
        }

        class TabVS2005 : Tab
        {
            bool m_flag;
            int m_maxWidth;
            int m_tabWidth;
            int m_tabX;

            public TabVS2005(IDockContent content) : base(content)
            {
            }

            protected internal bool Flag
            {
                get { return m_flag; }
                set { m_flag = value; }
            }

            public int MaxWidth
            {
                get { return m_maxWidth; }
                set { m_maxWidth = value; }
            }

            public int TabWidth
            {
                get { return m_tabWidth; }
                set { m_tabWidth = value; }
            }

            public int TabX
            {
                get { return m_tabX; }
                set { m_tabX = value; }
            }
        }

        #region consts

        const int _DocumentButtonGapBetween = 0;
        const int _DocumentButtonGapBottom = 4;
        const int _DocumentButtonGapRight = 3;
        const int _DocumentButtonGapTop = 4;
        const int _DocumentIconGapBottom = 2;
        const int _DocumentIconGapLeft = 8;
        const int _DocumentIconGapRight = 0;
        const int _DocumentIconHeight = 16;
        const int _DocumentIconWidth = 16;
        const int _DocumentStripGapBottom = 1;
        const int _DocumentStripGapTop = 0;
        const int _DocumentTabGapLeft = 3;
        const int _DocumentTabGapRight = 3;
        const int _DocumentTabGapTop = 3;
        const int _DocumentTabMaxWidth = 200;
        const int _DocumentTextGapRight = 3;
        const int _ToolWindowImageGapBottom = 1;
        const int _ToolWindowImageGapLeft = 2;
        const int _ToolWindowImageGapRight = 0;
        const int _ToolWindowImageGapTop = 3;
        const int _ToolWindowImageHeight = 16;
        const int _ToolWindowImageWidth = 16;
        const int _ToolWindowStripGapBottom = 1;
        const int _ToolWindowStripGapLeft = 0;
        const int _ToolWindowStripGapRight = 0;
        const int _ToolWindowStripGapTop = 0;
        const int _ToolWindowTabSeperatorGapBottom = 3;
        const int _ToolWindowTabSeperatorGapTop = 3;
        const int _ToolWindowTextGapRight = 3;

        #endregion
    }
}