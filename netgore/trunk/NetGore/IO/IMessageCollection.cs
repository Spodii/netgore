using System.Collections.Generic;
using System.Linq;

namespace NetGore.IO
{
    public interface IMessageCollection<T> : IEnumerable<KeyValuePair<T, string>>
    {
        /// <summary>
        /// Gets the specified message, parsed using the supplied parameters.
        /// </summary>
        /// <param name="id">ID of the message to get.</param>
        /// <param name="args">Parameters used to parse the message.</param>
        /// <returns>Parsed message for the <paramref name="id"/>, or null if the <paramref name="id"/> 
        /// is not found or invalid.</returns>
        string GetMessage(T id, params string[] args);
    }
}