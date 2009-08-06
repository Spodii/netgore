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
        protected const string copyValuesFromMethodName = "CopyValuesFrom";
        protected const string copyValuesMethodName = "CopyValues";
        protected const string dataReaderName = "dataReader";
        protected const string dbColumnsField = "_dbColumns";
        readonly List<ColumnCollection> _columnCollections = new List<ColumnCollection>();

        /// <summary>
        /// Dictionary of the DataReader Read method names for a given Type.
        /// </summary>
        readonly Dictionary<Type, string> _dataReaderReadMethods = new Dictionary<Type, string>();

        readonly Dictionary<string, IEnumerable<DbColumnInfo>> _dbTables = new Dictionary<string, IEnumerable<DbColumnInfo>>();

        /// <summary>
        /// Using directives to add.
        /// </summary>
        readonly List<string> _usings = new List<string>();

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
            AddUsing(new string[] { "System", "System.Linq" });
        }

        public void AddColumnCollection(string name, Type keyType, Type valueType, IEnumerable<string> tables,
                                        IEnumerable<ColumnCollectionItem> columns)
        {
            ColumnCollection columnCollection = new ColumnCollection(name, keyType, valueType, tables, columns);
            _columnCollections.Add(columnCollection);
        }

        public void AddColumnCollection(string name, Type keyType, Type valueType, string table,
                                        IEnumerable<ColumnCollectionItem> columns)
        {
            ColumnCollection columnCollection = new ColumnCollection(name, keyType, valueType, new string[] { table }, columns);
            _columnCollections.Add(columnCollection);
        }

        /// <summary>
        /// Adds a Using directive to the generated code.
        /// </summary>
        /// <param name="namespaceName">Namespace to use.</param>
        public void AddUsing(string namespaceName)
        {
            if (!_usings.Contains(namespaceName, StringComparer.OrdinalIgnoreCase))
                _usings.Add(namespaceName);
        }

        /// <summary>
        /// Adds a Using directive to the generated code.
        /// </summary>
        /// <param name="namespaceNames">Namespaces to use.</param>
        public void AddUsing(IEnumerable<string> namespaceNames)
        {
            foreach (string n in namespaceNames)
            {
                AddUsing(n);
            }
        }

        protected virtual string CreateCode(string tableName, IEnumerable<DbColumnInfo> columns, string namespaceName)
        {
            columns = columns.OrderBy(x => x.Name);

            ClassData cd = new ClassData(tableName, columns, Formatter, _dataReaderReadMethods, _columnCollections);

            StringBuilder sb = new StringBuilder(16384);

            // Header
            foreach (string usingNamespace in _usings)
            {
                sb.AppendLine(Formatter.GetUsing(usingNamespace));
            }

            sb.AppendLine(Formatter.GetNamespace(namespaceName));
            sb.AppendLine(Formatter.OpenBrace);
            {
                // Interface
                sb.AppendLine(CreateCodeForInterface(cd));

                // Class
                sb.AppendLine(CreateCodeForClass(cd));
            }
            sb.AppendLine(Formatter.CloseBrace);

            return sb.ToString();
        }

        /// <summary>
        /// Creates the code for the class.
        /// </summary>
        /// <param name="cd">Class data.</param>
        /// <returns>The code for the class.</returns>
        protected virtual string CreateCodeForClass(ClassData cd)
        {
            StringBuilder sb = new StringBuilder(8192);

            sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateCode.ClassSummary, cd.TableName)));
            sb.AppendLine(Formatter.GetClass(cd.ClassName, MemberVisibilityLevel.Public, new string[] { cd.InterfaceName }));
            sb.AppendLine(Formatter.OpenBrace);
            {
                // Other Fields/Properties
                string fieldNamesCode = Formatter.GetStringArrayCode(cd.Columns.Select(x => x.Name));
                sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.ColumnArrayField));
                sb.AppendLine(Formatter.GetField(dbColumnsField, typeof(string[]), MemberVisibilityLevel.Private, fieldNamesCode,
                                                 true, true));

                sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.ColumnIEnumerableProperty));
                sb.AppendLine(Formatter.GetProperty("DbColumns", typeof(IEnumerable<string>), MemberVisibilityLevel.Public, null,
                                                    dbColumnsField, false));

                sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.TableName));
                sb.AppendLine(Formatter.GetConstField("TableName", typeof(string), MemberVisibilityLevel.Public,
                                                      "\"" + cd.TableName + "\""));

                sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.ColumnCount));
                sb.AppendLine(Formatter.GetConstField("ColumnCount", typeof(int), MemberVisibilityLevel.Public,
                                                      cd.Columns.Count().ToString()));

                // Properties for the interface implementation
                sb.AppendLine(CreateFields(cd));

                // Constructor (empty)
                sb.AppendLine(CreateConstructor(cd, string.Empty, false));

                // Constructor (full)
                string fullConstructorBody = FullConstructorMemberBody(cd);
                sb.AppendLine(CreateConstructor(cd, fullConstructorBody, true));

                // Constructor (IDataReader)
                sb.AppendLine(Formatter.GetXmlComment(cd.ClassName + " constructor.", null,
                                                      new KeyValuePair<string, string>(dataReaderName,
                                                                                       Comments.CreateCode.
                                                                                           ConstructorParameterIDataReader)));
                string drConstructorBody = Formatter.GetCallMethod("ReadValues", dataReaderName);
                var drConstructorParams = new MethodParameter[]
                                          { new MethodParameter(dataReaderName, typeof(IDataReader), Formatter) };
                sb.AppendLine(Formatter.GetConstructorHeader(cd.ClassName, MemberVisibilityLevel.Public, drConstructorParams));
                sb.AppendLine(Formatter.GetMethodBody(drConstructorBody));

                // Constructor (self-referencing interface)
                var sriConstructorParams = new MethodParameter[] { new MethodParameter("source", cd.InterfaceName) };
                sb.AppendLine(Formatter.GetConstructorHeader(cd.ClassName, MemberVisibilityLevel.Public, sriConstructorParams));
                sb.AppendLine(Formatter.GetMethodBody(Formatter.GetCallMethod(copyValuesFromMethodName, "source")));

                // Methods
                sb.AppendLine(CreateMethodReadValues(cd));
                sb.AppendLine(CreateMethodCopyValuesToDict(cd));
                sb.AppendLine(CreateMethodCopyValuesToDbParameterValues(cd));
                sb.AppendLine(CreateMethodCopyValuesFrom(cd));
            }
            sb.AppendLine(Formatter.CloseBrace);

            return sb.ToString();
        }

        /// <summary>
        /// Creates the code for the interface.
        /// </summary>
        /// <param name="cd">Class data.</param>
        /// <returns>The class code for the interface.</returns>
        protected virtual string CreateCodeForInterface(ClassData cd)
        {
            StringBuilder sb = new StringBuilder(2048);

            sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateCode.InterfaceSummary, cd.TableName)));
            sb.AppendLine(Formatter.GetInterface(cd.InterfaceName, MemberVisibilityLevel.Public));
            sb.AppendLine(Formatter.OpenBrace);
            {
                var addedCollections = new List<ColumnCollection>();
                foreach (DbColumnInfo column in cd.Columns)
                {
                    ColumnCollectionItem collItem;
                    ColumnCollection coll = cd.GetCollectionForColumn(column, out collItem);

                    if (coll == null)
                    {
                        // No collection - just add the property like normal
                        sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateCode.InterfaceGetProperty, column.Name)));
                        sb.AppendLine(Formatter.GetInterfaceProperty(cd.GetPublicName(column), column.Type, false));
                    }
                    else if (!addedCollections.Contains(coll))
                    {
                        // Has a collection - only add the code if the collection hasn't been added yet
                        addedCollections.Add(coll);
                        // TODO: ColumnCollection interface comments
                        string name = cd.GetPublicName(coll);
                        var keyParameter = new MethodParameter("key", coll.KeyType, Formatter);
                        sb.AppendLine(Formatter.GetInterfaceMethod("Get" + name, coll.ValueType, new MethodParameter[] { keyParameter }));
                        sb.AppendLine(Formatter.GetInterfaceMethod("Set" + name, typeof(void), new MethodParameter[] { keyParameter, new MethodParameter("value", coll.ValueType, Formatter) }));
                    }
                }
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
                var kvp = new KeyValuePair<string, string>(p.Name, Comments.CreateConstructor.Parameter);
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
            var addedCollections = new List<ColumnCollection>();
            foreach (DbColumnInfo column in cd.Columns)
            {
                ColumnCollectionItem collItem;
                ColumnCollection coll = cd.GetCollectionForColumn(column, out collItem);

                if (coll == null)
                {
                    // Not part of a collection
                    string comment = string.Format(Comments.CreateFields.Field, column.Name);
                    sb.AppendLine(Formatter.GetXmlComment(comment));
                    sb.AppendLine(Formatter.GetField(cd.GetPrivateName(column), column.Type, MemberVisibilityLevel.Private));
                }
                else if (!addedCollections.Contains(coll))
                {
                    // Has a collection - only add the code if the collection hasn't been added yet
                    addedCollections.Add(coll);
                    // TODO: ColumnCollection field comment

                    string collType = cd.GetCollectionTypeString(coll);
                    sb.AppendLine(Formatter.GetField(cd.GetPrivateName(coll), collType, MemberVisibilityLevel.Private, "new " + collType + "()", true, false));
                }
            }

            // Properties for the fields
            addedCollections.Clear();
            foreach (DbColumnInfo column in cd.Columns)
            {
                ColumnCollectionItem collItem;
                ColumnCollection coll = cd.GetCollectionForColumn(column, out collItem);

                if (coll == null)
                {
                    // Not part of a collection
                    string comment = string.Format(Comments.CreateFields.Property, column.Name, column.DatabaseType);

                    if (column.DefaultValue != null && !string.IsNullOrEmpty(column.DefaultValue.ToString()))
                        comment += string.Format(Comments.CreateFields.PropertyHasDefaultValue, column.DefaultValue);
                    else
                        comment += Comments.CreateFields.PropertyNoDefaultValue;

                    if (!string.IsNullOrEmpty(column.Comment))
                        comment += string.Format(Comments.CreateFields.PropertyDbComment, column.Comment);

                    sb.AppendLine(Formatter.GetXmlComment(comment));

                    sb.AppendLine(Formatter.GetProperty(cd.GetPublicName(column), column.Type, MemberVisibilityLevel.Public, MemberVisibilityLevel.Public,
                                                        cd.GetPrivateName(column), false));
                }
                else if (!addedCollections.Contains(coll))
                {
                    // Has a collection - only add the code if the collection hasn't been added yet
                    addedCollections.Add(coll);

                    string name = cd.GetPublicName(coll);
                    var keyParameter = new MethodParameter("key", coll.KeyType, Formatter);
                    string field = cd.GetPrivateName(coll) +  Formatter.OpenIndexer + Formatter.GetCast(coll.KeyType) + "key" + Formatter.CloseIndexer;

                    // TODO: ColumnCollection property comments
                    sb.AppendLine(Formatter.GetMethodHeader("Get" + name, MemberVisibilityLevel.Public, new MethodParameter[] { keyParameter }, coll.ValueType, false, false));
                    sb.AppendLine(Formatter.GetMethodBody("return " + field + Formatter.EndOfLine));

                    sb.AppendLine(Formatter.GetMethodHeader("Set" + name, MemberVisibilityLevel.Public, new MethodParameter[] { keyParameter, new MethodParameter("value", coll.ValueType, Formatter) }, typeof(void), false, false));
                    sb.AppendLine(Formatter.GetMethodBody(Formatter.GetSetValue(field, "value", true, false)));
                }
            }

            return sb.ToString();
        }

        protected virtual string CreateMethodCopyValuesFrom(ClassData cd)
        {
            const string sourceName = "source";

            var parameters = new MethodParameter[] { new MethodParameter(sourceName, cd.InterfaceName) };

            StringBuilder sb = new StringBuilder(2048);

            // Header
            sb.AppendLine(Formatter.GetMethodHeader(copyValuesFromMethodName, MemberVisibilityLevel.Public, parameters,
                                                    typeof(void), false, false));

            // Body
            StringBuilder bodySB = new StringBuilder(2048);
            foreach (DbColumnInfo column in cd.Columns)
            {
                bodySB.AppendLine(cd.GetColumnValueMutator(column, sourceName + "." + cd.GetColumnValueAccessor(column)));
            }
            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        protected virtual string CreateMethodCopyValuesToDbParameterValues(ClassData cd)
        {
            const string parameterName = "paramValues";
            const string sourceName = "source";

            var iParameters = new MethodParameter[] { new MethodParameter(parameterName, typeof(DbParameterValues), Formatter) };
            var sParameters =
                new MethodParameter[] { new MethodParameter(sourceName, cd.InterfaceName) }.Concat(iParameters).ToArray();

            StringBuilder sb = new StringBuilder(2048);

            // Instanced header
            sb.AppendLine(Formatter.GetXmlComment(Comments.CopyToDPV.Summary, null,
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   Comments.CopyToDPV.ParameterDbParameterValues)));

            sb.AppendLine(Formatter.GetMethodHeader(copyValuesMethodName, MemberVisibilityLevel.Public, iParameters, typeof(void),
                                                    false, false));

            // Instanced body
            sb.AppendLine(Formatter.GetMethodBody(Formatter.GetCallMethod(copyValuesMethodName, "this", parameterName)));

            // Static hader
            sb.AppendLine(Formatter.GetXmlComment(Comments.CopyToDPV.Summary, null,
                                                  new KeyValuePair<string, string>(sourceName, Comments.CopyToDPV.ParameterSource),
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   Comments.CopyToDPV.ParameterDbParameterValues)));

            sb.AppendLine(Formatter.GetMethodHeader(copyValuesMethodName, MemberVisibilityLevel.Public, sParameters, typeof(void),
                                                    false, true));

            // Static body
            StringBuilder bodySB = new StringBuilder(2048);
            foreach (DbColumnInfo column in cd.Columns)
            {
                string left = parameterName + "[\"@" + column.Name + "\"]";
                string right = sourceName + "." + cd.GetColumnValueAccessor(column);
                string line = Formatter.GetSetValue(left, right, false, false, column.Type);
                bodySB.AppendLine(line);
            }
            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        protected virtual string CreateMethodCopyValuesToDict(ClassData cd)
        {
            const string parameterName = "dic";
            const string sourceName = "source";

            var iParameters = new MethodParameter[]
                              { new MethodParameter(parameterName, typeof(IDictionary<string, object>), Formatter) };
            var sParameters =
                new MethodParameter[] { new MethodParameter(sourceName, cd.InterfaceName) }.Concat(iParameters).ToArray();

            StringBuilder sb = new StringBuilder(2048);

            // Instanced header
            sb.AppendLine(Formatter.GetXmlComment(Comments.CopyToDict.Summary, null,
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   Comments.CopyToDict.ParameterDict)));

            sb.AppendLine(Formatter.GetMethodHeader(copyValuesMethodName, MemberVisibilityLevel.Public, iParameters, typeof(void),
                                                    false, false));

            // Instanced body
            sb.AppendLine(Formatter.GetMethodBody(Formatter.GetCallMethod(copyValuesMethodName, "this", parameterName)));

            // Static hader
            sb.AppendLine(Formatter.GetXmlComment(Comments.CopyToDict.Summary, null,
                                                  new KeyValuePair<string, string>(sourceName, Comments.CopyToDict.ParameterSource),
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   Comments.CopyToDict.ParameterDict)));

            sb.AppendLine(Formatter.GetMethodHeader(copyValuesMethodName, MemberVisibilityLevel.Public, sParameters, typeof(void),
                                                    false, true));

            // Static body
            StringBuilder bodySB = new StringBuilder(2048);
            foreach (DbColumnInfo column in cd.Columns)
            {
                string left = parameterName + "[\"@" + column.Name + "\"]";
                string right = sourceName + "." + cd.GetColumnValueAccessor(column);
                string line = Formatter.GetSetValue(left, right, false, false, column.Type);
                bodySB.AppendLine(line);
            }
            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        protected virtual string CreateMethodReadValues(ClassData cd)
        {
            var parameters = new MethodParameter[] { new MethodParameter(dataReaderName, typeof(IDataReader), Formatter) };

            StringBuilder sb = new StringBuilder(2048);

            sb.AppendLine(Formatter.GetXmlComment(Comments.ReadValues.Summary, null,
                                                  new KeyValuePair<string, string>(dataReaderName,
                                                                                   Comments.ReadValues.ParameterDataReader)));

            // Header
            string header = Formatter.GetMethodHeader("ReadValues", MemberVisibilityLevel.Public, parameters, typeof(void), false,
                                                      false);
            sb.AppendLine(header);

            // Body
            StringBuilder bodySB = new StringBuilder(2048);
            foreach (DbColumnInfo column in cd.Columns)
            {
                string right = cd.GetDataReaderAccessor(column);
                string line = cd.GetColumnValueMutator(column, right);
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
                string right = cd.GetParameterName(column);
                sb.AppendLine(cd.GetColumnValueMutator(column, right));
            }
            return sb.ToString();
        }

        public virtual void Generate(string codeNamespace, string outputDir)
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

        public virtual IEnumerable<GeneratedTableCode> Generate(string codeNamespace)
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

        /// <summary>
        /// Sets the name of the method to use for the DataReader to read a given type.
        /// </summary>
        /// <param name="type">The type to read.</param>
        /// <param name="methodName">Name of the DataReader method to use to read the <paramref name="type"/>.</param>
        public void SetDataReaderReadMethod(Type type, string methodName)
        {
            if (_dataReaderReadMethods.ContainsKey(type))
                _dataReaderReadMethods[type] = methodName;
            else
                _dataReaderReadMethods.Add(type, methodName);
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
            public readonly IEnumerable<ColumnCollection> ColumnCollections;
            public readonly IEnumerable<DbColumnInfo> Columns;
            public readonly CodeFormatter Formatter;
            public readonly string InterfaceName;
            public readonly string TableName;
            readonly Dictionary<Type, string> _dataReaderReadMethods;
            // ReSharper restore UnaccessedField.Global

            readonly IDictionary<DbColumnInfo, string> _parameterNames = new Dictionary<DbColumnInfo, string>();
            readonly IDictionary<DbColumnInfo, string> _privateNames = new Dictionary<DbColumnInfo, string>();
            readonly IDictionary<DbColumnInfo, string> _publicNames = new Dictionary<DbColumnInfo, string>();

            public ClassData(string tableName, IEnumerable<DbColumnInfo> columns, CodeFormatter formatter,
                             Dictionary<Type, string> dataReaderReadMethods, IEnumerable<ColumnCollection> columnCollections)
            {
                TableName = tableName;
                Columns = columns;
                Formatter = formatter;
                _dataReaderReadMethods = dataReaderReadMethods;

                ClassName = formatter.GetClassName(tableName);
                InterfaceName = formatter.GetInterfaceName(tableName);

                ColumnCollections =
                    columnCollections.Where(
                        x => x.Columns.Count() > 0 && x.Tables.Contains(TableName, StringComparer.OrdinalIgnoreCase)).ToArray();

                foreach (DbColumnInfo column in columns)
                {
                    _privateNames.Add(column, formatter.GetFieldName(column.Name, MemberVisibilityLevel.Private, column.Type));
                    _publicNames.Add(column, formatter.GetFieldName(column.Name, MemberVisibilityLevel.Public, column.Type));
                    _parameterNames.Add(column, formatter.GetParameterName(column.Name, column.Type));
                }
            }

            /// <summary>
            /// Gets the ColumnCollection for a given DbColumnInfo, or null if the DbColumnInfo is not part of
            /// andy ColumnCollection in this table.
            /// </summary>
            /// <param name="dbColumn">The DbColumnInfo to get the ColumnCollection for.</param>
            /// <returns>The ColumnCollection the DbColumnInfo is part of, or null if it
            /// is not part of a ColumnCollection.</returns>
            public ColumnCollection GetCollectionForColumn(DbColumnInfo dbColumn)
            {
                ColumnCollectionItem item;
                return GetCollectionForColumn(dbColumn, out item);
            }

            /// <summary>
            /// Gets the ColumnCollection for a given DbColumnInfo, or null if the DbColumnInfo is not part of
            /// andy ColumnCollection in this table.
            /// </summary>
            /// <param name="dbColumn">The DbColumnInfo to get the ColumnCollection for.</param>
            /// <<param name="item">The ColumnCollectionItem for the <paramref name="dbColumn"/> in the ColumnCollection.</param>
            /// <returns>The ColumnCollection the DbColumnInfo is part of, or null if it
            /// is not part of a ColumnCollection.</returns>
            public ColumnCollection GetCollectionForColumn(DbColumnInfo dbColumn, out ColumnCollectionItem item)
            {
                foreach (ColumnCollection columnCollection in ColumnCollections)
                {
                    var matches =
                        columnCollection.Columns.Where(x => x.ColumnName.Equals(dbColumn.Name, StringComparison.OrdinalIgnoreCase));
                    int count = matches.Count();
                    if (count == 1)
                    {
                        item = matches.First();
                        return columnCollection;
                    }
                    else if (count > 1)
                    {
                        throw new Exception(
                            string.Format("DbColumnInfo for column `{0}` in table `{1}` matched more than one ColumnCollection!",
                                          dbColumn.Name, TableName));
                    }
                }

                item = default(ColumnCollectionItem);
                return null;
            }

            /// <summary>
            /// Gets the code to use for the accessor for a DbColumnInfo.
            /// </summary>
            /// <param name="dbColumn">The DbColumnInfo to get the value accessor for.</param>
            /// <returns>The code to use for the accessor for a DbColumnInfo.</returns>
            public string GetColumnValueAccessor(DbColumnInfo dbColumn)
            {
                ColumnCollectionItem item;
                ColumnCollection coll = GetCollectionForColumn(dbColumn, out item);

                if (coll == null)
                {
                    // Not part of a collection
                    return GetPublicName(dbColumn);
                }
                else
                {
                    // Part of a collection
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Get" + GetPublicName(coll));
                    sb.Append(Formatter.OpenParameterString);
                    sb.Append(Formatter.GetCast(coll.KeyType));
                    sb.Append(item.Key);
                    sb.Append(Formatter.CloseParameterString);
                    return sb.ToString();
                }
            }

            /// <summary>
            /// Gets the code to use for the mutator for a DbColumnInfo.
            /// </summary>
            /// <param name="dbColumn">The DbColumnInfo to get the value mutator for.</param>
            /// <param name="valueName">Code to generate for the value to set.</param>
            /// <returns>The code to use for the mutator for a DbColumnInfo.</returns>
            public string GetColumnValueMutator(DbColumnInfo dbColumn, string valueName)
            {
                ColumnCollectionItem item;
                ColumnCollection coll = GetCollectionForColumn(dbColumn, out item);

                StringBuilder sb = new StringBuilder();

                if (coll == null)
                {
                    // Not part of a collection
                    sb.Append(GetPublicName(dbColumn));
                    sb.Append(" = ");
                    sb.Append(Formatter.GetCast(dbColumn.Type));
                    sb.Append(valueName);
                    sb.Append(Formatter.EndOfLine);
                }
                else
                {
                    // Part of a collection
                    sb.Append("Set" + GetPublicName(coll));
                    sb.Append(Formatter.OpenParameterString);
                    sb.Append(Formatter.GetCast(coll.KeyType));
                    sb.Append(item.Key);
                    sb.Append(Formatter.ParameterSpacer);
                    sb.Append(Formatter.GetCast(coll.ValueType));
                    sb.Append(valueName);
                    sb.Append(Formatter.CloseParameterString);
                    sb.Append(Formatter.EndOfLine);
                }

                return sb.ToString();
            }

            /// <summary>
            /// Gets the code string used for accessing a database DbColumnInfo's value from a DataReader.
            /// </summary>
            /// <param name="column">The DbColumnInfo to get the value from.</param>
            /// <returns>The code string used for accessing a database DbColumnInfo's value.</returns>
            public string GetDataReaderAccessor(DbColumnInfo column)
            {
                string callMethod = GetDataReaderReadMethodName(column.Type);

                // Find the method to use for reading the value
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

            /// <summary>
            /// Gets the name of the method used by the DataReader to read the given Type.
            /// </summary>
            /// <param name="type">Type to read.</param>
            /// <returns>The name of the method used by the DataReader to read the given Type.</returns>
            public string GetDataReaderReadMethodName(Type type)
            {
                string callMethod;
                if (_dataReaderReadMethods.TryGetValue(type, out callMethod))
                    return callMethod;

                return "Get" + type.Name;
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

            public string GetPrivateName(ColumnCollection columnCollection)
            {
                return Formatter.GetFieldName(columnCollection.Name, MemberVisibilityLevel.Private, columnCollection.ValueType);
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

            public string GetPublicName(ColumnCollection columnCollection)
            {
                return Formatter.GetFieldName(columnCollection.Name, MemberVisibilityLevel.Public, columnCollection.ValueType);
            }

            public string GetCollectionTypeString(ColumnCollection columnCollection)
            {
                var baseType = typeof(Dictionary<,>);
                var genericTypes = new Type[] { columnCollection.KeyType, columnCollection.ValueType };
                return Formatter.GetComplexTypeString(baseType,
                                                                   genericTypes);
            }
        }

        /// <summary>
        /// Contains all the code comments.
        /// </summary>
        protected static class Comments
        {
            /// <summary>
            /// Comments used in CreateMethodCopyValuesToDict().
            /// </summary>
            public static class CopyToDict
            {
                public const string ParameterDict = "The Dictionary to copy the values into.";

                public const string ParameterSource = "The object to copy the values from.";

                public static readonly string Summary =
                    "Copies the column values into the given Dictionary using the database column name" + Environment.NewLine +
                    "with a prefixed @ as the key. The keys must already exist in the Dictionary;" + Environment.NewLine +
                    " this method will not create them if they are missing.";
            }

            /// <summary>
            /// Comments used in CreateMethodCopyValuesToDbParameterValues().
            /// </summary>
            public static class CopyToDPV
            {
                public const string ParameterDbParameterValues = "The DbParameterValues to copy the values into.";

                public const string ParameterSource = "The object to copy the values from.";

                public static readonly string Summary =
                    "Copies the column values into the given DbParameterValues using the database column name" +
                    Environment.NewLine + "with a prefixed @ as the key. The keys must already exist in the DbParameterValues;" +
                    Environment.NewLine + " this method will not create them if they are missing.";
            }

            /// <summary>
            /// Comments used in CreateCode().
            /// </summary>
            public static class CreateCode
            {
                public const string ClassSummary = "Provides a strongly-typed structure for the database table `{0}`.";
                public const string ColumnArrayField = "Array of the database column names.";

                public const string ColumnCount = "The number of columns in the database table that this class represents.";

                public const string ColumnIEnumerableProperty =
                    "Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.";

                public const string ConstructorParameterIDataReader =
                    "The IDataReader to read the values from. See method ReadValues() for details.";

                public const string InterfaceGetProperty = "Gets the value for the database column `{0}`.";

                public const string InterfaceSummary =
                    "Interface for a class that can be used to serialize values to the database table `{0}`.";

                public const string TableName = "The name of the database table that this class represents.";
            }

            /// <summary>
            /// Comments used in CreateConstructor().
            /// </summary>
            public static class CreateConstructor
            {
                public const string Parameter = "The initial value for the corresponding property.";
            }

            /// <summary>
            /// Comments used in CreateFields().
            /// </summary>
            public static class CreateFields
            {
                public const string Field = "The field that maps onto the database column `{0}`.";

                public const string PropertyHasDefaultValue = " with the default value of `{0}`.";
                public const string PropertyNoDefaultValue = ".";

                public static readonly string Property =
                    "Gets or sets the value for the field that maps onto the database column `{0}`." + Environment.NewLine +
                    "The underlying database type is `{1}`";

                public static readonly string PropertyDbComment = " The database column contains the comment: " +
                                                                  Environment.NewLine + "\"{0}\".";
            }

            /// <summary>
            /// Comments used in CreateMethodReadValues().
            /// </summary>
            public static class ReadValues
            {
                public const string ParameterDataReader =
                    "The IDataReader to read the values from. Must already be ready to be read from.";

                public static readonly string Summary =
                    "Reads the values from an IDataReader and assigns the read values to this" + Environment.NewLine +
                    "object's properties. The database column's name is used to as the key, so the value" + Environment.NewLine +
                    "will not be found if any aliases are used or not all columns were selected.";
            }
        }
    }
}