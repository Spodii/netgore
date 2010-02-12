using System.Linq;

namespace DemoGame.Server
{
    /// <summary>
    /// Delegate for handling events from the <see cref="Character"/>.
    /// </summary>
    /// <param name="character">The <see cref="Character"/> that the event came from.</param>
    /// <typeparam name="T1">The type of the first event argument.</typeparam>
    /// <typeparam name="T2">The type of the second event argument.</typeparam>
    /// <typeparam name="T3">The type of the third event argument.</typeparam>
    /// <param name="arg1">The first argument related to the event.</param>
    /// <param name="arg2">The second argument related to the event.</param>
    /// <param name="arg3">The third argument related to the event.</param>
    public delegate void CharacterEventHandler<T1, T2, T3>(Character character, T1 arg1, T2 arg2, T3 arg3);

    /// <summary>
    /// Delegate for handling events from the <see cref="Character"/>.
    /// </summary>
    /// <param name="character">The <see cref="Character"/> that the event came from.</param>
    public delegate void CharacterEventHandler(Character character);

    /// <summary>
    /// Delegate for handling events from the <see cref="Character"/>.
    /// </summary>
    /// <param name="character">The <see cref="Character"/> that the event came from.</param>
    /// <typeparam name="T1">The type of the first event argument.</typeparam>
    /// <param name="arg1">The first argument related to the event.</param>
    public delegate void CharacterEventHandler<T1>(Character character, T1 arg1);

    /// <summary>
    /// Delegate for handling events from the <see cref="Character"/>.
    /// </summary>
    /// <param name="character">The <see cref="Character"/> that the event came from.</param>
    /// <typeparam name="T1">The type of the first event argument.</typeparam>
    /// <typeparam name="T2">The type of the second event argument.</typeparam>
    /// <param name="arg1">The first argument related to the event.</param>
    /// <param name="arg2">The second argument related to the event.</param>
    public delegate void CharacterEventHandler<T1, T2>(Character character, T1 arg1, T2 arg2);

}