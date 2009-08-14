using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using NetGore;
using NetGore.Collections;

namespace DemoGame.Client
{
    public static class SkillInfoManager
    {
        const string _fileName = "skillinfo.xml";
        static readonly Dictionary<SkillType, SkillInfo> _skillInfo;

        static SkillInfoManager()
        {
            string filePath = GetFilePath(ContentPaths.Build);
            _skillInfo = Load(filePath);

            if (!File.Exists(filePath))
                Save();
        }

        static string GetFilePath(ContentPaths contentPath)
        {
            return contentPath.Data.Join(_fileName);
        }

        public static SkillInfo GetSkillInfo(SkillType skillType)
        {
            return _skillInfo[skillType];
        }

        static Dictionary<SkillType, SkillInfo> Load(string filePath)
        {
            var ret = new Dictionary<SkillType, SkillInfo>(EnumComparer<SkillType>.Instance);

            if (File.Exists(filePath))
            {

                var serializer = new DataContractSerializer(typeof(SkillInfo[]));
                SkillInfo[] items;

                using (var r = XmlReader.Create(filePath))
                    items = (SkillInfo[])serializer.ReadObject(r);

                foreach (var item in items)
                    ret.Add(item.SkillType, item);
            }

            return ret;
        }

        public static void Save()
        {
            Save(GetFilePath(ContentPaths.Dev));
        }

        static void Save(string filePath)
        {
            var serializer = new DataContractSerializer(typeof(SkillInfo[]));
            SkillInfo[] items = _skillInfo.Values.ToArray();
            using (var w = XmlWriter.Create(filePath, new XmlWriterSettings { Indent = true }))
                serializer.WriteObject(w, items);
        }
    }
}