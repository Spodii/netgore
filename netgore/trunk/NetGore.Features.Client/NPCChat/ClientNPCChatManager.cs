using System.Linq;
using NetGore.IO;

namespace NetGore.Features.NPCChat
{
    /// <summary>
    /// Provides a means for accessing the NPC chat dialogs.
    /// </summary>
    public class ClientNPCChatManager : NPCChatManagerBase
    {
        /// <summary>
        /// The NPCChatManagerBase implementation instance.
        /// </summary>
        static readonly ClientNPCChatManager _instance;

        /// <summary>
        /// Initializes the <see cref="ClientNPCChatManager"/> class.
        /// </summary>
        static ClientNPCChatManager()
        {
            _instance = new ClientNPCChatManager();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientNPCChatManager"/> class.
        /// </summary>
        ClientNPCChatManager() : base(true)
        {
        }

        /// <summary>
        /// Gets the <see cref="ClientNPCChatManager"/> instance.
        /// </summary>
        public static ClientNPCChatManager Instance
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
            return new ClientNPCChatDialog(reader);
        }
    }
}