using System.Linq;

#if !TOPDOWN
#pragma warning disable 1587
#endif

#if TOPDOWN

using System.Linq;
using DemoGame.Server.AI;
using NetGore;
using NetGore.AI;
using NetGore.World;

namespace DemoGame.Server
{
    [AI(_id)]
    class StateMachine : AIBase
    {
        public enum State : byte
        {
            Evade,
            Patrol,
            Attack,
            Idle
        }

        const int _id = 2;
        const int _updateRate = 3000;

        State _characterState = State.Patrol;
        bool _isAttackingTarget;
        TickCount _lastUpdateTime = TickCount.MinValue;
        float _lastX;
        Character _target;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachine"/> class.
        /// </summary>
        /// <param name="actor">Character for this AI module to control.</param>
        public StateMachine(Character actor) : base(actor)
        {
            //Adds an event handler so we know when the actor has been attacked.
            Actor.AttackedByCharacter += Actor_AttackedByCharacter;
        }

        /// <summary>
        /// When overridden in the derived class, gets the ID of this AI.
        /// </summary>
        public override AIID ID
        {
            get { return new AIID(_id); }
        }

        void Actor_AttackedByCharacter(Character attacker, Character attacked, int damage)
        {
            //Set the _target as the attacker.
            _target = attacker;

            //Set up event handler for when the target dies.
            _target.Killed += _target_Killed;

            if (Actor.HP <= 10)
            {
                _isAttackingTarget = false;
                _characterState = State.Evade;
                return;
            }

            //Sets the Actor to attack the _target.
            _isAttackingTarget = true;
            _characterState = State.Attack;
        }

        /// <summary>
        /// Chases the target by checking where abouts the _target is in relation to the Actor.
        /// </summary>
        public void ChaseTarget()
        {
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

            if (Actor.Position.X == _lastX)
            {
                if (_lastUpdateTime + 5000 < GetTime()) //Only execute this after 5 seconds.
                {
                    if (Actor.IsMovingRight)
                        Actor.MoveLeft();
                    if (Actor.IsMovingLeft)
                        Actor.MoveRight();
                }
            }

            if (IsInMeleeRange(_target))
            {
                Actor.StopMoving();
                Actor.Attack(_target);
            }
        }

        /// <summary>
        /// Handles the real updating of the AI.
        /// </summary>
        protected override void DoUpdate()
        {
            //Updates a few variables that don't need to be updated every frame
            //and calls the EvaluateState method
            var time = GetTime();
            if (_lastUpdateTime + _updateRate < time)
            {
                _lastUpdateTime = time;
                _lastX = Actor.Position.X;
                EvaluateState();
            }

            //Updates the Actor depending on its current state.
            UpdateState(_characterState);
        }

        void EvadeTarget()
        {
            //Checks whether the _target is above the Actor.
            if (_target.Position.Y > Actor.Position.Y)
            {
                //_target below
                //Move upwards
                Actor.MoveUp();

                //Move lright because _target is to the left.
                if (_target.Position.X < Actor.Position.X)
                    Actor.MoveRight();

                //Move left becuase target is to the right.
                if (_target.Position.X > Actor.Position.X)
                    Actor.MoveLeft();
            }

            if (_target.Position.Y < Actor.Position.Y)
            {
                //_target above
                //Move downwards

                Actor.MoveDown();

                if (_target.Position.X <= Actor.Position.X)
                    Actor.MoveRight();

                if (_target.Position.X >= Actor.Position.X)
                    Actor.MoveLeft();
            }

            if (_target.Position.Y == Actor.Position.Y)
            {
                //target is level
                if (_target.Position.X <= Actor.Position.X)
                    Actor.MoveRight();

                if (_target.Position.X >= Actor.Position.X)
                    Actor.MoveLeft();
            }

            if (Actor.Position.X == _lastX)
            {
                if (_lastUpdateTime + 5000 < GetTime()) //Only execute this after 5 seconds.
                {
                    if (Actor.IsMovingRight)
                        Actor.MoveLeft();
                    if (Actor.IsMovingLeft)
                        Actor.MoveRight();
                }
            }

            if (IsInMeleeRange(_target))
            {
                Actor.StopMoving();
                Actor.Attack(_target);
            }
        }

        /// <summary>
        /// Uses a criteria to change the current state of the Actor. 
        /// This is where the main logic for this class is held in terms of how the AI responds.
        /// </summary>
        void EvaluateState()
        {
            if (_target != null)
            {
                //This is so that the Character has the opportunity to evade this Actor
                if (_target.GetDistance(Actor) > 50)
                    _isAttackingTarget = false;

                if (_isAttackingTarget)
                {
                    //We have a hostile target so lets Attack them.
                    _characterState = State.Attack;
                }
                else
                {
                    //There is no hostile target therefore just Patrol.
                    _target = null;
                    _isAttackingTarget = false;
                    _characterState = State.Patrol;
                }
            }
            else
            {
                //Just patrol if there is no _target.
                _isAttackingTarget = false;
                _characterState = State.Patrol;
            }
        }

        public void Patrol()
        {
            //Move randomly.

            //Force target to null.
            _target = null;
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

                    if (Rand(0, 2) == 0)
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
        }

        /// <summary>
        /// Updates the Actor depending on its current state. Should only be called from DoUpdate().
        /// </summary>
        /// <param name="CurrentState">The CurrentState of the actor.</param>
        void UpdateState(State CurrentState)
        {
            //If the AI has been disabled just set to Idle and ignore anything else.
            if (AISettings.AIDisabled)
                CurrentState = State.Idle;

            switch (CurrentState)
            {
                case State.Idle:
                    if (Actor.IsMoving)
                        Actor.StopMoving();
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

        void _target_Killed(Character character)
        {
            //Stop attacking target. Job done.
            _isAttackingTarget = false;
        }
    }
}

#endif