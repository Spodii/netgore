using System.Linq;

namespace DemoGame.Server
{
    /// <summary>
    /// Interface for an object that is to be saved when the server performs routine saving.
    /// </summary>
    public interface IServerSaveable
    {
        /// <summary>
        /// Saves the state of this object and all <see cref="IServerSaveable"/> objects under it to the database.
        /// </summary>
        void ServerSave();
    }
}