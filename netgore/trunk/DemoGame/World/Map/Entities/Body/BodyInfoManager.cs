using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace DemoGame
{
    public sealed class BodyInfoManager
    {
        const string _bodyNodeName = "Body";
        const string _rootNodeName = "Bodies";
        static readonly BodyInfoManager _instance;

        readonly Dictionary<BodyIndex, BodyInfo> _bodies = new Dictionary<BodyIndex, BodyInfo>();

        /// <summary>
        /// Initializes the <see cref="BodyInfoManager"/> class.
        /// </summary>
        static BodyInfoManager()
        {
            _instance = Load(ContentPaths.Build);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BodyInfoManager"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the collection values from.</param>
        BodyInfoManager(IValueReader reader)
        {
            Read(reader);
        }

        public static BodyInfoManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the <see cref="BodyInfo"/> at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the body to get.</param>
        /// <returns>The <see cref="BodyInfo"/> at the given <paramref name="index"/>, or null if the
        /// <paramref name="index"/> was invalid or no body exists for the given value.</returns>
        public BodyInfo GetBody(BodyIndex index)
        {
            BodyInfo ret;
            if (_bodies.TryGetValue(index, out ret))
                return ret;

            return null;
        }

        public static string GetDefaultFilePath(ContentPaths contentPath)
        {
            return contentPath.Data.Join("bodies.xml");
        }

        static BodyInfoManager Load(string filePath)
        {
            var reader = new XmlValueReader(filePath, _rootNodeName);
            return new BodyInfoManager(reader);
        }

        static BodyInfoManager Load(ContentPaths contentPath)
        {
            return Load(GetDefaultFilePath(contentPath));
        }

        public void Read(IValueReader reader)
        {
            var bodies = reader.ReadManyNodes<BodyInfo>(_bodyNodeName, BodyInfo.Read);
            _bodies.Clear();

            foreach (var body in bodies)
            {
                _bodies.Add(body.Index, body);
            }
        }

        public void Save(string filePath)
        {
            using (var writer = new XmlValueWriter(filePath, _rootNodeName))
            {
                Write(writer);
            }
        }

        public void Save(ContentPaths contentPath)
        {
            Save(GetDefaultFilePath(contentPath));
        }

        public void Write(IValueWriter writer)
        {
            writer.WriteManyNodes(_bodyNodeName, _bodies.Values, (w, body) => body.Write(w));
        }
    }
}