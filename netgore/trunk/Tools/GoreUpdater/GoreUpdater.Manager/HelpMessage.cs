using System.Linq;

namespace GoreUpdater.Manager
{
    /// <summary>
    /// Describes the title and message body of a help message.
    /// </summary>
    public struct HelpMessage
    {
        readonly string _message;
        readonly string _title;

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpMessage"/> struct.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        public HelpMessage(string title, string message)
        {
            _title = title;
            _message = message;
        }

        /// <summary>
        /// Gets the help message body.
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// Gets the help title.
        /// </summary>
        public string Title
        {
            get { return _title; }
        }
    }
}