using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// Delegate for finding the mean of an IEnumerable of values.
    /// </summary>
    /// <typeparam name="T">Type of values to work with.</typeparam>
    /// <param name="values">Array of values to find the mean of.</param>
    /// <param name="offset">Array offset to start at.</param>
    /// <param name="count">Number of elements in the array to use.</param>
    /// <returns>Mean of all the values in the <paramref name="values"/> array.</returns>
    public delegate T MeanFinder<T>(T[] values, int offset, int count);

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
            MethodInfo method = typeof(MeanStack).GetMethod("Mean", new Type[] { typeof(T[]), typeof(int), typeof(int) });

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
        /// <param name="offset">Array offset to start at.</param>
        /// <param name="count">Number of elements in the array to use.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static int Mean(int[] values, int offset, int count)
        {
            if (count == 0)
                return 0;

            int sum = 0;

            for (int i = offset; i < offset + count; i++)
            {
                sum += values[i];
            }

            return sum / count;
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <param name="offset">Array offset to start at.</param>
        /// <param name="count">Number of elements in the array to use.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static uint Mean(uint[] values, int offset, int count)
        {
            if (count == 0)
                return 0;

            uint sum = 0;

            for (int i = offset; i < offset + count; i++)
            {
                sum += values[i];
            }

            return sum / (uint)count;
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <param name="offset">Array offset to start at.</param>
        /// <param name="count">Number of elements in the array to use.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static Vector2 Mean(Vector2[] values, int offset, int count)
        {
            if (count == 0)
                return Vector2.Zero;

            Vector2 sum = Vector2.Zero;

            for (int i = offset; i < offset + count; i++)
            {
                sum += values[i];
            }

            return sum / count;
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <param name="offset">Array offset to start at.</param>
        /// <param name="count">Number of elements in the array to use.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static ushort Mean(ushort[] values, int offset, int count)
        {
            if (count == 0)
                return 0;

            int sum = 0;

            for (int i = offset; i < offset + count; i++)
            {
                sum += values[i];
            }

            return (ushort)(sum / count);
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <param name="offset">Array offset to start at.</param>
        /// <param name="count">Number of elements in the array to use.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static short Mean(short[] values, int offset, int count)
        {
            if (count == 0)
                return 0;

            int sum = 0;

            for (int i = offset; i < offset + count; i++)
            {
                sum += values[i];
            }

            return (short)(sum / count);
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <param name="offset">Array offset to start at.</param>
        /// <param name="count">Number of elements in the array to use.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static byte Mean(byte[] values, int offset, int count)
        {
            if (count == 0)
                return 0;

            int sum = 0;

            for (int i = offset; i < offset + count; i++)
            {
                sum += values[i];
            }

            return (byte)(sum / count);
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <param name="offset">Array offset to start at.</param>
        /// <param name="count">Number of elements in the array to use.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static sbyte Mean(sbyte[] values, int offset, int count)
        {
            if (count == 0)
                return 0;

            int sum = 0;

            for (int i = offset; i < offset + count; i++)
            {
                sum += values[i];
            }

            return (sbyte)(sum / count);
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <param name="offset">Array offset to start at.</param>
        /// <param name="count">Number of elements in the array to use.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static float Mean(float[] values, int offset, int count)
        {
            if (count == 0)
                return 0;

            float sum = 0;

            for (int i = offset; i < offset + count; i++)
            {
                sum += values[i];
            }

            return sum / count;
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <param name="offset">Array offset to start at.</param>
        /// <param name="count">Number of elements in the array to use.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static double Mean(double[] values, int offset, int count)
        {
            if (count == 0)
                return 0;

            double sum = 0;

            for (int i = offset; i < offset + count; i++)
            {
                sum += values[i];
            }

            return sum / count;
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <param name="offset">Array offset to start at.</param>
        /// <param name="count">Number of elements in the array to use.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static long Mean(long[] values, int offset, int count)
        {
            if (count == 0)
                return 0;

            long sum = 0;

            for (int i = offset; i < offset + count; i++)
            {
                sum += values[i];
            }

            return sum / count;
        }

        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <param name="offset">Array offset to start at.</param>
        /// <param name="count">Number of elements in the array to use.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static ulong Mean(ulong[] values, int offset, int count)
        {
            if (count == 0)
                return 0;

            ulong sum = 0;

            for (int i = offset; i < offset + count; i++)
            {
                sum += values[i];
            }

            return sum / (ulong)count;
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
        /// Highest index of the buffer used.
        /// </summary>
        int _high = -1;

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
        /// Fills the stack with the specified value.
        /// </summary>
        /// <param name="value">Value to fill the stack with.</param>
        public void Fill(T value)
        {
            for (int i = 0; i < _buffer.Length; i++)
            {
                _buffer[i] = value;
            }
            _high = _buffer.Length - 1;
        }

        /// <summary>
        /// Finds the mean of the values in the stack.
        /// </summary>
        /// <returns>Mean of the values in the stack.</returns>
        public T Mean()
        {
            // If we have nothing, return the default value
            if (_high == -1)
                return default(T);

            // Return the mean
            return _mf(_buffer, 0, _high + 1);
        }

        /// <summary>
        /// Pushes a value into the stack
        /// </summary>
        /// <param name="value">Value to push into the stack</param>
        public void Push(T value)
        {
            // Increase the highest value used
            if (_high < _buffer.Length - 1)
                _high++;

            // Shift up all existing values, dropping the last one
            for (int i = _high; i > 0; i--)
                _buffer[i] = _buffer[i - 1];

            // Store the new value in the first index
            _buffer[0] = value;
        }
    }
}