using System.Linq;
using DemoGame;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Class containing all of the messages for the correspoding GameMessage.
    /// </summary>
    public class GameMessages : MessageCollectionBase<GameMessage>
    {
        /// <summary>
        /// Name of the default language to use.
        /// </summary>
        const string _defaultLanguageName = "English";

        /// <summary>
        /// Suffix for the language files.
        /// </summary>
        const string _languageFileSuffix = ".txt";

        /// <summary>
        /// Reference to the GameMessages of the default language.
        /// </summary>
        static readonly GameMessages _defaultMessages = new GameMessages();

        readonly string _language;

        /// <summary>
        /// Gets the name of this language.
        /// </summary>
        public string Language
        {
            get { return _language; }
        }

        /// <summary>
        /// GameMessages constructor.
        /// </summary>
        public GameMessages() : this(_defaultLanguageName)
        {
        }

        /// <summary>
        /// GameMessages constructor.
        /// </summary>
        /// <param name="language">Name of the language to load.</param>
        public GameMessages(string language)
            : base(ContentPaths.Build.Languages.Join(language.ToLower() + _languageFileSuffix), _defaultMessages)
        {
            _language = language;
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