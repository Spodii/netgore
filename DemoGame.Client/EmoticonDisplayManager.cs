using System.Linq;
using NetGore.Features.Emoticons;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Handles displaying the individual emoticon instances.
    /// </summary>
    public class EmoticonDisplayManager : EmoticonDisplayManager<Emoticon, EmoticonInfo<Emoticon>>
    {
        static readonly EmoticonDisplayManager _instance;

        /// <summary>
        /// Initializes the <see cref="EmoticonDisplayManager"/> class.
        /// </summary>
        static EmoticonDisplayManager()
        {
            _instance = new EmoticonDisplayManager(EmoticonInfoManager.Instance);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmoticonDisplayManager{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="emoticonInfoManager">The <see cref="EmoticonInfoManagerBase{TKey, TValue}"/> to use to look up the information
        /// for emoticons.</param>
        public EmoticonDisplayManager(EmoticonInfoManagerBase<Emoticon, EmoticonInfo<Emoticon>> emoticonInfoManager)
            : base(emoticonInfoManager)
        {
            GetDrawPositionHandler = EmoticonDrawPositionHandler;
        }

        /// <summary>
        /// Gets the <see cref="EmoticonDisplayManager"/> instance.
        /// </summary>
        public static EmoticonDisplayManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// The default handler for finding the position to draw an emoticon.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to get the draw position for.</param>
        /// <returns>The world position to draw the emoticon for the given <paramref name="spatial"/>.</returns>
        static Vector2 EmoticonDrawPositionHandler(ISpatial spatial)
        {
            Vector2 pos;

            // Get the draw offset
            Character character;
            if ((character = spatial as Character) != null)
                pos = character.DrawPosition;
            else
                pos = spatial.Position;

            // Move the emoticon a bit up and to the right
            pos += new Vector2(5, -24);

            return pos;
        }
    }
}