//-----------------------------------------------------------------------------------------------------
// <copyright file="NSubstituteDbSetExtensions.cs" company="Justin Yoo">
//   Copyright (c) 2014 Justin Yoo.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace NSubstitute
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using EntityFramework.Testing;
    using NSubstitute.Core;

    /// <summary>
    /// Extension methods for <see cref="NSubstitute"/>.
    /// </summary>
    public static class NSubstituteDbSetExtensions
    {
        /// <summary>
        /// Setup data to <see cref="DbSet{T}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="dbSet">The <see cref="DbSet{T}"/>.</param>
        /// <param name="data">The seed data.</param>
        /// <param name="find">The find action.</param>
        /// <returns>The updated <see cref="DbSet{T}"/>.</returns>
        public static DbSet<TEntity> SetupData<TEntity>(this DbSet<TEntity> dbSet, ICollection<TEntity> data = null, Func<object[], TEntity> find = null) where TEntity : class
        {
            data = data ?? new List<TEntity>();
            find = find ?? (o => null);

            // In order to avoid enumerator modification exception, use the lambda expression to send query to the captured list.
            // http://stackoverflow.com/a/27474056
            Func<InMemoryAsyncQueryable<TEntity>> getQuery = () => new InMemoryAsyncQueryable<TEntity>(data.AsQueryable());

            ((IQueryable<TEntity>)dbSet).Provider.Returns(info => getQuery().Provider);
            ((IQueryable<TEntity>)dbSet).Expression.Returns(info => getQuery().Expression);
            ((IQueryable<TEntity>)dbSet).ElementType.Returns(info => getQuery().ElementType);
            ((IQueryable<TEntity>)dbSet).GetEnumerator().Returns(info => getQuery().GetEnumerator());

#if !NET40
            ((IDbAsyncEnumerable<TEntity>)dbSet).GetAsyncEnumerator().Returns(info => getQuery().GetAsyncEnumerator());
#endif

            dbSet.Include(Arg.Any<string>()).Returns(dbSet);
            dbSet.Find(Arg.Any<object[]>()).Returns(find as Func<CallInfo, TEntity>);

            dbSet.Remove(Arg.Do<TEntity>(entity => data.Remove(entity)));

            dbSet.RemoveRange(Arg.Do<IEnumerable<TEntity>>(entities =>
            {
                foreach (var entity in entities)
                {
                    data.Remove(entity);
                }
            }));

            dbSet.Add(Arg.Do<TEntity>(data.Add));

            dbSet.AddRange(Arg.Do<IEnumerable<TEntity>>(entities =>
            {
                foreach (var entity in entities)
                {
                    data.Add(entity);
                }
            }));

            return dbSet;
        }
    }
}