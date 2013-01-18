using System.Linq;
using DemoGame.Server.Queries;
using NetGore.Db;
using NetGore.Features.Quests;

namespace DemoGame.Server.Quests
{
    /// <summary>
    /// Manages all of the quests.
    /// </summary>
    public class QuestManager : QuestCollection<User>
    {
        static readonly QuestManager _instance;

        readonly IDbController _dbController;

        /// <summary>
        /// Initializes the <see cref="QuestManager"/> class.
        /// </summary>
        static QuestManager()
        {
            var qmInstance = new QuestManager(DbControllerBase.GetInstance());
            _instance = qmInstance;
            foreach (var quest in _instance)
            {
                _instance.LoadQuest(quest.QuestID);
                _instance.Reload(quest.QuestID);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestManager"/> class.
        /// </summary>
        /// <param name="dbController">The <see cref="IDbController"/>.</param>
        QuestManager(IDbController dbController)
        {
            _dbController = dbController;

            LoadQuests(dbController.GetQuery<SelectQuestIDsQuery>().Execute());
        }

        /// <summary>
        /// Gets the <see cref="QuestManager"/> instance.
        /// </summary>
        public static QuestManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// When overridden in the derived class, loads the quest with the specified <see cref="QuestID"/>.
        /// </summary>
        /// <param name="questID">The ID of the quest to load.</param>
        /// <returns>The <see cref="IQuest{TCharacter}"/> for the <paramref name="questID"/>.</returns>
        protected override IQuest<User> LoadQuest(QuestID questID)
        {
            return new Quest(questID, _dbController);
        }
    }
}