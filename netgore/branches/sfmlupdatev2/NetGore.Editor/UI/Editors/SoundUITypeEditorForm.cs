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
    public class SoundUITypeEditorForm : UITypeEditorListForm<ISoundInfo>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly ISoundInfo _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="cm">The cm.</param>
        /// <param name="current">The currently selected <see cref="ISoundInfo"/>. Can be null.</param>
        public SoundUITypeEditorForm(IContentManager cm, object current)
        {
            ISoundManager sm;
            try
            {
                sm = AudioManager.GetInstance(cm).SoundManager;
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
                if (current is SoundID)
                    _current = sm.GetSoundInfo((SoundID)current);
                else if (current is SoundID? && ((SoundID?)current).HasValue)
                    _current = sm.GetSoundInfo(((SoundID?)current).Value);
                else if (current is ISoundInfo)
                    _current = (ISoundInfo)current;
                else
                    _current = sm.GetSoundInfo(current.ToString());
            }
        }

        /// <summary>
        /// Gets the string to display for an item.
        /// </summary>
        /// <param name="item">The item to get the display string for.</param>
        /// <returns>The string to display for the <paramref name="item"/>.</returns>
        protected override string GetItemDisplayString(ISoundInfo item)
        {
            return item.ID + ". " + item.Name;
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected override IEnumerable<ISoundInfo> GetListItems()
        {
            ISoundManager sm;
            try
            {
                sm = AudioManager.GetInstance(null).SoundManager;
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

            return sm.SoundInfos.OrderBy(x => x.ID);
        }

        /// <summary>
        /// When overridden in the derived class, gets if the given <paramref name="item"/> is valid to be
        /// used as the returned item.
        /// </summary>
        /// <param name="item">The item to validate.</param>
        /// <returns>
        /// If the given <paramref name="item"/> is valid to be used as the returned item.
        /// </returns>
        protected override bool IsItemValid(ISoundInfo item)
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
        protected override ISoundInfo SetDefaultSelectedItem(IEnumerable<ISoundInfo> items)
        {
            return _current;
        }
    }
}