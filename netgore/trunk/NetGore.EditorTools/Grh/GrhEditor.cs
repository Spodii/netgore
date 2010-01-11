using System;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using NetGore.Graphics;

namespace NetGore.EditorTools
{
    public class GrhEditor : UITypeEditor
    {
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            var grh = value as Grh;

            if (svc != null && grh != null)
            {
                using (var editorForm = new GrhUITypeEditorForm(grh))
                {
                    var originalGrhData = grh.GrhData;
                    if (svc.ShowDialog(editorForm) != DialogResult.OK)
                    {
                        // Revert to the original
                        grh.SetGrh(originalGrhData);
                    }
                }
            }

            return value;

        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}