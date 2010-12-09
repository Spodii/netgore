using System.Linq;
using System.Windows.Forms;
using NetGore.Audio;
using NetGore.Content;
using NetGore.Editor;

namespace DemoGame.Editor
{
    public class SoundInfoListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoundInfoListBox"/> class.
        /// </summary>
        public SoundInfoListBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        protected virtual string GetDrawString(ISoundInfo item)
        {
            if (item == null)
                return null;

            return string.Format("{0}. {1}", item.ID, item.Name);
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.Control.CreateControl"/> method.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (DesignMode)
                return;

            UpdateList();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"/> that contains the event data. </param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (DesignMode || !ControlHelper.DrawListItem<ISoundInfo>(Items, e, x => GetDrawString(x)))
                base.OnDrawItem(e);
        }

        /// <summary>
        /// Updates the list.
        /// </summary>
        public void UpdateList()
        {
            var contentManager = ContentManager.Create();
            var audioManager = AudioManager.GetInstance(contentManager);
            var soundManager = audioManager.SoundManager;

            var items = soundManager.SoundInfos.Cast<object>().ToArray();
            var selected = SelectedItem;

            Items.Clear();
            Items.AddRange(items);

            if (selected != null)
                SelectedItem = selected;
        }
    }
}