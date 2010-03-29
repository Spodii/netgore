using System;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace CustomTasks
{
    /// <summary>
    /// A msbuild <see cref="Task"/> that performs a Regex match test.
    /// </summary>
    public class RegexMatchTask : Task
    {
        /// <summary>
        /// Gets or sets the Regex string to use for making the comparison.
        /// </summary>
        [Required]
        public string Regex { get; set; }

        /// <summary>
        /// Gets or sets the value to compare the Regex to.
        /// </summary>
        [Required]
        public string Value { get; set; }

        /// <summary>
        /// Gets the results of the Regex match.
        /// </summary>
        [Output]
        public bool Output { get; private set; }

        /// <summary>
        /// Executes a task.
        /// </summary>
        /// <returns>
        /// true if the task executed successfully; otherwise, false.
        /// </returns>
        public override bool Execute()
        {
            const RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline;
           
            try
            {
                Output = System.Text.RegularExpressions.Regex.IsMatch(Value, Regex, options);
            }
            catch (Exception)
            {
                Output = false;
            }

            return true;
        }
    }
}