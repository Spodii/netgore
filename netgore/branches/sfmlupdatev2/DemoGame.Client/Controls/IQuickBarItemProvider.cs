using System.Linq;

namespace DemoGame.Client
{
    /// <summary>
    /// Interface for an object that can add to the quick bar.
    /// </summary>
    public interface IQuickBarItemProvider
    {
        /// <summary>
        /// Gets the <see cref="QuickBarItemType"/> and value to add to the quick bar.
        /// </summary>
        /// <param name="type">When this method returns true, contains the <see cref="QuickBarItemType"/>
        /// to add.</param>
        /// <param name="value">When this method returns true, contains the value for for the quick bar item.</param>
        /// <returns>True if the item can be added to the quick bar; otherwise false.</returns>
        bool TryAddToQuickBar(out QuickBarItemType type, out int value);
    }
}