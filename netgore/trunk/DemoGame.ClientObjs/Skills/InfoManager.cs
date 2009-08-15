using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using NetGore;
using NetGore.IO;

namespace DemoGame.Client
{
    public class InfoManager<TKey, TValue>
    {
        const string _fileItemElementName = "Item";

        readonly string _fileName;

        readonly Dictionary<TKey, TValue> _dict;

        readonly Func<IValueReader, TValue> _reader;
        readonly Action<IValueWriter, TValue> _writer;
        readonly Func<TValue, TKey> _getKey;

        public TValue this[TKey key] { get { return _dict[key]; } }

        public InfoManager(string fileName, IEqualityComparer<TKey> comparer, Func<IValueReader, TValue> reader, Action<IValueWriter, TValue> writer, Func<TValue, TKey> getKey)
        {
            _fileName = fileName;
            _reader = reader;
            _writer = writer;
            _getKey = getKey;
            _dict = new Dictionary<TKey, TValue>(comparer);

            var values = Load(GetFilePath(ContentPaths.Build));
            foreach (var value in values)
                _dict.Add(value.Key, value.Value);
        }

        public void Save()
        {
            string devPath = GetFilePath(ContentPaths.Dev);
            string buildPath = GetFilePath(ContentPaths.Build);

            Save(devPath);

            if (File.Exists(buildPath))
                File.Delete(buildPath);

            File.Copy(devPath, buildPath);
        }

        void Save(string filePath)
        {
            using (var w = XmlWriter.Create(filePath, new XmlWriterSettings { Indent = true }))
            {
                w.WriteStartDocument();
                w.WriteStartElement(_fileItemElementName + "s");

                foreach (var value in _dict.Values)
                {
                    using (var valueWriter = new XmlValueWriter(w, _fileItemElementName))
                    {
                        _writer(valueWriter, value);
                    }
                }

                w.WriteEndElement();
                w.WriteEndDocument();
            }
        }

        IEnumerable<KeyValuePair<TKey, TValue>> Load(string filePath)
        {
            if (!File.Exists(filePath))
                return Enumerable.Empty<KeyValuePair<TKey, TValue>>();

            List<KeyValuePair<TKey, TValue>> ret = new List<KeyValuePair<TKey, TValue>>();

            using (var r = XmlReader.Create(filePath))
            {
                while (r.Read())
                {
                    if (r.NodeType != XmlNodeType.Element || r.Name != _fileItemElementName)
                        continue;

                    XmlValueReader valueReader = new XmlValueReader(r, _fileItemElementName);
                    var obj = _reader(valueReader);
                    var key = _getKey(obj);

                    ret.Add(new KeyValuePair<TKey, TValue>(key, obj));
                }
            }

            return ret;
        }

        public string GetFilePath(ContentPaths contentPath)
        {
            return contentPath.Data.Join(_fileName);
        }

        public void AddMissingTypes(IEnumerable<TKey> keys, Func<TKey, TValue> creator)
        {
            foreach (var key in keys)
            {
                if (_dict.ContainsKey(key))
                    continue;

                TValue obj = creator(key);
                _dict.Add(key, obj);
            }
        }
    }
}
