using System;
using System.Linq;
using NetGore.IO;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Interface for a class that manages the skinning information and settings for GUI <see cref="Control"/>s.
    /// </summary>
    public interface ISkinManager
    {
        /// <summary>
        /// Notifies listeners when the active skin has changed.
        /// </summary>
        event SkinChangeEventHandler OnChangeSkin;

        /// <summary>
        /// Gets or sets the name of the currently active skin.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null or empty.</exception>
        string CurrentSkin { get; set; }

        /// <summary>
        /// Gets the <see cref="ControlBorder"/> for the <see cref="Control"/> with the given name.
        /// </summary>
        /// <param name="controlName">The name of the <see cref="Control"/>.</param>
        /// <returns>The <see cref="ControlBorder"/> for the <see cref="Control"/> with the given name.</returns>
        ControlBorder GetBorder(string controlName);

        /// <summary>
        /// Gets the <see cref="ControlBorder"/> for the <see cref="Control"/> with the given name.
        /// </summary>
        /// <param name="controlName">The name of the <see cref="Control"/>.</param>
        /// <param name="subCategory">The sprite sub-category under the <paramref name="controlName"/>.</param>
        /// <returns>The <see cref="ControlBorder"/> for the <see cref="Control"/> with the given name.</returns>
        ControlBorder GetBorder(string controlName, SpriteCategory subCategory);

        /// <summary>
        /// Adds a <see cref="GUIManagerBase"/> to this <see cref="ISkinManager"/>.
        /// </summary>
        /// <param name="guiManager">The <see cref="GUIManagerBase"/> to add.</param>
        void AddGUIManager(GUIManagerBase guiManager);

        /// <summary>
        /// Removes a <see cref="GUIManagerBase"/> from this <see cref="ISkinManager"/>.
        /// </summary>
        /// <param name="guiManager">The <see cref="GUIManagerBase"/> to add.</param>
        bool RemoveGUIManager(GUIManagerBase guiManager);

        /// <summary>
        /// Gets the <see cref="ISprite"/> for the <see cref="Control"/> with the given <paramref name="spriteTitle"/>.
        /// </summary>
        /// <param name="controlName">The name of the <see cref="Control"/>.</param>
        /// <param name="spriteTitle">The <see cref="SpriteTitle"/> of the <see cref="ISprite"/> to load.</param>
        /// <returns>The <see cref="ISprite"/> for the <see cref="Control"/> with the given
        /// <paramref name="spriteTitle"/>, or null if no <see cref="ISprite"/> could be created with the
        /// given categorization information.</returns>
        ISprite GetControlSprite(string controlName, SpriteTitle spriteTitle);

        /// <summary>
        /// Gets the <see cref="ISprite"/> for the <see cref="Control"/> with the given <paramref name="spriteTitle"/>.
        /// </summary>
        /// <param name="controlName">The name of the <see cref="Control"/>.</param>
        /// <param name="subCategory">The sprite sub-category under the <paramref name="controlName"/>.</param>
        /// <param name="spriteTitle">The <see cref="SpriteTitle"/> of the <see cref="ISprite"/> to load.</param>
        /// <returns>The <see cref="ISprite"/> for the <see cref="Control"/> with the given
        /// <paramref name="spriteTitle"/>, or null if no <see cref="ISprite"/> could be created with the
        /// given categorization information.</returns>
        ISprite GetControlSprite(string controlName, SpriteCategory subCategory, SpriteTitle spriteTitle);

        /// <summary>
        /// Gets the <see cref="ISprite"/> for the skin with the given <see cref="SpriteTitle"/>.
        /// </summary>
        /// <param name="spriteTitle">The title of the <see cref="ISprite"/> to get.</param>
        /// <returns>The <see cref="ISprite"/> for the skin with the given <see cref="SpriteTitle"/>,
        /// or null if no <see cref="ISprite"/> could be created with the given categorization information.</returns>
        ISprite GetSprite(SpriteTitle spriteTitle);

        /// <summary>
        /// Gets the <see cref="ISprite"/> for the skin with the given <see cref="SpriteTitle"/>.
        /// </summary>
        /// <param name="subCategory">The sprite sub-category under the skin.</param>
        /// <param name="spriteTitle">The title of the <see cref="ISprite"/> to get.</param>
        /// <returns>The <see cref="ISprite"/> for the skin with the given <see cref="SpriteTitle"/>,
        /// or null if no <see cref="ISprite"/> could be created with the given categorization information.</returns>
        ISprite GetSprite(SpriteCategory subCategory, SpriteTitle spriteTitle);
    }
}