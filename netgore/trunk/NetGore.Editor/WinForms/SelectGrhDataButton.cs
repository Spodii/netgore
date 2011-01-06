using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using NetGore;
using NetGore.Editor.UI;
using NetGore.Graphics;

namespace DemoGame.Editor
{
    /// <summary>
    /// A <see cref="Button"/> that is used to select a <see cref="GrhData"/>.
    /// </summary>
    public class SelectGrhDataButton : Button
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectGrhDataButton"/> class.
        /// </summary>
        public SelectGrhDataButton()
        {
            Text = "...";
            Size = new Size(24, 23);
        }

        /// <summary>
        /// Notifies listeners when a <see cref="GrhData"/> is selected in this control. A null <see cref="GrhData"/> reference
        /// will be used when no item was selected, but the selection was not canceled.
        /// </summary>
        public event EventHandler<EventArgs<GrhData>> GrhDataSelected;

        /// <summary>
        /// Gets or sets the <see cref="GetCurrentlySelectedGrhDataCallback"/> used to determine what <see cref="GrhData"/>
        /// is currently selected. Can be null.
        /// </summary>
        public GetCurrentlySelectedGrhDataCallback SelectedGrhDataHandler { get; set; }

        /// <summary>
        /// Gets or sets the height and width of the control.
        /// </summary>
        /// <returns>The <see cref="T:System.Drawing.Size"/> that represents the height and width of the control in pixels.</returns>
        [DefaultValue(typeof(Point), "24, 23")]
        public new Size Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }

        /// <summary>
        /// Gets or sets the text associated with this control.
        /// </summary>
        /// <returns>The text associated with this control.</returns>
        [DefaultValue("...")]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        /// Gets the currently selected <see cref="GrhData"/> using the <see cref="SelectedGrhDataHandler"/>.
        /// </summary>
        /// <returns>The currently selected <see cref="GrhData"/>.</returns>
        GrhData GetCurrentlySelectedGrhData()
        {
            GrhData selected = null;
            try
            {
                if (SelectedGrhDataHandler != null)
                    selected = SelectedGrhDataHandler(this);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to get selected GrhData for `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, ex);
                Debug.Fail(string.Format(errmsg, this, ex));

                selected = null;
            }

            return selected;
        }

        /// <summary>
        /// Handles the <see cref="Button.Click"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (DesignMode)
                return;

            var currSelected = GetCurrentlySelectedGrhData();

            using (var frm = new GrhUITypeEditorForm(currSelected))
            {
                var result = frm.ShowDialog(this);
                if (result == DialogResult.Abort || result == DialogResult.Cancel || result == DialogResult.Ignore)
                    return;

                var newSelectedIndex = frm.SelectedValue;
                var newSelected = GrhInfo.GetData(newSelectedIndex);

                if (GrhDataSelected != null)
                    GrhDataSelected.Raise(this, EventArgsHelper.Create(newSelected));
            }
        }
    }
}