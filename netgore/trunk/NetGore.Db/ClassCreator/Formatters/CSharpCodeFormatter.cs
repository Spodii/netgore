using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// A <see cref="CodeFormatter"/> for CSharp.
    /// </summary>
    public class CSharpCodeFormatter : CodeFormatter
    {
        /// <summary>
        /// Gets the closing char for a brace.
        /// </summary>
        public override string CloseBrace
        {
            get { return "}"; }
        }

        /// <summary>
        /// Gets the closing char for an indexer.
        /// </summary>
        public override string CloseIndexer
        {
            get { return "]"; }
        }

        /// <summary>
        /// Gets the char for an end of a line.
        /// </summary>
        public override string EndOfLine
        {
            get { return ";"; }
        }

        /// <summary>
        /// Gets the file name suffix.
        /// </summary>
        public override string FilenameSuffix
        {
            get { return "cs"; }
        }

        /// <summary>
        /// Gets the opening char for a brace.
        /// </summary>
        public override string OpenBrace
        {
            get { return "{"; }
        }

        /// <summary>
        /// Gets the opening char for an indexer.
        /// </summary>
        public override string OpenIndexer
        {
            get { return "["; }
        }

        /// <summary>
        /// Gets the char to use to separate parameters.
        /// </summary>
        public override string ParameterSpacer
        {
            get { return ", "; }
        }

        /// <summary>
        /// Gets the code for an attribute.
        /// </summary>
        /// <param name="attributeType">The attribute type</param>
        /// <param name="args">The attribute arguments.</param>
        /// <returns>The code.</returns>
        public override string GetAttribute(string attributeType, params string[] args)
        {
            var sb = new StringBuilder();
            sb.Append(OpenIndexer);
            sb.Append(attributeType);
            sb.Append(OpenParameterString);

            if (args != null && args.Length > 0)
            {
                for (var i = 0; i < args.Length - 1; i++)
                {
                    sb.Append(args[i]);
                    sb.Append(ParameterSpacer);
                }
                sb.Append(args[args.Length - 1]);
            }

            sb.Append(CloseParameterString);
            sb.Append(CloseIndexer);

            return sb.ToString();
        }

        /// <summary>
        /// Gets the code for a class.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="isStatic">Whether or not it is a static class.</param>
        /// <param name="interfaces">The interfaces.</param>
        /// <returns>The code.</returns>
        public override string GetClass(string className, MemberVisibilityLevel visibility, bool isStatic, IEnumerable<string> interfaces)
        {
            var sb = new StringBuilder();
            sb.Append(GetVisibilityLevel(visibility));
            if (isStatic)
                sb.Append(" static ");
            sb.Append(" class ");
            sb.Append(className);

            if (interfaces != null && !interfaces.IsEmpty())
            {
                sb.Append(" : ");
                foreach (var i in interfaces)
                {
                    sb.Append(i);
                    sb.Append(ParameterSpacer);
                }
                sb.Length -= ParameterSpacer.Length;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the code for a constant field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="type">The type.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="value">The value.</param>
        /// <returns>The code.</returns>
        public override string GetConstField(string fieldName, Type type, MemberVisibilityLevel visibility, string value)
        {
            var sb = new StringBuilder();
            sb.Append(GetVisibilityLevel(visibility));
            sb.Append(" const ");
            sb.Append(GetTypeString(type));
            sb.Append(" ");
            sb.Append(fieldName);
            sb.Append(" = ");
            sb.Append(value);
            sb.Append(EndOfLine);
            return sb.ToString();
        }

        /// <summary>
        /// Gets the code for a field.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="type">The type.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="code">The code.</param>
        /// <param name="isReadonly">If this field is readonly.</param>
        /// <param name="isStatic">If this field is static.</param>
        /// <returns>The code.</returns>
        public override string GetField(string memberName, string type, MemberVisibilityLevel visibility, string code,
                                        bool isReadonly, bool isStatic)
        {
            var sb = new StringBuilder();

            if (visibility != MemberVisibilityLevel.Private)
            {
                sb.Append(GetVisibilityLevel(visibility));
                sb.Append(" ");
            }

            if (isStatic)
                sb.Append(" static ");

            if (isReadonly)
                sb.Append(" readonly ");

            sb.Append(type);
            sb.Append(" ");
            sb.Append(memberName);

            if (!string.IsNullOrEmpty(code))
            {
                sb.Append(" = ");
                sb.Append(code);
            }

            sb.Append(EndOfLine);

            return sb.ToString();
        }

        /// <summary>
        /// Gets the code for an interface.
        /// </summary>
        /// <param name="interfaceName">The interface name.</param>
        /// <param name="visibility">The visibility.</param>
        /// <returns>The code.</returns>
        public override string GetInterface(string interfaceName, MemberVisibilityLevel visibility)
        {
            return GetVisibilityLevel(visibility) + " interface " + interfaceName;
        }

        /// <summary>
        /// Gets the code for a local field.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>The code.</returns>
        public override string GetLocalField(string memberName, string type, string value)
        {
            var sb = new StringBuilder();
            sb.Append(type);
            sb.Append(" ");
            sb.Append(memberName);

            if (!string.IsNullOrEmpty(value))
            {
                sb.Append(" = ");
                sb.Append(value);
            }

            sb.Append(EndOfLine);
            return sb.ToString();
        }

        /// <summary>
        /// Gets the code for a namespace.
        /// </summary>
        /// <param name="namespaceName">Name of the namespace.</param>
        /// <returns>The code.</returns>
        public override string GetNamespace(string namespaceName)
        {
            return "namespace " + namespaceName;
        }

        /// <summary>
        /// When overridden in the derived class, generates the code for an array of string literals.
        /// </summary>
        /// <param name="strings">The string literals to include.</param>
        /// <returns>The code for an array of string literals.</returns>
        public override string GetStringArrayCode(IEnumerable<string> strings)
        {
            var sb = new StringBuilder();
            sb.Append("new string[] ");

            sb.Append(OpenBrace);
            foreach (var s in strings)
            {
                sb.Append("\"");
                sb.Append(s);
                sb.Append("\"");
                sb.Append(ParameterSpacer);
            }

            if (!strings.IsEmpty())
                sb.Length -= ParameterSpacer.Length;

            sb.Append(" ");
            sb.Append(CloseBrace);

            return sb.ToString();
        }

        /// <summary>
        /// When overridden in the derived class, generates the code for a switch.
        /// </summary>
        /// <param name="switchOn">The code to switch on.</param>
        /// <param name="switches">The switches to use, where the key is the switch's value, and the value is the
        /// code used for the switch.</param>
        /// <param name="defaultCode">The code to use on a default. If null, no default switch will be made.</param>
        /// <returns>The code for a switch.</returns>
        public override string GetSwitch(string switchOn, IEnumerable<KeyValuePair<string, string>> switches, string defaultCode)
        {
            var sb = new StringBuilder(512);
            sb.Append("switch ");
            sb.Append(OpenParameterString);
            sb.Append(switchOn);
            sb.AppendLine(CloseParameterString);

            sb.AppendLine(OpenBrace);

            foreach (var item in switches)
            {
                sb.Append("case ");
                sb.Append(item.Key);
                sb.AppendLine(":");

                sb.AppendLine(item.Value);
                sb.AppendLine();
            }

            if (defaultCode != null)
            {
                sb.AppendLine("default:");
                sb.AppendLine(defaultCode);
            }

            sb.AppendLine(CloseBrace);

            return sb.ToString();
        }

        /// <summary>
        /// Gets the code for a using namespace directive.
        /// </summary>
        /// <param name="namespaceName">Name of the namespace.</param>
        /// <returns>The code.</returns>
        public override string GetUsing(string namespaceName)
        {
            return "using " + namespaceName + EndOfLine;
        }

        /// <summary>
        /// Gets the code to represent a visibility level.
        /// </summary>
        /// <param name="visibility">The visibility level.</param>
        /// <returns>The code.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>visibility</c> contains an undefined
        /// <see cref="MemberVisibilityLevel"/> value.</exception>
        public override string GetVisibilityLevel(MemberVisibilityLevel visibility)
        {
            switch (visibility)
            {
                case MemberVisibilityLevel.Private:
                    return "private";
                case MemberVisibilityLevel.Public:
                    return "public";
                case MemberVisibilityLevel.Internal:
                    return "internal";
                case MemberVisibilityLevel.Protected:
                    return "protected";
                case MemberVisibilityLevel.ProtectedInternal:
                    return "protected internal";
                default:
                    throw new ArgumentOutOfRangeException("visibility");
            }
        }
    }
}