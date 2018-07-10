//-----------------------------------------------------------------------------------------------------
// <copyright file="QueryTests.cs" company="Justin Yoo">
//   Copyright (c) 2014 Justin Yoo.
// </copyright>
//-----------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace EntityFramework.Testing.NSubstitute.Tests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Allowed test class to be left without being documented.")]
    public class QueryTests
    {
        [Fact]
        public void Can_enumerate_set()
        {
            var data = new List<Blog> { new Blog { }, new Blog { } };

            var set = this.GetSubstituteDbSet().SetupData(data);

            var count = 0;
            foreach (var item in set)
            {
                count++;
            }

            Assert.Equal(2, count);
        }

        [Fact]
        public async Task Can_enumerate_set_async()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = this.GetSubstituteDbSet().SetupData(data);

            var count = 0;
            await set.ForEachAsync(b => count++);

            Assert.Equal(2, count);
        }

        [Fact]
        public void Can_use_linq_materializer_directly_on_set()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = this.GetSubstituteDbSet().SetupData(data);

            var result = set.ToList();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task Can_use_linq_materializer_directly_on_set_async()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = this.GetSubstituteDbSet().SetupData(data);

            var result = await set.ToListAsync();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Can_use_linq_opeartors()
        {
            var data = new List<Blog>
            {
                new Blog { BlogId = 1 },
                new Blog { BlogId = 2 },
                new Blog { BlogId = 3}
            };

            var set = this.GetSubstituteDbSet().SetupData(data);

            var result = set.Where(b => b.BlogId > 1)
                            .OrderByDescending(b => b.BlogId)
                            .ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(3, result[0].BlogId);
            Assert.Equal(2, result[1].BlogId);
        }

        [Fact]
        public async Task Can_use_linq_opeartors_async()
        {
            var data = new List<Blog>
            {
                new Blog { BlogId = 1 },
                new Blog { BlogId = 2 },
                new Blog { BlogId = 3}
            };

            var set = this.GetSubstituteDbSet().SetupData(data);

            var result = await set.Where(b => b.BlogId > 1)
                                  .OrderByDescending(b => b.BlogId)
                                  .ToListAsync();

            Assert.Equal(2, result.Count);
            Assert.Equal(3, result[0].BlogId);
            Assert.Equal(2, result[1].BlogId);
        }

        [Fact]
        public void Can_use_include_directly_on_set()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = this.GetSubstituteDbSet().SetupData(data);

            var result = set.Include(b => b.Posts)
                            .ToList();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Can_use_include_after_linq_operator()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = this.GetSubstituteDbSet().SetupData(data);

            var result = set.OrderBy(b => b.BlogId)
                            .Include(b => b.Posts)
                            .ToList();

            Assert.Equal(2, result.Count);
        }

        private DbSet<Blog> GetSubstituteDbSet()
        {
            return Substitute.For<DbSet<Blog>, IQueryable<Blog>, IDbAsyncEnumerable<Blog>>();
        }

        public class Blog
        {
            public int BlogId { get; set; }

            public string Url { get; set; }

            public List<Post> Posts { get; set; }
        }

        public class Post
        {
            public int PostId { get; set; }

            public string Title { get; set; }

            public string Content { get; set; }

            public int BlogId { get; set; }

            public Blog Blog { get; set; }
        }
    }
}