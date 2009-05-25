using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SyncValueAttribute : Attribute
    {
        readonly string _customName;

        public string CustomName { get { return _customName; } }

        public SyncValueAttribute()
        {
        }

        public SyncValueAttribute(string customName)
        {
            _customName = customName;
        }
    }
}
