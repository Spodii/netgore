using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Class containing all of the messages for the correspoding GameMessage.
    /// </summary>
    public class GameMessageCollection : MessageCollectionBase<GameMessage>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Name of the default language to use. Messages that do not exist in other languages will be loaded from the
        /// default language. That way, you get at least some message, even if it is in the wrong language.
        /// </summary>
        const string _defaultLanguageName = "English";

        /// <summary>
        /// Suffix for the language files.
        /// </summary>
        const string _languageFileSuffix = ".txt";

        const string _tempLanguageName = "TEMPORARY_COMPILATION_TEST_LANGUAGE";

        /// <summary>
        /// Synchronization object for the <see cref="_currentLanguage"/>.
        /// </summary>
        static readonly object _currentLanguageSync = new object();

        /// <summary>
        /// The <see cref="GameMessageCollection"/> instance for the default language.
        /// </summary>
        static readonly GameMessageCollection _defaultMessages;

        /// <summary>
        /// Contains the instances of the <see cref="GameMessageCollection"/> indexed by language.
        /// </summary>
        static readonly Dictionary<string, GameMessageCollection> _instances;

        /// <summary>
        /// Synchronization object for the <see cref="_instances"/>.
        /// </summary>
        static readonly object _instancesSync = new object();

        static GameMessageCollection _currentLanguage;

        readonly string _language;
        readonly bool _rawMessagesOnly;

        /// <summary>
        /// Initializes the <see cref="GameMessageCollection"/> class.
        /// </summary>
        static GameMessageCollection()
        {
            _instances = new Dictionary<string, GameMessageCollection>(StringComparer.OrdinalIgnoreCase);
            _defaultMessages = Create();
            _currentLanguage = _defaultMessages;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameMessageCollection"/> class.
        /// </summary>
        /// <param name="language">Name of the language to load.</param>
        /// <param name="rawMessagesOnly">If true, only the raw messages will be loaded, and invoking the messages
        /// will not be supported.</param>
        GameMessageCollection(string language, bool rawMessagesOnly)
            : base(GetLanguageFile(ContentPaths.Build, language), _defaultMessages)
        {
            _rawMessagesOnly = rawMessagesOnly;
            _language = language;

            // Ensure we have all the messages loaded
            var missingKeys = EnumHelper<GameMessage>.Values.Except(this.Select(y => y.Key));
            if (!missingKeys.IsEmpty())
            {
                // One or more keys are missing
                if (StringComparer.OrdinalIgnoreCase.Equals(_defaultLanguageName, language))
                {
                    // Key(s) are missing from the default language, which is very bad
                    const string errmsg =
                        "GameMessages `{0}` for language `{1}` did not contain all GameMessages." +
                        " The default language needs all keys! Missing the following keys: {2}";
                    var err = string.Format(errmsg, this, _language, missingKeys.Implode());
                    if (log.IsErrorEnabled)
                        log.Error(err);
                    Debug.Fail(err);
                }
                else
                {
                    // Key(s) are missing from a non-default language, which isn't too bad since we can fall back on the default language
                    const string errmsg =
                        "GameMessages `{0}` for language `{1}` did not contain all GameMessages." +
                        " Will have to use the text from the default language `{3}` instead. Missing the following keys: {2}";
                    var err = string.Format(errmsg, this, _language, missingKeys.Implode(), _defaultLanguageName);
                    if (log.IsErrorEnabled)
                        log.Error(err);
                }
            }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="GameMessageCollection.CurrentLanguage"/> property has changed.
        /// </summary>
        public static event EventHandler<ValueChangedEventArgs<GameMessageCollection>> CurrentLanguageChanged;

        /// <summary>
        /// Gets the <see cref="GameMessageCollection"/> instance for the current language. It is highly recommended that you do not
        /// cache this object locally, and instead refer to this property whenever accessing the object. This way, you will not
        /// have to worry about whether or not your are using the <see cref="GameMessageCollection"/> for the correct language.
        /// This value will only change by calling <see cref="TryChangeCurrentLanguage"/>.
        /// </summary>
        public static GameMessageCollection CurrentLanguage
        {
            get { return _currentLanguage; }
        }

        /// <summary>
        /// Gets the file path to the global JScript file included in all generated <see cref="GameMessageCollection"/>s.
        /// </summary>
        public static string GlobalJScriptFilePath
        {
            get { return ContentPaths.Build.Languages.Join("global.js"); }
        }

        /// <summary>
        /// Gets the name of this language.
        /// </summary>
        public string Language
        {
            get { return _language; }
        }

        /// <summary>
        /// Gets the <see cref="GameMessageCollection"/> for the specified language.
        /// </summary>
        /// <param name="language">The game message language.</param>
        /// <returns>The <see cref="GameMessageCollection"/> for the specified language.</returns>
        public static GameMessageCollection Create(string language = _defaultLanguageName)
        {
            GameMessageCollection instance;

            lock (_instancesSync)
            {
                if (!_instances.TryGetValue(language, out instance))
                {
                    instance = new GameMessageCollection(language, false);
                    _instances.Add(language, instance);
                }
            }

            return instance;
        }

        /// <summary>
        /// Deletes the files for a language.
        /// </summary>
        /// <param name="language">The language to delete the files for.</param>
        public static void DeleteLanguageFiles(string language)
        {
            var file = GetLanguageFile(ContentPaths.Dev, language);

            // Delete the language messages file
            try
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
            catch (IOException ex)
            {
                const string errmsg = "Failed to delete language file `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, file, ex);
            }
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
        /// Gets the <see cref="GameMessageCollection"/> file for a certain language.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to get the path for.</param>
        /// <param name="language">The language to get the file for.</param>
        /// <returns>The <see cref="GameMessageCollection"/> file for the <paramref name="language"/>.</returns>
        public static string GetLanguageFile(ContentPaths contentPath, string language)
        {
            return contentPath.Languages.Join(language.ToLower() + _languageFileSuffix);
        }

        /// <summary>
        /// Gets the names of all of the available languages.
        /// </summary>
        /// <returns>The names of all of the available languages.</returns>
        public static IEnumerable<string> GetLanguages()
        {
            var comp = StringComparer.OrdinalIgnoreCase;

            var dir = ContentPaths.Build.Languages;
            var filePaths = Directory.GetFiles(dir, "*" + _languageFileSuffix, SearchOption.TopDirectoryOnly);

            var files = filePaths.Select(Path.GetFileNameWithoutExtension);
            files = files.Distinct(comp);
            files = files.OrderBy(x => x, comp);

            return files.ToImmutable();
        }

        /// <summary>
        /// Loads the raw <see cref="GameMessage"/>s for a language.
        /// </summary>
        /// <param name="language">The language to load the messages for.</param>
        /// <returns>The raw <see cref="GameMessage"/>s for the <paramref name="language"/>.</returns>
        public static IEnumerable<KeyValuePair<GameMessage, string>> LoadRawMessages(string language)
        {
            var coll = new GameMessageCollection(language, true);
            var messages = coll.ToImmutable();
            return messages;
        }

        /// <summary>
        /// Writes the raw <see cref="GameMessage"/>s to file.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="messages">The messages.</param>
        public static void SaveRawMessages(string language, IEnumerable<KeyValuePair<GameMessage, string>> messages)
        {
            var sb = new StringBuilder(2048);
            foreach (var msg in messages)
            {
                sb.AppendLine(msg.Key + ": " + msg.Value);
            }

            File.WriteAllText(GetLanguageFile(ContentPaths.Dev, language), sb.ToString());
        }

        /// <summary>
        /// Tests if the <see cref="GameMessageCollection"/> for a certain language exists and can compile
        /// successfully without error.
        /// </summary>
        /// <param name="language">The language to try to compile.</param>
        /// <param name="errors">When this method returns false, contains the compilation errors.</param>
        /// <returns>
        /// True if the <paramref name="language"/>'s <see cref="GameMessageCollection"/> compiled
        /// successfully; otherwise false.
        /// </returns>
        public static bool TestCompilation(string language, out IEnumerable<CompilerError> errors)
        {
            var coll = new GameMessageCollection(language, true);
            errors = coll.CompilationErrors;
            return coll.CompilationErrors.IsEmpty();
        }

        /// <summary>
        /// Tests if the <see cref="GameMessageCollection"/> for a certain language exists and can compile
        /// successfully without error.
        /// </summary>
        /// <param name="language">The language to try to compile.</param>
        /// <param name="errors">When this method returns false, contains the compilation errors as a string.</param>
        /// <returns>
        /// True if the <paramref name="language"/>'s <see cref="GameMessageCollection"/> compiled
        /// successfully; otherwise false.
        /// </returns>
        public static bool TestCompilation(string language, out string errors)
        {
            errors = string.Empty;

            IEnumerable<CompilerError> cerrors;
            var ret = TestCompilation(language, out cerrors);

            if (!ret)
            {
                var sb = new StringBuilder();
                sb.AppendLine("The following errors have caused the compilation to fail:");
                foreach (var e in cerrors)
                {
                    sb.AppendLine(e.ErrorNumber + ": " + e.ErrorText);
                }
                errors = sb.ToString();
            }

            return ret;
        }

        /// <summary>
        /// Tests if the <see cref="GameMessageCollection"/> for a certain language exists and can compile
        /// successfully without error.
        /// </summary>
        /// <param name="messages">The messages to try to compile.</param>
        /// <param name="errors">When this method returns false, contains the compilation errors as a string.</param>
        /// <returns>
        /// True if the <paramref name="messages"/>s compiled successfully; otherwise false.
        /// </returns>
        public static bool TestCompilation(IEnumerable<KeyValuePair<GameMessage, string>> messages, out string errors)
        {
            DeleteLanguageFiles(_tempLanguageName);

            bool success;
            try
            {
                SaveRawMessages(_tempLanguageName, messages);

                success = TestCompilation(_tempLanguageName, out errors);
            }
            finally
            {
                DeleteLanguageFiles(_tempLanguageName);
            }

            return success;
        }

        /// <summary>
        /// Tests if the <see cref="GameMessageCollection"/> for a certain language exists and can compile
        /// successfully without error.
        /// </summary>
        /// <param name="messages">The messages to try to compile.</param>
        /// <param name="errors">When this method returns false, contains the compilation errors.</param>
        /// <returns>
        /// True if the <paramref name="messages"/>s compiled successfully; otherwise false.
        /// </returns>
        public static bool TestCompilation(IEnumerable<KeyValuePair<GameMessage, string>> messages, out IEnumerable<CompilerError> errors)
        {
            bool success;
            try
            {
                SaveRawMessages(_tempLanguageName, messages);

                success = TestCompilation(_tempLanguageName, out errors);
            }
            finally
            {
                var langFile = GetLanguageFile(ContentPaths.Dev, _tempLanguageName);
                if (File.Exists(langFile))
                    File.Delete(langFile);
            }

            return success;
        }

        /// <summary>
        /// Attempts to change the <see cref="CurrentLanguage"/> to a new language.
        /// </summary>
        /// <param name="newLanguage">The name of the language to change to.</param>
        /// <returns>True if the language was successfully changed; false if the language was already set to the <paramref name="newLanguage"/>
        /// or the <paramref name="newLanguage"/> does not exist or is invalid.</returns>
        public static bool TryChangeCurrentLanguage(string newLanguage)
        {
            if (string.IsNullOrEmpty(newLanguage))
            {
                Debug.Fail("Invalid newLanguage value.");
                return false;
            }

            GameMessageCollection newLanguageCollection;

            // Try to create the GameMessageCollection for the new language
            try
            {
                newLanguageCollection = Create(newLanguage);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to change language to `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, newLanguage, ex);
                Debug.Fail(string.Format(errmsg, newLanguage, ex));
                return false;
            }

            // Change to the new language
            GameMessageCollection oldLanguage;
            lock (_currentLanguageSync)
            {
                // Check if we are just changing to the same language
                if (_currentLanguage == newLanguageCollection)
                    return false;

                oldLanguage = _currentLanguage;
                _currentLanguage = newLanguageCollection;
            }

            // Raise the event
            if (CurrentLanguageChanged != null)
                CurrentLanguageChanged.Raise(null, ValueChangedEventArgs.Create(oldLanguage, _currentLanguage));

            return true;
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