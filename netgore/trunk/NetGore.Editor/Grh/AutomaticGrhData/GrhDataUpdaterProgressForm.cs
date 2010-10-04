using System;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor.Grhs
{
    public partial class GrhDataUpdaterProgressForm : Form
    {
        /// <summary>
        /// The minimum amount of time that must elapse before <see cref="UpdateStatus"/> to actually update.
        /// </summary>
        const int _minSuggestUpdateRate = 500;

        /// <summary>
        /// The time the updating started.
        /// </summary>
        readonly TickCount _startTime;

        /// <summary>
        /// The total number of items.
        /// </summary>
        readonly int _total;

        /// <summary>
        /// The current item.
        /// </summary>
        int _current = 0;

        /// <summary>
        /// The time the status was last updated.
        /// </summary>
        TickCount _lastUpdateTime = TickCount.Now;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhDataUpdaterProgressForm"/> class.
        /// </summary>
        /// <param name="total">The total number of items.</param>
        public GrhDataUpdaterProgressForm(int total)
        {
            InitializeComponent();

            _total = total;
            _startTime = TickCount.Now;

            progBar.Minimum = 0;
            progBar.Maximum = total;
            progBar.Value = 0;
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="current">The current item count.</param>
        public void UpdateStatus(int current)
        {
            var currTime = TickCount.Now;
            var elapsed = currTime - _lastUpdateTime;

            if (elapsed < _minSuggestUpdateRate)
                return;

            _lastUpdateTime = currTime;

            if (current > _total)
                current = _total;

            _current = current;
            progBar.Value = _current;
            lblStatus.Text = string.Format("{0} of {1} ({2}%)", _current, _total, Math.Round(((float)_current / _total) * 100f));
            UpdateTimeRemaining();

            Application.DoEvents();
            Update();
        }

        /// <summary>
        /// Updates the amount of time remaining
        /// </summary>
        void UpdateTimeRemaining()
        {
            var currTime = TickCount.Now;
            var elapsed = currTime - _startTime;

            if (elapsed <= 0 || _current <= 1 || _total < 1)
                return;

            // Calculate how long it has taken so far per item
            var msPerItem = (float)elapsed / _current;

            // Use that average to calculate the total time for all remaining items (assuming the average remains constant)
            var estMSRem = (float)Math.Round(msPerItem * (_total - _current));

            // Convert to seconds
            var estSecRem = estMSRem / 1000f;

            string txt;

            // If over 100 seconds estimated remaining, use minutes
            if (estSecRem > 100)
            {
                // Display minutes
                var estMinRem = estSecRem / 60f;
                var disp = Math.Round(estMinRem);
                txt = disp + " minute" + (disp > 1 ? "s" : "");
            }
            else
            {
                // Display seconds
                var disp = Math.Round(estSecRem);
                txt = disp + " second" + (disp > 1 ? "s" : "");
            }

            lblTimeRemaining.Text = txt;
        }
    }
}