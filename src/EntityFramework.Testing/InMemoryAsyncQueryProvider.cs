//-----------------------------------------------------------------------------------------------------
// <copyright file="InMemoryAsyncQueryProvider.cs" company="Microsoft Open Technologies, Inc">
// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents in-memory async query provider.
    /// </summary>
    public class InMemoryAsyncQueryProvider : IQueryProvider, IDbAsyncQueryProvider
    {
        /// <summary>
        /// The method to create query.
        /// </summary>
        private static readonly MethodInfo CreateQueryMethod
            = typeof(InMemoryAsyncQueryProvider).GetDeclaredMethods().Single(m => m.IsGenericMethodDefinition && m.Name == "CreateQuery");

        /// <summary>
        /// The method to execute the query.
        /// </summary>
        private static readonly MethodInfo ExecuteMethod
            = typeof(InMemoryAsyncQueryProvider).GetDeclaredMethods().Single(m => m.IsGenericMethodDefinition && m.Name == "Execute");

        /// <summary>
        /// The query provider.
        /// </summary>
        private readonly IQueryProvider provider;

        /// <summary>
        /// The include action.
        /// </summary>
        private readonly Action<string, IEnumerable> include;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryAsyncQueryProvider"/> class.
        /// </summary>
        /// <param name="provider">The query provider.</param>
        /// <param name="include">The Include action.</param>
        public InMemoryAsyncQueryProvider(IQueryProvider provider, Action<string, IEnumerable> include = null)
        {
            this.provider = provider;
            this.include = include;
        }

        /// <summary>
        /// Creates query-able object.
        /// </summary>
        /// <param name="expression">The expression tree.</param>
        /// <returns>The query-able object.</returns>
        public IQueryable CreateQuery(Expression expression)
        {
            return (IQueryable)CreateQueryMethod
                .MakeGenericMethod(TryGetElementType(expression.Type))
                .Invoke(this, new object[] { expression });
        }

        /// <summary>
        /// Creates generic query-able object.
        /// </summary>
        /// <typeparam name="TElement">The element.</typeparam>
        /// <param name="expression">The expression tree.</param>
        /// <returns>The generic query-able object.</returns>
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new InMemoryAsyncQueryable<TElement>(this.provider.CreateQuery<TElement>(expression), this.include);
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="expression">The expression tree.</param>
        /// <returns>The result.</returns>
        public object Execute(Expression expression)
        {
            return ExecuteMethod.MakeGenericMethod(expression.Type).Invoke(this, new object[] { expression });
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="expression">The expression tree.</param>
        /// <returns>The result.</returns>
        public TResult Execute<TResult>(Expression expression)
        {
            return this.provider.Execute<TResult>(expression);
        }

        /// <summary>
        /// Executes the query asynchronously.
        /// </summary>
        /// <param name="expression">The expression tree.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The result task.</returns>
        public async Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return this.Execute(expression);
        }

        /// <summary>
        /// Executes the query asynchronously.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="expression">The expression tree.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The result task.</returns>
        public async Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return this.Execute<TResult>(expression);
        }

        /// <summary>
        /// Tries to get element type.
        /// </summary>
        /// <param name="type">The expression type.</param>
        /// <returns>The element type.</returns>
        private static Type TryGetElementType(Type type)
        {
            if (!type.IsGenericTypeDefinition)
            {
                var interfaceImpl = type.GetInterfaces()
                    .Union(new[] { type })
                    .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

                if (interfaceImpl != null)
                {
                    return interfaceImpl.GetGenericArguments().Single();
                }
            }

            return type;
        }
    }
}