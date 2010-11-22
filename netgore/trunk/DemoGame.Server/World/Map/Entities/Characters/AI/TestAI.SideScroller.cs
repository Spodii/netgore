#if TOPDOWN
#pragma warning disable 1587
#endif

#if !TOPDOWN

using System.Linq;
using NetGore;
using NetGore.AI;

namespace DemoGame.Server
{
    /// <summary>
    /// A very simple implementation of NPC AI.
    /// For SideScroller builds.
    /// </summary>
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

            // Jump randomly for no apparent reason
            if (Rand(0, 200) == 0)
                Actor.Jump();
        }

        void UpdateNoTarget()
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

        void UpdateWithTarget()
        {
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

            // Attack the target if they are in range
            if (Rand(0, 70) == 1)
            {
                if (IsInMeleeRange(_target))
                    Actor.Attack(_target);
            }
        }
    }
}

#endif