using System.Linq;
using DemoGame;
using NetGore;
using NetGore.RPGComponents;

namespace DemoGame
{
    public sealed class EquipmentSlotHelper : EnumHelper<EquipmentSlot>
    {
        static readonly EquipmentSlotHelper _instance;

        /// <summary>
        /// Gets the <see cref="EquipmentSlotHelper"/> instance.
        /// </summary>
        public static EquipmentSlotHelper Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Initializes the <see cref="EquipmentSlotHelper"/> class.
        /// </summary>
        static EquipmentSlotHelper()
        {
            _instance = new EquipmentSlotHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentSlotHelper"/> class.
        /// </summary>
        EquipmentSlotHelper()
        {
        }

        /// <summary>
        /// When overridden in the derived class, casts an int to type <see cref="EquipmentSlot"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The <paramref name="value"/> casted to type <see cref="EquipmentSlot"/>.</returns>
        protected override EquipmentSlot FromInt(int value)
        {
            return (EquipmentSlot)value;
        }

        /// <summary>
        /// When overridden in the derived class, casts type <see cref="EquipmentSlot"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> casted to an int.</returns>
        protected override int ToInt(EquipmentSlot value)
        {
            return (int)value;
        }
    }
}