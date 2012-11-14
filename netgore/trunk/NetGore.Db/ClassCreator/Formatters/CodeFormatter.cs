using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// The base class that describes how to format the generated code.
    /// </summary>
    public abstract class CodeFormatter
    {
        static readonly char[] _separatorCharacters = new char[] { '_', ' ', '-' };
        readonly Dictionary<string, string> _aliases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets a dictionary of aliases to use.
        /// </summary>
        public IDictionary<string, string> Aliases
        {
            get { return _aliases; }
        }

        /// <summary>
        /// Gets the class member qualifier.
        /// </summary>
        public virtual string ClassMemberQualifier
        {
            get { return "this."; }
        }

        /// <summary>
        /// Gets the closing char for a brace.
        /// </summary>
        public abstract string CloseBrace { get; }

        /// <summary>
        /// Gets the closing char for a generic.
        /// </summary>
        public virtual string CloseGeneric
        {
            get { return ">"; }
        }

        /// <summary>
        /// Gets the closing char for an indexer.
        /// </summary>
        public abstract string CloseIndexer { get; }

        /// <summary>
        /// Gets the closing char for a parameter string.
        /// </summary>
        public virtual string CloseParameterString
        {
            get { return ")"; }
        }

        /// <summary>
        /// Gets the char for an end of a line.
        /// </summary>
        public virtual string EndOfLine
        {
            get { return ""; }
        }

        /// <summary>
        /// Gets the file name suffix.
        /// </summary>
        public abstract string FilenameSuffix { get; }

        /// <summary>
        /// Gets the opening char for a brace.
        /// </summary>
        public abstract string OpenBrace { get; }

        /// <summary>
        /// Gets the opening char for a generic.
        /// </summary>
        public virtual string OpenGeneric
        {
            get { return "<"; }
        }

        /// <summary>
        /// Gets the opening char for an indexer.
        /// </summary>
        public abstract string OpenIndexer { get; }

        /// <summary>
        /// Gets the opening char for a list of parameters.
        /// </summary>
        public virtual string OpenParameterString
        {
            get { return "("; }
        }

        /// <summary>
        /// Gets the char to use to separate parameters.
        /// </summary>
        public abstract string ParameterSpacer { get; }

        /// <summary>
        /// Gets the string for a property getter.
        /// </summary>
        public virtual string PropertyGetString
        {
            get { return "get"; }
        }

        /// <summary>
        /// Gets the string for a property setter.
        /// </summary>
        public virtual string PropertySetString
        {
            get { return "set"; }
        }

        /// <summary>
        /// Gets the name of the variable used for getting the value passed to a property setter.
        /// </summary>
        public virtual string PropertyValue
        {
            get { return "value"; }
        }

        /// <summary>
        /// Gets the string for a return string.
        /// </summary>
        public virtual string ReturnString
        {
            get { return "return"; }
        }

        /// <summary>
        /// Gets the string for specifying something as static.
        /// </summary>
        public virtual string StaticString
        {
            get { return "static"; }
        }

        /// <summary>
        /// Gets the verbatim string char.
        /// </summary>
        public virtual string VerbatimIdentifier
        {
            get { return "@"; }
        }

        /// <summary>
        /// Gets the string for a virtual method.
        /// </summary>
        public virtual string VirtualString
        {
            get { return "virtual"; }
        }

        /// <summary>
        /// Adds an alias to the field naming so the names used in the code can differ from the naming
        /// actually used in the database.
        /// </summary>
        /// <param name="alias">Alias name of the field. This will be the name used in the generated code. This
        /// alias will automatically be used for any field who's name matches this alias except for with the casing.</param>
        public void AddAlias(string alias)
        {
            AddAlias(alias.ToLower(), alias);
            AddAlias(alias.ToUpper(), alias);
        }

        /// <summary>
        /// Adds an alias to the field naming so the names used in the code can differ from the naming
        /// actually used in the database.
        /// </summary>
        /// <param name="fieldName">Original name of the field.</param>
        /// <param name="alias">Alias name of the field. This will be the name used in the generated code.</param>
        public void AddAlias(string fieldName, string alias)
        {
            if (!_aliases.ContainsKey(fieldName))
                _aliases.Add(fieldName, alias);
            else
                _aliases[fieldName] = alias;
        }

        /// <summary>
        /// Adds parameter references to an xml comment.
        /// </summary>
        /// <param name="paramNames">The parameter names.</param>
        /// <param name="value">The string add the references on.</param>
        /// <param name="currentParameter">The name of the current parameter.</param>
        /// <returns>The new string to use.</returns>
        protected virtual string AddXmlCommentParamRefs(IEnumerable<string> paramNames, string value,
                                                        string currentParameter = null)
        {
            if (paramNames == null || paramNames.IsEmpty())
                return value;

            foreach (var parameter in paramNames)
            {
                if (currentParameter != null && parameter == currentParameter)
                    continue;

                var withoutPrefix = parameter;
                if (withoutPrefix.StartsWith(VerbatimIdentifier))
                    withoutPrefix = withoutPrefix.Substring(VerbatimIdentifier.Length);
                value = value.Replace(parameter, "<paramref name=\"" + withoutPrefix + "\"/>");
            }

            return value;
        }

        /// <summary>
        /// Applies an alias to a field.
        /// </summary>
        /// <param name="fieldName">The field name.</param>
        /// <returns>True if aliased; otherwise false.</returns>
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

        /// <summary>
        /// Escapes an xml comment entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>The escaped code.</returns>
        protected static string EscapeXmlCommentEntry(string entry)
        {
            return entry.Replace(Environment.NewLine, Environment.NewLine + "/// ");
        }

        /// <summary>
        /// Gets the code for an attribute.
        /// </summary>
        /// <param name="attributeType">The attribute type</param>
        /// <param name="args">The attribute arguments.</param>
        /// <returns>The code.</returns>
        public string GetAttribute(Type attributeType, params string[] args)
        {
            return GetAttribute(GetTypeString(attributeType), args);
        }

        /// <summary>
        /// Gets the code for an attribute.
        /// </summary>
        /// <param name="attributeType">The attribute type</param>
        /// <param name="args">The attribute arguments.</param>
        /// <returns>The code.</returns>
        public abstract string GetAttribute(string attributeType, params string[] args);

        /// <summary>
        /// Gets the code for calling a method.
        /// </summary>
        /// <param name="methodName">The name of the method to call.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The code.</returns>
        public virtual string GetCallMethod(string methodName, params string[] arguments)
        {
            var sb = new StringBuilder();
            sb.Append(methodName);
            sb.Append(OpenParameterString);
            foreach (var arg in arguments)
            {
                sb.Append(arg);
                sb.Append(ParameterSpacer);
            }
            sb.Length -= ParameterSpacer.Length;
            sb.Append(CloseParameterString);
            sb.AppendLine(EndOfLine);

            return sb.ToString();
        }

        /// <summary>
        /// Gets the code for calling a method on an object.
        /// </summary>
        /// <param name="objectToCallOn">The object the method is being called on.</param>
        /// <param name="methodName">The name of the method to call.</param>
        /// <param name="arguments">The arguments to pass to the method.</param>
        /// <returns>The code.</returns>
        public virtual string GetCallObjMethod(string objectToCallOn, string methodName, params string[] arguments)
        {
            return objectToCallOn + "." + GetCallMethod(methodName, arguments);
        }

        /// <summary>
        /// Gets the code for performing a cast.
        /// </summary>
        /// <param name="castType">The type to cast to.</param>
        /// <returns>The code.</returns>
        public string GetCast(Type castType)
        {
            return GetCast(GetTypeString(castType));
        }

        /// <summary>
        /// Gets the code for performing a cast.
        /// </summary>
        /// <param name="castType">The type to cast to.</param>
        /// <returns>The code.</returns>
        public virtual string GetCast(string castType)
        {
            return "(" + castType + ")";
        }

        /// <summary>
        /// Gets the code for a class.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="isStatic">Whether or not it is a static class.</param>
        /// <param name="interfaces">The interfaces.</param>
        /// <returns>The code.</returns>
        public abstract string GetClass(string className, MemberVisibilityLevel visibility, bool isStatic,
                                        IEnumerable<string> interfaces);

        /// <summary>
        /// Gets the name to use for a class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The class name.</returns>
        public virtual string GetClassName(string tableName)
        {
            return RemoveSeparatorCharacters(tableName) + "Table";
        }

        /// <summary>
        /// Gets the string to use in the code for a type with generics.
        /// </summary>
        /// <param name="baseType">The base type.</param>
        /// <param name="genericTypes">The generic type or types.</param>
        /// <returns>The string to use.</returns>
        public virtual string GetComplexTypeString(Type baseType, params Type[] genericTypes)
        {
            var baseTypeWithoutGenerics = GetTypeString(baseType);
            var start = baseTypeWithoutGenerics.LastIndexOf(OpenGeneric, StringComparison.Ordinal);
            baseTypeWithoutGenerics = baseTypeWithoutGenerics.Substring(0, start);

            var sb = new StringBuilder();
            sb.Append(baseTypeWithoutGenerics);
            sb.Append(OpenGeneric);
            for (var i = 0; i < genericTypes.Length; i++)
            {
                sb.Append(GetTypeString(genericTypes[i]));
                sb.Append(ParameterSpacer);
            }
            sb.Length -= ParameterSpacer.Length;
            sb.Append(CloseGeneric);

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
        public abstract string GetConstField(string fieldName, Type type, MemberVisibilityLevel visibility, string value);

        /// <summary>
        /// Gets the header for a constructor.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The code.</returns>
        public virtual string GetConstructorHeader(string className, MemberVisibilityLevel visibility,
                                                   MethodParameter[] parameters)
        {
            return GetMethodHeader(className, visibility, parameters, string.Empty, false, false);
        }

        /// <summary>
        /// Gets the code for the header of an extension method.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="extender">The extender.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <returns>The code.</returns>
        public virtual string GetExtensionMethodHeader(string methodName, MethodParameter extender, MethodParameter[] parameters,
                                                       Type returnType)
        {
            var sb = new StringBuilder(256);
            sb.Append(GetMethodNameAndVisibility(methodName, MemberVisibilityLevel.Public, returnType, false, true));

            sb.Append(OpenParameterString);

            // First parameter
            sb.Append("this ");
            sb.Append(GetParameter(extender));

            // Additional parameters
            if (parameters != null && parameters.Length > 0)
            {
                sb.Append(ParameterSpacer);
                sb.Append(GetParameters(parameters));
            }

            sb.Append(CloseParameterString);
            return sb.ToString();
        }

        /// <summary>
        /// Gets the code for a field.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="type">The type.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="value">The value.</param>
        /// <param name="isReadonly">If this field is readonly.</param>
        /// <param name="isStatic">If this field is static.</param>
        /// <returns>The code.</returns>
        public abstract string GetField(string memberName, string type, MemberVisibilityLevel visibility, string value,
                                        bool isReadonly, bool isStatic);

        /// <summary>
        /// Gets the code for a field.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="type">The type.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="value">The value.</param>
        /// <param name="isReadonly">If this field is readonly.</param>
        /// <param name="isStatic">If this field is static.</param>
        /// <returns>The code.</returns>
        public string GetField(string memberName, Type type, MemberVisibilityLevel visibility, string value, bool isReadonly,
                               bool isStatic)
        {
            return GetField(memberName, GetTypeString(type), visibility, value, isReadonly, isStatic);
        }

        /// <summary>
        /// Gets the code for a field.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="type">The type.</param>
        /// <param name="visibility">The visibility.</param>
        /// <returns>The code.</returns>
        public string GetField(string memberName, Type type, MemberVisibilityLevel visibility)
        {
            return GetField(memberName, type, visibility, string.Empty, false, false);
        }

        /// <summary>
        /// Gets the code for a field.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="type">The type.</param>
        /// <param name="visibility">The visibility.</param>
        /// <returns>The code.</returns>
        public string GetField(string memberName, string type, MemberVisibilityLevel visibility)
        {
            return GetField(memberName, type, visibility, string.Empty, false, false);
        }

        /// <summary>
        /// Gets the code for a field.
        /// </summary>
        /// <param name="inputName">The name of the field (can be aliased).</param>
        /// <param name="type">The type.</param>
        /// <param name="visibility">The visibility.</param>
        /// <returns>The code.</returns>
        public string GetFieldName(string inputName, MemberVisibilityLevel visibility, Type type)
        {
            return GetFieldName(inputName, visibility, GetTypeString(type));
        }

        /// <summary>
        /// Gets the code for a field.
        /// </summary>
        /// <param name="inputName">The name of the field (can be aliased).</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="type">The type.</param>
        /// <returns>The code.</returns>
        public virtual string GetFieldName(string inputName, MemberVisibilityLevel visibility, string type)
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

        /// <summary>
        /// Gets the code for an <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{T,U}"/>s.
        /// </summary>
        /// <param name="keyType">The type of the key.</param>
        /// <param name="valueType">The type of the value.</param>
        /// <returns>The code.</returns>
        public string GetIEnumerableKeyValuePair(Type keyType, Type valueType)
        {
            return GetIEnumerableKeyValuePair(GetTypeString(keyType), GetTypeString(valueType));
        }

        /// <summary>
        /// Gets the code for an <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{T,U}"/>s.
        /// </summary>
        /// <param name="keyType">The type of the key.</param>
        /// <param name="valueType">The type of the value.</param>
        /// <returns>The code.</returns>
        public string GetIEnumerableKeyValuePair(string keyType, Type valueType)
        {
            return GetIEnumerableKeyValuePair(keyType, GetTypeString(valueType));
        }

        /// <summary>
        /// Gets the code for an <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{T,U}"/>s.
        /// </summary>
        /// <param name="keyType">The type of the key.</param>
        /// <param name="valueType">The type of the value.</param>
        /// <returns>The code.</returns>
        public string GetIEnumerableKeyValuePair(Type keyType, string valueType)
        {
            return GetIEnumerableKeyValuePair(GetTypeString(keyType), valueType);
        }

        /// <summary>
        /// Gets the code for an <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{T,U}"/>s.
        /// </summary>
        /// <param name="keyType">The type of the key.</param>
        /// <param name="valueType">The type of the value.</param>
        /// <returns>The code.</returns>
        public virtual string GetIEnumerableKeyValuePair(string keyType, string valueType)
        {
            var sb = new StringBuilder();
            sb.Append("System.Collections.Generic.IEnumerable");
            sb.Append(OpenGeneric);
            sb.Append("System.Collections.Generic.KeyValuePair");
            sb.Append(OpenGeneric);
            sb.Append(keyType);
            sb.Append(ParameterSpacer);
            sb.Append(valueType);
            sb.Append(CloseGeneric);
            sb.Append(CloseGeneric);
            return sb.ToString();
        }

        /// <summary>
        /// Gets the code for an interface.
        /// </summary>
        /// <param name="interfaceName">The interface name.</param>
        /// <param name="visibility">The visibility.</param>
        /// <returns>The code.</returns>
        public abstract string GetInterface(string interfaceName, MemberVisibilityLevel visibility);

        /// <summary>
        /// Gets the code for an interface method.
        /// </summary>
        /// <param name="name">The method name.</param>
        /// <param name="returnType">The return type.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The code.</returns>
        public virtual string GetInterfaceMethod(string name, string returnType, params MethodParameter[] parameters)
        {
            var sb = new StringBuilder();
            sb.Append(returnType + " " + name);
            sb.Append(OpenParameterString);
            sb.Append(GetParameters(parameters));
            sb.Append(CloseParameterString);
            sb.AppendLine(EndOfLine);

            return sb.ToString();
        }

        /// <summary>
        /// Gets the code for an interface method.
        /// </summary>
        /// <param name="name">The method name.</param>
        /// <param name="returnType">The return type.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The code.</returns>
        public virtual string GetInterfaceMethod(string name, Type returnType, params MethodParameter[] parameters)
        {
            return GetInterfaceMethod(name, GetTypeString(returnType), parameters);
        }

        /// <summary>
        /// Gets the name to use for an interface for a table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The name to use.</returns>
        public virtual string GetInterfaceName(string tableName)
        {
            return "I" + GetClassName(tableName);
        }

        /// <summary>
        /// Gets the code for a property on an interface.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="hasSetter">If the property has a setter (if false, it just has a getter).</param>
        /// <returns>The code.</returns>
        public string GetInterfaceProperty(string name, Type returnType, bool hasSetter)
        {
            return GetInterfaceProperty(name, GetTypeString(returnType), hasSetter);
        }

        /// <summary>
        /// Gets the code for a property on an interface.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="hasSetter">If the property has a setter (if false, it just has a getter).</param>
        /// <returns>The code.</returns>
        public virtual string GetInterfaceProperty(string name, string returnType, bool hasSetter)
        {
            var sb = new StringBuilder();
            sb.AppendLine(returnType + " " + name);
            sb.AppendLine(OpenBrace);
            sb.AppendLine(PropertyGetString + EndOfLine);

            if (hasSetter)
                sb.AppendLine(PropertySetString + EndOfLine);

            sb.Append(CloseBrace);

            return sb.ToString();
        }

        /// <summary>
        /// Gets the code for a local field.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>The code.</returns>
        public string GetLocalField(string memberName, Type type, string value)
        {
            return GetLocalField(memberName, GetTypeString(type), value);
        }

        /// <summary>
        /// Gets the code for a local field.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>The code.</returns>
        public abstract string GetLocalField(string memberName, string type, string value);

        /// <summary>
        /// Gets the code for a method body.
        /// </summary>
        /// <param name="code">The method body code.</param>
        /// <returns>The code.</returns>
        public virtual string GetMethodBody(string code)
        {
            code = code.Trim();

            var sb = new StringBuilder(code.Length + 20);
            sb.AppendLine(OpenBrace);
            if (!string.IsNullOrEmpty(code))
                sb.AppendLine(code);
            sb.Append(CloseBrace);
            return sb.ToString();
        }

        /// <summary>
        /// Gets the code for a method header.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="isVirtual">If this method will be virtual.</param>
        /// <param name="isStatic">If this method will be static.</param>
        /// <returns>The code.</returns>
        public string GetMethodHeader(string methodName, MemberVisibilityLevel visibility, MethodParameter[] parameters,
                                      Type returnType, bool isVirtual, bool isStatic)
        {
            return GetMethodHeader(methodName, visibility, parameters, GetTypeString(returnType), isVirtual, isStatic);
        }

        /// <summary>
        /// Gets the code for a method header.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="isVirtual">If this method will be virtual.</param>
        /// <param name="isStatic">If this method will be static.</param>
        /// <returns>The code.</returns>
        public virtual string GetMethodHeader(string methodName, MemberVisibilityLevel visibility, MethodParameter[] parameters,
                                              string returnType, bool isVirtual, bool isStatic)
        {
            var sb = new StringBuilder(256);
            sb.Append(GetMethodNameAndVisibility(methodName, visibility, returnType, isVirtual, isStatic));

            // Parameters
            sb.Append(OpenParameterString);
            sb.Append(GetParameters(parameters));
            sb.Append(CloseParameterString);

            return sb.ToString();
        }

        /// <summary>
        /// Gets the name and visibility to use for a method.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="isVirtual">If this method will be virtual.</param>
        /// <param name="isStatic">If this method will be static.</param>
        /// <returns>The code.</returns>
        public virtual string GetMethodNameAndVisibility(string methodName, MemberVisibilityLevel visibility, Type returnType,
                                                         bool isVirtual, bool isStatic)
        {
            return GetMethodNameAndVisibility(methodName, visibility, GetTypeString(returnType), isVirtual, isStatic);
        }

        /// <summary>
        /// Gets the name and visibility to use for a method.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="isVirtual">If this method will be virtual.</param>
        /// <param name="isStatic">If this method will be static.</param>
        /// <returns>The code.</returns>
        /// <exception cref="ArgumentException">A method cannot be both virtual and static.</exception>
        public virtual string GetMethodNameAndVisibility(string methodName, MemberVisibilityLevel visibility, string returnType,
                                                         bool isVirtual, bool isStatic)
        {
            if (isVirtual && isStatic)
                throw new ArgumentException("A method cannot be both virtual and static.");

            var sb = new StringBuilder();
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

        /// <summary>
        /// Gets the code for a namespace.
        /// </summary>
        /// <param name="namespaceName">Name of the namespace.</param>
        /// <returns>The code.</returns>
        public abstract string GetNamespace(string namespaceName);

        /// <summary>
        /// Gets the code for creating a parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The code.</returns>
        public virtual string GetParameter(MethodParameter parameter)
        {
            return parameter.Type + " " + parameter.Name;
        }

        /// <summary>
        /// Gets the name to use for a parameter.
        /// </summary>
        /// <param name="inputName">Name of the input.</param>
        /// <param name="type">The type.</param>
        /// <returns>The parameter name.</returns>
        public virtual string GetParameterName(string inputName, Type type)
        {
            if (!ApplyAlias(ref inputName))
                inputName = RemoveSeparatorCharacters(inputName);

            return VerbatimIdentifier + inputName.Substring(0, 1).ToLower() + inputName.Substring(1);
        }

        /// <summary>
        /// Gets the code for multiple parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The code.</returns>
        public virtual string GetParameters(MethodParameter[] parameters)
        {
            if (parameters == null)
                return string.Empty;

            var sb = new StringBuilder();
            for (var i = 0; i < parameters.Length; i++)
            {
                sb.Append(GetParameter(parameters[i]));
                if (i < parameters.Length - 1)
                    sb.Append(ParameterSpacer);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the code for a property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="externalType">The external (exposed) type.</param>
        /// <param name="internalType">The internal (underlying) type.</param>
        /// <param name="getterVisibility">The getter visibility.</param>
        /// <param name="setterVisibility">The setter visibility.</param>
        /// <param name="member">The member.</param>
        /// <param name="isVirtual">If this property is virtual.</param>
        /// <param name="isStatic">If this property is static.</param>
        /// <returns>The code.</returns>
        public string GetProperty(string propertyName, Type externalType, Type internalType,
                                  MemberVisibilityLevel getterVisibility, MemberVisibilityLevel? setterVisibility, string member,
                                  bool isVirtual, bool isStatic)
        {
            return GetProperty(propertyName, GetTypeString(externalType), GetTypeString(internalType), getterVisibility,
                setterVisibility, member, isVirtual, isStatic);
        }

        /// <summary>
        /// Gets the code for a property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="externalType">The external (exposed) type.</param>
        /// <param name="internalType">The internal (underlying) type.</param>
        /// <param name="getterVisibility">The getter visibility.</param>
        /// <param name="setterVisibility">The setter visibility.</param>
        /// <param name="member">The member.</param>
        /// <param name="isVirtual">If this property is virtual.</param>
        /// <param name="isStatic">If this property is static.</param>
        /// <returns>The code.</returns>
        public virtual string GetProperty(string propertyName, string externalType, string internalType,
                                          MemberVisibilityLevel getterVisibility, MemberVisibilityLevel? setterVisibility,
                                          string member, bool isVirtual, bool isStatic)
        {
            var sb = new StringBuilder();
            sb.AppendLine(GetMethodNameAndVisibility(propertyName, getterVisibility, externalType, isVirtual, isStatic));
            sb.AppendLine(OpenBrace);
            {
                // Getter
                sb.AppendLine(PropertyGetString);
                sb.AppendLine(OpenBrace);
                {
                    sb.Append(ReturnString);
                    sb.Append(" ");
                    sb.Append(GetCast(externalType));
                    sb.Append(member);
                    sb.AppendLine(EndOfLine);
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
                        sb.AppendLine(GetSetValue(member, PropertyValue, true, false, internalType));
                    }
                    sb.AppendLine(CloseBrace);
                }
            }
            sb.Append(CloseBrace);

            return sb.ToString();
        }

        /// <summary>
        /// Gets the separator characters.
        /// </summary>
        /// <returns>The separator characters.</returns>
        protected virtual char[] GetSeparatorCharacters()
        {
            return _separatorCharacters;
        }

        /// <summary>
        /// Gets the code for setting a value.
        /// </summary>
        /// <param name="leftSide">The left side.</param>
        /// <param name="rightSide">The right side.</param>
        /// <param name="leftIsClassMember">If the left-side is a clas smember.</param>
        /// <param name="rightIsClassMember">If the right-side is a class member.</param>
        /// <param name="rightCastType">Type of the right cast.</param>
        /// <returns>The code.</returns>
        public virtual string GetSetValue(string leftSide, string rightSide, bool leftIsClassMember, bool rightIsClassMember,
                                          string rightCastType)
        {
            var sb = new StringBuilder();

            if (leftIsClassMember)
                sb.Append(ClassMemberQualifier);
            sb.Append(leftSide);

            sb.Append(" = ");

            if (!string.IsNullOrEmpty(rightCastType))
                sb.Append(GetCast(rightCastType));
            if (rightIsClassMember)
                sb.Append(ClassMemberQualifier);
            sb.Append(rightSide);
            if (!rightSide.Trim().EndsWith(EndOfLine))
                sb.Append(EndOfLine);

            return sb.ToString();
        }

        /// <summary>
        /// Gets the code for setting a value.
        /// </summary>
        /// <param name="leftSide">The left side.</param>
        /// <param name="rightSide">The right side.</param>
        /// <param name="leftIsClassMember">If the left-side is a clas smember.</param>
        /// <param name="rightIsClassMember">If the right-side is a class member.</param>
        /// <param name="rightCastType">Type of the right cast.</param>
        /// <returns>The code.</returns>
        public virtual string GetSetValue(string leftSide, string rightSide, bool leftIsClassMember, bool rightIsClassMember,
                                          Type rightCastType)
        {
            return GetSetValue(leftSide, rightSide, leftIsClassMember, rightIsClassMember, GetTypeString(rightCastType));
        }

        /// <summary>
        /// Gets the code for setting a value.
        /// </summary>
        /// <param name="leftSide">The left side.</param>
        /// <param name="rightSide">The right side.</param>
        /// <param name="leftIsClassMember">If the left-side is a clas smember.</param>
        /// <param name="rightIsClassMember">If the right-side is a class member.</param>
        /// <returns>The code.</returns>
        public string GetSetValue(string leftSide, string rightSide, bool leftIsClassMember, bool rightIsClassMember)
        {
            return GetSetValue(leftSide, rightSide, leftIsClassMember, rightIsClassMember, string.Empty);
        }

        /// <summary>
        /// When overridden in the derived class, generates the code for an array of string literals.
        /// </summary>
        /// <param name="strings">The string literals to include.</param>
        /// <returns>The code for an array of string literals.</returns>
        public abstract string GetStringArrayCode(IEnumerable<string> strings);

        /// <summary>
        /// When overridden in the derived class, generates the code for a switch.
        /// </summary>
        /// <param name="switchOn">The code to switch on.</param>
        /// <param name="switches">The switches to use, where the key is the switch's value, and the value is the
        /// code used for the switch.</param>
        /// <param name="defaultCode">The code to use on a default. If null, no default switch will be made.</param>
        /// <returns>The code for a switch.</returns>
        public abstract string GetSwitch(string switchOn, IEnumerable<KeyValuePair<string, string>> switches, string defaultCode);

        /// <summary>
        /// Gets the code to use to reference a type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The code.</returns>
        public virtual string GetTypeString(Type type)
        {
            if (type == typeof(void))
                return GetVoidTypeString();

            var s = type.FullName;

            if (type.IsGenericType)
            {
                s = type.ToString();
                var firstBracket = s.IndexOf("[", StringComparison.Ordinal) - 1;
                var start = s.IndexOf("`", StringComparison.Ordinal);
                s = s.Remove(start, firstBracket - start + 1);
                s = s.Replace("[", OpenGeneric);
                s = s.Replace("]", CloseGeneric);
            }

            if (type.IsNested)
                s = s.Replace("+", ".");

            return s;
        }

        /// <summary>
        /// Gets the code for a using namespace directive.
        /// </summary>
        /// <param name="namespaceName">Name of the namespace.</param>
        /// <returns>The code.</returns>
        public abstract string GetUsing(string namespaceName);

        /// <summary>
        /// Gets the code to represent a visibility level.
        /// </summary>
        /// <param name="visibility">The visibility level.</param>
        /// <returns>The code.</returns>
        public abstract string GetVisibilityLevel(MemberVisibilityLevel visibility);

        /// <summary>
        /// Gets the void type.
        /// </summary>
        /// <returns>The void type code.</returns>
        public virtual string GetVoidTypeString()
        {
            return "void";
        }

        /// <summary>
        /// Gets the codefor Xml documentation.
        /// </summary>
        /// <param name="summary">The summary block documentation.</param>
        /// <param name="returns">The return block documentation.</param>
        /// <param name="parameters">The documentation for the parameter blocks.</param>
        /// <returns>The code.</returns>
        public virtual string GetXmlComment(string summary, string returns, params KeyValuePair<string, string>[] parameters)
        {
            IEnumerable<string> paramNames;
            if (parameters == null || parameters.Length == 0)
                paramNames = Enumerable.Empty<string>();
            else
                paramNames = parameters.Select(x => x.Key);

            var sb = new StringBuilder();
            sb.AppendLine("/// <summary>");
            sb.AppendLine("/// " + EscapeXmlCommentEntry(AddXmlCommentParamRefs(paramNames, summary)));
            sb.AppendLine("/// </summary>");

            if (parameters != null && parameters.Length > 0)
            {
                foreach (var p in parameters)
                {
                    var key = p.Key;
                    if (key.StartsWith(VerbatimIdentifier))
                        key = key.Substring(VerbatimIdentifier.Length);

                    sb.Append("/// <param name=\"" + key + "\">");
                    sb.Append(EscapeXmlCommentEntry(AddXmlCommentParamRefs(paramNames, p.Value, p.Key)));
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

        /// <summary>
        /// Gets the codefor Xml documentation.
        /// </summary>
        /// <param name="summary">The summary block documentation.</param>
        /// <returns>The code.</returns>
        public string GetXmlComment(string summary)
        {
            return GetXmlComment(summary, null, null);
        }

        /// <summary>
        /// Removes the separator characters from a string (used to sanitize a database table or column name into a more code-friendly name).
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>The unseparated string.</returns>
        protected virtual string RemoveSeparatorCharacters(string str)
        {
            var parts = str.Split(GetSeparatorCharacters());
            var sb = new StringBuilder(parts.Sum(x => x.Length) + 1);
            foreach (var part in parts)
            {
                sb.Append(part.Substring(0, 1).ToUpper());
                sb.Append(part.Substring(1));
            }

            return sb.ToString();
        }
    }
}