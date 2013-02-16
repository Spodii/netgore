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
    public partial class SoundEditorForm : DockContent
    {

        SoundManager soundManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundEditorForm"/> class.
        /// </summary>
        public SoundEditorForm()
        {
            InitializeComponent();

            soundManager = new SoundManager(NetGore.Content.ContentManager.Create());
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
                    soundManager.Play(((SoundInfo)lstItems.SelectedItem).ID);

            }

            catch (Exception ex)
            {
                MessageBox.Show("An error occured while tring to play the file! The given media file is invalid?");
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            soundManager.Stop();
        }

        private void SoundEditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            soundManager.Stop();
        }

    }
}