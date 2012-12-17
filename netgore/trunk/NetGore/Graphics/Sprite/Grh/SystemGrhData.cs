using System.Drawing;
using System.Linq;
using NetGore.Properties;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Contains <see cref="ISprite"/>s created by and used by the system. These are constructed completely
    /// internally, so there is no concerns about accidentally deleting <see cref="ISprite"/>s or their
    /// corresponding <see cref="Texture"/>.
    /// </summary>
    public static class SystemSprites
    {
        static readonly ISprite _blank;
        static readonly ISprite _joint;
        static readonly ISprite _lightbulb;
        static readonly ISprite _move;
        static readonly ISprite _resize;
        static readonly ISprite _triangle;

        /// <summary>
        /// Initializes the <see cref="SystemSprites"/> class.
        /// </summary>
        static SystemSprites()
        {
            _blank = CreateSprite(Resources.Blank);
            _joint = CreateSprite(Resources.Joint);
            _lightbulb = CreateSprite(Resources.Lightbulb);
            _move = CreateSprite(Resources.Move);
            _resize = CreateSprite(Resources.Resize);
            _triangle = CreateSprite(Resources.Triangle);
        }

        /// <summary>
        /// Gets the <see cref="ISprite"/> for a blank image.
        /// </summary>
        public static ISprite Blank
        {
            get { return _blank; }
        }

        /// <summary>
        /// Gets the <see cref="ISprite"/> for skeleton joint.
        /// </summary>
        public static ISprite Joint
        {
            get { return _joint; }
        }

        /// <summary>
        /// Gets the <see cref="ISprite"/> for a lightbulb.
        /// </summary>
        public static ISprite Lightblub
        {
            get { return _lightbulb; }
        }

        /// <summary>
        /// Gets the <see cref="ISprite"/> for an object move box.
        /// </summary>
        public static ISprite Move
        {
            get { return _move; }
        }

        /// <summary>
        /// Gets the <see cref="ISprite"/> for an object resize box.
        /// </summary>
        public static ISprite Resize
        {
            get { return _resize; }
        }

        /// <summary>
        /// Gets the <see cref="ISprite"/> for a triangle.
        /// </summary>
        public static ISprite Triangle
        {
            get { return _triangle; }
        }

        /// <summary>
        /// Creates a <see cref="Sprite"/> from a <see cref="System.Drawing.Image"/>.
        /// </summary>
        /// <param name="sysImg">The <see cref="System.Drawing.Image"/>.</param>
        /// <returns>The <see cref="Sprite"/> created from the <paramref name="sysImg"/>.</returns>
        static Sprite CreateSprite(System.Drawing.Image sysImg)
        {
            var img = sysImg.ToSFMLImage();
            img.CreateMaskFromColor(EngineSettings.TransparencyColor);

            var sprite = new Sprite(new Texture(img));
            return sprite;
        }
    }
}