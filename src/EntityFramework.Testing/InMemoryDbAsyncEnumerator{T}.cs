//-----------------------------------------------------------------------------------------------------
// <copyright file="InMemoryDbAsyncEnumerator{T}.cs" company="Microsoft Open Technologies, Inc">
//   Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
//   Modified by Scott Xu to be compliance with StyleCop.
// </copyright>
//-----------------------------------------------------------------------------------------------------

#if !NET40
namespace EntityFramework.Testing
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents in-memory async enumerator.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    public class InMemoryDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        /// <summary>
        /// The generic enumerator.
        /// </summary>
        private readonly IEnumerator<T> enumerator;

        /// <summary>
        /// A Boolean to indicate whether this object is disposed or not.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryDbAsyncEnumerator{T}"/> class.
        /// </summary>
        /// <param name="enumerator">The generic enumerator.</param>
        public InMemoryDbAsyncEnumerator(IEnumerator<T> enumerator)
        {
            this.enumerator = enumerator;
        }

        /// <summary>
        /// Gets the current item.
        /// </summary>
        public T Current
        {
            get { return this.enumerator.Current; }
        }

        /// <summary>
        /// Gets the current item.
        /// </summary>
        object IDbAsyncEnumerator.Current
        {
            get { return this.Current; }
        }

        /// <summary>
        /// Move next asynchronously
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The result task.</returns>
        public async Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            return this.enumerator.MoveNext();
        }

        /// <summary>
        /// Dispose this object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Dispose the object
        /// </summary>
        /// <param name="disposing">A Boolean to indicate whether it's called by user or GC.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                this.enumerator.Dispose();
            }

            this.disposed = true;
        }
    }
}
#endif