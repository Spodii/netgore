using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Server;
using NetGore;
using NetGore.Editor;

namespace DemoGame.Editor.UITypeEditors
{
    public class ItemTemplateAmountListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemTemplateAmountListBox"/> class.
        /// </summary>
        public ItemTemplateAmountListBox()
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
        /// <param name="x"></param>
        /// <returns></returns>
        static KeyValuePair<string, string> GetDrawString(MutablePair<ItemTemplateID, byte> x)
        {
            var keyStr = x.Key.ToString();

            var itm = ItemTemplateManager.Instance;
            if (itm != null)
            {
                var template = itm[x.Key];
                if (template != null)
                    keyStr += " [" + template.Name + "]";
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
            if (DesignMode || !ControlHelper.DrawListItem<MutablePair<ItemTemplateID, byte>>(Items, e, x => GetDrawString(x)))
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

            var v = Items.OfType<MutablePair<ItemTemplateID, byte>>().ToImmutable();
            Items.Clear();
            Items.AddRange(v.Cast<object>().ToArray());
        }
    }
}