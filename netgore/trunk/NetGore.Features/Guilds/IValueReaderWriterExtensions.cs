using System;
using System.Linq;
using NetGore.IO;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Extension methods for the <see cref="IValueReader"/> and <see cref="IValueWriter"/>.
    /// </summary>
    public static class IValueReaderWriterExtensions
    {
        const string _concatDelimiter = ";";
        const string _nameValueKey = "Name";
        const string _rankValueKey = "Rank";

        /// <summary>
        /// Reads a <see cref="GuildMemberNameRank"/> from a <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the <see cref="GuildMemberNameRank"/> from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>The <see cref="GuildMemberNameRank"/> read from the <paramref name="reader"/>.</returns>
        public static GuildMemberNameRank ReadGuildMemberNameRank(this IValueReader reader, string name)
        {
            string memberName;
            GuildRank rank;

            if (reader.SupportsNameLookup)
            {
                if (reader.SupportsNodes)
                {
                    var r = reader.ReadNode(name);
                    memberName = r.ReadString(_nameValueKey);
                    rank = r.ReadByte(_rankValueKey);
                }
                else
                {
                    var s = reader.ReadString(name);
                    var lastDelimiter = s.LastIndexOf(_concatDelimiter, StringComparison.Ordinal);
                    memberName = s.Substring(0, lastDelimiter);
                    var sRank = s.Substring(lastDelimiter + 1);
                    rank = byte.Parse(sRank);
                }
            }
            else
            {
                memberName = reader.ReadString(null);
                rank = reader.ReadByte(null);
            }

            return new GuildMemberNameRank(memberName, rank);
        }

        /// <summary>
        /// Writes a <see cref="GuildMemberNameRank"/> to a <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the <paramref name="value"/> to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">The <see cref="GuildMemberNameRank"/> to write.</param>
        public static void Write(this IValueWriter writer, string name, GuildMemberNameRank value)
        {
            if (writer.SupportsNameLookup)
            {
                if (writer.SupportsNodes)
                {
                    // Write out using a child node
                    writer.WriteStartNode(name);
                    writer.Write(_nameValueKey, value.Name);
                    writer.Write(_rankValueKey, value.Rank);
                    writer.WriteEndNode(name);
                }
                else
                {
                    // Concat the values together so name lookup still works
                    var s = value.Name + _concatDelimiter + value.Rank;
                    writer.Write(name, s);
                }
            }
            else
            {
                // No need for name lookup, so just write out raw
                writer.Write(null, value.Name);
                writer.Write(null, value.Rank);
            }
        }
    }
}