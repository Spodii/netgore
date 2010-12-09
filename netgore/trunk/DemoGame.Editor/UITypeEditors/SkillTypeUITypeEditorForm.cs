using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NetGore;
using NetGore.Editor.UI;

namespace DemoGame.Editor.UITypeEditors
{
    /// <summary>
    /// A <see cref="Form"/> for listing the <see cref="SkillType"/> information.
    /// </summary>
    public class SkillTypeUITypeEditorForm : UITypeEditorListForm<SkillType>
    {
        readonly object _selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillTypeUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="selected">The default selected item.</param>
        public SkillTypeUITypeEditorForm(object selected)
        {
            _selected = selected;
        }

        /// <summary>
        /// Gets the string to display for an item.
        /// </summary>
        /// <param name="item">The item to get the display string for.</param>
        /// <returns>The string to display for the <paramref name="item"/>.</returns>
        protected override string GetItemDisplayString(SkillType item)
        {
            return item.GetValue() + ". " + item;
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected override IEnumerable<SkillType> GetListItems()
        {
            return EnumHelper<SkillType>.Values;
        }

        /// <summary>
        /// When overridden in the derived class, sets the item that will be selected by default.
        /// </summary>
        /// <param name="items">The items to choose from.</param>
        /// <returns>
        /// The item that will be selected by default.
        /// </returns>
        protected override SkillType SetDefaultSelectedItem(IEnumerable<SkillType> items)
        {
            if (_selected == null)
                return base.SetDefaultSelectedItem(items);

            if (_selected is string)
            {
                var stringComp = StringComparer.Ordinal;
                var asString = (string)_selected;
                return items.FirstOrDefault(x => stringComp.Equals(x, asString));
            }

            if (_selected is SkillType)
            {
                var asID = (SkillType)_selected;
                return items.FirstOrDefault(x => x == asID);
            }

            if (_selected is SkillType?)
            {
                var asID = (SkillType?)_selected;
                if (!asID.HasValue)
                    return base.SetDefaultSelectedItem(items);
                else
                    return items.FirstOrDefault(x => x == asID.Value);
            }

            if (_selected is int)
            {
                var asInt = (int)_selected;
                return items.FirstOrDefault(x => (int)x == asInt);
            }

            return base.SetDefaultSelectedItem(items);
        }
    }
}