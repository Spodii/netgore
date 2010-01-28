using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    /// <summary>
    /// Helper methods for the <see cref="IItemTable"/>.
    /// </summary>
    public static class ItemInfoHelper
    {
        static readonly string _lineBreak = Environment.NewLine;

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
            Color nameColor = Color.LightSeaGreen;
            Color generalColor = Color.LightBlue;
            Color bonusColor = Color.LightGreen;
            Color reqColor = Color.MediumVioletRed;

            if (itemInfo == null)
                return StyledText.EmptyArray;

            // Create and add name and description
            var ret = new List<StyledText>
            { new StyledText(itemInfo.Name, nameColor), new StyledText(_lineBreak + itemInfo.Description) };

            // Value, HP, MP
            CreateValueLine(ret, "Value", itemInfo.Value, generalColor);

            if (itemInfo.HP != 0)
                CreateValueLine(ret, "HP", itemInfo.HP, bonusColor);
            if (itemInfo.MP != 0)
                CreateValueLine(ret, "MP", itemInfo.MP, bonusColor);

            // Stat bonuses
            foreach (var stat in itemInfo.ReqStats.Where(x => x.Value != 0))
            {
                CreateValueLine(ret, stat.Key, stat.Value, bonusColor);
            }

            // Stat requirements
            foreach (var stat in itemInfo.ReqStats.Where(x => x.Value != 0))
            {
                CreateValueLine(ret, stat.Key, stat.Value, reqColor);
            }

            return ret.ToArray();
        }

        public static StyledText[] GetStyledText(IItemTable itemInfo)
        {
            Color nameColor = Color.LightSeaGreen;
            Color generalColor = Color.LightBlue;
            Color bonusColor = Color.LightGreen;
            Color reqColor = Color.MediumVioletRed;

            if (itemInfo == null)
                return StyledText.EmptyArray;

            // Create and add name and description
            var ret = new List<StyledText>
            { new StyledText(itemInfo.Name, nameColor), new StyledText(_lineBreak + itemInfo.Description) };

            // Value, HP, MP
            CreateValueLine(ret, "Value", itemInfo.Value, generalColor);

            if (itemInfo.HP != 0)
                CreateValueLine(ret, "HP", itemInfo.HP, bonusColor);
            if (itemInfo.MP != 0)
                CreateValueLine(ret, "MP", itemInfo.MP, bonusColor);

            // Stat bonuses
            foreach (var stat in itemInfo.ReqStats.Where(x => x.Value != 0))
            {
                CreateValueLine(ret, stat.Key, stat.Value, bonusColor);
            }

            // Stat requirements
            foreach (var stat in itemInfo.ReqStats.Where(x => x.Value != 0))
            {
                CreateValueLine(ret, stat.Key, stat.Value, reqColor);
            }

            return ret.ToArray();
        }
    }
}