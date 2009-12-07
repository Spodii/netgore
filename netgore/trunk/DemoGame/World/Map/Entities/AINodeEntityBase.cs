using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame
{
    public abstract class AINodeEntityBase : DynamicEntity
    {
        /// <summary>
        /// When overridden in the derived class, gets if this <see cref="Entity"/> will collide against
        /// walls. If false, this <see cref="Entity"/> will pass through walls and completely ignore them.
        /// </summary>
        public override bool CollidesAgainstWalls
        {
            get { return false; }
        }

        Vector2 _closestAINode;

        protected AINodeEntityBase()
        {
            Weight = 1.0f;
        }

        [Browsable(false)]
        public Vector2 ClosestNode
        {
            get { return _closestAINode; }
            set { _closestAINode = value; }
        }
    }
}