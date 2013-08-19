using System.Linq;

#if !TOPDOWN
#pragma warning disable 1587
#endif

#if TOPDOWN

using NetGore;
using NetGore.AI;

namespace DemoGame.Server
{
    /// <summary>
    /// A very simple implementation of NPC AI.
    /// For TopDown builds.
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
                    int moveDirection = Rand(0, 4);

                    switch (moveDirection)
                    {
                        case 0:
                            Actor.MoveUp();
                            return;
                        case 1:
                            Actor.MoveDown();
                            return;
                        case 2:
                            Actor.MoveLeft();
                            return;
                        case 3:
                            Actor.MoveRight();
                            return;
                    }
                }
            }
        }

        void UpdateWithTarget()
        {
            // Move towards the target until in range
            // Horizontal
            if (Actor.Center.X > _target.Center.X + 20)
                Actor.MoveLeft();
            else if (Actor.Center.X < _target.Center.X - 20)
                Actor.MoveRight();
            else
                Actor.StopMovingHorizontal();

            // Vertical
            if (Actor.Center.Y > _target.Center.Y + 20)
                Actor.MoveUp();
            else if (Actor.Center.Y < _target.Center.Y - 20)
                Actor.MoveDown();
            else
                Actor.StopMovingVertical();

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