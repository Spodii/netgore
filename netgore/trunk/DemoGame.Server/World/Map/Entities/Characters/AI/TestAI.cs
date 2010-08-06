using System.Linq;
using DemoGame.AI;
using NetGore;
using NetGore.AI;

namespace DemoGame.Server
{
    [AI(_id)]
    public class TestAI : AIBase
    {
        const int _id = 1;

        /// <summary>
        /// How frequently to check for a new target.
        /// </summary>
        const int _targetUpdateRate = 2000;

        TickCount _lastTargetUpdateTime = TickCount.MinValue;
        Character _target;

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
            if (AISettings.AIDisabled)
                return;

            var time = GetTime();

            // Ensure the target is still valid, or enough time has elapsed to check for a better target
            if ((_target != null && !IsValidTarget(_target)) || (_lastTargetUpdateTime + _targetUpdateRate < time))
            {
                _lastTargetUpdateTime = time;
                _target = GetClosestHostile();
            }

            // Check if we have a target or not
            if (_target == null)
                UpdateNoTarget();
            else
                UpdateWithTarget();

#if !TOPDOWN
            // Jump randomly for no apparent reason
            if (Rand(0, 200) == 0)
                Actor.Jump();
#endif
        }

        void UpdateNoTarget()
        {
            // Move around randomly
#if !TOPDOWN
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
#elif TOPDOWN
            if (Rand(0, 40) == 0)
            {
                if (Actor.IsMoving)
                    Actor.StopMoving();
                else
                {
                    if (Rand(0, 2) == 0)
                    {
                        Actor.MoveUp();
                        return;
                    }
                    
                    if (Rand(0,2) == 0)
                    {
                        Actor.MoveDown();
                        return;
                    }

                    if (Rand(0, 2) == 0)
                    {
                        Actor.MoveLeft();
                        return;
                    }

                    if (Rand(0, 2) == 0)
                    {
                        Actor.MoveRight();
                        return;
                    }
                }
            }
#endif
        }

        void UpdateWithTarget()
        {
#if !TOPDOWN
            // Move towards an enemy
            if (_target.Position.X > Actor.Position.X + 10)
                Actor.MoveRight();
            else if (_target.Position.X < Actor.Position.X - 10)
                Actor.MoveLeft();
            else
            {
                // Stop moving when close enough to the enemy
                Actor.StopMoving();

                // Face the correct direction
                if (Actor.Position.X > _target.Position.X)
                    Actor.SetHeading(Direction.West);
                else
                    Actor.SetHeading(Direction.East);
            }
#elif TOPDOWN

    //Checks whether the _target is above the Actor.
            if (_target.Position.Y < Actor.Position.Y)
            {
                //_target above
                //Move upwards
                Actor.MoveUp();
                Actor.SetHeading(Direction.North);

                //Move right because _target is to the right.
                if (_target.Position.X > Actor.Position.X)
                {
                    Actor.MoveRight();
                    Actor.SetHeading(Direction.NorthEast);
                }

                //Move left becuase target is to the left.
                if (_target.Position.X < Actor.Position.X)
                {
                    Actor.MoveLeft();
                    Actor.SetHeading(Direction.NorthWest);
                }
            }

            if (_target.Position.Y > Actor.Position.Y)
            {
                //_target below
                //Move downwards

                Actor.MoveDown();
                Actor.SetHeading(Direction.South);

                if (_target.Position.X >= Actor.Position.X)
                {
                    Actor.MoveRight();
                    Actor.SetHeading(Direction.SouthEast);
                }

                if (_target.Position.X <= Actor.Position.X)
                {
                    Actor.MoveLeft();
                    Actor.SetHeading(Direction.SouthWest);
                }
            }


            if (_target.Position.Y == Actor.Position.Y)
            {
                //target is level
                if (_target.Position.X >= Actor.Position.X)
                {
                    Actor.MoveRight();
                    Actor.SetHeading(Direction.East);
                }

                if (_target.Position.X <= Actor.Position.X)
                {
                    Actor.MoveLeft();
                    Actor.SetHeading(Direction.West);
                }
            }

#endif

            //Instead of attacking EVERY loop attack occasionally to save resources.
            // Attack if in range
            if (Rand(0, 100) == 1)
            {
                if (IsInMeleeRange(_target))
                    Actor.Attack(_target);
            }
        }
    }
}