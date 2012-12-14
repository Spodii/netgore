using System;
using System.Diagnostics;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Handles displaying pop-up text for <see cref="Control"/>s after they have been hovered over by the cursor
    /// for the appropriate amount of time.
    /// </summary>
    public class Tooltip : ITooltip
    {
        readonly TooltipArgs _args = new TooltipArgs();
        readonly StyledTextsDrawer _drawer;
        readonly IGUIManager _guiManager;

        Color _bgColor = new Color(0, 0, 0, 200);
        Vector2 _borderPadding = new Vector2(4, 4);
        Vector2 _borderSize;
        int _delay = 800;
        Vector2 _drawOffset = new Vector2(8, 8);
        Font _font;
        Color _fontColor = Color.White;
        bool _isVisible = true;
        TickCount _lastRefreshTime;
        Control _lastUnderCursor;
        int _maxWidth = 150;
        int _retryDelay = 500;

        /// <summary>
        /// The time that the current control under the cursor started being hovered over.
        /// </summary>
        TickCount _startHoverTime;

        int _timeout = 3000;

        bool _tooltipTimedOut = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tooltip"/> class.
        /// </summary>
        /// <param name="guiManager">The <see cref="IGUIManager"/> to attach the <see cref="Tooltip"/> to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager" /> is <c>null</c>.</exception>
        public Tooltip(IGUIManager guiManager)
        {
            if (guiManager == null)
                throw new ArgumentNullException("guiManager");

            _guiManager = guiManager;
            _font = guiManager.Font;
            _drawer = new StyledTextsDrawer(Font);

            Debug.Assert(Font != null);
        }

        /// <summary>
        /// Refreshes the text of a <see cref="Tooltip"/>.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        void RefreshText(TickCount currentTime)
        {
            // Request the tooltip text
            var tooltipTexts = _lastUnderCursor.Tooltip.Invoke(_lastUnderCursor, _args);
            _lastRefreshTime = currentTime;

            // If the tooltip text is null, increase the _startHoverTime to result in the needed retry delay
            if (tooltipTexts == null)
                _startHoverTime = (TickCount)(currentTime - Delay + RetryGetTooltipDelay);
            else
            {
                if (!tooltipTexts.IsEmpty())
                {
                    var texts = StyledText.ToMultiline(tooltipTexts, false, Font, MaxWidth);
                    _drawer.SetStyledTexts(texts);
                    UpdateBackground();
                }
            }
        }

        /// <summary>
        /// Handles updating the information needed for background of the <see cref="Tooltip"/>.
        /// </summary>
        protected virtual void UpdateBackground()
        {
            var maxWidth = _drawer.Texts.Max(x => x.Sum(y => y.GetWidth(Font)));
            float height = _drawer.Texts.Count() * Font.GetLineSpacing();
            _borderSize = new Vector2(maxWidth, height);
        }

        #region ITooltip Members

        /// <summary>
        /// Gets or sets the <see cref="Color"/> to draw the background of this <see cref="ITooltip"/>. This value
        /// is only valid for when the <see cref="ITooltip.Border"/> is null.
        /// </summary>
        public Color BackgroundColor
        {
            get { return _bgColor; }
            set { _bgColor = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="ControlBorder"/> for this <see cref="ITooltip"/>. Can be null.
        /// </summary>
        public ControlBorder Border { get; set; }

        /// <summary>
        /// Gets or sets number of pixels the border is padded on each side, where the X value corresponds to the
        /// number of pixels padded on the left and right side, and the Y value corresponds to the top and bottom.
        /// </summary>
        public Vector2 BorderPadding
        {
            get { return _borderPadding; }
            set { _borderPadding = value; }
        }

        /// <summary>
        /// Gets or sets the delay in milliseconds for the tooltip to be shown once a <see cref="Control"/> is
        /// hovered over. If this value is zero, the tooltip will be displayed immediately. If this value is set
        /// to less than zero, zero will be used instead.
        /// </summary>
        public int Delay
        {
            get { return _delay; }
            set { _delay = Math.Max(0, value); }
        }

        /// <summary>
        /// Gets or sets the offset from the cursor position to draw the <see cref="ITooltip"/>.
        /// </summary>
        public Vector2 DrawOffset
        {
            get { return _drawOffset; }
            set { _drawOffset = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Font"/> used by this <see cref="ITooltip"/>. By default, this value will
        /// be equal to the <see cref="IGUIManager"/>'s Font. Cannot be null.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        public Font Font
        {
            get { return _font; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (_font == value)
                    return;

                _font = value;
                _drawer.Font = _font;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Color"/> to draw the text when no color is specified.
        /// </summary>
        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        /// <summary>
        /// Gets the <see cref="IGUIManager"/> that this <see cref="ITooltip"/> is for.
        /// </summary>
        public IGUIManager GUIManager
        {
            get { return _guiManager; }
        }

        /// <summary>
        /// Gets if the <see cref="ITooltip"/> is currently being displayed.
        /// </summary>
        public bool IsDisplayed
        {
            get { return IsVisible && _drawer.Texts != null && !_tooltipTimedOut; }
        }

        /// <summary>
        /// Gets or sets if the <see cref="ITooltip"/> is currently visible. If false, the <see cref="ITooltip"/> will
        /// not make requests for the tooltip text from <see cref="Control"/>s and will not be drawn.
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

        /// <summary>
        /// Gets or sets the maximum width of the <see cref="ITooltip"/>.
        /// </summary>
        public int MaxWidth
        {
            get { return _maxWidth; }
            set { _maxWidth = value; }
        }

        /// <summary>
        /// Gets or sets the delay in milliseconds between retry attempts in getting the tooltip text. This
        /// only applies to when the <see cref="Control"/> returns a null collection for the tooltip text.
        /// </summary>
        public int RetryGetTooltipDelay
        {
            get { return _retryDelay; }
            set { _retryDelay = value; }
        }

        /// <summary>
        /// Gets or sets the timeout in milliseconds for the tooltip. After the tooltip has been displayed
        /// for the given amount of time, it will hide. If this value is zero, the tooltip will be displayed
        /// indefinitely. If the value is set to less than zero, zero will be used instead.
        /// </summary>
        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = Math.Max(0, value); }
        }

        /// <summary>
        /// Draws the <see cref="ITooltip"/>.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to draw the <see cref="ITooltip"/> with.</param>
        /// <exception cref="ArgumentNullException"><paramref name="sb" /> is <c>null</c>.</exception>
        public virtual void Draw(ISpriteBatch sb)
        {
            if (sb == null)
                throw new ArgumentNullException("sb");

            // Do nothing if not being displayed
            if (!IsDisplayed)
                return;

            Vector2 pos = GUIManager.CursorPosition + DrawOffset + BorderPadding;
            Vector2 size = _borderSize + (BorderPadding * 2);
            Vector2 ss = GUIManager.ScreenSize;

            // Ensure the tooltip is in the screen
            if (pos.X < 0)
                pos.X = 0;
            else if (pos.X + size.X > ss.X)
                pos.X = ss.X - size.X;

            if (pos.Y < 0)
                pos.Y = 0;
            else if (pos.Y + size.Y > ss.Y)
                pos.Y = ss.Y - size.Y;

            // Draw the border
            Rectangle borderRect = new Rectangle(pos.X, pos.Y, _borderSize.X, _borderSize.Y).Inflate(BorderPadding.X, BorderPadding.Y);

            ControlBorder b = _args.Border;
            if (b != null)
                b.Draw(sb, borderRect);
            else
                RenderRectangle.Draw(sb, borderRect, _args.BackgroundColor);

            // Draw the text
            _drawer.Draw(sb, _args.FontColor, pos);
        }

        /// <summary>
        /// Updates the <see cref="ITooltip"/>.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        public virtual void Update(TickCount currentTime)
        {
            // Do nothing if not visible
            if (!IsVisible)
                return;

            var currentUnderCursor = GUIManager.UnderCursor;

            // Check if the control under the cursor has changed
            if (_lastUnderCursor != currentUnderCursor)
            {
                _lastUnderCursor = currentUnderCursor;
                _startHoverTime = currentTime;
                _drawer.ClearTexts();
                _tooltipTimedOut = false;
            }

            // Check for no control under the cursor or the control has no tooltip
            if (currentUnderCursor == null || currentUnderCursor.Tooltip == null || _tooltipTimedOut)
                return;

            if (_drawer.Texts != null)
            {
                // If we already have the text, check if it is time to hide it
                if (_args.Timeout <= 0)
                    return;

                var timeoutTime = _startHoverTime + Delay + _args.Timeout;
                if (currentTime > timeoutTime)
                    _tooltipTimedOut = true;

                // Check to refresh the text
                if (_args.RefreshRate > 0 && currentTime - _lastRefreshTime > _args.RefreshRate)
                    RefreshText(currentTime);
            }
            else
            {
                // Check if enough time has elapsed for the tooltip to be displayed
                if (currentTime - _startHoverTime > Delay)
                {
                    _args.RestoreDefaults(this);
                    RefreshText(currentTime);
                }
            }
        }

        #endregion
    }
}