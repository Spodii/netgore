using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// Contains the settings specific to the <see cref="ToolBarControlType.ComboBox"/>.
    /// </summary>
    public interface IToolBarComboBoxSettings : IToolBarControlSettings
    {
        /// <summary>
        /// Occurs when the drop-down portion of a <see cref="System.Windows.Forms.ToolStripComboBox"/> is shown.
        /// </summary>
        event EventHandler DropDown;

        /// <summary>
        /// Occurs when the drop-down portion of the <see cref="System.Windows.Forms.ToolStripComboBox"/> has closed.
        /// </summary>
        event EventHandler DropDownClosed;

        /// <summary>
        /// Occurs when the <see cref="System.Windows.Forms.ToolStripComboBox.DropDownStyle"/> property has changed.
        /// </summary>
        event EventHandler DropDownStyleChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="System.Windows.Forms.ToolStripComboBox.SelectedIndex"/> property has changed.
        /// </summary>
        event EventHandler SelectedIndexChanged;

        /// <summary>
        /// Occurs when the <see cref="System.Windows.Forms.ToolStripComboBox"/> text has changed.
        /// </summary>
        event EventHandler TextUpdate;

        /// <summary>
        /// Gets or sets the custom string collection to use when the <see cref="System.Windows.Forms.ToolStripComboBox.AutoCompleteSource"/>
        /// property is set to <see cref="System.Windows.Forms.AutoCompleteSource.CustomSource"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        AutoCompleteStringCollection AutoCompleteCustomSource { set; get; }

        /// <summary>
        /// Gets or sets a value that indicates the text completion behavior of the control.
        /// The default is <see cref="System.Windows.Forms.AutoCompleteMode.None"/>.
        /// </summary>
        AutoCompleteMode AutoCompleteMode { set; get; }

        /// <summary>
        /// Gets or sets the source of complete strings used for automatic completion.
        /// The default is <see cref="System.Windows.Forms.AutoCompleteSource.None"/>.
        /// </summary>
        AutoCompleteSource AutoCompleteSource { set; get; }

        /// <summary>
        /// Gets a <see cref="System.Windows.Forms.ComboBox"/> in which the user can enter text, along with a list from which the user can select.
        /// </summary>
        ComboBox ComboBox { get; }

        /// <summary>
        /// Gets or sets the height, in pixels, of the drop-down portion box of a <see cref="System.Windows.Forms.ToolStripComboBox"/>.
        /// </summary>
        int DropDownHeight { set; get; }

        /// <summary>
        /// Gets or sets a value specifying the style of the <see cref="System.Windows.Forms.ToolStripComboBox"/>.
        /// The default is <see cref="System.Windows.Forms.ComboBoxStyle.DropDown"/>.
        /// </summary>
        ComboBoxStyle DropDownStyle { set; get; }

        /// <summary>
        /// Gets or sets the width, in pixels, of the drop-down portion of a <see cref="System.Windows.Forms.ToolStripComboBox"/>.
        /// </summary>
        int DropDownWidth { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control currently displays its drop-down portion.
        /// </summary>
        bool DroppedDown { set; get; }

        /// <summary>
        /// Gets or sets the appearance of the control.
        /// The default is <see cref="System.Windows.Forms.FlatStyle.Popup"/>.
        /// </summary>
        FlatStyle FlatStyle { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control should resize to avoid showing partial items.
        /// The default is true.
        /// </summary>
        bool IntegralHeight { set; get; }

        /// <summary>
        /// Gets a collection of the items contained in this control.
        /// </summary>
        ComboBox.ObjectCollection Items { get; }

        /// <summary>
        /// Gets or sets the maximum number of items to be shown in the drop-down portion of the control.
        /// </summary>
        int MaxDropDownItems { set; get; }

        /// <summary>
        /// Gets or sets the maximum number of characters allowed in the editable portion of a combo box.
        /// </summary>
        int MaxLength { set; get; }

        /// <summary>
        /// Gets or sets the index specifying the currently selected item.
        /// </summary>
        int SelectedIndex { set; get; }

        /// <summary>
        /// Gets or sets currently selected item in the control.
        /// </summary>
        object SelectedItem { set; get; }

        /// <summary>
        /// Gets or sets the text that is selected in the editable portion of the control.
        /// </summary>
        string SelectedText { set; get; }

        /// <summary>
        /// Gets or sets the number of characters selected in the editable portion of the control.
        /// </summary>
        int SelectionLength { set; get; }

        /// <summary>
        /// Gets or sets the starting index of text selected in the control.
        /// </summary>
        int SelectionStart { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the items in the control are sorted.
        /// The default is false.
        /// </summary>
        bool Sorted { set; get; }

        /// <summary>
        /// Maintains performance when items are added to the control one at a time.
        /// </summary>
        void BeginUpdate();

        /// <summary>
        /// Resumes painting the control after painting is suspended by the <see cref="IToolBarComboBoxSettings.BeginUpdate"/> method.
        /// </summary>
        void EndUpdate();

        /// <summary>
        /// Finds the first item after the given index which starts with the given string.
        /// </summary>
        /// <param name="s">The System.String to search for.</param>
        /// <param name="startIndex"> The zero-based index of the item before the first item to be searched.
        /// Set to -1 to search from the beginning of the control.</param>
        /// <returns>The zero-based index of the first item found; returns -1 if no match is found.</returns>
        int FindString(string s, int startIndex);

        /// <summary>
        /// Finds the first item in the control that starts with the specified string.
        /// </summary>
        /// <param name="s">The System.String to search for.</param>
        /// <returns>The zero-based index of the first item found; returns -1 if no match is found.</returns>
        int FindString(string s);

        /// <summary>
        /// Finds the first item after the specified index that exactly matches the specified string.
        /// </summary>
        /// <param name="s">The System.String to search for.</param>
        /// <param name="startIndex">The zero-based index of the item before the first item to be searched.
        /// Set to -1 to search from the beginning of the control.</param>
        /// <returns>The zero-based index of the first item found; returns -1 if no match is found.</returns>
        int FindStringExact(string s, int startIndex);

        /// <summary>
        /// Finds the first item in the control that exactly matches the specified string.
        /// </summary>
        /// <param name="s">The System.String to search for.</param>
        /// <returns>The zero-based index of the first item found; -1 if no match is found.</returns>
        int FindStringExact(string s);

        /// <summary>
        /// Returns the height, in pixels, of an item in the control.
        /// </summary>
        /// <param name="index">The index of the item to return the height of.</param>
        /// <returns>The height, in pixels, of the item at the specified index.</returns>
        int GetItemHeight(int index);

        /// <summary>
        /// Retrieves the size of a rectangular area into which a control can be fitted.
        /// </summary>
        /// <param name="constrainingSize">The custom-sized area for a control.</param>
        /// <returns>An ordered pair of type <see cref="System.Drawing.Size"/> representing the width and height of a rectangle.</returns>
        Size GetPreferredSize(Size constrainingSize);

        /// <summary>
        /// Selects a range of text in the editable portion of the control.
        /// </summary>
        /// <param name="start">The position of the first character in the current text selection within the text box.</param>
        /// <param name="length">The number of characters to select.</param>
        void Select(int start, int length);

        /// <summary>
        /// Selects all the text in the editable portion of the control.
        /// </summary>
        void SelectAll();
    }
}