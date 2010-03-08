using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NetGore.AI
{
    public interface IPathFinder
    {

        List<Node> FindPath(Vector2 Start, Vector2 End);

        Heuristics HeuristicFormula
        {
            get;
            set;
        }
    }
}
