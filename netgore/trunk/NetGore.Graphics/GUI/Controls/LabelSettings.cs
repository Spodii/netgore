using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    public class LabelSettings : TextControlSettings
    {
        static LabelSettings _default = null;

        public LabelSettings(ControlBorder border) : base(Color.Black, border)
        {
        }

        public static LabelSettings Default
        {
            get
            {
                if (_default == null)
                    _default = new LabelSettings(null);
                return _default;
            }
        }
    }
}