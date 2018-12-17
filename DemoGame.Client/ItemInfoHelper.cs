using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;
using NetGore.Graphics.GUI;
using SFML.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Helper methods for displaying the information for items.
    /// </summary>
    public static class ItemInfoHelper
    {
        /// <summary>
        /// The <see cref="Color"/> to use for the text for item bonuses.
        /// </summary>
        static readonly Color _bonusColor = new Color(144, 238, 144);

        /// <summary>
        /// The <see cref="Color"/> to use for the text for general item information.
        /// </summary>
        static readonly Color _generalColor = new Color(173, 216, 230);

        /// <summary>
        /// The <see cref="Color"/> to use for the text for the item's name.
        /// </summary>
        static readonly Color _nameColor = new Color(32, 178, 170);

        /// <summary>
        /// The <see cref="Color"/> to use for the text for item requirements.
        /// </summary>
        static readonly Color _reqColor = new Color(200, 21, 133);

        /// <summary>
        /// Create a line of text for items in the form of a key and value and adds it to the collection.
        /// </summary>
        /// <param name="dest">The collection to add the created line of text to.</param>
        /// <param name="key">The key portion of the line (e.g. the "Strength" in: "Strength: 5").</param>
        /// <param name="value">The value portion of the line (e.g. the "5" in: "Strength: 5").</param>
        /// <param name="keyColor">The color to give the key.</param>
        static void CreateValueLine(ICollection<StyledText> dest, object key, object value, Color keyColor)
        {
            CreateValueLine(dest, key.ToString(), value.ToString(), keyColor);
        }

        /// <summary>
        /// Create a line of text for items in the form of a key and value and adds it to the collection.
        /// </summary>
        /// <param name="dest">The collection to add the created line of text to.</param>
        /// <param name="key">The key portion of the line (e.g. the "Strength" in: "Strength: 5").</param>
        /// <param name="value">The value portion of the line (e.g. the "5" in: "Strength: 5").</param>
        /// <param name="keyColor">The color to give the key.</param>
        static void CreateValueLine(ICollection<StyledText> dest, string key, object value, Color keyColor)
        {
            CreateValueLine(dest, key, value.ToString(), keyColor);
        }

        /// <summary>
        /// Create a line of text for items in the form of a key and value and adds it to the collection.
        /// </summary>
        /// <param name="dest">The collection to add the created line of text to.</param>
        /// <param name="key">The key portion of the line (e.g. the "Strength" in: "Strength: 5").</param>
        /// <param name="value">The value portion of the line (e.g. the "5" in: "Strength: 5").</param>
        /// <param name="keyColor">The color to give the key.</param>
        static void CreateValueLine(ICollection<StyledText> dest, string key, string value, Color keyColor)
        {
            key += ": ";

            if (keyColor == StyledText.ColorForDefault)
                dest.Add(new StyledText(key + value, keyColor));
            else
            {
                dest.Add(new StyledText(Environment.NewLine + key, keyColor));
                dest.Add(new StyledText(value));
            }
        }

        /// <summary>
        /// Creates the <see cref="StyledText"/> lines needed to display the information for an item.
        /// </summary>
        /// <param name="itemInfo">The item information to create the <see cref="StyledText"/> lines for.</param>
        /// <returns>The <see cref="StyledText"/> lines needed to display the information for the <paramref name="itemInfo"/>.</returns>
        public static StyledText[] GetStyledText(IItemTemplateTable itemInfo)
        {
            if (itemInfo == null)
                return StyledText.EmptyArray;

            return GetStyledText(itemInfo.Name, itemInfo.Description, itemInfo.Value, itemInfo.HP, itemInfo.MP, itemInfo.Stats, itemInfo.ReqStats);
        }

        /// <summary>
        /// Creates the <see cref="StyledText"/> lines needed to display the information for an item.
        /// </summary>
        /// <param name="itemInfo">The item information to create the <see cref="StyledText"/> lines for.</param>
        /// <returns>The <see cref="StyledText"/> lines needed to display the information for the <paramref name="itemInfo"/>.</returns>
        public static StyledText[] GetStyledText(IItemTable itemInfo)
        {
            if (itemInfo == null)
                return StyledText.EmptyArray;

            return GetStyledText(itemInfo.Name, itemInfo.Description, itemInfo.Value, itemInfo.HP, itemInfo.MP, itemInfo.Stats, itemInfo.ReqStats);
        }

        /// <summary>
        /// Creates the <see cref="StyledText"/> lines needed to display the information for an item. Provides a general way to
        /// gather and process the item information so we can work on various different sources that are not compatible with one another
        /// (e.g. <see cref="IItemTable"/> and <see cref="IItemTemplateTable"/>).
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="desc">The description.</param>
        /// <param name="value">The value.</param>
        /// <param name="hp">The hp.</param>
        /// <param name="mp">The mp.</param>
        /// <param name="stats">The base stats.</param>
        /// <param name="reqStats">The requirement stats.</param>
        /// <returns>
        /// The <see cref="StyledText"/> lines needed to display the information for the item.
        /// </returns>
        static StyledText[] GetStyledText(string name, string desc, int value, SPValueType hp, SPValueType mp,
                                          IEnumerable<KeyValuePair<StatType, int>> stats,
                                          IEnumerable<KeyValuePair<StatType, int>> reqStats)
        {
            // Create and add name and description
            var ret = new List<StyledText>();

            if (!string.IsNullOrEmpty(name))
                ret.Add(new StyledText(name, _nameColor));

            if (!string.IsNullOrEmpty(desc))
                ret.Add(new StyledText(Environment.NewLine + desc));

            // Value, HP, MP
            CreateValueLine(ret, "Value", value, _generalColor);

            if (hp != 0)
                CreateValueLine(ret, "HP", hp, _bonusColor);
            if (mp != 0)
                CreateValueLine(ret, "MP", mp, _bonusColor);

            // Stat bonuses
            foreach (var stat in stats.Where(x => x.Value != 0))
            {
                CreateValueLine(ret, stat.Key, stat.Value, _bonusColor);
            }

            // Stat requirements
            foreach (var stat in reqStats.Where(x => x.Value != 0))
            {
                CreateValueLine(ret, stat.Key, stat.Value, _reqColor);
            }

            return ret.ToArray();
        }
    }
}