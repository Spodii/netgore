using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using NetGore.AI;

namespace NetGore.Editor.UI
{
    public class AIIDUITypeEditorForm : UITypeEditorListForm<AIID>
    {
        readonly object _selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="AIIDUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="selected">The currently selected AI.
        /// Multiple different types are supported. Can be null.</param>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "UITypeEditor")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AIFactory")]
        public AIIDUITypeEditorForm(object selected)
        {
            _selected = selected;

            if (!EnsureAIFactorySet())
                MessageBox.Show("Could not load the AI UITypeEditor form - AIFactory is not set.");
        }

        /// <summary>
        /// Gets or sets the <see cref="IAIFactory"/> used to display get the item to display in this form. Needs
        /// to be set before using the <see cref="AIIDUITypeEditorForm"/>.
        /// </summary>
        public static IAIFactory AIFactory { get; set; }

        /// <summary>
        /// Ensures the <see cref="AIIDUITypeEditorForm.AIFactory"/> is set.
        /// </summary>
        /// <returns>If false, the form will be unloaded.</returns>
        bool EnsureAIFactorySet()
        {
            if (AIFactory == null)
            {
                DialogResult = DialogResult.Abort;
                Close();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the string to display for an item.
        /// </summary>
        /// <param name="item">The item to get the display string for.</param>
        /// <returns>The string to display for the <paramref name="item"/>.</returns>
        protected override string GetItemDisplayString(AIID item)
        {
            return AIFactory.GetAIName(item);
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected override IEnumerable<AIID> GetListItems()
        {
            if (!EnsureAIFactorySet())
                return null;

            return AIFactory.AIs.Select(x => x.Key).OrderBy(x => x);
        }

        /// <summary>
        /// When overridden in the derived class, sets the item that will be selected by default.
        /// </summary>
        /// <param name="items">The items to choose from.</param>
        /// <returns>
        /// The item that will be selected by default.
        /// </returns>
        protected override AIID SetDefaultSelectedItem(IEnumerable<AIID> items)
        {
            if (_selected == null)
                return base.SetDefaultSelectedItem(items);

            if (_selected is string)
            {
                var stringComp = StringComparer.Ordinal;
                var asString = (string)_selected;
                return items.FirstOrDefault(x => stringComp.Equals(x, asString));
            }

            if (_selected is AIID)
            {
                var asID = (AIID)_selected;
                return items.FirstOrDefault(x => x == asID);
            }

            if (_selected is IAI)
            {
                var asAI = (IAI)_selected;
                return items.FirstOrDefault(x => x == asAI.ID);
            }

            return base.SetDefaultSelectedItem(items);
        }
    }
}