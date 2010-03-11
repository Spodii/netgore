using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;
using NetGore.Audio;
using NetGore.IO;

namespace NetGore.EditorTools
{
    public partial class MusicUITypeEditorForm : UITypeEditorListForm<IMusic>
    {
        readonly ContentManager _cm;
        readonly IMusic _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="cm">The cm.</param>
        /// <param name="current">The currently selected <see cref="IMusic"/>. Can be null.</param>
        public MusicUITypeEditorForm(ContentManager cm, IMusic current)
        {
            _cm = cm;
            _current = current;

            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="cm">The cm.</param>
        /// <param name="current">The name of the currently selected <see cref="IMusic"/>. Can be null.</param>
        public MusicUITypeEditorForm(ContentManager cm, string current) : this(cm, MusicManager.GetInstance(cm).GetItem(current))
        {
        }

        /// <summary>
        /// When overridden in the derived class, draws the <paramref name="item"/>.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.DrawItemEventArgs"/> instance containing the event data.</param>
        /// <param name="item">The item being drawn.</param>
        protected override void DrawListItem(DrawItemEventArgs e, IMusic item)
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
        /// <returns>
        /// The item that will be selected by default.
        /// </returns>
        protected override IMusic SetDefaultSelectedItem()
        {
            return _current;
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
    }
}
