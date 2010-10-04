using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using NetGore.Audio;
using NetGore.Content;

namespace NetGore.Editor.UI
{
    public class MusicUITypeEditorForm : UITypeEditorListForm<IMusicInfo>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly IMusicInfo _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="cm">The cm.</param>
        /// <param name="current">The currently selected <see cref="IMusicInfo"/>. Can be null.</param>
        public MusicUITypeEditorForm(IContentManager cm, object current)
        {
            IMusicManager mm;

            try
            {
                mm = AudioManager.GetInstance(cm).MusicManager;
            }
            catch (ArgumentNullException ex)
            {
                const string errmsg = "Failed to get AudioManager instance when passing a null ContentManager: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                DialogResult = DialogResult.Abort;
                Close();
                return;
            }

            if (current != null)
            {
                if (current is MusicID)
                    _current = mm.GetMusicInfo((MusicID)current);
                else if (current is MusicID? && ((MusicID?)current).HasValue)
                    _current = mm.GetMusicInfo(((MusicID?)current).Value);
                else if (current is IMusicInfo)
                    _current = (IMusicInfo)current;
                else
                    _current = mm.GetMusicInfo(current.ToString());
            }
        }

        /// <summary>
        /// Gets the string to display for an item.
        /// </summary>
        /// <param name="item">The item to get the display string for.</param>
        /// <returns>
        /// The string to display for the <paramref name="item"/>.
        /// </returns>
        protected override string GetItemDisplayString(IMusicInfo item)
        {
            return item.ID + ". " + item.Name;
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected override IEnumerable<IMusicInfo> GetListItems()
        {
            IMusicManager mm;
            try
            {
                mm = AudioManager.GetInstance(null).MusicManager;
            }
            catch (ArgumentNullException ex)
            {
                const string errmsg = "Failed to get AudioManager instance when passing a null ContentManager: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                DialogResult = DialogResult.Abort;
                Close();
                return null;
            }

            return mm.MusicInfos.OrderBy(x => x.ID);
        }

        /// <summary>
        /// When overridden in the derived class, gets if the given <paramref name="item"/> is valid to be
        /// used as the returned item.
        /// </summary>
        /// <param name="item">The item to validate.</param>
        /// <returns>
        /// If the given <paramref name="item"/> is valid to be used as the returned item.
        /// </returns>
        protected override bool IsItemValid(IMusicInfo item)
        {
            return item != null;
        }

        /// <summary>
        /// When overridden in the derived class, sets the item that will be selected by default.
        /// </summary>
        /// <param name="items">The items to choose from.</param>
        /// <returns>
        /// The item that will be selected by default.
        /// </returns>
        protected override IMusicInfo SetDefaultSelectedItem(IEnumerable<IMusicInfo> items)
        {
            return _current;
        }
    }
}