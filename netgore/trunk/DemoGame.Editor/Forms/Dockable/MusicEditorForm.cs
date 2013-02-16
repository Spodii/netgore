using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.Audio;
using NetGore.Content;
using WeifenLuo.WinFormsUI.Docking;
using NetGore.IO;

namespace DemoGame.Editor
{
    public partial class MusicEditorForm : DockContent
    {
        MusicManager musicManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicEditorForm"/> class.
        /// </summary>
        public MusicEditorForm()
        {
            InitializeComponent();

            musicManager = new MusicManager();
        }



        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstItems control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lstItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            pgItem.SelectedObject = lstItems.SelectedItem;
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {

            try
            {
                if (lstItems.SelectedItem != null)
                    musicManager.Play(((MusicInfo)lstItems.SelectedItem).ID);

            }

            catch (Exception ex)
            {
                MessageBox.Show("An error occured while tring to play the file! The given media file is invalid?");
            }


        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            musicManager.Stop();
        }

        private void MusicEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            musicManager.Stop();
            musicManager.Dispose();
        }
    }
}