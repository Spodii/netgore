using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Microsoft.Xna.Framework.Design
{
    abstract class MemberPropertyDescriptor : PropertyDescriptor
    {
        readonly MemberInfo _member;

        public MemberPropertyDescriptor(MemberInfo member)
            : base(member.Name, (Attribute[])member.GetCustomAttributes(typeof(Attribute), true))
        {
            _member = member;
        }

        public override Type ComponentType
        {
            get { return _member.DeclaringType; }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override bool Equals(object obj)
        {
            MemberPropertyDescriptor descriptor = obj as MemberPropertyDescriptor;
            return ((descriptor != null) && descriptor._member.Equals(_member));
        }

        public override int GetHashCode()
        {
            return _member.GetHashCode();
        }

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }
    }
}