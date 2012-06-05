using System.Linq;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// Describes the result of the generated code for a database table.
    /// </summary>
    public sealed class GeneratedTableCode
    {
        readonly string _className;
        readonly string _code;
        readonly GeneratedCodeType _codeType;
        readonly string _table;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratedTableCode"/> class.
        /// </summary>
        /// <param name="table">The table that the code was generated for.</param>
        /// <param name="className">The name of the class or interface that was generated.</param>
        /// <param name="code">The generated code.</param>
        /// <param name="codeType">The type of code that this is for.</param>
        public GeneratedTableCode(string table, string className, string code, GeneratedCodeType codeType)
        {
            _table = table;
            _className = className;
            _code = code;
            _codeType = codeType;
        }

        /// <summary>
        /// Gets the name of the class or interface that was generated.
        /// </summary>
        public string ClassName
        {
            get { return _className; }
        }

        /// <summary>
        /// Gets the generated code.
        /// </summary>
        public string Code
        {
            get { return _code; }
        }

        /// <summary>
        /// Gets the type of code that this is for.
        /// </summary>
        public GeneratedCodeType CodeType
        {
            get { return _codeType; }
        }

        /// <summary>
        /// Gets the table that the code was generated for.
        /// </summary>
        public string Table
        {
            get { return _table; }
        }
    }
}