using System.Linq;

namespace DemoGame.Client
{
    /// <summary>
    /// Delegate for handling events from the <see cref="Character"/>.
    /// </summary>
    /// <param name="sender">The <see cref="Character"/> the event came from.</param>
    public delegate void CharacterEventHandler(Character sender);
}