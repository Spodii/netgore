using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using NetGore.IO;

namespace NetGore.Graphics.Extensions
{
    public static class IValueReaderExtensions
    {
        public static SpriteBlendMode ReadSpriteBlendMode(this IValueReader reader, string name)
        {
            return reader.ReadEnum(SpriteBlendModeHelper.Instance, name);
        }
    }
}