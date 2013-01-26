using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Keeps track of what quests the user has the requirements for.
    /// </summary>
    public class HasQuestRequirementsTracker
    {
        readonly Action<QuestID> _sendRequest;
        readonly SortedList<QuestID, bool?> _statuses = new SortedList<QuestID, bool?>();

        /// <summary>
        /// Initializes a new instance of the <see cref="HasQuestRequirementsTracker"/> class.
        /// </summary>
        /// <param name="sendRequest">The <see cref="Action{QuestID}"/> used to send a request to the server for whether
        /// or not we have the requirements for a given <see cref="QuestID"/>.</param>
        public HasQuestRequirementsTracker(Action<QuestID> sendRequest)
        {
            _sendRequest = sendRequest;
        }

        /// <summary>
        /// Clears out the cached quest requirement statuses.
        /// </summary>
        public void Clear()
        {
            _statuses.Clear();
        }

        /// <summary>
        /// Gets if the user has the requirements for the given quest.
        /// </summary>
        /// <param name="quest">The ID of the quest to check for the requirements of.</param>
        /// <returns>True if the user has the requirements for the given <paramref name="quest"/>, false if they
        /// do not have the requirements, or null if the status is currently unknown.</returns>
        public bool? HasRequirements(QuestID quest)
        {
            bool? value;

            // Update the quest status before checking it's "current" value
            Update(quest);

            // Try to get the cached value
            if (!_statuses.TryGetValue(quest, out value))
            {
                // Value was not in the cache, so make the request, and return null for now
                value = null;
                Prepare(quest);
            }

            return value;
        }

        /// <summary>
        /// Starts getting the status for the given <paramref name="quests"/> if the value is not already acquired.
        /// </summary>
        /// <param name="quests">The quests to get the status for.</param>
        public void Prepare(IEnumerable<QuestID> quests)
        {
            foreach (var q in quests)
            {
                Prepare(q);
            }
        }

        /// <summary>
        /// Starts getting the status for the given <paramref name="quest"/> if the value is not already acquired.
        /// </summary>
        /// <param name="quest">The quest to get the status for.</param>
        public void Prepare(QuestID quest)
        {
            if (_statuses.ContainsKey(quest))
                return;

            _statuses.Add(quest, null);

            _sendRequest(quest);
        }

        /// <summary>
        /// Sets whether or not we have the requirements for a quest.
        /// </summary>
        /// <param name="questID">The ID of the quest.</param>
        /// <param name="hasRequirements">True if we have the requirements for the quest; false if we do not.</param>
        public void SetRequirementsStatus(QuestID questID, bool hasRequirements)
        {
            if (!_statuses.ContainsKey(questID))
                _statuses.Add(questID, hasRequirements);
            else
                _statuses[questID] = hasRequirements;
        }

        /// <summary>
        /// Updates the status for the given <paramref name="quest"/>.
        /// </summary>
        /// <param name="quest">The quest to get the status for.</param>
        public void Update(QuestID quest)
        {
            _sendRequest(quest);
        }
    }
}