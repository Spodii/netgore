using System.ComponentModel;
using System.Linq;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// A <see cref="TypeConverter"/> for the <see cref="BodyIndex"/> type.
    /// </summary>
    public class BodyIndexTypeConverter : SimpleTypeConverter<BodyIndex>
    {
        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/> to the output
        /// type.
        /// </summary>
        /// <param name="value">A string containing the value to parse.</param>
        /// <param name="wasConverted">Contains true if the parsing was successful, or false if the
        /// parsing failed.</param>
        /// <returns>The object parsed from the <paramref name="value"/>.</returns>
        protected override BodyIndex ConvertFromString(string value, out bool wasConverted)
        {
            int i;
            wasConverted = int.TryParse(value, out i);

            if (wasConverted)
                return new BodyIndex(i);

            return new BodyIndex(1);
        }

        /// <summary>
        /// When overridden in the derived class, gets the extended text to display in for the type
        /// while it is in a PropertyGrid but is not the selected item.
        /// </summary>
        /// <param name="value">The object to get the extended text for.</param>
        /// <returns>
        /// The extended text to display. Can be null.
        /// </returns>
        protected override string GetExtendedText(object value)
        {
            if (value is BodyIndex)
            {
                return BodyInfoManager.Instance.GetBody((BodyIndex)value).Body;
            }

            return base.GetExtendedText(value);
        }
    }
}