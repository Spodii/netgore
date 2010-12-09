using System;
using System.Linq;

namespace DemoGame.Client
{
    public class CreateAccountEventArgs : EventArgs
    {
        readonly string _errorMessage;
        readonly bool _successful;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAccountEventArgs"/> class.
        /// </summary>
        /// <param name="successful">If the account was successfully created.</param>
        /// <param name="errorMessage">If <paramref name="successful"/> is false, contains the reason
        /// why the account failed to be created.</param>
        public CreateAccountEventArgs(bool successful, string errorMessage)
        {
            _successful = successful;
            _errorMessage = errorMessage;
        }

        /// <summary>
        /// Gets the the reason why the account failed to be created when <see cref="Successful"/> is false.
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        /// <summary>
        /// Gets if the account was successfully created.
        /// </summary>
        public bool Successful
        {
            get { return _successful; }
        }
    }
}