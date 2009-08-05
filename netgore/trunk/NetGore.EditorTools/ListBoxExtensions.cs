using System.Linq;
using System.Windows.Forms;

namespace NetGore.EditorTools
{
    public static class ListBoxExtensions
    {
        public static void AddItemAndReselect(this ListBox listBox, object item)
        {
            listBox.Items.Add(item);
            listBox.SelectedItem = item;
        }

        public static void RemoveItemAndReselect(this ListBox listBox, object item)
        {
            if (item == null)
                return;

            RemoveItemAtAndReselect(listBox, listBox.Items.IndexOf(item));
        }

        public static void RemoveItemAtAndReselect(this ListBox listBox, int itemIndex)
        {
            if (itemIndex < 0)
                return;

            int selectedIndex = listBox.SelectedIndex;

            bool removedSelectedItem = (selectedIndex == itemIndex);

            listBox.Items.RemoveAt(itemIndex);

            if (removedSelectedItem)
            {
                if (listBox.Items.Count == 0)
                    listBox.SelectedItem = null;
                else if (selectedIndex >= listBox.Items.Count)
                    listBox.SelectedIndex = selectedIndex - 1;
                else
                    listBox.SelectedIndex = selectedIndex;
            }
        }
    }
}