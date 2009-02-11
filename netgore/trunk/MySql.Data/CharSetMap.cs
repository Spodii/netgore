using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MySql.Data.Common;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// Summary description for CharSetMap.
    /// </summary>
    class CharSetMap
    {
        static readonly Dictionary<string, string> defaultCollations = new Dictionary<string, string>();
        static readonly Dictionary<string, int> maxLengths = new Dictionary<string, int>();
        static Dictionary<string, CharacterSet> mapping;

        // we use a static constructor here since we only want to init
        // the mapping once
        static CharSetMap()
        {
            InitializeMapping();
        }

        public static CharacterSet GetChararcterSet(DBVersion version, string CharSetName)
        {
            CharacterSet cs = mapping[CharSetName];
            if (cs == null)
                throw new MySqlException("Character set '" + CharSetName + "' is not supported");
            return cs;
        }

        internal static string GetDefaultCollation(string charset, MySqlConnection connection)
        {
            lock (defaultCollations)
            {
                if (defaultCollations.Count == 0)
                    InitCollections(connection);
            }
            if (!defaultCollations.ContainsKey(charset))
                return null;
            return defaultCollations[charset];
        }

        /// <summary>
        /// Returns the text encoding for a given MySQL character set name
        /// </summary>
        /// <param name="version">Version of the connection requesting the encoding</param>
        /// <param name="CharSetName">Name of the character set to get the encoding for</param>
        /// <returns>Encoding object for the given character set name</returns>
        public static Encoding GetEncoding(DBVersion version, string CharSetName)
        {
            try
            {
                CharacterSet cs = GetChararcterSet(version, CharSetName);
                return Encoding.GetEncoding(cs.name);
            }
            catch (NotSupportedException)
            {
                return Encoding.GetEncoding(0);
            }
        }

        internal static int GetMaxLength(string charset, MySqlConnection connection)
        {
            // we lock on defaultCollations here too so GetDefaultCollation
            // is on the same lock as us.
            lock (defaultCollations)
            {
                if (maxLengths.Count == 0)
                    InitCollections(connection);
            }

            if (!maxLengths.ContainsKey(charset))
                return 1;
            return maxLengths[charset];
        }

        internal static void InitCollections(MySqlConnection connection)
        {
            MySqlCommand cmd = new MySqlCommand("SHOW CHARSET", connection);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    defaultCollations.Add(reader.GetString(0), reader.GetString(2));
                    maxLengths.Add(reader.GetString(0), Convert.ToInt32(reader.GetValue(3)));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        static void InitializeMapping()
        {
            LoadCharsetMap();
        }

        static void LoadCharsetMap()
        {
            mapping = new Dictionary<string, CharacterSet>
                      { { "latin1", new CharacterSet("latin1", 1) }, { "big5", new CharacterSet("big5", 2) } };

            mapping.Add("dec8", mapping["latin1"]);
            mapping.Add("cp850", new CharacterSet("ibm850", 1));
            mapping.Add("hp8", mapping["latin1"]);
            mapping.Add("koi8r", new CharacterSet("koi8-u", 1));
            mapping.Add("latin2", new CharacterSet("latin2", 1));
            mapping.Add("swe7", mapping["latin1"]);
            mapping.Add("ujis", new CharacterSet("EUC-JP", 3));
            mapping.Add("eucjpms", mapping["ujis"]);
            mapping.Add("sjis", new CharacterSet("sjis", 2));
            mapping.Add("cp932", mapping["sjis"]);
            mapping.Add("hebrew", new CharacterSet("hebrew", 1));
            mapping.Add("tis620", new CharacterSet("windows-874", 1));
            mapping.Add("euckr", new CharacterSet("euc-kr", 2));
            mapping.Add("euc_kr", mapping["euckr"]);
            mapping.Add("koi8u", new CharacterSet("koi8-u", 1));
            mapping.Add("koi8_ru", mapping["koi8u"]);
            mapping.Add("gb2312", new CharacterSet("gb2312", 2));
            mapping.Add("gbk", mapping["gb2312"]);
            mapping.Add("greek", new CharacterSet("greek", 1));
            mapping.Add("cp1250", new CharacterSet("windows-1250", 1));
            mapping.Add("win1250", mapping["cp1250"]);
            mapping.Add("latin5", new CharacterSet("latin5", 1));
            mapping.Add("armscii8", mapping["latin1"]);
            mapping.Add("utf8", new CharacterSet("utf-8", 3));
            mapping.Add("ucs2", new CharacterSet("UTF-16BE", 2));
            mapping.Add("cp866", new CharacterSet("cp866", 1));
            mapping.Add("keybcs2", mapping["latin1"]);
            mapping.Add("macce", new CharacterSet("x-mac-ce", 1));
            mapping.Add("macroman", new CharacterSet("x-mac-romanian", 1));
            mapping.Add("cp852", new CharacterSet("ibm852", 2));
            mapping.Add("latin7", new CharacterSet("iso-8859-7", 1));
            mapping.Add("cp1251", new CharacterSet("windows-1251", 1));
            mapping.Add("win1251ukr", mapping["cp1251"]);
            mapping.Add("cp1251csas", mapping["cp1251"]);
            mapping.Add("cp1251cias", mapping["cp1251"]);
            mapping.Add("win1251", mapping["cp1251"]);
            mapping.Add("cp1256", new CharacterSet("cp1256", 1));
            mapping.Add("cp1257", new CharacterSet("windows-1257", 1));
            mapping.Add("ascii", new CharacterSet("us-ascii", 1));
            mapping.Add("usa7", mapping["ascii"]);
            mapping.Add("binary", mapping["ascii"]);
            mapping.Add("latin3", new CharacterSet("latin3", 1));
            mapping.Add("latin4", new CharacterSet("latin4", 1));
            mapping.Add("latin1_de", new CharacterSet("iso-8859-1", 1));
            mapping.Add("german1", new CharacterSet("iso-8859-1", 1));
            mapping.Add("danish", new CharacterSet("iso-8859-1", 1));
            mapping.Add("czech", new CharacterSet("iso-8859-2", 1));
            mapping.Add("hungarian", new CharacterSet("iso-8859-2", 1));
            mapping.Add("croat", new CharacterSet("iso-8859-2", 1));
            mapping.Add("latvian", new CharacterSet("iso-8859-13", 1));
            mapping.Add("latvian1", new CharacterSet("iso-8859-13", 1));
            mapping.Add("estonia", new CharacterSet("iso-8859-13", 1));
            mapping.Add("dos", new CharacterSet("ibm437", 1));
        }
    }

    class CharacterSet
    {
        public int byteCount;
        public string name;

        public CharacterSet(string name, int byteCount)
        {
            this.name = name;
            this.byteCount = byteCount;
        }
    }
}