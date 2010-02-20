using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoGame.DbObjs;
using DemoGame.Server.Queries;
using NetGore;
using NetGore.Db;
using NetGore.Features.Quests;

namespace DemoGame.Server.Quests
{
    public class Quest : IQuest<User>
    {
        readonly QuestID _questID;
        readonly bool _repeatable;
        readonly IQuestRewardCollection<User> _rewards;
        readonly IQuestRequirementCollection<User> _startRequirements;
        readonly IQuestRequirementCollection<User> _finishRequirements;

        public Quest(QuestID questID, IDbController dbController)
        {
            _questID = questID;

            var info = dbController.GetQuery<SelectQuestQuery>().Execute(questID);
            _repeatable = info.Repeatable;

            _rewards = LoadRewards(questID, info, dbController);
            _startRequirements = LoadStartRequirements(questID, dbController);
            _finishRequirements = LoadFinishRequirements(questID, dbController);
        }

        static IQuestRewardCollection<User> LoadRewards(QuestID questID, IQuestTable table, IDbController dbController)
        {
            var l = new List<IQuestReward<User>>
            { 
                new MoneyQuestReward(table.RewardCash), 
                new ExpQuestReward(table.RewardExp) 
            };

            var rewardItems = dbController.GetQuery<SelectQuestRewardItemQuery>().Execute(questID);
            if (!rewardItems.IsEmpty())
                l.Add(new ItemsQuestReward(rewardItems.Select(x => new ItemTemplateAndAmount(x.ItemTemplateID,x.Amount))));

            return new QuestRewardCollection<User>(l);
        }

        static IQuestRequirementCollection<User> LoadStartRequirements(QuestID questID, IDbController dbController)
        {
            var l = new List<IQuestRequirement<User>>();

            var reqItems = dbController.GetQuery<SelectQuestRequireStartItemQuery>().Execute(questID);
            if (!reqItems.IsEmpty())
                l.Add(new ItemsQuestRequirement(reqItems.Select(x => new ItemTemplateAndAmount(x.ItemTemplateID, x.Amount))));

            // TODO: !! Add: Starting requirement quests

            return new QuestRequirementCollection<User>(l);
        }

        static IQuestRequirementCollection<User> LoadFinishRequirements(QuestID questID, IDbController dbController)
        {
            var l = new List<IQuestRequirement<User>>();

            var reqItems = dbController.GetQuery<SelectQuestRequireFinishItemQuery>().Execute(questID);
            if (!reqItems.IsEmpty())
                l.Add(new ItemsQuestRequirement(reqItems.Select(x => new ItemTemplateAndAmount(x.ItemTemplateID, x.Amount))));

            // TODO: !! Add: Finish kill requirements

            return new QuestRequirementCollection<User>(l);
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

        /// <summary>
        /// Gets the requirements for finishing this quest.
        /// </summary>
        public IQuestRequirementCollection<User> FinishRequirements
        {
            get { return _finishRequirements; }
        }
    }
}
