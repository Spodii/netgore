using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;

namespace DemoGame.Client
{
    public class TeleportEntity : TeleportEntityBase, IDrawableEntity
    {
        public override event EntityEventHandler<CharacterEntity> OnUse
        {
            add { }
            remove { }
        }

        public override bool CanUse(CharacterEntity charEntity)
        {
            return true;
        }

        public override bool Use(CharacterEntity charEntity)
        {
            throw new MethodAccessException("The client may not actually an IUsableEntity, only send requests to use them.");
        }

        #region IDrawableEntity Members

        public event MapRenderLayerChange OnChangeRenderLayer;

        public MapRenderLayer MapRenderLayer
        {
            get { return MapRenderLayer.Foreground; }
        }

        public void Draw(SpriteBatch sb)
        {
            Rectangle rect = CB.ToRectangle();
            XNARectangle.Draw(sb, rect, new Color(255, 255, 255, 100), Color.Black);
        }

        public bool InView(Camera2D camera)
        {
            return camera.InView(this);
        }

        #endregion
    }
}