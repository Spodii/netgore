using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoreUpdater.Manager
{
    public class MasterServerInfo
    {
        readonly string _host;
        readonly string _user;
        readonly string _password;

        public string Host { get { return _host; } }

        public string User { get { return _user; } }

        public string Password { get { return _password; } }

        public MasterServerInfo(string host, string user, string password)
        {
            _host = host;
            _user = user;
            _password = password;
        }
    }
}
