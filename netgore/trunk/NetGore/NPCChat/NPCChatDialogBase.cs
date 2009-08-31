using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;

namespace NetGore.NPCChat
{
    /// <summary>
    /// Describes all the parts of a conversation that can take place with an NPC.
    /// </summary>
    public abstract class NPCChatDialogBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// When overridden in the derived class, gets the unique index of this NPCChatDialogBase. This is used to
        /// distinguish each NPCChatDialogBase from one another.
        /// </summary>
        public abstract ushort Index { get; }

        /// <summary>
        /// When overridden in the derived class, gets the title for this dialog. The title is primarily
        /// used for debugging and development purposes only.
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// NPCChatDialogBase constructor.
        /// </summary>
        protected NPCChatDialogBase()
        {
        }

        /// <summary>
        /// NPCChatDialogBase constructor.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        protected NPCChatDialogBase(IValueReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// When overridden in the derived class, creates an NPCChatDialogItemBase using the given IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <returns>An NPCChatDialogItemBase created using the given IValueReader.</returns>
        protected abstract NPCChatDialogItemBase CreateDialogItem(IValueReader reader);

        /// <summary>
        /// Creates an ArgumentOutOfRangeException for the response index being out of range.
        /// </summary>
        /// <param name="parameterName">The name of the dialog page index parameter.</param>
        /// <param name="value">The specified, invalid dialog page index value.</param>
        /// <returns>An ArgumentOutOfRangeException for the response index being out of range.</returns>
        protected ArgumentOutOfRangeException CreateInvalidResponseIndexException(string parameterName, ushort value)
        {
            const string errmsg = "Dialog page index `{0}` was out of range for dialog `{1}`.";
            string err = string.Format(errmsg, value, Index);
            if (log.IsErrorEnabled)
                log.Error(err);

            return new ArgumentOutOfRangeException(err, parameterName);
        }

        /// <summary>
        /// Gets all of the NPCChatDialogItemBases that <paramref name="root"/> can go to from any response
        /// and assuming an infinite number of branches can be taken.
        /// </summary>
        /// <param name="root">The root dialog to get all the possible branches for.</param>
        /// <returns>All of the NPCChatDialogItemBases that this NPCChatDialogItemBase can go to from any response
        /// and assuming an infinite number of branches can be taken.</returns>
        public IEnumerable<NPCChatDialogItemBase> GetChildDialogItems(NPCChatDialogItemBase root)
        {
            if (root == null)
                throw new ArgumentNullException("root");

            var foundItems = new SortedList<ushort, NPCChatDialogItemBase>();
            GetChildDialogItems(root, foundItems);

            return foundItems.Values;
        }

        void GetChildDialogItems(NPCChatDialogItemBase current, IDictionary<ushort, NPCChatDialogItemBase> found)
        {
            foreach (NPCChatResponseBase response in current.Responses)
            {
                ushort page = response.Page;
                if (found.ContainsKey(page))
                    continue;

                NPCChatDialogItemBase dialog = GetDialogItem(page);
                found.Add(page, dialog);

                GetChildDialogItems(dialog, found);
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets the NPCChatDialogItemBase for the given page number.
        /// </summary>
        /// <param name="page">The page number of the NPCChatDialogItemBase to get.</param>
        /// <returns>The NPCChatDialogItemBase for the given <paramref name="page"/>, or null if no valid
        /// NPCChatDialogItemBase existed for the given <paramref name="page"/>.</returns>
        public abstract NPCChatDialogItemBase GetDialogItem(ushort page);

        /// <summary>
        /// When overridden in the derived class, gets an IEnumerable of the NPCChatDialogItemBases in this
        /// NPCChatDialogBase.
        /// </summary>
        /// <returns>An IEnumerable of the NPCChatDialogItemBases in this NPCChatDialogBase.</returns>
        protected abstract IEnumerable<NPCChatDialogItemBase> GetDialogItems();

        /// <summary>
        /// When overridden in the derived class, gets the initial NPCChatDialogItemBase that is used at the
        /// start of a conversation.
        /// </summary>
        /// <returns>The initial NPCChatDialogItemBase that is used at the start of a conversation.</returns>
        public abstract NPCChatDialogItemBase GetInitialDialogItem();

        /// <summary>
        /// Gets all of the NPCChatResponseBases in this NPCChatDialogBase.
        /// </summary>
        /// <returns>All of the possible NPCChatResponseBases in this NPCChatDialogBase.</returns>
        public IEnumerable<NPCChatResponseBase> GetResponses()
        {
            var allDialogs = GetDialogItems();
            var allResponses = allDialogs.SelectMany(x => x.Responses);
            return allResponses.Distinct();
        }

        /// <summary>
        /// Gets all of the NPCChatResponseBases that direct to the given <paramref name="dialogItem"/>.
        /// </summary>
        /// <param name="dialogItem">The NPCChatDialogItemBase to find the source NPCChatResponseBase for.</param>
        /// <returns>All of the NPCChatResponseBases that direct to the given <paramref name="dialogItem"/>.</returns>
        public IEnumerable<NPCChatResponseBase> GetSourceResponses(NPCChatDialogItemBase dialogItem)
        {
            var responses = GetResponses();
            return responses.Where(x => x.Page == dialogItem.Index);
        }

        /// <summary>
        /// Reads the values for this NPCChatDialogBase from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        protected void Read(IValueReader reader)
        {
            ushort index = reader.ReadUShort("Index");
            string title = reader.ReadString("Title");
            var items = reader.ReadManyNodes("Items", x => CreateDialogItem(x));

            SetReadValues(index, title, items);
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="title">The title.</param>
        /// <param name="items">The dialog items.</param>
        protected abstract void SetReadValues(ushort index, string title, IEnumerable<NPCChatDialogItemBase> items);

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(string.Format("{0} [Index: {1}, Title: {2}]", GetType().Name, Index, Title));
        }

        /// <summary>
        /// Writes the NPCChatDialogBase's values to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write the values to.</param>
        public void Write(IValueWriter writer)
        {
            var items = GetDialogItems();

            writer.Write("Index", Index);
            writer.Write("Title", Title);
            writer.WriteManyNodes("Items", items, ((w, item) => item.Write(w)));
        }
    }
}