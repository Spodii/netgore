using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using NetGore.Collections;

namespace NetGore
{
    /// <summary>
    /// Provides a means for quickly and easily parsing a string based on the Type alone. The Type must supply its own
    /// Parse function that accepts a string and has a return Type equal to itself, or implement an implicit or
    /// explicit operator that accepts one of the basic System value types and has a return Type equal to itself.
    /// </summary>
    public static class StringParser
    {
        /// <summary>
        /// System Types that are handled directly in TryParseSystemType.
        /// </summary>
        static readonly Type[] _handledSystemTypes = new Type[]
        {
            typeof(string), typeof(float), typeof(double), typeof(long), typeof(ulong), typeof(int), typeof(uint), typeof(short),
            typeof(ushort), typeof(byte), typeof(sbyte), typeof(DateTime), typeof(bool)
        };

        /// <summary>
        /// Cache of the MethodInfo for each Type. This is updated on-demand, so it will initially be empty
        /// until Types that are not in _handledSystemTypes are requested. The value can be null when there is no
        /// parsing method available for the Type.
        /// </summary>
        static readonly ICache<Type, MethodInfo> _parsers;

        /// <summary>
        /// Initializes the <see cref="StringParser"/> class.
        /// </summary>
        static StringParser()
        {
            var methodParameterType = new Type[] { typeof(string) };

            // The creator will first check for a method named "Parse" that takes a string and returns our value
            // type. If none is found, check to see if there is an implicit operator to cast from our type to
            // a string.
            Func<Type, MethodInfo> creator = x => GetMethodInfo("parse", x, methodParameterType) ?? GetOperatorMethodInfo(x);

            _parsers = new ThreadSafeHashCache<Type, MethodInfo>(creator);
        }

        /// <summary>
        /// Gets the Parser to use for all the parsing done in this class.
        /// </summary>
        static Parser ParserToUse
        {
            get { return Parser.Invariant; }
        }

        /// <summary>
        /// Checks if the <paramref name="type"/> has the ability to parse a string.
        /// </summary>
        /// <param name="type">Type to check for having the ability to parse a string.</param>
        /// <returns>True if the <paramref name="type"/> can parse a string; otherwise false.</returns>
        public static bool CanParseType(Type type)
        {
            if (_handledSystemTypes.Contains(type))
                return true;

            if (_parsers[type] != null)
                return true;

            return false;
        }

        static bool CastOperatorMethodInfoFilter(MethodInfo m)
        {
            if (!m.IsSpecialName || (m.Name != "op_Implicit" && m.Name != "op_Explicit"))
                return false;

            var parameters = m.GetParameters();
            if (parameters.Length != 1)
                return false;

            if (!_handledSystemTypes.Contains(parameters[0].ParameterType))
                return false;

            return true;
        }

        static MethodInfo GetMethodInfo(string name, Type handledType, Type[] parameters)
        {
            const BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod;
            var method = handledType.GetMethod(name, flags, null, parameters, null);
            if (method == null || method.ReturnType != handledType)
                return null;

            return method;
        }

        static MethodInfo GetOperatorMethodInfo(Type handledType)
        {
            const BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod;
            var methods = handledType.GetMethods(flags);
            var specialMethods = methods.Where(x => x.IsSpecialName);
            var opMethods = specialMethods.Where(CastOperatorMethodInfoFilter);
            var validOps = opMethods.Where(x => x.ReturnType == handledType);

            if (validOps.IsEmpty())
                return null;

            return validOps.First();
        }

        /// <summary>
        /// Tries to parse a string to the given <paramref name="type"/>.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="type">The Type to parse the string to.</param>
        /// <param name="value">If the parsing was successful, this will contain the parsed string <paramref name="s"/>
        /// as Type <paramref name="type"/>. If parsing was unsuccessful, this will be null.</param>
        /// <returns>True if the parsing was successful; otherwise false.</returns>
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
        public static bool TryParse(string s, Type type, out object value)
        {
            // Check for a System type
            if (TryParseSystemType(s, type, out value))
                return true;

            // Get the parse method for this Type
            var method = _parsers[type];
            if (method == null)
            {
                value = null;
                return false;
            }

            // Try to parse
            try
            {
                if (method.IsSpecialName)
                {
                    // An explicit/implicit operator - have to convert first to the system type, then call the operator
                    // to get whatever type this is
                    object parsedSystemType;
                    var successful = TryParseSystemType(s, method.GetParameters().First().ParameterType, out parsedSystemType);
                    if (!successful)
                    {
                        value = null;
                        return false;
                    }

                    value = method.Invoke(null, new object[] { parsedSystemType });
                }
                else
                {
                    // A parse method - nice and easy
                    value = method.Invoke(null, new object[] { s });
                }
            }
            catch (Exception)
            {
                value = null;
                return false;
            }

            // Parse successful
            return true;
        }

        /// <summary>
        /// Tries to parse a system Type. Every Type handled in here should also defined in _handledSystemTypes.
        /// </summary>
        /// <param name="s">String to parse.</param>
        /// <param name="type">Type to parse to.</param>
        /// <param name="value">The parsed type, or null if the parsing failed.</param>
        /// <returns>True if the parsing was successful; otherwise false.</returns>
        static bool TryParseSystemType(string s, Type type, out object value)
        {
            // If you add a Type to this, make sure to also add it to _handledSystemTypes

            if (type == typeof(string))
            {
                value = s;
                return true;
            }

            if (type == typeof(float))
            {
                float v;
                var ret = ParserToUse.TryParse(s, out v);
                value = v;
                return ret;
            }

            if (type == typeof(double))
            {
                double v;
                var ret = ParserToUse.TryParse(s, out v);
                value = v;
                return ret;
            }

            if (type == typeof(long))
            {
                long v;
                var ret = ParserToUse.TryParse(s, out v);
                value = v;
                return ret;
            }

            if (type == typeof(ulong))
            {
                ulong v;
                var ret = ParserToUse.TryParse(s, out v);
                value = v;
                return ret;
            }

            if (type == typeof(int))
            {
                int v;
                var ret = ParserToUse.TryParse(s, out v);
                value = v;
                return ret;
            }

            if (type == typeof(uint))
            {
                uint v;
                var ret = ParserToUse.TryParse(s, out v);
                value = v;
                return ret;
            }

            if (type == typeof(short))
            {
                short v;
                var ret = ParserToUse.TryParse(s, out v);
                value = v;
                return ret;
            }

            if (type == typeof(ushort))
            {
                ushort v;
                var ret = ParserToUse.TryParse(s, out v);
                value = v;
                return ret;
            }

            if (type == typeof(byte))
            {
                byte v;
                var ret = ParserToUse.TryParse(s, out v);
                value = v;
                return ret;
            }

            if (type == typeof(sbyte))
            {
                sbyte v;
                var ret = ParserToUse.TryParse(s, out v);
                value = v;
                return ret;
            }

            if (type == typeof(DateTime))
            {
                DateTime v;
                var ret = ParserToUse.TryParse(s, out v);
                value = v;
                return ret;
            }

            if (type == typeof(bool))
            {
                bool v;
                var ret = ParserToUse.TryParse(s, out v);
                value = v;
                return ret;
            }

            value = null;
            return false;
        }
    }
}