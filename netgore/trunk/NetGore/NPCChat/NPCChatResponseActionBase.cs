using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;
using NetGore.NPCChat.Conditionals;

namespace NetGore.NPCChat
{
    /// <summary>
    /// The base class for an action that takes place after choosing a <see cref="NPCChatResponseBase"/> in
    /// a <see cref="NPCChatDialogBase"/>.
    /// </summary>
    public abstract class NPCChatResponseActionBase
    {
        /// <summary>
        /// Dictionary that contains the <see cref="NPCChatResponseActionBase"/> instance
        /// of each derived class, with the <see cref="Name"/> as the key.
        /// </summary>
        static readonly Dictionary<string, NPCChatConditionalBase> _instances =
            new Dictionary<string, NPCChatConditionalBase>(_nameComparer);

        /// <summary>
        /// Gets an IEnumerable of the <see cref="NPCChatConditionalBase"/>s.
        /// </summary>
        public static IEnumerable<NPCChatConditionalBase> Conditionals
        {
            get { return _instances.Values; }
        }

        /// <summary>
        /// <see cref="StringComparer"/> used for the Name.
        /// </summary>
        static readonly StringComparer _nameComparer = StringComparer.Ordinal;

        /// <summary>
        /// Loads and manages the derived Types for this collection.
        /// </summary>
        static readonly FactoryTypeCollection _typeCollection;

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly string _name;

        /// <summary>
        /// Gets the unique name for this <see cref="NPCChatResponseActionBase"/>. This string is case-sensitive.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Initializes the <see cref="NPCChatResponseActionBase"/> class.
        /// </summary>
        static NPCChatResponseActionBase()
        {
            var filter = FactoryTypeCollection.CreateFilter(typeof(NPCChatConditionalBase), true, Type.EmptyTypes);
            _typeCollection = new FactoryTypeCollection(filter, OnLoadTypeHandler, false);
            _typeCollection.BeginLoading();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatResponseActionBase"/> class.
        /// </summary>
        /// <param name="name">The unique name for this <see cref="NPCChatResponseActionBase"/>.
        /// This string is case-sensitive.</param>
        protected NPCChatResponseActionBase(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            _name = name;
        }

        /// <summary>
        /// When overridden in the derived class, performs the actions implemented by this
        /// <see cref="NPCChatResponseActionBase"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="npc">The npc.</param>
        public abstract void Execute(object user, object npc);

        /// <summary>
        /// Gets if a <see cref="NPCChatResponseActionBase"/> with the given <paramref name="name"/> exists.
        /// </summary>
        /// <param name="name">The name of the <see cref="NPCChatResponseActionBase"/>.</param>
        /// <returns>True if a <see cref="NPCChatResponseActionBase"/> with the given <paramref name="name"/>
        /// exists; otherwise false.</returns>
        public static bool ContainsResponseAction(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            return _instances.ContainsKey(name);
        }

        /// <summary>
        /// Gets the <see cref="NPCChatResponseActionBase"/> with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="NPCChatResponseActionBase"/> to get.</param>
        /// <returns>The <see cref="NPCChatResponseActionBase"/> with the given <paramref name="name"/>.</returns>
        public static NPCChatConditionalBase GetResponseAction(string name)
        {
            return _instances[name];
        }

        /// <summary>
        /// Handles when a new type has been loaded into the <see cref="FactoryTypeCollection"/>.
        /// </summary>
        /// <param name="factoryTypeCollection">FactoryTypeCollection that the event occured on.</param>
        /// <param name="loadedType">Type that was loaded.</param>
        /// <param name="name">Name of the Type.</param>
        static void OnLoadTypeHandler(FactoryTypeCollection factoryTypeCollection, Type loadedType, string name)
        {
            NPCChatConditionalBase instance = (NPCChatConditionalBase)_typeCollection.GetTypeInstance(name);

            // Make sure the name is not already in use
            if (ContainsResponseAction(instance.Name))
            {
                const string errmsg = "Could not add Type `{0}` - a resposne action named `{1}` already exists as Type `{2}`.";
                string err = string.Format(errmsg, loadedType, instance.Name, _instances[instance.Name].GetType());
                if (log.IsFatalEnabled)
                    log.Fatal(err);
                Debug.Fail(err);
                throw new Exception(err);
            }

            // Add the value to the Dictionary
            _instances.Add(instance.Name, instance);

            if (log.IsInfoEnabled)
                log.InfoFormat("Loaded NPC chat response action `{0}` from Type `{1}`.", instance.Name, loadedType);
        }

        /// <summary>
        /// Tries to get the <see cref="NPCChatResponseActionBase"/> with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="NPCChatResponseActionBase"/> to get.</param>
        /// <param name="value">If the method returns true, contains the <see cref="NPCChatResponseActionBase"/>
        /// with the given <paramref name="name"/>.</param>
        /// <returns>True if the <paramref name="value"/> was successfully acquired; otherwise false.</returns>
        public static bool TryGetResponseAction(string name, out NPCChatConditionalBase value)
        {
            return _instances.TryGetValue(name, out value);
        }
    }
}