using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.IO
{
    public abstract class RuntimeSettingsBase : IRuntimeSettings
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly string _filePath;
        readonly GenericValueIOFormat _format;
        readonly string _rootNodeName;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeSettingsBase"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="rootNodeName">Name of the root node.</param>
        /// <param name="fileFormat">The file format.</param>
        protected RuntimeSettingsBase(string filePath, string rootNodeName, GenericValueIOFormat fileFormat)
        {
            _rootNodeName = rootNodeName;
            _filePath = filePath;
            _format = fileFormat;
        }

        /// <summary>
        /// Gets this class casted to <see cref="IRuntimeSettings"/>.
        /// Provide for a convenience to access the interface members implemented explicitly.
        /// </summary>
        protected IRuntimeSettings AsRS
        {
            get { return this; }
        }

        /// <summary>
        /// Handles resetting the values to their default value.
        /// </summary>
        protected virtual void HandleReset()
        {
            // TODO: !!
        }

        #region IRuntimeSettings Members

        /// <summary>
        /// Notifies listeners when the settings have been loaded.
        /// </summary>
        public event RuntimeSettingsEventHandler Loaded;

        /// <summary>
        /// Notifies listeners when the settings have been reset.
        /// </summary>
        public event RuntimeSettingsEventHandler Resetted;

        /// <summary>
        /// Notifies listeners when the settings have been saved.
        /// </summary>
        public event RuntimeSettingsEventHandler Saved;

        /// <summary>
        /// Gets the path to the file containing the settings.
        /// </summary>
        string IRuntimeSettings.FilePath
        {
            get { return _filePath; }
        }

        /// <summary>
        /// Gets the name of the root node in the settings file.
        /// </summary>
        string IRuntimeSettings.RootNodeName
        {
            get { return _rootNodeName; }
        }

        /// <summary>
        /// Restores the settings from file.
        /// </summary>
        public void Load()
        {
            var needsReset = true;

            try
            {
                if (File.Exists(AsRS.FilePath))
                {
                    var reader = GenericValueReader.CreateFromFile(AsRS.FilePath, AsRS.RootNodeName);
                    ReadState(reader);

                    needsReset = false;
                }
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to load settings for `{0}`. Settings will be reset. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, ex);
                Debug.Fail(string.Format(errmsg, this, ex));

                needsReset = true;
            }

            // Reset if we failed to load properly
            if (needsReset)
                Reset();

            if (Loaded != null)
                Loaded(this);
        }

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public virtual void ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);
        }

        /// <summary>
        /// Restores all the settings to their default values.
        /// </summary>
        public void Reset()
        {
            HandleReset();

            if (Resetted != null)
                Resetted(this);
        }

        /// <summary>
        /// Saves the settings to file.
        /// </summary>
        public void Save()
        {
            using (var writer = new GenericValueWriter(AsRS.FilePath, AsRS.RootNodeName, _format))
            {
                WriteState(writer);
            }

            if (Saved != null)
                Saved(this);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public virtual void WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);
        }

        #endregion
    }
}