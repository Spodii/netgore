using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame;
using DemoGame.Server.NPCChat.Conditionals;
using log4net;
using NetGore;
using NetGore.IO;
using NetGore.NPCChat;
using NetGore.NPCChat.Conditionals;

namespace DemoGame.Server.NPCChat
{
    /// <summary>
    /// Describes a single page of dialog in a NPCChatDialogBase, and the possible responses available for the page.
    /// This class is immutable and intended for use in the Server only.
    /// </summary>
    public class NPCChatDialogItem : NPCChatDialogItemBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        NPCChatConditionalCollectionBase _conditionals;
        ushort _index;
        bool _isBranch;
        NPCChatResponseBase[] _responses;

#if DEBUG
        // ReSharper disable UnaccessedField.Local
        /// <summary>
        /// The text. Only available in debug builds.
        /// </summary>
        string _text;

        // ReSharper restore UnaccessedField.Local
#endif

#if DEBUG
        // ReSharper disable UnaccessedField.Local
        /// <summary>
        /// The title. Only available in debug builds.
        /// </summary>
        string _title;

        // ReSharper restore UnaccessedField.Local
#endif

        /// <summary>
        /// When overridden in the derived class, gets the NPCChatConditionalCollectionBase that contains the
        /// conditionals used to evaluate if this NPCChatDialogItemBase may be used. If this value is null, it
        /// is assumed that there are no conditionals attached to this NPCChatDialogItemBase, and should be treated
        /// the same way as if the conditionals evaluated to true.
        /// </summary>
        public override NPCChatConditionalCollectionBase Conditionals
        {
            get { return _conditionals; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the page index of this NPCChatDialogItemBase in the
        /// NPCChatDialogBase. This value is unique to each NPCChatDialogItemBase in the NPCChatDialogBase.
        /// </summary>
        public override ushort Index
        {
            get { return _index; }
        }

        /// <summary>
        /// When overridden in the derived class, gets if this NPCChatDialogItemBase is a branch dialog or not. If
        /// true, the dialog should be automatically progressed by using EvaluateBranch() instead of waiting for
        /// and accepting input from the user for a response.
        /// </summary>
        public override bool IsBranch
        {
            get { return _isBranch; }
        }

        /// <summary>
        /// When overridden in the derived class, gets an IEnumerable of the EditorNPCChatResponses available
        /// for this page of dialog.
        /// </summary>
        public override IEnumerable<NPCChatResponseBase> Responses
        {
            get { return _responses; }
        }

        /// <summary>
        /// This property is not supported by the Server's NPCChatDialogItem, and will always return String.Empty.
        /// </summary>
        public override string Text
        {
            get
            {
                Debug.Fail("This property is not supported by the Server.");
                return string.Empty;
            }
        }

        /// <summary>
        /// This property is not supported by the Server's NPCChatDialogItem, and will always return String.Empty.
        /// </summary>
        public override string Title
        {
            get
            {
                Debug.Fail("This property is not supported by the Server.");
                return string.Empty;
            }
        }

        /// <summary>
        /// NPCChatDialogItem constructor.
        /// </summary>
        /// <param name="r">IValueReader to read the values from.</param>
        internal NPCChatDialogItem(IValueReader r) : base(r)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates a NPCChatConditionalCollectionBase.
        /// </summary>
        /// <returns>A new NPCChatConditionalCollectionBase instance, or null if the derived class does not
        /// want to load the conditionals when using Read.</returns>
        protected override NPCChatConditionalCollectionBase CreateConditionalCollection()
        {
            return new NPCChatConditionalCollection();
        }

        /// <summary>
        /// When overridden in the derived class, creates a NPCChatResponseBase using the given IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <returns>A NPCChatResponseBase created using the given IValueReader</returns>
        protected override NPCChatResponseBase CreateResponse(IValueReader reader)
        {
            return new NPCChatResponse(reader);
        }

        /// <summary>
        /// When overridden in the derived class, gets the NPCChatResponseBase of the response with the given
        /// <paramref name="responseIndex"/>.
        /// </summary>
        /// <param name="responseIndex">Index of the response.</param>
        /// <returns>The NPCChatResponseBase for the response at index <paramref name="responseIndex"/>, or null
        /// if the response is invalid or ends the chat dialog.</returns>
        public override NPCChatResponseBase GetResponse(byte responseIndex)
        {
            if (responseIndex >= _responses.Length)
            {
                const string errmsg = "Invalid response index `{0}` for page `{1}`. Max response index is `{2}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, responseIndex, Index, _responses.Length - 1);
                return null;
            }

            return _responses[responseIndex];
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="page">The index.</param>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="isBranch">The IsBranch value.</param>
        /// <param name="responses">The responses.</param>
        /// <param name="conditionals">The conditionals.</param>
        protected override void SetReadValues(ushort page, string title, string text, bool isBranch,
                                              IEnumerable<NPCChatResponseBase> responses,
                                              NPCChatConditionalCollectionBase conditionals)
        {
            Debug.Assert(_index == default(ushort) && _responses == default(IEnumerable<NPCChatResponseBase>),
                         "Values were already set?");

            _index = page;
            _isBranch = isBranch;
            _responses = responses.ToArray();
            _conditionals = conditionals;

#if DEBUG
            _text = text;
            _title = title;
#endif
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(string.Format("{0} [Index: {1}]", GetType().Name, Index));
        }
    }
}