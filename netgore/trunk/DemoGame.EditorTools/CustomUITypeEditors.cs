using System.ComponentModel;
using System.Drawing.Design;
using NetGore.Db;

namespace DemoGame.EditorTools
{
    /// <summary>
    /// Helper methods for the custom <see cref="UITypeEditor"/>s.
    /// </summary>
    public static class CustomUITypeEditors
    {
        static IDbController _dbController;

        /// <summary>
        /// Gets the <see cref="IDbController"/> that was used when calling AddEditors.
        /// </summary>
        internal static IDbController DbController
        {
            get { return _dbController; }
        }

        /// <summary>
        /// Adds all of the custom <see cref="UITypeEditor"/>s.
        /// </summary>
        /// <param name="dbController">The <see cref="IDbController"/>.</param>
        public static void AddEditors(IDbController dbController)
        {
            _dbController = dbController;

            TypeDescriptor.AddAttributes(typeof(CharacterTemplateID),
                                         new EditorAttribute(typeof(CharacterTemplateIDEditor), typeof(UITypeEditor)));
        }
    }
}