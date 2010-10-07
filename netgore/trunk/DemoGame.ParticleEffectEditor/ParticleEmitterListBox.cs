using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using NetGore.Editor.WinForms;
using NetGore.Graphics.ParticleEngine;

namespace DemoGame.ParticleEffectEditor
{
    public class ParticleEmitterListBox : TypedListBox<IParticleEmitter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterListBox"/> class.
        /// </summary>
        public ParticleEmitterListBox()
        {
            DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        }

        const string _displayTextFormat = "[{0}] {1}";

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"/> that contains the event data.</param>
        protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
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
    }
}
