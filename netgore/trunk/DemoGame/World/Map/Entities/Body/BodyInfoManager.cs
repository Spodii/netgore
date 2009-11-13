using System.Linq;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    public sealed class BodyInfoManager
    {
        static readonly BodyInfoManager _instance;

        /// <summary>
        /// Array of all the body information
        /// </summary>
        readonly BodyInfo[] _bodyInfo;

        /// <summary>
        /// Initializes the <see cref="BodyInfoManager"/> class.
        /// </summary>
        static BodyInfoManager()
        {
            _instance = new BodyInfoManager();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BodyInfoManager"/> class.
        /// </summary>
        BodyInfoManager()
        {
            PathString path = ContentPaths.Build.Data.Join("bodies.xml");
            _bodyInfo = BodyInfo.Load(path);
        }

        public static BodyInfoManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the <see cref="BodyInfo"/> at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the body to get.</param>
        /// <returns>The <see cref="BodyInfo"/> at the given <paramref name="index"/>, or null if the
        /// <paramref name="index"/> was invalid or no body exists for the given value.</returns>
        public BodyInfo GetBody(BodyIndex index)
        {
            if (index < 0 || index >= _bodyInfo.Length)
                return null;

            return _bodyInfo[(int)index];
        }
    }
}