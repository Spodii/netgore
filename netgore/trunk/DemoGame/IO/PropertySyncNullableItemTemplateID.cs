using System.Linq;
using NetGore.IO;
using NetGore.IO.PropertySync;

namespace DemoGame
{
    /// <summary>
    /// Implementation of a <see cref="PropertySyncBase{T}"/> that handles synchronizing an ItemTemplateID?.
    /// </summary>
    [PropertySyncHandler(typeof(ItemTemplateID?))]
    public sealed class PropertySyncNullableItemTemplateID : PropertySyncBase<ItemTemplateID?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySyncNullableItemTemplateID"/> class.
        /// </summary>
        /// <param name="syncValueAttributeInfo">The <see cref="SyncValueAttributeInfo"/>.</param>
        public PropertySyncNullableItemTemplateID(SyncValueAttributeInfo syncValueAttributeInfo)
            : base(syncValueAttributeInfo)
        {
        }

        /// <summary>
        /// When overridden in the derived class, reads a value with the specified name from an IValueReader.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="reader">IValueReader to read from.</param>
        /// <returns>Value read from the IValueReader.</returns>
        protected override ItemTemplateID? Read(string name, IValueReader reader)
        {
            ItemTemplateID? ret;

            if (reader.SupportsNameLookup)
            {
                if (reader.SupportsNodes)
                {
                    var r = reader.ReadNode(name);
                    var isNull = r.ReadBool("IsNull");
                    var value = r.ReadItemTemplateID("Value");

                    ret = (isNull ? (ItemTemplateID?)null : value);
                }
                else
                {
                    var value = reader.ReadString(name);
                    int parsed;
                    if (!int.TryParse(value, out parsed))
                        ret = null;
                    else
                        ret = new ItemTemplateID(parsed);
                }
            }
            else
            {
                var hasValue = reader.ReadBool(null);
                if (hasValue)
                    ret = reader.ReadItemTemplateID(null);
                else
                    ret = null;
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, writes a value to an IValueWriter with the specified name.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="value">Value to write.</param>
        protected override void Write(string name, IValueWriter writer, ItemTemplateID? value)
        {
            if (writer.SupportsNameLookup)
            {
                if (writer.SupportsNodes)
                {
                    writer.WriteStartNode(name);
                    writer.Write("IsNull", !value.HasValue);
                    writer.Write("Value", (value.HasValue ? value.Value : new ItemTemplateID(0)));
                    writer.WriteEndNode(name);
                }
                else
                {
                    writer.Write(name, (value.HasValue ? value.Value.ToString() : "NULL"));
                }
            }
            else
            {
                writer.Write(null, value.HasValue);
                if (value.HasValue)
                    writer.Write(null, value.Value);
            }
        }
    }
}