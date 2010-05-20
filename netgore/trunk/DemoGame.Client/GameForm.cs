using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using log4net;

namespace DemoGame.Client
{
    /// <summary>
    /// The <see cref="Form"/> that is used to contain the game screen.
    /// </summary>
    public partial class GameForm : Form
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly DemoGame _game;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameForm"/> class.
        /// </summary>
        public GameForm()
        {
            InitializeComponent();

            // Set up our form
            ClientSize = new Size((int)GameData.ScreenSize.X, (int)GameData.ScreenSize.Y);

            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            // Create the game
            _game = new DemoGame(Handle);
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

        /// <summary>
        /// Processes a command key.
        /// </summary>
        /// <param name="msg">A <see cref="T:System.Windows.Forms.Message"/>, passed by reference, that represents the
        /// Win32 message to process.</param>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys"/> values that represents
        /// the key to process.</param>
        /// <returns>
        /// true if the keystroke was processed and consumed by the control; otherwise, false to allow further processing.
        /// </returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Prevents closing the form via alt+F4
            if (keyData == (Keys.Alt | Keys.F4))
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
