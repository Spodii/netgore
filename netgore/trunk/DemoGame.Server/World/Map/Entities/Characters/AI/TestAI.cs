using System.Linq;
using NetGore;
using NetGore.AI;

namespace DemoGame.Server
{
    [AI(_id)]
    public class TestAI : AIBase
    {
        const int _id = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAI"/> class.
        /// </summary>
        /// <param name="actor">Character for this AI module to control.</param>
        public TestAI(Character actor) : base(actor)
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets the ID of this AI.
        /// </summary>
        public override AIID ID
        {
            get { return new AIID(_id); }
        }

        /// <summary>
        /// Handles the real updating of the AI.
        /// </summary>
        protected override void DoUpdate()
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
            // Jump randomly
            if (Rand(0, 200) == 0)
                Actor.Jump();
        }
    }
}