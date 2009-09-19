using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    public class Tooltip
    {
        readonly GUIManagerBase _guiManager;

        int _delay = 1000;
        SpriteFont _font;
        bool _isVisible = true;
        Control _lastUnderCursor;
        int _retryDelay = 500;

        /// <summary>
        /// The time that the current control under the cursor started being hovered over.
        /// </summary>
        int _startHoverTime;

        int _timeout = 0;

        /// <summary>
        /// The text to display for the tooltip. Can be null.
        /// </summary>
        StyledText[] _tooltipText;

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
        /// Gets or sets the <see cref="SpriteFont"/> used by this <see cref="Tooltip"/>. By default, this value will
        /// be equal to the <see cref="GUIManager"/>'s Font.
        /// </summary>
        public SpriteFont Font
        {
            get { return _font; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _font = value;
            }
        }

        /// <summary>
        /// Gets the GUIManager that this <see cref="Tooltip"/> is for.
        /// </summary>
        public GUIManagerBase GUIManager
        {
            get { return _guiManager; }
        }

        /// <summary>
        /// Gets if the <see cref="Tooltip"/> is currently being displayed.
        /// </summary>
        public bool IsDisplayed
        {
            get { return IsVisible && _tooltipText != null; }
        }

        /// <summary>
        /// Gets or sets if the <see cref="Tooltip"/> is currently visible. If false, the <see cref="Tooltip"/> will
        /// not make requests for the tooltip text from <see cref="Control"/>s and will not be drawn.
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
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
        /// Initializes a new instance of the <see cref="Tooltip"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager.</param>
        public Tooltip(GUIManagerBase guiManager)
        {
            if (guiManager == null)
                throw new ArgumentNullException("guiManager");

            _guiManager = guiManager;
            _font = guiManager.Font;

            // Set the Tooltip in the GUIManager
            GUIManager.Tooltip = this;

            Debug.Assert(GUIManager.Tooltip == this);
            Debug.Assert(Font != null);
        }

        /// <summary>
        /// Draws the <see cref="Tooltip"/>.
        /// </summary>
        /// <param name="sb">The <see cref="SpriteBatch"/> to draw the <see cref="Tooltip"/> with.</param>
        public virtual void Draw(SpriteBatch sb)
        {
            if (sb == null)
                throw new ArgumentNullException("sb");

            // Do nothing if not being displayed
            if (!IsDisplayed)
                return;

            // Draw
            Vector2 pos = GUIManager.CursorPosition;
            for (int i = 0; i < _tooltipText.Length; i++)
            {
                _tooltipText[i].Draw(sb, Font, pos);
                pos.X += _tooltipText[i].GetWidth(Font);
            }
        }

        /// <summary>
        /// Updates the <see cref="Tooltip"/>.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        public virtual void Update(int currentTime)
        {
            // Do nothing if not visible
            if (!IsVisible)
                return;

            Control currentUnderCursor = GUIManager.UnderCursor;

            // Check if the control under the cursor has changed
            if (_lastUnderCursor != currentUnderCursor)
            {
                _lastUnderCursor = currentUnderCursor;
                _startHoverTime = currentTime;
                _tooltipText = null;
            }

            // Check for no control under the cursor, the control has no tooltip, or we already have the tooltip text 
            if (currentUnderCursor == null || currentUnderCursor.Tooltip == null || _tooltipText != null)
                return;

            // Check if enough time has elapsed for the tooltip to be displayed
            if (currentTime - _startHoverTime > Delay)
            {
                // Request the tooltip text
                _tooltipText = currentUnderCursor.Tooltip.Invoke(currentUnderCursor);

                // If the tooltip text is null, increase the _startHoverTime to result in the needed retry delay
                if (_tooltipText == null)
                    _startHoverTime = currentTime - Delay + RetryGetTooltipDelay;
            }
        }
    }
}