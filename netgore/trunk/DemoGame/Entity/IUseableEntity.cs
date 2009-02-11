using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Platyform;
using Platyform.Extensions;

namespace DemoGame
{
    /// <summary>
    /// Interface for an <see cref="Entity"/> that can be used by a <see cref="CharacterEntity"/>
    /// </summary>
    public interface IUseableEntity
    {
        /// <summary>
        /// Notifies listeners that this <see cref="Entity"/> was used,
        /// and which <see cref="CharacterEntity"/> used it
        /// </summary>
        event EntityEventHandler<CharacterEntity> OnUse;

        /// <summary>
        /// Checks if this <see cref="Entity"/> can be used by the specified <paramref name="charEntity"/>, but does
        /// not actually use this <see cref="Entity"/>
        /// </summary>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that is trying to use this <see cref="Entity"/></param>
        /// <returns>True if this <see cref="Entity"/> can be used, else false</returns>
        bool CanUse(CharacterEntity charEntity);

        /// <summary>
        /// Uses this <see cref="Entity"/>
        /// </summary>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that is trying to use this <see cref="Entity"/></param>
        /// <returns>True if this <see cref="Entity"/> was successfully used, else false</returns>
        bool Use(CharacterEntity charEntity);
    }
}