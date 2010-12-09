using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.Audio;
using NetGore.Content;
using NetGore.Editor.Docking;
using NetGore.Editor.EditorTool;
using NetGore.IO;

namespace DemoGame.Editor
{
    public partial class MusicEditorForm : DockContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MusicEditorForm"/> class.
        /// </summary>
        public MusicEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstItems control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lstItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            pgItem.SelectedObject = lstItems.SelectedItem;
        }

        /// <summary>
        /// Handles the Click event of the btnUpdate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            const string title = "Update music";

            var cm = ContentManager.Create();
            var mm = AudioManager.GetInstance(cm).MusicManager;

            // Find all the music files
            var files = Directory.GetFiles(ContentPaths.Build.Music);

            // Find the new files (file exists, but MusicInfo does not)
            var newFiles = files.Where(f => !mm.MusicInfos.Any(mi => StringComparer.OrdinalIgnoreCase.Equals(mi.Name, Path.GetFileName(f)))).ToArray();

            // Find the removed files (MusicInfo exists, but file does not)
            var removedFiles = mm.MusicInfos.Where(mi => !files.Any(f => StringComparer.OrdinalIgnoreCase.Equals(mi.Name, Path.GetFileName(f)))).ToArray();
        
            // Check if there are any changes
            if (newFiles.Length <= 0 && removedFiles.Length <= 0)
            {
                const string upToDateMsg = "The music is already up-to-date.";
                MessageBox.Show(upToDateMsg, title, MessageBoxButtons.OK);
                return;
            }

            // Display list of changes
            const string confirmMsg = "The following changes are to be made (+ for add, - for remove):";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(confirmMsg);

            const int maxLines = 25;
            int lines = 0;

            foreach (var f in removedFiles)
            {
                sb.AppendLine(" - " + Path.GetFileName(f.Name) + " [" + f.ID + "]");
                if (++lines > maxLines)
                    break;
            }

            foreach (var f in newFiles)
            {
                sb.AppendLine(" + " + Path.GetFileName(f));
                if (++lines > maxLines)
                    break;
            }

            sb.AppendLine();
            sb.AppendLine("Accept these " + (newFiles.Length + removedFiles.Length) + " changes and update the music?");

            if (MessageBox.Show(sb.ToString(), title, MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // Update by taking the existing MusicInfos, removing the ones to remove, adding the new ones

            var mis = mm.MusicInfos.ToList();

            foreach (var toRemove in removedFiles)
                mis.Remove(toRemove);

            var usedIDs = new HashSet<MusicID>();
            foreach (var mi in mis)
                usedIDs.Add(mi.ID);

            var musicIDCounter = new MusicID(1);

            foreach (var toAdd in newFiles)
            {
                var name = Path.GetFileName(toAdd);
                var id = NextFreeID(usedIDs, musicIDCounter);
                mis.Add(new MusicInfo(name, id));
            }

            // Save it all
            mm.ReloadData(mis);
            mm.Save();

            lstItems.UpdateList();
        }

        /// <summary>
        /// Finds the next free <see cref="MusicID"/>.
        /// </summary>
        /// <param name="usedIDs">Collection of <see cref="MusicID"/>s already assigned.</param>
        /// <param name="start">The <see cref="MusicID"/> to start at.</param>
        /// <returns>The next free <see cref="MusicID"/>. The returned value will be marked as used in the
        /// <paramref name="usedIDs"/>.</returns>
        static MusicID NextFreeID(HashSet<MusicID> usedIDs, MusicID start)
        {
            while (!usedIDs.Add(start))
            {
                start++;
            }

            return start;
        }
    }
}
