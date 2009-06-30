using System;
using System.Collections.Generic;
using System.Diagnostics;
using DemoGame.Server;
using DemoGame;
using NetGore;

public class TestAI : AIBase
{
    public TestAI(Character actor)
        : base(actor)
    {
    }

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