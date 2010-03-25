using System;
using System.Linq;

namespace NetGore.IO.PropertySync
{
    /// <summary>
    /// An abstract implementation of the <see cref="IPropertySync"/>.
    /// </summary>
    /// <typeparam name="T">The type to synchronize.</typeparam>
    public abstract class PropertySyncBase<T> : IPropertySync
    {
        readonly SyncValueAttributeInfo<T> _syncValueAttributeInfo;

        T _lastSentValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySyncBase{T}"/> class.
        /// </summary>
        /// <param name="syncValueAttributeInfo">The <see cref="SyncValueAttributeInfo"/>.</param>
        protected PropertySyncBase(SyncValueAttributeInfo syncValueAttributeInfo)
        {
            _syncValueAttributeInfo = (SyncValueAttributeInfo<T>)syncValueAttributeInfo;
        }

        /// <summary>
        /// Gets the Type that this PropertySync handles.
        /// </summary>
        public Type HandledType
        {
            get { return typeof(T); }
        }

        /// <summary>
        /// Gets the IPropertyInterface used to read and write to the property.
        /// </summary>
        public IPropertyInterface<object, T> PropertyInterface
        {
            get { return _syncValueAttributeInfo.PropertyInterface; }
        }

        T GetValue(object binder)
        {
            return PropertyInterface.Get(binder);
        }

        /// <summary>
        /// Exposes the Read method internally.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="reader"><see cref="IValueReader"/> to read from.</param>
        /// <returns>Value read from the <see cref="IValueReader"/>.</returns>
        internal T InternalRead(string name, IValueReader reader)
        {
            return Read(name, reader);
        }

        /// <summary>
        /// Exposes the Write method internally.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="writer"><see cref="IValueWriter"/> to write to.</param>
        /// <param name="value">Value to write.</param>
        protected internal void InternalWrite(string name, IValueWriter writer, T value)
        {
            Write(name, writer, value);
        }

        /// <summary>
        /// When overridden in the derived class, reads a value with the specified name from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="reader"><see cref="IValueReader"/> to read from.</param>
        /// <returns>Value read from the <see cref="IValueReader"/>.</returns>
        protected abstract T Read(string name, IValueReader reader);

        void SetValue(object binder, T value)
        {
            PropertyInterface.Set(binder, value);
        }

        /// <summary>
        /// When overridden in the derived class, writes a value to an <see cref="IValueWriter"/> with the specified name.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="writer"><see cref="IValueWriter"/> to write to.</param>
        /// <param name="value">Value to write.</param>
        protected abstract void Write(string name, IValueWriter writer, T value);

        #region IPropertySync Members

        /// <summary>
        /// Gets the name of the synchronized value. This is what populates the Name parameter of the
        /// <see cref="IValueReader"/> and <see cref="IValueWriter"/> functions.
        /// </summary>
        public string Name
        {
            get { return _syncValueAttributeInfo.Name; }
        }

        /// <summary>
        /// Gets if this property should be skipped when synchronizing over the network.
        /// </summary>
        public bool SkipNetworkSync
        {
            get { return _syncValueAttributeInfo.SkipNetworkSync; }
        }

        /// <summary>
        /// Gets if the property's value has changed and needs to be re-synchronized.
        /// </summary>
        /// <param name="binder">The object to bind to to get the property value from or set the property value on.</param>
        /// <returns>
        /// True if the <see cref="IPropertySync"/> needs to be re-synchronized; otherwise false.
        /// </returns>
        public bool HasValueChanged(object binder)
        {
            return !Equals(_lastSentValue, PropertyInterface.Get(binder));
        }

        /// <summary>
        /// Reads the property's value from an <see cref="IValueReader"/> and updates the property's value with the
        /// value read.
        /// </summary>
        /// <param name="binder">The object to bind to to get the property value from or set the property value on.</param>
        /// <param name="reader"><see cref="IValueReader"/> to read the property's new value from.</param>
        public void ReadValue(object binder, IValueReader reader)
        {
            var value = Read(Name, reader);
            SetValue(binder, value);
        }

        /// <summary>
        /// Writes the property's value to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="binder">The object to bind to to get the property value from or set the property value on.</param>
        /// <param name="writer"><see cref="IValueWriter"/> to write the property's value to.</param>
        public void WriteValue(object binder, IValueWriter writer)
        {
            var value = GetValue(binder);
            Write(Name, writer, value);
            _lastSentValue = value;
        }

        #endregion
    }
}