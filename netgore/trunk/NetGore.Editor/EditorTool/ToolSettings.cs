using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using NetGore.Collections;
using NetGore.Graphics;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// Contains the settings used to create a <see cref="Tool"/> instance.
    /// </summary>
    public class ToolSettings
    {
        Image _disabledImage;
        bool _enabledByDefault = Tool.defaultIsEnabled;
        Image _enabledImage;
        string _enabledToolsGroup;
        bool _isLocked = false;
        IEnumerable<IMapDrawingExtension> _mapDrawingExtensions;
        string _name;
        bool _onToolBarByDefault = Tool.defaultIsOnToolBar;
        ToolBarControlType _toolBarControlType = ToolBarControlType.Button;
        ToolBarVisibility _toolBarVisibility = ToolBarVisibility.Global;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolSettings"/> class.
        /// </summary>
        /// <param name="name">The name of the <see cref="Tool"/>. While it is recommended that a tool's name is unique,
        /// it is not required.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        public ToolSettings(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the <see cref="Image"/> to display for the <see cref="Tool"/> when the <see cref="Tool.IsEnabled"/> state is irrelevant.
        /// If <see cref="EnabledImage"/> is set, this will return that <see cref="Image"/>. If <see cref="DisabledImage"/> is
        /// set and <see cref="EnabledImage"/> is not, then <see cref="DisabledImage"/> will be used. If none are set, null
        /// will be returned.
        /// </summary>
        public Image DefaultImage
        {
            get { return EnabledImage ?? DisabledImage; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Image"/> to display when the <see cref="Tool"/> is disabled. When null, the <see cref="Image"/>
        /// will not change when the <see cref="Tool"/> is disabled.
        /// </summary>
        /// <exception cref="InvalidOperationException">Tried to set the value while <see cref="ToolSettings.IsLocked"/>
        /// is set.</exception>
        public Image DisabledImage
        {
            get { return _disabledImage; }
            set
            {
                if (IsLocked)
                    throw GetIsLockedException("DisabledImage");

                _disabledImage = value;
            }
        }

        /// <summary>
        /// Gets or sets if the <see cref="Tool"/> is to be set to enabled by default when no user-defined preference are
        /// available.
        /// The default value is true.
        /// </summary>
        /// <exception cref="InvalidOperationException">Tried to set the value while <see cref="ToolSettings.IsLocked"/>
        /// is set.</exception>
        [DefaultValue(Tool.defaultIsEnabled)]
        public bool EnabledByDefault
        {
            get { return _enabledByDefault; }
            set
            {
                if (IsLocked)
                    throw GetIsLockedException("EnabledByDefault");

                _enabledByDefault = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Image"/> to display when the <see cref="Tool"/> is enabled. When null, the <see cref="Image"/>
        /// will not change when the <see cref="Tool"/> is enabled.
        /// </summary>
        /// <exception cref="InvalidOperationException">Tried to set the value while <see cref="ToolSettings.IsLocked"/>
        /// is set.</exception>
        public Image EnabledImage
        {
            get { return _enabledImage; }
            set
            {
                if (IsLocked)
                    throw GetIsLockedException("EnabledImage");

                _enabledImage = value;
            }
        }

        /// <summary>
        /// Gets the name of the group that the <see cref="Tool"/> is in for restricting the enabled status of <see cref="Tool"/>s.
        /// When this value is non-null, only one <see cref="Tool"/> from this group will be allowed to be enabled at a time. Enabling
        /// a <see cref="Tool"/> will disable all others in the group. When null, this feature will be disabled.
        /// The default value is null.
        /// </summary>
        /// <exception cref="InvalidOperationException">Tried to set the value while <see cref="ToolSettings.IsLocked"/>
        /// is set.</exception>
        [DefaultValue((string)null)]
        public string EnabledToolsGroup
        {
            get { return _enabledToolsGroup; }
            set
            {
                if (IsLocked)
                    throw GetIsLockedException("ToolBarVisibility");

                _enabledToolsGroup = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="StringComparer"/> to use for comparing the group names of <see cref="Tool"/>s.
        /// Uses <see cref="StringComparer.Ordinal"/>, so comparisons are case-sensitive.
        /// </summary>
        public static StringComparer GroupNameComparer
        {
            get { return StringComparer.Ordinal; }
        }

        /// <summary>
        /// Gets if the properties in this object are locked. When this property is true and you attempt to change
        /// any of the properties, an <see cref="InvalidOperationException"/> will be thrown.
        /// </summary>
        public bool IsLocked
        {
            get { return _isLocked; }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="IMapDrawingExtension"/> that the <see cref="Tool"/> will use by default.
        /// The <see cref="Tool"/> will internally handle enabling, disabling, and calling all the <see cref="IMapDrawingExtension"/>s
        /// specified based on whether or not the <see cref="Tool"/> is enabled.
        /// </summary>
        /// <exception cref="InvalidOperationException">Tried to set the value while <see cref="ToolSettings.IsLocked"/>
        /// is set.</exception>
        public IEnumerable<IMapDrawingExtension> MapDrawingExtensions
        {
            get { return _mapDrawingExtensions; }
            set
            {
                if (IsLocked)
                    throw GetIsLockedException("MapDrawingExtensions");

                _mapDrawingExtensions = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the tool. While it is recommended that a tool's name is unique, it is not required.
        /// This property is immutable.
        /// </summary>
        /// <exception cref="InvalidOperationException">Tried to set the value while <see cref="ToolSettings.IsLocked"/>
        /// is set.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public string Name
        {
            get { return _name; }
            set
            {
                if (IsLocked)
                    throw GetIsLockedException("Name");

                if (value == null)
                    throw new ArgumentNullException("value");

                _name = value;
            }
        }

        /// <summary>
        /// Gets or sets if the <see cref="Tool"/> is to be set to be added to the <see cref="ToolBar"/>
        /// by default when no user-defined preference are available.
        /// The default value is true.
        /// </summary>
        /// <exception cref="InvalidOperationException">Tried to set the value while <see cref="ToolSettings.IsLocked"/>
        /// is set.</exception>
        [DefaultValue(Tool.defaultIsOnToolBar)]
        public bool OnToolBarByDefault
        {
            get { return _onToolBarByDefault; }
            set
            {
                if (IsLocked)
                    throw GetIsLockedException("OnToolBarByDefault");

                _onToolBarByDefault = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ToolBarControlType"/> to use for displaying this <see cref="Tool"/>
        /// in a <see cref="ToolBar"/>.
        /// The default value is <see cref="NetGore.Editor.EditorTool.ToolBarControlType.Button"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">Tried to set the value while <see cref="ToolSettings.IsLocked"/>
        /// is set.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is not defined in the
        /// <see cref="NetGore.Editor.EditorTool.ToolBarControlType"/> enum.</exception>
        [DefaultValue(ToolBarControlType.Button)]
        public ToolBarControlType ToolBarControlType
        {
            get { return _toolBarControlType; }
            set
            {
                if (IsLocked)
                    throw GetIsLockedException("ToolBarControlType");

                if (!EnumHelper<ToolBarControlType>.IsDefined(value))
                    throw new ArgumentOutOfRangeException("value");

                _toolBarControlType = value;
            }
        }

        /// <summary>
        /// Gets or sets the visibility of this <see cref="Tool"/> in a <see cref="ToolBar"/>.
        /// The default value is <see cref="NetGore.Editor.EditorTool.ToolBarVisibility.Global"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">Tried to set the value while <see cref="ToolSettings.IsLocked"/>
        /// is set.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is not defined in the
        /// <see cref="NetGore.Editor.EditorTool.ToolBarVisibility"/> enum.</exception>
        public ToolBarVisibility ToolBarVisibility
        {
            get { return _toolBarVisibility; }
            set
            {
                if (IsLocked)
                    throw GetIsLockedException("ToolBarVisibility");

                if (!EnumHelper<ToolBarVisibility>.IsDefined(value))
                    throw new ArgumentOutOfRangeException("value");

                _toolBarVisibility = value;
            }
        }

        /// <summary>
        /// Gets or sets the help display name.
        /// </summary>
        public string HelpName { get; set; }

        /// <summary>
        /// Gets or sets the help wiki page title.
        /// </summary>
        public string HelpWikiPage { get; set; }

        /// <summary>
        /// Gets the <see cref="InvalidOperationException"/> to throw when trying to change a property while
        /// <see cref="IsLocked"/> is set.
        /// </summary>
        /// <param name="propertyName">The name of the property that was attempted to be changed.</param>
        /// <returns>The <see cref="InvalidOperationException"/> to throw when trying to change a property while
        /// <see cref="IsLocked"/> is set.</returns>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IsLocked")]
        InvalidOperationException GetIsLockedException(string propertyName)
        {
            const string errmsg = "Cannot alter property `{0}` on `{1}` since IsLocked is set.";
            return new InvalidOperationException(string.Format(errmsg, propertyName, this));
        }

        /// <summary>
        /// Locks the <see cref="ToolSettings"/> so that no more changes can be made to it.
        /// </summary>
        internal void Lock()
        {
            if (IsLocked)
                return;

            // Set as locked
            _isLocked = true;

            // Prevent collections from being editing
            if (_mapDrawingExtensions == null || _mapDrawingExtensions.IsEmpty())
                _mapDrawingExtensions = Enumerable.Empty<IMapDrawingExtension>();
            else
                _mapDrawingExtensions = new ImmutableArray<IMapDrawingExtension>(_mapDrawingExtensions.ToArray());
        }
    }
}