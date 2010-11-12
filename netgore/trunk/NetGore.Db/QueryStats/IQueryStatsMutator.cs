using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Interface for an object that can mutate the values of an <see cref="IQueryStats"/>.
    /// </summary>
    public interface IQueryStatsMutator
    {
        /// <summary>
        /// Increments the execution count for the query.
        /// </summary>
        void NotifyExecuted();
    }
}