//-----------------------------------------------------------------------------------------------------
// <copyright file="ManipulationTests.cs" company="Justin Yoo">
//   Copyright (c) 2014 Justin Yoo.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing.NSubstitute.Tests
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
#if !NET40
    using System.Threading.Tasks;
#endif
    using global::NSubstitute;
    using Xunit;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Allowed test class to be left without being documented.")]
    public class ManipulationTests
    {
        [Fact]
        public void Can_remove_set()
        {
            var blog = new Blog();
            var data = new List<Blog> { blog, new Blog() };

            var set = this.GetSubstituteDbSet().SetupData(data);

            set.Remove(blog);

            var result = set.ToList();

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void Can_return_entity_from_remove_set()
        {
            var blog = new Blog();
            var data = new List<Blog> { blog };

            var set = this.GetSubstituteDbSet().SetupData(data);

            var result = set.Remove(blog);

            Assert.NotNull(result);
            Assert.Equal(blog, result);
        }

        [Fact]
        public void Can_removeRange_sets()
        {
            var blog = new Blog();
            var blog2 = new Blog();
            var range = new List<Blog> { blog, blog2 };
            var data = new List<Blog> { blog, blog2, new Blog() };

            var set = this.GetSubstituteDbSet().SetupData(data);

            set.RemoveRange(range);

            var result = set.ToList();

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void Can_return_entities_from_removeRange_sets()
        {
            var blog = new Blog();
            var blog2 = new Blog();
            var range = new List<Blog> { blog, blog2 };
            var data = new List<Blog> { blog, blog2, new Blog() };

            var set = this.GetSubstituteDbSet().SetupData(data);

            var result = set.RemoveRange(range);

            Assert.NotNull(result);
            Assert.Equal(range, result);
        }

        [Fact]
        public void Can_add_set()
        {
            var blog = new Blog();
            var data = new List<Blog> { };

            var set = this.GetSubstituteDbSet().SetupData(data);

            set.Add(blog);

            var result = set.ToList();

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void Can_return_entity_from_add_set()
        {
            var blog = new Blog();
            var data = new List<Blog> { };

            var set = this.GetSubstituteDbSet().SetupData(data);

            var result = set.Add(blog);

            Assert.NotNull(result);
            Assert.Equal(blog, result);
        }

        [Fact]
        public void Can_addRange_sets()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = this.GetSubstituteDbSet().SetupData(new List<Blog> { new Blog() });

            set.AddRange(data);

            var result = set.ToList();

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void Can_return_entities_from_addRange_sets()
        {
            var blog = new Blog();
            var blog2 = new Blog();
            var range = new List<Blog> { blog, blog2 };

            var set = this.GetSubstituteDbSet().SetupData();

            var result = set.AddRange(range);

            Assert.NotNull(result);
            Assert.Equal(range, result);
        }

        [Fact]
        public void Can_toList_twice()
        {
            var set = this.GetSubstituteDbSet()
                .SetupData(new List<Blog> { new Blog() });

            var result = set.ToList();

            var result2 = set.ToList();

            Assert.Equal(1, result2.Count);
        }

#if !NET40
        [Fact]
        public async Task Can_find_set_async()
        {
            var data = new List<Blog>
            {
                new Blog { BlogId = 1 },
                new Blog { BlogId = 2 },
                new Blog { BlogId = 3 }
            };

            var set = this.GetSubstituteDbSet()
                .SetupData(data, objs => data.FirstOrDefault(b => b.BlogId == (int)objs.First()));

            var result = await set
                .FindAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.BlogId);
        }
#endif

        [Fact]
        public void Can_create_entity()
        {
            var set = this.GetSubstituteDbSet()
                .SetupData();

            Assert.IsType<Blog>(set.Create());
        }

        [Fact]
        public void Can_specify_create()
        {
            var expected = new Blog();

            var set = this.GetSubstituteDbSet()
                .SetupData()
                .SetupCreate<Blog>(() => expected);

            Assert.Equal<Blog>(expected, set.Create());
        }

        [Fact]
        public void Can_create_generic_entity()
        {
            var set = this.GetSubstituteDbSet()
                .SetupData()
                .SetupCreate<Blog, FeaturedBlog>();

            Assert.IsType<FeaturedBlog>(set.Create<FeaturedBlog>());
        }

        [Fact]
        public void Can_specify_generic_create()
        {
            var expected = new FeaturedBlog();

            var set = this.GetSubstituteDbSet()
                .SetupData()
                .SetupCreate<Blog, FeaturedBlog>(() => expected);

            Assert.Equal<Blog>(expected, set.Create<FeaturedBlog>());
        }

        [Fact]
        public void Can_specify_include()
        {
            var set = this.GetSubstituteDbSet()
                .SetupData(new List<Blog> { new Blog() });

            var result = set
                .Include(b => b.Posts)
                .ToList();

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void Can_specify_asNoTracking()
        {
            var set = this.GetSubstituteDbSet()
                .SetupData(new List<Blog> { new Blog() });

            var result = set
                .AsNoTracking()
                .ToList();

            Assert.Equal(1, result.Count);
        }

        private DbSet<Blog> GetSubstituteDbSet()
        {
#if NET40
            return Substitute.For<DbSet<Blog>, IQueryable<Blog>>();
#else
            return Substitute.For<DbSet<Blog>, IQueryable<Blog>, IDbAsyncEnumerable<Blog>>();
#endif
        }

        public class FeaturedBlog : Blog
        {
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