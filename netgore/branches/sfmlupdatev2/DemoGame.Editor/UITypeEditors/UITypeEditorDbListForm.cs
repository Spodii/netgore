using System.Linq;
using NetGore.Db;
using NetGore.Editor.UI;

namespace DemoGame.Editor.UITypeEditors
{
    /// <summary>
    /// A <see cref="UITypeEditorListForm{T}"/> for listing items from the database.
    /// </summary>
    /// <typeparam name="T">The type of item to list.</typeparam>
    public abstract class UITypeEditorDbListForm<T> : UITypeEditorListForm<T>
    {
        /// <summary>
        /// Gets a <see cref="IDbController"/> to use to perform queries.
        /// </summary>
        protected IDbController DbController
        {
            get
            {
                var ret = DbControllerBase.GetInstance();
                if (ret != null)
                    return ret;

                return CustomUITypeEditors.DbController;
            }
        }
    }
}