using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace NetGore
{
    /// <summary>
    /// An immutable implementation of <see cref="EventArgs"/> that holds generic, weakly-named properties.
    /// This should only be used when the purpose of the properties is obvious through the context.
    /// </summary>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    [Serializable]
    public class EventArgs<T1> : EventArgs
    {
        readonly T1 _item1;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T}"/> class.
        /// </summary>
        /// <param name="item1">The value of the first component.</param>
        public EventArgs(T1 item1)
        {
            _item1 = item1;
        }

        /// <summary>
        /// Gets the value of the first component.
        /// </summary>
        public T1 Item1
        {
            get { return _item1; }
        }

        /// <summary>
        /// Gets the string to use for delimiting the items in string built using <see cref="object.ToString()"/>.
        /// </summary>
        protected static string ToStringDelimiter
        {
            get { return ", "; }
        }

        /// <summary>
        /// Gets the string to use for end of the string built using <see cref="object.ToString()"/>.
        /// </summary>
        protected static string ToStringEnd
        {
            get { return ")"; }
        }

        /// <summary>
        /// Gets the string to use for start of the string built using <see cref="object.ToString()"/>.
        /// </summary>
        protected static string ToStringStart
        {
            get { return "("; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(ToStringStart);
            sb.Append(Item1);
            sb.Append(ToStringEnd);
            return sb.ToString();
        }
    }

    /// <summary>
    /// An immutable implementation of <see cref="EventArgs"/> that holds generic, weakly-named properties.
    /// This should only be used when the purpose of the properties is obvious through the context.
    /// </summary>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    /// <typeparam name="T2">The type of the second component.</typeparam>
    [Serializable]
    public class EventArgs<T1, T2> : EventArgs<T1>
    {
        readonly T2 _item2;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T}"/> class.
        /// </summary>
        /// <param name="item1">The value of the first component.</param>
        /// <param name="item2">The value of the second component.</param>
        public EventArgs(T1 item1, T2 item2) : base(item1)
        {
            _item2 = item2;
        }

        /// <summary>
        /// Gets the value of the second component.
        /// </summary>
        public T2 Item2
        {
            get { return _item2; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(ToStringStart);
            sb.Append(Item1);
            sb.Append(ToStringDelimiter);
            sb.Append(Item2);
            sb.Append(ToStringEnd);
            return sb.ToString();
        }
    }

    /// <summary>
    /// An immutable implementation of <see cref="EventArgs"/> that holds generic, weakly-named properties.
    /// This should only be used when the purpose of the properties is obvious through the context.
    /// </summary>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    /// <typeparam name="T2">The type of the second component.</typeparam>
    /// <typeparam name="T3">The type of the third component.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    [Serializable]
    public class EventArgs<T1, T2, T3> : EventArgs<T1, T2>
    {
        readonly T3 _item3;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T}"/> class.
        /// </summary>
        /// <param name="item1">The value of the first component.</param>
        /// <param name="item2">The value of the second component.</param>
        /// <param name="item3">The value of the third component.</param>
        public EventArgs(T1 item1, T2 item2, T3 item3) : base(item1, item2)
        {
            _item3 = item3;
        }

        /// <summary>
        /// Gets the value of the third component.
        /// </summary>
        public T3 Item3
        {
            get { return _item3; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(ToStringStart);
            sb.Append(Item1);
            sb.Append(ToStringDelimiter);
            sb.Append(Item2);
            sb.Append(ToStringDelimiter);
            sb.Append(Item3);
            sb.Append(ToStringEnd);
            return sb.ToString();
        }
    }

    /// <summary>
    /// An immutable implementation of <see cref="EventArgs"/> that holds generic, weakly-named properties.
    /// This should only be used when the purpose of the properties is obvious through the context.
    /// </summary>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    /// <typeparam name="T2">The type of the second component.</typeparam>
    /// <typeparam name="T3">The type of the third component.</typeparam>
    /// <typeparam name="T4">The type of the fourth component.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    [Serializable]
    public class EventArgs<T1, T2, T3, T4> : EventArgs<T1, T2, T3>
    {
        readonly T4 _item4;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T}"/> class.
        /// </summary>
        /// <param name="item1">The value of the first component.</param>
        /// <param name="item2">The value of the second component.</param>
        /// <param name="item3">The value of the third component.</param>
        /// <param name="item4">The value of the fourth component.</param>
        public EventArgs(T1 item1, T2 item2, T3 item3, T4 item4) : base(item1, item2, item3)
        {
            _item4 = item4;
        }

        /// <summary>
        /// Gets the value of the fourth component.
        /// </summary>
        public T4 Item4
        {
            get { return _item4; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(ToStringStart);
            sb.Append(Item1);
            sb.Append(ToStringDelimiter);
            sb.Append(Item2);
            sb.Append(ToStringDelimiter);
            sb.Append(Item3);
            sb.Append(ToStringDelimiter);
            sb.Append(Item4);
            sb.Append(ToStringEnd);
            return sb.ToString();
        }
    }

    /// <summary>
    /// An immutable implementation of <see cref="EventArgs"/> that holds generic, weakly-named properties.
    /// This should only be used when the purpose of the properties is obvious through the context.
    /// </summary>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    /// <typeparam name="T2">The type of the second component.</typeparam>
    /// <typeparam name="T3">The type of the third component.</typeparam>
    /// <typeparam name="T4">The type of the fourth component.</typeparam>
    /// <typeparam name="T5">The type of the fifth component.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    [Serializable]
    public class EventArgs<T1, T2, T3, T4, T5> : EventArgs<T1, T2, T3, T4>
    {
        readonly T5 _item5;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T}"/> class.
        /// </summary>
        /// <param name="item1">The value of the first component.</param>
        /// <param name="item2">The value of the second component.</param>
        /// <param name="item3">The value of the third component.</param>
        /// <param name="item4">The value of the fourth component.</param>
        /// <param name="item5">The value of the fifth component.</param>
        public EventArgs(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) : base(item1, item2, item3, item4)
        {
            _item5 = item5;
        }

        /// <summary>
        /// Gets the value of the fifth component.
        /// </summary>
        public T5 Item5
        {
            get { return _item5; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(ToStringStart);
            sb.Append(Item1);
            sb.Append(ToStringDelimiter);
            sb.Append(Item2);
            sb.Append(ToStringDelimiter);
            sb.Append(Item3);
            sb.Append(ToStringDelimiter);
            sb.Append(Item4);
            sb.Append(ToStringDelimiter);
            sb.Append(Item5);
            sb.Append(ToStringEnd);
            return sb.ToString();
        }
    }
}