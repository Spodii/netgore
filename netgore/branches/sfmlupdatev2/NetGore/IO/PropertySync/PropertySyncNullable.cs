using System.Linq;

namespace NetGore.IO.PropertySync
{
    /// <summary>
    /// Handles writing nullable structs for the <see cref="PropertySyncBase{T}"/>.
    /// </summary>
    /// <typeparam name="T">The non-nullable struct type.</typeparam>
    class PropertySyncNullable<T> : PropertySyncBase<T?> where T : struct
    {
        const string _hasValueValueKey = "HasValue";
        const string _valueValueKey = "Value";

        readonly PropertySyncBase<T> _nonNullableSync;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySyncBase{T}"/> class.
        /// </summary>
        /// <param name="syncValueAttributeInfo">The <see cref="SyncValueAttributeInfo"/>.</param>
        public PropertySyncNullable(SyncValueAttributeInfo syncValueAttributeInfo) : base(syncValueAttributeInfo)
        {
            var nonNullableType = TypeHelper.NullableToNonNullable(syncValueAttributeInfo.PropertyType);
            _nonNullableSync = (PropertySyncBase<T>)PropertySyncHelper.GetUnhookedPropertySync(nonNullableType);
        }

        /// <summary>
        /// When overridden in the derived class, reads a value with the specified name from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="reader"><see cref="IValueReader"/> to read from.</param>
        /// <returns>Value read from the <see cref="IValueReader"/>.</returns>
        protected override T? Read(string name, IValueReader reader)
        {
            T? ret;

            if (reader.SupportsNodes)
            {
                bool hasValue;
                T value;

                if (reader.SupportsNameLookup)
                {
                    var r = reader.ReadNode(name);
                    hasValue = r.ReadBool(_hasValueValueKey);
                    value = _nonNullableSync.InternalRead(_valueValueKey, r);
                }
                else
                {
                    hasValue = reader.ReadBool("__" + name + "_HasValue__");
                    value = _nonNullableSync.InternalRead(name, reader);
                }

                ret = (hasValue ? value : default(T?));
            }
            else
            {
                var hasValue = reader.ReadBool(null);
                if (hasValue)
                    ret = _nonNullableSync.InternalRead(name, reader);
                else
                    ret = default(T?);
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, writes a value to an <see cref="IValueWriter"/> with the specified name.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="writer"><see cref="IValueWriter"/> to write to.</param>
        /// <param name="value">Value to write.</param>
        protected override void Write(string name, IValueWriter writer, T? value)
        {
            if (writer.SupportsNodes)
            {
                if (writer.SupportsNameLookup)
                {
                    writer.WriteStartNode(name);
                    {
                        writer.Write(_hasValueValueKey, value.HasValue);
                        _nonNullableSync.InternalWrite(_valueValueKey, writer, (value.HasValue ? value.Value : default(T)));
                    }
                    writer.WriteEndNode(name);
                }
                else
                {
                    writer.Write("__" + name + "_HasValue__", value.HasValue);
                    _nonNullableSync.InternalWrite(name, writer, (value.HasValue ? value.Value : default(T)));
                }
            }
            else
            {
                writer.Write(null, value.HasValue);
                if (value.HasValue)
                    _nonNullableSync.InternalWrite(name, writer, value.Value);
            }
        }
    }
}