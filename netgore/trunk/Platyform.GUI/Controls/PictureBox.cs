using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Platyform.Extensions;

namespace Platyform.Graphics.GUI
{
    public class PictureBox : SpriteControl
    {
        public PictureBox(GUIManagerBase gui, PictureBoxSettings settings, Vector2 position, ISprite sprite, Vector2 size,
                          Control parent) : base(gui, settings, position, sprite, size, parent)
        {
        }

        public PictureBox(Vector2 position, ISprite sprite, Vector2 size, Control parent)
            : base(parent.GUIManager, parent.GUIManager.PictureBoxSettings, position, sprite, size, parent)
        {
        }

        public PictureBox(PictureBoxSettings settings, Vector2 position, ISprite sprite, Vector2 size, Control parent)
            : base(parent.GUIManager, settings, position, sprite, size, parent)
        {
        }

        public PictureBox(Vector2 position, ISprite sprite, Control parent)
            : base(position, parent.GUIManager.PictureBoxSettings, sprite, parent)
        {
        }

        public PictureBox(PictureBoxSettings settings, Vector2 position, ISprite sprite, Control parent)
            : base(position, settings, sprite, parent)
        {
        }
    }
}