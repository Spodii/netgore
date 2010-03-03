using System.Linq;

namespace NetGore.EditorTools
{
    /// <summary>
    /// A <see cref="TypedListBox{T}"/> for a collection of <see cref="WallEntityBase"/>s.
    /// </summary>
    public class WallsListBox : TypedListBox<WallEntityBase>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WallsListBox"/> class.
        /// </summary>
        public WallsListBox()
        {
// ReSharper disable DoNotCallOverridableMethodsInConstructor
            DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
// ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Gets the string to display for an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The string to display.</returns>
        public override string ItemToString(WallEntityBase item)
        {
            return string.Format("({0},{1}) [{2}x{3}]{4}",
                item.Position.X, item.Position.Y, item.Size.X, item.Size.Y,
                item.IsPlatform ? " - Platform" : string.Empty);
        }
    }
}