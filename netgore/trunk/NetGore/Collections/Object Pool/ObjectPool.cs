using System;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// Manages a pool of reusable objects.
    /// </summary>
    /// <typeparam name="T">The type of object to pool.</typeparam>
    public class ObjectPool<T> : IObjectPool<T> where T : class, IPoolable
    {
        /// <summary>
        /// Delegate describing how to create an object for the object pool.
        /// </summary>
        /// <param name="objectPool">The object pool that created the object.</param>
        /// <returns>The object instance.</returns>
        public delegate T ObjectPoolObjectCreator(ObjectPool<T> objectPool);

        /// <summary>
        /// Delegate for handling events on the objects in the pool.
        /// </summary>
        /// <param name="poolObject">The object the event is related to.</param>
        public delegate void ObjectPoolObjectHandler(T poolObject);

        readonly object _threadSync;
        ObjectPoolObjectCreator _creator;

        int _liveObjects;
        T[] _poolObjects;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPool{T}"/> class.
        /// </summary>
        /// <param name="creator">The delegate used to create new object instances.</param>
        /// <param name="threadSafe">If true, this collection will be thread safe at a slight performance cost.
        /// Set this value to true if you plan on ever accessing this collection from more than one thread.</param>
        public ObjectPool(ObjectPoolObjectCreator creator, bool threadSafe) : this(creator, null, null, threadSafe)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPool{T}"/> class.
        /// </summary>
        /// <param name="creator">The delegate used to create new object instances.</param>
        /// <param name="initializer">The delegate used to initialize an object as it is acquired from the pool
        /// (when Acquire() is called). Can be null.</param>
        /// <param name="deinitializer">The delegate used to deinitialize an object as it is freed (when Free()
        /// is called). Can be null.</param>
        /// <param name="threadSafe">If true, this collection will be thread safe at a slight performance cost.
        /// Set this value to true if you plan on ever accessing this collection from more than one thread.</param>
        public ObjectPool(ObjectPoolObjectCreator creator, ObjectPoolObjectHandler initializer,
                          ObjectPoolObjectHandler deinitializer, bool threadSafe)
            : this(16, creator, initializer, deinitializer, threadSafe)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPool{T}"/> class.
        /// </summary>
        /// <param name="initialSize">The initial size of the pool.</param>
        /// <param name="creator">The delegate used to create new object instances.</param>
        /// <param name="initializer">The delegate used to initialize an object as it is acquired from the pool
        /// (when Acquire() is called). Can be null.</param>
        /// <param name="deinitializer">The delegate used to deinitialize an object as it is freed (when Free()
        /// is called). Can be null.</param>
        /// <param name="threadSafe">If true, this collection will be thread safe at a slight performance cost.
        /// Set this value to true if you plan on ever accessing this collection from more than one thread.</param>
        public ObjectPool(int initialSize, ObjectPoolObjectCreator creator, ObjectPoolObjectHandler initializer,
                          ObjectPoolObjectHandler deinitializer, bool threadSafe)
        {
            if (threadSafe)
                _threadSync = new object();

            // Store our delegates
            Creator = creator;
            Initializer = initializer;
            Deinitializer = deinitializer;

            // Create the initial pool and the object instances
            _poolObjects = new T[initialSize];
            for (var i = 0; i < _poolObjects.Length; i++)
            {
                _poolObjects[i] = CreateObject(i);
            }

            AssertValidPoolIndexForLiveObjects();
        }

        /// <summary>
        /// Gets or sets the delegate used to create new object instances. Cannot be null.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public ObjectPoolObjectCreator Creator
        {
            get { return _creator; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _creator = value;
            }
        }

        /// <summary>
        /// Gets or sets the delegate used to deinitialize an object as it is freed (when Free()
        /// is called). Can be null.
        /// </summary>
        public ObjectPoolObjectHandler Deinitializer { get; set; }

        /// <summary>
        /// Gets or sets the delegate used to initialize an object as it is acquired from the pool
        /// (when Acquire() is called). Can be null.
        /// </summary>
        public ObjectPoolObjectHandler Initializer { get; set; }

        /// <summary>
        /// Gets the index of the last live object.
        /// </summary>
        int LastLiveObjectIndex
        {
            get { return LiveObjects - 1; }
        }

        /// <summary>
        /// Ensures the indices are all correct.
        /// </summary>
        [Conditional("DEBUG")]
        void AssertValidPoolIndexForLiveObjects()
        {
            for (var i = 0; i < LiveObjects; i++)
            {
                var obj = _poolObjects[i];
                Debug.Assert(obj.PoolIndex == i);
            }
        }

        /// <summary>
        /// Creates a new instance of the poolable object.
        /// </summary>
        /// <param name="index">The index of the object in the pool array.</param>
        /// <returns>A new instance of the poolable object.</returns>
        T CreateObject(int index)
        {
            var obj = Creator(this);
            obj.PoolIndex = index;
            return obj;
        }

        /// <summary>
        /// Expands the size of the object pool array.
        /// </summary>
        void ExpandPool()
        {
            var oldLength = _poolObjects.Length;

            // Expand the pool
            Array.Resize(ref _poolObjects, _poolObjects.Length << 2);

            // Allocate the new object instances
            for (var i = oldLength; i < _poolObjects.Length; i++)
            {
                Debug.Assert(_poolObjects[i] == null);
                _poolObjects[i] = CreateObject(i);
            }

            AssertValidPoolIndexForLiveObjects();
        }

        T InternalAcquire()
        {
            // Expand the pool if needed
            if (LiveObjects >= _poolObjects.Length - 1)
                ExpandPool();

            Debug.Assert(LiveObjects < _poolObjects.Length);

            // Grab the next free object
            var ret = _poolObjects[_liveObjects++];

            Debug.Assert(ret.PoolIndex == LiveObjects - 1);

            return ret;
        }

        void InternalClear()
        {
            // Call the deinitializer on all the live objects
            if (Deinitializer != null)
            {
                for (var i = 0; i < LiveObjects; i++)
                {
                    Deinitializer(_poolObjects[i]);
                }
            }

            // Decrease the live objects count to zero, marking all objects in the array as dead
            _liveObjects = 0;
        }

        void InternalFree(T poolObject, bool throwArgumentException)
        {
            // Ensure the object is in the living objects
            if (poolObject.PoolIndex > LastLiveObjectIndex)
                return;

            // Ensure that this object belongs to this pool instance
            if (_poolObjects[poolObject.PoolIndex] != poolObject)
            {
                if (throwArgumentException)
                {
                    const string errmsg =
                        "The poolObject belongs to a different pool than this one, or the IPoolable.Index was altered externally.";
                    throw new ArgumentException(errmsg, "poolObject");
                }
                else
                    return;
            }

            Debug.Assert(_poolObjects[poolObject.PoolIndex] == poolObject);

            // The object does belong to this pool, so free it by deinitializing it, then swapping it with the last
            // live object in the pool so we only have to relocate two objects. By deincrementing the live object count,
            // we which will push the object we just swapped to the end of the live objects block into the dead objects block,
            // effectively marking it as not live.
            if (Deinitializer != null)
                Deinitializer(poolObject);

            SwapPoolObjects(poolObject.PoolIndex, --_liveObjects);

            Debug.Assert(poolObject.PoolIndex >= LiveObjects);
        }

        int InternalFreeAll(Func<T, bool> condition)
        {
            var count = 0;

            // Loop through all live objects
            var i = 0;
            while (i < LiveObjects)
            {
                // Check the condition
                var current = _poolObjects[i];
                if (condition(current))
                {
                    // Free the object (the same way Free() does it, but without the validation checks)
                    Debug.Assert(_poolObjects[current.PoolIndex] == current);
                    if (Deinitializer != null)
                        Deinitializer(current);

                    SwapPoolObjects(current.PoolIndex, --_liveObjects);

                    // Increase the count for how many objects we removed
                    ++count;
                }
                else
                {
                    // Move on to the next object. Only do this when we didn't remove an object so that way we will
                    // re-check the object we just swapped with.
                    ++i;
                }
            }

            return count;
        }

        void InternalPerform(Action<T> action)
        {
            for (var i = 0; i < LiveObjects; i++)
            {
                action(_poolObjects[i]);
            }
        }

        /// <summary>
        /// Swaps two objects in the object pool.
        /// </summary>
        /// <param name="aIndex">The index of the first object.</param>
        /// <param name="bIndex">The index of the second object.</param>
        void SwapPoolObjects(int aIndex, int bIndex)
        {
            // Grab the object references
            var aObject = _poolObjects[aIndex];
            var bObject = _poolObjects[bIndex];

            // Swap the references in the array and update the indexes
            _poolObjects[bIndex] = aObject;
            aObject.PoolIndex = bIndex;

            _poolObjects[aIndex] = bObject;
            bObject.PoolIndex = aIndex;

            Debug.Assert(_poolObjects[aIndex].PoolIndex == aIndex);
            Debug.Assert(_poolObjects[bIndex].PoolIndex == bIndex);
        }

        #region IObjectPool<T> Members

        /// <summary>
        /// Gets the number of live objects in the pool.
        /// </summary>
        public int LiveObjects
        {
            get { return _liveObjects; }
        }

        /// <summary>
        /// Returns a free object instance from the pool.
        /// </summary>
        /// <returns>A free object instance from the pool.</returns>
        public T Acquire()
        {
            T ret;

            // Use thread synchronization if needed
            if (_threadSync != null)
            {
                // Thread-safe acquiring
                lock (_threadSync)
                {
                    ret = InternalAcquire();
                }
            }
            else
            {
                // Non thread-safe acquiring
                ret = InternalAcquire();
            }

            // Initialize
            if (Initializer != null)
                Initializer(ret);

            return ret;
        }

        /// <summary>
        /// Frees all live objects in the pool.
        /// </summary>
        public void Clear()
        {
            // Use thread synchronization if needed
            if (_threadSync != null)
            {
                // Thread-safe clearing
                lock (_threadSync)
                {
                    InternalClear();
                }
            }
            else
            {
                // Non thread-safe clearing
                InternalClear();
            }
        }

        /// <summary>
        /// Frees the object so the pool can reuse it. After freeing an object, it should not be used
        /// in any way, and be treated like it has been disposed. No exceptions will be thrown for trying to free
        /// an object that does not belong to this pool.
        /// </summary>
        /// <param name="poolObject">The object to be freed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="poolObject"/> is null.</exception>
        public void Free(T poolObject)
        {
            Free(poolObject, false);
        }

        /// <summary>
        /// Frees the object so the pool can reuse it. After freeing an object, it should not be used
        /// in any way, and be treated like it has been disposed.
        /// </summary>
        /// <param name="poolObject">The object to be freed.</param>
        /// <param name="throwArgumentException">Whether or not an <see cref="ArgumentException"/> will be thrown for
        /// objects that do not belong to this pool.</param>
        /// <exception cref="ArgumentException"><paramref name="throwArgumentException"/> is tru and the 
        /// <paramref name="poolObject"/> does not belong to this pool.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="poolObject"/> is null.</exception>
        public void Free(T poolObject, bool throwArgumentException)
        {
            if (poolObject == null)
                throw new ArgumentNullException("poolObject");

            // Use thread synchronization if needed
            if (_threadSync != null)
            {
                // Thread-safe freeing
                lock (_threadSync)
                {
                    InternalFree(poolObject, throwArgumentException);
                }
            }
            else
            {
                // Non thread-safe freeing
                InternalFree(poolObject, throwArgumentException);
            }
        }

        /// <summary>
        /// Frees all live objects in the pool that match the given <paramref name="condition"/>.
        /// </summary>
        /// <param name="condition">The condition used to determine if an object should be freed.</param>
        /// <returns>The number of objects that were freed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> is null.</exception>
        public int FreeAll(Func<T, bool> condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");

            // Use thread synchronization if needed
            int ret;
            if (_threadSync != null)
            {
                // Thread-safe freeing
                lock (_threadSync)
                {
                    ret = InternalFreeAll(condition);
                }
            }
            else
            {
                // Non thread-safe freeing
                ret = InternalFreeAll(condition);
            }

            return ret;
        }

        /// <summary>
        /// Performs the <paramref name="action"/> on all live objects in the object pool.
        /// </summary>
        /// <param name="action">The action to perform on all live objects in the object pool.</param>
        public void Perform(Action<T> action)
        {
            // Use thread synchronization if needed
            if (_threadSync != null)
            {
                // Thread-safe performing
                lock (_threadSync)
                {
                    InternalPerform(action);
                }
            }
            else
            {
                // Non thread-safe performing
                InternalPerform(action);
            }
        }

        #endregion
    }
}