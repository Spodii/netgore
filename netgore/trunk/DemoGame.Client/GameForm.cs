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
        public GameForm(DemoGame game)
        {
            DisposeGameOnClose = true;

            _game = game;

            InitializeComponent();

            // Set up our form
            ClientSize = new Size((int)GameData.ScreenSize.X, (int)GameData.ScreenSize.Y);

            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }

        /// <summary>
        /// Gets or sets if the game will be disposed when this form closes.
        /// </summary>
        [DefaultValue(true)]
        public bool DisposeGameOnClose { get; set; }

        /// <summary>
        /// Handles a frame of the main game loop.
        /// </summary>
        void HandleFrame()
        {

            //TODO: find out why exception swalloer causes issues
           // try
           // {
                // If the game has not been created yet, just don't do anything
                if (_game == null)
                    return;

                // If the game is running, handle the next frame. Otherwise, close the form.
                if (!_game.IsDisposed)
                    _game.HandleFrame();
                else
                    Close();
           // }
           // catch (Exception ex)
            //{
             //   ExceptionSwallower.Instance.Swallow(ex);
              //  if (ExceptionSwallower.Instance.Rethrow)
               //     throw;
           // }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                if (DisposeGameOnClose && _game != null && !_game.IsDisposed)
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
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (_game != null)
                _game.Title = Text;
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