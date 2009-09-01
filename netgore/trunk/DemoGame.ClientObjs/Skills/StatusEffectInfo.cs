using System;
using System.Linq;
using NetGore;
using NetGore.Collections;
using NetGore.IO;

namespace DemoGame.Client
{
    /// <summary>
    /// Describes a status effect with the given StatusEffectType.
    /// </summary>
    [Serializable]
    public class StatusEffectInfo
    {
        const string _fileName = "statuseffectinfo.xml";

        static readonly InfoManager<StatusEffectType, StatusEffectInfo> _infoManager =
            new InfoManager<StatusEffectType, StatusEffectInfo>(_fileName, EnumComparer<StatusEffectType>.Instance,
                                                                x => new StatusEffectInfo(x), (x, y) => y.Save(x),
                                                                x => x.StatusEffectType);

        /// <summary>
        /// Gets or sets the description of this status effect.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the GrhIndex of the icon for this status effect.
        /// </summary>
        public GrhIndex Icon { get; set; }

        /// <summary>
        /// Gets or sets the name of this status effect.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the StatusEffectType that this StatusEffectInfo describes.
        /// </summary>
        public StatusEffectType StatusEffectType { get; set; }

        static StatusEffectInfo()
        {
            _infoManager.AddMissingTypes(StatusEffectTypeHelper.AllValues,
                                         x =>
                                         new StatusEffectInfo
                                         { StatusEffectType = x, Name = x.ToString(), Description = string.Empty });
            _infoManager.Save();
        }

        /// <summary>
        /// StatusEffectInfo constructor.
        /// </summary>
        public StatusEffectInfo()
        {
        }

        public StatusEffectInfo(IValueReader r)
        {
            Read(r);
        }

        public static StatusEffectInfo GetStatusEffectInfo(StatusEffectType statusEffectType)
        {
            return _infoManager[statusEffectType];
        }

        void Read(IValueReader r)
        {
            StatusEffectType = r.ReadStatusEffectType("Type");
            Name = r.ReadString("Name");
            Description = r.ReadString("Description");
            Icon = r.ReadGrhIndex("Icon");
        }

        public static void Save()
        {
            _infoManager.Save();
        }

        public void Save(IValueWriter w)
        {
            w.Write("Type", StatusEffectType);
            w.Write("Name", Name);
            w.Write("Description", Description);
            w.Write("Icon", Icon);
        }
    }
}