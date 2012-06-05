using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NetGore
{
    /// <summary>
    /// Extension methods for the <see cref="ITypeDescriptorContext"/>.
    /// </summary>
    public static class ITypeDescriptorContextExtensions
    {
        const int _stateFlagSelected = 2;

        /// <summary>
        /// Gets if the extended text for a <see cref="Type"/>'s instance when going through a
        /// <see cref="TypeConverter"/> should be shown.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>True if the extended text should be shown; otherwise false.</returns>
        public static bool ShowExtendedText(this ITypeDescriptorContext context)
        {
            var ao = TryGetPropertyGridAccessibleObject(context);
            if (ao == null)
                return false;

            var flags = TryGetAccessibleObjectState(ao);
            return (flags & _stateFlagSelected) == 0;
        }

        /// <summary>
        /// Gets the State value of an AccessibleObject.
        /// </summary>
        /// <param name="accessibleObject">The AccessibleObject.</param>
        /// <returns>
        /// The State value of the <paramref name="accessibleObject"/>, or 0 any errors occured when attempting to
        /// get the value.
        /// </returns>
        static int TryGetAccessibleObjectState(object accessibleObject)
        {
            const BindingFlags flags =
                BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Instance;

            if (accessibleObject == null)
                return 0;

            try
            {
                // Get the state enum value from the AccessibilityObject.State property
                var propState = accessibleObject.GetType().GetProperty("State", flags);
                if (propState == null)
                    return 0;

                var ret = propState.GetValue(accessibleObject, null);
                return (int)ret;
            }
            catch (InvalidCastException)
            {
                return 0;
            }
            catch (TargetException)
            {
                return 0;
            }
            catch (TargetInvocationException)
            {
                return 0;
            }
            catch (ArgumentNullException)
            {
                return 0;
            }
            catch (ArgumentException)
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the AccessibilityObject from a PropertyGrid.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The AccessibilityObject, or null if invalid or not of the correct type.</returns>
        static object TryGetPropertyGridAccessibleObject(ITypeDescriptorContext context)
        {
            const BindingFlags flags =
                BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Instance;

            if (context == null)
                return null;

            try
            {
                // Check for the PropertyDescriptorGridEntry type
                if (context.GetType().FullName == "System.Windows.Forms.PropertyGridInternal.PropertyDescriptorGridEntry")
                {
                    // Get the AccessibileObject from the AcccessibilityObject property
                    var propAccessibilityObject = context.GetType().GetProperty("AccessibilityObject", flags);
                    if (propAccessibilityObject == null)
                        return null;

                    var accessibleObject = propAccessibilityObject.GetValue(context, null);
                    return accessibleObject;
                }
            }
            catch (InvalidCastException)
            {
                return null;
            }
            catch (TargetException)
            {
                return null;
            }
            catch (TargetInvocationException)
            {
                return null;
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }

            return null;
        }
    }
}