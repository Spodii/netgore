using System;
using System.ComponentModel;
using System.Linq;

namespace NetGore.Editor.Docking
{
    [AttributeUsage(AttributeTargets.All)]
    sealed class LocalizedCategoryAttribute : CategoryAttribute
    {
        public LocalizedCategoryAttribute(string key) : base(key)
        {
        }

        protected override string GetLocalizedString(string key)
        {
            return ResourceHelper.GetString(key);
        }
    }
}