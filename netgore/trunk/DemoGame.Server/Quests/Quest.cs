using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.Queries;
using NetGore;
using NetGore.Db;
using NetGore.Features.Quests;

namespace DemoGame.Server.Quests
{
    public class Quest : IQuest<User>
    {
        readonly IQuestRequirementCollection<User> _finishRequirements;
        readonly QuestID _questID;
        readonly bool _repeatable;
        readonly IQuestRewardCollection<User> _rewards;
        readonly IQuestRequirementCollection<User> _startRequirements;

        /// <summary>
        /// Initializes a new instance of the <see cref="Quest"/> class.
        /// </summary>
        /// <param name="questID">The quest's ID.</param>
        /// <param name="dbController">The <see cref="IDbController"/> to use to load the values.</param>
        public Quest(QuestID questID, IDbController dbController)
        {
            _questID = questID;

            var info = dbController.GetQuery<SelectQuestQuery>().Execute(questID);
            _repeatable = info.Repeatable;

            _rewards = LoadRewards(questID, info, dbController);
            _startRequirements = LoadStartRequirements(dbController);
            _finishRequirements = LoadFinishRequirements(dbController);
        }

        /// <summary>
        /// Loads the requirements for finishing a quest.
        /// </summary>
        /// <param name="dbController">The <see cref="IDbController"/> to use to load values.</param>
        /// <returns>The requirements for finishing a quest.</returns>
        IQuestRequirementCollection<User> LoadFinishRequirements(IDbController dbController)
        {
            var questManager = QuestManager.Instance;
            var l = new List<IQuestRequirement<User>>();

            // Items
            var reqItems = dbController.GetQuery<SelectQuestRequireFinishItemQuery>().Execute(QuestID);
            Debug.Assert(reqItems.All(x => x.QuestID == QuestID));
            if (!reqItems.IsEmpty())
                l.Add(new ItemsQuestRequirement(this,
                    reqItems.Select(x => new QuestItemTemplateAmount(x.ItemTemplateID, x.Amount))));

            // Kills
            var reqKills = dbController.GetQuery<SelectQuestRequireKillQuery>().Execute(QuestID);
            Debug.Assert(reqItems.All(x => x.QuestID == QuestID));
            if (!reqKills.IsEmpty())
            {
                l.Add(new KillQuestRequirement(this,
                    reqKills.Select(x => new KeyValuePair<CharacterTemplateID, ushort>(x.CharacterTemplateID, x.Amount))));
            }

            // Complete quests
            var reqCompleteQuests = dbController.GetQuery<SelectQuestRequireFinishCompleteQuestsQuery>().Execute(QuestID);
            if (!reqCompleteQuests.IsEmpty())
                if (!questManager.IsEmpty())
                    l.Add(new CompleteQuestQuestRequirement(this, reqCompleteQuests.Select(questManager.GetQuest)));

            return new QuestRequirementCollection<User>(l);
        }

        /// <summary>
        /// Loads the rewards for finishing a quest.
        /// </summary>
        /// <param name="questID">The ID of the quest.</param>
        /// <param name="table">The <see cref="IQuestTable"/>.</param>
        /// <param name="dbController">The <see cref="IDbController"/> to use to load values.</param>
        /// <returns>The rewards for finishing a quest.</returns>
        static IQuestRewardCollection<User> LoadRewards(QuestID questID, IQuestTable table, IDbController dbController)
        {
            var l = new List<IQuestReward<User>> { new MoneyQuestReward(table.RewardCash), new ExpQuestReward(table.RewardExp) };

            // Items
            var rewardItems = dbController.GetQuery<SelectQuestRewardItemQuery>().Execute(questID);
            if (!rewardItems.IsEmpty())
                l.Add(new ItemsQuestReward(rewardItems.Select(x => new QuestItemTemplateAmount(x.ItemTemplateID, x.Amount))));

            return new QuestRewardCollection<User>(l);
        }

        /// <summary>
        /// Loads the requirements for starting a quest.
        /// </summary>
        /// <param name="dbController">The <see cref="IDbController"/> to use to load values.</param>
        /// <returns>The requirements for starting a quest.</returns>
        IQuestRequirementCollection<User> LoadStartRequirements(IDbController dbController)
        {
            var questManager = QuestManager.Instance;
            var l = new List<IQuestRequirement<User>>();

            // Items
            var reqItems = dbController.GetQuery<SelectQuestRequireStartItemQuery>().Execute(QuestID);
            if (!reqItems.IsEmpty())
                l.Add(new ItemsQuestRequirement(this,
                    reqItems.Select(x => new QuestItemTemplateAmount(x.ItemTemplateID, x.Amount))));

            // Complete quests
            var reqCompleteQuests = dbController.GetQuery<SelectQuestRequireStartCompleteQuestsQuery>().Execute(QuestID);
            if (!reqCompleteQuests.IsEmpty())
                if (!questManager.IsEmpty())
                    l.Add(new CompleteQuestQuestRequirement(this, reqCompleteQuests.Select(questManager.GetQuest)));

            return new QuestRequirementCollection<User>(l);
        }

        #region IQuest<User> Members

        /// <summary>
        /// Gets the requirements for finishing this quest.
        /// </summary>
        public IQuestRequirementCollection<User> FinishRequirements
        {
            get { return _finishRequirements; }
        }

        /// <summary>
        /// Gets the unique ID of the quest.
        /// </summary>
        public QuestID QuestID
        {
            get { return _questID; }
        }

        /// <summary>
        /// Gets if this quest can be repeated.
        /// </summary>
        public bool Repeatable
        {
            get { return _repeatable; }
        }

        /// <summary>
        /// Gets the rewards for completing this quest.
        /// </summary>
        public IQuestRewardCollection<User> Rewards
        {
            get { return _rewards; }
        }

        /// <summary>
        /// Gets the requirements for starting this quest.
        /// </summary>
        public IQuestRequirementCollection<User> StartRequirements
        {
            get { return _startRequirements; }
        }

        #endregion
    }
}