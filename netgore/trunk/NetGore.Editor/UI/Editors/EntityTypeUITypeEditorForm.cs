using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.World;

namespace NetGore.Editor.UI
{
    public partial class EntityTypeUITypeEditorForm : UITypeEditorListForm<Type>
    {
        readonly Type _defaultSelected;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityTypeUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="defaultSelected">The default <see cref="Type"/> to select.</param>
        public EntityTypeUITypeEditorForm(Type defaultSelected = null)
        {
            _defaultSelected = defaultSelected;

            InitializeComponent();
        }

        /// <summary>
        /// Gets the string to display for an item.
        /// </summary>
        /// <param name="item">The item to get the display string for.</param>
        /// <returns>The string to display for the <paramref name="item"/>.</returns>
        protected override string GetItemDisplayString(Type item)
        {
            return item.Name;
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected override IEnumerable<Type> GetListItems()
        {
            return MapFileEntityAttribute.GetTypes().OrderBy(x => x.Name);
        }

        /// <summary>
        /// When overridden in the derived class, sets the item that will be selected by default.
        /// </summary>
        /// <param name="items">The items to choose from.</param>
        /// <returns>
        /// The item that will be selected by default.
        /// </returns>
        protected override Type SetDefaultSelectedItem(IEnumerable<Type> items)
        {
            return _defaultSelected;
        }
    }
}