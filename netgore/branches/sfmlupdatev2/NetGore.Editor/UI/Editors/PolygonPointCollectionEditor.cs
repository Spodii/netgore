using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics.ParticleEngine;
using SFML.Graphics;
using Point = System.Drawing.Point;

namespace NetGore.Editor.UI
{
    /// <summary>
    /// A <see cref="CollectionEditor"/> for the <see cref="PolygonPointCollection"/>.
    /// </summary>
    public class PolygonPointCollectionEditor : CollectionEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonPointCollectionEditor"/> class.
        /// </summary>
        public PolygonPointCollectionEditor() : base(typeof(PolygonPointCollection))
        {
        }

        /// <summary>
        /// Creates a new form to display and edit the current collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.Design.CollectionEditor.CollectionForm"/> to provide as the user interface for editing the collection.
        /// </returns>
        protected override CollectionForm CreateCollectionForm()
        {
            return new PolygonPointCollectionForm(this);
        }

        class PolygonPointCollectionForm : CollectionForm
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PolygonPointCollectionForm"/> class.
            /// </summary>
            /// <param name="editor">The <see cref="T:System.ComponentModel.Design.CollectionEditor"/> to use for editing the collection.</param>
            public PolygonPointCollectionForm(CollectionEditor editor) : base(editor)
            {
                lblInstruction = new Label();
                txtEntry = new TextBox();
                btnOk = new Button();
                btnCancel = new Button();
                Editor = (PolygonPointCollectionEditor)editor;
                InitializeComponent();
            }

            PolygonPointCollectionEditor Editor { get; set; }
            Button btnCancel { get; set; }
            Button btnOk { get; set; }
            Label lblInstruction { get; set; }
            TextBox txtEntry { get; set; }

            /// <summary>
            /// Initializes the component.
            /// </summary>
            [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
                MessageId = "PolygonPointCollection")]
            void InitializeComponent()
            {
                lblInstruction.Location = new Point(4, 7);
                lblInstruction.Size = new Size(0x1a6, 14);
                lblInstruction.TabIndex = 0;
                lblInstruction.TabStop = false;
                lblInstruction.Text = "Enter each Vector2 coordinate on a new line, using a comma as a delimiter";
                txtEntry.Location = new Point(4, 0x16);
                txtEntry.Size = new Size(0x1a6, 0xf4);
                txtEntry.TabIndex = 0;
                txtEntry.Text = "";
                txtEntry.AcceptsTab = false;
                txtEntry.AcceptsReturn = true;
                txtEntry.AutoSize = false;
                txtEntry.Multiline = true;
                txtEntry.ScrollBars = ScrollBars.Both;
                txtEntry.WordWrap = false;
                txtEntry.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
                txtEntry.KeyDown += txtEntry_KeyDown;
                btnOk.Location = new Point(0xb9, 0x112);
                btnOk.Size = new Size(0x4b, 0x17);
                btnOk.TabIndex = 1;
                btnOk.Text = "OK";
                btnOk.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
                btnOk.Click += btnOK_Click;
                btnCancel.Location = new Point(0x108, 0x112);
                btnCancel.Size = new Size(0x4b, 0x17);
                btnCancel.TabIndex = 2;
                btnCancel.Text = "Cancel";
                btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Location = new Point(7, 7);
                Text = "PolygonPointCollection Editor";
                AcceptButton = btnOk;
                AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                AutoScaleDimensions = new SizeF(6f, 13f);
                CancelButton = btnCancel;
                ClientSize = new Size(0x1ad, 0x133);
                MaximizeBox = false;
                MinimizeBox = false;
                ControlBox = false;
                ShowInTaskbar = false;
                StartPosition = FormStartPosition.CenterScreen;
                MinimumSize = new Size(300, 200);
                Controls.Clear();
                Controls.AddRange(new Control[] { lblInstruction, txtEntry, btnOk, btnCancel });
            }

            /// <summary>
            /// Provides an opportunity to perform processing when a collection value has changed.
            /// </summary>
            protected override void OnEditValueChanged()
            {
                var items = Items;
                var str = string.Empty;

                var converter = TypeDescriptor.GetConverter(typeof(Vector2));

                for (var i = 0; i < items.Length; i++)
                {
                    if (items[i] is Vector2)
                    {
                        str = str + converter.ConvertToString((Vector2)items[i]);

                        if (i != (items.Length - 1))
                            str = str + "\r\n";
                    }
                }
                txtEntry.Text = str;
            }

            /// <summary>
            /// Handles the Click event of the btnOk control.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            void btnOK_Click(object sender, EventArgs e)
            {
                var separator = new char[] { '\n' };
                var trimChars = new char[] { '\r' };

                var strArray = txtEntry.Text.Split(separator);
                var items = Items;

                var length = strArray.Length;

                if ((strArray.Length > 0) && (strArray[strArray.Length - 1].Length == 0))
                    length--;

                var numArray = new Vector2[length];

                for (var i = 0; i < length; i++)
                {
                    strArray[i] = strArray[i].Trim(trimChars);

                    try
                    {
                        var converter = TypeDescriptor.GetConverter(typeof(Vector2));

                        numArray[i] = (Vector2)converter.ConvertFromInvariantString(strArray[i]);
                    }
                    catch (Exception exception)
                    {
                        base.DisplayError(exception);
                    }
                }

                var flag = true;

                if (length == items.Length)
                {
                    var index = 0;
                    while (index < length)
                    {
                        if (!numArray[index].Equals((Vector2)items[index]))
                            break;

                        index++;
                    }
                    if (index == length)
                        flag = false;
                }

                if (!flag)
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;
                else
                {
                    var objArray2 = new object[length];

                    for (var j = 0; j < length; j++)
                    {
                        objArray2[j] = numArray[j];
                    }

                    Items = objArray2;
                }
            }

            /// <summary>
            /// Handles the KeyDown event of the txtEntry control.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
            void txtEntry_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    btnCancel.PerformClick();

                    e.Handled = true;
                }
            }
        }
    }
}