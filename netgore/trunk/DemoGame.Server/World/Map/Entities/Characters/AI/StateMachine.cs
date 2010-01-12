using System.Linq;
using NetGore;
using NetGore.AI;
using System.Diagnostics;

namespace DemoGame.Server
{
    //AIID 2 - Simple state machine algorithms that will make decisions beased on predefined logic.

    #region "Topdown AI Algotihm"
#if TOPDOWN         //Top down AI Algorithm
    [AI(_id)]
    class StateMachine : AIBase
    {

            public enum State
        {
            Evade,
            Patrol,
            Attack,
            Idle
        }

        const int _id = 2;
        const bool AIIDLE = true;
        const int _targetUpdateRate = 1000;
        int _lastTargetUpdateTime = int.MinValue;
            
            //TODO: Implement.
    }
#endif
    #endregion

    #region "Sidescroller AI Algorithm"
#if !TOPDOWN        //The Sidescroller AI Algorithm.            I will possibly expand logic to deal with Equipment :)
    [AI(_id)]
    class StateMachine : AIBase
    {
        public enum State
        {
            Evade,
            Patrol,
            Attack,
            Idle
        }

        const int _id = 2;
        const int _targetUpdateRate = 2000;
        int _lastTargetUpdateTime = int.MinValue;

        Character _target;
        State _characterState = State.Patrol;
        float _lastX;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachine"/> class.
        /// </summary>
        /// <param name="actor">Character for this AI module to control.</param>
        public StateMachine(Character actor) : base(actor)
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
            // Update the target
            int time = GetTime();
            if (_lastTargetUpdateTime + _targetUpdateRate < time)
            {
                _lastTargetUpdateTime = time;
                _target = GetClosestHostile();
                _lastX = Actor.Position.X;
                EvaluateState();
            }

            UpdateState(_characterState);
        }


        public void UpdateState(State CurrentState)
        {
            if (DemoGame.AI.AISettings.AIDisabled)
            { CurrentState = State.Idle; }
            switch (CurrentState)
            {
                case State.Idle:
                    if (Actor.IsMoving)
                    { Actor.StopMoving(); }
                    break;
                case State.Attack:
                    ChaseTarget();
                    break;
                case State.Evade:
                    EvadeTarget();
                    break;
                case State.Patrol:
                    Patrol();
                    break;
            }
        }

        public void ChaseTarget()
        {

           bool above = false;
           bool below = false;

            if (_target.Position.Y < Actor.Position.Y)
            {
                above = true;
                Actor.Jump();

                if (_target.Position.X > Actor.Position.X)
                {
                    Actor.SetHeading(Direction.West);
                    Actor.MoveRight();
                }
                if (_target.Position.X < Actor.Position.X)
                {
                    Actor.SetHeading(Direction.East);
                    Actor.MoveLeft();
                }
            }

            if (_target.Position.Y > Actor.Position.Y)
            {
                below = true;
                if (_target.Position.X > Actor.Position.X)
                {
                    Actor.SetHeading(Direction.West);
                    Actor.MoveRight();
                }

                if (_target.Position.X < Actor.Position.X)
                {
                    Actor.SetHeading(Direction.East);
                    Actor.MoveLeft();
                }
            }

            if (above & below)
            {
                //target is level
                if (_target.Position.X > Actor.Position.X)
                {
                    Actor.SetHeading(Direction.West);
                    Actor.MoveRight();
                }

                if (_target.Position.X < Actor.Position.X)
                {
                    Actor.SetHeading(Direction.East);
                    Actor.MoveLeft();
                }
            }

            if (Actor.Position.X == _lastX)
            {
                if (_lastTargetUpdateTime + 5000 < GetTime())       //Only execut this after 5 seconds.
                {
                    if (Actor.IsMovingRight)
                    {
                        Actor.SetHeading(Direction.West);
                        Actor.Jump();
                        Actor.MoveLeft();
                    }
                    if (Actor.IsMovingRight)
                    {
                        Actor.SetHeading(Direction.East);
                        Actor.Jump();
                        Actor.MoveRight();
                    }
                }
            }

            if (IsInMeleeRange(_target))
            {
                Actor.StopMoving();
                Actor.Attack();
            }
        }

        public void EvadeTarget()
        {
            //Opposite of attack.

            bool above = false;
            bool below = false;

            if (_target.Position.Y >= Actor.Position.Y)
            {
                above = true;
                Actor.Jump();

                if (_target.Position.X <= Actor.Position.X)
                {
                    Actor.SetHeading(Direction.West);
                    Actor.MoveRight();
                }
                if (_target.Position.X >= Actor.Position.X)
                {
                    Actor.SetHeading(Direction.East);
                    Actor.MoveLeft();
                }
            }

            if (_target.Position.Y <= Actor.Position.Y)
            {
                below = true;
                if (_target.Position.X <= Actor.Position.X)
                {
                    Actor.SetHeading(Direction.West);
                    Actor.MoveRight();
                }

                if (_target.Position.X > Actor.Position.X)
                {
                    Actor.SetHeading(Direction.East);
                    Actor.MoveLeft();
                }
            }

            if (above & below)
            {
                //target is level
                if (_target.Position.X < Actor.Position.X)
                {
                    Actor.SetHeading(Direction.West);
                    Actor.MoveRight();
                }

                if (_target.Position.X > Actor.Position.X)
                {
                    Actor.SetHeading(Direction.East);
                    Actor.MoveLeft();
                }
            }

            if (Actor.Position.X == _lastX + 20)
            {
                if (_lastTargetUpdateTime + 5000 < GetTime())       //Only execut this after 5 seconds.
                {
                    if (Actor.IsMovingRight)
                    {
                        Actor.SetHeading(Direction.West);
                        Actor.Jump();
                        Actor.MoveLeft();
                    }
                    if (Actor.IsMovingRight)
                    {
                        Actor.SetHeading(Direction.East);
                        Actor.Jump();
                        Actor.MoveRight();
                    }
                }
            }

            if (IsInMeleeRange(_target))
            {
                Actor.StopMoving();
                Actor.Attack();
            }
        }

        public void Patrol()
        {
            //Move randomly.
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

                if (Actor.Position.X == _lastX + 20)
                {
                    if (_lastTargetUpdateTime + 1000 < GetTime())       //Only execut this after 5 seconds.
                    {
                        if (Actor.IsMovingRight)
                        {
                            Actor.SetHeading(Direction.East);
                            Actor.Jump();
                            Actor.MoveLeft();
                        }
                        if (Actor.IsMovingRight)
                        {
                            Actor.SetHeading(Direction.West);
                            Actor.Jump();
                            Actor.MoveRight();
                        }
                    }
                }
            }
        }

        public void EvaluateState()
        {

                if (_target != null)
                {
                    if (Actor.HP < 20)
                    {
                        _characterState = State.Evade;
                        return;
                    }
                    else
                    {
                        _characterState = State.Attack;
                        
                        return;
                    }
                }
                else
                {
                   
                    _characterState = State.Patrol;
                    return;
                }
        }
            
    }
#endif 
    #endregion
}
   

