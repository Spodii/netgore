using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.World;

namespace NetGore.AI
{
    /// <summary>
    /// Interface for an AI factory.
    /// </summary>
    public interface IAIFactory
    {
        /// <summary>
        /// Gets all of the <see cref="AIID"/>s and the corresponding <see cref="Type"/> used to handle it.
        /// </summary>
        IEnumerable<KeyValuePair<AIID, Type>> AIs { get; }

        /// <summary>
        /// Gets the name of the AI for the given <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to get the AI name for.</param>
        /// <returns>The name of the AI for the given <paramref name="type"/>, or null if the
        /// <paramref name="type"/> is invalid or does not correspond to an AI.</returns>
        string GetAIName(Type type);

        /// <summary>
        /// Gets the name of the AI for the given <see cref="AIID"/>.
        /// </summary>
        /// <param name="aiID">The <see cref="AIID"/> to get the AI name for.</param>
        /// <returns>The name of the AI for the given <paramref name="aiID"/>, or null if the
        /// <paramref name="aiID"/> is invalid or does not correspond to an AI.</returns>
        string GetAIName(AIID aiID);

        /// <summary>
        /// Gets the <see cref="Type"/> used to handle the specified <see cref="AIID"/>.
        /// </summary>
        /// <param name="aiID">The <see cref="AIID"/> to get the <see cref="Type"/> for.</param>
        /// <returns>The <see cref="Type"/> of the class for handling the <paramref name="aiID"/>, or null
        /// if invalid or no value was found.</returns>
        Type GetAIType(AIID aiID);

        /// <summary>
        /// Gets the <see cref="Type"/> used to handle the AI with the given name.
        /// </summary>
        /// <param name="aiName">The name of the AI to get the <see cref="Type"/> for.</param>
        /// <returns>The <see cref="Type"/> of the class for handling the <paramref name="aiName"/>, or null
        /// if invalid or no value was found.</returns>
        Type GetAIType(string aiName);
    }

    /// <summary>
    /// Interface for an AI factory.
    /// </summary>
    /// <typeparam name="T">The Type of DynamicEntity that uses the AI.</typeparam>
    public interface IAIFactory<in T> : IAIFactory where T : DynamicEntity
    {
        /// <summary>
        /// Creates an <see cref="IAI"/> instance.
        /// </summary>
        /// <param name="aiName">Name of the AI.</param>
        /// <param name="entity">The <see cref="DynamicEntity"/> to bind the AI to.</param>
        /// <returns>An <see cref="IAI"/> instance of the given type bound to the <paramref name="entity"/>.</returns>
        IAI Create(string aiName, T entity);

        /// <summary>
        /// Creates an <see cref="IAI"/> instance.
        /// </summary>
        /// <param name="id">ID of the AI.</param>
        /// <param name="entity">The <see cref="DynamicEntity"/> to bind the AI to.</param>
        /// <returns>An <see cref="IAI"/> instance of the given type bound to the <paramref name="entity"/>.</returns>
        IAI Create(AIID id, T entity);
    }
}