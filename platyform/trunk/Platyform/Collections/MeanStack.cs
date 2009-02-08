using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Platyform.Extensions;

namespace Platyform
{
    /// <summary>
    /// Delegate for finding the mean of an IEnumerable of values.
    /// </summary>
    /// <typeparam name="T">Type of values to work with.</typeparam>
    /// <param name="values">IEnumerable of values to find the mean of.</param>
    /// <returns>Mean of all the values in the <paramref name="values"/> array.</returns>
    public delegate T MeanFinder<T>(IEnumerable<T> values);

    /// <summary>
    /// Contains a collection of methods for finding the mean of different types.
    /// </summary>
    public static class MeanStack
    {
        /// <summary>
        /// Cache of MeanFinders where the key is the type of MeanFinder to handle.
        /// </summary>
        static readonly Dictionary<Type, Delegate> _meanFinders = new Dictionary<Type, Delegate>();

        /// <summary>
        /// Gets the MeanFinder for the specified type.
        /// </summary>
        /// <typeparam name="T">Type to handle the mean for.</typeparam>
        /// <returns>MeanFinder for the specified type.</returns>
        public static MeanFinder<T> GetMeanFinder<T>()
        {
            // Try to get the method from the dictionary cache first
            Delegate del;
            if (_meanFinders.TryGetValue(typeof(T), out del))
                return (MeanFinder<T>)del;

            // Find the method to handle the type
            MethodInfo method = typeof(MeanStack).GetMethod("Mean", new Type[] { typeof(IEnumerable<T>) });

            // Ensure we got a valid method
            if (method == null)
                throw new MissingMethodException(string.Format("No MeanFinder found for type {0}.", typeof(T)));

            // Create the delegate
            del = Delegate.CreateDelegate(typeof(MeanFinder<T>), method);
            var mf = (MeanFinder<T>)del;

            // Add the finder to the dictionary for quick access later
            _meanFinders.Add(typeof(T), mf);

            return mf;
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static int Mean(IEnumerable<int> values)
        {
            int sum = 0;
            int count = 0;

            foreach (int v in values)
            {
                sum += v;
                count++;
            }

            return sum / count;
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static uint Mean(IEnumerable<uint> values)
        {
            uint sum = 0;
            uint count = 0;

            foreach (uint v in values)
            {
                sum += v;
                count++;
            }

            return sum / count;
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static Vector2 Mean(IEnumerable<Vector2> values)
        {
            Vector2 sum = Vector2.Zero;
            int count = 0;

            foreach (Vector2 v in values)
            {
                sum += v;
                count++;
            }

            return sum / count;
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static ushort Mean(IEnumerable<ushort> values)
        {
            int sum = 0;
            int count = 0;

            foreach (ushort v in values)
            {
                sum += v;
                count++;
            }

            return (ushort)(sum / count);
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static short Mean(IEnumerable<short> values)
        {
            int sum = 0;
            int count = 0;

            foreach (short v in values)
            {
                sum += v;
                count++;
            }

            return (short)(sum / count);
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static byte Mean(IEnumerable<byte> values)
        {
            int sum = 0;
            int count = 0;

            foreach (byte v in values)
            {
                sum += v;
                count++;
            }

            return (byte)(sum / count);
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static sbyte Mean(IEnumerable<sbyte> values)
        {
            int sum = 0;
            int count = 0;

            foreach (sbyte v in values)
            {
                sum += v;
                count++;
            }

            return (sbyte)(sum / count);
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static float Mean(IEnumerable<float> values)
        {
            float sum = 0;
            int count = 0;

            foreach (float v in values)
            {
                sum += v;
                count++;
            }

            return sum / count;
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static double Mean(IEnumerable<double> values)
        {
            double sum = 0;
            int count = 0;

            foreach (double v in values)
            {
                sum += v;
                count++;
            }

            return sum / count;
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static long Mean(IEnumerable<long> values)
        {
            long sum = 0;
            int count = 0;

            foreach (long v in values)
            {
                sum += v;
                count++;
            }

            return sum / count;
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static ulong Mean(IEnumerable<ulong> values)
        {
            ulong sum = 0;
            ulong count = 0;

            foreach (ulong v in values)
            {
                sum += v;
                count++;
            }

            return sum / count;
        }
    }

    /// <summary>
    /// Write-only stack used to find the mean of a small collection of values
    /// </summary>
    /// <typeparam name="T">Type of value to find the mean of</typeparam>
    public class MeanStack<T>
    {
        /// <summary>
        /// Buffer containing all the values to find the mean of
        /// </summary>
        readonly T[] _buffer;

        /// <summary>
        /// Delegate used to find the mean of the values for type T
        /// </summary>
        readonly MeanFinder<T> _mf;

        /// <summary>
        /// MeanStack constructor.
        /// </summary>
        /// <param name="length">Length of the buffer. Defines the number of values this
        /// stack will use for finding the mean.</param>
        /// <param name="mf">Delegate to find the mean of the specified type.</param>
        public MeanStack(int length, MeanFinder<T> mf)
        {
            _buffer = new T[length];
            _mf = mf;
        }

        /// <summary>
        /// MeanStack constructor.
        /// </summary>
        /// <param name="length">Length of the buffer. Defines the number of values this
        /// stack will use for finding the mean.</param>
        public MeanStack(int length)
        {
            _buffer = new T[length];
            _mf = MeanStack.GetMeanFinder<T>();
        }

        /// <summary>
        /// Fills the stack with the specified value
        /// </summary>
        /// <param name="value">Value to fill the stack with</param>
        public void Fill(T value)
        {
            for (int i = 0; i < _buffer.Length; i++)
            {
                _buffer[i] = value;
            }
        }

        /// <summary>
        /// Finds the mean of the values in the stack
        /// </summary>
        /// <returns>Mean of the values in the stack</returns>
        public T Mean()
        {
            return _mf(_buffer);
        }

        /// <summary>
        /// Pushes a value into the stack
        /// </summary>
        /// <param name="value">Value to push into the stack</param>
        public void Push(T value)
        {
            // Shift up all existing values, dropping the last one
            for (int i = _buffer.Length - 1; i > 0; i--)
            {
                _buffer[i] = _buffer[i - 1];
            }

            // Store the new value in the first index
            _buffer[0] = value;
        }
    }
}