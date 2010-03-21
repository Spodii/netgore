using System.Linq;
using NetGore.IO;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Contains the description of a quest.
    /// </summary>
    public class QuestDescription : IQuestDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestDescription"/> class.
        /// </summary>
        public QuestDescription()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestDescription"/> class.
        /// </summary>
        /// <param name="r">The <see cref="IValueReader"/>.</param>
        public QuestDescription(IValueReader r)
        {
            ReadState(r);
        }

        #region IQuestDescription Members

        /// <summary>
        /// Gets or sets the quest's description.
        /// </summary>
        [SyncValue]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the quest.
        /// </summary>
        [SyncValue]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IQuestDescription.QuestID"/> for the quest that this description is for.
        /// </summary>
        [SyncValue]
        public QuestID QuestID { get; set; }

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);
        }

        #endregion
    }
}