using System;
using System.Linq;
using NetGore;
using NetGore.Collections;
using NetGore.IO;

namespace DemoGame.Client
{
    /// <summary>
    /// Describes a skill with the given SkillType.
    /// </summary>
    [Serializable]
    public class SkillInfo
    {
        const string _fileName = "skillinfo.xml";
        const string _valueKeyDescription = "Description";
        const string _valueKeyIcon = "Icon";
        const string _valueKeyName = "Name";
        const string _valueKeyType = "Type";

        static readonly InfoManager<SkillType, SkillInfo> _infoManager;
        static readonly SkillTypeHelper _skillTypeHelper = SkillTypeHelper.Instance;

        /// <summary>
        /// Gets or sets the description of this Skill.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the GrhIndex for this Skill's icon.
        /// </summary>
        public GrhIndex Icon { get; set; }

        /// <summary>
        /// Gets or sets the name of this SkillType.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the SkillType that this SkillInfo is describing.
        /// </summary>
        public SkillType SkillType { get; set; }

        static SkillInfo()
        {
            _infoManager = new InfoManager<SkillType, SkillInfo>(_fileName, EnumComparer<SkillType>.Instance,
                                                                 x => new SkillInfo(x), (x, y) => y.Save(x), x => x.SkillType);
            _infoManager.AddMissingTypes(SkillTypeHelper.Values,
                                         x => new SkillInfo { SkillType = x, Name = x.ToString(), Description = string.Empty });
            _infoManager.Save();
        }

        /// <summary>
        /// SkillInfo constructor.
        /// </summary>
        public SkillInfo()
        {
        }

        /// <summary>
        /// SkillInfo constructor.
        /// </summary>
        /// <param name="skillType">The SkillType that this SkillInfo is describing</param>
        /// <param name="name">The name of this SkillType</param>
        /// <param name="description">The description of this Skill</param>
        /// <param name="icon">The GrhIndex for this Skill's icon</param>
        public SkillInfo(SkillType skillType, string name, string description, GrhIndex icon)
        {
            SkillType = skillType;
            Name = name;
            Description = description;
            Icon = icon;
        }

        public SkillInfo(IValueReader r)
        {
            Read(r);
        }

        public static SkillInfo GetSkillInfo(SkillType skillType)
        {
            return _infoManager[skillType];
        }

        void Read(IValueReader r)
        {
            SkillType = r.ReadEnum(_skillTypeHelper, _valueKeyType);
            Name = r.ReadString(_valueKeyName);
            Description = r.ReadString(_valueKeyDescription);
            Icon = r.ReadGrhIndex(_valueKeyIcon);
        }

        public static void Save()
        {
            _infoManager.Save();
        }

        public void Save(IValueWriter w)
        {
            w.WriteEnum(_skillTypeHelper, _valueKeyType, SkillType);
            w.Write(_valueKeyName, Name);
            w.Write(_valueKeyDescription, Description);
            w.Write(_valueKeyIcon, Icon);
        }
    }
}