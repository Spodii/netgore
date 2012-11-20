using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace DemoGame.Editor
{
    /// <summary>
    /// Manages the F1 help for the editor.
    /// </summary>
    public class EditorHelpManager
    {
        const string _helpMessageFormat = "Press F1 to get help for: {0}";
        const string _wikiPageUrlFormat = "http://www.netgore.com/wiki/{0}";

        class HelpInfo
        {
            public string DisplayName { get; set; }
            public string WikiPage { get; set; }
        }

        static readonly EditorHelpManager _instance;

        /// <summary>
        /// Gets the EditorHelpManager instance.
        /// </summary>
        public static EditorHelpManager Instance { get { return _instance; } }

        static EditorHelpManager()
        {
            _instance = new EditorHelpManager();
        }

        readonly Dictionary<Control, HelpInfo> _helpInfos = new Dictionary<Control, HelpInfo>();
        readonly EventHandler _mouseEnterHandler;
        readonly EventHandler _mouseLeaveHandler;
        readonly EventHandler _disposedHandler;

        Control _mouseOverControl;

        /// <summary>
        /// Gets or sets the help-enabled control the mouse is over.
        /// </summary>
        Control MouseOverControl
        {
            get
            {
                return _mouseOverControl;
            }
            set
            {
                if (_mouseOverControl == value)
                    return;

                Control oldValue = _mouseOverControl;
                _mouseOverControl = value;

                var lbl = StatusLabel;
                if (lbl != null)
                {
                    if (value == null)
                    {
                        string oldMsg = GetHelpMessage(oldValue);
                        if (lbl.Text == oldMsg)
                            lbl.Text = string.Empty;
                    }
                    else
                    {
                        string newMsg = GetHelpMessage(value);
                        lbl.Text = newMsg;
                    }
                }
            }
        }

        EditorHelpManager()
        {
            _mouseEnterHandler = ctrl_MouseEnter;
            _mouseLeaveHandler = ctrl_MouseLeave;
            _disposedHandler = ctrl_Disposed;
        }

        /// <summary>
        /// Raises the help for the control currently under the cursor.
        /// </summary>
        /// <returns>True if the help was raised; otherwise false.</returns>
        public bool RaiseHelp()
        {
            HelpInfo helpInfo = GetHelpInfo(_mouseOverControl);
            if (helpInfo == null)
                return false;

            string url = string.Format(_wikiPageUrlFormat, helpInfo.WikiPage);
            Process.Start(url);

            MouseOverControl = null;

            return true;
        }

        /// <summary>
        /// Gets or sets the control to to use to display the help message.
        /// </summary>
        public ToolStripItem StatusLabel { get; set; }

        /// <summary>
        /// Adds the help for a control.
        /// </summary>
        /// <param name="control">The control to add the help for.</param>
        /// <param name="displayName">The name to display in the status bar.</param>
        /// <param name="wikiPage">The wiki page to navigate to.</param>
        public void Add(Control control, string displayName, string wikiPage)
        {
            _helpInfos[control] = new HelpInfo { DisplayName = displayName, WikiPage = wikiPage };

            SetEventListeners(control, false);
            SetEventListeners(control, true);
        }

        /// <summary>
        /// Removes the help for a control.
        /// </summary>
        /// <param name="control">The control to remove the help for.</param>
        public void Remove(Control control)
        {
            _helpInfos.Remove(control);
            SetEventListeners(control, false);
        }

        string GetHelpMessage(Control ctrl)
        {
            return GetHelpMessage(GetHelpInfo(ctrl));
        }

        string GetHelpMessage(HelpInfo helpInfo)
        {
            if (helpInfo == null)
                return null;

            return string.Format(_helpMessageFormat, helpInfo.DisplayName);
        }

        HelpInfo GetHelpInfo(Control ctrl)
        {
            if (ctrl == null)
                return null;

            HelpInfo ret;
            if (!_helpInfos.TryGetValue(ctrl, out ret))
                return null;

            return ret;
        }

        /// <summary>
        /// Sets (or unsets) the event listeners on a control.
        /// </summary>
        /// <param name="ctrl">The control to set the events on.</param>
        /// <param name="add">True to add listeners; false to remove.</param>
        void SetEventListeners(Control ctrl, bool add)
        {
            if (ctrl == null)
                return;

            try
            {
                if (add)
                {
                    ctrl.MouseEnter += _mouseEnterHandler;
                    ctrl.MouseLeave += _mouseLeaveHandler;
                    ctrl.Disposed += _disposedHandler;
                }
                else
                {
                    ctrl.MouseEnter -= _mouseEnterHandler;
                    ctrl.MouseLeave -= _mouseLeaveHandler;
                    ctrl.Disposed -= _disposedHandler;
                }
            }
            catch
            {
            }
        }

        void ctrl_Disposed(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (c == null)
                return;

            Remove(c);
        }

        void ctrl_MouseLeave(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (c == null)
                return;

            if (MouseOverControl == c)
                MouseOverControl = null;
        }

        void ctrl_MouseEnter(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (c == null)
                return;

            MouseOverControl = c;
        }
    }
}