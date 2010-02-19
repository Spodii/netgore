using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Features.Quests;

namespace DemoGame.Server.Quests
{
    public class ItemsQuestRequirement : IQuestRequirement<User>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IEnumerable<ItemTemplateAndAmount> _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsQuestRequirement"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <exception cref="ArgumentNullException"><paramref name="items"/> is null or empty.</exception>
        public ItemsQuestRequirement(IEnumerable<ItemTemplateAndAmount> items)
        {
            if (items == null || items.IsEmpty())
                throw new ArgumentNullException("items");

            // Store the valid items
            _items = items.Where(x => x.AssertHasValidValues()).OrderBy(x => x.ItemTemplate.ID).ToCompact();
        }

        /// <summary>
        /// Gets the item templates and amounts required for the quest.
        /// </summary>
        public IEnumerable<ItemTemplateAndAmount> Items
        {
            get { return _items; }
        }

        #region IQuestRequirement<User> Members

        /// <summary>
        /// Checks if the <paramref name="character"/> meets this test requirement.
        /// </summary>
        /// <param name="character">The character to check if they meet the requirements.</param>
        /// <returns>True if the <paramref name="character"/> meets the requirements defined by this
        /// <see cref="IQuestRequirement{TCharacter}"/>; otherwise false.</returns>
        public bool HasRequirements(User character)
        {
            // Check each of the required items
            foreach (var item in Items)
            {
                var id = item.ItemTemplate.ID;

                // Count how many items the user has of this item template in their whole inventory
                int count = character.Inventory.Where(x => x.Value.ItemTemplateID == id).Sum(x => x.Value.Amount);

                // If they have less than the required amount, they don't meet all reuqirements
                if (count < item.Amount)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Takes the quest requirements from the <paramref name="character"/>, if applicable. Not required,
        /// and only applies for when turning in a quest and not starting a quest.
        /// </summary>
        /// <param name="character">The <paramref name="character"/> to take the requirements from.</param>
        public void TakeRequirements(User character)
        {
            // Loop through all the required items
            foreach (var item in Items)
            {
                var id = item.ItemTemplate.ID;

                // Keep track of how many left to remain
                int remaining = item.Amount;

                // Keep removing items until either the character runs out of the items in their inventory, or
                // we have taken the required amount
                foreach (var invItem in character.Inventory.Where(x => x.Value.ItemTemplateID == id))
                {
                    var invAmount = invItem.Value.Amount;
                    if (remaining >= invAmount)
                    {
                        // Remove all of the item from the inventory
                        remaining -= invAmount;
                        invItem.Value.Dispose();
                    }
                    else
                    {
                        // Remove only some of the item from the inventory
                        invItem.Value.Amount -= invAmount;
                        remaining -= invAmount;

                        Debug.Assert(remaining == 0);
                        break;
                    }
                }

                // Check that we got it all
                if (remaining > 0)
                {
                    const string errmsg =
                        "Unable to remove all of the quest item `{0}` from `{1}`'s inventory. {2} of {3} items were removed.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, item.ItemTemplate, character, item.Amount - remaining, item.Amount);
                    Debug.Fail(string.Format(errmsg, item.ItemTemplate, character, item.Amount - remaining, item.Amount));
                }
            }
        }

        #endregion
    }
}