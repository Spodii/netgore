using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoGame.AI;
using DemoGame.Server.Queries;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server
{
    /// <summary>
    /// Handles commands entered into the console.
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

        public IDbController DbController
        {
            get { return Server.DbController; }
        }

        public Server Server
        {
            get { return _server; }
        }

        [ConsoleCommand("AddUser")]
        public string AddUser(string userName, string accountName)
        {
            string errorMsg;
            var success = UserAccount.TryAddCharacter(DbController, accountName, userName, out errorMsg);

            if (!success)
                return "User creation failed: " + errorMsg;
            else
                return "User creation successful.";
        }

        static string BuildString(IEnumerable<string> strings, string delimiter)
        {
            var sb = new StringBuilder();
            foreach (var s in strings)
            {
                sb.Append(s);
                sb.Append(delimiter);
            }
            return sb.ToString();
        }

        [ConsoleCommand("CountAccountCharacters")]
        public string CountAccountCharacters(string accountName)
        {
            if (!GameData.AccountName.IsValid(accountName))
                return "Invalid account name";

            var result = DbController.GetQuery<CountAccountCharactersByNameQuery>().Execute(accountName);

            return string.Format("There are {0} characters in account {1}.", result, accountName);
        }

        [ConsoleCommand("CountAccountCharacters")]
        public string CountAccountCharacters(int id)
        {
            var accountID = new AccountID(id);

            var result = DbController.GetQuery<CountAccountCharactersByIDQuery>().Execute(accountID);

            return string.Format("There are {0} characters in account ID {1}.", result, accountID);
        }

        [ConsoleCommand("CreateAccount")]
        public string CreateAccount(string accountName, string accountPassword, string email)
        {
            AccountID accountID;
            string errorMessage;
            var success = UserAccount.TryCreateAccount(DbController, null, accountName, accountPassword, email, out accountID,
                                                       out errorMessage);

            if (success)
                return string.Format("Created account `{0}` with ID `{1}`.", accountName, accountID);
            else
                return "Failed to create new account: " + errorMessage;
        }

        [ConsoleCommand("CreateAccountUser")]
        public string CreateAccountUser(string accountName, string userName)
        {
            string errorMsg;
            if (!UserAccount.TryAddCharacter(DbController, accountName, userName, out errorMsg))
                return string.Format("Failed to create character `{0}` on account `{1}`: {2}", userName, accountName, errorMsg);

            return "Character successfully added to account.";
        }

        public string ExecuteCommand(string commandString)
        {
            ThreadAsserts.IsMainThread();

            string result;
            _parser.TryParse(this, commandString, out result);

            return result;
        }

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

        public ItemEntity FindItem(ItemID id, out object source)
        {
            foreach (var map in Server.World.Maps)
            {
                foreach (var item in map.DynamicEntities.OfType<ItemEntity>())
                {
                    if (item.ID == id)
                    {
                        source = map;
                        return item;
                    }
                }

                foreach (var character in map.DynamicEntities.OfType<Character>())
                {
                    foreach (var item in character.Equipped.Select(x => x.Value))
                    {
                        if (item.ID == id)
                        {
                            source = character.Equipped;
                            return item;
                        }
                    }

                    foreach (var item in character.Inventory.Select(x => x.Value))
                    {
                        if (item.ID == id)
                        {
                            source = character.Inventory;
                            return item;
                        }
                    }
                }
            }

            source = null;
            return null;
        }

        [ConsoleCommand("GetAccountID")]
        public string GetAccountID(string accountName)
        {
            if (!GameData.AccountName.IsValid(accountName))
                return "Invalid account name.";

            AccountID accountID;
            if (!UserAccount.TryGetAccountID(DbController, accountName, out accountID))
                return string.Format("Account {0} does not exist.", accountName);
            else
                return string.Format("Account {0} has the ID {1}.", accountName, accountID);
        }

        static string GetCharacterInfoShort(Character c)
        {
            var s = c.ToString();
            s += "\t Map: ";
            if (c.Map != null)
                s += c.Map.ID;
            else
                s += "null";

            s += " @ ";
            s += c.Position;
            return s;
        }

        static string GetCommandHeader(string header, params object[] args)
        {
            return _separator + _newLine + string.Format(header, args) + _newLine + _separator + _newLine;
        }

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

                var first = cmd.Value.FirstOrDefault();
                if (first != null)
                    sb.Append(StringCommandParser<ConsoleCommandAttribute>.GetParameterInfo(first));

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

        [ConsoleCommand("Quit")]
        public string Quit()
        {
            _server.Shutdown();
            return "Server shutting down";
        }

        [ConsoleCommand("ShowUsers")]
        public string ShowUsers()
        {
            var users = Server.World.GetUsers();
            var userInfo = BuildString(users.Select(GetCharacterInfoShort), Environment.NewLine);

            return GetCommandHeader("Total Users: {0}", users.Count()) + userInfo;
        }

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

        class ConsoleCommandParser : StringCommandParser<ConsoleCommandAttribute>
        {
            public ConsoleCommandParser() : base(typeof(ConsoleCommands))
            {
            }
        }
    }
}