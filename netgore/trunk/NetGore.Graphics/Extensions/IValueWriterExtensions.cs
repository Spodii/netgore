using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using NetGore.IO;

namespace NetGore.Graphics
{
    public static class IValueWriterExtensions
    {
        public static void Write(this IValueWriter writer, string name, SpriteBlendMode spriteBlendMode)
        {
            writer.WriteEnum(SpriteBlendModeHelper.Instance, name, spriteBlendMode);
        }
    }
}
