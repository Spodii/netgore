using System;
using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics.ParticleEngine;

namespace NetGore.Editor.WinForms
{
    /// <summary>
    /// A <see cref="ComboBox"/> containing the <see cref="ParticleEmitter"/>s.
    /// </summary>
    public class ParticleEmitterComboBox : ComboBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterComboBox"/> class.
        /// </summary>
        public ParticleEmitterComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        /// <summary>
        /// Notifies listeners when the selected <see cref="ParticleEmitter"/> has changed.
        /// </summary>
        public event TypedEventHandler<ParticleEmitterComboBox, EventArgs<Type>> SelectedEmitterChanged;

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.Control.CreateControl"/> method.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (DesignMode)
                return;

            Items.Clear();
            Items.AddRange(ParticleEmitter.EmitterTypes.Cast<object>().ToArray());
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ComboBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"/> that contains the event data.</param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (DesignMode || !ControlHelper.DrawListItem<Type>(Items, e, x => x.Name))
                base.OnDrawItem(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            var item = SelectedItem as Type;

            if (DesignMode)
                return;

            if (item != null)
            {
                if (SelectedEmitterChanged != null)
                    SelectedEmitterChanged.Raise(this, EventArgsHelper.Create(item));
            }
        }
    }
}