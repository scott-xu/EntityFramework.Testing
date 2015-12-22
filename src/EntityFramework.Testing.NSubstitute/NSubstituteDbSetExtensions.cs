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
#if !NET40
    using System.Threading;
    using System.Threading.Tasks;
#endif
    using EntityFramework.Testing;
    using NSubstitute.Core;

    /// <summary>
    /// Extension methods for <see cref="DbSet{T}"/>.
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

            dbSet.Create().Returns(info => getQuery().Create());
            dbSet.Include(Arg.Any<string>()).Returns(dbSet);
            dbSet.Find(Arg.Any<object[]>()).Returns(new Func<CallInfo, TEntity>(info => find(info.Arg<object[]>())));

#if !NET40
            dbSet.FindAsync(Arg.Any<object[]>()).Returns(new Func<CallInfo, Task<TEntity>>(info => Task.Run(() => find(info.Arg<object[]>()))));
            dbSet.FindAsync(Arg.Any<CancellationToken>(), Arg.Any<object[]>()).Returns(new Func<CallInfo, Task<TEntity>>(info => Task.Run(() => find(info.Arg<object[]>()), info.Arg<CancellationToken>())));
#endif

            dbSet.Remove(Arg.Do<TEntity>(entity => data.Remove(entity))).Returns(args => args[0]);

            dbSet.RemoveRange(Arg.Do<IEnumerable<TEntity>>(entities =>
            {
                foreach (var entity in entities)
                {
                    data.Remove(entity);
                }
            })).Returns(args => args[0]);

            dbSet.Add(Arg.Do<TEntity>(data.Add)).Returns(args => args[0]);

            dbSet.AddRange(Arg.Do<IEnumerable<TEntity>>(entities =>
            {
                foreach (var entity in entities)
                {
                    data.Add(entity);
                }
            })).Returns(args => args[0]);

            return dbSet;
        }
    }
}