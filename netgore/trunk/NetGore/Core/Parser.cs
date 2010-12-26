using System;
using System.Globalization;
using System.Linq;
using NetGore.Collections;

// ReSharper disable MemberCanBeMadeStatic.Global

namespace NetGore
{
    /// <summary>
    /// Provides parsing for values while forcing them to use the specified culture.
    /// </summary>
    public class Parser
    {
        const NumberStyles _nsByte = NumberStyles.Integer;
        const DateTimeStyles _nsDateTime = DateTimeStyles.None;
        const NumberStyles _nsDecimal = NumberStyles.Number;
        const NumberStyles _nsDouble = NumberStyles.Float | NumberStyles.AllowThousands;
        const NumberStyles _nsFloat = NumberStyles.Float | NumberStyles.AllowThousands;
        const NumberStyles _nsInt = NumberStyles.Integer;
        const NumberStyles _nsLong = NumberStyles.Integer;
        const NumberStyles _nsSByte = NumberStyles.Integer;
        const NumberStyles _nsShort = NumberStyles.Integer;
        const NumberStyles _nsUInt = NumberStyles.Integer;
        const NumberStyles _nsULong = NumberStyles.Integer;
        const NumberStyles _nsUShort = NumberStyles.Integer;

        /// <summary>
        /// The cache of <see cref="Parser"/>s for <see cref="CultureInfo"/>s.
        /// </summary>
        static readonly ICache<CultureInfo, Parser> _parserCache =
            new ThreadSafeHashCache<CultureInfo, Parser>(x => new Parser(x, x.NumberFormat, x.DateTimeFormat));

        static readonly Parser _parserCurrent;
        static readonly Parser _parserInvariant;

        readonly CultureInfo _culture;
        readonly DateTimeFormatInfo _dateTimeInfo;
        readonly NumberFormatInfo _info;

        /// <summary>
        /// Initializes the <see cref="Parser"/> class.
        /// </summary>
        static Parser()
        {
            _parserCurrent = new Parser(CultureInfo.CurrentCulture, NumberFormatInfo.CurrentInfo, DateTimeFormatInfo.CurrentInfo);
            _parserInvariant = new Parser(CultureInfo.InvariantCulture, NumberFormatInfo.InvariantInfo,
                DateTimeFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parser"/> class.
        /// </summary>
        /// <param name="culture">The culture information.</param>
        /// <param name="info">The number format information.</param>
        /// <param name="dateTimeInfo">The date and time format information.</param>
        /// <exception cref="ArgumentNullException"><paramref name="culture" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="info" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="dateTimeInfo" /> is <c>null</c>.</exception>
        Parser(CultureInfo culture, NumberFormatInfo info, DateTimeFormatInfo dateTimeInfo)
        {
            if (culture == null)
                throw new ArgumentNullException("culture");
            if (info == null)
                throw new ArgumentNullException("info");
            if (dateTimeInfo == null)
                throw new ArgumentNullException("dateTimeInfo");

            _culture = culture;
            _info = info;
            _dateTimeInfo = dateTimeInfo;
        }

        /// <summary>
        /// Gets the CultureInfo used for this Parser.
        /// </summary>
        public CultureInfo CultureInfo
        {
            get { return _culture; }
        }

        /// <summary>
        /// Gets the Parser for the current culture.
        /// </summary>
        public static Parser Current
        {
            get { return _parserCurrent; }
        }

        /// <summary>
        /// Gets the Parser for an invariant culture.
        /// </summary>
        public static Parser Invariant
        {
            get { return _parserInvariant; }
        }

        /// <summary>
        /// Gets the NumberFormatInfo used for this Parser.
        /// </summary>
        public NumberFormatInfo NumberFormatInfo
        {
            get { return _info; }
        }

        /// <summary>
        /// Gets the <see cref="Parser"/> for a given <see cref="CultureInfo"/>.
        /// </summary>
        /// <param name="ci">The <see cref="CultureInfo"/> to get the <see cref="Parser"/> for.</param>
        /// <returns>A <see cref="Parser"/> for the given <paramref name="ci"/>. If the <paramref name="ci"/> is null, returns the
        /// <see cref="Parser"/> for the invariant culture.</returns>
        public static Parser FromCulture(CultureInfo ci)
        {
            if (ci == null || ci == CultureInfo.InvariantCulture)
                return Invariant;

            if (ci == CultureInfo.CurrentCulture)
                return Current;

            return _parserCache[ci];
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The <paramref name="s"/> parsed to the desired type.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public bool ParseBool(string s)
        {
            return bool.Parse(s);
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The <paramref name="s"/> parsed to the desired type.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public byte ParseByte(string s)
        {
            return byte.Parse(s, _info);
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The <paramref name="s"/> parsed to the desired type.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public char ParseChar(string s)
        {
            return char.Parse(s);
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The <paramref name="s"/> parsed to the desired type.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public DateTime ParseDateTime(string s)
        {
            return DateTime.Parse(s, _dateTimeInfo);
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="format">A format specifier that defines the required format of <paramref name="s"/>.</param>
        /// <returns>
        /// The <paramref name="s"/> parsed to the desired type.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public DateTime ParseDateTimeExact(string s, string format)
        {
            return DateTime.ParseExact(s, format, _dateTimeInfo);
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="formats">An array of allowable formats of <paramref name="s"/>.</param>
        /// <returns>
        /// The <paramref name="s"/> parsed to the desired type.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public DateTime ParseDateTimeExact(string s, string[] formats)
        {
            return DateTime.ParseExact(s, formats, _dateTimeInfo, _nsDateTime);
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The <paramref name="s"/> parsed to the desired type.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public DateTimeOffset ParseDateTimeOffset(string s)
        {
            return DateTimeOffset.Parse(s, _dateTimeInfo);
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="format">A format specifier that defines the required format of <paramref name="s"/>.</param>
        /// <returns>
        /// The <paramref name="s"/> parsed to the desired type.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public DateTimeOffset ParseDateTimeOffsetExact(string s, string format)
        {
            return DateTimeOffset.ParseExact(s, format, _dateTimeInfo);
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="formats">An array of allowable formats of <paramref name="s"/>.</param>
        /// <returns>
        /// The <paramref name="s"/> parsed to the desired type.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public DateTimeOffset ParseDateTimeOffsetExact(string s, string[] formats)
        {
            return DateTimeOffset.ParseExact(s, formats, _dateTimeInfo, _nsDateTime);
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The <paramref name="s"/> parsed to the desired type.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public decimal ParseDecimal(string s)
        {
            return decimal.Parse(s, _info);
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The <paramref name="s"/> parsed to the desired type.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public double ParseDouble(string s)
        {
            return double.Parse(s, _info);
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The <paramref name="s"/> parsed to the desired type.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public float ParseFloat(string s)
        {
            return float.Parse(s, _info);
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The <paramref name="s"/> parsed to the desired type.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public int ParseInt(string s)
        {
            return int.Parse(s, _info);
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The <paramref name="s"/> parsed to the desired type.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public long ParseLong(string s)
        {
            return long.Parse(s, _info);
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The <paramref name="s"/> parsed to the desired type.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public sbyte ParseSByte(string s)
        {
            return sbyte.Parse(s, _info);
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The <paramref name="s"/> parsed to the desired type.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public short ParseShort(string s)
        {
            return short.Parse(s, _info);
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The <paramref name="s"/> parsed to the desired type.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public TimeSpan ParseTimeSpan(string s)
        {
            return TimeSpan.Parse(s);
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The <paramref name="s"/> parsed to the desired type.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public uint ParseUInt(string s)
        {
            return uint.Parse(s, _info);
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The <paramref name="s"/> parsed to the desired type.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public ulong ParseULong(string s)
        {
            return ulong.Parse(s, _info);
        }

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The <paramref name="s"/> parsed to the desired type.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="System.FormatException"><paramref name="s"/> cannot be parsed to the desired type.</exception>
        public ushort ParseUShort(string s)
        {
            return ushort.Parse(s, _info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(bool value)
        {
            return value.ToString(_info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(byte value)
        {
            return value.ToString(_info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <param name="format">A string that represents the format to convert to.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(byte value, string format)
        {
            return value.ToString(format, _info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(sbyte value)
        {
            return value.ToString(_info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <param name="format">A string that represents the format to convert to.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(sbyte value, string format)
        {
            return value.ToString(format, _info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(short value)
        {
            return value.ToString(_info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <param name="format">A string that represents the format to convert to.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(short value, string format)
        {
            return value.ToString(format, _info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(ushort value)
        {
            return value.ToString(_info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <param name="format">A string that represents the format to convert to.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(ushort value, string format)
        {
            return value.ToString(format, _info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(int value)
        {
            return value.ToString(_info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <param name="format">A string that represents the format to convert to.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(int value, string format)
        {
            return value.ToString(format, _info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(uint value)
        {
            return value.ToString(_info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <param name="format">A string that represents the format to convert to.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(uint value, string format)
        {
            return value.ToString(format, _info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(long value)
        {
            return value.ToString(_info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <param name="format">A string that represents the format to convert to.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(long value, string format)
        {
            return value.ToString(format, _info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(ulong value)
        {
            return value.ToString(_info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <param name="format">A string that represents the format to convert to.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(ulong value, string format)
        {
            return value.ToString(format, _info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(float value)
        {
            return value.ToString(_info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <param name="format">A string that represents the format to convert to.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(float value, string format)
        {
            return value.ToString(format, _info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(double value)
        {
            return value.ToString(_info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <param name="format">A string that represents the format to convert to.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(double value, string format)
        {
            return value.ToString(format, _info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(decimal value)
        {
            return value.ToString(_info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <param name="format">A string that represents the format to convert to.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(decimal value, string format)
        {
            return value.ToString(format, _info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="f">The value to get the string representation for.</param>
        /// <param name="format">A string that represents the format to convert to.</param>
        /// <returns>The string representation of the given <paramref name="f"/>.</returns>
        public string ToString(IFormattable f, string format)
        {
            return f.ToString(format, _info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(DateTime value)
        {
            return value.ToString(_info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <param name="format">A string that represents the format to convert to.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(DateTime value, string format)
        {
            return value.ToString(format, _info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(DateTimeOffset value)
        {
            return value.ToString(_info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <param name="format">A string that represents the format to convert to.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(DateTimeOffset value, string format)
        {
            return value.ToString(format, _info);
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(TimeSpan value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Gets the string representation of the given type and value.
        /// </summary>
        /// <param name="value">The value to get the string representation for.</param>
        /// <returns>The string representation of the given <paramref name="value"/>.</returns>
        public string ToString(char value)
        {
            return value.ToString(_info);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, out bool value)
        {
            return bool.TryParse(s, out value);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, out char value)
        {
            return char.TryParse(s, out value);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, out TimeSpan value)
        {
            return TimeSpan.TryParse(s, out value);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, out byte value)
        {
            return byte.TryParse(s, _nsByte, _info, out value);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, out DateTime value)
        {
            return DateTime.TryParse(s, _dateTimeInfo, _nsDateTime, out value);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="format">A format specifier that defines the required format of <paramref name="s"/>.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, string format, out DateTime value)
        {
            return DateTime.TryParseExact(s, format, _dateTimeInfo, _nsDateTime, out value);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="formats">An array of allowable formats of <paramref name="s"/>.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, string[] formats, out DateTime value)
        {
            return DateTime.TryParseExact(s, formats, _dateTimeInfo, _nsDateTime, out value);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, out DateTimeOffset value)
        {
            return DateTimeOffset.TryParse(s, _dateTimeInfo, _nsDateTime, out value);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="format">A format specifier that defines the required format of <paramref name="s"/>.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, string format, out DateTimeOffset value)
        {
            return DateTimeOffset.TryParseExact(s, format, _dateTimeInfo, _nsDateTime, out value);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="formats">An array of allowable formats of <paramref name="s"/>.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, string[] formats, out DateTimeOffset value)
        {
            return DateTimeOffset.TryParseExact(s, formats, _dateTimeInfo, _nsDateTime, out value);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, out decimal value)
        {
            return decimal.TryParse(s, _nsDecimal, _info, out value);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, out double value)
        {
            return double.TryParse(s, _nsDouble, _info, out value);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, out float value)
        {
            return float.TryParse(s, _nsFloat, _info, out value);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, out int value)
        {
            return int.TryParse(s, _nsInt, _info, out value);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, out long value)
        {
            return long.TryParse(s, _nsLong, _info, out value);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, out sbyte value)
        {
            return sbyte.TryParse(s, _nsSByte, _info, out value);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, out short value)
        {
            return short.TryParse(s, _nsShort, _info, out value);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, out uint value)
        {
            return uint.TryParse(s, _nsUInt, _info, out value);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, out ulong value)
        {
            return ulong.TryParse(s, _nsULong, _info, out value);
        }

        /// <summary>
        /// Converts a string to the specified type and returns a boolean representing if the conversion was successful.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="value">When this method returns true, contains the result of <paramref name="s"/> converted
        /// to the desired type.</param>
        /// <returns>True if value was converted successfully; otherwise, false.</returns>
        public bool TryParse(string s, out ushort value)
        {
            return ushort.TryParse(s, _nsUShort, _info, out value);
        }
    }
}