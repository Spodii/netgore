using System.Linq;

namespace NetGore.Graphics.GUI
{
    public class CheckBoxSettings : LabelSettings
    {
        static CheckBoxSettings _default = null;
        public readonly ISprite Ticked;
        public readonly ISprite TickedMouseOver;
        public readonly ISprite TickedPressed;
        public readonly ISprite Unticked;
        public readonly ISprite UntickedMouseOver;
        public readonly ISprite UntickedPressed;

        public CheckBoxSettings(ControlBorder border, ISprite ticked, ISprite tickedOver, ISprite tickedPressed, ISprite unticked,
                                ISprite untickedOver, ISprite untickedPressed) : base(border)
        {
            Ticked = ticked;
            TickedMouseOver = tickedOver;
            TickedPressed = tickedPressed;
            Unticked = unticked;
            UntickedMouseOver = untickedOver;
            UntickedPressed = untickedPressed;
        }

        public new static CheckBoxSettings Default
        {
            get
            {
                if (_default == null)
                    _default = new CheckBoxSettings(null, null, null, null, null, null, null);
                return _default;
            }
        }
    }
}