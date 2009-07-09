using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.Queries;

// NOTE: Wow, even I forgot how this works. Lack attributes much?

namespace DemoGame.Server
{
    class ConsoleCommands
    {
        static readonly Dictionary<string, MethodInfo> _methods;
        readonly Server _server;

        public Server Server { get { return _server; } }

        static ConsoleCommands()
        {
            _methods = new Dictionary<string, MethodInfo>(StringComparer.CurrentCultureIgnoreCase);

            foreach (MethodInfo method in typeof(ConsoleCommands).GetMethods())
            {
                // Must be public
                if (!method.IsPublic)
                    continue;

                // Check for a valid return type
                if (method.ReturnType != typeof(string))
                    continue;

                // Check for valid parameters
                bool isValid = true;
                foreach (ParameterInfo param in method.GetParameters())
                {
                    if (param.ParameterType != typeof(string))
                    {
                        isValid = false;
                        break;
                    }
                }
                if (!isValid)
                    continue;

                // Add to the Dictionary
                _methods.Add(method.Name, method);
            }
        }

        public ConsoleCommands(Server server)
        {
            _server = server;
        }

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

        ItemEntity FindItem(ItemID id, out object source)
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
            if (string.IsNullOrEmpty(commandString))
                return string.Empty;

            var split = commandString.Split(new string[] { " " }, 2, StringSplitOptions.RemoveEmptyEntries);
            string command = split[0];

            string[] parameters;
            if (split.Length == 1 || string.IsNullOrEmpty(split[1]))
                parameters = new string[0];
            else
                parameters = split[1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            // Check if the method exists
            if (!_methods.ContainsKey(command))
                return string.Format("Unknown command `{0}`", command);

            MethodInfo method = _methods[command];

            // Check for a valid number of parameters
            // If there are no parameters, just ignore the parameters
            var methodParams = method.GetParameters();
            if (methodParams.Length > 0 && methodParams.Length != parameters.Length)
            {
                string retStr = "Invalid number of parameters. Usage: AddUser(";
                for (int i = 0; i < parameters.Length; i++)
                {
                    retStr += methodParams[i].Name;
                    if (i < parameters.Length)
                        retStr += ",";
                }
                retStr += ")";
                return retStr;
            }

            // Execute the method
            return method.Invoke(this, parameters).ToString();
        }

        public string Quit()
        {
            _server.Shutdown();

            return "Server shutting down";
        }
    }
}