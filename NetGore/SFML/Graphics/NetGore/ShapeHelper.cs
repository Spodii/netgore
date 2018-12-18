using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFML.Graphics
{
    internal class ShapeHelper
    {
        static readonly Sprite _amdShapeRenderFixSprite;

        static ShapeHelper()
        {
            _amdShapeRenderFixSprite = new Sprite(Texture.BlankPixel)
            {
                Position = new Vector2(-10000), // Waayyy off screen
            };
        }

        /// <summary>
        /// There is a bug with some AMD drivers that will cause the state to get corrupt after drawing an OpenGL shape. When drawing a shape, the next
        /// object rendered will have the transparency fucked up. Then it is all good. So what we do here is whenever we draw a shape, we call this afterwards
        /// to draw a 1x1 transparent sprite off-screen. This sprite will get messed up, but its off-screen, so we don't care.
        /// 
        /// Details: http://en.sfml-dev.org/forums/index.php?topic=9561
        /// </summary>
        public static void AMDShapeRenderFix(RenderTarget target)
        {
            _amdShapeRenderFixSprite.Draw(target, RenderStates.Default);
        }
    }
}
