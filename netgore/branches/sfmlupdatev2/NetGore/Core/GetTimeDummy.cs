using System.Linq;

namespace NetGore
{
    /// <summary>
    /// A very straight-forward implementation of <see cref="IGetTime"/> that always returns <see cref="TickCount.Now"/>.
    /// It is intended that you try to avoid using this except for in situations where the time isn't actually
    /// important.
    /// </summary>
    public class GetTimeDummy : IGetTime
    {
        static readonly GetTimeDummy _instance;

        /// <summary>
        /// Initializes the <see cref="GetTimeDummy"/> class.
        /// </summary>
        static GetTimeDummy()
        {
            _instance = new GetTimeDummy();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTimeDummy"/> class.
        /// </summary>
        GetTimeDummy()
        {
        }

        /// <summary>
        /// Gets the <see cref="GetTimeDummy"/> instance.
        /// </summary>
        public static GetTimeDummy Instance
        {
            get { return _instance; }
        }

        #region IGetTime Members

        /// <summary>
        /// Gets the current time in milliseconds.
        /// </summary>
        /// <returns>The current time in milliseconds.</returns>
        public TickCount GetTime()
        {
            return TickCount.Now;
        }

        #endregion
    }
}