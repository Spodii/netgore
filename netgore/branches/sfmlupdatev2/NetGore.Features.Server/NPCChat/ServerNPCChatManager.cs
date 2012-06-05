using System.Linq;
using NetGore.IO;

namespace NetGore.Features.NPCChat
{
    /// <summary>
    /// Provides a means for accessing the NPC chat dialogs.
    /// </summary>
    public class ServerNPCChatManager : NPCChatManagerBase
    {
        /// <summary>
        /// The NPCChatManagerBase implementation instance.
        /// </summary>
        static readonly ServerNPCChatManager _instance;

        /// <summary>
        /// Initializes the <see cref="ServerNPCChatManager"/> class.
        /// </summary>
        static ServerNPCChatManager()
        {
            _instance = new ServerNPCChatManager();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerNPCChatManager"/> class.
        /// </summary>
        ServerNPCChatManager() : base(true)
        {
        }

        /// <summary>
        /// Gets the <see cref="ServerNPCChatManager"/> instance.
        /// </summary>
        public static ServerNPCChatManager Instance
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
            return new ServerNPCChatDialog(reader);
        }
    }
}