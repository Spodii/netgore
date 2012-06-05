using System.Linq;

#if !TOPDOWN

namespace DemoGame
{
    /// <summary>
    /// State of the character
    /// </summary>
    public enum CharacterState : byte
    {
        /// <summary>
        /// Standing still on the ground
        /// </summary>
        Idle,
        /// <summary>
        /// Moving to the left
        /// </summary>
        WalkingLeft,
        /// <summary>
        /// Moving to the right
        /// </summary>
        WalkingRight,
        /// <summary>
        /// Jumping (going up)
        /// </summary>
        Jumping,
        /// <summary>
        /// Jumping (going up) and moving to the left
        /// </summary>
        JumpingLeft,
        /// <summary>
        /// Jumping (going up) and moving to the right
        /// </summary>
        JumpingRight,
        /// <summary>
        /// Falling (going down)
        /// </summary>
        Falling,
        /// <summary>
        /// Falling (going down) and moving to the left
        /// </summary>
        FallingLeft,
        /// <summary>
        /// Falling (going down) and moving to the right
        /// </summary>
        FallingRight
    }
}

#endif