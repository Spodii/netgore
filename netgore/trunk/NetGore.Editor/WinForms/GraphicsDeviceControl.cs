using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using SFML.Graphics;
using Color = System.Drawing.Color;

namespace NetGore.Editor.WinForms
{
    /// <summary>
    /// Custom control that uses the graphics device to draw onto a WinForms control.
    /// Derived classes can override the Initialize and Draw methods to add their own drawing code.
    /// </summary>
    public class GraphicsDeviceControl : Control
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Timer _redrawTimer;

        bool _isInitialized = false;
        string _lastDrawError = null;
        RenderWindow _rw;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsDeviceControl"/> class.
        /// </summary>
        public GraphicsDeviceControl()
        {
            ForeColor = Color.White;
            BackColor = Color.Black;
            DoubleBuffered = false;
            ResizeRedraw = false;

            if (!DesignMode)
                _redrawTimer = new Timer { Interval = 1000 / 100 };
        }

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <returns>A <see cref="T:System.Drawing.Color"/> that represents the background color of the control.</returns>
        [DefaultValue(typeof(Color), "Black")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        /// <summary>
        /// Gets or sets the foreground color of the control.
        /// </summary>
        /// <returns>The foreground <see cref="T:System.Drawing.Color"/> of the control.</returns>
        [DefaultValue(typeof(Color), "White")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        /// <summary>
        /// Gets a RenderWindow this control can use.
        /// </summary>
        [Browsable(false)]
        public RenderWindow RenderWindow
        {
            get { return _rw; }
        }

        /// <summary>
        /// Attempts to begin drawing the control. Returns an error message string
        /// if this was not possible, which can happen if the graphics device is
        /// lost, or if we are running inside the Form designer.
        /// </summary>
        string BeginDraw()
        {
            if (IsDisposed)
                return "Control is disposed.";

            if (_rw == null || _rw.IsDisposed)
                return "RenderWindow is null or disposed.";

            if (RecreatingHandle)
                return "Handle is being recreated...";

            if (_rw.SystemHandle != Handle && Handle != IntPtr.Zero)
                RecreateRenderWindow(Handle);

            // If we have no graphics device we must be running in the designer
            if (DesignMode || _rw == null)
                return Text + Environment.NewLine + Environment.NewLine + GetType();

            return null;
        }

        /// <summary>
        /// Disposes the control
        /// </summary>
        /// <param name="disposing">If true, disposes of managed resources</param>
        protected override void Dispose(bool disposing)
        {
            if (!DesignMode && disposing)
            {
                // Stop the redraw timer
                _redrawTimer.Stop();
                _redrawTimer.Dispose();

                // Dispose the RenderWindow
                if (_rw != null)
                {
                    try
                    {
                        _rw.Dispose();
                    }
                    catch (Exception ex)
                    {
                        const string errmsg = "Failed to dispose RenderWindow: {0}";
                        if (log.IsWarnEnabled)
                            log.WarnFormat(errmsg, ex);
                        Debug.Fail(string.Format(errmsg, ex));
                    }
                }

                _rw = null;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Ends drawing the control.
        /// </summary>
        protected virtual void EndDraw()
        {
            _rw.Display();
        }

        /// <summary>
        /// When overridden in the derived class, draws the graphics to the control.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        protected virtual void HandleDraw(TickCount currentTime)
        {
        }

        /// <summary>
        /// Derived classes override this to initialize their drawing code.
        /// </summary>
        protected virtual void Initialize()
        {
        }

        /// <summary>
        /// Invokes this control to draw itself.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NetGore.EventArgs{TickCount}"/> instance containing the event data.</param>
        public void InvokeDrawing(object sender, EventArgs<TickCount> e)
        {
            if (!Visible || DesignMode)
                return;

            try
            {
                _lastDrawError = BeginDraw();
                if (_lastDrawError != null)
                    return;

                HandleDraw(e.Item1);
                EndDraw();
            }
            catch (Exception ex)
            {
                const string errmsg = "Exception thrown while drawing on `{0}`: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, ex);
                _lastDrawError = "Exception thrown while drawing: " + ex;
            }
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void OnCreateControl()
        {
            // Don't create the RenderWindow in the designer
            if (!DesignMode)
            {
                RecreateRenderWindow(Handle);

                if (!_isInitialized)
                {
                    // Give derived classes a chance to initialize themselves
                    Initialize();

                    // Create the redraw timer
                    _redrawTimer.Tick += delegate { Invalidate(); };
                    _redrawTimer.Start();

                    _isInitialized = true;
                }
            }

            base.OnCreateControl();
        }

        /// <summary>
        /// Handles MouseDown events.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!DesignMode)
                Focus();

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Redraws the control in response to a WinForms paint message.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // In design mode, use some custom, basic drawing
            if (DesignMode || _lastDrawError != null)
                base.OnPaint(e);
        }

        /// <summary>
        /// Ignore WinForms paint-background messages, unless using design mode.
        /// </summary>
        /// <param name="pevent">Event args.</param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (DesignMode || _lastDrawError != null)
                base.OnPaintBackground(pevent);
        }

        /// <summary>
        /// Allows derived classes to handle when the <see cref="RenderWindow"/> is created or re-created.
        /// </summary>
        /// <param name="newRenderWindow">The current <see cref="RenderWindow"/>.</param>
        protected virtual void OnRenderWindowCreated(RenderWindow newRenderWindow)
        {
        }

        void RecreateRenderWindow(IntPtr handle)
        {
            if (_rw != null)
            {
                if (!_rw.IsDisposed)
                    _rw.Dispose();

                _rw = null;
            }

            _rw = new RenderWindow(handle);
            _rw.SetVerticalSyncEnabled(false);
            _rw.SetVisible(true);

            OnRenderWindowCreated(_rw);
        }
    }
}