using System.Linq;
using NetGore;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// Describes the result of the generated code for a database table.
    /// </summary>
    public sealed class GeneratedTableCode
    {
        /// <summary>
        /// The name of the class or interface that was generated.
        /// </summary>
        public readonly string ClassName;

        /// <summary>
        /// The generated code.
        /// </summary>
        public readonly string Code;

        /// <summary>
        /// The type of code that this is for.
        /// </summary>
        public readonly GeneratedCodeType CodeType;

        /// <summary>
        /// The table that the code was generated for.
        /// </summary>
        public readonly string Table;

        /// <summary>
        /// GeneratedTableCode constructor.
        /// </summary>
        /// <param name="table">The table that the code was generated for.</param>
        /// <param name="className">The name of the class or interface that was generated.</param>
        /// <param name="code">The generated code.</param>
        /// <param name="codeType">The type of code that this is for.</param>
        public GeneratedTableCode(string table, string className, string code, GeneratedCodeType codeType)
        {
            Table = table;
            ClassName = className;
            Code = code;
            CodeType = codeType;
        }
    }
}