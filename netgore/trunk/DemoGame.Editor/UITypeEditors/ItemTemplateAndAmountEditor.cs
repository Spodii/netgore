using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms.Design;
using NetGore.Editor;

namespace DemoGame.Editor.UITypeEditors
{
    /// <summary>
    /// A <see cref="UITypeEditor"/> for selecting the <see cref="ItemTemplateAndAmountEditor"/>.
    /// </summary>
    public class ItemTemplateAndAmountEditor : UITypeEditor
    {
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
                var v = value as List<MutablePair<ItemTemplateID, byte>>;
                if (v == null)
                    return value;

                using (var editorForm = new ItemTemplateAndAmountListForm(v))
                {
                    svc.ShowDialog(editorForm);
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
    }
}