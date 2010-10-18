using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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

        DemoGame _game;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameForm"/> class.
        /// </summary>
        public GameForm()
        {
#if !MONO
            Application.Idle += Application_Idle;
#endif

            InitializeComponent();

            // Set up our form
            ClientSize = new Size((int)GameData.ScreenSize.X, (int)GameData.ScreenSize.Y);

            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. 
        ///                 </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Focus();
            Refresh();
            Update();
            Show();

            // Create the game
            _game = new DemoGame(this);
        }

        /// <summary>
        /// Handles a frame of the main game loop.
        /// </summary>
        void HandleFrame()
        {
            try
            {
                // If the game has not been created yet, just don't do anything
                if (_game == null)
                    return;

                // If the game is running, handle the next frame. Otherwise, close the form.
                if (!_game.IsDisposed)
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
                if (_game != null && !_game.IsDisposed)
                    _game.Dispose();
            }
            catch (Exception ex)
            {
                const string errmsg = "Exception thrown while trying to close the form. Exception: {0}";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, ex);
                Debug.Fail(string.Format(errmsg, ex));
            }

            base.OnClosing(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnClosed(EventArgs e)
        {
#if !MONO
            Application.Idle -= Application_Idle;
#endif

            base.OnClosed(e);
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

        #region Windows-specific game loop
#if !MONO
        /// <summary>
        /// Handles the <see cref="Application.Idle"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void Application_Idle(object sender, EventArgs e)
        {
            while (IsAppStillIdle)
            {
                HandleFrame();
            }
        }

        /// <summary>
        /// Gets if there are any system messages waiting for this application.
        /// </summary>
        private static bool IsAppStillIdle
        {
            get
            {
                Message msg;
                return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
            }
        }

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);
#endif
        #endregion

        #region Mono-friendly game loop
#if MONO
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x000F)
                HandleFrame();
            else
                base.WndProc(ref m);
        }
#endif
        #endregion
    }
}