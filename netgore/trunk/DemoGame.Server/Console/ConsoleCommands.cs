using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            bool success = UserAccount.TryAddCharacter(DbController, accountName, userName, out errorMsg);

            if (!success)
                return "User creation failed: " + errorMsg;
            else
                return "User creation successful.";
        }

        static string BuildString(IEnumerable<string> strings, string delimiter)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in strings)
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

            int result = DbController.GetQuery<CountAccountCharactersByNameQuery>().Execute(accountName);

            return string.Format("There are {0} characters in account {1}.", result, accountName);
        }

        [ConsoleCommand("CountAccountCharacters")]
        public string CountAccountCharacters(int id)
        {
            AccountID accountID = new AccountID(id);

            int result = DbController.GetQuery<CountAccountCharactersByIDQuery>().Execute(accountID);

            return string.Format("There are {0} characters in account ID {1}.", result, accountID);
        }

        [ConsoleCommand("CreateAccount")]
        public string CreateAccount(string accountName, string accountPassword, string email)
        {
            if (!GameData.AccountName.IsValid(accountName))
                return "Invalid account name.";

            if (!GameData.AccountPassword.IsValid(accountPassword))
                return "Invalid account password.";

            if (!GameData.AccountEmail.IsValid(email))
                return "Invalid email address.";

            AccountID accountID;
            bool success = UserAccount.TryCreateAccount(DbController, null, accountName, accountPassword, email, out accountID);

            if (success)
                return string.Format("Created account `{0}` with ID `{1}`.", accountName, accountID);
            else
                return "Failed to create new account. Make sure the name is available.";
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
            ItemEntity item = FindItem(id, out source);

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
            foreach (Map map in Server.World.Maps)
            {
                foreach (ItemEntity item in map.DynamicEntities.OfType<ItemEntity>())
                {
                    if (item.ID == id)
                    {
                        source = map;
                        return item;
                    }
                }

                foreach (Character character in map.DynamicEntities.OfType<Character>())
                {
                    foreach (ItemEntity item in character.Equipped.Select(x => x.Value))
                    {
                        if (item.ID == id)
                        {
                            source = character.Equipped;
                            return item;
                        }
                    }

                    foreach (ItemEntity item in character.Inventory.Select(x => x.Value))
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
            string s = c.ToString();
            s += "\t Map: ";
            if (c.Map != null)
                s += c.Map.Index;
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
            StringBuilder sb = new StringBuilder();
            var cmdsSorted = _parser.GetCommands().OrderBy(x => x.Key);

            sb.AppendLine("Server console commands:");

            foreach (var cmd in cmdsSorted)
            {
                sb.Append(" * ");
                sb.Append(cmd.Key);
                sb.Append("(");

                var first = cmd.Value.FirstOrDefault();
                if (first != null)
                    sb.Append(ConsoleCommandParser.GetParameterInfo(first));

                sb.Append(")");

                int count = cmd.Value.Count();
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
            string userInfo = BuildString(users.Select(x => GetCharacterInfoShort(x)), Environment.NewLine);

            return GetCommandHeader("Total Users: {0}", users.Count()) + userInfo;
        }

        class ConsoleCommandParser : StringCommandParser<ConsoleCommandAttribute>
        {
            public ConsoleCommandParser() : base(typeof(ConsoleCommands))
            {
            }
        }
    }
}