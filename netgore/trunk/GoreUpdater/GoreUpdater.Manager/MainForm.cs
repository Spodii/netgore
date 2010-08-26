using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoreUpdater.Manager
{
    public partial class MainForm : Form
    {
        static readonly ManagerSettings _settings = ManagerSettings.Instance;

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _settings.LiveVersionChanged += _settings_LiveVersionChanged;
            lblLiveVersion.Text = _settings.LiveVersion.ToString();
        }

        void _settings_LiveVersionChanged(ManagerSettings sender)
        {
            lblLiveVersion.Invoke((Action)(() => lblLiveVersion.Text = _settings.LiveVersion.ToString()));
        }
    }
}
