//-----------------------------------------------------------------------------------------------------
// <copyright file="MoqDbSetExtensions.cs" company="Rowan Miller, Scott Xu">
// Copyright (c) Rowan Miller, Scott Xu. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace Moq
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using EntityFramework.Testing;

    /// <summary>
    /// Extension methods for <see cref="Mock{T}"/>.
    /// </summary>
    public static class MoqDbSetExtensions
    {
        /// <summary>
        /// Setup data to <see cref="Mock{T}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="mock">The <see cref="Mock{T}"/>.</param>
        /// <param name="data">The seed data.</param>
        /// <param name="find">The find action.</param>
        /// <returns>The updated <see cref="Mock{T}"/>.</returns>
        public static Mock<DbSet<TEntity>> SetupData<TEntity>(this Mock<DbSet<TEntity>> mock, ICollection<TEntity> data = null, Func<object[], TEntity> find = null)
            where TEntity : class
        {
            data = data ?? new List<TEntity>();
            find = find ?? (o => null);

            var query = new InMemoryAsyncQueryable<TEntity>(data.AsQueryable());

            mock.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(query.Provider);
            mock.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(query.Expression);
            mock.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(query.ElementType);
            mock.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(query.GetEnumerator);
            mock.As<IDbAsyncEnumerable<TEntity>>().Setup(m => m.GetAsyncEnumerator()).Returns(query.GetAsyncEnumerator);
            mock.Setup(m => m.AsNoTracking()).Returns(mock.Object);
            mock.Setup(m => m.Include(It.IsAny<string>())).Returns(mock.Object);
            mock.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(find);

            mock.Setup(m => m.FindAsync(It.IsAny<object[]>())).Returns<object[]>(objs => Task.Run(() => find(objs)));
            mock.Setup(m => m.FindAsync(It.IsAny<CancellationToken>(), It.IsAny<object[]>())).Returns<CancellationToken, object[]>((tocken, objs) => Task.Run(() => find(objs), tocken));

            mock.Setup(m => m.Create()).Returns(() => Activator.CreateInstance<TEntity>());

            mock.Setup(m => m.Remove(It.IsAny<TEntity>()))
                .Callback<TEntity>(entity =>
                {
                    data.Remove(entity);
                    mock.SetupData(data, find);
                })
                .Returns<TEntity>(entity => entity);

            mock.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<TEntity>>()))
                .Callback<IEnumerable<TEntity>>(entities =>
                {
                    foreach (var entity in entities)
                    {
                        data.Remove(entity);
                    }

                    mock.SetupData(data, find);
                })
                .Returns<IEnumerable<TEntity>>(entities => entities);

            mock.Setup(m => m.Add(It.IsAny<TEntity>()))
                .Callback<TEntity>(entity =>
                {
                    data.Add(entity);
                    mock.SetupData(data, find);
                })
                .Returns<TEntity>(entity => entity);

            mock.Setup(m => m.Attach(It.IsAny<TEntity>()))
                .Callback<TEntity>(entity =>
                {
                    data.Add(entity);
                    mock.SetupData(data, find);
                })
                .Returns<TEntity>(entity => entity);

            mock.Setup(m => m.AddRange(It.IsAny<IEnumerable<TEntity>>()))
                .Callback<IEnumerable<TEntity>>(entities =>
                {
                    foreach (var entity in entities)
                    {
                        data.Add(entity);
                    }

                    mock.SetupData(data, find);
                })
                .Returns<IEnumerable<TEntity>>(entities => entities);

            return mock;
        }
    }
}