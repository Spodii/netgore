using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;

namespace DemoGame.Client
{
    public class TeleportEntity : TeleportEntityBase, IDrawableEntity
    {
        public override bool Use(CharacterEntity charEntity)
        {
            throw new MethodAccessException("The client may not actually an IUsableEntity, only send requests to use them.");
        }

        public override bool CanUse(CharacterEntity charEntity)
        {
            return true;
        }

        public override event EntityEventHandler<CharacterEntity> OnUse
        {
            add { }
            remove { }
        }

        public event MapRenderLayerChange OnChangeRenderLayer;
        public MapRenderLayer MapRenderLayer
        {
            get { return MapRenderLayer.Foreground; }
        }

        public void Draw(SpriteBatch sb)
        {
            var rect = CB.ToRectangle();
            XNARectangle.Draw(sb, rect, new Color(255, 255, 255, 100), Color.Black);
        }

        public bool InView(Camera2D camera)
        {
            return camera.InView(this);
        }
    }
}
