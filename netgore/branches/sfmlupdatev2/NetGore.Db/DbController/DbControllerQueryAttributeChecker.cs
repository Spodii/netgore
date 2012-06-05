using System;
using System.Linq;
using NetGore.Collections;

namespace NetGore.Db
{
    /// <summary>
    /// Checks that all instantiable classes that inherit from <see cref="IDbQuery"/> implement the
    /// attribute <see cref="DbControllerQueryAttribute"/>.
    /// </summary>
    public class DbControllerQueryAttributeChecker
    {
        readonly TypedEventHandler<DbControllerQueryAttributeChecker, EventArgs<Type>> _missingAttributeHandler;
        readonly Type[] _typesToIgnore;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbControllerQueryAttributeChecker"/> class.
        /// </summary>
        /// <param name="missingAttributeHandler">The event handler for types with missing attributes.
        /// Cannot be null.</param>
        /// <exception cref="ArgumentNullException"><paramref name="missingAttributeHandler"/> is null.</exception>
        /// <param name="typesToIgnore">Optional array of types to ignore. If a type is in this collection, it will
        /// never be invoked by the <paramref name="missingAttributeHandler"/>.</param>
        public DbControllerQueryAttributeChecker(
            TypedEventHandler<DbControllerQueryAttributeChecker, EventArgs<Type>> missingAttributeHandler,
            params Type[] typesToIgnore)
        {
            if (missingAttributeHandler == null)
                throw new ArgumentNullException("missingAttributeHandler");

            _missingAttributeHandler = missingAttributeHandler;
            _typesToIgnore = typesToIgnore;

            var filter = new TypeFilterCreator
            {
                IsClass = true,
                IsAbstract = false,
                IsInterface = false,
                RequireAttributes = false,
                Interfaces = new Type[] { typeof(IDbQuery) },
                MatchAllInterfaces = true,
                RequireInterfaces = false,
                RequireConstructor = false,
                IsEnum = false
            };

            new TypeFactory(filter.GetFilter(), LoadTypeHandler);
        }

        /// <summary>
        /// Handles when a <see cref="Type"/> is loaded into the <see cref="TypeFactory"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NetGore.Collections.TypeFactoryLoadedEventArgs"/> instance containing the event data.</param>
        void LoadTypeHandler(TypeFactory sender, TypeFactoryLoadedEventArgs e)
        {
            // Skip private nested types as they cannot be called by the controller anyways
            if (e.LoadedType.IsNested && !e.LoadedType.IsPublic)
                return;

            // Filter out types to ignore
            if (_typesToIgnore != null && _typesToIgnore.Contains(e.LoadedType))
                return;

            // Check for attribute
            var attribs = e.LoadedType.GetCustomAttributes(false);
            if (attribs == null || attribs.Length == 0)
                _missingAttributeHandler(this, EventArgsHelper.Create(e.LoadedType));
        }
    }
}