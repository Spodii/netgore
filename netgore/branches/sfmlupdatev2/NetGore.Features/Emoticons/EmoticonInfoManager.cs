using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace NetGore.Features.Emoticons
{
    /// <summary>
    /// Contains the <see cref="EmoticonInfo{TKey}"/>s for each corresponding key.
    /// </summary>
    /// <typeparam name="TKey">The emoticon key.</typeparam>
    /// <typeparam name="TValue">The emoticon information.</typeparam>
    public abstract class EmoticonInfoManagerBase<TKey, TValue> where TValue : EmoticonInfo<TKey>
    {
        const string _rootNodeName = "EmoticonInfos";

        readonly Dictionary<TKey, TValue> _dict;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmoticonInfoManagerBase{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="equalityComparer">The equality comparer.</param>
        protected EmoticonInfoManagerBase(IEqualityComparer<TKey> equalityComparer)
        {
            _dict = new Dictionary<TKey, TValue>(equalityComparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmoticonInfoManagerBase{TKey, TValue}"/> class.
        /// </summary>
        protected EmoticonInfoManagerBase()
        {
            _dict = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Gets the emoticon information based off of the given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The emoticon information, or null if the key does not exist.</returns>
        public TValue this[TKey key]
        {
            get
            {
                TValue ret;
                if (!_dict.TryGetValue(key, out ret))
                    return null;

                return ret;
            }
        }

        /// <summary>
        /// Creates a new <see cref="EmoticonInfo{TKey}"/> instance.
        /// </summary>
        /// <returns>A new <see cref="EmoticonInfo{TKey}"/> instance.</returns>
        protected virtual TValue CreateEmoticonInfo()
        {
            return (TValue)new EmoticonInfo<TKey>();
        }

        /// <summary>
        /// Reads the values into this collection from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void Read(IValueReader reader)
        {
            _dict.Clear();

            var nodes = reader.ReadManyNodes(_rootNodeName, ReadHandler);

            foreach (var node in nodes)
            {
                _dict.Add(node.Value, node);
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional reading of the <see cref="EmoticonInfoManagerBase{TKey,TValue}"/>
        /// values. This allows you to read and write values defined in a derived class.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        protected virtual void ReadExtra(IValueReader reader)
        {
        }

        /// <summary>
        /// Handles reading a <typeparamref name="TValue"/>.
        /// </summary>
        /// <param name="r">The <see cref="IValueReader"/>.</param>
        /// <returns>The read <typeparamref name="TValue"/>.</returns>
        TValue ReadHandler(IValueReader r)
        {
            var ret = CreateEmoticonInfo();
            ret.ReadState(r);
            ReadExtra(r);
            return ret;
        }

        /// <summary>
        /// Writes the values in this collection to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        public void Write(IValueWriter writer)
        {
            writer.WriteManyNodes(_rootNodeName, _dict.Values, (w, x) => x.WriteState(w));
            WriteExtra(writer);
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional writing of the <see cref="EmoticonInfoManagerBase{TKey,TValue}"/>
        /// values. This allows you to read and write values defined in a derived class.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        protected virtual void WriteExtra(IValueWriter writer)
        {
        }
    }
}