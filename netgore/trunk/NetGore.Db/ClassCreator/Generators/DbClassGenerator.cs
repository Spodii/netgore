using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// Base class for a class that generates the code to use to represent and perform queries with a database table.
    /// </summary>
    public abstract class DbClassGenerator : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Name of the class used to store the column metadata.
        /// </summary>
        public const string ColumnMetadataClassName = "ColumnMetadata";

        /// <summary>
        /// Name of the CopyValuesFrom method in the generated code.
        /// </summary>
        public const string CopyValuesFromMethodName = "CopyValuesFrom";

        /// <summary>
        /// Name of the CopyValues method in the generated code.
        /// </summary>
        public const string CopyValuesMethodName = "CopyValues";

        /// <summary>
        /// Name of the <see cref="IDataReader"/> when used in arguments in the generated code.
        /// </summary>
        public const string DataReaderName = "dataReader";

        /// <summary>
        /// Name of the <see cref="IDataRecord"/> when used in arguments in the generated code.
        /// </summary>
        public const string DataRecordName = "dataRecord";

        /// <summary>
        /// Name of the _dbColumns field in the generated code.
        /// </summary>
        public const string DbColumnsField = "_dbColumns";

        /// <summary>
        /// Name of the _dbColumnsKeys field in the generated code.
        /// </summary>
        public const string DbColumnsKeysField = "_dbColumnsKeys";

        /// <summary>
        /// Name of the _dbColumnsNonKeys field in the generated code.
        /// </summary>
        public const string DbColumnsNonKeysField = "_dbColumnsNonKey";

        /// <summary>
        /// Name of the HasSameValues method in the generated code.
        /// </summary>
        public const string HasSameValuesMethodName = "HasSameValues";

        /// <summary>
        /// Name of the ReadState method in the generated code.
        /// </summary>
        public const string ReadStateMethodName = "ReadState";

        /// <summary>
        /// Name of the TryCopyValues method in the generated code.
        /// </summary>
        public const string TryCopyValuesMethodName = "TryCopyValues";

        /// <summary>
        /// Name of the WriteState method in the generated code.
        /// </summary>
        public const string WriteStateMethodName = "WriteState";

        /// <summary>
        /// Name given to all extension method's first parameter.
        /// </summary>
        const string _extensionParamName = "source";

        static readonly IDictionary<Type, string> _valueReaderReadMethods;

        readonly List<ColumnCollection> _columnCollections = new List<ColumnCollection>();
        readonly List<CustomTypeMapping> _customTypes = new List<CustomTypeMapping>();

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
        /// Initializes the <see cref="DbClassGenerator"/> class.
        /// </summary>
        static DbClassGenerator()
        {
            _valueReaderReadMethods = new Dictionary<Type, string>
            {
                { typeof(byte), "ReadByte" },
                { typeof(sbyte), "ReadSByte" },
                { typeof(int), "ReadInt" },
                { typeof(uint), "ReadUInt" },
                { typeof(float), "ReadFloat" },
                { typeof(short), "ReadShort" },
                { typeof(ushort), "ReadUShort" },
                { typeof(long), "ReadLong" },
                { typeof(ulong), "ReadULong" },
                { typeof(double), "ReadDouble" },
                { typeof(bool), "ReadBool" },
                { typeof(string), "ReadString" }
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbClassGenerator"/> class.
        /// </summary>
        protected DbClassGenerator()
        {
            Formatter = new CSharpCodeFormatter();
            SetDataReaderReadMethod(typeof(float), "GetFloat");

            AddUsing("NetGore");
            AddUsing("NetGore.IO");
            AddUsing("System.Collections.Generic");
            AddUsing("System.Collections");
        }

        /// <summary>
        /// Gets the DbConnection used to connect to the database.
        /// </summary>
        public DbConnection DbConnection
        {
            get { return _dbConnction; }
        }

        /// <summary>
        /// Gets or sets the CodeFormatter to use. Default is the <see cref="CSharpCodeFormatter"/>.
        /// </summary>
        public CodeFormatter Formatter { get; set; }

        /// <summary>
        /// Gets or sets an IEnumerable of the name of the tables to generate. If this value is null or empty,
        /// all tables will be generated. Otherwise, only tables defined in this collection will be generated.
        /// </summary>
        public IEnumerable<string> TablesToGenerate { get; set; }

        /// <summary>
        /// Adds a column to the column collection.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="keyType">Type of the key.</param>
        /// <param name="externalType">The exposed type.</param>
        /// <param name="internalType">Type of the value and the internal storage type.</param>
        /// <param name="tables">The tables.</param>
        /// <param name="columns">The columns.</param>
        /// <exception cref="ArgumentException">Only Enums are supported for the keyType at the present.</exception>
        public void AddColumnCollection(string name, Type keyType, Type externalType, Type internalType,
                                        IEnumerable<string> tables, IEnumerable<ColumnCollectionItem> columns)
        {
            if (!(keyType.IsEnum))
                throw new ArgumentException("Only Enums are supported for the keyType at the present.", "keyType");

            var columnCollection = new ColumnCollection(name, keyType, externalType, internalType, tables, columns);
            _columnCollections.Add(columnCollection);
        }

        /// <summary>
        /// Adds a column to the column collection.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="keyType">Type of the key.</param>
        /// <param name="externalType">The exposed type.</param>
        /// <param name="internalType">Type of the value and the internal storage type.</param>
        /// <param name="table">The table.</param>
        /// <param name="columns">The columns.</param>
        /// <exception cref="ArgumentException">Only Enums are supported for the keyType at the present.</exception>
        public void AddColumnCollection(string name, Type keyType, Type externalType, Type internalType, string table,
                                        IEnumerable<ColumnCollectionItem> columns)
        {
            if (!(keyType.IsEnum))
            {
                const string errmsg = "Only Enums are supported for the keyType at the present.";
                throw new ArgumentException(errmsg, "keyType");
            }

            var columnCollection = new ColumnCollection(name, keyType, externalType, internalType, new string[] { table }, columns);
            _columnCollections.Add(columnCollection);
        }

        /// <summary>
        /// Applies the custom settings defined in classes that implement the <see cref="IDbClassGeneratorSettingsProvider"/>
        /// interface, using reflection to find the classes.
        /// </summary>
        /// <param name="throwOnCreateError">If set to true, classes that are instanceable (non-abstract) and implement
        /// <see cref="IDbClassGeneratorSettingsProvider"/>, but cannot be instantiated with an empty (default) constructor will
        /// throw an <see cref="TypeException"/>. If false, no exception will be thrown. In either case, classes that
        /// do not provide an empty constructor will not be able to be added.</param>
        public void AddCustomSettings(bool throwOnCreateError = true)
        {
            // Find the types
            var typeFilter = new TypeFilterCreator
            {
                Interfaces = new Type[] { typeof(IDbClassGeneratorSettingsProvider) },
                IsAbstract = false,
                IsClass = true,
                ConstructorParameters = Type.EmptyTypes,
                RequireConstructor = throwOnCreateError
            };

            var types = TypeHelper.FindTypes(typeFilter.GetFilter(), null);

            foreach (var type in types)
            {
                // Get the instance
                var instance = (IDbClassGeneratorSettingsProvider)TypeFactory.GetTypeInstance(type);

                // Apply the settings
                instance.ApplySettings(this);
            }
        }

        /// <summary>
        /// Adds a custom type to the generated code for specific column(s) on specific table(s).
        /// If no custom type is specified, the default system type for the database column will be used.
        /// The custom type must be able to be explicitly typed to and from the internal system type.
        /// </summary>
        /// <param name="type">The type to generate.</param>
        /// <param name="table">The table that contains the column or columns to use the <paramref name="type"/> for.</param>
        /// <param name="columns">The column or columns to use the custom <paramref name="type"/> on.</param>
        public void AddCustomType(Type type, string table, params string[] columns)
        {
            AddCustomType(type, new string[] { table }, columns);
        }

        /// <summary>
        /// Adds a custom type to the generated code for specific column(s) on specific table(s).
        /// If no custom type is specified, the default system type for the database column will be used.
        /// The custom type must be able to be explicitly typed to and from the internal system type.
        /// </summary>
        /// <param name="type">The name of the type to generate. The name needs to be included, too
        /// (e.g. <see cref="System.String"/> instead of just <see cref="String"/>).</param>
        /// <param name="table">The table that contains the column or columns to use the <paramref name="type"/> for.</param>
        /// <param name="columns">The column or columns to use the custom <paramref name="type"/> on.</param>
        public void AddCustomType(string type, string table, params string[] columns)
        {
            AddCustomType(type, new string[] { table }, columns);
        }

        /// <summary>
        /// Adds a custom type to the generated code for specific column(s) on specific table(s).
        /// If no custom type is specified, the default system type for the database column will be used.
        /// The custom type must be able to be explicitly typed to and from the internal system type.
        /// </summary>
        /// <param name="type">The type to generate.</param>
        /// <param name="tables">The tables that contains the column or columns to use the <paramref name="type"/> for.</param>
        /// <param name="columns">The column or columns to use the custom <paramref name="type"/> on.</param>
        public void AddCustomType(Type type, IEnumerable<string> tables, params string[] columns)
        {
            AddCustomType(Formatter.GetTypeString(type), tables, columns);
        }

        /// <summary>
        /// Adds a custom type to the generated code for specific column(s) on specific table(s).
        /// If no custom type is specified, the default system type for the database column will be used.
        /// The custom type must be able to be explicitly typed to and from the internal system type.
        /// </summary>
        /// <param name="type">The name of the type to generate. The name needs to be included, too
        /// (e.g. <see cref="System.String"/> instead of just <see cref="String"/>).</param>
        /// <param name="tables">The tables that contains the column or columns to use the <paramref name="type"/> for.</param>
        /// <param name="columns">The column or columns to use the custom <paramref name="type"/> on.</param>
        public void AddCustomType(string type, IEnumerable<string> tables, params string[] columns)
        {
            _customTypes.Add(new CustomTypeMapping(tables, columns, type));
        }

        /// <summary>
        /// Adds a Using directive to the generated code. This will be applied to all generated code files and added
        /// at the top of the generated code with the default Using directives.
        /// </summary>
        /// <param name="namespaceName">Namespace to use.</param>
        public void AddUsing(string namespaceName)
        {
            if (!_usings.Contains(namespaceName, StringComparer.Ordinal))
                _usings.Add(namespaceName);
        }

        /// <summary>
        /// Adds a Using directive to the generated code. This will be applied to all generated code files and added
        /// at the top of the generated code with the default Using directives.
        /// </summary>
        /// <param name="namespaceNames">Namespaces to use.</param>
        public void AddUsing(IEnumerable<string> namespaceNames)
        {
            foreach (var namespaceName in namespaceNames)
            {
                AddUsing(namespaceName);
            }
        }

        /// <summary>
        /// Creates all of the code files needed for a table.
        /// </summary>
        /// <param name="tableName">The name of the table to generate the code for.</param>
        /// <param name="columns">The columns in the table.</param>
        /// <param name="classNamespace">The namespace to use for the class.</param>
        /// <param name="interfaceNamespace">The namespace to use for the interface.</param>
        /// <returns>The generated code files for the table.</returns>
        protected virtual IEnumerable<GeneratedTableCode> CreateCode(string tableName, IEnumerable<DbColumnInfo> columns,
                                                                     string classNamespace, string interfaceNamespace)
        {
            columns = columns.OrderBy(x => x.Name);

            var cd = new DbClassData(tableName, columns, Formatter, _dataReaderReadMethods, _columnCollections, _customTypes);

            yield return
                new GeneratedTableCode(tableName, cd.ClassName, WrapCodeFile(CreateCodeForClass(cd), classNamespace, false),
                    GeneratedCodeType.Class);
            yield return
                new GeneratedTableCode(tableName, cd.InterfaceName,
                    WrapCodeFile(CreateCodeForInterface(cd), interfaceNamespace, true), GeneratedCodeType.Interface);

            yield return
                new GeneratedTableCode(tableName, cd.ExtensionClassName,
                    WrapCodeFile(CreateCodeForExtensions(cd), classNamespace, false), GeneratedCodeType.ClassDbExtensions);
        }

        /// <summary>
        /// Creates the code for the class.
        /// </summary>
        /// <param name="cd">Class data.</param>
        /// <returns>The code for the class.</returns>
        protected virtual string CreateCodeForClass(DbClassData cd)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("Creating class code for class `{0}` (Table: `{1}`).", cd.ClassName, cd.TableName);

            var sb = new StringBuilder(8192);

            sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateCode.ClassSummary, cd.TableName)));
            sb.AppendLine(Formatter.GetClass(cd.ClassName, MemberVisibilityLevel.Public, false,
                new string[] { cd.InterfaceName, cd.Formatter.GetTypeString(typeof(IPersistable)) }));
            sb.AppendLine(Formatter.OpenBrace);
            {
                // Other Fields/Properties
                {
                    if (log.IsDebugEnabled)
                        log.DebugFormat("Creating fields and properties.");

                    // All fields
                    var fieldNamesCode = Formatter.GetStringArrayCode(cd.Columns.Select(x => x.Name));
                    sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.ColumnArrayField));
                    sb.AppendLine(Formatter.GetField(DbColumnsField, typeof(string[]), MemberVisibilityLevel.Private,
                        fieldNamesCode, true, true));

                    sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.ColumnIEnumerableProperty));
                    sb.AppendLine(Formatter.GetProperty("DbColumns", typeof(IEnumerable<string>), typeof(IEnumerable<string>),
                        MemberVisibilityLevel.Public, null, DbColumnsField, false, true));
                }

                {
                    if (log.IsDebugEnabled)
                        log.DebugFormat("Creating key fields.");

                    // Key fields
                    var keyFieldNamesCode =
                        Formatter.GetStringArrayCode(
                            cd.Columns.Where(x => x.KeyType == DbColumnKeyType.Primary).Select(x => x.Name));
                    sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.KeyColumnArrayField));
                    sb.AppendLine(Formatter.GetField(DbColumnsKeysField, typeof(string[]), MemberVisibilityLevel.Private,
                        keyFieldNamesCode, true, true));

                    sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.KeyColumnIEnumerableProperty));
                    sb.AppendLine(Formatter.GetProperty("DbKeyColumns", typeof(IEnumerable<string>), typeof(IEnumerable<string>),
                        MemberVisibilityLevel.Public, null, DbColumnsKeysField, false, true));
                }

                {
                    if (log.IsDebugEnabled)
                        log.DebugFormat("Creating non-key fields.");

                    // Non-key fields
                    var nonKeyFieldNamesCode =
                        Formatter.GetStringArrayCode(
                            cd.Columns.Where(x => x.KeyType != DbColumnKeyType.Primary).Select(x => x.Name));
                    sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.NonKeyColumnArrayField));
                    sb.AppendLine(Formatter.GetField(DbColumnsNonKeysField, typeof(string[]), MemberVisibilityLevel.Private,
                        nonKeyFieldNamesCode, true, true));

                    sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.NonKeyColumnIEnumerableProperty));
                    sb.AppendLine(Formatter.GetProperty("DbNonKeyColumns", typeof(IEnumerable<string>),
                        typeof(IEnumerable<string>), MemberVisibilityLevel.Public, null, DbColumnsNonKeysField, false, true));
                }

                {
                    if (log.IsDebugEnabled)
                        log.DebugFormat("Creating fields for collections.");

                    // Collection fields
                    foreach (var coll in cd.ColumnCollections)
                    {
                        var privateFieldName = cd.GetPrivateName(coll) + "Columns";
                        var publicPropertyName = cd.GetPublicName(coll) + "Columns";

                        var currColl = coll;
                        var columnsInCollection = cd.Columns.Where(x => cd.GetCollectionForColumn(x) == currColl);
                        if (columnsInCollection.IsEmpty())
                            continue;

                        // Getter
                        var columnNamesCode = Formatter.GetStringArrayCode(columnsInCollection.Select(x => x.Name));
                        sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateCode.ColumnCollectionField, coll.Name)));
                        sb.AppendLine(Formatter.GetField(privateFieldName, typeof(string[]), MemberVisibilityLevel.Private,
                            columnNamesCode, true, true));

                        // Setter
                        sb.AppendLine(
                            Formatter.GetXmlComment(string.Format(Comments.CreateCode.ColumnCollectionProperty, coll.Name)));
                        sb.AppendLine(Formatter.GetProperty(publicPropertyName, typeof(IEnumerable<string>),
                            typeof(IEnumerable<string>), MemberVisibilityLevel.Public, null, privateFieldName, false, true));

                        // Collection
                        sb.AppendLine(
                            Formatter.GetXmlComment(string.Format(Comments.CreateCode.ColumnCollectionValueProperty, coll.Name)));
                        var ikvpType = Formatter.GetIEnumerableKeyValuePair(coll.KeyType, coll.ExternalType);
                        sb.AppendLine(Formatter.GetProperty(coll.CollectionPropertyName, ikvpType, ikvpType,
                            MemberVisibilityLevel.Public, null, cd.GetPrivateName(coll), false, false));
                    }
                }

                sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.TableName));
                sb.AppendLine(Formatter.GetConstField("TableName", typeof(string), MemberVisibilityLevel.Public,
                    "\"" + cd.TableName + "\""));

                sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.ColumnCount));
                sb.AppendLine(Formatter.GetConstField("ColumnCount", typeof(int), MemberVisibilityLevel.Public,
                    cd.Columns.Count().ToString()));

                // Properties for the interface implementation
                sb.AppendLine(CreateFields(cd));

                if (log.IsDebugEnabled)
                    log.DebugFormat("Creating method: DeepCopy()");

                // DeepCopy implementation
                sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.DeepCopySummary, Comments.CreateCode.DeepCopyReturn));
                sb.AppendLine(Formatter.GetMethodHeader("DeepCopy", MemberVisibilityLevel.Public, null, cd.InterfaceName, true,
                    false));
                sb.AppendLine(
                    Formatter.GetMethodBody("return new " + cd.ClassName + Formatter.OpenParameterString + "this" +
                                            Formatter.CloseParameterString + Formatter.EndOfLine));

                if (log.IsDebugEnabled)
                    log.DebugFormat("Creating method: .ctor()");

                // Constructor (empty)
                sb.AppendLine(CreateConstructor(cd, string.Empty, false));

                if (log.IsDebugEnabled)
                    log.DebugFormat("Creating method: .ctor(...)");

                // Constructor (full)
                var fullConstructorBody = FullConstructorMemberBody(cd);
                sb.AppendLine(CreateConstructor(cd, fullConstructorBody, true));

                if (log.IsDebugEnabled)
                    log.DebugFormat("Creating method: .ctor([class interface])");

                // Constructor (self-referencing interface)
                var sriConstructorParams = new MethodParameter[] { new MethodParameter("source", cd.InterfaceName) };
                sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateCode.ConstructorSummary, cd.ClassName), null,
                    new KeyValuePair<string, string>("source",
                        string.Format(Comments.CreateCode.ConstructorInterfaceParameter, cd.InterfaceName))));
                sb.AppendLine(Formatter.GetConstructorHeader(cd.ClassName, MemberVisibilityLevel.Public, sriConstructorParams));
                sb.AppendLine(Formatter.GetMethodBody(Formatter.GetCallMethod(CopyValuesFromMethodName, "source")));

                // Methods
                sb.AppendLine(CreateMethodCopyValuesToDict(cd));
                sb.AppendLine(CreateMethodCopyValuesFrom(cd));
                sb.AppendLine(CreateMethodGetValue(cd));
                sb.AppendLine(CreateMethodSetValue(cd));
                sb.AppendLine(CreateMethodGetColumnData(cd));
                sb.AppendLine(CreateMethodReadState(cd));
                sb.AppendLine(CreateMethodWriteState(cd));
            }
            sb.AppendLine(Formatter.CloseBrace);

            return sb.ToString();
        }

        /// <summary>
        /// Creates the class for the class that holds the column metadata.
        /// </summary>
        /// <param name="codeNamespace">The namespace to use for the generated code.</param>
        /// <returns>The generated code.</returns>
        protected virtual GeneratedTableCode CreateCodeForColumnMetadata(string codeNamespace)
        {
            var code = DbResources.ColumnMetadataCode;
            var sb = new StringBuilder(code.Length + 200);

            sb.AppendLine(Formatter.GetUsing("System"));

            sb.AppendLine(Formatter.GetNamespace(codeNamespace));
            sb.AppendLine(Formatter.OpenBrace);
            {
                sb.AppendLine(code);
            }
            sb.AppendLine(Formatter.CloseBrace);

            return new GeneratedTableCode(ColumnMetadataClassName, ColumnMetadataClassName, sb.ToString(),
                GeneratedCodeType.ColumnMetadata);
        }

        /// <summary>
        /// Creates the code files for the const dictionaries, which are look-up tables for enums used to reference
        /// database columns by keys instead of each individual column explicitly.
        /// </summary>
        /// <param name="interfaceNamespace">The namespace to use.</param>
        /// <returns>The generated code.</returns>
        protected virtual IEnumerable<GeneratedTableCode> CreateCodeForConstDictionaries(string interfaceNamespace)
        {
            // ConstEnumDictionary class
            foreach (var cc in _columnCollections.Distinct(new ColumnCollectionDistinctKeyComparer()))
            {
                var code = WrapCodeFile(GetConstEnumDictonaryCode(cc), interfaceNamespace, false);
                yield return
                    new GeneratedTableCode(string.Empty, GetConstEnumDictonaryName(cc), code,
                        GeneratedCodeType.ColumnCollectionClass);
            }
        }

        /// <summary>
        /// Creates the code for the class containing the extension methods for each class for the database tables.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <returns>The generated code.</returns>
        protected virtual string CreateCodeForExtensions(DbClassData cd)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("Creating class code for extension class `{0}` (Table: `{1}`).", cd.ClassName, cd.TableName);

            var sb = new StringBuilder(8192);

            sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateCode.ExtensionClassSummary, cd.ClassName)));
            sb.AppendLine(Formatter.GetClass(cd.ExtensionClassName, MemberVisibilityLevel.Public, true, null));
            sb.AppendLine(Formatter.OpenBrace);
            {
                // Extension methods
                sb.AppendLine(CreateMethodCopyValuesToDbParameterValues(cd));
                sb.AppendLine(CreateMethodReadValues(cd));
                sb.AppendLine(CreateMethodTryReadValues(cd));
                sb.AppendLine(CreateMethodTryCopyValuesToDbParameterValues(cd));
                sb.AppendLine(CreateMethodHasSameValues(cd));
            }
            sb.AppendLine(Formatter.CloseBrace);

            return sb.ToString();
        }

        /// <summary>
        /// Creates the code for the interface.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <returns>The class code for the interface.</returns>
        protected virtual string CreateCodeForInterface(DbClassData cd)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("Creating class code for interface `{0}` (Table: `{1}`).", cd.ClassName, cd.TableName);

            var sb = new StringBuilder(2048);

            sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateCode.InterfaceSummary, cd.TableName)));
            sb.AppendLine(Formatter.GetInterface(cd.InterfaceName, MemberVisibilityLevel.Public));
            sb.AppendLine(Formatter.OpenBrace);
            {
                // DeepCopy
                sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.DeepCopySummary, Comments.CreateCode.DeepCopyReturn));
                sb.AppendLine(Formatter.GetInterfaceMethod("DeepCopy", cd.InterfaceName));

                // Columns
                var addedCollections = new List<ColumnCollection>();
                foreach (var column in cd.Columns)
                {
                    ColumnCollectionItem collItem;
                    var coll = cd.GetCollectionForColumn(column, out collItem);

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
                        var name = cd.GetPublicName(coll);
                        var keyParameter = new MethodParameter("key", coll.KeyType, Formatter);

                        // Getter
                        sb.AppendLine(
                            Formatter.GetXmlComment(string.Format(Comments.CreateCode.InterfaceCollectionGetter, coll.Name),
                                Comments.CreateCode.InterfaceCollectionReturns,
                                new KeyValuePair<string, string>("key", Comments.CreateCode.InterfaceCollectionParamKey)));
                        sb.AppendLine(Formatter.GetInterfaceMethod("Get" + name, coll.ExternalType, keyParameter));

                        // Collection
                        sb.AppendLine(
                            Formatter.GetXmlComment(string.Format(Comments.CreateCode.ColumnCollectionValueProperty, coll.Name)));
                        sb.AppendLine(Formatter.GetInterfaceProperty(coll.CollectionPropertyName,
                            Formatter.GetIEnumerableKeyValuePair(coll.KeyType, coll.ExternalType), false));
                    }
                }
            }
            sb.AppendLine(Formatter.CloseBrace);

            return sb.ToString();
        }

        /// <summary>
        /// Creates the code for a constructor.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <param name="code">The code to include in the constructor body.</param>
        /// <param name="addParameters">The constructor parameters.</param>
        /// <returns>The generated block of code.</returns>
        protected virtual string CreateConstructor(DbClassData cd, string code, bool addParameters)
        {
            // Get the parameters
            MethodParameter[] parameters;
            if (addParameters)
                parameters = GetConstructorParameters(cd);
            else
                parameters = MethodParameter.Empty;

            // Turn parameters into a KeyValuePair
            var cParams = new List<KeyValuePair<string, string>>(Math.Max(1, parameters.Length));
            cParams.AddRange(parameters.Select(p => new KeyValuePair<string, string>(p.Name, Comments.CreateConstructor.Parameter)));

            // Generate the code
            var sb = new StringBuilder(2048);

            sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateCode.ConstructorSummary, cd.ClassName), null,
                cParams.ToArray()));

            sb.AppendLine(Formatter.GetConstructorHeader(cd.ClassName, MemberVisibilityLevel.Public, parameters));

            sb.Append(Formatter.GetMethodBody(code));

            return sb.ToString();
        }

        /// <summary>
        /// Generates the code for the fields.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <returns>The generated code.</returns>
        protected virtual string CreateFields(DbClassData cd)
        {
            var sb = new StringBuilder(2048);

            // Column fields
            var addedCollections = new List<ColumnCollection>();
            foreach (var column in cd.Columns)
            {
                ColumnCollectionItem collItem;
                var coll = cd.GetCollectionForColumn(column, out collItem);

                if (coll == null)
                {
                    // Not part of a collection
                    var comment = string.Format(Comments.CreateFields.Field, column.Name);
                    sb.AppendLine(Formatter.GetXmlComment(comment));
                    sb.AppendLine(Formatter.GetField(cd.GetPrivateName(column), cd.GetInternalType(column),
                        MemberVisibilityLevel.Private));
                }
                else if (!addedCollections.Contains(coll))
                {
                    // Has a collection - only add the code if the collection hasn't been added yet
                    addedCollections.Add(coll);
                    sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateFields.CollectionField, coll.Name)));
                    var collType = GetConstEnumDictonaryName(coll);
                    sb.AppendLine(Formatter.GetField(cd.GetPrivateName(coll), collType, MemberVisibilityLevel.Private,
                        "new " + collType + Formatter.OpenParameterString + Formatter.CloseParameterString, true, false));
                }
            }

            // Properties for the fields
            addedCollections.Clear();
            foreach (var column in cd.Columns)
            {
                ColumnCollectionItem collItem;
                var coll = cd.GetCollectionForColumn(column, out collItem);

                if (coll == null)
                {
                    // Not part of a collection
                    var comment = string.Format(Comments.CreateFields.Property, column.Name, column.DatabaseType);

                    if (column.DefaultValue != null && !string.IsNullOrEmpty(column.DefaultValue.ToString()))
                        comment += string.Format(Comments.CreateFields.PropertyHasDefaultValue, column.DefaultValue);
                    else
                        comment += Comments.CreateFields.PropertyNoDefaultValue;

                    if (!string.IsNullOrEmpty(column.Comment))
                        comment += string.Format(Comments.CreateFields.PropertyDbComment, column.Comment);

                    sb.AppendLine(Formatter.GetXmlComment(comment));

                    if (!string.IsNullOrEmpty(column.Comment))
                        sb.AppendLine(Formatter.GetAttribute("System.ComponentModel.Description",
                            "\"" + column.Comment.Replace("\"", "\\\"") + "\""));

                    sb.AppendLine(Formatter.GetAttribute(typeof(SyncValueAttribute)));
                    sb.AppendLine(Formatter.GetProperty(cd.GetPublicName(column), cd.GetExternalType(column),
                        cd.GetInternalType(column), MemberVisibilityLevel.Public, MemberVisibilityLevel.Public,
                        cd.GetPrivateName(column), false, false));
                }
                else if (!addedCollections.Contains(coll))
                {
                    // Has a collection - only add the code if the collection hasn't been added yet
                    addedCollections.Add(coll);

                    var name = cd.GetPublicName(coll);
                    var keyParameter = new MethodParameter("key", coll.KeyType, Formatter);
                    var field = cd.GetPrivateName(coll) + Formatter.OpenIndexer + Formatter.GetCast(coll.KeyType) + "key" +
                                Formatter.CloseIndexer;

                    // Getter
                    sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateFields.PublicMethodGet, coll.Name),
                        Comments.CreateFields.PublicMethodGetReturns,
                        new KeyValuePair<string, string>("key", Comments.CreateFields.PublicMethodGetKeyParameter)));
                    sb.AppendLine(Formatter.GetMethodHeader("Get" + name, MemberVisibilityLevel.Public,
                        new MethodParameter[] { keyParameter }, coll.ExternalType, false, false));
                    sb.AppendLine(
                        Formatter.GetMethodBody("return " + Formatter.GetCast(coll.ExternalType) + field + Formatter.EndOfLine));

                    // Setter
                    sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateFields.PublicMethodGet, coll.Name), null,
                        new KeyValuePair<string, string>("key", Comments.CreateFields.PublicMethodGetKeyParameter),
                        new KeyValuePair<string, string>("value", Comments.CreateFields.PublicMethodValueParameter)));
                    sb.AppendLine(Formatter.GetMethodHeader("Set" + name, MemberVisibilityLevel.Public,
                        new MethodParameter[] { keyParameter, new MethodParameter("value", coll.ExternalType, Formatter) },
                        typeof(void), false, false));

                    sb.AppendLine(Formatter.GetMethodBody(Formatter.GetSetValue(field, "value", true, false, coll.InternalType)));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generates the code for the CopyValuesFrom method.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <returns>The generated code.</returns>
        protected virtual string CreateMethodCopyValuesFrom(DbClassData cd)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("Creating method: {0}()", CopyValuesFromMethodName);

            const string sourceName = "source";

            var parameters = new MethodParameter[] { new MethodParameter(sourceName, cd.InterfaceName) };

            var sb = new StringBuilder(2048);

            // Header
            sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CopyValuesFrom.Summary, cd.ClassName), null,
                new KeyValuePair<string, string>(sourceName,
                    string.Format(Comments.CopyValuesFrom.SourceParameter, cd.InterfaceName))));
            sb.AppendLine(Formatter.GetMethodHeader(CopyValuesFromMethodName, MemberVisibilityLevel.Public, parameters,
                typeof(void), false, false));

            // Body
            var bodySB = new StringBuilder(2048);
            foreach (var column in cd.Columns)
            {
                bodySB.AppendLine(cd.GetColumnValueMutator(column, sourceName + "." + cd.GetColumnValueAccessor(column)));
            }
            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        /// <summary>
        /// Generates the code for the CopyValuesTo method with a DbParameterValues parameter.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <returns>The generated code.</returns>
        protected virtual string CreateMethodCopyValuesToDbParameterValues(DbClassData cd)
        {
            const string parameterName = "paramValues";

            var sb = new StringBuilder(2048);

            // Header
            sb.AppendLine(Formatter.GetXmlComment(Comments.CopyToDPV.Summary, null,
                new KeyValuePair<string, string>(_extensionParamName, Comments.CopyToDPV.ParameterSource),
                new KeyValuePair<string, string>(parameterName, Comments.CopyToDPV.ParameterDbParameterValues)));
            sb.AppendLine(Formatter.GetExtensionMethodHeader(CopyValuesMethodName,
                new MethodParameter(_extensionParamName, cd.InterfaceName),
                new MethodParameter[] { new MethodParameter(parameterName, typeof(DbParameterValues), Formatter) }, typeof(void)));

            // Body
            var bodySB = new StringBuilder(2048);
            foreach (var column in cd.Columns)
            {
                var left = parameterName + "[\"" + column.Name + "\"]";
                var right = _extensionParamName + "." + cd.GetColumnValueAccessor(column);
                var line = Formatter.GetSetValue(left, right, false, false, cd.GetInternalType(column));
                bodySB.AppendLine(line);
            }
            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        /// <summary>
        /// Generates the code for the CopyValuesTo method with a dictionary parameter.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <returns>The generated code.</returns>
        protected virtual string CreateMethodCopyValuesToDict(DbClassData cd)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("Creating method: {0}()", CopyValuesMethodName);

            const string parameterName = "dic";
            const string sourceName = "source";

            var iParameters = new MethodParameter[]
            { new MethodParameter(parameterName, typeof(IDictionary<string, object>), Formatter) };
            var sParameters =
                new MethodParameter[] { new MethodParameter(sourceName, cd.InterfaceName) }.Concat(iParameters).ToArray();

            var sb = new StringBuilder(2048);

            // Instanced header
            sb.AppendLine(Formatter.GetXmlComment(Comments.CopyToDict.Summary, null,
                new KeyValuePair<string, string>(parameterName, Comments.CopyToDict.ParameterDict)));

            sb.AppendLine(Formatter.GetMethodHeader(CopyValuesMethodName, MemberVisibilityLevel.Public, iParameters, typeof(void),
                false, false));

            // Instanced body
            sb.AppendLine(Formatter.GetMethodBody(Formatter.GetCallMethod(CopyValuesMethodName, "this", parameterName)));

            // Static hader
            sb.AppendLine(Formatter.GetXmlComment(Comments.CopyToDict.Summary, null,
                new KeyValuePair<string, string>(sourceName, Comments.CopyToDict.ParameterSource),
                new KeyValuePair<string, string>(parameterName, Comments.CopyToDict.ParameterDict)));

            sb.AppendLine(Formatter.GetMethodHeader(CopyValuesMethodName, MemberVisibilityLevel.Public, sParameters, typeof(void),
                false, true));

            // Static body
            var bodySB = new StringBuilder(2048);
            foreach (var column in cd.Columns)
            {
                var left = parameterName + "[\"" + column.Name + "\"]";
                var right = sourceName + "." + cd.GetColumnValueAccessor(column);
                var line = Formatter.GetSetValue(left, right, false, false, cd.GetExternalType(column));
                bodySB.AppendLine(line);
            }
            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        /// <summary>
        /// Generates the code for the GetColumnData method.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <returns>The generated code.</returns>
        protected virtual string CreateMethodGetColumnData(DbClassData cd)
        {
            const string methodName = "GetColumnData";
            const string parameterName = "columnName";

            if (log.IsDebugEnabled)
                log.DebugFormat("Creating method: {0}()", methodName);

            var parameters = new MethodParameter[] { new MethodParameter(parameterName, typeof(string), Formatter) };

            var sb = new StringBuilder(4096);

            // Header
            sb.AppendLine(Formatter.GetXmlComment(Comments.GetColumnData.Summary, Comments.GetColumnData.Returns,
                new KeyValuePair<string, string>(parameterName, Comments.GetColumnData.ColumnNameParameter)));
            sb.AppendLine(Formatter.GetMethodHeader(methodName, MemberVisibilityLevel.Public, parameters, ColumnMetadataClassName,
                false, true));

            // Body
            var switches =
                cd.Columns.Select(
                    x => new KeyValuePair<string, string>("\"" + x.Name + "\"", CreateMethodGetColumnDataReturnString(x)));
            var defaultCode = "throw new ArgumentException(\"Field not found.\",\"" + parameterName + "\")" + Formatter.EndOfLine;
            sb.AppendLine(Formatter.GetMethodBody(Formatter.GetSwitch(parameterName, switches, defaultCode)));

            return sb.ToString();
        }

        /// <summary>
        /// Generates the code for a single column in the GetMethodData method, which contains the construction and
        /// return of an ColumnMetadataClass instance. To be used by <see cref="CreateMethodGetColumnData"/>.
        /// </summary>
        /// <param name="column">The column to create the line for.</param>
        /// <returns>The generated code.</returns>
        /// <seealso cref="CreateMethodGetColumnData"/>
        protected string CreateMethodGetColumnDataReturnString(DbColumnInfo column)
        {
            var sb = new StringBuilder(256);

            sb.Append(Formatter.ReturnString);
            sb.Append(" ");
            sb.Append("new ");
            sb.Append(ColumnMetadataClassName);
            sb.Append(Formatter.OpenParameterString);
            sb.Append("\"" + column.Name + "\"" + Formatter.ParameterSpacer);
            sb.Append("\"" + column.Comment.Replace("\"", "\\\"") + "\"" + Formatter.ParameterSpacer);
            sb.Append("\"" + column.DatabaseType.Replace("\"", "\\\"") + "\"" + Formatter.ParameterSpacer);

            if (column.DefaultValue == null ||
                (string.IsNullOrEmpty(column.DefaultValue.ToString()) && !(column.DefaultValue is string)))
                sb.Append("null" + Formatter.ParameterSpacer);
            else if (column.DefaultValue is string)
                sb.Append("\"" + column.DefaultValue.ToString().Replace("\"", "\\\"") + "\"" + Formatter.ParameterSpacer);
            else
                sb.Append(column.DefaultValue + Formatter.ParameterSpacer);

            sb.Append("typeof(" + Formatter.GetTypeString(column.Type) + ")" + Formatter.ParameterSpacer);
            sb.Append(column.IsNullable.ToString().ToLower() + Formatter.ParameterSpacer);
            sb.Append((column.KeyType == DbColumnKeyType.Primary).ToString().ToLower() + Formatter.ParameterSpacer);
            sb.Append((column.KeyType == DbColumnKeyType.Foreign).ToString().ToLower());
            sb.Append(Formatter.CloseParameterString);
            sb.Append(Formatter.EndOfLine);

            return sb.ToString();
        }

        /// <summary>
        /// Generates the code for the GetValue method.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <returns>The generated code.</returns>
        protected virtual string CreateMethodGetValue(DbClassData cd)
        {
            const string parameterName = "columnName";
            const string methodName = "GetValue";

            if (log.IsDebugEnabled)
                log.DebugFormat("Creating method: {0}()", methodName);

            var parameters = new MethodParameter[] { new MethodParameter(parameterName, typeof(string), Formatter) };

            var sb = new StringBuilder(2048);

            // Header
            sb.AppendLine(Formatter.GetXmlComment(Comments.GetValue.Summary, Comments.GetValue.Returns,
                new KeyValuePair<string, string>(parameterName, Comments.GetValue.ColumnNameParameter)));
            sb.AppendLine(Formatter.GetMethodHeader(methodName, MemberVisibilityLevel.Public, parameters, typeof(object), false,
                false));

            // Body
            var switches =
                cd.Columns.Select(
                    x =>
                    new KeyValuePair<string, string>("\"" + x.Name + "\"",
                        Formatter.ReturnString + " " + cd.GetColumnValueAccessor(x) + Formatter.EndOfLine));
            var defaultCode = "throw new ArgumentException(\"Field not found.\",\"" + parameterName + "\")" + Formatter.EndOfLine;
            sb.AppendLine(Formatter.GetMethodBody(Formatter.GetSwitch(parameterName, switches, defaultCode)));

            return sb.ToString();
        }

        /// <summary>
        /// Generates the code for the HasSameValues method.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <returns>The generated code.</returns>
        protected virtual string CreateMethodHasSameValues(DbClassData cd)
        {
            const string otherName = "otherItem";

            var parameters = new MethodParameter[] { new MethodParameter(otherName, cd.InterfaceName) };

            var sb = new StringBuilder(2048);

            // Header
            sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.HasSameValues.Summary, cd.InterfaceName),
                string.Format(Comments.HasSameValues.Returns, cd.InterfaceName),
                new KeyValuePair<string, string>(_extensionParamName,
                    string.Format(Comments.HasSameValues.SourceParameter, cd.InterfaceName)),
                new KeyValuePair<string, string>(otherName, string.Format(Comments.HasSameValues.OtherParameter, cd.InterfaceName))));

            sb.AppendLine(Formatter.GetExtensionMethodHeader(HasSameValuesMethodName,
                new MethodParameter(_extensionParamName, cd.InterfaceName), parameters, typeof(bool)));

            // Body
            var bodySB = new StringBuilder(2048);
            bodySB.Append(Formatter.ReturnString + " ");
            foreach (var column in cd.Columns)
            {
                var accStr = cd.GetColumnValueAccessor(column);
                var left = _extensionParamName + "." + accStr;
                var right = otherName + "." + accStr;

                bodySB.Append("Equals");
                bodySB.Append(Formatter.OpenParameterString);
                bodySB.Append(left);
                bodySB.Append(Formatter.ParameterSpacer);
                bodySB.Append(right);
                bodySB.Append(Formatter.CloseParameterString);

                if (column != cd.Columns.Last())
                {
                    bodySB.Append(" && ");
                    bodySB.AppendLine();
                }
                else
                    bodySB.Append(Formatter.EndOfLine);
            }

            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        /// <summary>
        /// Generates the code for the ReadState method.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <returns>The generated code.</returns>
        protected virtual string CreateMethodReadState(DbClassData cd)
        {
            const string parameterName = "reader";

            if (log.IsDebugEnabled)
                log.DebugFormat("Creating method: {0}()", ReadStateMethodName);

            var sb = new StringBuilder(2048);

            // Header
            sb.AppendLine(Formatter.GetXmlComment(Comments.ReadState.Summary, null,
                new KeyValuePair<string, string>(parameterName, Comments.ReadState.ParameterWriter)));
            sb.AppendLine(Formatter.GetMethodHeader(ReadStateMethodName, MemberVisibilityLevel.Public,
                new MethodParameter[] { new MethodParameter(parameterName, typeof(IValueReader), cd.Formatter) }, typeof(void),
                true, false));

            // Body
            var bodySB = new StringBuilder(2048);
            var methodCall = Formatter.GetCallObjMethod(Formatter.GetTypeString(typeof(PersistableHelper)), "Read", "this",
                parameterName);

            foreach (var cc in cd.ColumnCollections)
            {
                var key = "\"" + cc.CollectionPropertyName + "\"";
                var readNodeCall = Formatter.GetCallObjMethod(parameterName, "ReadNode", key);
                bodySB.Append(Formatter.GetCallObjMethod(cd.GetPrivateName(cc), "ReadState", readNodeCall));
            }

            bodySB.AppendLine();
            bodySB.Append(bodySB);
            sb.AppendLine(Formatter.GetMethodBody(methodCall));

            return sb.ToString();
        }

        /// <summary>
        /// Generates the code for the ReadValues method.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <returns>The generated code.</returns>
        protected virtual string CreateMethodReadValues(DbClassData cd)
        {
            var sb = new StringBuilder(2048);

            // Header
            sb.AppendLine(Formatter.GetXmlComment(Comments.ReadValues.Summary, null,
                new KeyValuePair<string, string>(_extensionParamName, Comments.Extensions.ExtensionParameter),
                new KeyValuePair<string, string>(DataRecordName, Comments.ReadValues.ParameterDataRecord)));

            sb.AppendLine(Formatter.GetExtensionMethodHeader("ReadValues", new MethodParameter(_extensionParamName, cd.ClassName),
                new MethodParameter[] { new MethodParameter(DataRecordName, typeof(IDataRecord), Formatter) }, typeof(void)));

            // Body
            var bodySB = new StringBuilder(2048);
            bodySB.AppendLine(Formatter.GetLocalField("i", typeof(int), null));
            bodySB.AppendLine();
            foreach (var column in cd.Columns)
            {
                bodySB.AppendLine(Formatter.GetSetValue("i",
                    Formatter.GetCallObjMethod(DataRecordName, "GetOrdinal", "\"" + column.Name + "\""), false, false));

                var right = cd.GetDataReaderAccessor(column, "i", DataRecordName);
                bodySB.AppendLine(cd.GetColumnValueMutator(column, right, _extensionParamName));
                bodySB.AppendLine();
            }

            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        /// <summary>
        /// Generates the code for the SetValue method.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <returns>The generated code.</returns>
        protected virtual string CreateMethodSetValue(DbClassData cd)
        {
            const string parameterName = "columnName";
            const string valueName = "value";
            const string methodName = "SetValue";

            if (log.IsDebugEnabled)
                log.DebugFormat("Creating method: {0}()", methodName);

            var parameters = new MethodParameter[]
            {
                new MethodParameter(parameterName, typeof(string), Formatter),
                new MethodParameter(valueName, typeof(object), Formatter)
            };

            var sb = new StringBuilder(2048);

            // Header
            sb.AppendLine(Formatter.GetXmlComment(Comments.SetValue.Summary, null,
                new KeyValuePair<string, string>(parameterName, Comments.SetValue.ColumnNameParameter),
                new KeyValuePair<string, string>(valueName, Comments.SetValue.ValueParameter)));
            sb.AppendLine(Formatter.GetMethodHeader(methodName, MemberVisibilityLevel.Public, parameters, typeof(void), false,
                false));

            // Body
            var switches =
                cd.Columns.Select(
                    x =>
                    new KeyValuePair<string, string>("\"" + x.Name + "\"",
                        cd.GetColumnValueMutator(x, valueName) + Environment.NewLine + "break" + Formatter.EndOfLine));
            var defaultCode = "throw new ArgumentException(\"Field not found.\",\"" + parameterName + "\")" + Formatter.EndOfLine;
            sb.AppendLine(Formatter.GetMethodBody(Formatter.GetSwitch(parameterName, switches, defaultCode)));

            return sb.ToString();
        }

        /// <summary>
        /// Generates the code for the TryCopyValues method with a DbParameterValues as the parameter.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <returns>The generated code.</returns>
        protected virtual string CreateMethodTryCopyValuesToDbParameterValues(DbClassData cd)
        {
            const string parameterName = "paramValues";

            var sb = new StringBuilder(2048);

            // Header
            sb.AppendLine(Formatter.GetXmlComment(Comments.TryCopyValues.Summary, null,
                new KeyValuePair<string, string>(_extensionParamName, Comments.TryCopyValues.ParameterSource),
                new KeyValuePair<string, string>(parameterName, Comments.TryCopyValues.ParameterDbParameterValues)));

            sb.AppendLine(Formatter.GetExtensionMethodHeader(TryCopyValuesMethodName,
                new MethodParameter(_extensionParamName, cd.InterfaceName),
                new MethodParameter[] { new MethodParameter(parameterName, typeof(DbParameterValues), Formatter) }, typeof(void)));

            // Body
            var bodySB = new StringBuilder(2048);
            bodySB.AppendLine("for (int i = 0; i < " + parameterName + ".Count; i++)");
            bodySB.AppendLine(Formatter.OpenBrace);
            {
                bodySB.AppendLine(Formatter.GetSwitch(parameterName + ".GetParameterName(i)",
                    cd.Columns.Select(
                        x =>
                        new KeyValuePair<string, string>("\"" + x.Name + "\"",
                            CreateMethodTryCopyValuesToDbParameterValuesSwitchString(cd, x, parameterName))), null));
            }
            bodySB.Append(Formatter.CloseBrace);

            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        /// <summary>
        /// Generates the code for getting the values for a single column. To be used by
        /// <see cref="CreateMethodTryCopyValuesToDbParameterValues"/>.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <param name="column">The column.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>The generated code.</returns>
        /// <seealso cref="CreateMethodTryCopyValuesToDbParameterValues"/>
        protected string CreateMethodTryCopyValuesToDbParameterValuesSwitchString(DbClassData cd, DbColumnInfo column,
                                                                                  string parameterName)
        {
            var sb = new StringBuilder();
            sb.AppendLine(Formatter.GetSetValue(parameterName + Formatter.OpenIndexer + "i" + Formatter.CloseIndexer,
                _extensionParamName + "." + cd.GetColumnValueAccessor(column), false, false, cd.GetInternalType(column)));
            sb.AppendLine("break" + Formatter.EndOfLine);
            return sb.ToString();
        }

        /// <summary>
        /// Generates the code for the TryReadValues method.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <returns>The generated code.</returns>
        protected virtual string CreateMethodTryReadValues(DbClassData cd)
        {
            var sb = new StringBuilder(2048);

            // Header
            sb.AppendLine(Formatter.GetXmlComment(Comments.TryReadValues.Summary, null,
                new KeyValuePair<string, string>(_extensionParamName, Comments.Extensions.ExtensionParameter),
                new KeyValuePair<string, string>(DataRecordName, Comments.TryReadValues.ParameterDataReader)));

            sb.AppendLine(Formatter.GetExtensionMethodHeader("TryReadValues",
                new MethodParameter(_extensionParamName, cd.ClassName),
                new MethodParameter[] { new MethodParameter(DataRecordName, typeof(IDataRecord), Formatter) }, typeof(void)));

            // Body
            var bodySB = new StringBuilder(2048);
            bodySB.AppendLine("for (int i = 0; i < " + DataRecordName + ".FieldCount; i++)");
            bodySB.AppendLine(Formatter.OpenBrace);
            {
                bodySB.AppendLine(Formatter.GetSwitch(DataRecordName + ".GetName(i)",
                    cd.Columns.Select(
                        x => new KeyValuePair<string, string>("\"" + x.Name + "\"", CreateMethodTryReadValuesSwitchString(cd, x))),
                    null));
            }
            bodySB.Append(Formatter.CloseBrace);

            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        /// <summary>
        /// Creates the method for reading a value of a column. To be used by <see cref="CreateMethodTryReadValues"/>.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <param name="column">The <see cref="DbColumnInfo"/>.</param>
        /// <returns>The generated code.</returns>
        protected string CreateMethodTryReadValuesSwitchString(DbClassData cd, DbColumnInfo column)
        {
            var sb = new StringBuilder();
            var valueName = cd.GetDataReaderAccessor(column, "i", DataRecordName);
            var s = cd.GetColumnValueMutator(column, valueName, _extensionParamName);
            sb.AppendLine(s);
            sb.AppendLine("break" + Formatter.EndOfLine);
            return sb.ToString();
        }

        /// <summary>
        /// Generates the code for the WriteState method.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <returns>The generated code.</returns>
        protected virtual string CreateMethodWriteState(DbClassData cd)
        {
            const string parameterName = "writer";

            if (log.IsDebugEnabled)
                log.DebugFormat("Creating method: {0}()", WriteStateMethodName);

            var sb = new StringBuilder(2048);

            // Header
            sb.AppendLine(Formatter.GetXmlComment(Comments.WriteState.Summary, null,
                new KeyValuePair<string, string>(parameterName, Comments.WriteState.ParameterWriter)));
            sb.AppendLine(Formatter.GetMethodHeader(WriteStateMethodName, MemberVisibilityLevel.Public,
                new MethodParameter[] { new MethodParameter(parameterName, typeof(IValueWriter), cd.Formatter) }, typeof(void),
                true, false));

            // Body
            var bodySB = new StringBuilder(2048);
            var methodCall = Formatter.GetCallObjMethod(Formatter.GetTypeString(typeof(PersistableHelper)), "Write", "this",
                parameterName);

            foreach (var cc in cd.ColumnCollections)
            {
                var key = "\"" + cc.CollectionPropertyName + "\"";
                bodySB.Append(Formatter.GetCallObjMethod(parameterName, "WriteStartNode", key));
                bodySB.Append(Formatter.GetCallObjMethod(cd.GetPrivateName(cc), "Write", parameterName));
                bodySB.Append(Formatter.GetCallObjMethod(parameterName, "WriteEndNode", key));
                bodySB.AppendLine();
            }

            bodySB.Append(bodySB);
            sb.AppendLine(Formatter.GetMethodBody(methodCall));

            return sb.ToString();
        }

        /// <summary>
        /// Generates the code for the body of the constructor containing all of the parameters.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <returns>The generated code.</returns>
        protected virtual string FullConstructorMemberBody(DbClassData cd)
        {
            var sb = new StringBuilder(1024);
            foreach (var column in cd.Columns)
            {
                var right = cd.GetParameterName(column);
                sb.AppendLine(cd.GetColumnValueMutator(column, right));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Starts the code generation.
        /// </summary>
        /// <param name="classNamespace">The class namespace to use.</param>
        /// <param name="interfaceNamespace">The interface namespace to use.</param>
        /// <param name="classOutputDir">The output directory for class files.</param>
        /// <param name="interfaceOutputDir">The output directory for interface files.</param>
        public virtual void Generate(string classNamespace, string interfaceNamespace, string classOutputDir,
                                     string interfaceOutputDir)
        {
            if (!classOutputDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                classOutputDir += Path.DirectorySeparatorChar.ToString();

            if (!interfaceOutputDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                interfaceOutputDir += Path.DirectorySeparatorChar.ToString();

            if (!Directory.Exists(classOutputDir))
                Directory.CreateDirectory(classOutputDir);

            if (!Directory.Exists(interfaceOutputDir))
                Directory.CreateDirectory(interfaceOutputDir);

            var items = Generate(classNamespace, interfaceNamespace);

            foreach (var item in items)
            {
                string filePath;
                if (item.CodeType == GeneratedCodeType.Interface)
                    filePath = interfaceOutputDir;
                else
                    filePath = classOutputDir;

                filePath += item.ClassName + "." + Formatter.FilenameSuffix;

                File.WriteAllText(filePath, item.Code);
            }
        }

        /// <summary>
        /// Starts the code generation.
        /// </summary>
        /// <param name="classNamespace">The class namespace to use.</param>
        /// <param name="interfaceNamespace">The interface namespace to use.</param>
        /// <returns>The generated code files.</returns>
        public virtual IEnumerable<GeneratedTableCode> Generate(string classNamespace, string interfaceNamespace)
        {
            LoadDbContent();

            var isFilteringTables = (TablesToGenerate != null && !TablesToGenerate.IsEmpty());

            foreach (var table in _dbTables)
            {
                if (isFilteringTables && !TablesToGenerate.Contains(table.Key, StringComparer.OrdinalIgnoreCase))
                    continue;

                var generatedCodes = CreateCode(table.Key, table.Value, classNamespace, interfaceNamespace);
                foreach (var item in generatedCodes)
                {
                    yield return item;
                }
            }

            foreach (var c in CreateCodeForConstDictionaries(interfaceNamespace))
            {
                yield return c;
            }

            yield return CreateCodeForColumnMetadata(classNamespace);
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="DbColumnInfo"/>s for the given
        /// <paramref name="table"/>.
        /// </summary>
        /// <param name="table">The name of the table to get the <see cref="DbColumnInfo"/>s for.</param>
        /// <returns>The <see cref="DbColumnInfo"/>s for the given <paramref name="table"/>.</returns>
        protected abstract IEnumerable<DbColumnInfo> GetColumns(string table);

        /// <summary>
        /// Creates a ConstDictionary for an enum.
        /// </summary>
        /// <param name="columnCollection">The <see cref="ColumnCollection"/> that this ConstDictionary being created is for.</param>
        /// <returns>The generated code.</returns>
        protected string GetConstEnumDictonaryCode(ColumnCollection columnCollection)
        {
            var sb = new StringBuilder(DbResources.ConstEnumDictionaryCode);
            sb.Replace("[CLASSNAME]", GetConstEnumDictonaryName(columnCollection));
            sb.Replace("[INVALUETYPE]", Formatter.GetTypeString(columnCollection.InternalType));
            sb.Replace("[EXVALUETYPE]", Formatter.GetTypeString(columnCollection.ExternalType));
            sb.Replace("[KEYTYPE]", Formatter.GetTypeString(columnCollection.KeyType));
            sb.Replace("[COLUMNCOLLECTIONNAME]", columnCollection.Name);
            sb.Replace("[VALUEREADERREADMETHOD]", GetValueReaderReadMethodName(columnCollection.InternalType));
            return sb.ToString();
        }

        /// <summary>
        /// Gets the name to use for a ConstDictionary for an enum.
        /// </summary>
        /// <param name="columnCollection">The <see cref="ColumnCollection"/> for the ConstDictionary to get the name of.</param>
        /// <returns>The name of the ConstDictionary for the <paramref name="columnCollection"/>.</returns>
        protected static string GetConstEnumDictonaryName(ColumnCollection columnCollection)
        {
            return columnCollection.KeyType.Name + "ConstDictionary";
        }

        /// <summary>
        /// Gets the parameters to use for the constructor containing all parameters.
        /// </summary>
        /// <param name="cd">The <see cref="DbClassData"/>.</param>
        /// <returns>The parameters.</returns>
        protected virtual MethodParameter[] GetConstructorParameters(DbClassData cd)
        {
            var columnsArray = cd.Columns.ToArray();
            var parameters = new MethodParameter[columnsArray.Length];
            for (var i = 0; i < columnsArray.Length; i++)
            {
                var column = columnsArray[i];
                var p = new MethodParameter(cd.GetParameterName(column), cd.GetExternalType(column));
                parameters[i] = p;
            }

            return parameters;
        }

        /// <summary>
        /// When overridden in the derived class, gets the name of the tables in the database.
        /// </summary>
        /// <returns>The name of the tables in the database.</returns>
        protected abstract IEnumerable<string> GetTables();

        /// <summary>
        /// Gets the name of a method to use for a ValueReader to read a certain type.
        /// </summary>
        /// <param name="type">The type to read.</param>
        /// <returns>The ValueReader method to use to read the <paramref name="type"/>.</returns>
        protected string GetValueReaderReadMethodName(Type type)
        {
            string ret;
            if (_valueReaderReadMethods.TryGetValue(type, out ret))
                return ret;

            return DbClassData.GetDataReaderReadMethodName(type, _dataReaderReadMethods);
        }

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
            try
            {
                var tables = GetTables();

                foreach (var table in tables)
                {
                    var columns = GetColumns(table);
                    _dbTables.Add(table, columns.ToArray());
                }
            }
            finally
            {
                DbConnection.Close();
            }
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

        /// <summary>
        /// Sets the database connection.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <exception cref="MethodAccessException">The DbConnection has already been set.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "DbConnection")]
        protected void SetDbConnection(DbConnection dbConnection)
        {
            if (_dbConnction != null)
            {
                const string errmsg = "The DbConnection has already been set.";
                throw new MethodAccessException(errmsg);
            }

            _dbConnction = dbConnection;
        }

        /// <summary>
        /// Wraps a code file with the using directives and namespace.
        /// </summary>
        /// <param name="code">The body of code for the file.</param>
        /// <param name="namespaceName">The namespace.</param>
        /// <param name="isInterface">If the file is for an interface.</param>
        /// <returns>The generated code.</returns>
        string WrapCodeFile(string code, string namespaceName, bool isInterface)
        {
            var sb = new StringBuilder(code.Length + 512);

            // Header
            IEnumerable<string> usings = new string[] { "System", "System.Linq" };
            if (!isInterface)
                usings = usings.Concat(_usings);

            foreach (var usingNamespace in usings)
            {
                sb.AppendLine(Formatter.GetUsing(usingNamespace));
            }

            // Namespace
            sb.AppendLine(Formatter.GetNamespace(namespaceName));
            sb.AppendLine(Formatter.OpenBrace);
            {
                // Code
                sb.AppendLine(code);
            }
            sb.AppendLine(Formatter.CloseBrace);

            return sb.ToString();
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public abstract void Dispose();

        #endregion

        class ColumnCollectionDistinctKeyComparer : IEqualityComparer<ColumnCollection>
        {
            #region IEqualityComparer<ColumnCollection> Members

            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object of type <see cref="ColumnCollection"/> to compare.</param>
            /// <param name="y">The second object of type <see cref="ColumnCollection"/> to compare.</param>
            /// <returns>
            /// true if the specified objects are equal; otherwise, false.
            /// </returns>
            public bool Equals(ColumnCollection x, ColumnCollection y)
            {
                return x.KeyType == y.KeyType;
            }

            /// <summary>
            /// Returns a hash code for the specified object.
            /// </summary>
            /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param>
            /// <returns>A hash code for the specified object.</returns>
            /// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type
            /// and <paramref name="obj"/> is null.</exception>
            public int GetHashCode(ColumnCollection obj)
            {
                return obj.KeyType.GetHashCode();
            }

            #endregion
        }
    }
}