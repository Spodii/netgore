using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;

namespace NetGore.Db.ClassCreator
{
    public abstract class DbClassGenerator : IDisposable
    {
        protected const string copyValuesMethodName = "CopyValues";
        protected const string dataReaderName = "dataReader";
        protected const string dbColumnsField = "_dbColumns";

        readonly Dictionary<string, IEnumerable<DbColumnInfo>> _dbTables = new Dictionary<string, IEnumerable<DbColumnInfo>>();

        DbConnection _dbConnction;
        bool _isDbContentLoaded = false;

        /// <summary>
        /// Gets the DbConnection used to connect to the database.
        /// </summary>
        public DbConnection DbConnection
        {
            get { return _dbConnction; }
        }

        /// <summary>
        /// Gets or sets the CodeFormatter to use. Default is the CSharpCodeFormatter.
        /// </summary>
        public CodeFormatter Formatter { get; set; }

        protected DbClassGenerator()
        {
            Formatter = new CSharpCodeFormatter();
        }

        protected virtual string CreateCode(string tableName, IEnumerable<DbColumnInfo> columns, string namespaceName)
        {
            columns = columns.OrderBy(x => x.Name);

            ClassData cd = new ClassData(tableName, columns, Formatter);

            StringBuilder sb = new StringBuilder(16384);

            // Header
            sb.AppendLine(Formatter.GetUsing("System"));
            sb.AppendLine(Formatter.GetUsing("System.Linq"));

            sb.AppendLine(Formatter.GetNamespace(namespaceName));
            sb.AppendLine(Formatter.OpenBrace);
            {
                // Interface
                sb.AppendLine(
                    Formatter.GetXmlComment("Interface for a class that can be used to serialize values to the database table `" +
                                            tableName + "`."));
                sb.AppendLine(Formatter.GetInterface(cd.InterfaceName, MemberVisibilityLevel.Public));
                sb.AppendLine(Formatter.OpenBrace);
                {
                    foreach (DbColumnInfo column in columns)
                    {
                        sb.AppendLine(Formatter.GetXmlComment("Gets the value for the database column `" + column.Name + "`."));
                        sb.AppendLine(Formatter.GetInterfaceProperty(cd.GetPublicName(column), column.Type, false));
                    }
                }
                sb.AppendLine(Formatter.CloseBrace);

                // Class
                sb.AppendLine(
                    Formatter.GetXmlComment("Provides a strongly-typed structure for the database table `" + tableName + "`."));
                sb.AppendLine(Formatter.GetClass(cd.ClassName, MemberVisibilityLevel.Public, new string[] { cd.InterfaceName }));
                sb.AppendLine(Formatter.OpenBrace);
                {
                    // Fields/Properties
                    string fieldNamesCode = Formatter.GetStringArrayCode(columns.Select(x => x.Name));
                    sb.AppendLine(Formatter.GetXmlComment("Array of the database column names."));
                    sb.AppendLine(Formatter.GetField(dbColumnsField, typeof(string[]), MemberVisibilityLevel.Private,
                                                     fieldNamesCode, true, true));

                    sb.AppendLine(
                        Formatter.GetXmlComment(
                            "Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents."));
                    sb.AppendLine(Formatter.GetProperty("DbColumns", typeof(IEnumerable<string>), MemberVisibilityLevel.Public,
                                                        null, dbColumnsField, false));

                    sb.AppendLine(Formatter.GetXmlComment("The name of the database table that this class represents."));
                    sb.AppendLine(Formatter.GetConstField("TableName", typeof(string), MemberVisibilityLevel.Public,
                                                          "\"" + tableName + "\""));

                    sb.AppendLine(
                        Formatter.GetXmlComment("The number of columns in the database table that this class represents."));
                    sb.AppendLine(Formatter.GetConstField("ColumnCount", typeof(int), MemberVisibilityLevel.Public,
                                                          columns.Count().ToString()));

                    sb.AppendLine(CreateFields(cd));

                    // Constructor (empty)
                    sb.AppendLine(CreateConstructor(cd, string.Empty, false));

                    // Constructor (full)
                    string fullConstructorBody = FullConstructorMemberBody(cd);
                    sb.AppendLine(CreateConstructor(cd, fullConstructorBody, true));

                    // Constructor (IDataReader)
                    sb.AppendLine(Formatter.GetXmlComment(cd.ClassName + " constructor.", null,
                                                          new KeyValuePair<string, string>("dataReader",
                                                                                           "The IDataReader to read the values from. See method ReadValues() for details.")));
                    string drConstructorBody = Formatter.GetCallMethod("ReadValues", "dataReader");
                    var drConstructorParams = new MethodParameter[]
                                              { new MethodParameter("dataReader", typeof(IDataReader), Formatter) };
                    sb.AppendLine(Formatter.GetConstructorHeader(cd.ClassName, MemberVisibilityLevel.Public, drConstructorParams));
                    sb.AppendLine(Formatter.GetMethodBody(drConstructorBody));

                    // Methods
                    sb.AppendLine(CreateMethodReadValues(cd));
                    sb.AppendLine(CreateMethodCopyValuesToDict(cd));
                    sb.AppendLine(CreateMethodCopyValuesToDbParameterValues(cd));
                }
                sb.AppendLine(Formatter.CloseBrace);
            }
            sb.AppendLine(Formatter.CloseBrace);

            return sb.ToString();
        }

        protected virtual string CreateConstructor(ClassData cd, string code, bool addParameters)
        {
            MethodParameter[] parameters;
            if (addParameters)
                parameters = GetConstructorParameters(cd);
            else
                parameters = MethodParameter.Empty;

            string cSummary = cd.ClassName + " constructor.";
            var cParams = new List<KeyValuePair<string, string>>(Math.Max(1, parameters.Length));
            foreach (MethodParameter p in parameters)
            {
                var kvp = new KeyValuePair<string, string>(p.Name, "The initial value for the corresponding property.");
                cParams.Add(kvp);
            }

            StringBuilder sb = new StringBuilder(2048);
            sb.AppendLine(Formatter.GetXmlComment(cSummary, null, cParams.ToArray()));
            sb.AppendLine(Formatter.GetConstructorHeader(cd.ClassName, MemberVisibilityLevel.Public, parameters));
            sb.Append(Formatter.GetMethodBody(code));
            return sb.ToString();
        }

        protected virtual string CreateFields(ClassData cd)
        {
            StringBuilder sb = new StringBuilder(2048);

            // Column fields
            foreach (DbColumnInfo column in cd.Columns)
            {
                string name = cd.GetPrivateName(column);
                string comment = "The field that maps onto the database column `" + column.Name + "`.";
                sb.AppendLine(Formatter.GetXmlComment(comment));
                sb.AppendLine(Formatter.GetField(name, column.Type, MemberVisibilityLevel.Private));
            }

            // Properties for the fields
            foreach (DbColumnInfo column in cd.Columns)
            {
                string name = cd.GetPublicName(column);
                string fieldName = cd.GetPrivateName(column);
                string comment = "Gets or sets the value for the field that maps onto the database column `" + column.Name + "`." +
                                 Environment.NewLine + "The underlying database type is `" + column.DatabaseType + "`";
                if (column.DefaultValue != null && !string.IsNullOrEmpty(column.DefaultValue.ToString()))
                    comment += " with the default value of `" + column.DefaultValue + "`.";
                else
                    comment += ".";
                if (!string.IsNullOrEmpty(column.Comment))
                    comment += " The database column contains the comment: " + Environment.NewLine + "\"" + column.Comment + "\".";

                sb.AppendLine(Formatter.GetXmlComment(comment));
                sb.AppendLine(Formatter.GetProperty(name, column.Type, MemberVisibilityLevel.Public, MemberVisibilityLevel.Public,
                                                    fieldName, false));
            }

            return sb.ToString();
        }

        protected virtual string CreateMethodCopyValuesToDbParameterValues(ClassData cd)
        {
            const string parameterName = "paramValues";

            var iParameters = new MethodParameter[] { new MethodParameter(parameterName, typeof(DbParameterValues), Formatter) };
            var sParameters =
                new MethodParameter[] { new MethodParameter("source", cd.InterfaceName) }.Concat(iParameters).ToArray();

            StringBuilder sb = new StringBuilder(2048);

            string cSummary = "Copies the column values into the given DbParameterValues using the database column name" +
                              Environment.NewLine +
                              "with a prefixed @ as the key. The keys must already exist in the DbParameterValues;" +
                              Environment.NewLine + " this method will not create them if they are missing.";

            // Instanced header
            sb.AppendLine(Formatter.GetXmlComment(cSummary, null,
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   "The DbParameterValues to copy the values into.")));

            sb.AppendLine(Formatter.GetMethodHeader(copyValuesMethodName, MemberVisibilityLevel.Public, iParameters, typeof(void),
                                                    false, false));

            // Instanced body
            sb.AppendLine(Formatter.GetMethodBody(Formatter.GetCallMethod(copyValuesMethodName, "this", parameterName)));

            // Static hader
            sb.AppendLine(Formatter.GetXmlComment(cSummary, null,
                                                  new KeyValuePair<string, string>("source", "The object to copy the values from."),
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   "The DbParameterValues to copy the values into.")));

            sb.AppendLine(Formatter.GetMethodHeader(copyValuesMethodName, MemberVisibilityLevel.Public, sParameters, typeof(void),
                                                    false, true));

            // Static body
            StringBuilder bodySB = new StringBuilder(2048);
            foreach (DbColumnInfo column in cd.Columns)
            {
                string left = parameterName + "[\"@" + column.Name + "\"]";
                string right = "source." + cd.GetPublicName(column);
                string line = Formatter.GetSetValue(left, right, false, false, column.Type);
                bodySB.AppendLine(line);
            }
            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        protected virtual string CreateMethodCopyValuesToDict(ClassData cd)
        {
            const string parameterName = "dic";

            var iParameters = new MethodParameter[]
                              { new MethodParameter(parameterName, typeof(IDictionary<string, object>), Formatter) };
            var sParameters =
                new MethodParameter[] { new MethodParameter("source", cd.InterfaceName) }.Concat(iParameters).ToArray();

            StringBuilder sb = new StringBuilder(2048);

            string cSummary = "Copies the column values into the given Dictionary using the database column name" +
                              Environment.NewLine + "with a prefixed @ as the key. The keys must already exist in the Dictionary;" +
                              Environment.NewLine + " this method will not create them if they are missing.";

            // Instanced header
            sb.AppendLine(Formatter.GetXmlComment(cSummary, null,
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   "The Dictionary to copy the values into.")));

            sb.AppendLine(Formatter.GetMethodHeader(copyValuesMethodName, MemberVisibilityLevel.Public, iParameters, typeof(void),
                                                    false, false));

            // Instanced body
            sb.AppendLine(Formatter.GetMethodBody(Formatter.GetCallMethod(copyValuesMethodName, "this", parameterName)));

            // Static hader
            sb.AppendLine(Formatter.GetXmlComment(cSummary, null,
                                                  new KeyValuePair<string, string>("source", "The object to copy the values from."),
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   "The Dictionary to copy the values into.")));

            sb.AppendLine(Formatter.GetMethodHeader(copyValuesMethodName, MemberVisibilityLevel.Public, sParameters, typeof(void),
                                                    false, true));

            // Static body
            StringBuilder bodySB = new StringBuilder(2048);
            foreach (DbColumnInfo column in cd.Columns)
            {
                string left = parameterName + "[\"@" + column.Name + "\"]";
                string right = "source." + cd.GetPublicName(column);
                string line = Formatter.GetSetValue(left, right, false, false, column.Type);
                bodySB.AppendLine(line);
            }
            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        protected virtual string CreateMethodReadValues(ClassData cd)
        {
            var parameters = new MethodParameter[] { new MethodParameter("dataReader", typeof(IDataReader), Formatter) };

            StringBuilder sb = new StringBuilder(2048);

            sb.AppendLine(
                Formatter.GetXmlComment(
                    "Reads the values from an IDataReader and assigns the read values to this" + Environment.NewLine +
                    "object's properties. The database column's name is used to as the key, so the value" + Environment.NewLine +
                    "will not be found if any aliases are used or not all columns were selected.", null,
                    new KeyValuePair<string, string>("dataReader",
                                                     "The IDataReader to read the values from. Must already be ready to be read from.")));

            // Header
            string header = Formatter.GetMethodHeader("ReadValues", MemberVisibilityLevel.Public, parameters, typeof(void), false,
                                                      false);
            sb.AppendLine(header);

            // Body
            StringBuilder bodySB = new StringBuilder(2048);
            foreach (DbColumnInfo column in cd.Columns)
            {
                string line = Formatter.GetSetValue(cd.GetPublicName(column), cd.GetDataReaderAccessor(column), true, false);
                bodySB.AppendLine(line);
            }

            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        protected virtual string FullConstructorMemberBody(ClassData cd)
        {
            StringBuilder sb = new StringBuilder(1024);
            foreach (DbColumnInfo column in cd.Columns)
            {
                string left = cd.GetPublicName(column);
                string right = cd.GetParameterName(column);
                sb.AppendLine(Formatter.GetSetValue(left, right, true, false));
            }
            return sb.ToString();
        }

        public void Generate(string codeNamespace, string outputDir)
        {
            if (!outputDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                outputDir += Path.DirectorySeparatorChar.ToString();

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            var items = Generate(codeNamespace);

            foreach (GeneratedTableCode item in items)
            {
                string filePath = outputDir + item.ClassName + "." + Formatter.FilenameSuffix;
                File.WriteAllText(filePath, item.Code);
            }
        }

        public IEnumerable<GeneratedTableCode> Generate(string codeNamespace)
        {
            LoadDbContent();

            var ret = new List<GeneratedTableCode>();

            foreach (var table in _dbTables)
            {
                string code = CreateCode(table.Key, table.Value, codeNamespace);
                ret.Add(new GeneratedTableCode(table.Key, Formatter.GetClassName(table.Key), code));
            }

            return ret;
        }

        protected abstract IEnumerable<DbColumnInfo> GetColumns(string table);

        protected virtual MethodParameter[] GetConstructorParameters(ClassData cd)
        {
            var columnsArray = cd.Columns.ToArray();
            var parameters = new MethodParameter[columnsArray.Length];
            for (int i = 0; i < columnsArray.Length; i++)
            {
                DbColumnInfo column = columnsArray[i];
                MethodParameter p = new MethodParameter(cd.GetParameterName(column), column.Type, Formatter);
                parameters[i] = p;
            }

            return parameters;
        }

        protected abstract IEnumerable<string> GetTables();

        /// <summary>
        /// Loads the content (table and column data) from the database and populates _dbTables. This only happens once
        /// per object instance.
        /// </summary>
        void LoadDbContent()
        {
            if (_isDbContentLoaded)
                return;

            _isDbContentLoaded = true;

            DbConnection.Open();

            var tables = GetTables();

            foreach (string table in tables)
            {
                var columns = GetColumns(table);
                _dbTables.Add(table, columns.ToArray());
            }

            DbConnection.Close();
        }

        protected void SetDbConnection(DbConnection dbConnection)
        {
            if (_dbConnction != null)
                throw new MethodAccessException("The DbConnection has already been set!");

            _dbConnction = dbConnection;
        }

        #region IDisposable Members

        public abstract void Dispose();

        #endregion

        public class ClassData
        {
            // ReSharper disable UnaccessedField.Global
            public readonly string ClassName;
            public readonly IEnumerable<DbColumnInfo> Columns;
            public readonly CodeFormatter Formatter;
            public readonly string InterfaceName;
            public readonly string TableName;
            // ReSharper restore UnaccessedField.Global

            readonly IDictionary<DbColumnInfo, string> _parameterNames = new Dictionary<DbColumnInfo, string>();
            readonly IDictionary<DbColumnInfo, string> _privateNames = new Dictionary<DbColumnInfo, string>();
            readonly IDictionary<DbColumnInfo, string> _publicNames = new Dictionary<DbColumnInfo, string>();

            public ClassData(string tableName, IEnumerable<DbColumnInfo> columns, CodeFormatter formatter)
            {
                TableName = tableName;
                Columns = columns;
                Formatter = formatter;

                ClassName = formatter.GetClassName(tableName);
                InterfaceName = formatter.GetInterfaceName(tableName);

                foreach (DbColumnInfo column in columns)
                {
                    _privateNames.Add(column, formatter.GetFieldName(column.Name, MemberVisibilityLevel.Private, column.Type));
                    _publicNames.Add(column, formatter.GetFieldName(column.Name, MemberVisibilityLevel.Public, column.Type));
                    _parameterNames.Add(column, formatter.GetParameterName(column.Name, column.Type));
                }
            }

            /// <summary>
            /// Gets the parameter name for a DbColumnInfo.
            /// </summary>
            /// <param name="dbColumn">The DbColumnInfo to get the parameter name for.</param>
            /// <returns>The parameter name for the DbColumnInfo.</returns>
            public string GetParameterName(DbColumnInfo dbColumn)
            {
                return _parameterNames[dbColumn];
            }

            /// <summary>
            /// Gets the private name for a DbColumnInfo.
            /// </summary>
            /// <param name="dbColumn">The DbColumnInfo to get the private name for.</param>
            /// <returns>The private name for the DbColumnInfo.</returns>
            public string GetPrivateName(DbColumnInfo dbColumn)
            {
                return _privateNames[dbColumn];
            }

            /// <summary>
            /// Gets the public name for a DbColumnInfo.
            /// </summary>
            /// <param name="dbColumn">The DbColumnInfo to get the public name for.</param>
            /// <returns>The public name for the DbColumnInfo.</returns>
            public string GetPublicName(DbColumnInfo dbColumn)
            {
                return _publicNames[dbColumn];
            }

            /// <summary>
            /// Gets the code string used for accessing a database DbColumnInfo's value from a DataReader.
            /// </summary>
            /// <param name="column">The DbColumnInfo to get the value from.</param>
            /// <returns>The code string used for accessing a database DbColumnInfo's value.</returns>
            public string GetDataReaderAccessor(DbColumnInfo column)
            {
                // Find the method to use for reading the value
                // TODO: string callMethod = "Get" + column.Type.Name;
                string callMethod = "GetValue";
                StringBuilder sb = new StringBuilder();

                // Cast
                sb.Append(Formatter.GetCast(column.Type));

                // Accessor
                sb.Append(dataReaderName + ".");
                sb.Append(callMethod);
                sb.Append("(");
                sb.Append(dataReaderName);
                sb.Append(".GetOrdinal(\"");
                sb.Append(column.Name);
                sb.Append("\"))");

                return sb.ToString();
            }
        }
    }
}