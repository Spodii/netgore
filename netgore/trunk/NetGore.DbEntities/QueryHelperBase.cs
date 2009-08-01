using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;

namespace NetGore.DbEntities
{
    /// <summary>
    /// A base class that assists in executing queries without all the verbosity, such as with a CompiledQuery.
    /// </summary>
    /// <typeparam name="T">The type of ObjectContext.</typeparam>
    public abstract class QueryHelperBase<T> where T : ObjectContext
    {
        /// <summary>
        /// Assists in invoking a Func created with CompiledQuery.
        /// </summary>
        /// <typeparam name="TRet">The return type.</typeparam>
        /// <param name="func">The Func to invoke.</param>
        /// <returns>The return value of the Func.</returns>
        public TRet Invoke<TRet>(Func<T, TRet> func)
        {
            TRet ret;
            using (var db = GetObjectContext())
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
        public TRet Invoke<TRet, TArg0>(Func<T, TArg0, TRet> func, TArg0 arg0)
        {
            TRet ret;
            using (var db = GetObjectContext())
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
        public TRet Invoke<TRet, TArg0, TArg1>(Func<T, TArg0, TArg1, TRet> func, TArg0 arg0, TArg1 arg1)
        {
            TRet ret;
            using (var db = GetObjectContext())
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
        public TRet Invoke<TRet, TArg0, TArg1, TArg2>(Func<T, TArg0, TArg1, TArg2, TRet> func, TArg0 arg0, TArg1 arg1, TArg2 arg2)
        {
            TRet ret;
            using (var db = GetObjectContext())
            {
                ret = func.Invoke(db, arg0, arg1, arg2);
            }
            return ret;
        }

        /// <summary>
        /// Gets an available ObjectContext of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>An available ObjectContext of type <typeparamref name="T"/>.</returns>
        protected abstract T GetObjectContext();
    }
}
