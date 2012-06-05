using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;

namespace NetGore.Features.NPCChat
{
    /// <summary>
    /// The base class for an action that takes place after choosing a <see cref="NPCChatResponseBase"/> in
    /// a <see cref="NPCChatDialogBase"/>.
    /// </summary>
    public abstract class NPCChatResponseActionBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static readonly NPCChatResponseActionBase[] _emptyActions;

        /// <summary>
        /// Dictionary that contains the <see cref="NPCChatResponseActionBase"/> instance
        /// of each derived class, with the <see cref="Name"/> as the key.
        /// </summary>
        static readonly Dictionary<string, NPCChatResponseActionBase> _instances =
            new Dictionary<string, NPCChatResponseActionBase>(StringComparer.Ordinal);

        readonly string _name;

        /// <summary>
        /// Initializes the <see cref="NPCChatResponseActionBase"/> class.
        /// </summary>
        static NPCChatResponseActionBase()
        {
            _emptyActions = new NPCChatResponseActionBase[0];

            var filter = new TypeFilterCreator
            {
                IsClass = true,
                IsAbstract = false,
                Subclass = typeof(NPCChatResponseActionBase),
                RequireConstructor = true,
                ConstructorParameters = Type.EmptyTypes
            };

            new TypeFactory(filter.GetFilter(), OnLoadTypeHandler);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatResponseActionBase"/> class.
        /// </summary>
        /// <param name="name">The unique name for this <see cref="NPCChatResponseActionBase"/>.
        /// This string is case-sensitive.</param>
        /// <exception cref="ArgumentNullException">Argument is null.</exception>
        protected NPCChatResponseActionBase(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            _name = name;
        }

        /// <summary>
        /// Gets an IEnumerable of the <see cref="NPCChatResponseActionBase"/>s.
        /// </summary>
        public static IEnumerable<NPCChatResponseActionBase> Conditionals
        {
            get { return _instances.Values; }
        }

        /// <summary>
        /// Gets an empty array of <see cref="NPCChatResponseActionBase"/>s.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public static NPCChatResponseActionBase[] EmptyActions
        {
            get { return _emptyActions; }
        }

        /// <summary>
        /// Gets the unique name for this <see cref="NPCChatResponseActionBase"/>. This string is case-sensitive.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

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
        /// When overridden in the derived class, performs the actions implemented by this
        /// <see cref="NPCChatResponseActionBase"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="npc">The npc.</param>
        public abstract void Execute(object user, object npc);

        /// <summary>
        /// Gets the <see cref="NPCChatResponseActionBase"/> with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="NPCChatResponseActionBase"/> to get.</param>
        /// <returns>The <see cref="NPCChatResponseActionBase"/> with the given <paramref name="name"/>.</returns>
        public static NPCChatResponseActionBase GetResponseAction(string name)
        {
            return _instances[name];
        }

        /// <summary>
        /// Handles when a new type has been loaded into the <see cref="TypeFactory"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NetGore.Collections.TypeFactoryLoadedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="DuplicateKeyException">The type in <paramref name="e"/> was already loaded.</exception>
        static void OnLoadTypeHandler(TypeFactory sender, TypeFactoryLoadedEventArgs e)
        {
            var instance = (NPCChatResponseActionBase)sender.GetTypeInstance(e.Name);

            // Make sure the name is not already in use
            if (ContainsResponseAction(instance.Name))
            {
                const string errmsg = "Could not add Type `{0}` - a response action named `{1}` already exists as Type `{2}`.";
                var err = string.Format(errmsg, e.LoadedType, instance.Name, _instances[instance.Name].GetType());
                if (log.IsFatalEnabled)
                    log.Fatal(err);
                Debug.Fail(err);
                throw new DuplicateKeyException(err);
            }

            // Add the value to the Dictionary
            _instances.Add(instance.Name, instance);

            if (log.IsDebugEnabled)
                log.DebugFormat("Loaded NPC chat response action `{0}` from Type `{1}`.", instance.Name, e.LoadedType);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Tries to get the <see cref="NPCChatResponseActionBase"/> with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="NPCChatResponseActionBase"/> to get.</param>
        /// <param name="value">If the method returns true, contains the <see cref="NPCChatResponseActionBase"/>
        /// with the given <paramref name="name"/>.</param>
        /// <returns>True if the <paramref name="value"/> was successfully acquired; otherwise false.</returns>
        public static bool TryGetResponseAction(string name, out NPCChatResponseActionBase value)
        {
            return _instances.TryGetValue(name, out value);
        }
    }
}