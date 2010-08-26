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

        /// <summary>
        /// Handles the Click event of the btnNewVersion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnNewVersion_Click(object sender, EventArgs e)
        {
            var newVersionForm = new NewVersionForm();
            btnNewVersion.Enabled = false;
            newVersionForm.FormClosed += newVersionForm_FormClosed;

            newVersionForm.Show();
        }

        /// <summary>
        /// Handles the FormClosed event of the newVersionForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosedEventArgs"/> instance containing the event data.</param>
        void newVersionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            btnNewVersion.Enabled = true;
        }
    }
}
