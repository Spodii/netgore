using System;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Server;
using NetGore;
using NetGore.Editor;

namespace DemoGame.Editor.UITypeEditors
{
    public class CharacterTemplateInventoryItemListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTemplateInventoryItemListBox"/> class.
        /// </summary>
        public CharacterTemplateInventoryItemListBox()
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
        static string GetDrawString(CharacterTemplateInventoryItem x)
        {
            var t = ItemTemplateManager.Instance[x.ID];
            string item;

            if (t == null)
                item = "ID " + x;
            else
                item = t.ID + ". " + t.Name;

            var chance = string.Format("[{0:#,000.00}%] [{1} - {2}]", Math.Round(x.Chance.Percentage * 100, 2), x.Min, x.Max);

            return chance + " " + item;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"/> that contains the event data.</param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (DesignMode || !ControlHelper.DrawListItem<CharacterTemplateInventoryItem>(Items, e, x => GetDrawString(x)))
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

            var v = Items.Cast<CharacterTemplateInventoryItem>().OrderByDescending(x => x.Chance).ToImmutable();
            Items.Clear();
            Items.AddRange(v.Cast<object>().ToArray());
        }
    }
}