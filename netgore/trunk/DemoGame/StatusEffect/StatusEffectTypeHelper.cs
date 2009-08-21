using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore;

namespace DemoGame
{
    public static class StatusEffectTypeHelper
    {
        static readonly StatusEffectType[] _allValues = EnumHelper.GetValues<StatusEffectType>();

        /// <summary>
        /// Gets an IEnumerable of all of the StatusEffectTypes.
        /// </summary>
        public static IEnumerable<StatusEffectType> AllValues
        {
            get { return _allValues; }
        }
    }
}
