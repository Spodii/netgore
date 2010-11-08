using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.IO;

namespace NetGore.Features.NPCChat
{
    public class EditorNPCChatManager : NPCChatManagerBase
    {
        static readonly EditorNPCChatManager _instance = new EditorNPCChatManager();

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorNPCChatManager"/> class.
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
        /// Gets the <see cref="EditorNPCChatManager"/> instance.
        /// </summary>
        public static EditorNPCChatManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Adds an <see cref="EditorNPCChatDialog"/>.
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
        /// When overridden in the derived class, creates a <see cref="NPCChatDialogBase"/> from the given <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <returns>A NPCChatDialogBase created from the given IValueReader.</returns>
        protected override NPCChatDialogBase CreateDialog(IValueReader reader)
        {
            return new EditorNPCChatDialog(reader);
        }

        /// <summary>
        /// Creates a new <see cref="EditorNPCChatDialog"/> and adds it to this collection.
        /// </summary>
        /// <returns>The new <see cref="EditorNPCChatDialog"/>.</returns>
        public static EditorNPCChatDialog CreateNewDialog()
        {
            _instance.Reorganize();

            // Find the first free index
            var i = 0;
            while (_instance.DialogExists((NPCChatDialogID)i))
            {
                ++i;
            }

            // Create the new instance
            var dialog = new EditorNPCChatDialog();
            dialog.SetID(new NPCChatDialogID(i));

            // Create the initial dialog item
            var dialogItem = new EditorNPCChatDialogItem(dialog.GetFreeDialogItemID(), "New dialog");
            dialogItem.SetText("<Enter the initial text to display>");
            dialog.Add(dialogItem);

            // Add to the collection
            AddDialog(dialog);

            return dialog;
        }

        /// <summary>
        /// Deletes a <see cref="EditorNPCChatDialog"/>.
        /// </summary>
        /// <param name="dialog">The <see cref="EditorNPCChatDialog"/> to delete.</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void DeleteDialog(EditorNPCChatDialog dialog)
        {
            _instance.Reorganize();

            if (GetDialog(dialog.ID) != dialog)
                return;

            _instance[dialog.ID] = null;
        }

        /// <summary>
        /// Gets the <see cref="EditorNPCChatDialog"/> at the specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the <see cref="EditorNPCChatDialog"/> to get.</param>
        /// <returns>The <see cref="EditorNPCChatDialog"/> at the specified <paramref name="id"/>.</returns>
        public static EditorNPCChatDialog GetDialog(NPCChatDialogID id)
        {
            if (!_instance.DialogExists(id))
            {
                _instance.Reorganize();
                if (!_instance.DialogExists(id))
                    return null;
            }

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