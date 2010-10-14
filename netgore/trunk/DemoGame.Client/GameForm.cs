using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using NetGore;

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

            Focus();
        }

        void HandleFrame()
        {
            try
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

                // If the game is running, handle the next frame. Otherwise, close the form.
                if (isOpened)
                    _game.HandleFrame();
                else
                    Close();
            }
            catch (Exception ex)
            {
                ExceptionSwallower.Instance.Swallow(ex);
                if (ExceptionSwallower.Instance.Rethrow)
                    throw;
            }
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
            try
            {
                // Prevents closing the form via alt+F4
                if (keyData == (Keys.Alt | Keys.F4))
                    return true;

                return base.ProcessCmdKey(ref msg, keyData);
            }
            catch (Exception ex)
            {
                ExceptionSwallower.Instance.Swallow(ex);
                if (ExceptionSwallower.Instance.Rethrow)
                    throw;
            }

            return false;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x000F)
                HandleFrame();
            else
                base.WndProc(ref m);
        }
    }
}