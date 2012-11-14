using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using DemoGame.Server.Quests;
using NetGore;
using NetGore.Db;
using NetGore.Editor;
using NetGore.Features.Quests;
using NetGore.IO;

namespace DemoGame.Editor
{
    /// <summary>
    /// A <see cref="Quest"/> that is to be used in editors in a <see cref="PropertyGrid"/>.
    /// </summary>
    public class EditorQuest : IQuestTable
    {
        const string _categoryQuest = "Quest";
        const string _categoryQuestFinishReqs = "Quest Finish Requirements";
        const string _categoryQuestRewards = "Quest Rewards";
        const string _categoryQuestStartReqs = "Quest Start Requirements";
        static readonly IQuestDescriptionCollection _questDescriptions = QuestDescriptionCollection.Create(ContentPaths.Dev);

        readonly QuestID _id;

        List<MutablePair<ItemTemplateID, byte>> _finishItems;
        List<QuestID> _finishQuests;
        List<MutablePair<CharacterTemplateID, ushort>> _kills;
        List<MutablePair<ItemTemplateID, byte>> _rewardItems;
        List<MutablePair<ItemTemplateID, byte>> _startItems;
        List<QuestID> _startQuests;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorQuest"/> class.
        /// </summary>
        /// <param name="questID">The quest ID.</param>
        /// <param name="dbController">The <see cref="IDbController"/>.</param>
        /// <exception cref="ArgumentException">No quests for the given <paramref name="questID"/> exist.</exception>
        public EditorQuest(QuestID questID, IDbController dbController)
        {
            _id = questID;

            // Get the quest database table row
            var questTable = dbController.GetQuery<SelectQuestQuery>().Execute(questID);
            if (questTable == null)
            {
                const string errmsg = "Invalid QuestID ({0}) supplied - no quests with this ID exists.";
                throw new ArgumentException(string.Format(errmsg, questID), "questID");
            }

            Debug.Assert(questID == questTable.ID);

            // Get the quest description stuff from the client-side
            var qd = _questDescriptions[ID];
            if (qd != null)
            {
                Name = qd.Name;
                Description = qd.Description;
            }
            else
            {
                Name = string.Empty;
                Description = string.Empty;
            }

            // Get the quest details from the server side
            RewardExp = questTable.RewardExp;
            RewardCash = questTable.RewardCash;
            Repeatable = questTable.Repeatable;

            _startItems =
                dbController.GetQuery<SelectQuestRequireStartItemQuery>().Execute(questID).Select(
                    x => new MutablePair<ItemTemplateID, byte>(x.ItemTemplateID, x.Amount)).ToList();
            _finishItems =
                dbController.GetQuery<SelectQuestRequireFinishItemQuery>().Execute(questID).Select(
                    x => new MutablePair<ItemTemplateID, byte>(x.ItemTemplateID, x.Amount)).ToList();

            _startQuests = dbController.GetQuery<SelectQuestRequireStartCompleteQuestsQuery>().Execute(questID).ToList();
            _finishQuests = dbController.GetQuery<SelectQuestRequireFinishCompleteQuestsQuery>().Execute(questID).ToList();

            _rewardItems =
                dbController.GetQuery<SelectQuestRewardItemQuery>().Execute(questID).Select(
                    x => new MutablePair<ItemTemplateID, byte>(x.ItemTemplateID, x.Amount)).ToList();

            _kills =
                dbController.GetQuery<SelectQuestRequireKillQuery>().Execute(questID).Select(
                    x => new MutablePair<CharacterTemplateID, ushort>(x.CharacterTemplateID, x.Amount)).ToList();
        }

        /// <summary>
        /// Gets or sets the description of the quest.
        /// </summary>
        [Browsable(true)]
        [Description("The long description of the quest.")]
        [Category(_categoryQuest)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the items required to finish the quest.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Browsable(true)]
        [Description("The items required to finish the quest.")]
        [Category(_categoryQuestFinishReqs)]
        public List<MutablePair<ItemTemplateID, byte>> FinishItems
        {
            get
            {
                _finishItems.RemoveDuplicates((x, y) => x.Key == y.Key);
                return _finishItems;
            }
            set
            {
                _finishItems = value ?? new List<MutablePair<ItemTemplateID, byte>>();
                _finishItems.RemoveDuplicates((x, y) => x.Key == y.Key);
            }
        }

        /// <summary>
        /// Gets or sets the quests required to be completed to be able to finish the quest.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Browsable(true)]
        [Description("The quests required to be completed to be able to finish the quest.")]
        [Category(_categoryQuestFinishReqs)]
        public List<QuestID> FinishQuests
        {
            get
            {
                _finishQuests.RemoveDuplicates((x, y) => x == y);
                return _finishQuests;
            }
            set
            {
                _finishQuests = value ?? new List<QuestID>();
                _finishQuests.RemoveDuplicates((x, y) => x == y);
            }
        }

        /// <summary>
        /// Gets or sets the characters required to kill, and the amount to kill, to finish the quest.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Browsable(true)]
        [Description("The characters required to kill, and the amount to kill, to finish the quest.")]
        [Category(_categoryQuestFinishReqs)]
        public List<MutablePair<CharacterTemplateID, ushort>> Kills
        {
            get
            {
                _kills.RemoveDuplicates((x, y) => x.Key == y.Key);
                return _kills;
            }
            set
            {
                _kills = value ?? new List<MutablePair<CharacterTemplateID, ushort>>();
                _kills.RemoveDuplicates((x, y) => x.Key == y.Key);
            }
        }

        /// <summary>
        /// Gets or sets the name of the quest.
        /// </summary>
        [Browsable(true)]
        [Description("The short name of the quest. Doesn't need to be unique, but is usually a good idea.")]
        [Category(_categoryQuest)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the item templates and amounts given as a reward for finishing this quest.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Browsable(true)]
        [Description("The item templates and amounts given as a reward for finishing this quest.")]
        [Category(_categoryQuestRewards)]
        public List<MutablePair<ItemTemplateID, byte>> RewardItems
        {
            get
            {
                _rewardItems.RemoveDuplicates((x, y) => x.Key == y.Key);
                return _rewardItems;
            }
            set
            {
                _rewardItems = value ?? new List<MutablePair<ItemTemplateID, byte>>();
                _rewardItems.RemoveDuplicates((x, y) => x.Key == y.Key);
            }
        }

        /// <summary>
        /// Gets or sets the item templates and amounts required to start this quest.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Browsable(true)]
        [Description("The item templates and amounts required to start this quest.")]
        [Category(_categoryQuestStartReqs)]
        public List<MutablePair<ItemTemplateID, byte>> StartItems
        {
            get
            {
                _startItems.RemoveDuplicates((x, y) => x.Key == y.Key);
                return _startItems;
            }
            set
            {
                _startItems = value ?? new List<MutablePair<ItemTemplateID, byte>>();
                _startItems.RemoveDuplicates((x, y) => x.Key == y.Key);
            }
        }

        /// <summary>
        /// Gets or sets the quests that must be finished before starting this quest.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Browsable(true)]
        [Description("The quests that must be finished before starting this quest.")]
        [Category(_categoryQuestStartReqs)]
        public List<QuestID> StartQuests
        {
            get
            {
                _startQuests.RemoveDuplicates((x, y) => x == y);
                return _startQuests;
            }
            set
            {
                _startQuests = value ?? new List<QuestID>();
                _startQuests.RemoveDuplicates((x, y) => x == y);
            }
        }

        #region IQuestTable Members

        /// <summary>
        /// Gets the unique ID of the quest.
        /// </summary>
        [Browsable(true)]
        [Description("The unique ID of the quest.")]
        [Category(_categoryQuest)]
        public QuestID ID
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets or sets if this quest can be repeated.
        /// </summary>
        [Browsable(true)]
        [Description("If the quest can be repeated by someone multiple times. If false, this quest can only be done once.")]
        [Category(_categoryQuest)]
        public bool Repeatable { get; set; }

        /// <summary>
        /// Gets or sets the amount of cash given as a reward for finishing this quest.
        /// </summary>
        [Browsable(true)]
        [Description("The amount of cash given as a reward for finishing this quest.")]
        [Category(_categoryQuestRewards)]
        public int RewardCash { get; set; }

        /// <summary>
        /// Gets or sets the amount of experience given as a reward for finishing this quest.
        /// </summary>
        [Browsable(true)]
        [Description("The amount of experience given as a reward for finishing this quest.")]
        [Category(_categoryQuestRewards)]
        public int RewardExp { get; set; }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IQuestTable IQuestTable.DeepCopy()
        {
            return new QuestTable(this);
        }

        #endregion
    }
}