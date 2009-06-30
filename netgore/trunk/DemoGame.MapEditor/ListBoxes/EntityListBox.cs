using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Client;

using NetGore;
using NetGore.EditorTools;

namespace DemoGame.MapEditor
{
    public class EntityListBox : MapItemListBox<Map, Entity>
    {
        protected override void Clone(Entity item)
        {
            // TODO: ...
            throw new NotImplementedException();
        }

        protected override void Delete(Entity item)
        {
            Map.RemoveEntity(item);
        }

        protected override IEnumerable<Entity> GetItems()
        {
            return Map.DynamicEntities.OfType<Entity>();
        }

        protected override void Locate(Entity item)
        {
            Camera.Center(item);
        }
    }
}