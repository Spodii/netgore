using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// Manages the frames per second count.
    /// </summary>
    public class FrameCounter
    {
        /// <summary>
        /// Target elapsed time.
        /// </summary>
        const int _targetElapseTime = 1000;

        /// <summary>
        /// Counts the FPS.
        /// </summary>
        int _frameCounter = 60;

        /// <summary>
        /// Last completed FPS value.
        /// </summary>
        int _frameRate = 60;

        /// <summary>
        /// Target time (will reset FPS count after hitting this value).
        /// </summary>
        TickCount _targetTime = TickCount.MinValue;

        /// <summary>
        /// Gets the last FPS. This value is updated once per second.
        /// </summary>
        public int FrameRate
        {
            get { return _frameRate; }
        }

        /// <summary>
        /// Updates the frame counter and the tick count.
        /// </summary>
        /// <param name="gameTime">The current time in milliseconds.</param>
        public void Update(TickCount gameTime)
        {
            // Increases the frame count
            _frameCounter++;

            // Check if the target time has elapsed
            if (_targetTime <= gameTime)
            {
                // Reduce the elapsed time, store the frame rate and reset the counter
                _targetTime += _targetElapseTime;
                if (_targetTime <= gameTime)
                    _targetTime = gameTime + _targetElapseTime;

                _frameRate = _frameCounter;
                _frameCounter = 0;
            }
        }
    }
}