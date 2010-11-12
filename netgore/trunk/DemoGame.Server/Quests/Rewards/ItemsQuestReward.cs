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
    public class ItemsQuestReward : IQuestReward<User>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IEnumerable<QuestItemTemplateAmount> _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsQuestReward"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <exception cref="ArgumentNullException"><paramref name="items"/> is null or empty.</exception>
        public ItemsQuestReward(IEnumerable<QuestItemTemplateAmount> items)
        {
            if (items == null || items.IsEmpty())
                throw new ArgumentNullException("items");

            // Store the valid items
            _items = items.Where(x => x.AssertHasValidValues()).OrderBy(x => x.ItemTemplate.ID).ToCompact();
        }

        /// <summary>
        /// Gets the items given as a quest reward.
        /// </summary>
        public IEnumerable<QuestItemTemplateAmount> Items
        {
            get { return _items; }
        }

        #region IQuestReward<User> Members

        /// <summary>
        /// Checks if the <paramref name="character"/> is able to receive this quest reward.
        /// </summary>
        /// <param name="character">The character to check if able to receive this quest reward.</param>
        /// <returns>True if the <paramref name="character"/> can receive this quest reward; otherwise false.</returns>
        public bool CanGive(User character)
        {
            // FUTURE: Right now, this check just checks for one free spot per item. Ideally, it'd take into consideration stacking.

            return Items.Count() <= character.Inventory.FreeSlots;
        }

        /// <summary>
        /// Gives the quest reward to the <paramref name="character"/>.
        /// </summary>
        /// <param name="character">The character to give the quest reward to.</param>
        public void Give(User character)
        {
            // Loop through each item to add
            foreach (var item in Items)
            {
                // Create the new item instance from the template
                var addItem = new ItemEntity(item.ItemTemplate, item.Amount);

                // Add to the character's inventory
                var remaining = character.TryGiveItem(addItem);

                // Ensure it was all added
                if (remaining != null)
                {
                    const string errmsg =
                        "Failed to add all of the quest reward item `{0}` to `{1}`'s inventory. Only {2} of {3} of the items were added.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, addItem, character, item.Amount - remaining.Amount, item.Amount);
                    Debug.Fail(string.Format(errmsg, addItem, character, item.Amount - remaining.Amount, item.Amount));

                    // Throw any remainder on the ground (not much else we can do with it)
                    character.DropItem(remaining);
                }
            }
        }

        #endregion
    }
}