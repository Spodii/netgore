using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Extensions for the System.Collections.Generic.Stack.
    /// </summary>
    public static class StackExtensions
    {
        /// <summary>
        /// Sorts all of the items in a Stack so they will pop in ascending order.
        /// </summary>
        /// <typeparam name="T">Type of item contained in the stack.</typeparam>
        /// <param name="stack">Stack to perform the sort on.</param>
        /// <param name="where">Condition that all items need to include to remain in the Stack.
        /// Every item that fails to meet this condition will end up removed from the Stack completely.</param>
        public static void Sort<T>(this Stack<T> stack, Func<T, bool> where = null)
        {
            // Copy the values to a temporary List
            List<T> list;
            if (where != null)
                list = stack.Where(where).ToList();
            else
                list = stack.ToList();

            // Clear the stack's values
            stack.Clear();

            // Sort the List using the List's built-in search
            list.Sort();

            // Push all the items back into the stack backwards
            foreach (var item in list.Reverse<T>())
            {
                stack.Push(item);
            }
        }

        /// <summary>
        /// Sorts all of the items in a Stack so they will pop in descending order.
        /// </summary>
        /// <typeparam name="T">Type of item contained in the stack.</typeparam>
        /// <param name="stack">Stack to perform the sort on.</param>
        /// <param name="where">Condition that all items need to include to remain in the Stack.
        /// Every item that fails to meet this condition will end up removed from the Stack completely.</param>
        public static void SortDescending<T>(this Stack<T> stack, Func<T, bool> where = null)
        {
            // Copy the values to a temporary List
            List<T> list;
            if (where != null)
                list = stack.Where(where).ToList();
            else
                list = stack.ToList();

            // Clear the stack's values
            stack.Clear();

            // Sort the List using the List's built-in search
            list.Sort();

            // Push all the items back into the stack
            foreach (var item in list)
            {
                stack.Push(item);
            }
        }
    }
}