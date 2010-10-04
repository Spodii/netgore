using System;
using System.ComponentModel;
using System.Linq;

namespace NetGore.EditorTools.Docking
{
    [AttributeUsage(AttributeTargets.All)]
    sealed class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        bool m_initialized = false;

        public LocalizedDescriptionAttribute(string key) : base(key)
        {
        }

        public override string Description
        {
            get
            {
                if (!m_initialized)
                {
                    var key = base.Description;
                    DescriptionValue = ResourceHelper.GetString(key);
                    if (DescriptionValue == null)
                        DescriptionValue = String.Empty;

                    m_initialized = true;
                }

                return DescriptionValue;
            }
        }
    }
}