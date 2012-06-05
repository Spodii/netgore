using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;

namespace NetGore.Features.NPCChat.Conditionals
{
    /// <summary>
    /// The base class for a conditional used in the NPC chatting. Each instanceable derived class
    /// must include a parameterless constructor (preferably private).
    /// </summary>
    public abstract class NPCChatConditionalBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Array used for an empty set of <see cref="NPCChatConditionalParameterType"/>s.
        /// </summary>
        static readonly NPCChatConditionalParameterType[] _emptyParameterTypes;

        /// <summary>
        /// Array used for an empty set of <see cref="NPCChatConditionalParameter"/>s.
        /// </summary>
        static readonly NPCChatConditionalParameter[] _emptyParameters;

        /// <summary>
        /// Dictionary that contains the <see cref="NPCChatConditionalBase"/> instance
        /// of each derived class, with the <see cref="Name"/> as the key.
        /// </summary>
        static readonly Dictionary<string, NPCChatConditionalBase> _instances =
            new Dictionary<string, NPCChatConditionalBase>(StringComparer.Ordinal);

        readonly string _name;
        readonly NPCChatConditionalParameterType[] _parameterTypes;

        /// <summary>
        /// Initializes the <see cref="NPCChatConditionalBase"/> class.
        /// </summary>
        static NPCChatConditionalBase()
        {
            _emptyParameters = new NPCChatConditionalParameter[0];
            _emptyParameterTypes = new NPCChatConditionalParameterType[0];

            var filter = new TypeFilterCreator
            {
                IsClass = true,
                IsAbstract = false,
                Subclass = typeof(NPCChatConditionalBase),
                RequireConstructor = true,
                ConstructorParameters = Type.EmptyTypes
            };

            new TypeFactory(filter.GetFilter(), OnLoadTypeHandler);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatConditionalBase"/> class.
        /// </summary>
        /// <param name="name">The unique display name of this <see cref="NPCChatConditionalBase"/>. This name
        /// must be unique for each derived class type. This string is case-sensitive.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <exception cref="ArgumentNullException">Argument is null.</exception>
        protected NPCChatConditionalBase(string name, params NPCChatConditionalParameterType[] parameterTypes)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (parameterTypes == null || parameterTypes.Length == 0)
                parameterTypes = _emptyParameterTypes;

            _name = name;
            _parameterTypes = parameterTypes;
        }

        /// <summary>
        /// Gets an IEnumerable of the <see cref="NPCChatConditionalBase"/>s.
        /// </summary>
        public static IEnumerable<NPCChatConditionalBase> Conditionals
        {
            get { return _instances.Values; }
        }

        /// <summary>
        /// Gets the unique name for this <see cref="NPCChatConditionalBase"/>. This string is case-sensitive.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the number of parameters required by this <see cref="NPCChatConditionalBase"/>.
        /// </summary>
        public int ParameterCount
        {
            get { return _parameterTypes.Length; }
        }

        /// <summary>
        /// Gets an IEnumerable of the <see cref="NPCChatConditionalParameterType"/> used in this
        /// <see cref="NPCChatConditionalBase"/>.
        /// </summary>
        public IEnumerable<NPCChatConditionalParameterType> ParameterTypes
        {
            get { return _parameterTypes; }
        }

        /// <summary>
        /// Gets if a <see cref="NPCChatConditionalBase"/> with the given <paramref name="name"/> exists.
        /// </summary>
        /// <param name="name">The name of the <see cref="NPCChatConditionalBase"/>.</param>
        /// <returns>True if a <see cref="NPCChatConditionalBase"/> with the given <paramref name="name"/>
        /// exists; otherwise false.</returns>
        public static bool ContainsConditional(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            return _instances.ContainsKey(name);
        }

        /// <summary>
        /// When overridden in the derived class, performs the actual conditional evaluation.
        /// </summary>
        /// <param name="user">The User.</param>
        /// <param name="npc">The NPC.</param>
        /// <param name="parameters">The parameters to use. </param>
        /// <returns>True if the conditional returns true for the given <paramref name="user"/>,
        /// <paramref name="npc"/>, and <paramref name="parameters"/>; otherwise false.</returns>
        protected abstract bool DoEvaluate(object user, object npc, NPCChatConditionalParameter[] parameters);

        /// <summary>
        /// Checks the conditional against the given <paramref name="user"/> and <paramref name="npc"/>.
        /// </summary>
        /// <param name="user">The User.</param>
        /// <param name="npc">The NPC.</param>
        /// <param name="parameters">The parameters to use for performing the conditional check.</param>
        /// <returns>True if the conditional returns true for the given <paramref name="user"/>,
        /// <paramref name="npc"/>, and <paramref name="parameters"/>; otherwise false.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="user"/> or <paramref name="npc"/> is null.</exception>
        /// <exception cref="ArgumentException">An invalid number of INPCChatConditionalParameters specified in the
        /// <paramref name="parameters"/>, or one or more of the ValueTypes in the <paramref name="parameters"/>
        /// are not of the correct type.</exception>
        public bool Evaluate(object user, object npc, params NPCChatConditionalParameter[] parameters)
        {
            const string errmsgNumberOfParameters = "Invalid number of parameters. Expected {0}, but was given {1}.";
            const string errmsgValueType = "Invalid ValueType for parameter {0}. Expected `{1}`, but was given `{2}`.";

            // Check for valid arguments
            if (user == null)
                throw new ArgumentNullException("user");

            if (npc == null)
                throw new ArgumentNullException("npc");

            // Use an empty array instead of a null one
            if (parameters == null)
                parameters = _emptyParameters;

            // Check for a valid number of parameters
            if (parameters.Length != _parameterTypes.Length)
            {
                var err = string.Format(errmsgNumberOfParameters, _parameterTypes.Length, parameters.Length);
                throw new ArgumentException(err, "parameters");
            }

            // Check that the parameters are of the correct type
            for (var i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ValueType != _parameterTypes[i])
                {
                    var err = string.Format(errmsgValueType, i, _parameterTypes[i], parameters[i].ValueType);
                    throw new ArgumentException(err, "parameters");
                }
            }

            // Parameters are all valid, so process the conditional
            return DoEvaluate(user, npc, parameters);
        }

        /// <summary>
        /// Gets the NPCChatConditionalBase with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the NPCChatConditionalBase to get.</param>
        /// <returns>The NPCChatConditionalBase with the given <paramref name="name"/>.</returns>
        public static NPCChatConditionalBase GetConditional(string name)
        {
            return _instances[name];
        }

        /// <summary>
        /// Gets the NPCChatConditionalParameterType for the parameter at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Index of the parameter to get the NPCChatConditionalParameterType for.</param>
        /// <returns>The NPCChatConditionalParameterType for the parameter at the given <paramref name="index"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="index"/> is less than 0 or greater
        /// than ParameterCount.</exception>
        public NPCChatConditionalParameterType GetParameter(int index)
        {
            if (index < 0 || index >= _parameterTypes.Length)
                throw new ArgumentOutOfRangeException("index");

            return _parameterTypes[index];
        }

        /// <summary>
        /// Handles when a new type has been loaded into the <see cref="TypeFactory"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NetGore.Collections.TypeFactoryLoadedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="ArgumentException">The loaded type in <paramref name="e"/> was invalid or already loaded.</exception>
        static void OnLoadTypeHandler(TypeFactory sender, TypeFactoryLoadedEventArgs e)
        {
            var instance = (NPCChatConditionalBase)sender.GetTypeInstance(e.Name);

            // Make sure the name is not already in use
            if (ContainsConditional(instance.Name))
            {
                const string errmsg =
                    "Could not add Type `{0}` - a NPC chat conditional named `{1}` already exists as Type `{2}`.";
                var err = string.Format(errmsg, e.LoadedType, instance.Name, _instances[instance.Name].GetType());
                if (log.IsFatalEnabled)
                    log.Fatal(err);
                Debug.Fail(err);
                throw new ArgumentException(err);
            }

            // Add the value to the Dictionary
            _instances.Add(instance.Name, instance);

            if (log.IsDebugEnabled)
                log.DebugFormat("Loaded NPC chat conditional `{0}` from Type `{1}`.", instance.Name, e.LoadedType);
        }

        /// <summary>
        /// Tries to get the <see cref="NPCChatConditionalBase"/> with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="NPCChatConditionalBase"/> to get.</param>
        /// <param name="value">If the method returns true, contains the <see cref="NPCChatConditionalBase"/>
        /// with the given <paramref name="name"/>.</param>
        /// <returns>True if the <paramref name="value"/> was successfully acquired; otherwise false.</returns>
        public static bool TryGetResponseAction(string name, out NPCChatConditionalBase value)
        {
            return _instances.TryGetValue(name, out value);
        }
    }
}