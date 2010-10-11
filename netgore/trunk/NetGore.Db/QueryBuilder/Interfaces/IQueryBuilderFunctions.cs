using System;
using System.Collections.Generic;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Interface for various query functions for an <see cref="IQueryBuilder"/>.
    /// </summary>
    public interface IQueryBuilderFunctions
    {
        /// <summary>
        /// Counts the number of rows.
        /// </summary>
        /// <returns>The SQL string for the function.</returns>
        string Count();

        /// <summary>
        /// Counts the number of non-null values in the <paramref name="expr"/>.
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        string Count(string expr);

        /// <summary>
        /// Gets the absolute value of the <paramref name="expr"/>.
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        string Abs(string expr);

        /// <summary>
        /// Performs a logical AND.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string And(string left, string right);

        /// <summary>
        /// Performs a logical OR.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string Or(string left, string right);

        /// <summary>
        /// Performs a logical XOR.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string Xor(string left, string right);

        /// <summary>
        /// Performs a bitwise AND.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string BitAnd(string left, string right);

        /// <summary>
        /// Performs a bitwise OR.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string BitOr(string left, string right);

        /// <summary>
        /// Performs a bitwise XOR.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string BitXor(string left, string right);

        /// <summary>
        /// Performs a bitwise NOT (bit inversion).
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        string BitNot(string expr);

        /// <summary>
        /// Rounds a value up to the nearest integer.
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        string Ceiling(string expr);

        /// <summary>
        /// Rounds a value down to the nearest integer.
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        string Floor(string expr);

        /// <summary>
        /// Tests if an expression is null.
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        string IsNull(string expr);

        /// <summary>
        /// Tests if an expression is not null.
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        string IsNotNull(string expr);

        /// <summary>
        /// Returns the first non-null value in a list.
        /// </summary>
        /// <param name="exprs">The SQL expressions, where each array element is an individual argument for the function.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="exprs"/> is null or empty.</exception>
        string Coalesce(IEnumerable<string> exprs);

        /// <summary>
        /// Returns the first non-null value in a list.
        /// </summary>
        /// <param name="exprs">The SQL expressions, where each array element is an individual argument for the function.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="exprs"/> is null or empty.</exception>
        string Coalesce(params string[] exprs);

        /// <summary>
        /// Gets the default value for a column.
        /// </summary>
        /// <param name="expr">The SQL expression, typically the column name to get the default value for.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        string Default(string expr);

        /// <summary>
        /// Adds two values.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string Add(string left, string right);

        /// <summary>
        /// Subtracts two values.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string Subtract(string left, string right);

        /// <summary>
        /// Multiplies two values.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string Multiply(string left, string right);

        /// <summary>
        /// Divides two values.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string Divide(string left, string right);

        /// <summary>
        /// Gets the remainder of a division.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string Mod(string left, string right);

        /// <summary>
        /// Gets the current date and time.
        /// </summary>
        /// <returns>The SQL string for the function.</returns>
        string Now();
    }
}