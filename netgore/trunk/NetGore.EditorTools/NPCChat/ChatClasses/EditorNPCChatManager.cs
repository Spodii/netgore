using System.Collections.Generic;
using System.Linq;
using NetGore.IO;
using NetGore.NPCChat;

namespace NetGore.EditorTools.NPCChat
{
    public class EditorNPCChatManager : NPCChatManagerBase
    {
        static readonly EditorNPCChatManager _instance = new EditorNPCChatManager();

        /// <summary>
        /// ManagerImplementation constructor.
        /// </summary>
        EditorNPCChatManager() : base(false)
        {
        }

        /// <summary>
        /// Gets an IEnmerable of the EditorNPCChatDialogs in this collection.
        /// </summary>
        public static IEnumerable<EditorNPCChatDialog> Dialogs
        {
            get { return _instance.Cast<EditorNPCChatDialog>(); }
        }

        /// <summary>
        /// Adds a EditorNPCChatDialog.
        /// </summary>
        /// <param name="dialog">The EditorNPCChatDialog to add.</param>
        public static void AddDialog(EditorNPCChatDialog dialog)
        {
            // Just always reorganize, since I'm not very trusting that things are always done right
            _instance.Reorganize();

            // Add the new dialog
            _instance[dialog.ID] = dialog;
        }

        /// <summary>
        /// When overridden in the derived class, creates a NPCChatDialogBase from the given IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <returns>A NPCChatDialogBase created from the given IValueReader.</returns>
        protected override NPCChatDialogBase CreateDialog(IValueReader reader)
        {
            return new EditorNPCChatDialog(reader);
        }

        /// <summary>
        /// Gets the EditorNPCChatDialog at the specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the EditorNPCChatDialog to get.</param>
        /// <returns>The EditorNPCChatDialog at the specified <paramref name="id"/>.</returns>
        public static EditorNPCChatDialog GetDialog(NPCChatDialogID id)
        {
            var ret = _instance[id];

            // If we grabbed the wrong one, or nothing, try reorganizing
            if (ret == null || ret.ID != id)
            {
                _instance.Reorganize();
                ret = _instance[id];
            }

            // Return whatever we found, since its not like we can do anything else even if it is invalid
            return (EditorNPCChatDialog)ret;
        }

        /// <summary>
        /// Saves this collection to file.
        /// </summary>
        public static void SaveDialogs()
        {
            // Save the dialog to both build and dev
            _instance.Save(ContentPaths.Build);
            _instance.Save(ContentPaths.Dev);
        }
    }
}