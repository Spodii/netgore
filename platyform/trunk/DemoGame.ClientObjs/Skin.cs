using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Platyform.Extensions;
using Platyform.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Handles when a skin's currently used skin has changed.
    /// </summary>
    /// <param name="newSkin">Name of the new skin.</param>
    /// <param name="oldSkin">Name of the previously used skin.</param>
    public delegate void SkinChangeHandler(string newSkin, string oldSkin);

    /// <summary>
    /// Keeps track of the name of the skin being used, and assists in using the skin.
    /// </summary>
    public static class Skin
    {
        /// <summary>
        /// Name of the default skin to use. It is assumed this skin is complete (ie. contains all
        /// assets used by skins), and that any assets found in other skin will fall back onto this skin.
        /// </summary>
        public const string DefaultSkin = "Default";

        static string _current = DefaultSkin;

        /// <summary>
        /// Notifies listeners when the skin has changed.
        /// </summary>
        public static event SkinChangeHandler OnChange;

        /// <summary>
        /// Gets or sets the current skin.
        /// </summary>
        public static string Current
        {
            get { return _current; }
            set
            {
                if (_current == value)
                    return;

                string last = _current;
                _current = value;

                if (OnChange != null)
                    OnChange(_current, last);
            }
        }

        /// <summary>
        /// Gets the GrhData used by this Skin for the given subcategory and name. If no GrhData is found
        /// under this skin, the default skin will be used.
        /// </summary>
        /// <param name="subCategory">GUI subcategory.</param>
        /// <param name="title">Title of the GrhData.</param>
        /// <returns>GrhData for this skin, or default skin, under the given <paramref name="subCategory"/>
        /// and <paramref name="title"/>.</returns>
        public static GrhData GetSkinGrhData(string subCategory, string title)
        {
            string skin = Current;

            // Check the current skin
            string category = ResolveCategory(skin, subCategory);
            GrhData ret = GrhInfo.GetData(category, title);

            // Check the default skin if it wasn't found in the current skin
            if (ret == null && DefaultSkin != skin)
            {
                category = ResolveCategory(DefaultSkin, subCategory);
                ret = GrhInfo.GetData(category, title);
            }

            if (ret == null)
                throw new ArgumentException(string.Format("Failed to find GrhData `{0}`.", category + "." + title));

            return ret;
        }

        /// <summary>
        /// Gets the GrhData for the given toolbar item for this skin.
        /// </summary>
        /// <param name="itemName">Name of the toolbar item.</param>
        /// <returns>GrhData for the given toolbar item name.</returns>
        public static GrhData GetToolbarItem(string itemName)
        {
            return GetSkinGrhData("Toolbar", itemName);
        }

        /// <summary>
        /// Gets the complete skin category from the given <paramref name="subCategory"/>.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        /// <param name="subCategory">Skin subcategory.</param>
        /// <returns>Complete skin category, including the <paramref name="subCategory"/>.</returns>
        static string ResolveCategory(string skin, string subCategory)
        {
            string category;

            // Remove any trailing periods
            if (subCategory.EndsWith("."))
                subCategory = subCategory.Substring(0, subCategory.Length - 1);

            // Handle having an extra period at the start of the subcategory
            if (subCategory.StartsWith("."))
                category = "GUI." + Current + subCategory;
            else
                category = "GUI." + Current + "." + subCategory;

            return category;
        }
    }
}