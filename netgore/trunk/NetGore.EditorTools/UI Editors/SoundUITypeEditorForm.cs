using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;
using NetGore.Audio;

namespace NetGore.EditorTools
{
    public class SoundUITypeEditorForm : UITypeEditorListForm<ISound>
    {
        readonly ContentManager _cm;
        readonly ISound _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="cm">The cm.</param>
        /// <param name="current">The currently selected <see cref="ISound"/>. Can be null.</param>
        public SoundUITypeEditorForm(ContentManager cm, ISound current)
        {
            _cm = cm;
            _current = current;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="cm">The cm.</param>
        /// <param name="current">The name of the currently selected <see cref="ISound"/>. Can be null.</param>
        public SoundUITypeEditorForm(ContentManager cm, string current) : this(cm, SoundManager.GetInstance(cm).GetItem(current))
        {
        }

        /// <summary>
        /// When overridden in the derived class, draws the <paramref name="item"/>.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.DrawItemEventArgs"/> instance containing the event data.</param>
        /// <param name="item">The item being drawn.</param>
        protected override void DrawListItem(DrawItemEventArgs e, ISound item)
        {
            e.DrawBackground();

            if (item == null)
                return;

            using (var brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(item.Index + ". " + item.Name, e.Font, brush, e.Bounds);
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected override IEnumerable<ISound> GetListItems()
        {
            var mm = SoundManager.GetInstance(_cm);
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
        protected override bool IsItemValid(ISound item)
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
        protected override ISound SetDefaultSelectedItem(IEnumerable<ISound> items)
        {
            return _current;
        }
    }
}