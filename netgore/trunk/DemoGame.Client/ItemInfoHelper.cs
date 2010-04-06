using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;
using NetGore.Graphics.GUI;
using SFML.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Helper methods for the <see cref="IItemTable"/>.
    /// </summary>
    public static class ItemInfoHelper
    {
        static readonly Color _bonusColor = new Color(144, 238, 144);
        static readonly Color _generalColor = new Color(173, 216, 230);
        static readonly string _lineBreak = Environment.NewLine;
        static readonly Color _nameColor = new Color(32, 178, 170);
        static readonly Color _reqColor = new Color(200, 21, 133);

        static void CreateValueLine(ICollection<StyledText> dest, object key, object value, Color keyColor)
        {
            CreateValueLine(dest, key.ToString(), value.ToString(), keyColor);
        }

        static void CreateValueLine(ICollection<StyledText> dest, string key, object value, Color keyColor)
        {
            CreateValueLine(dest, key, value.ToString(), keyColor);
        }

        static void CreateValueLine(ICollection<StyledText> dest, string key, string value, Color keyColor)
        {
            key += ": ";

            if (keyColor == StyledText.ColorForDefault)
                dest.Add(new StyledText(key + value, keyColor));
            else
            {
                dest.Add(new StyledText(_lineBreak + key, keyColor));
                dest.Add(new StyledText(value));
            }
        }

        public static StyledText[] GetStyledText(IItemTemplateTable itemInfo)
        {
            if (itemInfo == null)
                return StyledText.EmptyArray;

            // Create and add name and description
            var ret = new List<StyledText>
            { new StyledText(itemInfo.Name, _nameColor), new StyledText(_lineBreak + itemInfo.Description) };

            // Value, HP, MP
            CreateValueLine(ret, "Value", itemInfo.Value, _generalColor);

            if (itemInfo.HP != 0)
                CreateValueLine(ret, "HP", itemInfo.HP, _bonusColor);
            if (itemInfo.MP != 0)
                CreateValueLine(ret, "MP", itemInfo.MP, _bonusColor);

            // Stat bonuses
            foreach (var stat in itemInfo.ReqStats.Where(x => x.Value != 0))
            {
                CreateValueLine(ret, stat.Key, stat.Value, _bonusColor);
            }

            // Stat requirements
            foreach (var stat in itemInfo.ReqStats.Where(x => x.Value != 0))
            {
                CreateValueLine(ret, stat.Key, stat.Value, _reqColor);
            }

            return ret.ToArray();
        }

        public static StyledText[] GetStyledText(IItemTable itemInfo)
        {
            if (itemInfo == null)
                return StyledText.EmptyArray;

            // Create and add name and description
            var ret = new List<StyledText>
            { new StyledText(itemInfo.Name, _nameColor), new StyledText(_lineBreak + itemInfo.Description) };

            // Value, HP, MP
            CreateValueLine(ret, "Value", itemInfo.Value, _generalColor);

            if (itemInfo.HP != 0)
                CreateValueLine(ret, "HP", itemInfo.HP, _bonusColor);
            if (itemInfo.MP != 0)
                CreateValueLine(ret, "MP", itemInfo.MP, _bonusColor);

            // Stat bonuses
            foreach (var stat in itemInfo.ReqStats.Where(x => x.Value != 0))
            {
                CreateValueLine(ret, stat.Key, stat.Value, _bonusColor);
            }

            // Stat requirements
            foreach (var stat in itemInfo.ReqStats.Where(x => x.Value != 0))
            {
                CreateValueLine(ret, stat.Key, stat.Value, _reqColor);
            }

            return ret.ToArray();
        }
    }
}