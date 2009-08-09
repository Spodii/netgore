using System.Linq;

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
        /// The table that the code was generated for.
        /// </summary>
        public readonly string Table;

        /// <summary>
        /// If the generated code is for an interface.
        /// </summary>
        public readonly bool IsInterface;

        /// <summary>
        /// If the generated code is for a special class. This will be true whenever the generated code is not for
        /// a database table class or interface.
        /// </summary>
        public readonly bool IsSpecialClass;

        /// <summary>
        /// GeneratedTableCode constructor.
        /// </summary>
        /// <param name="table">The table that the code was generated for.</param>
        /// <param name="className">The name of the class or interface that was generated.</param>
        /// <param name="code">The generated code.</param>
        /// <param name="isInterface">If the generated code is for an interface.</param>
        /// <param name="isSpecialClass">If the generated code is for a special class.</param>
        public GeneratedTableCode(string table, string className, string code, bool isInterface, bool isSpecialClass)
        {
            Table = table;
            ClassName = className;
            Code = code;
            IsInterface = isInterface;
            IsSpecialClass = isSpecialClass;
        }
    }
}