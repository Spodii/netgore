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
        const string _valueKeyDescription = "Description";
        const string _valueKeyIcon = "Icon";
        const string _valueKeyName = "Name";
        const string _valueKeyType = "Type";

        static readonly InfoManager<StatusEffectType, StatusEffectInfo> _infoManager =
            new InfoManager<StatusEffectType, StatusEffectInfo>(_fileName, EnumComparer<StatusEffectType>.Instance,
                                                                x => new StatusEffectInfo(x), (x, y) => y.Save(x),
                                                                x => x.StatusEffectType);

        static readonly StatusEffectTypeHelper _statusEffectTypeHelper = StatusEffectTypeHelper.Instance;

        static StatusEffectInfo()
        {
            _infoManager.AddMissingTypes(EnumHelper<StatusEffectType>.Values,
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

        public static StatusEffectInfo GetStatusEffectInfo(StatusEffectType statusEffectType)
        {
            return _infoManager[statusEffectType];
        }

        void Read(IValueReader r)
        {
            StatusEffectType = r.ReadEnum(_statusEffectTypeHelper, _valueKeyType);
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
            w.WriteEnum(_statusEffectTypeHelper, _valueKeyType, StatusEffectType);
            w.Write(_valueKeyName, Name);
            w.Write(_valueKeyDescription, Description);
            w.Write(_valueKeyIcon, Icon);
        }
    }
}