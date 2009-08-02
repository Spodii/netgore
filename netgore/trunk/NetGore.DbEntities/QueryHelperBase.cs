using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;

namespace NetGore.DbEntities
{
    /// <summary>
    /// A base class that assists in executing queries without all the verbosity, such as with a CompiledQuery.
    /// </summary>
    /// <typeparam name="TObjContext">The type of ObjectContext.</typeparam>
    public abstract class QueryHelperBase<TObjContext> where TObjContext : ObjectContext
    {
        /// <summary>
        /// Makes sure that the <paramref name="value"/> is not of type IQueryable.
        /// </summary>
        /// <param name="value">The object to check.</param>
        static void EnsureNotIQueryable(object value)
        {
            if (value is IQueryable)
            {
                const string errmsg =
                    "Return type cannot be of type IQueryable because the ObjectContext is disposed" +
                    " by the time this method returns!";
                throw new ArgumentException(errmsg);
            }
        }

        /// <summary>
        /// Gets an available ObjectContext of type <typeparamref name="TObjContext"/>.
        /// </summary>
        /// <returns>An available ObjectContext of type <typeparamref name="TObjContext"/>.</returns>
        protected abstract TObjContext GetObjectContext();

        /// <summary>
        /// Assists in invoking a Func created with CompiledQuery.
        /// </summary>
        /// <typeparam name="TRet">The return type.</typeparam>
        /// <param name="func">The Func to invoke.</param>
        /// <returns>The return value of the Func.</returns>
        public TRet Invoke<TRet>(Func<TObjContext, TRet> func)
        {
            EnsureNotIQueryable(func);

            TRet ret;
            using (TObjContext db = GetObjectContext())
            {
                ret = func.Invoke(db);
            }
            return ret;
        }

        /// <summary>
        /// Assists in invoking a Func created with CompiledQuery.
        /// </summary>
        /// <typeparam name="TRet">The return type.</typeparam>
        /// <typeparam name="TArg0">The first argument type.</typeparam>
        /// <param name="func">The Func to invoke.</param>
        /// <param name="arg0">The first argument.</param>
        /// <returns>The return value of the Func.</returns>
        public TRet Invoke<TRet, TArg0>(Func<TObjContext, TArg0, TRet> func, TArg0 arg0)
        {
            EnsureNotIQueryable(func);

            TRet ret;
            using (TObjContext db = GetObjectContext())
            {
                ret = func.Invoke(db, arg0);
            }
            return ret;
        }

        /// <summary>
        /// Assists in invoking a Func created with CompiledQuery.
        /// </summary>
        /// <typeparam name="TRet">The return type.</typeparam>
        /// <typeparam name="TArg0">The first argument type.</typeparam>
        /// <typeparam name="TArg1">The second argument type.</typeparam>
        /// <param name="func">The Func to invoke.</param>
        /// <param name="arg0">The first argument.</param>
        /// <param name="arg1">The second argument.</param>
        /// <returns>The return value of the Func.</returns>
        public TRet Invoke<TRet, TArg0, TArg1>(Func<TObjContext, TArg0, TArg1, TRet> func, TArg0 arg0, TArg1 arg1)
        {
            EnsureNotIQueryable(func);

            TRet ret;
            using (TObjContext db = GetObjectContext())
            {
                ret = func.Invoke(db, arg0, arg1);
            }
            return ret;
        }

        /// <summary>
        /// Assists in invoking a Func created with CompiledQuery.
        /// </summary>
        /// <typeparam name="TRet">The return type.</typeparam>
        /// <typeparam name="TArg0">The first argument type.</typeparam>
        /// <typeparam name="TArg1">The second argument type.</typeparam>
        /// <typeparam name="TArg2">The third argument type.</typeparam>
        /// <param name="func">The Func to invoke.</param>
        /// <param name="arg0">The first argument.</param>
        /// <param name="arg1">The second argument.</param>
        /// <param name="arg2">The third argument.</param>
        /// <returns>The return value of the Func.</returns>
        public TRet Invoke<TRet, TArg0, TArg1, TArg2>(Func<TObjContext, TArg0, TArg1, TArg2, TRet> func, TArg0 arg0, TArg1 arg1,
                                                      TArg2 arg2)
        {
            EnsureNotIQueryable(func);

            TRet ret;
            using (TObjContext db = GetObjectContext())
            {
                ret = func.Invoke(db, arg0, arg1, arg2);
            }
            return ret;
        }

        /// <summary>
        /// Assists in invoking a Func created with CompiledQuery that uses a selector to return a custom type.
        /// </summary>
        /// <typeparam name="TRet">The query return type.</typeparam>
        /// <typeparam name="TSelectRet">The desired return type.</typeparam>
        /// <param name="func">The CompiledQuery Func to invoke.</param>
        /// <param name="selector">The Func used to transform type <typeparamref name="TRet"/>
        /// into <typeparamref name="TSelectRet"/>.</param>
        /// <returns>The return of the <paramref name="func"/> query as type <typeparamref name="TSelectRet"/>.</returns>
        public IEnumerable<TSelectRet> InvokeAndSelectMany<TRet, TSelectRet>(Func<TObjContext, IQueryable<TRet>> func,
                                                                             Func<TRet, TSelectRet> selector)
        {
            IEnumerable<TSelectRet> ret;

            using (TObjContext db = GetObjectContext())
            {
                var results = func.Invoke(db);
                ret = SelectIQueryableToIEnumerable(results, selector);
            }
            return ret;
        }

        /// <summary>
        /// Assists in invoking a Func created with CompiledQuery that uses a selector to return a custom type.
        /// </summary>
        /// <typeparam name="TRet">The query return type.</typeparam>
        /// <typeparam name="TSelectRet">The desired return type.</typeparam>
        /// <typeparam name="TArg0">The first argument type.</typeparam>
        /// <param name="func">The CompiledQuery Func to invoke.</param>
        /// <param name="selector">The Func used to transform type <typeparamref name="TRet"/>
        /// <param name="arg0">The first argument.</param>
        /// into <typeparamref name="TSelectRet"/>.</param>
        /// <returns>The return of the <paramref name="func"/> query as type <typeparamref name="TSelectRet"/>.</returns>
        public IEnumerable<TSelectRet> InvokeAndSelectMany<TRet, TSelectRet, TArg0>(Func<TObjContext, TArg0, IQueryable<TRet>> func, Func<TRet, TSelectRet> selector, TArg0 arg0)
        {
            IEnumerable<TSelectRet> ret;

            using (TObjContext db = GetObjectContext())
            {
                var results = func.Invoke(db, arg0);
                ret = SelectIQueryableToIEnumerable(results, selector);
            }
            return ret;
        }

        /// <summary>
        /// Assists in invoking a Func created with CompiledQuery that uses a selector to return a custom type.
        /// </summary>
        /// <typeparam name="TRet">The query return type.</typeparam>
        /// <typeparam name="TSelectRet">The desired return type.</typeparam>
        /// <typeparam name="TArg0">The first argument type.</typeparam>
        /// <typeparam name="TArg1">The second argument type.</typeparam>
        /// <param name="func">The CompiledQuery Func to invoke.</param>
        /// <param name="selector">The Func used to transform type <typeparamref name="TRet"/>
        /// <param name="arg0">The first argument.</param>
        /// <param name="arg1">The second argument.</param>
        /// into <typeparamref name="TSelectRet"/>.</param>
        /// <returns>The return of the <paramref name="func"/> query as type <typeparamref name="TSelectRet"/>.</returns>
        public IEnumerable<TSelectRet> InvokeAndSelectMany<TRet, TSelectRet, TArg0, TArg1>(
            Func<TObjContext, TArg0, TArg1, IQueryable<TRet>> func, Func<TRet, TSelectRet> selector, TArg0 arg0, TArg1 arg1)
        {
            IEnumerable<TSelectRet> ret;

            using (TObjContext db = GetObjectContext())
            {
                var results = func.Invoke(db, arg0, arg1);
                ret = SelectIQueryableToIEnumerable(results, selector);
            }
            return ret;
        }

        /// <summary>
        /// Assists in invoking a Func created with CompiledQuery that uses a selector to return a custom type.
        /// </summary>
        /// <typeparam name="TRet">The query return type.</typeparam>
        /// <typeparam name="TSelectRet">The desired return type.</typeparam>
        /// <typeparam name="TArg0">The first argument type.</typeparam>
        /// <typeparam name="TArg1">The second argument type.</typeparam>
        /// <typeparam name="TArg2">The third argument type.</typeparam>
        /// <param name="func">The CompiledQuery Func to invoke.</param>
        /// <param name="selector">The Func used to transform type <typeparamref name="TRet"/>
        /// <param name="arg0">The first argument.</param>
        /// <param name="arg1">The second argument.</param>
        /// <param name="arg2">The third argument.</param>
        /// into <typeparamref name="TSelectRet"/>.</param>
        /// <returns>The return of the <paramref name="func"/> query as type <typeparamref name="TSelectRet"/>.</returns>
        public IEnumerable<TSelectRet> InvokeAndSelectMany<TRet, TSelectRet, TArg0, TArg1, TArg2>(
            Func<TObjContext, TArg0, TArg1, TArg2, IQueryable<TRet>> func, Func<TRet, TSelectRet> selector, TArg0 arg0, TArg1 arg1,
            TArg2 arg2)
        {
            IEnumerable<TSelectRet> ret;

            using (TObjContext db = GetObjectContext())
            {
                var results = func.Invoke(db, arg0, arg1, arg2);
                ret = SelectIQueryableToIEnumerable(results, selector);
            }
            return ret;
        }

        /// <summary>
        /// Takes an IQueryable with elements of type <typeparamref name="TOld"/> and converts it to an IEnumerable with
        /// elements of type <typeparamref name="TNew"/>.
        /// </summary>
        /// <typeparam name="TOld">The old/input type.</typeparam>
        /// <typeparam name="TNew">The new/output type.</typeparam>
        /// <param name="queryable">The IQueryable containing the elements to process.</param>
        /// <param name="selector">The Func that can turn type <typeparamref name="TOld"/>
        /// to type <typeparamref name="TNew"/>.</param>
        /// <returns>An IEnumerable containing the elements found in <paramref name="queryable"/>.</returns>
// ReSharper disable SuggestBaseTypeForParameter
        static IEnumerable<TNew> SelectIQueryableToIEnumerable<TOld, TNew>(IQueryable<TOld> queryable, Func<TOld, TNew> selector)
            // ReSharper restore SuggestBaseTypeForParameter
        {
            var queryableArray = queryable.ToArray();
            var ret = new TNew[queryableArray.Length];

            for (int i = 0; i < queryableArray.Length; i++)
            {
                ret[i] = selector(queryableArray[i]);
            }

            return ret;
        }
    }
}