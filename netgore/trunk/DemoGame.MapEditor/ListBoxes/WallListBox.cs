using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoGame.Client;
using NetGore;
using NetGore.EditorTools;

namespace DemoGame.MapEditor
{
    public class WallListBox : MapItemListBox<Map, WallEntityBase>
    {
        protected override void Clone(WallEntityBase item)
        {
            // TODO: Clone WallEntity
            throw new NotImplementedException();
        }

        protected override void Delete(WallEntityBase item)
        {
            // TODO: Delete WallEntity
            throw new NotImplementedException();
        }

        protected override void Locate(WallEntityBase item)
        {
            // TODO: Locate WallEntity
            throw new NotImplementedException();
        }

        protected override IEnumerable<WallEntityBase> GetItems()
        {
            return Map.Entities.OfType<WallEntityBase>();
        }
    }
}
