using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace NetGore.Features.NPCChat
{
    /// <summary>
    /// Describes all the parts of a conversation that can take place with an NPC.
    /// </summary>
    public abstract class NPCChatDialogBase
    {
        const string _nodeValueKeyID = "ID";
        const string _nodeValueKeyItems = "Items";
        const string _nodeValueKeyTitle = "Title";

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatDialogBase"/> class.
        /// </summary>
        protected NPCChatDialogBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatDialogBase"/> class.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read the values from.</param>
        protected NPCChatDialogBase(IValueReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// When overridden in the derived class, gets the unique index of this <see cref="NPCChatDialogBase"/>. This is used to
        /// distinguish each <see cref="NPCChatDialogBase"/> from one another.
        /// </summary>
        public abstract NPCChatDialogID ID { get; }

        /// <summary>
        /// When overridden in the derived class, gets the title for this dialog. The title is primarily
        /// used for debugging and development purposes only.
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// When overridden in the derived class, creates an <see cref="NPCChatDialogItemBase"/> using the given <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read the values from.</param>
        /// <returns>An <see cref="NPCChatDialogItemBase"/> created using the given <see cref="IValueReader"/>.</returns>
        protected abstract NPCChatDialogItemBase CreateDialogItem(IValueReader reader);

        /// <summary>
        /// Gets all of the <see cref="NPCChatDialogItemBase"/>s that <paramref name="root"/> can go to from any response
        /// and assuming an infinite number of branches can be taken.
        /// </summary>
        /// <param name="root">The root dialog to get all the possible branches for.</param>
        /// <returns>All of the <see cref="NPCChatDialogItemBase"/>s that this <see cref="NPCChatDialogItemBase"/> can go to from any response
        /// and assuming an infinite number of branches can be taken.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="root" /> is <c>null</c>.</exception>
        public IEnumerable<NPCChatDialogItemBase> GetChildDialogItems(NPCChatDialogItemBase root)
        {
            if (root == null)
                throw new ArgumentNullException("root");

            var foundItems = new SortedList<NPCChatDialogItemID, NPCChatDialogItemBase>();
            GetChildDialogItems(root, foundItems);

            return foundItems.Values;
        }

        void GetChildDialogItems(NPCChatDialogItemBase current, IDictionary<NPCChatDialogItemID, NPCChatDialogItemBase> found)
        {
            foreach (var response in current.Responses)
            {
                var page = response.Page;
                if (found.ContainsKey(page))
                    continue;

                var dialog = GetDialogItem(page);
                found.Add(page, dialog);

                GetChildDialogItems(dialog, found);
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="NPCChatDialogItemBase"/> for the given page number.
        /// </summary>
        /// <param name="chatDialogItemID">The page number of the <see cref="NPCChatDialogItemBase"/> to get.</param>
        /// <returns>
        /// The <see cref="NPCChatDialogItemBase"/> for the given <paramref name="chatDialogItemID"/>, or null if
        /// no valid <see cref="NPCChatDialogItemBase"/> existed for the given <paramref name="chatDialogItemID"/> or if
        /// the <paramref name="chatDialogItemID"/> is equal to
        /// <see cref="NPCChatResponseBase.EndConversationPage"/>.
        /// </returns>
        public abstract NPCChatDialogItemBase GetDialogItem(NPCChatDialogItemID chatDialogItemID);

        /// <summary>
        /// When overridden in the derived class, gets an IEnumerable of the <see cref="NPCChatDialogItemBase"/>s in this
        /// <see cref="NPCChatDialogBase"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of the <see cref="NPCChatDialogItemBase"/>s in
        /// this <see cref="NPCChatDialogBase"/>.</returns>
        protected abstract IEnumerable<NPCChatDialogItemBase> GetDialogItems();

        /// <summary>
        /// When overridden in the derived class, gets the initial <see cref="NPCChatDialogItemBase"/> that is used at the
        /// start of a conversation.
        /// </summary>
        /// <returns>The initial <see cref="NPCChatDialogItemBase"/> that is used at the start of a conversation.</returns>
        public abstract NPCChatDialogItemBase GetInitialDialogItem();

        /// <summary>
        /// Gets all of the <see cref="NPCChatResponseBase"/>s in this <see cref="NPCChatDialogBase"/>.
        /// </summary>
        /// <returns>All of the possible <see cref="NPCChatResponseBase"/>s in this <see cref="NPCChatDialogBase"/>.</returns>
        public IEnumerable<NPCChatResponseBase> GetResponses()
        {
            var allDialogs = GetDialogItems();
            var allResponses = allDialogs.SelectMany(x => x.Responses);
            return allResponses.Distinct();
        }

        /// <summary>
        /// Gets all of the <see cref="NPCChatResponseBase"/>s that direct to the given <paramref name="dialogItem"/>.
        /// </summary>
        /// <param name="dialogItem">The <see cref="NPCChatDialogItemBase"/> to find the source <see cref="NPCChatResponseBase"/> for.</param>
        /// <returns>All of the <see cref="NPCChatResponseBase"/>s that direct to the given <paramref name="dialogItem"/>.</returns>
        public IEnumerable<NPCChatResponseBase> GetSourceResponses(NPCChatDialogItemBase dialogItem)
        {
            var responses = GetResponses();
            return responses.Where(x => x.Page == dialogItem.ID);
        }

        /// <summary>
        /// Reads the values for this <see cref="NPCChatDialogBase"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read the values from.</param>
        protected void Read(IValueReader reader)
        {
            var id = reader.ReadNPCChatDialogID(_nodeValueKeyID);
            var title = reader.ReadString(_nodeValueKeyTitle);
            var items = reader.ReadManyNodes(_nodeValueKeyItems, CreateDialogItem);

            SetReadValues(id, title, items);
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="title">The title.</param>
        /// <param name="items">The dialog items.</param>
        protected abstract void SetReadValues(NPCChatDialogID id, string title, IEnumerable<NPCChatDialogItemBase> items);

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(string.Format("{0} [ID: {1}, Title: {2}]", GetType().Name, ID, Title));
        }

        /// <summary>
        /// Writes the <see cref="NPCChatDialogBase"/>'s values to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer"><see cref="IValueWriter"/> to write the values to.</param>
        public void Write(IValueWriter writer)
        {
            var items = GetDialogItems();

            writer.Write(_nodeValueKeyID, ID);
            writer.Write(_nodeValueKeyTitle, Title);
            writer.WriteManyNodes(_nodeValueKeyItems, items, ((w, item) => item.Write(w)));
        }
    }
}