using System;
using System.Linq;
using DemoGame.Server.Queries;
using NetGore.Db;

namespace DemoGame.Server
{
    public static class CharacterTemplateIDExtensions
    {
        /// <summary>
        /// Checks if the CharacterTemplate with the given CharacterTemplateID exists in the database.
        /// </summary>
        /// <param name="id">CharacterTemplateID to check.</param>
        /// <returns>True if a CharacterTemplate with the given id exists; otherwise false.</returns>
        public static bool TemplateExists(this CharacterTemplateID id)
        {
            IDbController dbController = DbControllerBase.GetInstance();
            var query = dbController.GetQuery<SelectCharacterTemplateQuery>();

            try
            {
                var result = query.Execute(id);
                if (result == null)
                    return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }
    }
}