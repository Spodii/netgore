using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using XnaColor = Microsoft.Xna.Framework.Graphics.Color;

namespace NetGore.EditorTools
{
    /// <summary>
    /// <see cref="ColorEditor"/> for the Xna <see cref="Microsoft.Xna.Framework.Graphics.Color"/>.
    /// </summary>
    public class XnaColorEditor : ColorEditor
    {
        /// <summary>
        /// Edits the given object value using the editor style provided by the
        /// <see cref="M:System.Drawing.Design.ColorEditor.GetEditStyle(System.ComponentModel.ITypeDescriptorContext)"/>
        /// method.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/>
        /// that can be used to gain additional context information.</param>
        /// <param name="provider">An <see cref="T:System.IServiceProvider"/> through which editing
        /// services may be obtained.</param>
        /// <param name="value">An instance of the value being edited.</param>
        /// <returns>
        /// The new value of the object. If the value of the object has not changed, this should
        /// return the same object it was passed.
        /// </returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (value is Color)
            {
                Color c = (Color)value;
                return new XnaColor(c.R, c.G, c.B, c.A);
            }

            if (value is XnaColor)
            {
                XnaColor c = (XnaColor)value;
                return new XnaColor(c.R, c.G, c.B, c.A);
            }

            if (value is Color?)
            {
                Color? nc = (Color?)value;
                if (!nc.HasValue)
                {
                    if (context.PropertyDescriptor.PropertyType == typeof(XnaColor))
                        return new XnaColor();
                    else
                        return null;
                }
                else
                {
                    var c = nc.Value;
                    return new XnaColor(c.R, c.G, c.B, c.A);
                }
            }

            if (value is XnaColor?)
            {
                XnaColor? nc = (XnaColor?)value;
                if (!nc.HasValue)
                {
                    if (context.PropertyDescriptor.PropertyType == typeof(XnaColor))
                        return new XnaColor();
                    else
                        return null;
                }
                else
                {
                    var c = nc.Value;
                    return new XnaColor(c.R, c.G, c.B, c.A);
                }
            }

            return base.EditValue(context, provider, value);
        }

        /// <summary>
        /// Gets the editing style of the Edit method. If the method is not supported,
        /// this will return <see cref="F:System.Drawing.Design.UITypeEditorEditStyle.None"/>.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/>
        /// that can be used to gain additional context information.</param>
        /// <returns>
        /// An enum value indicating the provided editing style.
        /// </returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.None;
        }

        /// <summary>
        /// Paints a representative value of the given object to the provided canvas.
        /// </summary>
        /// <param name="e">What to paint and where to paint it.</param>
        public override void PaintValue(PaintValueEventArgs e)
        {
            if (!(e.Value is XnaColor))
                return;

            var c = (XnaColor)e.Value;
            var displayColor = Color.FromArgb(c.R, c.G, c.B);

            using (var brush = new SolidBrush(displayColor))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }
        }
    }
}