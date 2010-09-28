    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
    using SFML.Graphics;

namespace DemoGame.Server
{
    /// <summary>
    /// Represents an <see cref="NPC"/> which was summoned
    /// </summary>
    public class ThralledNPC : NPC
    {
        public ThralledNPC(World parent, CharacterTemplate template, Map map, Vector2 position) 
            : base(parent, template, map, position)
        {
            //This NPC should never respawn. Once it's dead, that should be it!
            this.RespawnMapID = null;
            this.RespawnPosition = Vector2.Zero;
        }

    }
}
