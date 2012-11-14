using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetGore
{
    /// <summary>
    /// Parses complex math strings and returns the resulting value of the equation. Supports operations
    /// +,-,*,/,^ along with parenthesis for both order of operations and functions. Built-in functions 
    /// support most all static methods found in the System.Math class. Warrenty void if seal is broken.
    /// </summary>
    public class MathString
    {
        /// <summary>
        /// Regex string for numbers
        /// </summary>
        const string _nums = "-*[0-9.]";

        /// <summary>
        /// Regex string for all operators
        /// </summary>
        const string _ops = _opsMD + _opsA + _opsE;

        /// <summary>
        /// Regex string for addition operator
        /// </summary>
        const string _opsA = @"\+";

        /// <summary>
        /// Regex string for the exponent operator
        /// </summary>
        const string _opsE = @"\^";

        /// <summary>
        /// Regex string for division and multiplication operators
        /// </summary>
        const string _opsMD = @"\/\*";

        /// <summary>
        /// Regex constructor options
        /// </summary>
        const RegexOptions _regexOptions = RegexOptions.Compiled | RegexOptions.Singleline;

        /// <summary>
        /// Regex for finding the function (name included) (ie abs(a+b))
        /// </summary>
        static readonly Regex _function = new Regex(@"\w*\([^\(\)]*\)", _regexOptions);

        /// <summary>
        /// Regex for finding a number next to a parenthesis (ie 5(1+2) or (1+2)5)
        /// </summary>
        static readonly Regex _multPar = new Regex(@"([0-9]\()|(\)[0-9])", _regexOptions);

        /// <summary>
        /// Regex for finding the operator in a given problem
        /// </summary>
        static readonly Regex _opFinder = new Regex(string.Format("[{0}]", _ops), _regexOptions);

        /// <summary>
        /// Regex for addition problems (a+b)
        /// </summary>
        static readonly Regex _operationsA = new Regex(string.Format("{1}+[{0}]{1}+", _opsA, _nums), _regexOptions);

        /// <summary>
        /// Regex for exponent problems (a^b)
        /// </summary>
        static readonly Regex _operationsE = new Regex(string.Format("{1}+[{0}]{1}+", _opsE, _nums), _regexOptions);

        /// <summary>
        /// Regex for multiplication and divison problems (a/b or a*b)
        /// </summary>
        static readonly Regex _operationsMD = new Regex(string.Format("{1}+[{0}]{1}+", _opsMD, _nums), _regexOptions);

        static readonly Regex _repMinusMinus = new Regex(string.Format("{0}[ ]*-[ ]*-[ ]*{0}", "[^" + _ops + "]"), _regexOptions);

        /// <summary>
        /// Regex for finding subtraction operators (while properly ignoring negative values) (ie 5-2, but not -2+5)
        /// </summary>
        static readonly Regex _repSub = new Regex(string.Format("{0}-{0}", "[^" + _ops + "]"), _regexOptions);

        /// <summary>
        /// Dictionary of all the variables
        /// </summary>
        readonly Dictionary<string, double> _variables = new Dictionary<string, double>(2);

        /// <summary>
        /// Gets the IDictionary containing all the variables.
        /// </summary>
        public IDictionary<string, double> Variables
        {
            get { return _variables; }
        }

        /// <summary>
        /// Parses a math string.
        /// </summary>
        /// <param name="text">String to parse.</param>
        /// <param name="variables">Variables found in the string. If using functions, be sure to use
        /// unique variable naming to prevent name conflicts.</param>
        /// <returns>Resulting value of the string as a double.</returns>
        public static double Parse(string text, Dictionary<string, double> variables)
        {
            // Replace the variables with their values
            if (variables != null)
            {
                foreach (var variable in variables.Keys)
                {
                    text = text.Replace(variable, variables[variable].ToString());
                }
            }

            // Remove spaces
            text = text.Replace(" ", string.Empty);

            // Add multiply operators for numbers next to parenthesis
            Match match;
            while ((match = _multPar.Match(text)).Success)
            {
                // Grab the matched value
                var mv = match.Value;

                // Replace the operations as needed
                mv = mv.Replace("(", "*(");
                mv = mv.Replace(")", ")*");

                // Remove the old text and insert the new
                text = text.Remove(match.Index, match.Length);
                text = text.Insert(match.Index, mv);
            }

            // Replace minus negative with plus
            while ((match = _repMinusMinus.Match(text)).Success)
            {
                // Grab the matched value
                var mv = match.Value;

                // Replace the operations as needed
                mv = mv.Replace("--", "+");

                // Remove the old text and insert the new
                text = text.Remove(match.Index, match.Length);
                text = text.Insert(match.Index, mv);
            }

            // Replace subtraction with "plus-negative"
            while ((match = _repSub.Match(text)).Success)
            {
                // Grab the matched value
                var mv = match.Value;

                // Replace the operations as needed
                mv = mv.Replace("-", "+-");

                // Remove the old text and insert the new
                text = text.Remove(match.Index, match.Length);
                text = text.Insert(match.Index, mv);
            }

            return ParseSubString(text, null);
        }

        /// <summary>
        /// Parses a math string
        /// </summary>
        /// <param name="text">String to parse</param>
        /// <returns>Resulting value of the string as a double</returns>
        public double Parse(string text)
        {
            return Parse(text, _variables);
        }

        /// <summary>
        /// Performs a function given its name
        /// </summary>
        /// <param name="value">Value to apply to the function</param>
        /// <param name="function">Name of the function</param>
        /// <returns>Resulting value of the function as a double</returns>
        /// <exception cref="ArgumentException"><paramref name="function"/> is not a defined math string operator.</exception>
        static double ParseFunction(double value, string function)
        {
            function = function.ToUpperInvariant();

            switch (function)
            {
                case "ABS":
                    return Math.Abs(value);
                case "ACOS":
                    return Math.Acos(value);
                case "ASIN":
                    return Math.Asin(value);
                case "ATAN":
                    return Math.Atan(value);
                case "CEIL":
                case "CEILING":
                    return Math.Ceiling(value);
                case "COS":
                    return Math.Cos(value);
                case "COSH":
                    return Math.Cosh(value);
                case "EXP":
                    return Math.Exp(value);
                case "FLOOR":
                    return Math.Floor(value);
                case "LOG":
                    return Math.Log10(value);
                case "ROUND":
                    return Math.Round(value, 0);
                case "SIGN":
                    return Math.Sign(value);
                case "SIN":
                    return Math.Sin(value);
                case "SINH":
                    return Math.Sinh(value);
                case "SQRT":
                    return Math.Sqrt(value);
                case "TAN":
                    return Math.Tan(value);
                case "TANH":
                    return Math.Tanh(value);
                default:
                    throw new ArgumentException(string.Format("Unrecognized function '{0}'", function), function);
            }
        }

        /// <summary>
        /// Parses an individual operation for a given problem
        /// </summary>
        /// <param name="text">String for the problem to parse (in format a op b, where op is any operator, such
        /// as a+b or a*b)</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The operator could not be found in the <paramref name="text"/>.</exception>
        static double ParseOperation(string text)
        {
            // Find the operator used in the problem
            var match = _opFinder.Match(text);
            if (!match.Success)
                throw new ArgumentException(string.Format("Failed to find operator in the text '{0}'", text), "text");

            var op = match.Value;

            // Find the values (should only be 2 - left and right side of the operator)
            var values = text.Split(new[] { op }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length != 2)
                throw new ArgumentException(string.Format("Failed to acquire values in the text '{0}'", text), "text");

            var v1 = double.Parse(values[0]);
            var v2 = double.Parse(values[1]);

            // Perform the operation and return the result
            switch (op)
            {
                case "+":
                    return v1 + v2;
                case "-":
                    return v1 - v2;
                case "*":
                    return v1 * v2;
                case "/":
                    return v1 / v2;
                case "^":
                    return Math.Pow(v1, v2);
                default:
                    throw new ArgumentException(string.Format("Unrecognized operator '{0}'", op), "text");
            }
        }

        /// <summary>
        /// Parses all of the specified operations for a given string
        /// </summary>
        /// <param name="text">String to parse</param>
        /// <param name="operations">Regex for the operations</param>
        /// <returns>Resulting value of the function as a double</returns>
        static string ParseOperations(string text, Regex operations)
        {
            // Iterate through the operations
            Match match;
            while ((match = operations.Match(text)).Success)
            {
                // Get the resulting value of the operation
                var result = ParseOperation(match.Value);

                // Replace the operation text with the result vlaue
                text = text.Remove(match.Index, match.Length);
                text = text.Insert(match.Index, result.ToString());
            }
            return text;
        }

        /// <summary>
        /// Recursively parses a math string and all of its sub-strings (functions)
        /// </summary>
        /// <param name="text">Math string to parse</param>
        /// <param name="function">Current function name, if any (null for none)</param>
        /// <returns>Resulting value of the sub-string as a double</returns>
        static double ParseSubString(string text, string function)
        {
            // Functions
            Match match;
            while ((match = _function.Match(text)).Success)
            {
                var t = match.Value;

                // Get the function name, if any, and cut it out
                var f = t.Substring(0, t.IndexOf("(", StringComparison.Ordinal));
                t = t.Remove(0, f.Length);

                // Remove the first and last paranthesis
                t = t.Remove(t.IndexOf("(", StringComparison.Ordinal), 1);
                t = t.Remove(t.LastIndexOf(")", StringComparison.Ordinal), 1);

                // Find the result of the function
                var result = ParseSubString(t, f);

                // Replace the function with the value
                text = text.Remove(match.Index, match.Length);
                text = text.Insert(match.Index, result.ToString());
            }

            // Operations
            text = ParseOperations(text, _operationsE);
            text = ParseOperations(text, _operationsMD);
            text = ParseOperations(text, _operationsA);

            double value = 0;
            try
            {
                value = double.Parse(text);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            // Functions
            if (!string.IsNullOrEmpty(function))
                value = ParseFunction(value, function);

            return value;
        }
    }
}