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
        /// <summary>
        /// Initializes a new instance of the <see cref="SoundEditorForm"/> class.
        /// </summary>
        public SoundEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Finds the next free <see cref="SoundID"/>.
        /// </summary>
        /// <param name="usedIDs">Collection of <see cref="SoundID"/>s already assigned.</param>
        /// <param name="start">The <see cref="SoundID"/> to start at.</param>
        /// <returns>The next free <see cref="SoundID"/>. The returned value will be marked as used in the
        /// <paramref name="usedIDs"/>.</returns>
        static SoundID NextFreeID(HashSet<SoundID> usedIDs, SoundID start)
        {
            while (!usedIDs.Add(start))
            {
                start++;
            }

            return start;
        }

        /// <summary>
        /// Handles the Click event of the btnUpdate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnUpdate_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            const string title = "Update sound";
            const string confirmMsg = "The following changes are to be made (+ for add, - for remove):";
            const string upToDateMsg = "The sound is already up-to-date.";
            const string acceptChangesMsg = "Accept these {0} changes and update the sound?";
            const string doneMsg = "Sound successfully updated!";

            var cm = ContentManager.Create();
            var sm = AudioManager.GetInstance(cm).SoundManager;

            // Find all the sound files
            var files = Directory.GetFiles(ContentPaths.Build.Sounds);

            // Find the new files (file exists, but SoundInfo does not)
            var newFiles =
                files.Where(f => !sm.SoundInfos.Any(si => StringComparer.OrdinalIgnoreCase.Equals(si.Name, Path.GetFileName(f)))).
                    ToArray();

            // Find the removed files (SoundInfo exists, but file does not)
            var removedFiles =
                sm.SoundInfos.Where(si => !files.Any(f => StringComparer.OrdinalIgnoreCase.Equals(si.Name, Path.GetFileName(f)))).
                    ToArray();

            // Check if there are any changes
            if (newFiles.Length <= 0 && removedFiles.Length <= 0)
            {
                MessageBox.Show(upToDateMsg, title, MessageBoxButtons.OK);
                return;
            }

            // Display list of changes

            var sb = new StringBuilder();
            sb.AppendLine(confirmMsg);

            const int maxLines = 25;
            var lines = 0;

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
            sb.AppendLine(string.Format(acceptChangesMsg, newFiles.Length + removedFiles.Length));

            if (MessageBox.Show(sb.ToString(), title, MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // Update by taking the existing SoundInfos, removing the ones to remove, adding the new ones

            var sis = sm.SoundInfos.ToList();

            foreach (var toRemove in removedFiles)
            {
                sis.Remove(toRemove);
            }

            var usedIDs = new HashSet<SoundID>();
            foreach (var si in sis)
            {
                usedIDs.Add(si.ID);
            }

            var soundIDCounter = new SoundID(1);

            foreach (var toAdd in newFiles)
            {
                var name = Path.GetFileName(toAdd);
                var id = NextFreeID(usedIDs, soundIDCounter);
                sis.Add(new SoundInfo(name, id));
            }

            // Save it all
            sm.ReloadData(sis);
            sm.Save();

            lstItems.UpdateList();

            MessageBox.Show(doneMsg, title, MessageBoxButtons.OK);
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
    }
}