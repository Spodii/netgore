using System.Linq;

namespace DemoGame.Server
{
    public struct ActiveStatusEffectID
    {
        readonly int _value;

        public ActiveStatusEffectID(int value)
        {
            _value = value;
        }

        public static explicit operator int(ActiveStatusEffectID value)
        {
            return value._value;
        }

        public static explicit operator ActiveStatusEffectID(int value)
        {
            return new ActiveStatusEffectID(value);
        }
    }
}