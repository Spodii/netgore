using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Db.ClassCreator
{
    public abstract class CodeFormatter
    {
        protected readonly char[] separatorCharacters = new char[] { '_', ' ', '-' };
        readonly Dictionary<string, string> _aliases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public IDictionary<string, string> Aliases
        {
            get { return _aliases; }
        }

        /// <summary>
        /// When overridden in the derived class, generates the code for an array of string literals.
        /// </summary>
        /// <param name="strings">The string literals to include.</param>
        /// <returns>The code for an array of string literals.</returns>
        public abstract string GetStringArrayCode(IEnumerable<string> strings);

        public virtual string ClassMemberQualifier
        {
            get { return "this."; }
        }

        public abstract string CloseBrace { get; }

        public virtual string CloseGeneric
        {
            get { return ">"; }
        }

        public virtual string CloseParameterString
        {
            get { return ")"; }
        }

        public virtual string EndOfLine
        {
            get { return ""; }
        }

        public abstract string FilenameSuffix { get; }

        public abstract string OpenBrace { get; }

        public abstract string OpenIndexer { get; }
        public abstract string CloseIndexer { get; }

        public virtual string OpenGeneric
        {
            get { return "<"; }
        }

        public virtual string OpenParameterString
        {
            get { return "("; }
        }

        public abstract string ParameterSpacer { get; }

        public virtual string PropertyGetString
        {
            get { return "get"; }
        }

        public virtual string PropertySetString
        {
            get { return "set"; }
        }

        public virtual string PropertyValue
        {
            get { return "value"; }
        }

        public virtual string ReturnString
        {
            get { return "return"; }
        }

        public virtual string StaticString
        {
            get { return "static"; }
        }

        public virtual string VerbatinIdentifier
        {
            get { return "@"; }
        }

        public virtual string VirtualString
        {
            get { return "virtual"; }
        }

        public void AddAlias(string fieldName, string alias)
        {
            _aliases.Add(fieldName, alias);
        }

        protected virtual string AddXmlCommentParamRefs(IEnumerable<string> paramNames, string value)
        {
            if (paramNames == null || paramNames.Count() == 0)
                return value;

            foreach (string parameter in paramNames)
            {
                string withoutPrefix = parameter;
                if (withoutPrefix.StartsWith(VerbatinIdentifier))
                    withoutPrefix = withoutPrefix.Substring(VerbatinIdentifier.Length);
                value = value.Replace(parameter, "<paramref name=\"" + withoutPrefix + "\"/>");
            }

            return value;
        }

        protected bool ApplyAlias(ref string fieldName)
        {
            string value;
            if (_aliases.TryGetValue(fieldName, out value))
            {
                fieldName = value;
                return true;
            }

            return false;
        }

        protected static string EscapeXmlCommentEntry(string entry)
        {
            return entry.Replace(Environment.NewLine, Environment.NewLine + "/// ");
        }

        public virtual string GetCallMethod(string methodName, params string[] arguments)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(methodName);
            sb.Append(OpenParameterString);
            foreach (string arg in arguments)
            {
                sb.Append(arg);
                sb.Append(ParameterSpacer);
            }
            sb.Length -= ParameterSpacer.Length;
            sb.Append(CloseParameterString);
            sb.AppendLine(EndOfLine);

            return sb.ToString();
        }

        public string GetCast(Type castType)
        {
            return GetCast(GetTypeString(castType));
        }

        public virtual string GetCast(string castType)
        {
            return "(" + castType + ")";
        }

        public abstract string GetClass(string className, MemberVisibilityLevel visibility, IEnumerable<string> interfaces);

        public virtual string GetClassName(string tableName)
        {
            return RemoveSeparatorCharacters(tableName) + "Table";
        }

        public abstract string GetConstField(string fieldName, Type type, MemberVisibilityLevel visibility, string value);

        public virtual string GetConstructorHeader(string className, MemberVisibilityLevel visibility,
                                                   MethodParameter[] parameters)
        {
            return GetMethodHeader(className, visibility, parameters, string.Empty, false, false);
        }

        public abstract string GetField(string memberName, string type, MemberVisibilityLevel visibility, string value,
                                        bool isReadonly, bool isStatic);

        public string GetField(string memberName, Type type, MemberVisibilityLevel visibility, string value,
                                        bool isReadonly, bool isStatic)
        {
            return GetField(memberName, GetTypeString(type), visibility, value, isReadonly, isStatic);
        }

        public string GetField(string memberName, Type type, MemberVisibilityLevel visibility)
        {
            return GetField(memberName, type, visibility, string.Empty, false, false);
        }

        public string GetField(string memberName, string type, MemberVisibilityLevel visibility)
        {
            return GetField(memberName, type, visibility, string.Empty, false, false);
        }

        public virtual string GetFieldName(string inputName, MemberVisibilityLevel visibility, Type type)
        {
            if (ApplyAlias(ref inputName))
            {
                if (visibility != MemberVisibilityLevel.Private)
                    return inputName;
            }

            inputName = RemoveSeparatorCharacters(inputName);

            switch (visibility)
            {
                case MemberVisibilityLevel.Private:
                    // Prefix _ and make the first character lowercase
                    return "_" + inputName.Substring(0, 1).ToLower() + inputName.Substring(1);

                default:
                    // Make first character uppercase
                    return inputName.Substring(0, 1).ToUpper() + inputName.Substring(1);
            }
        }

        public abstract string GetInterface(string interfaceName, MemberVisibilityLevel visibility);

        public virtual string GetInterfaceName(string tableName)
        {
            return "I" + GetClassName(tableName);
        }

        public virtual string GetComplexTypeString(Type baseType, params Type[] genericTypes)
        {
            string baseTypeWithoutGenerics = GetTypeString(baseType);
            int start = baseTypeWithoutGenerics.LastIndexOf(OpenGeneric);
            baseTypeWithoutGenerics = baseTypeWithoutGenerics.Substring(0, start);

            StringBuilder sb = new StringBuilder();
            sb.Append(baseTypeWithoutGenerics);
            sb.Append(OpenGeneric);
            for (int i = 0; i < genericTypes.Length; i++)
            {
                sb.Append(GetTypeString(genericTypes[i]));
                sb.Append(ParameterSpacer);
            }
            sb.Length -= ParameterSpacer.Length;
            sb.Append(CloseGeneric);

            return sb.ToString();
        }

        public string GetInterfaceProperty(string name, Type returnType, bool hasSetter)
        {
            return GetInterfaceProperty(name, GetTypeString(returnType), hasSetter);
        }

        public virtual string GetInterfaceMethod(string name, Type returnType, params MethodParameter[] parameters)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(returnType + " " + name);
            sb.Append(OpenParameterString);
            sb.Append(GetParameters(parameters));
            sb.Append(CloseParameterString);
            sb.AppendLine(EndOfLine);

            return sb.ToString();
        }

        public virtual string GetInterfaceProperty(string name, string returnType, bool hasSetter)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(returnType + " " + name);
            sb.AppendLine(OpenBrace);
            sb.AppendLine(PropertyGetString + EndOfLine);

            if (hasSetter)
                sb.AppendLine(PropertySetString + EndOfLine);

            sb.Append(CloseBrace);

            return sb.ToString();
        }

        public virtual string GetMethodBody(string code)
        {
            code = code.Trim();

            StringBuilder sb = new StringBuilder(code.Length + 20);
            sb.AppendLine(OpenBrace);
            if (!string.IsNullOrEmpty(code))
                sb.AppendLine(code);
            sb.Append(CloseBrace);
            return sb.ToString();
        }

        public string GetMethodHeader(string methodName, MemberVisibilityLevel visibility, MethodParameter[] parameters,
                                      Type returnType, bool isVirtual, bool isStatic)
        {
            return GetMethodHeader(methodName, visibility, parameters, GetTypeString(returnType), isVirtual, isStatic);
        }

        public virtual string GetMethodHeader(string methodName, MemberVisibilityLevel visibility, MethodParameter[] parameters,
                                              string returnType, bool isVirtual, bool isStatic)
        {
            StringBuilder sb = new StringBuilder(1024);
            sb.Append(GetMethodNameAndVisibility(methodName, visibility, returnType, isVirtual, isStatic));

            // Parameters
            sb.Append(OpenParameterString);
            sb.Append(GetParameters(parameters));
            sb.Append(CloseParameterString);

            return sb.ToString();
        }

        public virtual string GetMethodNameAndVisibility(string methodName, MemberVisibilityLevel visibility, Type returnType,
                                                         bool isVirtual, bool isStatic)
        {
            return GetMethodNameAndVisibility(methodName, visibility, GetTypeString(returnType), isVirtual, isStatic);
        }

        public virtual string GetMethodNameAndVisibility(string methodName, MemberVisibilityLevel visibility, string returnType,
                                                         bool isVirtual, bool isStatic)
        {
            if (isVirtual && isStatic)
                throw new ArgumentException("A method cannot be both virtual and static.");

            StringBuilder sb = new StringBuilder();
            sb.Append(GetVisibilityLevel(visibility));
            sb.Append(" ");
            if (isVirtual)
            {
                sb.Append(VirtualString);
                sb.Append(" ");
            }
            else if (isStatic)
            {
                sb.Append(StaticString);
                sb.Append(" ");
            }
            if (!string.IsNullOrEmpty(returnType))
            {
                sb.Append(returnType);
                sb.Append(" ");
            }
            sb.Append(methodName);

            return sb.ToString();
        }

        public abstract string GetNamespace(string namespaceName);

        public virtual string GetParameter(MethodParameter parameter)
        {
            return parameter.Type + " " + parameter.Name;
        }

        public virtual string GetParameterName(string inputName, Type type)
        {
            if (!ApplyAlias(ref inputName))
                inputName = RemoveSeparatorCharacters(inputName);

            return VerbatinIdentifier + inputName.Substring(0, 1).ToLower() + inputName.Substring(1);
        }

        public virtual string GetParameters(MethodParameter[] parameters)
        {
            if (parameters == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < parameters.Length; i++)
            {
                sb.Append(GetParameter(parameters[i]));
                if (i < parameters.Length - 1)
                    sb.Append(ParameterSpacer);
            }
            return sb.ToString();
        }

        public string GetProperty(string propertyName, Type type, MemberVisibilityLevel getterVisibility,
                                          MemberVisibilityLevel? setterVisibility, string member, bool isVirtual)
        {
            return GetProperty(propertyName, GetTypeString(type), getterVisibility, setterVisibility, member, isVirtual);
        }

        public virtual string GetProperty(string propertyName, string type, MemberVisibilityLevel getterVisibility,
                                          MemberVisibilityLevel? setterVisibility, string member, bool isVirtual)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(GetMethodNameAndVisibility(propertyName, getterVisibility, type, isVirtual, false));
            sb.AppendLine(OpenBrace);
            {
                // Getter
                sb.AppendLine(PropertyGetString);
                sb.AppendLine(OpenBrace);
                {
                    sb.AppendLine(ReturnString + " " + member + EndOfLine);
                }
                sb.AppendLine(CloseBrace);

                // Setter
                if (setterVisibility.HasValue)
                {
                    if (setterVisibility.Value != getterVisibility)
                        sb.Append(GetVisibilityLevel(setterVisibility.Value) + " ");
                    sb.AppendLine(PropertySetString);
                    sb.AppendLine(OpenBrace);
                    {
                        sb.AppendLine(GetSetValue(member, PropertyValue, true, false));
                    }
                    sb.AppendLine(CloseBrace);
                }
            }
            sb.Append(CloseBrace);

            return sb.ToString();
        }

        public virtual string GetSetValue(string leftSide, string rightSide, bool leftIsClassMember, bool rightIsClassMember,
                                          string rightCastType)
        {
            StringBuilder sb = new StringBuilder();

            if (leftIsClassMember)
                sb.Append(ClassMemberQualifier);
            sb.Append(leftSide);

            sb.Append(" = ");

            if (!string.IsNullOrEmpty(rightCastType))
                sb.Append(GetCast(rightCastType));
            if (rightIsClassMember)
                sb.Append(ClassMemberQualifier);
            sb.Append(rightSide);
            sb.Append(EndOfLine);

            return sb.ToString();
        }

        public virtual string GetSetValue(string leftSide, string rightSide, bool leftIsClassMember, bool rightIsClassMember,
                                          Type rightCastType)
        {
            return GetSetValue(leftSide, rightSide, leftIsClassMember, rightIsClassMember, GetTypeString(rightCastType));
        }

        public string GetSetValue(string leftSide, string rightSide, bool leftIsClassMember, bool rightIsClassMember)
        {
            return GetSetValue(leftSide, rightSide, leftIsClassMember, rightIsClassMember, string.Empty);
        }

        public virtual string GetTypeString(Type type)
        {
            if (type == typeof(void))
                return "void";

            if (type.IsGenericType)
            {
                string s = type.ToString();
                int firstBracket = s.IndexOf("[") - 1;
                int start = s.IndexOf("`");
                s = s.Remove(start, firstBracket - start + 1);
                s = s.Replace("[", OpenGeneric);
                s = s.Replace("]", CloseGeneric);
                return s;
            }

            return type.FullName;
        }

        public abstract string GetUsing(string namespaceName);

        public abstract string GetVisibilityLevel(MemberVisibilityLevel visibility);

        public virtual string GetVoidTypeString()
        {
            return "void";
        }

        public virtual string GetXmlComment(string summary, string returns, params KeyValuePair<string, string>[] parameters)
        {
            IEnumerable<string> paramNames;
            if (parameters == null || parameters.Length == 0)
                paramNames = Enumerable.Empty<string>();
            else
                paramNames = parameters.Select(x => x.Key);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("/// <summary>");
            sb.AppendLine("/// " + EscapeXmlCommentEntry(AddXmlCommentParamRefs(paramNames, summary)));
            sb.AppendLine("/// </summary>");

            if (parameters != null && parameters.Length > 0)
            {
                foreach (var p in parameters)
                {
                    string key = p.Key;
                    if (key.StartsWith(VerbatinIdentifier))
                        key = key.Substring(VerbatinIdentifier.Length);

                    sb.Append("/// <param name=\"" + key + "\">");
                    sb.Append(EscapeXmlCommentEntry(AddXmlCommentParamRefs(paramNames, p.Value)));
                    sb.AppendLine("</param>");
                }
            }

            if (!string.IsNullOrEmpty(returns))
            {
                sb.AppendLine("/// <returns>");
                sb.AppendLine("/// " + EscapeXmlCommentEntry(AddXmlCommentParamRefs(paramNames, returns)));
                sb.AppendLine("/// </returns>");
            }

            sb.Length -= Environment.NewLine.Length;
            return sb.ToString();
        }

        public string GetXmlComment(string summary)
        {
            return GetXmlComment(summary, null, null);
        }

        protected virtual string RemoveSeparatorCharacters(string str)
        {
            var parts = str.Split(separatorCharacters);
            StringBuilder sb = new StringBuilder();
            foreach (string part in parts)
            {
                sb.Append(part.Substring(0, 1).ToUpper());
                sb.Append(part.Substring(1).ToLower());
            }

            return sb.ToString();
        }
    }
}