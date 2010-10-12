using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using DemoGame.Server;
using NetGore.Editor;

namespace DemoGame.Editor.TypeConverters
{
    /// <summary>
    /// A <see cref="TypeConverter"/> for a collection of <see cref="CharacterTemplateID"/>s and values.
    /// </summary>
    public class CharacterTemplateAndAmountListTypeConverter : TypeConverter
    {
        /// <summary>
        /// Returns whether this converter can convert the object to the specified type, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/>
        /// that provides a format context.</param>
        /// <param name="destinationType">A <see cref="T:System.Type"/> that represents the type
        /// you want to convert to.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a
        /// format context.</param>
        /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"/>. If null is passed, the current
        /// culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
        /// <param name="destinationType">The <see cref="T:System.Type"/> to convert the <paramref name="value"/>
        /// parameter to.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="destinationType"/> parameter is null. </exception>
        /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                var ev = value as IEnumerable<MutablePair<CharacterTemplateID, ushort>>;
                if (ev != null)
                {
                    var m = CharacterTemplateManager.Instance;
                    if (m != null)
                    {
                        var sb = new StringBuilder();

                        sb.Append("{");
                        foreach (var v in ev.OrderByDescending(x => x.Value).ThenBy(x => x.Key))
                        {
                            sb.Append("(");

                            var t = m[v.Key];
                            if (t == null)
                                sb.Append(v.Key.ToString());
                            else
                                sb.Append(t.TemplateTable.Name);

                            sb.Append(",");
                            sb.Append(v.Value.ToString());
                            sb.Append(")");
                        }

                        if (sb.Length > 1)
                            sb.Length--;

                        sb.Append("}");

                        return sb.ToString();
                    }
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}