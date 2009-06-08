using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoGame.Client;
using NetGore.EditorTools;
using NetGore.Graphics;

namespace DemoGame.MapEditor
{
    public class BackgroundItemListBox : MapItemListBox<Map, BackgroundImage>
    {
        protected override void Clone(BackgroundImage item)
        {
            // TODO: Clone BackgroundImage
            throw new NotImplementedException();
        }

        protected override void Delete(BackgroundImage item)
        {
            // TODO: Remove BackgroundImage
            throw new NotImplementedException();
        }

        protected override void Locate(BackgroundImage item)
        {
        }

        protected override IEnumerable<BackgroundImage> GetItems()
        {
            return Map.BackgroundImages;
        }
    }
}
