using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Collections;

namespace NetGore.Db
{
    /// <summary>
    /// Checks that all instantiable classes that inherit from <see cref="IDbQueryHandler"/> implement the
    /// attribute <see cref="DbControllerQueryAttribute"/>.
    /// </summary>
    public class DbControllerQueryAttributeChecker
    {
        readonly Type[] _typesToIgnore;
        readonly TypeFactory _typeFactory;
        readonly DbControllerQueryAttributeCheckerEventHandler _missingAttributeHandler;

        void LoadTypeHandler(TypeFactory typeFactory, Type loadedType, string name)
        {
            // Skip private nested types as they cannot be called by the controller anyways
            if (loadedType.IsNested && !loadedType.IsPublic)
                return;

            // Filter out types to ignore
            if (_typesToIgnore != null && _typesToIgnore.Contains(loadedType))
                return;

            // Check for attribute
            var attribs = loadedType.GetCustomAttributes(false);
            if (attribs == null || attribs.Length == 0)
            {
                _missingAttributeHandler(this, loadedType);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbControllerQueryAttributeChecker"/> class.
        /// </summary>
        /// <param name="missingAttributeHandler">The event handler for types with missing attributes.
        /// Cannot be null.</param>
        /// <exception cref="ArgumentNullException"><paramref name="missingAttributeHandler"/> is null.</exception>
        /// <param name="typesToIgnore">Optional array of types to ignore. If a type is in this collection, it will
        /// never be invoked by the <paramref name="missingAttributeHandler"/>.</param>
        public DbControllerQueryAttributeChecker(DbControllerQueryAttributeCheckerEventHandler missingAttributeHandler,
            params Type[] typesToIgnore)
        {
            if (missingAttributeHandler == null)
                throw new ArgumentNullException("missingAttributeHandler");

            _missingAttributeHandler = missingAttributeHandler;
            _typesToIgnore = typesToIgnore;

            TypeFilterCreator filter = new TypeFilterCreator
            {
                IsClass = true,
                IsAbstract = false,
                IsInterface = false,
                RequireAttributes = false,
                Interfaces = new Type[] { typeof(IDbQueryHandler) },
                MatchAllInterfaces = true,
                RequireInterfaces = false,
                RequireConstructor = false,
                IsEnum = false
            };

            _typeFactory = new TypeFactory(filter.GetFilter(), LoadTypeHandler, false);
            _typeFactory.BeginLoading();
        }
    }
}
