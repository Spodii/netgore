using System.Linq;
using NetGore;
using NetGore.Features.Skills;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Contains the information for <see cref="SkillType"/>s.
    /// </summary>
    public class SkillInfoManager : SkillInfoManagerBase<SkillType, SkillInfo<SkillType>>
    {
        const string _rootNodeName = "SkillInfoManager";

        static readonly SkillInfoManager _instance;

        /// <summary>
        /// Initializes the <see cref="SkillInfoManager"/> class.
        /// </summary>
        static SkillInfoManager()
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
        /// Gets the <see cref="SkillInfoManager"/> instance for <see cref="ContentPaths.Build"/>.
        /// </summary>
        public static SkillInfoManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Loads the <see cref="SkillInfoManager"/> from file.
        /// </summary>
        /// <param name="filePath">The file path to load from.</param>
        public static SkillInfoManager Load(string filePath)
        {
            var r = GenericValueReader.CreateFromFile(filePath, _rootNodeName);

            var ret = new SkillInfoManager();
            ret.Read(r);

            return ret;
        }

        /// <summary>
        /// Loads the <see cref="SkillInfoManager"/> from file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to load from.</param>
        public static SkillInfoManager Load(ContentPaths contentPath)
        {
            return Load(contentPath.Data.Join("skills" + EngineSettings.DataFileSuffix));
        }

        /// <summary>
        /// Saves the <see cref="SkillInfoManager"/> to file.
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