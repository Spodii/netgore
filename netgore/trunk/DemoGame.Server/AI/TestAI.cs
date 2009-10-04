using System.Linq;
using DemoGame;
using NetGore;
using NetGore.AI;

namespace DemoGame.Server
{
    [AI(0)]
    public class TestAI : AIBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestAI"/> class.
        /// </summary>
        /// <param name="actor">Character for this AI module to control.</param>
        public TestAI(Character actor) : base(actor)
        {
        }

        /// <summary>
        /// When overridden in the derived class, updates the AI. This is called at most once per frame, and only
        /// called whe the Actor is alive and active.
        /// </summary>
        public override void Update()
        {
            Character target = GetClosestHostile();

            if (target == null)
            {
                // Move around randomly
                if (Rand(0, 40) == 0)
                {
                    if (Actor.IsMoving)
                        Actor.StopMoving();
                    else
                    {
                        if (Rand(0, 2) == 0)
                            Actor.MoveLeft();
                        else
                            Actor.MoveRight();
                    }
                }
            }
            else
            {
                // Move towards an enemy
                if (target.Position.X > Actor.Position.X + 10)
                    Actor.MoveRight();
                else if (target.Position.X < Actor.Position.X - 10)
                    Actor.MoveLeft();
                else
                {
                    // Stop moving when close enough to the enemy
                    Actor.StopMoving();

                    // Face the correct direction
                    if (Actor.Position.X > target.Position.X)
                        Actor.SetHeading(Direction.West);
                    else
                        Actor.SetHeading(Direction.East);
                }

                // Attack if in range
                if (IsInMeleeRange(target))
                    Actor.Attack();
            }

            if (Rand(0, 200) == 0)
                Actor.Jump();
        }
    }
}