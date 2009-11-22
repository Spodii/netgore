using System.ComponentModel;
using System.Linq;

namespace NetGore
{
    public interface IVariableValue<T>
    {
        /// <summary>
        /// Gets or sets the inclusive maximum possible value. If this value is set to less than <see cref="Min"/>,
        /// then <see cref="Min"/> will be lowered to equal this value.
        /// </summary>
        [Description("The inclusive maximum possible value.")]
        [Category("Variable Value")]
        [EditorBrowsable]
        T Max { get; set; }

        /// <summary>
        /// Gets or sets the inclusive minimum possible value. If this value is set to greater than <see cref="Max"/>,
        /// then <see cref="Max"/> will be raised to equal this value.
        /// </summary>
        [Description("The inclusive minimum possible value.")]
        [Category("Variable Value")]
        [EditorBrowsable]
        T Min { get; set; }

        /// <summary>
        /// Gets the next value, based off of the <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <returns>The next value, based off of the <see cref="Min"/> and <see cref="Max"/>.</returns>
        T GetNext();
    }
}