using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using log4net;
using NetGore.Editor.Grhs;
using NetGore.Graphics;

namespace NetGore.Editor.UI
{
    /// <summary>
    /// A <see cref="UITypeEditor"/> for selecting the <see cref="GrhData"/> to use on a <see cref="Grh"/>.
    /// </summary>
    public class GrhEditor : UITypeEditor
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Edits the specified object's value using the editor style indicated by the
        /// <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"/> method.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that can be
        /// used to gain additional context information.</param>
        /// <param name="provider">An <see cref="T:System.IServiceProvider"/> that this editor can use to
        /// obtain services.</param>
        /// <param name="value">The object to edit.</param>
        /// <returns>
        /// The new value of the object. If the value of the object has not changed, this should return the
        /// same object it was passed.
        /// </returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

            if (svc != null)
            {
                using (var editorForm = new GrhUITypeEditorForm(value))
                {
                    var pt = context.PropertyDescriptor.PropertyType;

                    if (svc.ShowDialog(editorForm) == DialogResult.OK)
                    {
                        var sel = editorForm.SelectedValue;

                        // Handle setting the value based on the type we are working with
                        if (pt == typeof(GrhIndex) || pt == typeof(GrhIndex?))
                            value = sel;
                        else if (pt == typeof(Grh))
                        {
                            var asGrh = value as Grh;
                            if (asGrh != null)
                                asGrh.SetGrh(sel);
                            else
                                value = new Grh(sel, AnimType.Loop, 0);
                        }
                        else if (pt == typeof(GrhData))
                            value = GrhInfo.GetData(sel);
                        else
                        {
                            const string errmsg =
                                "Don't know how to handle the source property type `{0}`. In value: {1}. Editor type: {2}";
                            if (log.IsErrorEnabled)
                                log.ErrorFormat(errmsg, pt, value, editorForm.GetType());
                            Debug.Fail(string.Format(errmsg, pt, value, editorForm.GetType()));
                        }
                    }
                    else
                    {
                        if (pt == typeof(GrhIndex?))
                            value = null;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Gets the editor style used by the
        /// <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"/> method.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that can be used
        /// to gain additional context information.</param>
        /// <returns>
        /// A <see cref="T:System.Drawing.Design.UITypeEditorEditStyle"/> value that indicates the style of editor
        /// used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"/>
        /// method. If the <see cref="T:System.Drawing.Design.UITypeEditor"/> does not support this method,
        /// then <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"/> will return
        /// <see cref="F:System.Drawing.Design.UITypeEditorEditStyle.None"/>.
        /// </returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// Indicates whether the specified context supports painting a representation of an object's value within the
        /// specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that can be used to gain
        /// additional context information.</param>
        /// <returns>
        /// true if <see cref="M:System.Drawing.Design.UITypeEditor.PaintValue(System.Object,System.Drawing.Graphics,System.Drawing.Rectangle)"/>
        /// is implemented; otherwise, false.
        /// </returns>
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Paints a representation of the value of an object using the specified
        /// <see cref="T:System.Drawing.Design.PaintValueEventArgs"/>.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Drawing.Design.PaintValueEventArgs"/> that indicates what to paint and
        /// where to paint it.</param>
        public override void PaintValue(PaintValueEventArgs e)
        {
            var image = GrhImageList.Instance.TryGetImage(e.Value);
            if (image != null)
                e.Graphics.DrawImage(image, e.Bounds);

            base.PaintValue(e);
        }
    }
}