namespace NetGore.Graphics.GUI
{
    public abstract class ControlSettings
    {
        ControlBorder _border;
        bool _canDrag = true;
        bool _canFocus = true;
        bool _isBoundToParentArea = true;
        bool _isEnabled = true;
        bool _isVisible = true;

        public ControlBorder Border
        {
            get { return _border; }
            set { _border = value; }
        }

        public bool CanDrag
        {
            get { return _canDrag; }
            set { _canDrag = value; }
        }

        public bool CanFocus
        {
            get { return _canFocus; }
            set { _canFocus = value; }
        }

        public bool IsBoundToParentArea
        {
            get { return _isBoundToParentArea; }
            set { _isBoundToParentArea = value; }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

        protected ControlSettings(ControlBorder border)
        {
            _border = border;
        }
    }
}