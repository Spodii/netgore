using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using log4net;
using NetGore.Features.NPCChat;

namespace NetGore.Editor.UI
{
    /// <summary>
    /// A <see cref="UITypeEditor"/> for selecting the <see cref="NPCChatDialogBase"/>.
    /// </summary>
    public class NPCChatDialogEditor : UITypeEditor
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
                using (var editorForm = new NPCChatDialogUITypeEditorForm(value))
                {
                    var pt = context.PropertyDescriptor.PropertyType;

                    if (svc.ShowDialog(editorForm) == DialogResult.OK)
                    {
                        if (pt == typeof(NPCChatDialogID) || pt == typeof(NPCChatDialogID?))
                            value = editorForm.SelectedItem.ID;
                        else if (pt == typeof(NPCChatDialogBase))
                            value = editorForm.SelectedItem;
                        else if (pt == typeof(string))
                            value = editorForm.SelectedItem.ID.ToString();
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
                        if (pt == typeof(NPCChatDialogID?))
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
    }
}