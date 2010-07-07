using System.Linq;
using NetGore.World;

namespace NetGore.AI
{
    /// <summary>
    /// Interface for a class that implements AI for a <see cref="DynamicEntity"/>.
    /// </summary>
    public interface IAI
    {
        /// <summary>
        /// Gets the <see cref="DynamicEntity"/> that this AI is for.
        /// </summary>
        DynamicEntity Actor { get; }

        /// <summary>
        /// Gets the ID of this AI.
        /// </summary>
        AIID ID { get; }

        /// <summary>
        /// Updates the AI. This is called at most once per frame, but does not require to be called every frame.
        /// </summary>
        void Update();
    }
}