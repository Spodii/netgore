using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NetGore.Db;

namespace NetGore.Db.ClassCreator
{
    public abstract class DbClassGenerator : IDisposable
    {
        const string _copyValuesMethodName = "CopyValues";
        const string _dataReaderName = "dataReader";
        const string _dbColumnsField = "_dbColumns";

        protected virtual string CreateCode(string tableName, IEnumerable<DbColumnInfo> columns, string namespaceName,
                                            CodeFormatter formatter)
        {
            columns = columns.OrderBy(x => x.Name);

            string className = formatter.GetClassName(tableName);
            string interfaceName = formatter.GetInterfaceName(tableName);

            var privateColumnNames = new Dictionary<DbColumnInfo, string>();
            var publicColumnNames = new Dictionary<DbColumnInfo, string>();
            var parameterColumnNames = new Dictionary<DbColumnInfo, string>();

            foreach (DbColumnInfo column in columns)
            {
                privateColumnNames.Add(column, formatter.GetFieldName(column.Name, MemberVisibilityLevel.Private, column.Type));
                publicColumnNames.Add(column, formatter.GetFieldName(column.Name, MemberVisibilityLevel.Public, column.Type));
                parameterColumnNames.Add(column, formatter.GetParameterName(column.Name, column.Type));
            }

            StringBuilder sb = new StringBuilder(16384);

            // Header
            sb.AppendLine(formatter.GetUsing("System"));
            sb.AppendLine(formatter.GetUsing("System.Linq"));

            sb.AppendLine(formatter.GetNamespace(namespaceName));
            sb.AppendLine(formatter.OpenBrace);
            {
                // Interface
                sb.AppendLine(
                    formatter.GetXmlComment("Interface for a class that can be used to serialize values to the database table `" +
                                            tableName + "`."));
                sb.AppendLine(formatter.GetInterface(interfaceName, MemberVisibilityLevel.Public));
                sb.AppendLine(formatter.OpenBrace);
                {
                    foreach (DbColumnInfo column in columns)
                    {
                        sb.AppendLine(formatter.GetXmlComment("Gets the value for the database column `" + column.Name + "`."));
                        sb.AppendLine(formatter.GetInterfaceProperty(publicColumnNames[column], column.Type, false));
                    }
                }
                sb.AppendLine(formatter.CloseBrace);

                // Class
                sb.AppendLine(
                    formatter.GetXmlComment("Provides a strongly-typed structure for the database table `" + tableName + "`."));
                sb.AppendLine(formatter.GetClass(className, MemberVisibilityLevel.Public, new string[] { interfaceName }));
                sb.AppendLine(formatter.OpenBrace);
                {
                    // Fields/Properties
                    string fieldNamesCode = GetStringArrayCode(columns.Select(x => x.Name), formatter);
                    sb.AppendLine(formatter.GetXmlComment("Array of the database column names."));
                    sb.AppendLine(formatter.GetField(_dbColumnsField, typeof(string[]), MemberVisibilityLevel.Private,
                                                     fieldNamesCode, true, true));

                    sb.AppendLine(
                        formatter.GetXmlComment(
                            "Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents."));
                    sb.AppendLine(formatter.GetProperty("DbColumns", typeof(IEnumerable<string>), MemberVisibilityLevel.Public,
                                                        null, _dbColumnsField, false));

                    sb.AppendLine(formatter.GetXmlComment("The name of the database table that this class represents."));
                    sb.AppendLine(formatter.GetConstField("TableName", typeof(string), MemberVisibilityLevel.Public,
                                                          "\"" + tableName + "\""));

                    sb.AppendLine(
                        formatter.GetXmlComment("The number of columns in the database table that this class represents."));
                    sb.AppendLine(formatter.GetConstField("ColumnCount", typeof(int), MemberVisibilityLevel.Public,
                                                          columns.Count().ToString()));

                    sb.AppendLine(CreateFields(columns, privateColumnNames, publicColumnNames, formatter));

                    // Constructor (empty)
                    sb.AppendLine(CreateConstructor(className, columns, parameterColumnNames, formatter, string.Empty, false));

                    // Constructor (full)
                    string fullConstructorBody = FullConstructorMemberBody(columns, parameterColumnNames, publicColumnNames,
                                                                           formatter);
                    sb.AppendLine(CreateConstructor(className, columns, parameterColumnNames, formatter, fullConstructorBody, true));

                    // Constructor (IDataReader)
                    sb.AppendLine(formatter.GetXmlComment(className + " constructor.", null,
                                                          new KeyValuePair<string, string>("dataReader",
                                                                                           "The IDataReader to read the values from. See method ReadValues() for details.")));
                    string drConstructorBody = formatter.GetCallMethod("ReadValues", "dataReader");
                    var drConstructorParams = new MethodParameter[]
                                              { new MethodParameter("dataReader", typeof(IDataReader), formatter) };
                    sb.AppendLine(formatter.GetConstructorHeader(className, MemberVisibilityLevel.Public, drConstructorParams));
                    sb.AppendLine(formatter.GetMethodBody(drConstructorBody));

                    // Methods
                    sb.AppendLine(CreateMethodReadValues(columns, publicColumnNames, formatter));
                    sb.AppendLine(CreateMethodCopyValuesToDict(interfaceName, columns, publicColumnNames, formatter));
                    sb.AppendLine(CreateMethodCopyValuesToDbParameterValues(interfaceName, columns, publicColumnNames, formatter));
                }
                sb.AppendLine(formatter.CloseBrace);
            }
            sb.AppendLine(formatter.CloseBrace);

            return sb.ToString();
        }

        protected virtual string CreateConstructor(string className, IEnumerable<DbColumnInfo> columns,
                                                   Dictionary<DbColumnInfo, string> parameterNames, CodeFormatter formatter,
                                                   string code, bool addParameters)
        {
            MethodParameter[] parameters;
            if (addParameters)
                parameters = GetMethodParameters(columns, parameterNames, formatter);
            else
                parameters = MethodParameter.Empty;

            string cSummary = className + " constructor.";
            var cParams = new List<KeyValuePair<string, string>>(Math.Max(1, parameters.Length));
            foreach (MethodParameter p in parameters)
            {
                var kvp = new KeyValuePair<string, string>(p.Name, "The initial value for the corresponding property.");
                cParams.Add(kvp);
            }

            StringBuilder sb = new StringBuilder(2048);
            sb.AppendLine(formatter.GetXmlComment(cSummary, null, cParams.ToArray()));
            sb.AppendLine(formatter.GetConstructorHeader(className, MemberVisibilityLevel.Public, parameters));
            sb.Append(formatter.GetMethodBody(code));
            return sb.ToString();
        }

        protected virtual string CreateFields(IEnumerable<DbColumnInfo> columns, Dictionary<DbColumnInfo, string> privateNames,
                                              Dictionary<DbColumnInfo, string> publicNames, CodeFormatter formatter)
        {
            StringBuilder sb = new StringBuilder(2048);

            // Column fields
            foreach (DbColumnInfo column in columns)
            {
                string name = privateNames[column];
                string comment = "The field that maps onto the database column `" + column.Name + "`.";
                sb.AppendLine(formatter.GetXmlComment(comment));
                sb.AppendLine(formatter.GetField(name, column.Type, MemberVisibilityLevel.Private));
            }

            // Properties for the fields
            foreach (DbColumnInfo column in columns)
            {
                string name = publicNames[column];
                string fieldName = privateNames[column];
                string comment = "Gets or sets the value for the field that maps onto the database column `" + column.Name + "`." +
                                 Environment.NewLine + "The underlying database type is `" + column.DatabaseType + "`";
                if (column.DefaultValue != null && !string.IsNullOrEmpty(column.DefaultValue.ToString()))
                    comment += " with the default value of `" + column.DefaultValue + "`.";
                else
                    comment += ".";
                if (!string.IsNullOrEmpty(column.Comment))
                    comment += " The database column contains the comment: " + Environment.NewLine + "\"" + column.Comment + "\".";

                sb.AppendLine(formatter.GetXmlComment(comment));
                sb.AppendLine(formatter.GetProperty(name, column.Type, MemberVisibilityLevel.Public, MemberVisibilityLevel.Public,
                                                    fieldName, false));
            }

            return sb.ToString();
        }

        protected virtual string CreateMethodCopyValuesToDbParameterValues(string interfaceName, IEnumerable<DbColumnInfo> columns,
                                                                           Dictionary<DbColumnInfo, string> publicNames,
                                                                           CodeFormatter formatter)
        {
            const string parameterName = "paramValues";

            var iParameters = new MethodParameter[] { new MethodParameter(parameterName, typeof(DbParameterValues), formatter) };
            var sParameters = new MethodParameter[] { new MethodParameter("source", interfaceName) }.Concat(iParameters).ToArray();

            StringBuilder sb = new StringBuilder(2048);

            string cSummary = "Copies the column values into the given DbParameterValues using the database column name" +
                              Environment.NewLine +
                              "with a prefixed @ as the key. The keys must already exist in the DbParameterValues;" +
                              Environment.NewLine + " this method will not create them if they are missing.";

            // Instanced header
            sb.AppendLine(formatter.GetXmlComment(cSummary, null,
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   "The DbParameterValues to copy the values into.")));

            sb.AppendLine(formatter.GetMethodHeader(_copyValuesMethodName, MemberVisibilityLevel.Public, iParameters, typeof(void),
                                                    false, false));

            // Instanced body
            sb.AppendLine(formatter.GetMethodBody(formatter.GetCallMethod(_copyValuesMethodName, "this", parameterName)));

            // Static hader
            sb.AppendLine(formatter.GetXmlComment(cSummary, null,
                                                  new KeyValuePair<string, string>("source", "The object to copy the values from."),
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   "The DbParameterValues to copy the values into.")));

            sb.AppendLine(formatter.GetMethodHeader(_copyValuesMethodName, MemberVisibilityLevel.Public, sParameters, typeof(void),
                                                    false, true));

            // Static body
            StringBuilder bodySB = new StringBuilder(2048);
            foreach (DbColumnInfo column in columns)
            {
                string left = parameterName + "[\"@" + column.Name + "\"]";
                string right = "source." + publicNames[column];
                string line = formatter.GetSetValue(left, right, false, false, column.Type);
                bodySB.AppendLine(line);
            }
            sb.AppendLine(formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        protected virtual string CreateMethodCopyValuesToDict(string interfaceName, IEnumerable<DbColumnInfo> columns,
                                                              Dictionary<DbColumnInfo, string> publicNames,
                                                              CodeFormatter formatter)
        {
            const string parameterName = "dic";

            var iParameters = new MethodParameter[]
                              { new MethodParameter(parameterName, typeof(IDictionary<string, object>), formatter) };
            var sParameters = new MethodParameter[] { new MethodParameter("source", interfaceName) }.Concat(iParameters).ToArray();

            StringBuilder sb = new StringBuilder(2048);

            string cSummary = "Copies the column values into the given Dictionary using the database column name" +
                              Environment.NewLine + "with a prefixed @ as the key. The keys must already exist in the Dictionary;" +
                              Environment.NewLine + " this method will not create them if they are missing.";

            // Instanced header
            sb.AppendLine(formatter.GetXmlComment(cSummary, null,
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   "The Dictionary to copy the values into.")));

            sb.AppendLine(formatter.GetMethodHeader(_copyValuesMethodName, MemberVisibilityLevel.Public, iParameters, typeof(void),
                                                    false, false));

            // Instanced body
            sb.AppendLine(formatter.GetMethodBody(formatter.GetCallMethod(_copyValuesMethodName, "this", parameterName)));

            // Static hader
            sb.AppendLine(formatter.GetXmlComment(cSummary, null,
                                                  new KeyValuePair<string, string>("source", "The object to copy the values from."),
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   "The Dictionary to copy the values into.")));

            sb.AppendLine(formatter.GetMethodHeader(_copyValuesMethodName, MemberVisibilityLevel.Public, sParameters, typeof(void),
                                                    false, true));

            // Static body
            StringBuilder bodySB = new StringBuilder(2048);
            foreach (DbColumnInfo column in columns)
            {
                string left = parameterName + "[\"@" + column.Name + "\"]";
                string right = "source." + publicNames[column];
                string line = formatter.GetSetValue(left, right, false, false, column.Type);
                bodySB.AppendLine(line);
            }
            sb.AppendLine(formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        protected virtual string CreateMethodReadValues(IEnumerable<DbColumnInfo> columns,
                                                        Dictionary<DbColumnInfo, string> publicNames, CodeFormatter formatter)
        {
            var parameters = new MethodParameter[] { new MethodParameter("dataReader", typeof(IDataReader), formatter) };

            StringBuilder sb = new StringBuilder(2048);

            sb.AppendLine(
                formatter.GetXmlComment(
                    "Reads the values from an IDataReader and assigns the read values to this" + Environment.NewLine +
                    "object's properties. The database column's name is used to as the key, so the value" + Environment.NewLine +
                    "will not be found if any aliases are used or not all columns were selected.", null,
                    new KeyValuePair<string, string>("dataReader",
                                                     "The IDataReader to read the values from. Must already be ready to be read from.")));

            // Header
            string header = formatter.GetMethodHeader("ReadValues", MemberVisibilityLevel.Public, parameters, typeof(void), false,
                                                      false);
            sb.AppendLine(header);

            // Body
            StringBuilder bodySB = new StringBuilder(2048);
            foreach (DbColumnInfo column in columns)
            {
                string line = formatter.GetSetValue(publicNames[column], GetDataReaderAccessor(column, formatter), true, false);
                bodySB.AppendLine(line);
            }

            sb.AppendLine(formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        protected virtual string FullConstructorMemberBody(IEnumerable<DbColumnInfo> columns,
                                                           Dictionary<DbColumnInfo, string> parameterNames,
                                                           Dictionary<DbColumnInfo, string> publicNames, CodeFormatter formatter)
        {
            StringBuilder sb = new StringBuilder(1024);
            foreach (DbColumnInfo column in columns)
            {
                string left = publicNames[column];
                string right = parameterNames[column];
                sb.AppendLine(formatter.GetSetValue(left, right, true, false));
            }
            return sb.ToString();
        }

        protected abstract IEnumerable<DbColumnInfo> GetColumns(string table);

        protected static string GetDataReaderAccessor(DbColumnInfo column, CodeFormatter formatter)
        {
            // Find the method to use for reading the value
            // TODO: string callMethod = "Get" + column.Type.Name;
            string callMethod = "GetValue";
            StringBuilder sb = new StringBuilder();

            // Cast
            sb.Append(formatter.GetCast(column.Type));

            // Accessor
            sb.Append(_dataReaderName + ".");
            sb.Append(callMethod);
            sb.Append("(");
            sb.Append(_dataReaderName);
            sb.Append(".GetOrdinal(\"");
            sb.Append(column.Name);
            sb.Append("\"))");

            return sb.ToString();
        }

        protected virtual MethodParameter[] GetMethodParameters(IEnumerable<DbColumnInfo> columns,
                                                                Dictionary<DbColumnInfo, string> parameterNames,
                                                                CodeFormatter formatter)
        {
            var columnsArray = columns.ToArray();
            var parameters = new MethodParameter[columnsArray.Length];
            for (int i = 0; i < columnsArray.Length; i++)
            {
                DbColumnInfo column = columnsArray[i];
                MethodParameter p = new MethodParameter(parameterNames[column], column.Type, formatter);
                parameters[i] = p;
            }

            return parameters;
        }

        protected virtual string GetStringArrayCode(IEnumerable<string> strings, CodeFormatter formatter)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("new string[] ");

            sb.Append(formatter.OpenBrace);
            foreach (string s in strings)
            {
                sb.Append("\"");
                sb.Append(s);
                sb.Append("\"");
                sb.Append(formatter.ParameterSpacer);
            }

            if (strings.Count() > 0)
                sb.Length -= formatter.ParameterSpacer.Length;

            sb.Append(" ");
            sb.Append(formatter.CloseBrace);

            return sb.ToString();
        }

        protected abstract IEnumerable<string> GetTables();

        #region IDisposable Members

        public abstract void Dispose();

        #endregion
    }
}