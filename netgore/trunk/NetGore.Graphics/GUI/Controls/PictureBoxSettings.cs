using System.Linq;

namespace NetGore.Graphics.GUI
{
    public class PictureBoxSettings : SpriteControlSettings
    {
        static PictureBoxSettings _default = null;

        public PictureBoxSettings(ControlBorder border) : base(border)
        {
        }

        public static PictureBoxSettings Default
        {
            get
            {
                if (_default == null)
                    _default = new PictureBoxSettings(null);
                return _default;
            }
        }
    }
}