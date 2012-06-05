using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Interface for various query functions for an <see cref="IQueryBuilder"/>.
    /// </summary>
    public interface IQueryBuilderFunctions
    {
        /// <summary>
        /// Gets the absolute value of the <paramref name="expr"/>.
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        string Abs(string expr);

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
        /// Performs a logical AND.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string And(string left, string right);

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
        /// Performs a bitwise NOT (bit inversion).
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        string BitNot(string expr);

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
        /// Rounds a value up to the nearest integer.
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        string Ceiling(string expr);

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
        /// Adds two dates together.
        /// </summary>
        /// <param name="date">The SQL string containing the date to add to.</param>
        /// <param name="interval">The SQL string containing the date interval to subtract.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="date"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="interval"/> is null or empty.</exception>
        string DateAddInterval(string date, string interval);

        /// <summary>
        /// Adds two dates together.
        /// </summary>
        /// <param name="date">The SQL string containing the date to add to.</param>
        /// <param name="interval">The type of time unit.</param>
        /// <param name="value">The amount of time.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="date"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="interval"/> is not a defined <see cref="QueryIntervalType"/>
        /// value.</exception>
        string DateAddInterval(string date, QueryIntervalType interval, int value);

        /// <summary>
        /// Adds two dates together.
        /// </summary>
        /// <param name="date">The SQL string containing the date to add to.</param>
        /// <param name="interval">The type of time unit.</param>
        /// <param name="value">The amount of time.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="date"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="interval"/> is not a defined <see cref="QueryIntervalType"/>
        /// value.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null or empty.</exception>
        string DateAddInterval(string date, QueryIntervalType interval, string value);

        /// <summary>
        /// Subtracts two dates.
        /// </summary>
        /// <param name="date">The SQL string containing the date to subtract from.</param>
        /// <param name="interval">The SQL string containing the date interval to add.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="date"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="interval"/> is null or empty.</exception>
        string DateSubtractInterval(string date, string interval);

        /// <summary>
        /// Subtracts two dates.
        /// </summary>
        /// <param name="date">The SQL string containing the date to subtract from.</param>
        /// <param name="interval">The type of time unit.</param>
        /// <param name="value">The amount of time.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="date"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="interval"/> is not a defined <see cref="QueryIntervalType"/>
        /// value.</exception>
        string DateSubtractInterval(string date, QueryIntervalType interval, int value);

        /// <summary>
        /// Subtracts two dates.
        /// </summary>
        /// <param name="date">The SQL string containing the date to subtract from.</param>
        /// <param name="interval">The type of time unit.</param>
        /// <param name="value">The amount of time.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="date"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="interval"/> is not a defined <see cref="QueryIntervalType"/>
        /// value.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null or empty.</exception>
        string DateSubtractInterval(string date, QueryIntervalType interval, string value);

        /// <summary>
        /// Gets the default value for a column.
        /// </summary>
        /// <param name="expr">The SQL expression, typically the column name to get the default value for.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        string Default(string expr);

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
        /// Equality operator.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string Equals(string left, string right);

        /// <summary>
        /// Rounds a value down to the nearest integer.
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        string Floor(string expr);

        /// <summary>
        /// Checks if the left side is greater than or equal to the right.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string GreaterOrEqual(string left, string right);

        /// <summary>
        /// Checks if the left side is greater than the right.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string GreaterThan(string left, string right);

        /// <summary>
        /// Gets an interval of time.
        /// </summary>
        /// <param name="interval">The unit of time.</param>
        /// <param name="value">The amount of time.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="interval"/> is not a defined <see cref="QueryIntervalType"/>
        /// value.</exception>
        string Interval(QueryIntervalType interval, int value);

        /// <summary>
        /// Gets an interval of time.
        /// </summary>
        /// <param name="interval">The unit of time.</param>
        /// <param name="value">The amount of time.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="interval"/> is not a defined <see cref="QueryIntervalType"/>
        /// value.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null or empty.</exception>
        string Interval(QueryIntervalType interval, string value);

        /// <summary>
        /// Tests if an expression is not null.
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        string IsNotNull(string expr);

        /// <summary>
        /// Tests if an expression is null.
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        string IsNull(string expr);

        /// <summary>
        /// Checks if the left side is less than or equal to the right.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string LessOrEqual(string left, string right);

        /// <summary>
        /// Checks if the left side is less than the right.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string LessThan(string left, string right);

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
        /// Multiplies two values.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string Multiply(string left, string right);

        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string NotEqual(string left, string right);

        /// <summary>
        /// Gets the current date and time.
        /// </summary>
        /// <returns>The SQL string for the function.</returns>
        string Now();

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
        /// Subtracts two values.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string Subtract(string left, string right);

        /// <summary>
        /// Performs a logical XOR.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        string Xor(string left, string right);
    }
}