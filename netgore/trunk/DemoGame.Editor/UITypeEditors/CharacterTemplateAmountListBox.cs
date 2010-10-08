using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Server;
using NetGore;
using NetGore.Editor;

namespace DemoGame.Editor.UITypeEditors
{
    public class CharacterTemplateAmountListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTemplateAmountListBox"/> class.
        /// </summary>
        public CharacterTemplateAmountListBox()
        {
            if (DesignMode)
                return;

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            DrawMode = DrawMode.OwnerDrawFixed;
            Sorted = true;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Gets the string to draw for a list item.
        /// </summary>
        /// <param name="x">The list item.</param>
        /// <returns>The string to draw for a list item.</returns>
        static KeyValuePair<string, string> GetDrawString(MutablePair<CharacterTemplateID, ushort> x)
        {
            var keyStr = x.Key.ToString();

            var itm = CharacterTemplateManager.Instance;
            if (itm != null)
            {
                var template = itm[x.Key];
                if (template != null)
                    keyStr += " [" + template.TemplateTable.Name + "]";
            }
            keyStr += " - ";

            return new KeyValuePair<string, string>(keyStr, x.Value.ToString());
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"/> that contains the event data.</param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (DesignMode ||
                !ControlHelper.DrawListItem<MutablePair<CharacterTemplateID, ushort>>(Items, e, x => GetDrawString(x)))
                base.OnDrawItem(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (DesignMode)
                return;

            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Delete)
                this.DeleteSelectedItem();
        }

        /// <summary>
        /// Sorts the items in the <see cref="T:System.Windows.Forms.ListBox"/>.
        /// </summary>
        protected override void Sort()
        {
            if (DesignMode)
                return;

            var v = Items.Cast<MutablePair<CharacterTemplateID, ushort>>().ToImmutable();
            Items.Clear();
            Items.AddRange(v.Cast<object>().ToArray());
        }
    }
}