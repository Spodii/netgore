using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Extensions;

namespace DemoGame.Server
{
    class ConsoleCommands
    {
        static readonly Dictionary<string, MethodInfo> _methods;
        readonly Server _server;

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

            if (!User.AddNewUser(_server.DBController.InsertUser, name, password))
                return "Unknown error";

            return "New user successfully created.";
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