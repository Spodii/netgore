 // NOTE: Unused

using System;
using System.ComponentModel;
using System.Reflection;

namespace NetGore
{
    public class SimpleMemberDescriptor : PropertyDescriptor
    {
        public override Type ComponentType
        {
            get { return Member.DeclaringType; }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        protected MemberInfo Member { get; set; }

        public override Type PropertyType
        {
            get
            {
                FieldInfo fieldInfo;
                PropertyInfo propertyInfo;

                if ((fieldInfo = Member as FieldInfo) != null)
                    return fieldInfo.FieldType;
                else if ((propertyInfo = Member as PropertyInfo) != null)
                    return propertyInfo.PropertyType;
                else
                    return null;
            }
        }

        public SimpleMemberDescriptor(Type type, string memberName, string displayName, string description, string category)
            : base(
                memberName,
                new Attribute[]
                { new CategoryAttribute(category), new DisplayNameAttribute(displayName), new DescriptionAttribute(description) })
        {
            PropertyInfo prop = type.GetProperty(memberName);
            if (prop != null)
                Member = prop;
            else
            {
                FieldInfo field = type.GetField(memberName);
                if (field != null)
                    Member = field;
                else
                {
                    // TODO: Throw error that the property or field could not be found
                }
            }
        }

        public SimpleMemberDescriptor(MemberInfo member)
            : base(member.Name, (Attribute[])member.GetCustomAttributes(typeof(Attribute), true))
        {
            Member = member;
        }

        public SimpleMemberDescriptor(MemberInfo member, params Attribute[] attributes) : base(member.Name, attributes)
        {
            Member = member;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override bool Equals(object obj)
        {
            SimpleMemberDescriptor descriptor = obj as SimpleMemberDescriptor;
            return ((descriptor != null) && descriptor.Member.Equals(Member));
        }

        public override int GetHashCode()
        {
            return Member.GetHashCode();
        }

        public override object GetValue(object component)
        {
            FieldInfo fieldInfo;
            PropertyInfo propertyInfo;

            if ((fieldInfo = Member as FieldInfo) != null)
                return fieldInfo.GetValue(component);
            else if ((propertyInfo = Member as PropertyInfo) != null)
                return propertyInfo.GetValue(component, null);
            else
                return null;
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object component, object value)
        {
            FieldInfo fieldInfo;
            PropertyInfo propertyInfo;

            if ((fieldInfo = Member as FieldInfo) != null)
                fieldInfo.SetValue(component, value);
            else if ((propertyInfo = Member as PropertyInfo) != null)
                propertyInfo.SetValue(component, value, null);

            OnValueChanged(component, EventArgs.Empty);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }
    }
}