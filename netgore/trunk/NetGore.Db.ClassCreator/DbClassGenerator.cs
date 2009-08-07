using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using NetGore.Db.ClassCreator.Properties;

namespace NetGore.Db.ClassCreator
{
    public abstract class DbClassGenerator : IDisposable
    {
        /// <summary>
        /// Name of the CopyValuesFrom method in the generated code.
        /// </summary>
        public const string CopyValuesFromMethodName = "CopyValuesFrom";
        
        /// <summary>
        /// Name of the CopyValues method in the generated code.
        /// </summary>
        public const string CopyValuesMethodName = "CopyValues";

        /// <summary>
        /// Name of the dataReader when used in arguments in the generated code.
        /// </summary>
        public const string DataReaderName = "dataReader";

        /// <summary>
        /// Name of the _dbColumns field in the generated code.
        /// </summary>
        public const string DbColumnsField = "_dbColumns";

        readonly List<ColumnCollection> _columnCollections = new List<ColumnCollection>();

        /// <summary>
        /// Dictionary of the DataReader Read method names for a given Type.
        /// </summary>
        readonly Dictionary<Type, string> _dataReaderReadMethods = new Dictionary<Type, string>();

        readonly Dictionary<string, IEnumerable<DbColumnInfo>> _dbTables = new Dictionary<string, IEnumerable<DbColumnInfo>>();

        readonly List<CustomTypeMapping> _customTypes = new List<CustomTypeMapping>();

        public void AddCustomType(Type type, string table, params string[] columns)
        {
            AddCustomType(type, new string[] { table}, columns);
        }

        public void AddCustomType(string type, string table, params string[] columns)
        {
            AddCustomType(type, new string[] { table }, columns);
        }

        public void AddCustomType(Type type, IEnumerable<string> tables, params string[] columns)
        {
            AddCustomType(Formatter.GetTypeString(type), tables, columns);
        }

        public void AddCustomType(string type, IEnumerable<string> tables, params string[] columns)
        {
            _customTypes.Add(new CustomTypeMapping(tables, columns, type));
        }

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
            if (!(keyType.IsEnum))
                throw new ArgumentException("Only Enums are supported for the keyType at the present.", "keyType");

            ColumnCollection columnCollection = new ColumnCollection(name, keyType, valueType, tables, columns);
            _columnCollections.Add(columnCollection);
        }

        public void AddColumnCollection(string name, Type keyType, Type valueType, string table,
                                        IEnumerable<ColumnCollectionItem> columns)
        {
            if (!(keyType.IsEnum))
                throw new ArgumentException("Only Enums are supported for the keyType at the present.", "keyType");

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

            DbClassData cd = new DbClassData(tableName, columns, Formatter, _dataReaderReadMethods, _columnCollections, _customTypes);

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
        protected virtual string CreateCodeForClass(DbClassData cd)
        {
            StringBuilder sb = new StringBuilder(8192);

            sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateCode.ClassSummary, cd.TableName)));
            sb.AppendLine(Formatter.GetClass(cd.ClassName, MemberVisibilityLevel.Public, new string[] { cd.InterfaceName }));
            sb.AppendLine(Formatter.OpenBrace);
            {
                // Other Fields/Properties
                string fieldNamesCode = Formatter.GetStringArrayCode(cd.Columns.Select(x => x.Name));
                sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.ColumnArrayField));
                sb.AppendLine(Formatter.GetField(DbColumnsField, typeof(string[]), MemberVisibilityLevel.Private, fieldNamesCode,
                                                 true, true));

                sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.ColumnIEnumerableProperty));
                sb.AppendLine(Formatter.GetProperty("DbColumns", typeof(IEnumerable<string>), typeof(IEnumerable<string>), MemberVisibilityLevel.Public, null,
                                                    DbColumnsField, false));

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
                                                      new KeyValuePair<string, string>(DataReaderName,
                                                                                       Comments.CreateCode.
                                                                                           ConstructorParameterIDataReader)));
                string drConstructorBody = Formatter.GetCallMethod("ReadValues", DataReaderName);
                var drConstructorParams = new MethodParameter[]
                                          { new MethodParameter(DataReaderName, typeof(IDataReader), Formatter) };
                sb.AppendLine(Formatter.GetConstructorHeader(cd.ClassName, MemberVisibilityLevel.Public, drConstructorParams));
                sb.AppendLine(Formatter.GetMethodBody(drConstructorBody));

                // Constructor (self-referencing interface)
                var sriConstructorParams = new MethodParameter[] { new MethodParameter("source", cd.InterfaceName) };
                sb.AppendLine(Formatter.GetConstructorHeader(cd.ClassName, MemberVisibilityLevel.Public, sriConstructorParams));
                sb.AppendLine(Formatter.GetMethodBody(Formatter.GetCallMethod(CopyValuesFromMethodName, "source")));

                // Methods
                sb.AppendLine(CreateMethodReadValues(cd));
                sb.AppendLine(CreateMethodCopyValuesToDict(cd));
                sb.AppendLine(CreateMethodCopyValuesToDbParameterValues(cd));
                sb.AppendLine(CreateMethodCopyValuesFrom(cd));
                sb.AppendLine(CreateMethodGetValue(cd));
                sb.AppendLine(CreateMethodSetValue(cd));

                // ConstEnumDictionary class
                foreach (ColumnCollection coll in cd.ColumnCollections)
                {
                    sb.AppendLine(cd.GetConstEnumDictonaryCode(coll));
                }
            }
            sb.AppendLine(Formatter.CloseBrace);

            return sb.ToString();
        }

        /// <summary>
        /// Creates the code for the interface.
        /// </summary>
        /// <param name="cd">Class data.</param>
        /// <returns>The class code for the interface.</returns>
        protected virtual string CreateCodeForInterface(DbClassData cd)
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
                        sb.AppendLine(Formatter.GetInterfaceProperty(cd.GetPublicName(column), cd.GetExternalType(column), false));
                    }
                    else if (!addedCollections.Contains(coll))
                    {
                        // Has a collection - only add the code if the collection hasn't been added yet
                        addedCollections.Add(coll);
                        // TODO: ColumnCollection interface comments
                        string name = cd.GetPublicName(coll);
                        MethodParameter keyParameter = new MethodParameter("key", coll.KeyType, Formatter);
                        sb.AppendLine(Formatter.GetInterfaceMethod("Get" + name, coll.ValueType,
                                                                   new MethodParameter[] { keyParameter }));
                        sb.AppendLine(Formatter.GetInterfaceMethod("Set" + name, typeof(void),
                                                                   new MethodParameter[]
                                                                   {
                                                                       keyParameter,
                                                                       new MethodParameter("value", coll.ValueType, Formatter)
                                                                   }));
                    }
                }
            }
            sb.AppendLine(Formatter.CloseBrace);

            return sb.ToString();
        }

        protected virtual string CreateConstructor(DbClassData cd, string code, bool addParameters)
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

        protected virtual string CreateFields(DbClassData cd)
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
                    sb.AppendLine(Formatter.GetField(cd.GetPrivateName(column), cd.GetInternalType(column), MemberVisibilityLevel.Private));
                }
                else if (!addedCollections.Contains(coll))
                {
                    // Has a collection - only add the code if the collection hasn't been added yet
                    addedCollections.Add(coll);
                    // TODO: ColumnCollection field comment

                    string collType = DbClassData.GetCollectionTypeString(coll);
                    sb.AppendLine(Formatter.GetField(cd.GetPrivateName(coll), collType, MemberVisibilityLevel.Private,
                                                     "new " + collType + "()", true, false));
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

                    sb.AppendLine(Formatter.GetProperty(cd.GetPublicName(column), cd.GetExternalType(column), cd.GetInternalType(column), MemberVisibilityLevel.Public,
                                                        MemberVisibilityLevel.Public, cd.GetPrivateName(column), false));
                }
                else if (!addedCollections.Contains(coll))
                {
                    // Has a collection - only add the code if the collection hasn't been added yet
                    addedCollections.Add(coll);

                    string name = cd.GetPublicName(coll);
                    MethodParameter keyParameter = new MethodParameter("key", coll.KeyType, Formatter);
                    string field = cd.GetPrivateName(coll) + Formatter.OpenIndexer + Formatter.GetCast(coll.KeyType) + "key" +
                                   Formatter.CloseIndexer;

                    // TODO: ColumnCollection property comments
                    sb.AppendLine(Formatter.GetMethodHeader("Get" + name, MemberVisibilityLevel.Public,
                                                            new MethodParameter[] { keyParameter }, coll.ValueType, false, false));
                    sb.AppendLine(Formatter.GetMethodBody("return " + Formatter.GetCast(cd.GetExternalType(column)) + field + Formatter.EndOfLine));

                    sb.AppendLine(Formatter.GetMethodHeader("Set" + name, MemberVisibilityLevel.Public,
                                                            new MethodParameter[]
                                                            {
                                                                keyParameter,
                                                                new MethodParameter("value", coll.ValueType, Formatter)
                                                            },
                                                            typeof(void), false, false));
                    sb.AppendLine(Formatter.GetMethodBody(Formatter.GetSetValue(field, "value", true, false, cd.GetInternalType(column))));
                }
            }

            return sb.ToString();
        }

        protected virtual string CreateMethodCopyValuesFrom(DbClassData cd)
        {
            const string sourceName = "source";

            var parameters = new MethodParameter[] { new MethodParameter(sourceName, cd.InterfaceName) };

            StringBuilder sb = new StringBuilder(2048);

            // Header
            sb.AppendLine(Formatter.GetMethodHeader(CopyValuesFromMethodName, MemberVisibilityLevel.Public, parameters,
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

        protected virtual string CreateMethodCopyValuesToDbParameterValues(DbClassData cd)
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

            sb.AppendLine(Formatter.GetMethodHeader(CopyValuesMethodName, MemberVisibilityLevel.Public, iParameters, typeof(void),
                                                    false, false));

            // Instanced body
            sb.AppendLine(Formatter.GetMethodBody(Formatter.GetCallMethod(CopyValuesMethodName, "this", parameterName)));

            // Static hader
            sb.AppendLine(Formatter.GetXmlComment(Comments.CopyToDPV.Summary, null,
                                                  new KeyValuePair<string, string>(sourceName, Comments.CopyToDPV.ParameterSource),
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   Comments.CopyToDPV.ParameterDbParameterValues)));

            sb.AppendLine(Formatter.GetMethodHeader(CopyValuesMethodName, MemberVisibilityLevel.Public, sParameters, typeof(void),
                                                    false, true));

            // Static body
            StringBuilder bodySB = new StringBuilder(2048);
            foreach (DbColumnInfo column in cd.Columns)
            {
                string left = parameterName + "[\"@" + column.Name + "\"]";
                string right = sourceName + "." + cd.GetColumnValueAccessor(column);
                string line = Formatter.GetSetValue(left, right, false, false, cd.GetExternalType(column));
                bodySB.AppendLine(line);
            }
            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        protected virtual string CreateMethodGetValue(DbClassData cd)
        {
            const string parameterName = "columnName";
            const string methodName = "GetValue";

            StringBuilder sb = new StringBuilder(2048);

            // Header
            // TODO: Header XML comments
            var parameters = new MethodParameter[] { new MethodParameter(parameterName, typeof(string), Formatter)};
            sb.AppendLine(Formatter.GetMethodHeader(methodName, MemberVisibilityLevel.Public, parameters, typeof(object), false, false));

            // Body
            var switches= cd.Columns.Select(x => new KeyValuePair<string, string>("\"" + x.Name + "\"", Formatter.ReturnString + " " + cd.GetColumnValueAccessor(x) + Formatter.EndOfLine));
            var defaultCode = "throw new ArgumentException(\"Field not found.\",\"" + parameterName + "\")" + Formatter.EndOfLine;
            sb.AppendLine(Formatter.GetMethodBody(Formatter.GetSwitch(parameterName, switches, defaultCode)));

            return sb.ToString();
        }

        protected virtual string CreateMethodSetValue(DbClassData cd)
        {
            const string parameterName = "columnName";
            const string valueName = "value";
            const string methodName = "SetValue";

            StringBuilder sb = new StringBuilder(2048);

            // Header
            // TODO: Header XML comments
            var parameters = new MethodParameter[] { new MethodParameter(parameterName, typeof(string), Formatter), new MethodParameter(valueName, typeof(object), Formatter) };
            sb.AppendLine(Formatter.GetMethodHeader(methodName, MemberVisibilityLevel.Public, parameters, typeof(void), false, false));

            // Body
            var switches = cd.Columns.Select(x => new KeyValuePair<string, string>("\"" + x.Name + "\"",
                cd.GetColumnValueMutator(x, valueName) + Environment.NewLine + "break" + Formatter.EndOfLine));
            var defaultCode = "throw new ArgumentException(\"Field not found.\",\"" + parameterName + "\")" + Formatter.EndOfLine;
            sb.AppendLine(Formatter.GetMethodBody(Formatter.GetSwitch(parameterName, switches, defaultCode)));

            return sb.ToString();
        }

        protected virtual string CreateMethodCopyValuesToDict(DbClassData cd)
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

            sb.AppendLine(Formatter.GetMethodHeader(CopyValuesMethodName, MemberVisibilityLevel.Public, iParameters, typeof(void),
                                                    false, false));

            // Instanced body
            sb.AppendLine(Formatter.GetMethodBody(Formatter.GetCallMethod(CopyValuesMethodName, "this", parameterName)));

            // Static hader
            sb.AppendLine(Formatter.GetXmlComment(Comments.CopyToDict.Summary, null,
                                                  new KeyValuePair<string, string>(sourceName, Comments.CopyToDict.ParameterSource),
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   Comments.CopyToDict.ParameterDict)));

            sb.AppendLine(Formatter.GetMethodHeader(CopyValuesMethodName, MemberVisibilityLevel.Public, sParameters, typeof(void),
                                                    false, true));

            // Static body
            StringBuilder bodySB = new StringBuilder(2048);
            foreach (DbColumnInfo column in cd.Columns)
            {
                string left = parameterName + "[\"@" + column.Name + "\"]";
                string right = sourceName + "." + cd.GetColumnValueAccessor(column);
                string line = Formatter.GetSetValue(left, right, false, false, cd.GetExternalType(column));
                bodySB.AppendLine(line);
            }
            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        protected virtual string CreateMethodReadValues(DbClassData cd)
        {
            var parameters = new MethodParameter[] { new MethodParameter(DataReaderName, typeof(IDataReader), Formatter) };

            StringBuilder sb = new StringBuilder(2048);

            sb.AppendLine(Formatter.GetXmlComment(Comments.ReadValues.Summary, null,
                                                  new KeyValuePair<string, string>(DataReaderName,
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

        protected virtual string FullConstructorMemberBody(DbClassData cd)
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

        public const string ColumnMetadataClassName = "ColumnMetadata";

        protected virtual GeneratedTableCode CreateCodeForColumnMetadata(string codeNamespace)
        {
            string code = Resources.ColumnMetadataCode;
            StringBuilder sb = new StringBuilder(code.Length + 200);

            sb.AppendLine(Formatter.GetUsing("System"));

            sb.AppendLine(Formatter.GetNamespace(codeNamespace));
            sb.AppendLine(Formatter.OpenBrace);
            {
                sb.AppendLine(code);
            }
            sb.AppendLine(Formatter.CloseBrace);

            return new GeneratedTableCode(ColumnMetadataClassName, ColumnMetadataClassName, sb.ToString());
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

            ret.Add(CreateCodeForColumnMetadata(codeNamespace));

            return ret;
        }

        protected abstract IEnumerable<DbColumnInfo> GetColumns(string table);

        protected virtual MethodParameter[] GetConstructorParameters(DbClassData cd)
        {
            var columnsArray = cd.Columns.ToArray();
            var parameters = new MethodParameter[columnsArray.Length];
            for (int i = 0; i < columnsArray.Length; i++)
            {
                DbColumnInfo column = columnsArray[i];
                MethodParameter p = new MethodParameter(cd.GetParameterName(column), cd.GetExternalType(column));
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