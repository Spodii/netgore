using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;

namespace NetGore.NPCChat
{
    /// <summary>
    /// The base class for a conditional used in the NPC chatting. Each instanceable derived class
    /// must include a parameterless constructor (preferably private).
    /// </summary>
    public abstract class NPCChatConditionalBase
    {
        /// <summary>
        /// Array used for an empty set of INPCChatConditionalParameters.
        /// </summary>
        static readonly NPCChatConditionalParameter[] _emptyParameters = new NPCChatConditionalParameter[0];

        /// <summary>
        /// Array used for an empty set of NPCChatConditionalParameterTypes.
        /// </summary>
        static readonly NPCChatConditionalParameterType[] _emptyParameterTypes = new NPCChatConditionalParameterType[0];

        /// <summary>
        /// Dictionary that contains the NPCChatConditionalBase instance of each derived class, with the Name as the key.
        /// </summary>
        static readonly Dictionary<string, NPCChatConditionalBase> _instances =
            new Dictionary<string, NPCChatConditionalBase>(_nameComparer);

        /// <summary>
        /// StringComparer used for the Name.
        /// </summary>
        static readonly StringComparer _nameComparer = StringComparer.Ordinal;

        static readonly FactoryTypeCollection _typeCollection;
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly string _name;
        readonly NPCChatConditionalParameterType[] _parameterTypes;

        /// <summary>
        /// Gets an IEnumerable of the NPCChatConditionalBases.
        /// </summary>
        public static IEnumerable<NPCChatConditionalBase> Conditionals
        {
            get { return _instances.Values; }
        }

        /// <summary>
        /// Gets the unique name for this INPCChatConditional. This string is case-sensitive.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the number of parameters required by this NPCChatConditionalBase.
        /// </summary>
        public int ParameterCount
        {
            get { return _parameterTypes.Length; }
        }

        /// <summary>
        /// Gets an IEnumerable of the NPCChatConditionalParameterTypes used in this NPCChatConditionalBase.
        /// </summary>
        public IEnumerable<NPCChatConditionalParameterType> ParameterTypes
        {
            get { return _parameterTypes; }
        }

        /// <summary>
        /// Initializes the <see cref="NPCChatConditionalBase"/> class.
        /// </summary>
        static NPCChatConditionalBase()
        {
            var filter = FactoryTypeCollection.CreateFilter(typeof(NPCChatConditionalBase), true, Type.EmptyTypes);
            _typeCollection = new FactoryTypeCollection(filter, OnLoadTypeHandler, false);
            _typeCollection.BeginLoading();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatConditionalBase"/> class.
        /// </summary>
        /// <param name="name">The unique display name of this NPCChatConditionalBase. This name must be unique
        /// for each derived class type. This string is case-sensitive.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        protected NPCChatConditionalBase(string name, params NPCChatConditionalParameterType[] parameterTypes)
        {
            if (parameterTypes == null || parameterTypes.Length == 0)
                parameterTypes = _emptyParameterTypes;

            _name = name;
            _parameterTypes = parameterTypes;
        }

        /// <summary>
        /// Gets if a NPCChatConditionalBase with the given <paramref name="name"/> exists.
        /// </summary>
        /// <param name="name">The name of the NPCChatConditionalBase.</param>
        /// <returns>True if a NPCChatConditionalBase with the given <paramref name="name"/> exists; otherwise false.</returns>
        public static bool ContainsConditional(string name)
        {
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
                string err = string.Format(errmsgNumberOfParameters, _parameterTypes.Length, parameters.Length);
                throw new ArgumentException(err, "parameters");
            }

            // Check that the parameters are of the correct type
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ValueType != _parameterTypes[i])
                {
                    string err = string.Format(errmsgValueType, i, _parameterTypes[i], parameters[i].ValueType);
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
        /// Handles when a new type has been loaded into a FactoryTypeCollection.
        /// </summary>
        /// <param name="factoryTypeCollection">FactoryTypeCollection that the event occured on.</param>
        /// <param name="loadedType">Type that was loaded.</param>
        /// <param name="name">Name of the Type.</param>
        static void OnLoadTypeHandler(FactoryTypeCollection factoryTypeCollection, Type loadedType, string name)
        {
            NPCChatConditionalBase instance = (NPCChatConditionalBase)_typeCollection.GetTypeInstance(name);

            // Make sure the name is not already in use
            if (ContainsConditional(instance.Name))
            {
                const string errmsg =
                    "Could not add Type `{0}` - a NPC chat conditional named `{1}` already exists as Type `{2}`.";
                string err = string.Format(errmsg, loadedType, instance.Name, _instances[instance.Name].GetType());
                if (log.IsFatalEnabled)
                    log.Fatal(err);
                Debug.Fail(err);
                throw new Exception(err);
            }

            // Add the value to the Dictionary
            _instances.Add(instance.Name, instance);

            if (log.IsInfoEnabled)
                log.InfoFormat("Loaded NPC chat conditional `{0}` from Type `{1}`.", instance.Name, loadedType);
        }
    }
}