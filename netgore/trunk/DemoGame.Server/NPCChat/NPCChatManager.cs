using System.Collections.Generic;
using System.Linq;
using NetGore.IO;
using NetGore.NPCChat;

namespace DemoGame.Server.NPCChat
{
    /// <summary>
    /// Provides a means for accessing the NPC chat dialogs.
    /// </summary>
    public static class NPCChatManager
    {
        /// <summary>
        /// The NPCChatManagerBase implementation instance.
        /// </summary>
        static readonly NPCChatManagerBase _instance = new ManagerImplementation();

        /// <summary>
        /// Gets an IEnumerable of all the NPC chat dialogs in this manager.
        /// </summary>
        public static IEnumerable<NPCChatDialogBase> Dialogs
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the NPCChatDialogBase for the dialog with the given index.
        /// </summary>
        /// <param name="dialogIndex">Index of the dialog to get.</param>
        /// <returns>The NPCChatDialogBase for the dialog with the given index.</returns>
        public static NPCChatDialogBase GetDialog(int dialogIndex)
        {
            return _instance[dialogIndex];
        }

        /// <summary>
        /// Provides the implementation of the NPCChatManagerBase for this NPC chat manager.
        /// </summary>
        class ManagerImplementation : NPCChatManagerBase
        {
            /// <summary>
            /// NPCChatManagerBase constructor.
            /// </summary>
            public ManagerImplementation() : base(true)
            {
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
}