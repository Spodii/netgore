using System.Linq;

namespace DemoGame.Client
{
    /// <summary>
    /// Represents a Character that is controlled by the computer. This class should only be instantiated by the
    /// DyanmicEntityFactory.
    /// </summary>
    public class NPC : Character
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NPC"/> class.
        /// </summary>
        protected NPC()
        {
        }
    }
}