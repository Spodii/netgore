using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NetGore;
using NetGore.Editor.UI;

namespace DemoGame.Editor.UITypeEditors
{
    /// <summary>
    /// A <see cref="Form"/> for listing the <see cref="GameMessageCollection"/> language.
    /// </summary>
    public class GameMessageCollectionLanguageEditorUITypeEditorForm : UITypeEditorListForm<string>
    {
        readonly object _selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameMessageCollectionLanguageEditorUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="selected">The default selected item.</param>
        public GameMessageCollectionLanguageEditorUITypeEditorForm(object selected)
        {
            _selected = selected;
        }

        /// <summary>
        /// Gets the string to display for an item.
        /// </summary>
        /// <param name="item">The item to get the display string for.</param>
        /// <returns>The string to display for the <paramref name="item"/>.</returns>
        protected override string GetItemDisplayString(string item)
        {
            return item;
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected override IEnumerable<string> GetListItems()
        {
            return GameMessageCollection.GetLanguages().OrderBy(x => x, NaturalStringComparer.Instance);
        }

        /// <summary>
        /// When overridden in the derived class, sets the item that will be selected by default.
        /// </summary>
        /// <param name="items">The items to choose from.</param>
        /// <returns>
        /// The item that will be selected by default.
        /// </returns>
        protected override string SetDefaultSelectedItem(IEnumerable<string> items)
        {
            if (_selected == null)
                return base.SetDefaultSelectedItem(items);

            if (_selected is string)
            {
                var stringComp = StringComparer.Ordinal;
                var asString = (string)_selected;
                return items.FirstOrDefault(x => stringComp.Equals(x, asString));
            }

            if (_selected is GameMessageCollection)
            {
                var asColl = (GameMessageCollection)_selected;
                return items.FirstOrDefault(x => x == asColl.Language);
            }

            return base.SetDefaultSelectedItem(items);
        }
    }
}