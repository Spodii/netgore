using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace NetGore.Features.Skills
{
    /// <summary>
    /// Contains the <see cref="SkillInfo{TKey}"/>s for each corresponding key.
    /// </summary>
    /// <typeparam name="TKey">The Skill key.</typeparam>
    /// <typeparam name="TValue">The Skill information.</typeparam>
    public abstract class SkillInfoManagerBase<TKey, TValue> where TValue : SkillInfo<TKey>
    {
        const string _rootNodeName = "SkillInfos";

        readonly Dictionary<TKey, TValue> _dict;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillInfoManagerBase{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="equalityComparer">The equality comparer.</param>
        protected SkillInfoManagerBase(IEqualityComparer<TKey> equalityComparer)
        {
            _dict = new Dictionary<TKey, TValue>(equalityComparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillInfoManagerBase{TKey, TValue}"/> class.
        /// </summary>
        protected SkillInfoManagerBase()
        {
            _dict = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Gets the skill information based off of the given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The skill information, or null if the key does not exist.</returns>
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
        /// Creates a new <see cref="SkillInfo{TKey}"/> instance.
        /// </summary>
        /// <returns>A new <see cref="SkillInfo{TKey}"/> instance.</returns>
        protected virtual TValue CreateSkillInfo()
        {
            return (TValue)new SkillInfo<TKey>();
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
        /// When overridden in the derived class, allows for additional reading of the <see cref="SkillInfoManagerBase{TKey,TValue}"/>
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
            var ret = CreateSkillInfo();
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
        /// When overridden in the derived class, allows for additional writing of the <see cref="SkillInfoManagerBase{TKey,TValue}"/>
        /// values. This allows you to read and write values defined in a derived class.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        protected virtual void WriteExtra(IValueWriter writer)
        {
        }
    }
}