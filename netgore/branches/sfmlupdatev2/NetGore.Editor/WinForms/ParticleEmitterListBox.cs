using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics.ParticleEngine;

namespace NetGore.Editor
{
    public class ParticleEmitterListBox : ListBox
    {
        const string _displayTextFormat = "[{0}] {1}";

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterListBox"/> class.
        /// </summary>
        public ParticleEmitterListBox()
        {
            if (DesignMode)
                return;

            DrawMode = DrawMode.OwnerDrawFixed;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"/> that contains the event data.</param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (DesignMode)
            {
                base.OnDrawItem(e);
                return;
            }

            // Get the selected emitter to draw
            var emitter = (e.Index < 0 || e.Index >= Items.Count ? null : Items[e.Index]) as IParticleEmitter;

            // Let the base implementation handle an invalid item
            if (emitter == null)
            {
                base.OnDrawItem(e);
                return;
            }

            // Draw the item
            e.DrawBackground();

            var txt = string.Format(_displayTextFormat, emitter.GetType().Name, emitter.Name);
            using (var brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(txt, e.Font, brush, e.Bounds);
            }

            e.DrawFocusRectangle();
        }

        public void RebuildList(IParticleEffect pe)
        {
            try
            {
                BeginUpdate();

                var selected = SelectedItem;
                Items.Clear();
                Items.AddRange(pe.Emitters.ToArray<object>());

                SelectedItem = selected;
            }
            finally
            {
                EndUpdate();
            }
        }
    }
}