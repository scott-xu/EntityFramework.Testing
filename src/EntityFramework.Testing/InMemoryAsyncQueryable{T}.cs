//-----------------------------------------------------------------------------------------------------
// <copyright file="InMemoryAsyncQueryable{T}.cs" company="Microsoft Open Technologies, Inc">
//   Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
//   Modified by Scott Xu to be compliance with StyleCop.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
#if !NET40
    using System.Data.Entity.Infrastructure;
#endif
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Represents in-memory async query-able object.
    /// </summary>
    /// <typeparam name="T">The type of the content of the data source.</typeparam>
#if !NET40
    public class InMemoryAsyncQueryable<T> : IOrderedQueryable<T>, IDbAsyncEnumerable<T>
#else
    public class InMemoryAsyncQueryable<T> : IOrderedQueryable<T>
#endif
    {
        /// <summary>
        /// The query-able object.
        /// </summary>
        private readonly IQueryable<T> queryable;

        /// <summary>
        /// The Include action.
        /// </summary>
        private readonly Action<string, IEnumerable> include;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryAsyncQueryable{T}"/> class.
        /// </summary>
        /// <param name="queryable">The query-able object.</param>
        /// <param name="include">The Include action.</param>
        public InMemoryAsyncQueryable(IQueryable<T> queryable, Action<string, IEnumerable> include = null)
        {
            this.queryable = queryable;
            this.include = include;
        }

        /// <summary>
        /// Gets the expression tree that is associated with the instance of <see cref="InMemoryAsyncQueryable{T}"/>.
        /// </summary>
        public Expression Expression
        {
            get { return this.queryable.Expression; }
        }

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree
        /// associated with this instance of <see cref="InMemoryAsyncQueryable{T}"/> is executed.
        /// </summary>
        public Type ElementType
        {
            get { return this.queryable.ElementType; }
        }

        /// <summary>
        /// Gets the <see cref="InMemoryAsyncQueryProvider"/>.
        /// </summary>
        public IQueryProvider Provider
        {
            get { return new InMemoryAsyncQueryProvider(this.queryable.Provider, this.include); }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.queryable.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="IEnumerator"/> that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Creates a new instance of an entity for the type of this set.
        /// Note that this instance is NOT added or attached to the set.
        /// </summary>
        /// <returns>The entity instance.</returns>
        public T Create()
        {
            return Activator.CreateInstance<T>();
        }

        /// <summary>
        /// Creates a new instance of an entity for the type of this set or for a type derived from the type of this set.
        /// Note that this instance is NOT added or attached to the set.
        /// </summary>
        /// <typeparam name="TDerivedEntity">The type of entity to create.</typeparam>
        /// <returns>The entity instance.</returns>
        public TDerivedEntity Create<TDerivedEntity>()
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        /// <summary>
        /// Include navigation properties.
        /// </summary>
        /// <param name="path">The property path.</param>
        /// <returns>The query-able object itself.</returns>
        public IQueryable<T> Include(string path)
        {
            if (this.include != null)
            {
                this.include(path, this.queryable);
            }

            return this;
        }

#if !NET40
        /// <summary>
        /// Get generic DB async enumerator
        /// </summary>
        /// <returns>The <see cref="IDbAsyncEnumerator{T}"/>.</returns>
        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new InMemoryDbAsyncEnumerator<T>(this.GetEnumerator());
        }

        /// <summary>
        /// Get DB async enumerator
        /// </summary>
        /// <returns>The <see cref="IDbAsyncEnumerator"/>.</returns>
        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return this.GetAsyncEnumerator();
        }
#endif
    }
}
