using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Db.ClassCreator
{
    public class CSharpCodeFormatter : CodeFormatter
    {
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

        public override string GetField(string memberName, Type type, MemberVisibilityLevel visibility, string code,
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

            sb.Append(GetTypeString(type));
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

        public override string GetInterface(string interfaceName, MemberVisibilityLevel visibility)
        {
            return GetVisibilityLevel(visibility) + " interface " + interfaceName;
        }

        public override string GetNamespace(string namespaceName)
        {
            return "namespace " + namespaceName;
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