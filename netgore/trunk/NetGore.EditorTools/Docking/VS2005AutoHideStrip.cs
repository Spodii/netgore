using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NetGore.EditorTools.Docking
{
    class VS2005AutoHideStrip : AutoHideStripBase
    {
        const int _ImageGapBottom = 2;
        const int _ImageGapLeft = 4;
        const int _ImageGapRight = 2;
        const int _ImageGapTop = 2;
        const int _ImageHeight = 16;
        const int _ImageWidth = 16;
        const int _TabGapBetween = 10;
        const int _TabGapLeft = 4;
        const int _TabGapTop = 3;
        const int _TextGapLeft = 0;
        const int _TextGapRight = 0;

        static readonly Matrix _matrixIdentity = new Matrix();

        static DockState[] _dockStates;

        static GraphicsPath _graphicsPath;

        #region Customizable Properties

        static StringFormat _stringFormatTabHorizontal;

        static StringFormat _stringFormatTabVertical;

        static int ImageGapBottom
        {
            get { return _ImageGapBottom; }
        }

        static int ImageGapLeft
        {
            get { return _ImageGapLeft; }
        }

        static int ImageGapRight
        {
            get { return _ImageGapRight; }
        }

        static int ImageGapTop
        {
            get { return _ImageGapTop; }
        }

        static int ImageHeight
        {
            get { return _ImageHeight; }
        }

        static int ImageWidth
        {
            get { return _ImageWidth; }
        }

        static Pen PenTabBorder
        {
            get { return SystemPens.GrayText; }
        }

        StringFormat StringFormatTabHorizontal
        {
            get
            {
                if (_stringFormatTabHorizontal == null)
                {
                    _stringFormatTabHorizontal = new StringFormat();
                    _stringFormatTabHorizontal.Alignment = StringAlignment.Near;
                    _stringFormatTabHorizontal.LineAlignment = StringAlignment.Center;
                    _stringFormatTabHorizontal.FormatFlags = StringFormatFlags.NoWrap;
                    _stringFormatTabHorizontal.Trimming = StringTrimming.None;
                }

                if (RightToLeft == RightToLeft.Yes)
                    _stringFormatTabHorizontal.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                else
                    _stringFormatTabHorizontal.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;

                return _stringFormatTabHorizontal;
            }
        }

        StringFormat StringFormatTabVertical
        {
            get
            {
                if (_stringFormatTabVertical == null)
                {
                    _stringFormatTabVertical = new StringFormat();
                    _stringFormatTabVertical.Alignment = StringAlignment.Near;
                    _stringFormatTabVertical.LineAlignment = StringAlignment.Center;
                    _stringFormatTabVertical.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.DirectionVertical;
                    _stringFormatTabVertical.Trimming = StringTrimming.None;
                }
                if (RightToLeft == RightToLeft.Yes)
                    _stringFormatTabVertical.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                else
                    _stringFormatTabVertical.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;

                return _stringFormatTabVertical;
            }
        }

        static int TabGapBetween
        {
            get { return _TabGapBetween; }
        }

        static int TabGapLeft
        {
            get { return _TabGapLeft; }
        }

        static int TabGapTop
        {
            get { return _TabGapTop; }
        }

        static Font TextFont
        {
            get { return SystemInformation.MenuFont; }
        }

        static int TextGapLeft
        {
            get { return _TextGapLeft; }
        }

        static int TextGapRight
        {
            get { return _TextGapRight; }
        }

        #endregion

        public VS2005AutoHideStrip(DockPanel panel) : base(panel)
        {
            SetStyle(
                ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            BackColor = SystemColors.ControlLight;
        }

        static DockState[] DockStates
        {
            get
            {
                if (_dockStates == null)
                {
                    _dockStates = new DockState[4];
                    _dockStates[0] = DockState.DockLeftAutoHide;
                    _dockStates[1] = DockState.DockRightAutoHide;
                    _dockStates[2] = DockState.DockTopAutoHide;
                    _dockStates[3] = DockState.DockBottomAutoHide;
                }
                return _dockStates;
            }
        }

        internal static GraphicsPath GraphicsPath
        {
            get
            {
                if (_graphicsPath == null)
                    _graphicsPath = new GraphicsPath();

                return _graphicsPath;
            }
        }

        static Matrix MatrixIdentity
        {
            get { return _matrixIdentity; }
        }

        void CalculateTabs()
        {
            CalculateTabs(DockState.DockTopAutoHide);
            CalculateTabs(DockState.DockBottomAutoHide);
            CalculateTabs(DockState.DockLeftAutoHide);
            CalculateTabs(DockState.DockRightAutoHide);
        }

        void CalculateTabs(DockState dockState)
        {
            var rectTabStrip = GetLogicalTabStripRectangle(dockState);

            var imageHeight = rectTabStrip.Height - ImageGapTop - ImageGapBottom;
            var imageWidth = ImageWidth;
            if (imageHeight > ImageHeight)
                imageWidth = ImageWidth * (imageHeight / ImageHeight);

            var x = TabGapLeft + rectTabStrip.X;
            foreach (var pane in GetPanes(dockState))
            {
                foreach (TabVS2005 tab in pane.AutoHideTabs)
                {
                    var width = imageWidth + ImageGapLeft + ImageGapRight +
                                TextRenderer.MeasureText(tab.Content.DockHandler.TabText, TextFont).Width + TextGapLeft +
                                TextGapRight;
                    tab.TabX = x;
                    tab.TabWidth = width;
                    x += width;
                }

                x += TabGapBetween;
            }
        }

        protected override Tab CreateTab(IDockContent content)
        {
            return new TabVS2005(content);
        }

        void DrawTab(System.Drawing.Graphics g, TabVS2005 tab)
        {
            var rectTabOrigin = GetTabRectangle(tab);
            if (rectTabOrigin.IsEmpty)
                return;

            var dockState = tab.Content.DockHandler.DockState;
            var content = tab.Content;

            var path = GetTabOutline(tab, false, true);

            var startColor = DockPanel.Skin.AutoHideStripSkin.TabGradient.StartColor;
            var endColor = DockPanel.Skin.AutoHideStripSkin.TabGradient.EndColor;
            var gradientMode = DockPanel.Skin.AutoHideStripSkin.TabGradient.LinearGradientMode;
            g.FillPath(new LinearGradientBrush(rectTabOrigin, startColor, endColor, gradientMode), path);
            g.DrawPath(PenTabBorder, path);

            // Set no rotate for drawing icon and text
            var matrixRotate = g.Transform;
            g.Transform = MatrixIdentity;

            // Draw the icon
            var rectImage = rectTabOrigin;
            rectImage.X += ImageGapLeft;
            rectImage.Y += ImageGapTop;
            var imageHeight = rectTabOrigin.Height - ImageGapTop - ImageGapBottom;
            var imageWidth = ImageWidth;
            if (imageHeight > ImageHeight)
                imageWidth = ImageWidth * (imageHeight / ImageHeight);
            rectImage.Height = imageHeight;
            rectImage.Width = imageWidth;
            rectImage = GetTransformedRectangle(dockState, rectImage);
            g.DrawIcon(((Form)content).Icon, RtlTransform(rectImage, dockState));

            // Draw the text
            var rectText = rectTabOrigin;
            rectText.X += ImageGapLeft + imageWidth + ImageGapRight + TextGapLeft;
            rectText.Width -= ImageGapLeft + imageWidth + ImageGapRight + TextGapLeft;
            rectText = RtlTransform(GetTransformedRectangle(dockState, rectText), dockState);

            var textColor = DockPanel.Skin.AutoHideStripSkin.TabGradient.TextColor;

            if (dockState == DockState.DockLeftAutoHide || dockState == DockState.DockRightAutoHide)
                g.DrawString(content.DockHandler.TabText, TextFont, new SolidBrush(textColor), rectText, StringFormatTabVertical);
            else
                g.DrawString(content.DockHandler.TabText, TextFont, new SolidBrush(textColor), rectText, StringFormatTabHorizontal);

            // Set rotate back
            g.Transform = matrixRotate;
        }

        void DrawTabStrip(System.Drawing.Graphics g)
        {
            DrawTabStrip(g, DockState.DockTopAutoHide);
            DrawTabStrip(g, DockState.DockBottomAutoHide);
            DrawTabStrip(g, DockState.DockLeftAutoHide);
            DrawTabStrip(g, DockState.DockRightAutoHide);
        }

        void DrawTabStrip(System.Drawing.Graphics g, DockState dockState)
        {
            var rectTabStrip = GetLogicalTabStripRectangle(dockState);

            if (rectTabStrip.IsEmpty)
                return;

            var matrixIdentity = g.Transform;
            if (dockState == DockState.DockLeftAutoHide || dockState == DockState.DockRightAutoHide)
            {
                var matrixRotated = new Matrix();
                matrixRotated.RotateAt(90,
                                       new PointF(rectTabStrip.X + (float)rectTabStrip.Height / 2,
                                                  rectTabStrip.Y + (float)rectTabStrip.Height / 2));
                g.Transform = matrixRotated;
            }

            foreach (var pane in GetPanes(dockState))
            {
                foreach (TabVS2005 tab in pane.AutoHideTabs)
                {
                    DrawTab(g, tab);
                }
            }
            g.Transform = matrixIdentity;
        }

        Rectangle GetLogicalTabStripRectangle(DockState dockState, bool transformed = false)
        {
            if (!DockHelper.IsDockStateAutoHide(dockState))
                return Rectangle.Empty;

            var leftPanes = GetPanes(DockState.DockLeftAutoHide).Count;
            var rightPanes = GetPanes(DockState.DockRightAutoHide).Count;
            var topPanes = GetPanes(DockState.DockTopAutoHide).Count;
            var bottomPanes = GetPanes(DockState.DockBottomAutoHide).Count;

            int x, y, width;

            int height = MeasureHeight();
            if (dockState == DockState.DockLeftAutoHide && leftPanes > 0)
            {
                x = 0;
                y = (topPanes == 0) ? 0 : height;
                width = Height - (topPanes == 0 ? 0 : height) - (bottomPanes == 0 ? 0 : height);
            }
            else if (dockState == DockState.DockRightAutoHide && rightPanes > 0)
            {
                x = Width - height;
                if (leftPanes != 0 && x < height)
                    x = height;
                y = (topPanes == 0) ? 0 : height;
                width = Height - (topPanes == 0 ? 0 : height) - (bottomPanes == 0 ? 0 : height);
            }
            else if (dockState == DockState.DockTopAutoHide && topPanes > 0)
            {
                x = leftPanes == 0 ? 0 : height;
                y = 0;
                width = Width - (leftPanes == 0 ? 0 : height) - (rightPanes == 0 ? 0 : height);
            }
            else if (dockState == DockState.DockBottomAutoHide && bottomPanes > 0)
            {
                x = leftPanes == 0 ? 0 : height;
                y = Height - height;
                if (topPanes != 0 && y < height)
                    y = height;
                width = Width - (leftPanes == 0 ? 0 : height) - (rightPanes == 0 ? 0 : height);
            }
            else
                return Rectangle.Empty;

            if (!transformed)
                return new Rectangle(x, y, width, height);
            else
                return GetTransformedRectangle(dockState, new Rectangle(x, y, width, height));
        }

        GraphicsPath GetTabOutline(TabVS2005 tab, bool transformed, bool rtlTransform)
        {
            var dockState = tab.Content.DockHandler.DockState;
            var rectTab = GetTabRectangle(tab, transformed);
            if (rtlTransform)
                rectTab = RtlTransform(rectTab, dockState);
            var upTab = (dockState == DockState.DockLeftAutoHide || dockState == DockState.DockBottomAutoHide);
            DrawHelper.GetRoundedCornerTab(GraphicsPath, rectTab, upTab);

            return GraphicsPath;
        }

        Rectangle GetTabRectangle(TabVS2005 tab, bool transformed = false)
        {
            var dockState = tab.Content.DockHandler.DockState;
            var rectTabStrip = GetLogicalTabStripRectangle(dockState);

            if (rectTabStrip.IsEmpty)
                return Rectangle.Empty;

            var x = tab.TabX;
            var y = rectTabStrip.Y +
                    (dockState == DockState.DockTopAutoHide || dockState == DockState.DockRightAutoHide ? 0 : TabGapTop);
            var width = tab.TabWidth;
            var height = rectTabStrip.Height - TabGapTop;

            if (!transformed)
                return new Rectangle(x, y, width, height);
            else
                return GetTransformedRectangle(dockState, new Rectangle(x, y, width, height));
        }

        Rectangle GetTransformedRectangle(DockState dockState, Rectangle rect)
        {
            if (dockState != DockState.DockLeftAutoHide && dockState != DockState.DockRightAutoHide)
                return rect;

            var pts = new PointF[1];
            // the center of the rectangle
            pts[0].X = rect.X + (float)rect.Width / 2;
            pts[0].Y = rect.Y + (float)rect.Height / 2;
            var rectTabStrip = GetLogicalTabStripRectangle(dockState);
            var matrix = new Matrix();
            matrix.RotateAt(90,
                            new PointF(rectTabStrip.X + (float)rectTabStrip.Height / 2,
                                       rectTabStrip.Y + (float)rectTabStrip.Height / 2));
            matrix.TransformPoints(pts);

            return new Rectangle((int)(pts[0].X - (float)rect.Height / 2 + .5F), (int)(pts[0].Y - (float)rect.Width / 2 + .5F),
                                 rect.Height, rect.Width);
        }

        protected override IDockContent HitTest(Point ptMouse)
        {
            foreach (var state in DockStates)
            {
                var rectTabStrip = GetLogicalTabStripRectangle(state, true);
                if (!rectTabStrip.Contains(ptMouse))
                    continue;

                foreach (var pane in GetPanes(state))
                {
                    foreach (TabVS2005 tab in pane.AutoHideTabs)
                    {
                        var path = GetTabOutline(tab, true, true);
                        if (path.IsVisible(ptMouse))
                            return tab.Content;
                    }
                }
            }

            return null;
        }

        protected internal override int MeasureHeight()
        {
            return Math.Max(ImageGapBottom + ImageGapTop + ImageHeight, TextFont.Height) + TabGapTop;
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            CalculateTabs();
            base.OnLayout(levent);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            var startColor = DockPanel.Skin.AutoHideStripSkin.DockStripGradient.StartColor;
            var endColor = DockPanel.Skin.AutoHideStripSkin.DockStripGradient.EndColor;
            var gradientMode = DockPanel.Skin.AutoHideStripSkin.DockStripGradient.LinearGradientMode;
            using (var brush = new LinearGradientBrush(ClientRectangle, startColor, endColor, gradientMode))
            {
                g.FillRectangle(brush, ClientRectangle);
            }

            DrawTabStrip(g);
        }

        protected override void OnRefreshChanges()
        {
            CalculateTabs();
            Invalidate();
        }

        Rectangle RtlTransform(Rectangle rect, DockState dockState)
        {
            Rectangle rectTransformed;
            if (dockState == DockState.DockLeftAutoHide || dockState == DockState.DockRightAutoHide)
                rectTransformed = rect;
            else
                rectTransformed = DrawHelper.RtlTransform(this, rect);

            return rectTransformed;
        }

        class TabVS2005 : Tab
        {
            int m_tabWidth = 0;
            int m_tabX = 0;

            internal TabVS2005(IDockContent content) : base(content)
            {
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
    }
}