#if TOPDOWN
#pragma warning disable 1587
#endif

#if !TOPDOWN

using System;
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

        const int _UpdateRate = 3000;
        const int _id = 2;

        State _characterState = State.Patrol;
        bool _hasTarget;
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

        /// <summary>
        /// Called when a character attacks the Actor. It checks certain criteria before trying to
        /// attack the attacker.
        /// </summary>
        /// <param name="sender">The character that attacked the Actor.</param>
        /// <param name="e">The <see cref="CharacterAttackEventArgs"/> instance containing the event data.</param>
        void Actor_AttackedByCharacter(Character sender, CharacterAttackEventArgs e)
        {
            //We already had a target!
            if (_target != null && _target != sender)
            {
                //find hostile with lowest health.
                if (sender.HP >= _target.HP)
                {
                    //Don't change target so don't do anything.
                }
                else
                {
                    //Change to hostile with lower health value.
                    _target = sender;
                }
            }
            else
                _target = sender;

            //Set up event handler for when the _target dies.
            _target.Killed += _target_Killed;

            //Adds some logic where if the Actors HP is below an amount the Actor tries to run away.
            if (Actor.HP <= 10)
            {
                _hasTarget = false;
                _characterState = State.Evade;
                return;
            }

            //Sets the Actor to attack the _target.
            _hasTarget = true;
            _characterState = State.Attack;
        }

        /// <summary>
        /// Chases the target by checking where abouts the Target is in relation to the Actor.
        /// </summary>
        void ChaseTarget()
        {
            //Checks whether the _target is above the Actor.
            if (_target.Position.Y < Actor.Position.Y)
            {
                //_target above
                //Jump first (so we can move left and right while in the air)
                Actor.Jump();

                //Move right because _target is to the right.
                if (_target.Position.X > Actor.Position.X)
                {
                    Actor.SetHeading(Direction.West);
                    Actor.MoveRight();
                }

                //Move left becuase target is to the left.
                if (_target.Position.X < Actor.Position.X)
                {
                    Actor.SetHeading(Direction.East);
                    Actor.MoveLeft();
                }
            }

            if (_target.Position.Y > Actor.Position.Y)
            {
                //_target below
                if (_target.Position.X >= Actor.Position.X)
                {
                    Actor.SetHeading(Direction.West);
                    Actor.MoveRight();
                }

                if (_target.Position.X <= Actor.Position.X)
                {
                    Actor.SetHeading(Direction.East);
                    Actor.MoveLeft();
                }
            }

            var YDifference = Convert.ToInt32((_target.Position.Y - Actor.Position.Y));

            if ((YDifference <= 5) && (YDifference >= -5))
            {
                //target is level
                if (_target.Position.X >= Actor.Position.X)
                {
                    Actor.SetHeading(Direction.West);
                    Actor.MoveRight();
                }

                if (_target.Position.X <= Actor.Position.X)
                {
                    Actor.SetHeading(Direction.East);
                    Actor.MoveLeft();
                }
            }

            if (_lastUpdateTime + 5000 < GetTime())
            {
                if (Actor.Position.X == _lastX) //Only execut this after 5 seconds.
                {
                    if (Actor.IsMovingRight)
                    {
                        Actor.SetHeading(Direction.West);
                        Actor.Jump();
                        Actor.MoveLeft();
                    }
                    if (Actor.IsMovingLeft)
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
            if (_lastUpdateTime + _UpdateRate < time)
            {
                _lastUpdateTime = time;
                _lastX = Actor.Position.X;
                EvaluateState();
            }

            //Updates the Actor depending on its current state.
            UpdateState(_characterState);
        }

        public void EvadeTarget()
        {
            //Opposite of chase logic.

            if (_target.Position.Y >= Actor.Position.Y)
            {
                //_target above
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
                //_target below
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

            var YDifference = Convert.ToInt32((_target.Position.Y - Actor.Position.Y));

            if ((YDifference <= 5) && (YDifference >= -5))
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
                if (_lastUpdateTime + 5000 < GetTime()) //Only execut this after 5 seconds.
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
                if (Actor.HP <= 10)
                {
                    //Actor is low on health perhaps it should run away.
                    _characterState = State.Evade;

                    //Skip any other processing so that the state stays as Evade.
                    return;
                }

                //This is so that the _target has the opportunity to evade this Actor
                if (_target.GetDistance(Actor) > 60)
                    _hasTarget = false;

                if (_hasTarget)
                {
                    //We have a hostile target so lets Attack them.
                    _characterState = State.Attack;
                }
                else
                {
                    //There is no hostile target therefore just Patrol.
                    _hasTarget = false;
                    _target = null;
                    _characterState = State.Patrol;
                }
            }
            else
            {
                //Just patrol if there is no _target.
                _hasTarget = false;
                _characterState = State.Patrol;

                //clear _target
                _target = null;
            }
        }

        void Patrol()
        {
            //Move randomly.
            if (Rand(0, 40) == 0)
            {
                if (Actor.IsMoving)
                    Actor.StopMoving();
                else
                {
                    if ((Rand(0, 55) == 0))
                        Actor.Jump();

                    if (Rand(0, 2) == 0)
                        Actor.MoveLeft();
                    else
                        Actor.MoveRight();
                }
            }
        }

        /// <summary>
        /// Updates the Actor depending on its current state. Should only be called from DoUpdate().
        /// </summary>
        /// <param name="currentState">The currrent <see cref="State"/> of the actor.</param>
        void UpdateState(State currentState)
        {
            // If the AI has been disabled just set to Idle and ignore anything else
            if (AISettings.AIDisabled)
                currentState = State.Idle;

            switch (currentState)
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

        /// <summary>
        /// Called when the target is killed. It resets whether the Actor should be trying to attack the _target.
        /// </summary>
        /// <param name="sender">The character that was killed.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void _target_Killed(Character sender, EventArgs e)
        {
            // Stop attacking target. Job done.
            _hasTarget = false;
        }
    }
}

#endif