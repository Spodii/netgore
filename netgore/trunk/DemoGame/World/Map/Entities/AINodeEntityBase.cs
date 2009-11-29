using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame
{
    public abstract class AINodeEntityBase : DynamicEntity
    {
        Vector2 _closestAINode;

        protected AINodeEntityBase()
        {
            Weight = 1.0f;
        }

        [Browsable(false)]
        public Vector2 ClosestNode
        {
            get { return _closestAINode; }
            set { _closestAINode = GetClosestAINode(); }
        }

        Vector2 Size
        {
            set
            {
                Vector2 FixedSize;
                FixedSize.X = 32;
                FixedSize.Y = 32;
                Size = FixedSize;
            }
        }
    }
}
