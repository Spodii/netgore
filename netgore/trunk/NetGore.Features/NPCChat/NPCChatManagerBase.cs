using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Features.NPCChat
{
    /// <summary>
    /// Base class for managing the <see cref="NPCChatDialogBase"/>s.
    /// </summary>
    public abstract class NPCChatManagerBase : IEnumerable<NPCChatDialogBase>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        const string _chatDialogsNodeName = "ChatDialogs";
        const string _rootNodeName = "NPCChatManager";

        readonly bool _isReadonly;
        readonly DArray<NPCChatDialogBase> _npcChatDialogs = new DArray<NPCChatDialogBase>(32);

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatManagerBase"/> class.
        /// </summary>
        /// <param name="isReadonly">If this manager is read-only.</param>
        protected NPCChatManagerBase(bool isReadonly)
        {
            _isReadonly = isReadonly;
            Load(ContentPaths.Build);
        }

        /// <summary>
        /// Gets the <see cref="NPCChatDialogBase"/> at the specified index.
        /// </summary>
        /// <param name="id">Index of the <see cref="NPCChatDialogBase"/>.</param>
        /// <returns>The <see cref="NPCChatDialogBase"/> at the specified index, or null if invalid.</returns>
        /// <exception cref="MethodAccessException">Tried to set when <see cref="IsReadonly"/> was true.</exception>
        public NPCChatDialogBase this[NPCChatDialogID id]
        {
            get
            {
                // Check for a valid index
                if (!_npcChatDialogs.CanGet((int)id))
                {
                    const string errmsg = "Invalid NPC chat dialog index `{0}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, id);
                    Debug.Fail(string.Format(errmsg, id));
                    return null;
                }

                return _npcChatDialogs[(int)id];
            }
            set
            {
                if (IsReadonly)
                    throw CreateReadonlyException();

                _npcChatDialogs[(int)id] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="GenericValueIOFormat"/> to use for when an instance of this class
        /// writes itself out to a new <see cref="GenericValueWriter"/>. If null, the format to use
        /// will be inherited from <see cref="GenericValueWriter.DefaultFormat"/>.
        /// Default value is null.
        /// </summary>
        public static GenericValueIOFormat? EncodingFormat { get; set; }

        /// <summary>
        /// Gets if this manager is read-only.
        /// </summary>
        public bool IsReadonly
        {
            get { return _isReadonly; }
        }

        /// <summary>
        /// When overridden in the derived class, creates a <see cref="NPCChatDialogBase"/> from the given <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read the values from.</param>
        /// <returns>A <see cref="NPCChatDialogBase"/> created from the given <see cref="IValueReader"/>.</returns>
        protected abstract NPCChatDialogBase CreateDialog(IValueReader reader);

        /// <summary>
        /// Creates a <see cref="MethodAccessException"/> to use for when trying to access a method that is cannot be access when read-only. 
        /// </summary>
        /// <returns>A <see cref="MethodAccessException"/> to use for when trying to access a method that is cannot be
        /// access when read-only.</returns>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "NPCChatManagerBase")]
        protected static MethodAccessException CreateReadonlyException()
        {
            return new MethodAccessException("Cannot access this method when the NPCChatManagerBase is set to Read-Only.");
        }

        /// <summary>
        /// Gets if a chat dialog exists at the given ID.
        /// </summary>
        /// <param name="id">The ID to check if contains a dialog.</param>
        /// <returns>True if a dialog exists at the given <paramref name="id"/>; otherwise false.</returns>
        public bool DialogExists(NPCChatDialogID id)
        {
            if (!_npcChatDialogs.CanGet((int)id))
                return false;

            return _npcChatDialogs[(int)id] != null;
        }

        /// <summary>
        /// Gets the path for the data file.
        /// </summary>
        /// <param name="contentPath"><see cref="ContentPaths"/> to use.</param>
        /// <returns>The path for the data file.</returns>
        protected static string GetFilePath(ContentPaths contentPath)
        {
            return contentPath.Data.Join("npcchat" + EngineSettings.DataFileSuffix);
        }

        /// <summary>
        /// Loads the data from file.
        /// </summary>
        /// <param name="contentPath">The content path to load the data from.</param>
        void Load(ContentPaths contentPath)
        {
            _npcChatDialogs.Clear();

            var filePath = GetFilePath(contentPath);

            if (!File.Exists(filePath))
            {
                _npcChatDialogs.Trim();
                return;
            }

            var reader = GenericValueReader.CreateFromFile(filePath, _rootNodeName);
            var items = reader.ReadManyNodes(_chatDialogsNodeName, CreateDialog);

            for (var i = 0; i < items.Length; i++)
            {
                if (items[i] != null)
                    _npcChatDialogs[i] = items[i];
            }

            _npcChatDialogs.Trim();
        }

        /// <summary>
        /// Reorganizes the internal buffer to ensure the indices all match up. Only needed if IsReadonly is false
        /// and you don't manually update the indices.
        /// </summary>
        public void Reorganize()
        {
            var dialogs = _npcChatDialogs.ToArray();
            _npcChatDialogs.Clear();

            foreach (var dialog in dialogs.Where(x => x != null))
            {
                _npcChatDialogs[(int)dialog.ID] = dialog;
            }
        }

        /// <summary>
        /// Saves the <see cref="NPCChatDialogBase"/>s in this <see cref="NPCChatManagerBase"/> to file.
        /// </summary>
        /// <param name="contentPath">The content path.</param>
        public void Save(ContentPaths contentPath)
        {
            var dialogs = _npcChatDialogs.Where(x => x != null);

            // Write
            var filePath = GetFilePath(contentPath);
            using (var writer = GenericValueWriter.Create(filePath, _rootNodeName, EncodingFormat))
            {
                writer.WriteManyNodes(_chatDialogsNodeName, dialogs, ((w, item) => item.Write(w)));
            }
        }

        #region IEnumerable<NPCChatDialogBase> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<NPCChatDialogBase> GetEnumerator()
        {
            return ((IEnumerable<NPCChatDialogBase>)_npcChatDialogs).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}