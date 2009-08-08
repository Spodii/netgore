using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Db.ClassCreator
{
    public class CSharpCodeFormatter : CodeFormatter
    {
        public override string GetTypeString(Type type)
        {
            if (type == typeof(void))
                return "void";

            return base.GetTypeString(type);
        }

        /// <summary>
        /// When overridden in the derived class, generates the code for an array of string literals.
        /// </summary>
        /// <param name="strings">The string literals to include.</param>
        /// <returns>The code for an array of string literals.</returns>
        public override string GetStringArrayCode(IEnumerable<string> strings)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("new string[] ");

            sb.Append(OpenBrace);
            foreach (string s in strings)
            {
                sb.Append("\"");
                sb.Append(s);
                sb.Append("\"");
                sb.Append(ParameterSpacer);
            }

            if (strings.Count() > 0)
                sb.Length -= ParameterSpacer.Length;

            sb.Append(" ");
            sb.Append(CloseBrace);

            return sb.ToString();
        }

        public override string CloseBrace
        {
            get { return "}"; }
        }

        public override string EndOfLine
        {
            get { return ";"; }
        }

        public override string FilenameSuffix
        {
            get { return "cs"; }
        }

        public override string OpenBrace
        {
            get { return "{"; }
        }

        public override string OpenIndexer
        {
            get { return "["; }
        }

        public override string CloseIndexer
        {
            get { return "]"; }
        }

        public override string ParameterSpacer
        {
            get { return ", "; }
        }

        public override string GetClass(string className, MemberVisibilityLevel visibility, IEnumerable<string> interfaces)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetVisibilityLevel(visibility));
            sb.Append(" class ");
            sb.Append(className);

            if (interfaces.Count() > 0)
            {
                sb.Append(" : ");
                foreach (string i in interfaces)
                {
                    sb.Append(i);
                    sb.Append(ParameterSpacer);
                }
                sb.Length -= ParameterSpacer.Length;
            }

            return sb.ToString();
        }

        public override string GetConstField(string fieldName, Type type, MemberVisibilityLevel visibility, string value)
        {
            StringBuilder sb = new StringBuilder();
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

        public override string GetField(string memberName, string type, MemberVisibilityLevel visibility, string code,
                                        bool isReadonly, bool isStatic)
        {
            StringBuilder sb = new StringBuilder();

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

        public override string GetLocalField(string memberName, string type, string value)
        {
            StringBuilder sb = new StringBuilder();
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

        public override string GetInterface(string interfaceName, MemberVisibilityLevel visibility)
        {
            return GetVisibilityLevel(visibility) + " interface " + interfaceName;
        }

        public override string GetNamespace(string namespaceName)
        {
            return "namespace " + namespaceName;
        }

        public override string GetSwitch(string switchOn, IEnumerable<KeyValuePair<string, string>> switches, string defaultCode)
        {
            StringBuilder sb = new StringBuilder(512);
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

        public override string GetUsing(string namespaceName)
        {
            return "using " + namespaceName + EndOfLine;
        }

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