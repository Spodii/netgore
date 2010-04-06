using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;

namespace DemoGame.Client
{
    /// <summary>
    /// The <see cref="Form"/> that is used to contain the game screen.
    /// </summary>
    public class GameForm : Form
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly DemoGame _game;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameForm"/> class.
        /// </summary>
        public GameForm()
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor

            // Set up our form
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            AutoScaleMode = AutoScaleMode.Font;
            Text = "NetGore";
            ClientSize = new Size((int)GameData.ScreenSize.X, (int)GameData.ScreenSize.Y);
            MaximizeBox = false;

            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            // Create the game
            _game = new DemoGame(Handle);

            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                bool isOpened;
                try
                {
                    isOpened = _game.IsOpened();
                }
                catch (AccessViolationException)
                {
                    // SFML likes to throw an AccessViolationException when the game is disposed
                    isOpened = false;
                }

                // If the game was not closed, close it and abort so the main loop can take care of it
                if (isOpened)
                {
                    try
                    {
                        _game.Dispose();
                    }
                    catch (Exception ex)
                    {
                        const string errmsg = "Exception thrown when trying to dispose the game: {0}";
                        if (log.IsWarnEnabled)
                            log.WarnFormat(errmsg, ex);
                        Debug.Fail(string.Format(errmsg, ex));
                    }
                }
            }
            finally
            {
                base.OnClosing(e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // This makes sure that certain key strokes do not active Windows events, such as pressing alt to bring up
            // the form's menu. It is recommended you leave this here so it doesn't disturb players.
            // Try to avoid actually handling key events in the form. Do it through the game instead.
            e.SuppressKeyPress = true;
            e.Handled = true;

            base.OnKeyDown(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyPress"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyPressEventArgs"/> that contains the event data.</param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            // This makes sure that certain key strokes do not active Windows events, such as pressing alt to bring up
            // the form's menu. It is recommended you leave this here so it doesn't disturb players.
            // Try to avoid actually handling key events in the form. Do it through the game instead.
            e.Handled = true;

            base.OnKeyPress(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyUp"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            // This makes sure that certain key strokes do not active Windows events, such as pressing alt to bring up
            // the form's menu. It is recommended you leave this here so it doesn't disturb players.
            // Try to avoid actually handling key events in the form. Do it through the game instead.
            e.SuppressKeyPress = true;
            e.Handled = true;

            base.OnKeyUp(e);
        }

        /// <summary>
        /// Paints the screen.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Check if the game is running
            bool isOpened;
            try
            {
                isOpened = _game.IsOpened();
            }
            catch (AccessViolationException)
            {
                // SFML likes to throw an AccessViolationException when the game is disposed
                isOpened = false;
            }

            try
            {
                // If the game is running, handle the next frame. Otherwise, close the form.
                if (isOpened)
                    _game.HandleFrame();
                else
                    Close();
            }
            finally
            {
                // Invalidate the whole screen so that it will be fully redrawn as soon as possible
                Invalidate();
            }
        }
    }
}