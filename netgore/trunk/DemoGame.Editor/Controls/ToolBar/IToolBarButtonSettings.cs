namespace DemoGame.Editor
{
    /// <summary>
    /// Contains the settings specific to the <see cref="ToolBarControlType.DropDownButton"/>.
    /// </summary>
    public interface IToolBarSplitButtonSettings : IToolBarDropDownItemSettings
    {
        /// <summary>
        /// Retrieves the size of a rectangular area into which a <see cref="System.Windows.Forms.ToolStripSplitButton"/> can be fitted.
        /// </summary>
        /// <param name="constrainingSize">The custom-sized area for a control.</param>
        /// <returns>An ordered pair of type <see cref="System.Drawing.Size"/>, representing the width and height of a rectangle.</returns>
        System.Drawing.Size GetPreferredSize(System.Drawing.Size constrainingSize);

        /// <summary>
        /// If the <see cref="System.Windows.Forms.ToolStripItem.Enabled"/> property is true,
        /// calls the <see cref="System.Windows.Forms.ToolStripSplitButton.OnButtonClick(System.EventArgs)"/> method.
        /// </summary>
        void PerformButtonClick();

        /// <summary>
        /// Gets or sets a value indicating whether default or custom <see cref="System.Windows.Forms.ToolTip"/> text
        /// is displayed on the <see cref="System.Windows.Forms.ToolStripSplitButton"/>.
        /// </summary>
        bool AutoToolTip { set; get; }

        /// <summary>
        /// Gets the size and location of the standard button portion of a <see cref="System.Windows.Forms.ToolStripSplitButton"/>.
        /// </summary>
        System.Drawing.Rectangle ButtonBounds { get; }

        /// <summary>
        /// Gets a value indicating whether the button portion of the <see cref="System.Windows.Forms.ToolStripSplitButton"/>
        /// is in the pressed state.
        /// </summary>
        bool ButtonPressed { get; }

        /// <summary>
        /// Gets a value indicating whether the standard button portion of a <see cref="System.Windows.Forms.ToolStripSplitButton"/>
        /// is selected or the <see cref="System.Windows.Forms.ToolStripSplitButton.DropDownButtonPressed"/> property is true.
        /// </summary>
        bool ButtonSelected { get; }

        /// <summary>
        /// Gets or sets the portion of the <see cref="System.Windows.Forms.ToolStripSplitButton"/> that is activated when the
        /// control is first selected.
        /// </summary>
        System.Windows.Forms.ToolStripItem DefaultItem { set; get; }

        /// <summary>
        /// Gets the size and location, in screen coordinates, of the drop-down button portion of a
        /// <see cref="System.Windows.Forms.ToolStripSplitButton"/>.
        /// </summary>
        System.Drawing.Rectangle DropDownButtonBounds { get; }

        /// <summary>
        /// Gets a value indicating whether the drop-down portion of the <see cref="System.Windows.Forms.ToolStripSplitButton"/>
        /// is in the pressed state.
        /// </summary>
        bool DropDownButtonPressed { get; }

        /// <summary>
        /// Gets a value indicating whether the drop-down button portion of a
        /// <see cref="System.Windows.Forms.ToolStripSplitButton"/> is selected.
        /// </summary>
        bool DropDownButtonSelected { get; }

        /// <summary>
        /// The width, in pixels, of the drop-down button portion of a <see cref="System.Windows.Forms.ToolStripSplitButton"/>.
        /// </summary>
        int DropDownButtonWidth { set; get; }

        /// <summary>
        /// Gets the boundaries of the separator between the standard and drop-down button portions of a
        /// <see cref="System.Windows.Forms.ToolStripSplitButton"/>.
        /// </summary>
        System.Drawing.Rectangle SplitterBounds { get; }

        /// <summary>
        /// Occurs when the standard button portion of a <see cref="System.Windows.Forms.ToolStripSplitButton"/> is clicked.
        /// </summary>
        event System.EventHandler ButtonClick;

        /// <summary>
        /// Occurs when the standard button portion of a <see cref="System.Windows.Forms.ToolStripSplitButton"/> is double-clicked.
        /// </summary>
        event System.EventHandler ButtonDoubleClick;

        /// <summary>
        /// Occurs when the <see cref="System.Windows.Forms.ToolStripSplitButton.DefaultItem"/> has changed.
        /// </summary>
        event System.EventHandler DefaultItemChanged;
    }

    /// <summary>
    /// Contains the settings specific to the <see cref="ToolBarControlType.DropDownButton"/>.
    /// </summary>
    public interface IToolBarDropDownButtonSettings : IToolBarDropDownItemSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use the Text property or the
        /// <see cref="System.Windows.Forms.ToolStripItem.ToolTipText"/> property for the
        /// <see cref="System.Windows.Forms.ToolStripDropDownButton"/> ToolTip.
        /// </summary>
        bool AutoToolTip { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether an arrow is displayed on the
        /// <see cref="System.Windows.Forms.ToolStripDropDownButton"/>,
        /// which indicates that further options are available in a drop-down list.
        /// </summary>
        bool ShowDropDownArrow { set; get; }
    }

    /// <summary>
    /// Contains the settings specific to the <see cref="ToolBarControlType.DropDownButton"/>.
    /// </summary>
    public interface IToolBarDropDownItemSettings : IToolBarControlSettings
    {
        /// <summary>
        /// Makes a visible <see cref="System.Windows.Forms.ToolStripDropDown"/> hidden.
        /// </summary>
        void HideDropDown();

        /// <summary>
        /// Displays the <see cref="System.Windows.Forms.ToolStripDropDownItem"/> control associated with this
        /// <see cref="System.Windows.Forms.ToolStripDropDownItem"/>.
        /// </summary>
        void ShowDropDown();

        /// <summary>
        /// Gets or sets the <see cref="System.Windows.Forms.ToolStripDropDown"/> that will be displayed when
        /// this <see cref="System.Windows.Forms.ToolStripDropDownItem"/> is clicked.
        /// </summary>
        System.Windows.Forms.ToolStripDropDown DropDown { set; get; }

        /// <summary>
        /// Gets or sets a value indicating the direction in which the <see cref="System.Windows.Forms.ToolStripDropDownItem"/>
        /// emerges from its parent container.
        /// </summary>
        System.Windows.Forms.ToolStripDropDownDirection DropDownDirection { set; get; }

        /// <summary>
        /// Gets the collection of items in the <see cref="System.Windows.Forms.ToolStripDropDown"/> that is associated
        /// with this <see cref="System.Windows.Forms.ToolStripDropDownItem"/>.
        /// </summary>
        System.Windows.Forms.ToolStripItemCollection DropDownItems { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="System.Windows.Forms.ToolStripDropDownItem"/>
        /// has <see cref="System.Windows.Forms.ToolStripDropDown"/> controls associated with it.
        /// </summary>
        bool HasDropDownItems { get; }

        /// <summary>
        /// Occurs when the <see cref="System.Windows.Forms.ToolStripDropDown"/> closes.
        /// </summary>
        event System.EventHandler DropDownClosed;

        /// <summary>
        /// Occurs when the <see cref="System.Windows.Forms.ToolStripDropDown"/> is clicked.
        /// </summary>
        event System.Windows.Forms.ToolStripItemClickedEventHandler DropDownItemClicked;

        /// <summary>
        /// Occurs when the <see cref="System.Windows.Forms.ToolStripDropDown"/> has opened.
        /// </summary>
        event System.EventHandler DropDownOpened;

        /// <summary>
        /// Occurs as the <see cref="System.Windows.Forms.ToolStripDropDown"/> is opening.
        /// </summary>
        event System.EventHandler DropDownOpening;
    }

    /// <summary>
    /// Contains the settings specific to the <see cref="ToolBarControlType.ComboBox"/>.
    /// </summary>
    public interface IToolBarComboBoxSettings : IToolBarControlSettings
    {
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
        System.Drawing.Size GetPreferredSize(System.Drawing.Size constrainingSize);

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

        /// <summary>
        /// Gets or sets the custom string collection to use when the <see cref="System.Windows.Forms.ToolStripComboBox.AutoCompleteSource"/>
        /// property is set to <see cref="System.Windows.Forms.AutoCompleteSource.CustomSource"/>.
        /// </summary>
        System.Windows.Forms.AutoCompleteStringCollection AutoCompleteCustomSource { set; get; }

        /// <summary>
        /// Gets or sets a value that indicates the text completion behavior of the control.
        /// The default is <see cref="System.Windows.Forms.AutoCompleteMode.None"/>.
        /// </summary>
        System.Windows.Forms.AutoCompleteMode AutoCompleteMode { set; get; }

        /// <summary>
        /// Gets or sets the source of complete strings used for automatic completion.
        /// The default is <see cref="System.Windows.Forms.AutoCompleteSource.None"/>.
        /// </summary>
        System.Windows.Forms.AutoCompleteSource AutoCompleteSource { set; get; }

        /// <summary>
        /// Gets a <see cref="System.Windows.Forms.ComboBox"/> in which the user can enter text, along with a list from which the user can select.
        /// </summary>
        System.Windows.Forms.ComboBox ComboBox { get; }

        /// <summary>
        /// Gets or sets the height, in pixels, of the drop-down portion box of a <see cref="System.Windows.Forms.ToolStripComboBox"/>.
        /// </summary>
        int DropDownHeight { set; get; }

        /// <summary>
        /// Gets or sets a value specifying the style of the <see cref="System.Windows.Forms.ToolStripComboBox"/>.
        /// The default is <see cref="System.Windows.Forms.ComboBoxStyle.DropDown"/>.
        /// </summary>
        System.Windows.Forms.ComboBoxStyle DropDownStyle { set; get; }

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
        System.Windows.Forms.FlatStyle FlatStyle { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control should resize to avoid showing partial items.
        /// The default is true.
        /// </summary>
        bool IntegralHeight { set; get; }

        /// <summary>
        /// Gets a collection of the items contained in this control.
        /// </summary>
        System.Windows.Forms.ComboBox.ObjectCollection Items { get; }

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
        /// Occurs when the drop-down portion of a <see cref="System.Windows.Forms.ToolStripComboBox"/> is shown.
        /// </summary>
        event System.EventHandler DropDown;

        /// <summary>
        /// Occurs when the drop-down portion of the <see cref="System.Windows.Forms.ToolStripComboBox"/> has closed.
        /// </summary>
        event System.EventHandler DropDownClosed;

        /// <summary>
        /// Occurs when the <see cref="System.Windows.Forms.ToolStripComboBox.DropDownStyle"/> property has changed.
        /// </summary>
        event System.EventHandler DropDownStyleChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="System.Windows.Forms.ToolStripComboBox.SelectedIndex"/> property has changed.
        /// </summary>
        event System.EventHandler SelectedIndexChanged;

        /// <summary>
        /// Occurs when the <see cref="System.Windows.Forms.ToolStripComboBox"/> text has changed.
        /// </summary>
        event System.EventHandler TextUpdate;
    }

    /// <summary>
    /// Contains the settings specific to the <see cref="ToolBarControlType.TextBox"/>.
    /// </summary>
    public interface IToolBarTextBoxSettings : IToolBarControlSettings
    {
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
        char GetCharFromPosition(System.Drawing.Point pt);

        /// <summary>
        /// Retrieves the index of the character nearest to the specified location.
        /// </summary>
        /// <param name="pt">The location to search.</param>
        /// <returns>The zero-based character index at the specified location.</returns>
        int GetCharIndexFromPosition(System.Drawing.Point pt);

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
        System.Drawing.Point GetPositionFromCharIndex(int index);

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
        System.Windows.Forms.AutoCompleteStringCollection AutoCompleteCustomSource { set; get; }

        /// <summary>
        /// Gets or sets an option that controls how automatic completion works for the
        /// <see cref="System.Windows.Forms.ToolStripTextBox"/>.
        /// </summary>
        System.Windows.Forms.AutoCompleteMode AutoCompleteMode { set; get; }

        /// <summary>
        /// Gets or sets a value specifying the source of complete strings used for automatic completion.
        /// </summary>
        System.Windows.Forms.AutoCompleteSource AutoCompleteSource { set; get; }

        /// <summary>
        /// Gets or sets the border type of the <see cref="System.Windows.Forms.ToolStripTextBox"/> control.
        /// </summary>
        System.Windows.Forms.BorderStyle BorderStyle { set; get; }

        /// <summary>
        /// Gets a value indicating whether the user can undo the previous operation in a
        /// <see cref="System.Windows.Forms.ToolStripTextBox"/> control.
        /// </summary>
        bool CanUndo { get; }

        /// <summary>
        /// Gets or sets whether the <see cref="System.Windows.Forms.ToolStripTextBox"/> control modifies the case of
        /// characters as they are typed.
        /// </summary>
        System.Windows.Forms.CharacterCasing CharacterCasing { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the selected text in the text box control remains
        /// highlighted when the control loses focus.
        /// </summary>
        bool HideSelection { set; get; }

        /// <summary>
        /// Gets or sets the lines of text in a <see cref="System.Windows.Forms.ToolStripTextBox"/> control.
        /// </summary>
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
        System.Windows.Forms.TextBox TextBox { get; }

        /// <summary>
        /// Gets or sets how text is aligned in a <see cref="System.Windows.Forms.TextBox"/> control.
        /// </summary>
        System.Windows.Forms.HorizontalAlignment TextBoxTextAlign { set; get; }

        /// <summary>
        /// Gets the length of text in the control.
        /// </summary>
        int TextLength { get; }

        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarTextBoxSettings.AcceptsTab"/> property changes.
        /// </summary>
        event System.EventHandler AcceptsTabChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarTextBoxSettings.BorderStyle"/> property changes.
        /// </summary>
        event System.EventHandler BorderStyleChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarTextBoxSettings.HideSelection"/> property changes.
        /// </summary>
        event System.EventHandler HideSelectionChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarTextBoxSettings.Modified"/> property changes.
        /// </summary>
        event System.EventHandler ModifiedChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarTextBoxSettings.ReadOnly"/> property changes.
        /// </summary>
        event System.EventHandler ReadOnlyChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarTextBoxSettings.TextBoxTextAlign"/> property changes.
        /// </summary>
        event System.EventHandler TextBoxTextAlignChanged;
    }

    /// <summary>
    /// Contains the settings specific to the <see cref="ToolBarControlType.ProgressBar"/>.
    /// </summary>
    public interface IToolBarProgressBarSettings : IToolBarControlSettings
    {
        /// <summary>
        /// Gets or sets a value representing the delay between each
        /// <see cref="System.Windows.Forms.ProgressBarStyle.Marquee"/> display update, in milliseconds.
        /// </summary>
        int MarqueeAnimationSpeed { set; get; }

        /// <summary>
        /// Gets or sets the upper bound of the range that is defined for this control.
        /// </summary>
        int Maximum { set; get; }

        /// <summary>
        /// Gets or sets the lower bound of the range that is defined for this control.
        /// </summary>
        int Minimum { set; get; }

        /// <summary>
        /// Gets the <see cref="System.Windows.Forms.ProgressBar"/>.
        /// </summary>
        System.Windows.Forms.ProgressBar ProgressBar { get; }

        /// <summary>
        /// Advances the current position of the progress bar by the specified amount.
        /// </summary>
        /// <param name="value">The amount by which to increment the progress bar's current position.</param>
        void Increment(int value);

        /// <summary>
        /// Advances the current position of the progress bar by the amount of the
        /// <see cref="IToolBarProgressBarSettings.Step"/> property.
        /// </summary>
        void PerformStep();

        /// <summary>
        /// Gets or sets the amount by which to increment the current value of the progress bar each time
        /// <see cref="IToolBarProgressBarSettings.PerformStep"/> is called.
        /// </summary>
        int Step { set; get; }

        /// <summary>
        /// Gets or sets the style of the progress bar.
        /// </summary>
        System.Windows.Forms.ProgressBarStyle Style { set; get; }

        /// <summary>
        /// Gets or sets the current value of the progress bar.
        /// </summary>
        int Value { set; get; }
    }

    /// <summary>
    /// Contains the settings specific to the <see cref="ToolBarControlType.Button"/>.
    /// </summary>
    public interface IToolBarButtonSettings : IToolBarControlSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether the control should automatically appear pressed in and not
        /// pressed in when clicked.
        /// </summary>
        bool CheckOnClick { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control is in the pressed or not pressed state by default,
        /// or is in an indeterminate state.
        /// </summary>
        System.Windows.Forms.CheckState CheckState { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control is pressed or not pressed.
        /// </summary>
        bool Checked { set; get; }

        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarButtonSettings.CheckState"/> property changes.
        /// </summary>
        event System.EventHandler CheckStateChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarButtonSettings.Checked"/> property changes.
        /// </summary>
        event System.EventHandler CheckedChanged;
    }
}