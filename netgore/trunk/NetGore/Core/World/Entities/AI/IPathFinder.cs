using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;

namespace NetGore.AI
{
    public interface IPathFinder
    {
        // TODO: Documentation

        Heuristics HeuristicFormula { get; set; }

        IEnumerable<AINode> FindPath(Vector2 Start, Vector2 End);
    }
}