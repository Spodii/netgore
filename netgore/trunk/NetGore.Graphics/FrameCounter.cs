using System;

namespace NetGore.Graphics
{
    /// <summary>
    /// Manages the frames per second count
    /// </summary>
    public class FrameCounter
    {
        /// <summary>
        /// Target elapsed time
        /// </summary>
        readonly TimeSpan _targetElapseTime = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Combination of the elapsed times (resets after hitting one second)
        /// </summary>
        TimeSpan _elapsedCounter = TimeSpan.Zero;

        /// <summary>
        /// Counts the FPS
        /// </summary>
        int _frameCounter = 0;

        /// <summary>
        /// Last compelted FPS value
        /// </summary>
        int _frameRate = 0;

        /// <summary>
        /// Gets the last FPS rate
        /// </summary>
        public int FrameRate
        {
            get { return _frameRate; }
        }

        /// <summary>
        /// Updates the frame counter and the tick count
        /// </summary>
        /// <param name="elapsedTime">The elapsed real time between the frames</param>
        public void Update(TimeSpan elapsedTime)
        {
            // Increases the frame count
            _frameCounter++;

            // Increase the elapsed amount of time
            _elapsedCounter += elapsedTime;

            // Check if the target time has elapsed
            if (_elapsedCounter > _targetElapseTime)
            {
                // Reduce the elapsed time, store the frame rate and reset the counter
                _elapsedCounter -= _targetElapseTime;
                _frameRate = _frameCounter;
                _frameCounter = 0;
            }
        }
    }
}