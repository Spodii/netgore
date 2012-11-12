using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Db.Schema
{
    /// <summary>
    /// Used to compare two <see cref="SchemaReader"/> results.
    /// </summary>
    public static class SchemaComparer
    {
        /// <summary>
        /// Compares two <see cref="SchemaReader"/>s.
        /// </summary>
        /// <param name="main">The first <see cref="SchemaReader"/>.</param>
        /// <param name="sub">The other <see cref="SchemaReader"/>.</param>
        /// <returns>Text describing the difference between the two <see cref="SchemaReader"/>s.</returns>
        public static IEnumerable<string> Compare(SchemaReader main, SchemaReader sub)
        {
            var missingColumns = new List<ColumnSchema>();
            var mismatchColumns = new List<ColumnSchema>();

            foreach (var table in main.TableSchemas)
            {
                var t = table;
                var t2 = sub.TableSchemas.FirstOrDefault(x => x.TableName == t.TableName);
                if (t2 == null)
                {
                    // Table not found
                    yield return string.Format("TABLE `{0}`: MISSING", t.TableName);
                }
                else
                {
                    // Table found
                    missingColumns.Clear();
                    mismatchColumns.Clear();

                    foreach (var column in t.Columns)
                    {
                        var c = column;
                        var c2 = t2.Columns.FirstOrDefault(x => x.Name == c.Name);
                        if (c2 == null)
                            missingColumns.Add(c);
                        else if (!c.EqualValues(c2))
                            mismatchColumns.Add(c);
                    }

                    // List of missing and mismatches
                    if (missingColumns.Count > 0)
                        yield return string.Format("TABLE `{0}`: COLUMNS MISSING: {1}", t.TableName, Concat(missingColumns));

                    if (mismatchColumns.Count > 0)
                        yield return string.Format("TABLE `{0}`: DIFFERENT COLUMNS: {1}", t.TableName, Concat(mismatchColumns));
                }
            }
        }

        /// <summary>
        /// Concatenates the <see cref="ColumnSchema"/>s into a comma-delimited string.
        /// </summary>
        /// <param name="columns">The <see cref="ColumnSchema"/>s.</param>
        /// <returns>The <see cref="ColumnSchema"/>s concatenated a comma-delimited string.</returns>
        static string Concat(IEnumerable<ColumnSchema> columns)
        {
            if (columns.IsEmpty())
                return string.Empty;

            if (columns.Count() == 1)
                return columns.First().Name;

            columns = columns.OrderBy(x => x.Name);

            var sb = new StringBuilder();
            foreach (var c in columns)
            {
                sb.Append(c.Name);
                sb.Append(", ");
            }
            sb.Length -= 2;

            return sb.ToString();
        }
    }
}