using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Contains the <see cref="BodyInfo"/> information.
    /// </summary>
    public sealed class BodyInfoManager
    {
        const string _bodyNodeName = "Body";
        const string _rootNodeName = "Bodies";
        static readonly BodyInfoManager _instance;

        readonly Dictionary<BodyIndex, BodyInfo> _bodies = new Dictionary<BodyIndex, BodyInfo>();

        /// <summary>
        /// Gets the <see cref="BodyInfo"/>s in this <see cref="BodyInfoManager"/>.
        /// </summary>
        public IEnumerable<BodyInfo> Bodies { get { return _bodies.Values; } }

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

        /// <summary>
        /// Gets the <see cref="BodyInfoManager"/> instance.
        /// </summary>
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

        /// <summary>
        /// Gets the default file path for the body info file for the given <see cref="ContentPaths"/>.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to get the default body info file path for.</param>
        /// <returns>The default file path for the body info file for the <paramref name="contentPath"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="contentPath"/> is null.</exception>
        public static string GetDefaultFilePath(ContentPaths contentPath)
        {
            if (contentPath == null)
                throw new ArgumentNullException("contentPath");

            return contentPath.Data.Join("bodies.xml");
        }

        /// <summary>
        /// Loads the <see cref="BodyInfoManager"/> from the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>
        /// The loaded <see cref="BodyInfoManager"/>.
        /// </returns>
        static BodyInfoManager Load(string filePath)
        {
            var reader = new XmlValueReader(filePath, _rootNodeName);
            return new BodyInfoManager(reader);
        }

        /// <summary>
        /// Loads the <see cref="BodyInfoManager"/> from the specified file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to use to load the file from.</param>
        /// <returns>
        /// The loaded <see cref="BodyInfoManager"/>.
        /// </returns>
        static BodyInfoManager Load(ContentPaths contentPath)
        {
            return Load(GetDefaultFilePath(contentPath));
        }

        /// <summary>
        /// Reads the <see cref="BodyInfoManager"/> data from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the <see cref="BodyInfoManager"/> data from.</param>
        public void Read(IValueReader reader)
        {
            var bodies = reader.ReadManyNodes<BodyInfo>(_bodyNodeName, BodyInfo.Read);
            _bodies.Clear();

            foreach (var body in bodies)
            {
                _bodies.Add(body.Index, body);
            }
        }

        /// <summary>
        /// Saves the <see cref="BodyInfoManager"/> data to file.
        /// </summary>
        /// <param name="filePath">The file to save to.</param>
        public void Save(string filePath)
        {
            using (var writer = new XmlValueWriter(filePath, _rootNodeName))
            {
                Write(writer);
            }
        }

        /// <summary>
        /// Saves the <see cref="BodyInfoManager"/> data to file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to save to.</param>
        public void Save(ContentPaths contentPath)
        {
            Save(GetDefaultFilePath(contentPath));
        }

        /// <summary>
        /// Writes the <see cref="BodyInfoManager"/> data to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the <see cref="BodyInfoManager"/> data to.</param>
        public void Write(IValueWriter writer)
        {
            writer.WriteManyNodes(_bodyNodeName, _bodies.Values, (w, body) => body.Write(w));
        }
    }
}