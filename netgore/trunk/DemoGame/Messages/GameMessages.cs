using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Class containing all of the messages for the correspoding GameMessage.
    /// </summary>
    public class GameMessages : MessageCollectionBase<GameMessage>
    {
        // TODO: Rename "GameMessages" to "GameMessageManager"

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Name of the default language to use.
        /// </summary>
        const string _defaultLanguageName = "English";

        /// <summary>
        /// Suffix for the language files.
        /// </summary>
        const string _languageFileSuffix = ".txt";

        /// <summary>
        /// The <see cref="GameMessages"/> instance for the default language.
        /// </summary>
        static readonly GameMessages _defaultMessages;

        /// <summary>
        /// Contains the instances of the <see cref="GameMessages"/> indexed by language.
        /// </summary>
        static readonly Dictionary<string, GameMessages> _instances;

        readonly string _language;

        /// <summary>
        /// Initializes the <see cref="GameMessages"/> class.
        /// </summary>
        static GameMessages()
        {
            _instances = new Dictionary<string, GameMessages>(StringComparer.OrdinalIgnoreCase);
            _defaultMessages = Create();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameMessages"/> class.
        /// </summary>
        /// <param name="language">Name of the language to load.</param>
        GameMessages(string language)
            : base(ContentPaths.Build.Languages.Join(language.ToLower() + _languageFileSuffix), _defaultMessages)
        {
            _language = language;

            // Ensure we have all the messages loaded
            var missingKeys = EnumHelper<GameMessage>.Values.Except(this.Select(y => y.Key));
            if (missingKeys.Count() > 0)
            {
                const string errmsg = "GameMessages `{0}` for language `{1}` did not contain all GameMessages. Missing keys: {2}";
                string err = string.Format(errmsg, this, _language, missingKeys.Implode());
                if (log.IsErrorEnabled)
                    log.Error(err);
                Debug.Fail(err);
            }
        }

        /// <summary>
        /// Gets the name of this language.
        /// </summary>
        public string Language
        {
            get { return _language; }
        }

        /// <summary>
        /// Gets the <see cref="GameMessages"/> for the default language.
        /// </summary>
        /// <returns>The <see cref="GameMessages"/> for the default language.</returns>
        public static GameMessages Create()
        {
            return Create(_defaultLanguageName);
        }

        /// <summary>
        /// Gets the <see cref="GameMessages"/> for the specified language.
        /// </summary>
        /// <param name="language">The game message language.</param>
        /// <returns>The <see cref="GameMessages"/> for the specified language.</returns>
        public static GameMessages Create(string language)
        {
            GameMessages instance;
            if (!_instances.TryGetValue(language, out instance))
            {
                instance = new GameMessages(language);
                _instances.Add(language, instance);
            }

            return instance;
        }

        /// <summary>
        /// Gets the IEqualityComparer to use for collections created by this collection.
        /// </summary>
        /// <returns>
        /// The IEqualityComparer to use for collections created by this collection.
        /// </returns>
        protected override IEqualityComparer<GameMessage> GetEqualityComparer()
        {
            return EnumComparer<GameMessage>.Instance;
        }

        /// <summary>
        /// When overridden in the derived class, tries to parse a string to get the ID.
        /// </summary>
        /// <param name="str">String to parse.</param>
        /// <param name="id">Parsed ID.</param>
        /// <returns>True if the ID was parsed successfully, else false.</returns>
        protected override bool TryParseID(string str, out GameMessage id)
        {
            return ParseEnumHelper(str, out id);
        }
    }
}