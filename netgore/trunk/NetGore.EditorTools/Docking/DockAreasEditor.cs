using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace NetGore.EditorTools.Docking
{
    class DockAreasEditor : UITypeEditor
    {
        DockAreasEditorControl m_ui = null;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value)
        {
            if (m_ui == null)
                m_ui = new DockAreasEditorControl();

            m_ui.SetStates((DockAreas)value);

            var edSvc = (IWindowsFormsEditorService)sp.GetService(typeof(IWindowsFormsEditorService));
            edSvc.DropDownControl(m_ui);

            return m_ui.DockAreas;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        class DockAreasEditorControl : UserControl
        {
            readonly CheckBox checkBoxDockBottom;
            readonly CheckBox checkBoxDockFill;
            readonly CheckBox checkBoxDockLeft;
            readonly CheckBox checkBoxDockRight;
            readonly CheckBox checkBoxDockTop;
            readonly CheckBox checkBoxFloat;
            DockAreas m_oldDockAreas;

            public DockAreasEditorControl()
            {
                checkBoxFloat = new CheckBox();
                checkBoxDockLeft = new CheckBox();
                checkBoxDockRight = new CheckBox();
                checkBoxDockTop = new CheckBox();
                checkBoxDockBottom = new CheckBox();
                checkBoxDockFill = new CheckBox();

                SuspendLayout();

                checkBoxFloat.Appearance = Appearance.Button;
                checkBoxFloat.Dock = DockStyle.Top;
                checkBoxFloat.Height = 24;
                checkBoxFloat.Text = Strings.DockAreaEditor_FloatCheckBoxText;
                checkBoxFloat.TextAlign = ContentAlignment.MiddleCenter;
                checkBoxFloat.FlatStyle = FlatStyle.System;

                checkBoxDockLeft.Appearance = Appearance.Button;
                checkBoxDockLeft.Dock = DockStyle.Left;
                checkBoxDockLeft.Width = 24;
                checkBoxDockLeft.FlatStyle = FlatStyle.System;

                checkBoxDockRight.Appearance = Appearance.Button;
                checkBoxDockRight.Dock = DockStyle.Right;
                checkBoxDockRight.Width = 24;
                checkBoxDockRight.FlatStyle = FlatStyle.System;

                checkBoxDockTop.Appearance = Appearance.Button;
                checkBoxDockTop.Dock = DockStyle.Top;
                checkBoxDockTop.Height = 24;
                checkBoxDockTop.FlatStyle = FlatStyle.System;

                checkBoxDockBottom.Appearance = Appearance.Button;
                checkBoxDockBottom.Dock = DockStyle.Bottom;
                checkBoxDockBottom.Height = 24;
                checkBoxDockBottom.FlatStyle = FlatStyle.System;

                checkBoxDockFill.Appearance = Appearance.Button;
                checkBoxDockFill.Dock = DockStyle.Fill;
                checkBoxDockFill.FlatStyle = FlatStyle.System;

                Controls.AddRange(new Control[]
                { checkBoxDockFill, checkBoxDockBottom, checkBoxDockTop, checkBoxDockRight, checkBoxDockLeft, checkBoxFloat });

                Size = new Size(160, 144);
                BackColor = SystemColors.Control;
                ResumeLayout();
            }

            public DockAreas DockAreas
            {
                get
                {
                    DockAreas dockAreas = 0;
                    if (checkBoxFloat.Checked)
                        dockAreas |= DockAreas.Float;
                    if (checkBoxDockLeft.Checked)
                        dockAreas |= DockAreas.DockLeft;
                    if (checkBoxDockRight.Checked)
                        dockAreas |= DockAreas.DockRight;
                    if (checkBoxDockTop.Checked)
                        dockAreas |= DockAreas.DockTop;
                    if (checkBoxDockBottom.Checked)
                        dockAreas |= DockAreas.DockBottom;
                    if (checkBoxDockFill.Checked)
                        dockAreas |= DockAreas.Document;

                    if (dockAreas == 0)
                        return m_oldDockAreas;
                    else
                        return dockAreas;
                }
            }

            public void SetStates(DockAreas dockAreas)
            {
                m_oldDockAreas = dockAreas;
                if ((dockAreas & DockAreas.DockLeft) != 0)
                    checkBoxDockLeft.Checked = true;
                if ((dockAreas & DockAreas.DockRight) != 0)
                    checkBoxDockRight.Checked = true;
                if ((dockAreas & DockAreas.DockTop) != 0)
                    checkBoxDockTop.Checked = true;
                if ((dockAreas & DockAreas.DockTop) != 0)
                    checkBoxDockTop.Checked = true;
                if ((dockAreas & DockAreas.DockBottom) != 0)
                    checkBoxDockBottom.Checked = true;
                if ((dockAreas & DockAreas.Document) != 0)
                    checkBoxDockFill.Checked = true;
                if ((dockAreas & DockAreas.Float) != 0)
                    checkBoxFloat.Checked = true;
            }
        }
    }
}