using System.Linq;
using NetGore.IO;
using NetGore.NPCChat;

namespace DemoGame.Client.NPCChat
{
    /// <summary>
    /// Provides a means for accessing the NPC chat dialogs.
    /// </summary>
    public class NPCChatManager : NPCChatManagerBase
    {
        /// <summary>
        /// The NPCChatManagerBase implementation instance.
        /// </summary>
        static readonly NPCChatManager _instance;

        /// <summary>
        /// Initializes the <see cref="NPCChatManager"/> class.
        /// </summary>
        static NPCChatManager()
        {
            _instance = new NPCChatManager();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatManager"/> class.
        /// </summary>
        NPCChatManager() : base(true)
        {
        }

        /// <summary>
        /// Gets the <see cref="NPCChatManager"/> instance.
        /// </summary>
        public static NPCChatManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// When overridden in the derived class, creates a NPCChatDialogBase from the given IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <returns>A NPCChatDialogBase created from the given IValueReader.</returns>
        protected override NPCChatDialogBase CreateDialog(IValueReader reader)
        {
            return new NPCChatDialog(reader);
        }
    }
}