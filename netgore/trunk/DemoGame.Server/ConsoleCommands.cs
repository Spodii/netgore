using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.Queries;
using NetGore;

namespace DemoGame.Server
{
    class ConsoleCommands
    {
        class ConsoleCommandParser : StringCommandParser<ConsoleCommandAttribute>
        {
            public ConsoleCommandParser() : base(typeof(ConsoleCommands))
            {
            }
        }

        readonly ConsoleCommandParser _parser = new ConsoleCommandParser();
        readonly Server _server;

        public Server Server { get { return _server; } }

        public ConsoleCommands(Server server)
        {
            _server = server;
        }

        [ConsoleCommand("AddUser")]
        public string AddUser(string name, string password)
        {
            if (!Character.IsValidName(name))
                return "Invalid name";

            if (!Character.IsValidPassword(password))
                return "Invalid password";

            if (!User.AddNewUser(_server.DBController.GetQuery<InsertUserQuery>(), name, password))
                return "Unknown error";

            return "New user successfully created.";
        }

        [ConsoleCommand("FindItem")]
        public string FindItem(string itemID)
        {
            ItemID id = new ItemID(int.Parse(itemID));

            object source;
            ItemEntity item = FindItem(id, out source);

            if (item == null)
                return "Item not found.";

            if (source is Map)
                return string.Format("Item `{0}` is on Map `{1}` at `{2}`.", item, source, item.Position);
            else if (source is CharacterEquipped)
                return string.Format("Item `{0}` is equipped by Character `{1}`.", item, ((CharacterEquipped)source).Character);
            else if (source is CharacterInventory)
                return string.Format("Item `{0}` is in the inventory of Character `{1}`.", item, ((CharacterInventory)source).Character);
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

        public string ExecuteCommand(string commandString)
        {
            string result;
            _parser.TryParse(this, commandString, out result);

            return result;
        }

        [ConsoleCommand("Quit")]
        public string Quit()
        {
            _server.Shutdown();
            return "Server shutting down";
        }
    }
}