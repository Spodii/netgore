using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;

namespace NetGore.Features.Quests
{
    public class UserQuestInformation
    {
        /// <summary>
        /// Delegate for handling events from the <see cref="UserQuestInformation"/>.
        /// </summary>
        /// <param name="sender">The <see cref="UserQuestInformation"/> the event came from.</param>
        public delegate void UserQuestInformationEventHandler(UserQuestInformation sender);

        /// <summary>
        /// Delegate for handling events from the <see cref="UserQuestInformation"/>.
        /// </summary>
        /// <param name="sender">The <see cref="UserQuestInformation"/> the event came from.</param>
        /// <param name="quest">The <see cref="QuestID"/> for the quest related to the event.</param>
        public delegate void UserQuestInformationQuestEventHandler(UserQuestInformation sender, QuestID quest);

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly List<QuestID> _activeQuests = new List<QuestID>();
        readonly List<QuestID> _completedQuests = new List<QuestID>();

        /// <summary>
        /// Notifies listeners when a new quest has been added to the active quests list.
        /// </summary>
        public event UserQuestInformationQuestEventHandler ActiveQuestAdded;

        /// <summary>
        /// Notifies listeners when a quest has been removed from the active quests list.
        /// </summary>
        public event UserQuestInformationQuestEventHandler ActiveQuestRemoved;

        /// <summary>
        /// Notifies listeners when a new quest has been added to the completed quests list.
        /// </summary>
        public event UserQuestInformationQuestEventHandler CompletedQuestAdded;

        /// <summary>
        /// Notifies listeners when the initial quest values have been loaded.
        /// </summary>
        public event UserQuestInformationEventHandler Loaded;

        /// <summary>
        /// Gets the list of active quests.
        /// </summary>
        public IEnumerable<QuestID> ActiveQuests
        {
            get { return _activeQuests; }
        }

        /// <summary>
        /// Gets the list of completed quests.
        /// </summary>
        public IEnumerable<QuestID> CompletedQuests
        {
            get { return _completedQuests; }
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the
        /// <see cref="UserQuestInformation.ActiveQuestAdded"/> event.
        /// </summary>
        /// <param name="questID">The ID of the quest that was added.</param>
        protected virtual void OnActiveQuestAdded(QuestID questID)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the
        /// <see cref="UserQuestInformation.ActiveQuestRemoved"/> event.
        /// </summary>
        /// <param name="questID">The ID of the quest that was removed.</param>
        protected virtual void OnActiveQuestRemoved(QuestID questID)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the
        /// <see cref="UserQuestInformation.CompletedQuestAdded"/> event.
        /// </summary>
        /// <param name="questID">The ID of the quest that was added.</param>
        protected virtual void OnCompletedQuestAdded(QuestID questID)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the
        /// <see cref="UserQuestInformation.Loaded"/> event.
        /// </summary>
        protected virtual void OnLoaded()
        {
        }

        /// <summary>
        /// Reads the data from the server related to the user quest information. This should only be used by the client.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> containing the data.</param>
        public void Read(BitStream bs)
        {
            var id = bs.ReadEnum<QuestInfoMessages>();
            switch (id)
            {
                case QuestInfoMessages.AddCompletedQuest:
                    ReadAddCompletedQuest(bs);
                    break;

                case QuestInfoMessages.AddActiveQuest:
                    ReadAddActiveQuest(bs);
                    break;

                case QuestInfoMessages.RemoveActiveQuest:
                    ReadRemoveActiveQuest(bs);
                    break;

                case QuestInfoMessages.LoadInitialValues:
                    ReadLoadInitialValues(bs);
                    break;

                default:
                    const string errmsg = "Unknown QuestInfoMessages value `{0}`. Could not parse!";
                    string err = string.Format(errmsg, id);
                    log.Fatal(err);
                    Debug.Fail(err);
                    return;
            }
        }

        /// <summary>
        /// Handles <see cref="QuestInfoMessages.AddActiveQuest"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to read from.</param>
        void ReadAddActiveQuest(BitStream bs)
        {
            var questID = bs.ReadQuestID();

            // Ensure the quest isn't already in the active list
            if (_activeQuests.BinarySearch(questID) >= 0)
                return;

            // Add
            _activeQuests.Add(questID);
            _activeQuests.Sort();

            // Raise events
            OnActiveQuestAdded(questID);

            if (ActiveQuestAdded != null)
                ActiveQuestAdded(this, questID);
        }

        /// <summary>
        /// Handles <see cref="QuestInfoMessages.AddCompletedQuest"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to read from.</param>
        void ReadAddCompletedQuest(BitStream bs)
        {
            var questID = bs.ReadQuestID();

            // Ensure the quest isn't already in the completed list
            if (_completedQuests.BinarySearch(questID) >= 0)
                return;

            // Add
            _completedQuests.Add(questID);
            _completedQuests.Sort();

            // Raise events
            OnCompletedQuestAdded(questID);

            if (CompletedQuestAdded != null)
                CompletedQuestAdded(this, questID);
        }

        /// <summary>
        /// Handles <see cref="QuestInfoMessages.LoadInitialValues"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to read from.</param>
        void ReadLoadInitialValues(BitStream bs)
        {
            _completedQuests.Clear();
            _activeQuests.Clear();

            // Read completed quests
            byte count = bs.ReadByte();
            for (int i = 0; i < count; i++)
            {
                _completedQuests.Add(bs.ReadQuestID());
            }

            // Read Active quests
            count = bs.ReadByte();
            for (int i = 0; i < count; i++)
            {
                _activeQuests.Add(bs.ReadQuestID());
            }

            // Raise events
            OnLoaded();

            if (Loaded != null)
                Loaded(this);
        }

        /// <summary>
        /// Handles <see cref="QuestInfoMessages.RemoveActiveQuest"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to read from.</param>
        void ReadRemoveActiveQuest(BitStream bs)
        {
            var questID = bs.ReadQuestID();

            // Ensure the quest is in the active list
            int index;
            if ((index = _activeQuests.BinarySearch(questID)) < 0)
                return;

            Debug.Assert(_activeQuests[index] == questID);

            // Remove
            _activeQuests.RemoveAt(index);

            // Raise events
            OnActiveQuestRemoved(questID);

            if (ActiveQuestRemoved != null)
                ActiveQuestRemoved(this, questID);
        }

        /// <summary>
        /// Appends a message to a <see cref="BitStream"/> for synchronizing the client's quest information for when
        /// a quest is added to the active quests.
        /// The message is then read and handled by the receiver using <see cref="UserQuestInformation.Read"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to append the message to.</param>
        /// <param name="questID">The ID of the added active quest.</param>
        public static void WriteAddActiveQuest(BitStream bs, QuestID questID)
        {
            bs.WriteEnum(QuestInfoMessages.AddActiveQuest);
            bs.Write(questID);
        }

        /// <summary>
        /// Appends a message to a <see cref="BitStream"/> for synchronizing the client's quest information for when
        /// a quest is completed.
        /// The message is then read and handled by the receiver using <see cref="UserQuestInformation.Read"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to append the message to.</param>
        /// <param name="questID">The ID of the completed quest.</param>
        public static void WriteAddCompletedQuest(BitStream bs, QuestID questID)
        {
            bs.WriteEnum(QuestInfoMessages.AddCompletedQuest);
            bs.Write(questID);
        }

        /// <summary>
        /// Appends a message to a <see cref="BitStream"/> for synchronizing all of the client's quest information.
        /// The message is then read and handled by the receiver using <see cref="UserQuestInformation.Read"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to append the message to.</param>
        /// <param name="completedQuests">All of the completed quests.</param>
        /// <param name="activeQuests">All of the active quests.</param>
        public static void WriteQuestInfo(BitStream bs, IEnumerable<QuestID> completedQuests, IEnumerable<QuestID> activeQuests)
        {
            completedQuests = completedQuests.ToImmutable();
            activeQuests = activeQuests.ToImmutable();

            bs.WriteEnum(QuestInfoMessages.RemoveActiveQuest);

            // Write the completed quests
            bs.Write((ushort)completedQuests.Count());
            foreach (var q in completedQuests)
            {
                bs.Write(q);
            }

            // Write the active quests
            bs.Write((byte)activeQuests.Count());
            foreach (var q in activeQuests)
            {
                bs.Write(q);
            }
        }

        /// <summary>
        /// Appends a message to a <see cref="BitStream"/> for synchronizing the client's quest information for when
        /// a quest is removed from the active quests.
        /// The message is then read and handled by the receiver using <see cref="UserQuestInformation.Read"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to append the message to.</param>
        /// <param name="questID">The ID of the removed active quest.</param>
        public static void WriteRemoveActiveQuest(BitStream bs, QuestID questID)
        {
            bs.WriteEnum(QuestInfoMessages.RemoveActiveQuest);
            bs.Write(questID);
        }

        /// <summary>
        /// Enum of the different packet messages for this class.
        /// </summary>
        enum QuestInfoMessages
        {
            /// <summary>
            /// Adds a quest to the list of completed quests.
            /// </summary>
            AddCompletedQuest,

            /// <summary>
            /// Adds a quest to the list of active quests.
            /// </summary>
            AddActiveQuest,

            /// <summary>
            /// Removes a quest from the list of active quests.
            /// </summary>
            RemoveActiveQuest,

            /// <summary>
            /// Loads the initial quest status values.
            /// </summary>
            LoadInitialValues
        }
    }
}