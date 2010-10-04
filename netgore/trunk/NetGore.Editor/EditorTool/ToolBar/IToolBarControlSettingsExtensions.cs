using System;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// Extension methods for the <see cref="IToolBarControlSettings"/>.
    /// </summary>
    public static class IToolBarControlSettingsExtensions
    {
        /// <summary>
        /// Gets the <see cref="IToolBarControlSettings"/> as a <see cref="IToolBarButtonSettings"/>.
        /// Only valid when <see cref="IToolBarControl.ControlType"/> equals <see cref="ToolBarControlType.Button"/>.
        /// </summary>
        /// <param name="controlSettings">The <see cref="IToolBarControlSettings"/> to try to cast to the
        /// desired derived control settings type.</param>
        /// <returns>
        /// The <paramref name="controlSettings"/> as <see cref="IToolBarButtonSettings"/>,
        /// or null if <paramref name="controlSettings"/> is not of the expected type.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="controlSettings"/> is null.</exception>
        public static IToolBarButtonSettings AsButtonSettings(this IToolBarControlSettings controlSettings)
        {
            if (controlSettings == null)
                throw new ArgumentNullException("controlSettings");

            Debug.Assert(controlSettings is IToolBarButtonSettings);

            return controlSettings as IToolBarButtonSettings;
        }

        /// <summary>
        /// Gets the <see cref="IToolBarControlSettings"/> as a <see cref="IToolBarComboBoxSettings"/>.
        /// Only valid when <see cref="IToolBarControl.ControlType"/> equals <see cref="ToolBarControlType.ComboBox"/>.
        /// </summary>
        /// <param name="controlSettings">The <see cref="IToolBarControlSettings"/> to try to cast to the
        /// desired derived control settings type.</param>
        /// <returns>
        /// The <paramref name="controlSettings"/> as <see cref="IToolBarComboBoxSettings"/>,
        /// or null if <paramref name="controlSettings"/> is not of the expected type.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="controlSettings"/> is null.</exception>
        public static IToolBarComboBoxSettings AsComboBoxSettings(this IToolBarControlSettings controlSettings)
        {
            if (controlSettings == null)
                throw new ArgumentNullException("controlSettings");

            Debug.Assert(controlSettings is IToolBarComboBoxSettings);

            return controlSettings as IToolBarComboBoxSettings;
        }

        /// <summary>
        /// Gets the <see cref="IToolBarControlSettings"/> as a <see cref="IToolBarDropDownButtonSettings"/>.
        /// Only valid when <see cref="IToolBarControl.ControlType"/> equals <see cref="ToolBarControlType.DropDownButton"/>.
        /// </summary>
        /// <param name="controlSettings">The <see cref="IToolBarControlSettings"/> to try to cast to the
        /// desired derived control settings type.</param>
        /// <returns>
        /// The <paramref name="controlSettings"/> as <see cref="IToolBarDropDownButtonSettings"/>,
        /// or null if <paramref name="controlSettings"/> is not of the expected type.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="controlSettings"/> is null.</exception>
        public static IToolBarDropDownButtonSettings AsDropDownButtonSettings(this IToolBarControlSettings controlSettings)
        {
            if (controlSettings == null)
                throw new ArgumentNullException("controlSettings");

            Debug.Assert(controlSettings is IToolBarDropDownButtonSettings);

            return controlSettings as IToolBarDropDownButtonSettings;
        }

        /// <summary>
        /// Gets the <see cref="IToolBarControlSettings"/> as a <see cref="IToolBarDropDownItemSettings"/>.
        /// Only valid when <see cref="IToolBarControl.ControlType"/> equals <see cref="ToolBarControlType.DropDownButton"/>
        /// or <see cref="ToolBarControlType.SplitButton"/>.
        /// </summary>
        /// <param name="controlSettings">The <see cref="IToolBarControlSettings"/> to try to cast to the
        /// desired derived control settings type.</param>
        /// <returns>
        /// The <paramref name="controlSettings"/> as <see cref="IToolBarDropDownItemSettings"/>,
        /// or null if <paramref name="controlSettings"/> is not of the expected type.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="controlSettings"/> is null.</exception>
        public static IToolBarDropDownItemSettings AsDropDownItemSettings(this IToolBarControlSettings controlSettings)
        {
            if (controlSettings == null)
                throw new ArgumentNullException("controlSettings");

            Debug.Assert(controlSettings is IToolBarDropDownItemSettings);

            return controlSettings as IToolBarDropDownItemSettings;
        }

        /// <summary>
        /// Gets the <see cref="IToolBarControlSettings"/> as a <see cref="IToolBarLabelSettings"/>.
        /// Only valid when <see cref="IToolBarControl.ControlType"/> equals <see cref="ToolBarControlType.Label"/>.
        /// </summary>
        /// <param name="controlSettings">The <see cref="IToolBarControlSettings"/> to try to cast to the
        /// desired derived control settings type.</param>
        /// <returns>
        /// The <paramref name="controlSettings"/> as <see cref="IToolBarLabelSettings"/>,
        /// or null if <paramref name="controlSettings"/> is not of the expected type.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="controlSettings"/> is null.</exception>
        public static IToolBarLabelSettings AsLabelSettings(this IToolBarControlSettings controlSettings)
        {
            if (controlSettings == null)
                throw new ArgumentNullException("controlSettings");

            Debug.Assert(controlSettings is IToolBarLabelSettings);

            return controlSettings as IToolBarLabelSettings;
        }

        /// <summary>
        /// Gets the <see cref="IToolBarControlSettings"/> as a <see cref="IToolBarProgressBarSettings"/>.
        /// Only valid when <see cref="IToolBarControl.ControlType"/> equals <see cref="ToolBarControlType.ProgressBar"/>.
        /// </summary>
        /// <param name="controlSettings">The <see cref="IToolBarControlSettings"/> to try to cast to the
        /// desired derived control settings type.</param>
        /// <returns>
        /// The <paramref name="controlSettings"/> as <see cref="IToolBarProgressBarSettings"/>,
        /// or null if <paramref name="controlSettings"/> is not of the expected type.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="controlSettings"/> is null.</exception>
        public static IToolBarProgressBarSettings AsProgressBarSettings(this IToolBarControlSettings controlSettings)
        {
            if (controlSettings == null)
                throw new ArgumentNullException("controlSettings");

            Debug.Assert(controlSettings is IToolBarProgressBarSettings);

            return controlSettings as IToolBarProgressBarSettings;
        }

        /// <summary>
        /// Gets the <see cref="IToolBarControlSettings"/> as a <see cref="IToolBarSplitButtonSettings"/>.
        /// Only valid when <see cref="IToolBarControl.ControlType"/> equals <see cref="ToolBarControlType.SplitButton"/>.
        /// </summary>
        /// <param name="controlSettings">The <see cref="IToolBarControlSettings"/> to try to cast to the
        /// desired derived control settings type.</param>
        /// <returns>
        /// The <paramref name="controlSettings"/> as <see cref="IToolBarSplitButtonSettings"/>,
        /// or null if <paramref name="controlSettings"/> is not of the expected type.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="controlSettings"/> is null.</exception>
        public static IToolBarSplitButtonSettings AsSplitButtonSettings(this IToolBarControlSettings controlSettings)
        {
            if (controlSettings == null)
                throw new ArgumentNullException("controlSettings");

            Debug.Assert(controlSettings is IToolBarSplitButtonSettings);

            return controlSettings as IToolBarSplitButtonSettings;
        }

        /// <summary>
        /// Gets the <see cref="IToolBarControlSettings"/> as a <see cref="IToolBarTextBoxSettings"/>.
        /// Only valid when <see cref="IToolBarControl.ControlType"/> equals <see cref="ToolBarControlType.TextBox"/>.
        /// </summary>
        /// <param name="controlSettings">The <see cref="IToolBarControlSettings"/> to try to cast to the
        /// desired derived control settings type.</param>
        /// <returns>
        /// The <paramref name="controlSettings"/> as <see cref="IToolBarTextBoxSettings"/>,
        /// or null if <paramref name="controlSettings"/> is not of the expected type.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="controlSettings"/> is null.</exception>
        public static IToolBarTextBoxSettings AsTextBoxSettings(this IToolBarControlSettings controlSettings)
        {
            if (controlSettings == null)
                throw new ArgumentNullException("controlSettings");

            Debug.Assert(controlSettings is IToolBarTextBoxSettings);

            return controlSettings as IToolBarTextBoxSettings;
        }
    }
}