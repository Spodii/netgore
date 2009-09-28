using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;

namespace DemoGame
{
    [Obsolete]
    public static class SkillTypeHelper
    {
        static readonly SkillType[] _allValues = EnumHelper<SkillType>.Values.ToArray();

        /// <summary>
        /// Gets an IEnumerable of all of the SkillTypes.
        /// </summary>
        public static IEnumerable<SkillType> AllValues
        {
            get { return _allValues; }
        }
    }
}