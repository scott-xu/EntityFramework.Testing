//-----------------------------------------------------------------------------------------------------
// <copyright file="FakeItEasyDbSetExtensions.cs" company="Scott Xu">
//   Copyright (c) 2015 Scott Xu.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace FakeItEasy
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

    /// <summary>
    /// Extension methods for <see cref="DbSet{T}"/>.
    /// </summary>
    public static class FakeItEasyDbSetExtensions
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

            var query = new InMemoryAsyncQueryable<TEntity>(data.AsQueryable());

            A.CallTo(() => ((IQueryable<TEntity>)dbSet).Provider).ReturnsLazily(info => query.Provider);
            A.CallTo(() => ((IQueryable<TEntity>)dbSet).Expression).ReturnsLazily(info => query.Expression);
            A.CallTo(() => ((IQueryable<TEntity>)dbSet).ElementType).ReturnsLazily(info => query.ElementType);
            A.CallTo(() => ((IQueryable<TEntity>)dbSet).GetEnumerator()).ReturnsLazily(info => query.GetEnumerator());

#if !NET40
            A.CallTo(() => ((IDbAsyncEnumerable<TEntity>)dbSet).GetAsyncEnumerator()).ReturnsLazily(info => query.GetAsyncEnumerator());
#endif
            A.CallTo(() => dbSet.AsNoTracking()).Returns(dbSet);
            SetupCreate(dbSet);
            A.CallTo(() => dbSet.Include(A<string>._)).Returns(dbSet);
            A.CallTo(() => dbSet.Find(A<object[]>._)).ReturnsLazily<TEntity, object[]>(objs => find(objs));

#if !NET40
            A.CallTo(() => dbSet.FindAsync(A<object[]>._)).ReturnsLazily<Task<TEntity>, object[]>(objs => Task.Run(() => find(objs)));
            A.CallTo(() => dbSet.FindAsync(A<CancellationToken>._, A<object[]>._)).ReturnsLazily<Task<TEntity>, CancellationToken, object[]>((token, objs) => Task.Run(() => find(objs), token));
#endif

            A.CallTo(() => dbSet.Remove(A<TEntity>._)).ReturnsLazily<TEntity, TEntity>(entity =>
            {
                data.Remove(entity);
                return entity;
            });

            A.CallTo(() => dbSet.RemoveRange(A<IEnumerable<TEntity>>._)).ReturnsLazily<IEnumerable<TEntity>, IEnumerable<TEntity>>(entities =>
            {
                foreach (var entity in entities)
                {
                    data.Remove(entity);
                }

                return entities;
            });

            A.CallTo(() => dbSet.Add(A<TEntity>._)).ReturnsLazily<TEntity, TEntity>(entity =>
            {
                data.Add(entity);
                return entity;
            });

            A.CallTo(() => dbSet.AddRange(A<IEnumerable<TEntity>>._)).ReturnsLazily<IEnumerable<TEntity>, IEnumerable<TEntity>>(entities =>
            {
                foreach (var entity in entities)
                {
                    data.Add(entity);
                }

                return entities;
            });

            return dbSet;
        }

        /// <summary>
        /// Sets up the Create function on the <see cref="Mock{T}"/>.
        /// </summary>    
        /// <param name="mock">The <see cref="Mock{T}"/>.</param>
        /// <param name="create">The create action.</param>
        /// <returns>The updated <see cref="Mock{T}"/>.</returns>
        public static DbSet<TEntity> SetupCreate<TEntity>(this DbSet<TEntity> dbSet, Func<TEntity> create = null)
             where TEntity : class
        {
            create = create ?? InMemoryAsyncQueryable<TEntity>.Create;
            A.CallTo(() => dbSet.Create()).ReturnsLazily(info => create());
            return dbSet;
        }

        /// <summary>
        /// Sets up the generic Create function on the <see cref="Mock{T}"/>.
        /// </summary>    
        /// <typeparam name="TDerivedEntity">The type of entity to create.</typeparam>    
        /// <param name="mock">The <see cref="Mock{T}"/>.</param>
        /// <param name="create">The create action.</param>
        /// <returns>The updated <see cref="Mock{T}"/>.</returns>
        public static DbSet<TEntity> SetupCreate<TEntity, TDerivedEntity>(this DbSet<TEntity> dbSet, Func<TDerivedEntity> create = null)
            where TEntity : class
            where TDerivedEntity : class, TEntity
        {
            create = create ?? InMemoryAsyncQueryable<TEntity>.Create<TDerivedEntity>;
            A.CallTo(() => dbSet.Create<TDerivedEntity>()).ReturnsLazily(info => create());
            return dbSet;
        }
    }
}
