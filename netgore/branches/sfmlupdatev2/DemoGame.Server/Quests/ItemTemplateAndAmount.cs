using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using log4net;

namespace DemoGame.Server.Quests
{
    [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    public struct QuestItemTemplateAmount
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly ItemTemplateManager _itemTemplateManager = ItemTemplateManager.Instance;

        readonly byte _amount;
        readonly IItemTemplateTable _itemTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestItemTemplateAmount"/> struct.
        /// </summary>
        /// <param name="itemTemplate">The item template.</param>
        /// <param name="amount">The amount.</param>
        public QuestItemTemplateAmount(IItemTemplateTable itemTemplate, byte amount)
        {
            _itemTemplate = itemTemplate;
            _amount = amount;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestItemTemplateAmount"/> struct.
        /// </summary>
        /// <param name="itemTemplateID">The item template ID.</param>
        /// <param name="amount">The amount.</param>
        public QuestItemTemplateAmount(ItemTemplateID itemTemplateID, byte amount)
            : this(_itemTemplateManager[itemTemplateID], amount)
        {
        }

        /// <summary>
        /// Gets the amount of items.
        /// </summary>
        public byte Amount
        {
            get { return _amount; }
        }

        /// <summary>
        /// Gets the item template.
        /// </summary>
        public IItemTemplateTable ItemTemplate
        {
            get { return _itemTemplate; }
        }

        /// <summary>
        /// Checks if this <see cref="QuestItemTemplateAmount"/> contains valid values for the amount and item template.
        /// The amount must be greater than 0 and the item template must not be null.
        /// </summary>
        /// <returns>True if the values are valid; otherwise false.</returns>
        public bool AssertHasValidValues()
        {
            // Check for valid template
            if (ItemTemplate == null)
            {
                const string errmsg =
                    "Quest item `{0}` requirement has a null item template. Item will not be used in the requirements.";

                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));

                return false;
            }

            // Check for valid amount
            if (Amount <= 0)
            {
                const string errmsg =
                    "Invalid item amount ({0}) specified for quest requirement on item template `{1}`. Item will not be used in the requirements.";

                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, Amount, ItemTemplate);
                Debug.Fail(string.Format(errmsg, Amount, ItemTemplate));

                return false;
            }

            return true;
        }
    }
}