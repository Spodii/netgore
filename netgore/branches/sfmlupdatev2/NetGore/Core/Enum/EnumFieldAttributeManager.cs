using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore
{
    /// <summary>
    /// Facilitates lookup of the attributes of type <typeparamref name="TAttribute"/> for the values of the enum of
    /// type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The type of enum.</typeparam>
    /// <typeparam name="TAttribute">The type of attribute.</typeparam>
    public class EnumFieldAttributeManager<TEnum, TAttribute>
        where TEnum : struct, IComparable, IConvertible, IFormattable where TAttribute : Attribute
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Dictionary<TEnum, TAttribute> _attributes = new Dictionary<TEnum, TAttribute>(EnumComparer<TEnum>.Instance);

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumFieldAttributeManager{TEnum, TAttribute}"/> class.
        /// </summary>
        /// <param name="requireAttribute">If true, all of the enum's defined values must contain the attribute
        /// <typeparamref name="TAttribute"/>. If any fields are missing this attribute, a <see cref="TypeException"/>
        /// will be thrown.</param>
        /// <exception cref="TypeException">The enum has one or more values that do not contain the required attribute.</exception>
        public EnumFieldAttributeManager(bool requireAttribute)
        {
            var fields = typeof(TEnum).GetFields().Where(x => !x.IsSpecialName);
            foreach (var field in fields)
            {
                var value = (TEnum)field.GetValue(null);

                // Grab the given attribute
                var attribute = field.GetCustomAttributes(typeof(TAttribute), true).OfType<TAttribute>().FirstOrDefault();
                if (attribute == null)
                {
                    // The attribute didn't exist, so throw the exception or just move to the next field
                    if (requireAttribute)
                    {
                        const string errmsg = "Enum `{0}`'s value `{1}` does not contain the required attribute `{2}`.";
                        var err = string.Format(errmsg, typeof(TEnum), field.Name, typeof(TAttribute));
                        if (log.IsFatalEnabled)
                            log.Fatal(err);
                        Debug.Fail(err);
                        throw new TypeException(err, typeof(TEnum));
                    }
                    else
                        continue;
                }

                // Add to the dictionary
                _attributes.Add(value, attribute);

                // Allow for additional handling
                HandleLoadAttribute(value, attribute);
            }
        }

        /// <summary>
        /// Gets the attribute for the given enum value.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>The attribute for the given enum value, or null if no attribute was found.</returns>
        public TAttribute GetAttribute(TEnum enumValue)
        {
            TAttribute attribute;
            if (_attributes.TryGetValue(enumValue, out attribute))
                return attribute;

            return null;
        }

        /// <summary>
        /// Allows for a chance to provide additional handling of when an attribute is loaded for an enum value.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="attribute">The attribute.</param>
        protected virtual void HandleLoadAttribute(TEnum enumValue, TAttribute attribute)
        {
        }
    }
}