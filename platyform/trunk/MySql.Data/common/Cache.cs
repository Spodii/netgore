using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MySql.Data.Common
{
    class Cache<KeyType, ValueType>
    {
        readonly int _capacity;
        readonly Dictionary<KeyType, ValueType> _contents;
        readonly Queue<KeyType> _keyQ;

        public ValueType this[KeyType key]
        {
            get
            {
                ValueType val;
                if (_contents.TryGetValue(key, out val))
                    return val;
                else
                    return default(ValueType);
            }
            set { InternalAdd(key, value); }
        }

        public Cache(int initialCapacity, int capacity)
        {
            _capacity = capacity;
            _contents = new Dictionary<KeyType, ValueType>(initialCapacity);

            if (capacity > 0)
                _keyQ = new Queue<KeyType>(initialCapacity);
        }

        public void Add(KeyType key, ValueType value)
        {
            InternalAdd(key, value);
        }

        void InternalAdd(KeyType key, ValueType value)
        {
            if (!_contents.ContainsKey(key))
            {
                if (_capacity > 0)
                {
                    _keyQ.Enqueue(key);

                    if (_keyQ.Count > _capacity)
                        _contents.Remove(_keyQ.Dequeue());
                }
            }

            _contents[key] = value;
        }
    }
}