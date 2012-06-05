using System.Linq;
using System.Windows.Forms;
using NetGore.IO;

namespace NetGore.Editor.WinForms
{
    /// <summary>
    /// A <see cref="RadioButton"/> that implements <see cref="IPersistable"/>.
    /// </summary>
    public class PersistableRadioButton : RadioButton, IPersistable
    {
        const string _checkedValueKey = "Checked";

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public virtual void ReadState(IValueReader reader)
        {
            Checked = reader.ReadBool(_checkedValueKey);
            PersistableHelper.Read(this, reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public virtual void WriteState(IValueWriter writer)
        {
            writer.Write(_checkedValueKey, Checked);
            PersistableHelper.Write(this, writer);
        }

        #endregion
    }
}