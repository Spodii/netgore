using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// Contains the settings specific to the <see cref="ToolBarControlType.TextBox"/>.
    /// </summary>
    public interface IToolBarTextBoxSettings : IToolBarControlSettings
    {
        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarTextBoxSettings.AcceptsTab"/> property changes.
        /// </summary>
        event EventHandler AcceptsTabChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarTextBoxSettings.BorderStyle"/> property changes.
        /// </summary>
        event EventHandler BorderStyleChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarTextBoxSettings.HideSelection"/> property changes.
        /// </summary>
        event EventHandler HideSelectionChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarTextBoxSettings.Modified"/> property changes.
        /// </summary>
        event EventHandler ModifiedChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarTextBoxSettings.ReadOnly"/> property changes.
        /// </summary>
        event EventHandler ReadOnlyChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarTextBoxSettings.TextBoxTextAlign"/> property changes.
        /// </summary>
        event EventHandler TextBoxTextAlignChanged;

        /// <summary>
        /// Gets or sets a value indicating whether pressing ENTER in a multiline <see cref="System.Windows.Forms.TextBox"/>
        /// control creates a new line of text in the control or activates the default button for the form.
        /// </summary>
        bool AcceptsReturn { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether pressing the TAB key in a multiline text box control types a TAB
        /// character in the control instead of moving the focus to the next control in the tab order.
        /// </summary>
        bool AcceptsTab { set; get; }

        /// <summary>
        /// Gets or sets a custom string collection to use when the
        /// <see cref="System.Windows.Forms.ToolStripTextBox.AutoCompleteSource"/> property is set to CustomSource.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        AutoCompleteStringCollection AutoCompleteCustomSource { set; get; }

        /// <summary>
        /// Gets or sets an option that controls how automatic completion works for the
        /// <see cref="System.Windows.Forms.ToolStripTextBox"/>.
        /// </summary>
        AutoCompleteMode AutoCompleteMode { set; get; }

        /// <summary>
        /// Gets or sets a value specifying the source of complete strings used for automatic completion.
        /// </summary>
        AutoCompleteSource AutoCompleteSource { set; get; }

        /// <summary>
        /// Gets or sets the border type of the <see cref="System.Windows.Forms.ToolStripTextBox"/> control.
        /// </summary>
        BorderStyle BorderStyle { set; get; }

        /// <summary>
        /// Gets a value indicating whether the user can undo the previous operation in a
        /// <see cref="System.Windows.Forms.ToolStripTextBox"/> control.
        /// </summary>
        bool CanUndo { get; }

        /// <summary>
        /// Gets or sets whether the <see cref="System.Windows.Forms.ToolStripTextBox"/> control modifies the case of
        /// characters as they are typed.
        /// </summary>
        CharacterCasing CharacterCasing { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the selected text in the text box control remains
        /// highlighted when the control loses focus.
        /// </summary>
        bool HideSelection { set; get; }

        /// <summary>
        /// Gets or sets the lines of text in a <see cref="System.Windows.Forms.ToolStripTextBox"/> control.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        string[] Lines { set; get; }

        /// <summary>
        /// Gets or sets the maximum number of characters the user can type or paste into the text box control.
        /// The default is 32767 characters.
        /// </summary>
        int MaxLength { set; get; }

        /// <summary>
        /// Gets or sets a value that indicates that the <see cref="System.Windows.Forms.ToolStripTextBox"/> control has been
        /// modified by the user since the control was created or its contents were last set.
        /// </summary>
        bool Modified { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether text in the control is read-only.
        /// </summary>
        bool ReadOnly { set; get; }

        /// <summary>
        /// Gets or sets a value indicating the currently selected text in the control.
        /// </summary>
        string SelectedText { set; get; }

        /// <summary>
        /// Gets or sets the number of characters selected in control.
        /// </summary>
        int SelectionLength { set; get; }

        /// <summary>
        /// Gets or sets the starting point of text selected in the control.
        /// </summary>
        int SelectionStart { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the defined shortcuts are enabled.
        /// </summary>
        bool ShortcutsEnabled { set; get; }

        /// <summary>
        /// Gets the hosted <see cref="System.Windows.Forms.TextBox"/> control.
        /// </summary>
        TextBox TextBox { get; }

        /// <summary>
        /// Gets or sets how text is aligned in a <see cref="System.Windows.Forms.TextBox"/> control.
        /// </summary>
        HorizontalAlignment TextBoxTextAlign { set; get; }

        /// <summary>
        /// Gets the length of text in the control.
        /// </summary>
        int TextLength { get; }

        /// <summary>
        /// Appends text to the current text of the control.
        /// </summary>
        /// <param name="text">The text to append.</param>
        void AppendText(string text);

        /// <summary>
        /// Clears all of the text from the control.
        /// </summary>
        void Clear();

        /// <summary>
        /// Clears information about the most recent operation from the undo buffer of the control.
        /// </summary>
        void ClearUndo();

        /// <summary>
        /// Copies the current selection in the control to the Clipboard.
        /// </summary>
        void Copy();

        /// <summary>
        /// Moves the current selection in the control to the Clipboard.
        /// </summary>
        void Cut();

        /// <summary>
        /// Deselects all of the text in the control.
        /// </summary>
        void DeselectAll();

        /// <summary>
        /// Retrieves the character that is closest to the specified location within the control.
        /// </summary>
        /// <param name="pt">The location from which to seek the nearest character.</param>
        /// <returns>The character at the specified location.</returns>
        char GetCharFromPosition(Point pt);

        /// <summary>
        /// Retrieves the index of the character nearest to the specified location.
        /// </summary>
        /// <param name="pt">The location to search.</param>
        /// <returns>The zero-based character index at the specified location.</returns>
        int GetCharIndexFromPosition(Point pt);

        /// <summary>
        /// Retrieves the index of the first character of a given line.
        /// </summary>
        /// <param name="lineNumber">The line for which to get the index of its first character.</param>
        /// <returns>The zero-based character index in the specified line.</returns>
        int GetFirstCharIndexFromLine(int lineNumber);

        /// <summary>
        /// Retrieves the index of the first character of the current line.
        /// </summary>
        /// <returns>The zero-based character index in the current line.</returns>
        int GetFirstCharIndexOfCurrentLine();

        /// <summary>
        /// Retrieves the line number from the specified character position within the text of the control.
        /// </summary>
        /// <param name="index">The character index position to search.</param>
        /// <returns>The zero-based line number in which the character index is located.</returns>
        int GetLineFromCharIndex(int index);

        /// <summary>
        /// Retrieves the location within the control at the specified character index.
        /// </summary>
        /// <param name="index">The index of the character for which to retrieve the location.</param>
        /// <returns>The location of the specified character.</returns>
        Point GetPositionFromCharIndex(int index);

        /// <summary>
        /// Replaces the current selection in the text box with the contents of the Clipboard.
        /// </summary>
        void Paste();

        /// <summary>
        /// Scrolls the contents of the control to the current caret position.
        /// </summary>
        void ScrollToCaret();

        /// <summary>
        /// Selects a range of text in the text box.
        /// </summary>
        /// <param name="start">The position of the first character in the current text selection within the text box.</param>
        /// <param name="length">The number of characters to select.</param>
        void Select(int start, int length);

        /// <summary>
        /// Selects all text in the text box.
        /// </summary>
        void SelectAll();

        /// <summary>
        /// Undoes the last edit operation in the text box.
        /// </summary>
        void Undo();
    }
}