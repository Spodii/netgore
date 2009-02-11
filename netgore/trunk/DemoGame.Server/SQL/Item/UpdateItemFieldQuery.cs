using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using MySql.Data.MySqlClient;
using Platyform.Extensions;

// FUTURE: Don't have all the database table names hard-coded

namespace DemoGame.Server
{
    public class UpdateItemFieldQuery : IDisposable
    {
        readonly MySqlConnection _conn;
        readonly Dictionary<string, FieldUpdateQuery> _fieldUpdateQueries = new Dictionary<string, FieldUpdateQuery>();
        bool _disposed;

        public UpdateItemFieldQuery(MySqlConnection conn)
        {
            if (conn == null)
                throw new ArgumentNullException("conn");

            _conn = conn;
        }

        public void Execute(string field, UpdateItemFieldValues value)
        {
            field = field.ToLower();

            FieldUpdateQuery fieldUpdate = GetFieldUpdateQuery(field);
            fieldUpdate.Execute(value);
        }

        FieldUpdateQuery GetFieldUpdateQuery(string field)
        {
            FieldUpdateQuery ret;
            if (_fieldUpdateQueries.TryGetValue(field, out ret))
                return ret;

            ret = new FieldUpdateQuery(_conn, field);
            _fieldUpdateQueries.Add(field, ret);

            return ret;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            foreach (FieldUpdateQuery item in _fieldUpdateQueries.Values)
            {
                item.Dispose();
            }
        }

        #endregion

        class FieldUpdateQuery : NonReaderQueryBase<UpdateItemFieldValues>
        {
            readonly MySqlParameter _itemGuid = new MySqlParameter("@itemGuid", null);
            readonly MySqlParameter _value = new MySqlParameter("@value", null);

            public FieldUpdateQuery(MySqlConnection conn, string field) : base(conn)
            {
                string query = "UPDATE `{0}` SET `{1}`=@value WHERE `guid`=@itemGuid";
                query = string.Format(query, ReplaceItemQuery.ItemsTableName, field);

                AddParameters(_value, _itemGuid);
                Initialize(query);
            }

            protected override void SetParameters(UpdateItemFieldValues item)
            {
                _itemGuid.Value = item.ItemGuid;
                _value.Value = item.Value;
            }
        }
    }
}