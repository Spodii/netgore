using System.Linq;
using NetGore;
using NetGore.Features.Emoticons;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Contains the information for <see cref="Emoticon"/>s.
    /// </summary>
    public sealed class EmoticonInfoManager : EmoticonInfoManagerBase<Emoticon, EmoticonInfo<Emoticon>>
    {
        const string _rootNodeName = "EmoticonInfoManager";

        static readonly EmoticonInfoManager _instance;

        /// <summary>
        /// Initializes the <see cref="EmoticonInfoManager"/> class.
        /// </summary>
        static EmoticonInfoManager()
        {
            _instance = Load(ContentPaths.Build);
        }

        /// <summary>
        /// Gets or sets the <see cref="GenericValueIOFormat"/> to use for when an instance of this class
        /// writes itself out to a new <see cref="GenericValueWriter"/>. If null, the format to use
        /// will be inherited from <see cref="GenericValueWriter.DefaultFormat"/>.
        /// </summary>
        public static GenericValueIOFormat? EncodingFormat { get; set; }

        /// <summary>
        /// Gets the <see cref="EmoticonInfoManager"/> instance for <see cref="ContentPaths.Build"/>.
        /// </summary>
        public static EmoticonInfoManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Loads the <see cref="EmoticonInfoManager"/> from file.
        /// </summary>
        /// <param name="filePath">The file path to load from.</param>
        public static EmoticonInfoManager Load(string filePath)
        {
            var r = GenericValueReader.CreateFromFile(filePath, _rootNodeName);

            var ret = new EmoticonInfoManager();
            ret.Read(r);

            return ret;
        }

        /// <summary>
        /// Loads the <see cref="EmoticonInfoManager"/> from file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to load from.</param>
        public static EmoticonInfoManager Load(ContentPaths contentPath)
        {
            return Load(contentPath.Data.Join("emoticons" + EngineSettings.DataFileSuffix));
        }

        /// <summary>
        /// Saves the <see cref="EmoticonInfoManager"/> to file.
        /// </summary>
        /// <param name="filePath">The file path to save to.</param>
        public void Save(string filePath)
        {
            using (var w = GenericValueWriter.Create(filePath, _rootNodeName, EncodingFormat))
            {
                Write(w);
            }
        }
    }
}