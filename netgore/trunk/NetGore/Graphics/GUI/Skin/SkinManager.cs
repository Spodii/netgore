using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NetGore.IO;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A basic implementation of an <see cref="ISkinManager"/> that should suit most all skinning systems.
    /// </summary>
    public class SkinManager : ISkinManager
    {
        readonly Dictionary<string, ControlBorder> _borderCache =
            new Dictionary<string, ControlBorder>(StringComparer.OrdinalIgnoreCase);

        readonly string _defaultSkin;
        readonly List<IGUIManager> _guiManagers = new List<IGUIManager>();
        readonly Dictionary<string, ISprite> _spriteCache = new Dictionary<string, ISprite>(StringComparer.OrdinalIgnoreCase);

        string _currentSkin;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkinManager"/> class.
        /// </summary>
        /// <param name="defaultSkin">The name of the default skin to use.</param>
        /// <exception cref="ArgumentNullException"><paramref name="defaultSkin"/> is null or empty.</exception>
        public SkinManager(string defaultSkin)
        {
            if (string.IsNullOrEmpty(defaultSkin))
                throw new ArgumentNullException("defaultSkin");

            _defaultSkin = defaultSkin;
            CurrentSkin = defaultSkin;
        }

        /// <summary>
        /// Gets the name of the default skin.
        /// </summary>
        public string DefaultSkin
        {
            get { return _defaultSkin; }
        }

        /// <summary>
        /// Gets if the current skin is the default skin.
        /// </summary>
        protected bool IsCurrentSkinDefault { get; private set; }

        /// <summary>
        /// Creates a new <see cref="ControlBorder"/> for the given skin and <see cref="Control"/>. This method should
        /// always return a new <see cref="ControlBorder"/> and not use any sort of caching.
        /// </summary>
        /// <param name="controlName">The name of the <see cref="Control"/>.</param>
        /// <param name="subCategory">The optional sub-category. Can be null.</param>
        /// <returns>A new <see cref="ControlBorder"/> for the given skin and <see cref="Control"/>.</returns>
        protected virtual ControlBorder CreateBorder(string controlName, SpriteCategory subCategory)
        {
            // Get the full sub-category
            var fullSubCategory = GetControlSpriteSubCategory(controlName);
            if (subCategory != null && subCategory.ToString().Length > 0)
                fullSubCategory += SpriteCategorization.Delimiter + subCategory;

            // Load all the sides
            var bg = GetSprite(fullSubCategory, "Background");
            var l = GetSprite(fullSubCategory, "Left");
            var r = GetSprite(fullSubCategory, "Right");
            var b = GetSprite(fullSubCategory, "Bottom");
            var bl = GetSprite(fullSubCategory, "BottomLeft");
            var br = GetSprite(fullSubCategory, "BottomRight");
            var tl = GetSprite(fullSubCategory, "TopLeft");
            var t = GetSprite(fullSubCategory, "Top");
            var tr = GetSprite(fullSubCategory, "TopRight");

            return new ControlBorder(tl, t, tr, r, br, b, bl, l, bg);
        }

        /// <summary>
        /// Gets the <see cref="SpriteCategory"/> for a <see cref="Control"/> relative to the skin's root.
        /// </summary>
        /// <param name="controlName">The name of the <see cref="Control"/>.</param>
        /// <returns>The <see cref="SpriteCategory"/> for a <see cref="Control"/> relative to the skin's root.</returns>
        protected virtual SpriteCategory GetControlSpriteSubCategory(string controlName)
        {
            return "Controls" + SpriteCategorization.Delimiter + controlName;
        }

        /// <summary>
        /// Gets the absolute <see cref="SpriteCategory"/> path for the given skin and skin sub-category.
        /// </summary>
        /// <param name="skinName">The name of the skin.</param>
        /// <param name="subCategory">The sub-category under the skin directory. Can be null or empty.</param>
        /// <returns>The absolute <see cref="SpriteCategory"/> path for the given skin and skin sub-category.</returns>
        protected virtual SpriteCategory GetSpriteCategory(string skinName, SpriteCategory subCategory)
        {
            const string del = SpriteCategorization.Delimiter;

            var sb = new StringBuilder(64);
            sb.Append("GUI");
            sb.Append(del);
            sb.Append(skinName);

            if (subCategory != null && subCategory.ToString().Length > 0)
            {
                sb.Append(del);
                sb.Append(subCategory);
            }

            return new SpriteCategory(sb.ToString());
        }

        /// <summary>
        /// Handles when the skin has changed and applies the new skin to all the <see cref="Control"/>s. This
        /// method is invoked immediately before the <see cref="ISkinManager.SkinChanged"/> event.
        /// </summary>
        protected virtual void OnSkinChanged()
        {
            foreach (var guiManager in GUIManagers)
            {
                foreach (var control in guiManager.GetAllControls())
                {
                    control.LoadSkin(this);
                }
            }
        }

        #region ISkinManager Members

        /// <summary>
        /// Notifies listeners when the active skin has changed.
        /// </summary>
        public event SkinChangeEventHandler SkinChanged;

        /// <summary>
        /// Gets or sets the name of the currently active skin.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null or empty.</exception>
        public string CurrentSkin
        {
            get { return _currentSkin; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("value");

                if (_currentSkin != null && _currentSkin.Equals(value, StringComparison.OrdinalIgnoreCase))
                    return;

                var oldSkin = _currentSkin;
                _currentSkin = value;

                // Clear our caches
                _borderCache.Clear();
                _spriteCache.Clear();

                // Cache if this is the default skin
                if (CurrentSkin.Equals(DefaultSkin, StringComparison.OrdinalIgnoreCase))
                    IsCurrentSkinDefault = true;
                else
                    IsCurrentSkinDefault = false;

                // Apply the new skin to existing Controls
                OnSkinChanged();

                // Only raise the change event if the skin actually changed, not the skin was just set from nothing
                if (oldSkin != null && SkinChanged != null)
                    SkinChanged(_currentSkin, oldSkin);
            }
        }

        /// <summary>
        /// Gets the <see cref="IGUIManager"/>s that this <see cref="ISkinManager"/> is currently managing.
        /// </summary>
        public IEnumerable<IGUIManager> GUIManagers
        {
            get { return _guiManagers; }
        }

        /// <summary>
        /// Adds an <see cref="IGUIManager"/> to this <see cref="ISkinManager"/>.
        /// </summary>
        /// <param name="guiManager">The <see cref="IGUIManager"/> to add.</param>
        void ISkinManager.AddGUIManager(IGUIManager guiManager)
        {
            if (!_guiManagers.Contains(guiManager))
                _guiManagers.Add(guiManager);
        }

        /// <summary>
        /// Gets the <see cref="ControlBorder"/> for the <see cref="Control"/> with the given name.
        /// </summary>
        /// <param name="controlName">The name of the <see cref="Control"/>.</param>
        /// <returns>The <see cref="ControlBorder"/> for the <see cref="Control"/> with the given name.</returns>
        public ControlBorder GetBorder(string controlName)
        {
            return GetBorder(controlName, null);
        }

        /// <summary>
        /// Gets the <see cref="ControlBorder"/> for the <see cref="Control"/> with the given name.
        /// </summary>
        /// <param name="controlName">The name of the <see cref="Control"/>.</param>
        /// <param name="subCategory">The sprite sub-category under the <paramref name="controlName"/>.</param>
        /// <returns>The <see cref="ControlBorder"/> for the <see cref="Control"/> with the given name.</returns>
        public ControlBorder GetBorder(string controlName, SpriteCategory subCategory)
        {
            var key = controlName;
            if (subCategory != null && subCategory.ToString().Length > 0)
                key += subCategory;

            // First, try to get the border from the cache
            ControlBorder ret;
            if (_borderCache.TryGetValue(key, out ret))
                return ret;

            // Didn't exist in the cache, so create the new border and add it to the cache
            ret = CreateBorder(controlName, subCategory);

            try
            {
                _borderCache.Add(key, ret);
            }
            catch (ArgumentException ex)
            {
                Debug.Fail(
                    "Key already exists. Multi-threading conflict? This should never happen, but its likely not critical." + ex);
            }

            // Return the border
            return ret;
        }

        /// <summary>
        /// Gets the <see cref="ISprite"/> for the <see cref="Control"/> with the given <paramref name="spriteTitle"/>.
        /// </summary>
        /// <param name="controlName">The name of the <see cref="Control"/>.</param>
        /// <param name="spriteTitle">The <see cref="SpriteTitle"/> of the <see cref="ISprite"/> to load.</param>
        /// <returns>The <see cref="ISprite"/> for the <see cref="Control"/> with the given
        /// <paramref name="spriteTitle"/>, or null if no <see cref="ISprite"/> could be created with the
        /// given categorization information.</returns>
        public ISprite GetControlSprite(string controlName, SpriteTitle spriteTitle)
        {
            return GetSprite(GetControlSpriteSubCategory(controlName), spriteTitle);
        }

        /// <summary>
        /// Gets the <see cref="ISprite"/> for the <see cref="Control"/> with the given <paramref name="spriteTitle"/>.
        /// </summary>
        /// <param name="controlName">The name of the <see cref="Control"/>.</param>
        /// <param name="subCategory">The sprite sub-category under the <paramref name="controlName"/>.</param>
        /// <param name="spriteTitle">The <see cref="SpriteTitle"/> of the <see cref="ISprite"/> to load.</param>
        /// <returns>The <see cref="ISprite"/> for the <see cref="Control"/> with the given
        /// <paramref name="spriteTitle"/>.</returns>
        public ISprite GetControlSprite(string controlName, SpriteCategory subCategory, SpriteTitle spriteTitle)
        {
            var completeSubCategory = GetControlSpriteSubCategory(controlName);
            if (subCategory != null && subCategory.ToString().Length > 0)
                completeSubCategory += SpriteCategorization.Delimiter + subCategory;

            return GetSprite(completeSubCategory, spriteTitle);
        }

        /// <summary>
        /// Gets the <see cref="ISprite"/> for the skin with the given <see cref="SpriteTitle"/>.
        /// </summary>
        /// <param name="spriteTitle">The title of the <see cref="ISprite"/> to get.</param>
        /// <returns>The <see cref="ISprite"/> for the skin with the given <see cref="SpriteTitle"/>,
        /// or null if no <see cref="ISprite"/> could be created with the given categorization information.</returns>
        public ISprite GetSprite(SpriteTitle spriteTitle)
        {
            return GetSprite(null, spriteTitle);
        }

        /// <summary>
        /// Gets the <see cref="ISprite"/> for the skin with the given <see cref="SpriteTitle"/>.
        /// </summary>
        /// <param name="subCategory">The sprite sub-category under the skin.</param>
        /// <param name="spriteTitle">The title of the <see cref="ISprite"/> to get.</param>
        /// <returns>The <see cref="ISprite"/> for the skin with the given <see cref="SpriteTitle"/>,
        /// or null if no <see cref="ISprite"/> could be created with the given categorization information.</returns>
        public ISprite GetSprite(SpriteCategory subCategory, SpriteTitle spriteTitle)
        {
            var key = string.Empty;
            if (subCategory != null && subCategory.ToString().Length > 0)
                key += subCategory;

            if (spriteTitle != null)
                key += "." + spriteTitle;

            // Check for the sprite in the cache
            ISprite sprite;
            if (_spriteCache.TryGetValue(key, out sprite))
                return sprite;

            // Check for the sprite in the current skin
            var fullSC = GetSpriteCategory(CurrentSkin, subCategory);
            var grhData = GrhInfo.GetData(fullSC, spriteTitle);

            // The sprite was not found in the current category, so check the default category only if the current category
            // is not the default category
            if (grhData == null && !IsCurrentSkinDefault)
            {
                fullSC = GetSpriteCategory(DefaultSkin, subCategory);
                grhData = GrhInfo.GetData(fullSC, spriteTitle);
            }

            // If the grhData is not null, create the sprite, otherwise have the sprite be null
            if (grhData == null)
                sprite = null;
            else
                sprite = new Grh(grhData);

            // Add the sprite to the cache, even if it is null
            try
            {
                _spriteCache.Add(key, sprite);
            }
            catch (ArgumentException ex)
            {
                Debug.Fail(
                    "Key already exists. Multi-threading conflict? This should never happen, but its likely not critical." + ex);
            }

            // Return the sprite
            return sprite;
        }

        /// <summary>
        /// Removes an <see cref="IGUIManager"/> from this <see cref="ISkinManager"/>.
        /// </summary>
        /// <param name="guiManager">The <see cref="IGUIManager"/> to remove.</param>
        bool ISkinManager.RemoveGUIManager(IGUIManager guiManager)
        {
            return _guiManagers.Remove(guiManager);
        }

        #endregion
    }
}