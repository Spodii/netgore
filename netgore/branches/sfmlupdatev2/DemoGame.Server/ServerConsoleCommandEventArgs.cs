using System;
using System.Linq;

namespace DemoGame.Server
{
    public class ServerConsoleCommandEventArgs : EventArgs
    {
        readonly string _command;
        readonly string _returnString;

        public ServerConsoleCommandEventArgs(string command, string returnString)
        {
            _command = command;
            _returnString = returnString;
        }

        public string Command
        {
            get { return _command; }
        }

        public string ReturnString
        {
            get { return _returnString; }
        }
    }
}