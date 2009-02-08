using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using MySql.Data.MySqlClient;
using Platyform.Extensions;

namespace DemoGame.Server
{
    public abstract class UserQueryBase : NonReaderQueryBase<UserQueryValues>
    {
        public const string UsersTableName = "users";

        static string _queryFieldsStr;
        readonly MySqlParameter _body = new MySqlParameter("@body", null);

        readonly MySqlParameter _guid = new MySqlParameter("@guid", null);
        readonly MySqlParameter _map = new MySqlParameter("@map", null);
        readonly MySqlParameter _name = new MySqlParameter("@name", null);
        readonly Dictionary<StatType, MySqlParameter> _statParams = new Dictionary<StatType, MySqlParameter>();
        readonly MySqlParameter _x = new MySqlParameter("@x", null);
        readonly MySqlParameter _y = new MySqlParameter("@y", null);

        /// <summary>
        /// Gets an IEnumerable of the name of each field in Parameters by using GetFieldName().
        /// </summary>
        protected IEnumerable<string> FieldNames
        {
            get { return from param in Parameters select GetFieldName(param); }
        }

        /// <summary>
        /// Gets an IEnumerable of all the MySqlParameters for the User EXCEPT for the ID.
        /// </summary>
        protected IEnumerable<MySqlParameter> Parameters
        {
            get
            {
                var charVars = new MySqlParameter[] { _name, _map, _x, _y, _body };
                Dictionary<StatType, MySqlParameter>.ValueCollection charStats = _statParams.Values;
                return charVars.Concat(charStats);
            }
        }

        /// <summary>
        /// Gets the static cache of the GetValuesQuery(FieldNames).
        /// </summary>
        protected string QueryFieldsStr
        {
            get
            {
                if (_queryFieldsStr == null)
                    _queryFieldsStr = GetValuesQuery(FieldNames);

                return _queryFieldsStr;
            }
        }

        protected UserQueryBase(MySqlConnection conn) : base(conn)
        {
            // Add the stats to the dictionary
            foreach (StatType statType in UserStats.DatabaseStats)
            {
                string fieldName = statType.GetDatabaseField();
                _statParams.Add(statType, new MySqlParameter("@" + fieldName, null));
            }

            // Add all of the MySqlParameter fields to the parameters list
            AddParameter(_guid);
            AddParameters(Parameters);
        }

        /// <summary>
        /// Sets the database parameters to the appropriate parameters for the <paramref name="user"/>.
        /// </summary>
        /// <param name="user">User to set the parameters for.</param>
        protected override void SetParameters(UserQueryValues user)
        {
            _guid.Value = user.Guid;
            _map.Value = user.MapIndex;
            _x.Value = user.X;
            _y.Value = user.Y;
            _body.Value = user.BodyIndex;
            _name.Value = user.Name;

            foreach (IStat stat in user.Stats)
            {
                _statParams[stat.StatType].Value = stat.Value;
            }
        }
    }
}