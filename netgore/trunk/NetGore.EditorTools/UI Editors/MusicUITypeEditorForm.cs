using System.Collections.Generic;
using System.Linq;
using NetGore.Audio;
using NetGore.Content;

namespace NetGore.EditorTools
{
    public class MusicUITypeEditorForm : UITypeEditorListForm<IMusic>
    {
        readonly IContentManager _cm;
        readonly IMusic _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="cm">The cm.</param>
        /// <param name="current">The currently selected <see cref="IMusic"/>. Can be null.</param>
        public MusicUITypeEditorForm(IContentManager cm, object current)
        {
            _cm = cm;

            var mm = MusicManager.GetInstance(cm);

            if (current != null)
            {
                if (current is MusicID)
                    _current = mm.GetItem((MusicID)current);
                else if (current is MusicID? && ((MusicID?)current).HasValue)
                    _current = mm.GetItem(((MusicID?)current).Value);
                else if (current is IMusic)
                    _current = (IMusic)current;
                else
                    _current = mm.GetItem(current.ToString());
            }
        }

        /// <summary>
        /// Gets the string to display for an item.
        /// </summary>
        /// <param name="item">The item to get the display string for.</param>
        /// <returns>
        /// The string to display for the <paramref name="item"/>.
        /// </returns>
        protected override string GetItemDisplayString(IMusic item)
        {
            return item.Index + ". " + item.Name;
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected override IEnumerable<IMusic> GetListItems()
        {
            var mm = MusicManager.GetInstance(_cm);
            return mm.Items.OrderBy(x => x.Index);
        }

        /// <summary>
        /// When overridden in the derived class, gets if the given <paramref name="item"/> is valid to be
        /// used as the returned item.
        /// </summary>
        /// <param name="item">The item to validate.</param>
        /// <returns>
        /// If the given <paramref name="item"/> is valid to be used as the returned item.
        /// </returns>
        protected override bool IsItemValid(IMusic item)
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
        protected override IMusic SetDefaultSelectedItem(IEnumerable<IMusic> items)
        {
            return _current;
        }
    }
}