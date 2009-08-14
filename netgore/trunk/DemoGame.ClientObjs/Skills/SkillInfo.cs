using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using NetGore;
using NetGore.IO;

namespace DemoGame.Client
{
    [Serializable]
    public class SkillInfo
    {
        public GrhIndex Icon { get; set; }
        public string Description { get; set; }
        public SkillType SkillType { get; set; }
        public string Name { get; set; }

        public SkillInfo()
        {
        }

        public SkillInfo(SkillType skillType, string name, string description, GrhIndex icon)
        {
            SkillType = skillType;
            Name = name;
            Description = description;
            Icon = icon;
        }
    }
}
