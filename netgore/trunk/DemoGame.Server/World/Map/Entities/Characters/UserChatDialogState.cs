using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Network;
using NetGore.NPCChat;

namespace DemoGame.Server
{
    /// <summary>
    /// Describes the current state of a chat dialog a User is having with a NPC.
    /// </summary>
    public class UserChatDialogState
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Defines how the to handle when the conditionals to a response chosen by the client evaluate to false.
        /// </summary>
        const ResponseConditionalFailureHandleType _responseConditionalFailureType =
            ResponseConditionalFailureHandleType.ResendDialogItem;

        /// <summary>
        /// The User that the dialog is taking place with.
        /// </summary>
        readonly User _user;

        /// <summary>
        /// The NPC that the dialog is taking place with.
        /// </summary>
        NPC _chattingWith;

        /// <summary>
        /// The current dialog item.
        /// </summary>
        NPCChatDialogItemBase _dialogItem;

        /// <summary>
        /// Initializes the <see cref="UserChatDialogState"/> class.
        /// </summary>
        static UserChatDialogState()
        {
            if (!EnumHelper<ResponseConditionalFailureHandleType>.IsDefined(_responseConditionalFailureType))
            {
                const string errmsg = "Invalid _responseConditionalFailureType value `{0}`.";
                string err = string.Format(errmsg, _responseConditionalFailureType);
                log.Fatal(err);
                Debug.Fail(err);
                throw new Exception(err);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserChatDialogState"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public UserChatDialogState(User user)
        {
            _user = user;
        }

        /// <summary>
        /// Gets the current NPCChatDialogBase, or null if the User is not currently chatting.
        /// </summary>
        public NPCChatDialogBase ChatDialog
        {
            get
            {
                NPC n = _chattingWith;
                if (n == null)
                    return null;

                return n.ChatDialog;
            }
        }

        /// <summary>
        /// Gets if the User is currently chatting.
        /// </summary>
        public bool IsChatting
        {
            get { return _chattingWith != null; }
        }

        /// <summary>
        /// Checks if the User can start a dialog with the given <paramref name="npc"/>.
        /// </summary>
        /// <param name="npc">The NPC to start the dialog with.</param>
        /// <returns>True if the User can start a dialog with the given <paramref name="npc"/>; otherwise false.</returns>
        bool CanStartChat(Character npc)
        {
            const string errmsg = "Cannot start chat between `{0}` and `{1}` - {2}.";

            // Check if a chat is already going
            if (IsChatting)
            {
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, _user, npc, "Chat dialog already open");
                return false;
            }

            // Check for valid states
            if (npc == null)
                throw new ArgumentNullException("npc");

            if (!_user.IsAlive)
            {
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, _user, npc, "User is dead");
                return false;
            }

            if (!npc.IsAlive)
            {
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, _user, npc, "NPC is dead");
                return false;
            }

            if (!_user.OnGround)
            {
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, _user, npc, "User is not on the ground");
                return false;
            }

            if (_user.Map == null)
            {
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, _user, npc, "User's map is null");
                return false;
            }

            if (npc.ChatDialog == null)
            {
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, _user, npc, "NPC has no chat dialog");
                return false;
            }

            // Check for a valid distance
            if (_user.Map != npc.Map)
            {
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, _user, npc, "Characters are on different maps");
                return false;
            }

            if (!_user.CB.Intersect(npc.CB))
            {
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, _user, npc, "Characters are not touching");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Forces the chat session to end if it has not already.
        /// </summary>
        public void EndChat()
        {
            if (!IsChatting)
                return;

            _dialogItem = null;
            _chattingWith = null;

            NotifyUserOfNewPage();
        }

        /// <summary>
        /// Progresses the chat dialog by using the given <paramref name="responseIndex"/>.
        /// </summary>
        /// <param name="responseIndex">The index of the response to use for the current dialog page.</param>
        public void EnterResponse(byte responseIndex)
        {
            // Ensure there is a chat session going on
            if (!IsChatting)
            {
                const string errmsg = "Could not enter response of index `{0}` since there is no chat session active.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, responseIndex);
                Debug.Fail(string.Format(errmsg, responseIndex));
                return;
            }

            // Check for a valid range
            if (!_user.CB.Intersect(_chattingWith.CB))
            {
                if (log.IsInfoEnabled)
                    log.Info("Dialog aborted since the User is no longer near the target.");
                EndChat();
                return;
            }

            // Get the response
            NPCChatResponseBase response = _dialogItem.GetResponse(responseIndex);
            if (response == null)
            {
                EndChat();
                return;
            }

            Debug.Assert(response.Value == responseIndex, "Something went wrong, and we got the wrong response. lolwtf?");

            // Ensure the selected response index is allowed (response conditionals check)
            if (!response.CheckConditionals(_user, _chattingWith))
            {
#pragma warning disable 162
                switch (_responseConditionalFailureType)
                {
                    case ResponseConditionalFailureHandleType.EndDialog:
                        EndChat();
                        return;

                    case ResponseConditionalFailureHandleType.ResendDialogItem:
                        NotifyUserOfNewPage();
                        return;

                    default:
                        throw new Exception("Invalid _responseConditionalFailureType.");
                }
#pragma warning restore 162
            }

            // Execute the actions
            if (response.Actions != null)
            {
                foreach (NPCChatResponseActionBase action in response.Actions)
                {
                    action.Execute(_user, _chattingWith);
                }
            }

            // Get the next page
            NPCChatDialogItemBase nextPage = ChatDialog.GetDialogItem(response.Page);
            nextPage = GetNextDialogPage(nextPage);

            // Set the new page
            _dialogItem = nextPage;

            // Check if the dialog has ended, otherwise just notify the user of the new page
            if (_dialogItem == null)
                EndChat();
            else
                NotifyUserOfNewPage();
        }

        /// <summary>
        /// If the given <paramref name="page"/> is a dialog-less page, this will skip to the next page
        /// that contains dialog. If the <paramref name="page"/> has dialog, this will just return
        /// that same <paramref name="page"/>.
        /// </summary>
        /// <param name="page">The page to attempt to skip through.</param>
        /// <returns>The page to use, or null if the dialog has ended.</returns>
        NPCChatDialogItemBase GetNextDialogPage(NPCChatDialogItemBase page)
        {
            // Skip until we find a null page, or we are no longer at a branch
            while (page != null && page.IsBranch)
            {
                // Evaluate the branch to get the response
                NPCChatResponseBase branchResponse = page.EvaluateBranch(_user, _chattingWith);

                // Make sure we execute any actions on the response
                if (branchResponse.Actions != null)
                {
                    foreach (NPCChatResponseActionBase action in branchResponse.Actions)
                    {
                        action.Execute(_user, _chattingWith);
                    }
                }

                // Get the next dialog item page from the response
                page = ChatDialog.GetDialogItem(branchResponse.Page);
            }

            return page;
        }

        /// <summary>
        /// Gets an IEnumerable of the indexes of the responses to skip for the current dialog item.
        /// </summary>
        /// <returns>An IEnumerable of the indexes of the responses to skip for the current dialog item.</returns>
        IEnumerable<byte> GetResponsesToSkip()
        {
            List<byte> retValues = null;

            foreach (NPCChatResponseBase response in _dialogItem.Responses)
            {
                if (!response.CheckConditionals(_user, _chattingWith))
                {
                    if (retValues == null)
                        retValues = new List<byte>();

                    retValues.Add(response.Value);
                }
            }

            return retValues;
        }

        /// <summary>
        /// Notifies the User about the new page.
        /// </summary>
        void NotifyUserOfNewPage()
        {
            if (_dialogItem == null)
            {
                // Dialog has ended
                using (PacketWriter pw = ServerPacket.EndChatDialog())
                {
                    _user.Send(pw);
                }
            }
            else
            {
                // New page
                using (PacketWriter pw = ServerPacket.SetChatDialogPage(_dialogItem.Index, GetResponsesToSkip()))
                {
                    _user.Send(pw);
                }
            }
        }

        /// <summary>
        /// Attempts to start a chat dialog with the given <paramref name="npc"/>.
        /// </summary>
        /// <param name="npc">NPC to start the chat dialog with.</param>
        /// <returns>True if the dialog was started with the <paramref name="npc"/>; otherwise false.</returns>
        public bool StartChat(NPC npc)
        {
            // Check if the chat can be started
            if (!CanStartChat(npc))
                return false;

            _chattingWith = npc;

            // Tell the client to open the dialog
            using (PacketWriter pw = ServerPacket.StartChatDialog(npc.MapEntityIndex, ChatDialog.Index))
            {
                _user.Send(pw);
            }

            // Get the first page to use
            NPCChatDialogItemBase initialPage = ChatDialog.GetInitialDialogItem();
            Debug.Assert(initialPage != null);
            _dialogItem = GetNextDialogPage(initialPage);

            // Tell the user which page to use
            NotifyUserOfNewPage();

            return true;
        }

        /// <summary>
        /// Enum of the different ways to handle having the conditionals to a response chosen by the client
        /// being invalid.
        /// </summary>
        enum ResponseConditionalFailureHandleType
        {
            /// <summary>
            /// The dialog is ended immediately.
            /// </summary>
            EndDialog,

            /// <summary>
            /// The dialog item is resent to the user under the assumption that the response was valid when
            /// the page was opened, but was no longer valid by the time a response was chosen (User's state
            /// has changed).
            /// </summary>
            ResendDialogItem,
        }
    }
}