using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.AI
{
    public interface IPathFinder
    {
        Heuristics HeuristicFormula { get; set; }

        List<Node> FindPath(Vector2 Start, Vector2 End);
    }
}