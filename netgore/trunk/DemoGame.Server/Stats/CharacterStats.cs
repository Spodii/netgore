using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server
{
    public abstract class CharacterStats : CharacterStatsBase
    {
        readonly Character _character;

        public Character Character { get { return _character; } }

        protected CharacterStats(Character character, StatCollectionType statCollectionType) : base(statCollectionType)
        {
            if (character == null)
                throw new ArgumentNullException("character");

            _character = character;
        }
    }
}
