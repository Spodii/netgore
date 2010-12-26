using System;
using System.Linq;
using System.Text;
using DemoGame.Server.AI;
using DemoGame.Server.Queries;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server
{
    /// <summary>
    /// Handles commands entered directly into the server through the server's console.
    /// </summary>
    class ConsoleCommands
    {
        const string _separator = "-------------------";
        static readonly string _newLine = Environment.NewLine;
        readonly ConsoleCommandParser _parser = new ConsoleCommandParser();
        readonly Server _server;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleCommands"/> class.
        /// </summary>
        /// <param name="server">The server.</param>
        public ConsoleCommands(Server server)
        {
            _server = server;
        }

        /// <summary>
        /// Gets the <see cref="IDbController"/> instance.
        /// </summary>
        public IDbController DbController
        {
            get { return Server.DbController; }
        }

        /// <summary>
        /// Gets the <see cref="Server"/> instance.
        /// </summary>
        public Server Server
        {
            get { return _server; }
        }

        /// <summary>
        /// Counts the number of characters in an account.
        /// </summary>
        /// <param name="accountName">The name of the account.</param>
        /// <returns>The number of characters in the account.</returns>
        [ConsoleCommand("CountAccountCharacters")]
        public string CountAccountCharacters(string accountName)
        {
            if (!GameData.AccountName.IsValid(accountName))
                return "Invalid account name";

            var accountID = DbController.GetQuery<SelectAccountIDFromNameQuery>().Execute(accountName);
            if (!accountID.HasValue)
                return string.Format("No account with the name `{0}` exists.", accountName);

            return CountAccountCharacters((int)accountID.Value);
        }

        /// <summary>
        /// Counts the number of characters in an account.
        /// </summary>
        /// <param name="id">The ID of the account.</param>
        /// <returns>The number of characters in the account.</returns>
        [ConsoleCommand("CountAccountCharacters")]
        public string CountAccountCharacters(int id)
        {
            var accountID = new AccountID(id);

            var result = DbController.GetQuery<CountAccountCharactersByIDQuery>().TryExecute(accountID);

            if (!result.HasValue)
            {
                // Invalid account
                return string.Format("Account ID `{0}` does not exist.", id);
            }
            else
            {
                // Valid account
                return string.Format("There are {0} characters in account ID {1}.", result.Value, accountID);
            }
        }

        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="accountName">The account name.</param>
        /// <param name="accountPassword">The account password.</param>
        /// <param name="email">The account email address.</param>
        /// <returns>The results of the operation.</returns>
        [ConsoleCommand("CreateAccount")]
        public string CreateAccount(string accountName, string accountPassword, string email)
        {
            GameMessage failReason;
            var success = Server.UserAccountManager.TryCreateAccount(null, accountName, accountPassword, email, out failReason);

            if (success)
                return string.Format("Created account `{0}`.", accountName);
            else
                return "Failed to create new account: " + failReason;
        }

        /// <summary>
        /// Creates a user on an account.
        /// </summary>
        /// <param name="accountName">The account to add the user to.</param>
        /// <param name="userName">The name of the user to create.</param>
        /// <returns>The results of the operation.</returns>
        [ConsoleCommand("CreateAccountUser")]
        public string CreateAccountUser(string accountName, string userName)
        {
            string errorMsg;
            if (!Server.UserAccountManager.TryAddCharacter(accountName, userName, out errorMsg))
                return string.Format("Failed to create character `{0}` on account `{1}`: {2}", userName, accountName, errorMsg);

            return "Character successfully added to account.";
        }

        /// <summary>
        /// Tries to execute a command.
        /// </summary>
        /// <param name="commandString">The command string to execute.</param>
        /// <returns>The </returns>
        public string ExecuteCommand(string commandString)
        {
            ThreadAsserts.IsMainThread();

            string result;
            if (!_parser.TryParse(this, commandString, out result))
            {
                if (string.IsNullOrEmpty(result))
                {
                    const string errmsg = "Failed to execute command string: {0}";
                    result = string.Format(errmsg, commandString);
                }
            }

            return result;
        }

        /// <summary>
        /// Finds where a live item resides.
        /// </summary>
        /// <param name="itemID">The ID of the item to search for.</param>
        /// <returns>The location of the live item, or null if not found.</returns>
        [ConsoleCommand("FindItem")]
        public string FindItem(string itemID)
        {
            ItemID id;
            if (!Parser.Current.TryParse(itemID, out id))
                return string.Format("Invalid ItemID `{0}`.", id);

            object source;
            var item = FindItem(id, out source);

            if (item == null)
                return "Item not found.";

            if (source is Map)
                return string.Format("Item `{0}` is on Map `{1}` at `{2}`.", item, source, item.Position);
            else if (source is CharacterEquipped)
                return string.Format("Item `{0}` is equipped by Character `{1}`.", item, ((CharacterEquipped)source).Character);
            else if (source is CharacterInventory)
                return string.Format("Item `{0}` is in the inventory of Character `{1}`.", item,
                    ((CharacterInventory)source).Character);
            else
                return string.Format("Item `{0}` found at unknown source `{1}`.", item, source);
        }

        /// <summary>
        /// Finds an item in the world.
        /// </summary>
        /// <param name="id">The ID of the item to find.</param>
        /// <param name="source">When this method returns a non-null value, contains the object that was holding the item.</param>
        /// <returns>The <see cref="ItemEntity"/> for the given <paramref name="id"/>; otherwise null.</returns>
        public ItemEntity FindItem(ItemID id, out object source)
        {
            // Search all maps in the world
            foreach (var map in Server.World.Maps)
            {
                // Check if the item is on the map itself
                foreach (var item in map.DynamicEntities.OfType<ItemEntity>().Where(item => item.ID == id))
                {
                    source = map;
                    return item;
                }

                // Check for the item in the inventory of characters
                foreach (var character in map.DynamicEntities.OfType<Character>())
                {
                    // Check the equipment
                    foreach (var item in character.Equipped.Select(x => x.Value).Where(item => item.ID == id))
                    {
                        source = character.Equipped;
                        return item;
                    }

                    // Check the inventory
                    foreach (var item in character.Inventory.Select(x => x.Value).Where(item => item.ID == id))
                    {
                        source = character.Inventory;
                        return item;
                    }
                }
            }

            source = null;
            return null;
        }

        /// <summary>
        /// Gets the ID of an account.
        /// </summary>
        /// <param name="accountName">The name of the account.</param>
        /// <returns>The ID of the account.</returns>
        [ConsoleCommand("GetAccountID")]
        public string GetAccountID(string accountName)
        {
            if (!GameData.AccountName.IsValid(accountName))
                return "Invalid account name.";

            AccountID accountID;
            if (!Server.UserAccountManager.TryGetAccountID(accountName, out accountID))
                return string.Format("Account {0} does not exist.", accountName);
            else
                return string.Format("Account {0} has the ID {1}.", accountName, accountID);
        }

        /// <summary>
        /// Gets the basic spatial information for a <see cref="Character"/>.
        /// </summary>
        /// <param name="c">The <see cref="Character"/>.</param>
        /// <returns>The information for the <paramref name="c"/> as a string.</returns>
        static string GetCharacterInfoShort(Character c)
        {
            var sb = new StringBuilder();

            sb.Append(c.ToString());
            sb.Append("\t Map: ");

            if (c.Map != null)
                sb.Append(c.Map);
            else
                sb.Append("[null]");

            sb.Append(" @ ");
            sb.Append(c.Position);

            return sb.ToString();
        }

        /// <summary>
        /// Gets the header string for a command.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The string to display.</returns>
        static string GetCommandHeader(string header, params object[] args)
        {
            return _separator + _newLine + string.Format(header, args) + _newLine + _separator + _newLine;
        }

        /// <summary>
        /// Displays the console command help.
        /// </summary>
        /// <returns>The console command help.</returns>
        [ConsoleCommand("Help")]
        public string Help()
        {
            var sb = new StringBuilder();
            var cmdsSorted = _parser.GetCommands().OrderBy(x => x.Key);

            sb.AppendLine("Server console commands:");

            foreach (var cmd in cmdsSorted)
            {
                sb.Append(" * ");
                sb.Append(cmd.Key);
                sb.Append("(");

                var first = cmd.Value.Select(x => x.Method).FirstOrDefault();
                if (first != null)
                    sb.Append(StringCommandParser.GetParameterInfo(first));

                sb.Append(")");

                var count = cmd.Value.Count();
                if (count > 1)
                {
                    sb.Append(" [+");
                    sb.Append(count - 1);
                    sb.Append(" overload(s)]");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Terminates the server.
        /// </summary>
        /// <returns>The results of the operation.</returns>
        [ConsoleCommand("Quit")]
        public string Quit()
        {
            _server.Shutdown();
            return "Server shutting down";
        }

        /// <summary>
        /// Shows all of the users that are currently online.
        /// </summary>
        /// <returns>All of the online users.</returns>
        [ConsoleCommand("ShowUsers")]
        public string ShowUsers()
        {
            var users = Server.World.GetUsers();
            var userInfo = users.Select(GetCharacterInfoShort).Implode(Environment.NewLine);

            return GetCommandHeader("Total Users: {0}", users.Count()) + userInfo;
        }

        /// <summary>
        /// Toggles the AI.
        /// </summary>
        /// <returns>The results of the operation.</returns>
        [ConsoleCommand("ToggleAI")]
        public string ToggleAI()
        {
            if (!AISettings.AIDisabled)
            {
                AISettings.AIDisabled = true;
                return "AI has been disabled.";
            }
            else
            {
                AISettings.AIDisabled = false;
                return "AI has been enabled.";
            }
        }

        /// <summary>
        /// Parser for the console commands. The actual handling is done in the <see cref="ConsoleCommands"/> class.
        /// </summary>
        class ConsoleCommandParser : StringCommandParser<ConsoleCommandAttribute>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ConsoleCommandParser"/> class.
            /// </summary>
            public ConsoleCommandParser() : base(typeof(ConsoleCommands))
            {
            }
        }
    }
}