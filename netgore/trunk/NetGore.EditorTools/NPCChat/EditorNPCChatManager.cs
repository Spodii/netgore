using System.Linq;
using NetGore.IO;
using NetGore.NPCChat;

namespace NetGore.EditorTools
{
    public static class EditorNPCChatManager
    {
        static readonly ManagerImplementation _instance = new ManagerImplementation();

        public static void AddDialog(EditorNPCChatDialog dialog)
        {
            // Just always reorganize, since I'm not very trusting that things are always done right
            _instance.Reorganize();

            // Add the new dialog
            _instance[dialog.Index] = dialog;
        }

        public static EditorNPCChatDialog GetDialog(int index)
        {
            NPCChatDialogBase ret = _instance[index];

            // If we grabbed the wrong one, or nothing, try reorganizing
            if (ret == null || ret.Index != index)
            {
                _instance.Reorganize();
                ret = _instance[index];
            }

            // Return whatever we found, since its not like we can do anything else even if it is invalid
            return (EditorNPCChatDialog)ret;
        }

        public static void Save()
        {
            _instance.Save();
        }

        class ManagerImplementation : NPCChatManagerBase
        {
            /// <summary>
            /// ManagerImplementation constructor.
            /// </summary>
            public ManagerImplementation() : base(false)
            {
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
        }
    }
}