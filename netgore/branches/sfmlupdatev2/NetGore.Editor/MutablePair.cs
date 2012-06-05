using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace NetGore.Editor
{
    /// <summary>
    /// A mutable object version of a <see cref="KeyValuePair{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public class MutablePair<TKey, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MutablePair{TKey, TValue}"/> class.
        /// </summary>
        public MutablePair()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MutablePair{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public MutablePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MutablePair{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="kvp">The <see cref="KeyValuePair{TKey, TValue}"/>.</param>
        public MutablePair(KeyValuePair<TKey, TValue> kvp) : this(kvp.Key, kvp.Value)
        {
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        [Browsable(true)]
        [Description("The key.")]
        public TKey Key { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Browsable(true)]
        [Description("The value.")]
        public TValue Value { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "{" + Key + ", " + Value + "}";
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="NetGore.Editor.MutablePair{TKey,TValue}"/>
        /// to <see cref="System.Collections.Generic.KeyValuePair{TKey,TValue}"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator KeyValuePair<TKey, TValue>(MutablePair<TKey, TValue> v)
        {
            return new KeyValuePair<TKey, TValue>(v.Key, v.Value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Collections.Generic.KeyValuePair{TKey,TValue}"/>
        /// to <see cref="NetGore.Editor.MutablePair{TKey,TValue}"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MutablePair<TKey, TValue>(KeyValuePair<TKey, TValue> v)
        {
            return new MutablePair<TKey, TValue>(v.Key, v.Value);
        }
    }
}