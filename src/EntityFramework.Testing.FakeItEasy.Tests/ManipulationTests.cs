namespace EntityFramework.Testing.FakeItEasy.Tests
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading.Tasks;
    using global::FakeItEasy;
    using Xunit;

    public class ManipulationTests
    {
        [Fact]
        public void Can_remove_set()
        {
            var blog = new Blog();
            var data = new List<Blog> { blog };

            var set = this.GetFakeDbSet()
                .SetupData(data);

            set.Remove(blog);

            var result = set.ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void Can_return_entity_from_remove_set()
        {
            var blog = new Blog();
            var data = new List<Blog> { blog };

            var set = this.GetFakeDbSet().SetupData(data);

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

            var set = this.GetFakeDbSet()
                .SetupData(data);

            set.RemoveRange(range);

            var result = set.ToList();

            Assert.Single(result);
        }

        [Fact]
        public void Can_removeRange_referencingSelf_sets()
        {
            var blog = new Blog() { BlogId = 1, Url = "X" };
            var blog2 = new Blog() { BlogId = 2 };
            var blog3 = new Blog() { BlogId = 3 };
            var data = new List<Blog> { blog, blog2, blog3 };

            var set = this.GetFakeDbSet().SetupData(data);

            set.RemoveRange(from b in set where b.Url == null select b);

            var result = set.ToList();

            Assert.Single(result);
        }

        [Fact]
        public void Can_return_entities_from_removeRange_sets()
        {
            var blog = new Blog();
            var blog2 = new Blog();
            var range = new List<Blog> { blog, blog2 };
            var data = new List<Blog> { blog, blog2, new Blog() };

            var set = this.GetFakeDbSet().SetupData(data);

            var result = set.RemoveRange(range);

            Assert.NotNull(result);
            Assert.Equal(range, result);
        }

        [Fact]
        public void Can_attach_set()
        {
            var blog = new Blog();
            var data = new List<Blog> { };

            var set = this.GetFakeDbSet()
                .SetupData(data);

            set.Attach(blog);

            var result = set.ToList();

            Assert.Single(result);
        }

        [Fact]
        public void Can_add_set()
        {
            var blog = new Blog();
            var data = new List<Blog> { };

            var set = this.GetFakeDbSet()
                .SetupData(data);

            set.Add(blog);

            var result = set.ToList();

            Assert.Single(result);
        }

        [Fact]
        public void Can_return_entity_from_add_set()
        {
            var blog = new Blog();
            var data = new List<Blog> { };

            var set = this.GetFakeDbSet().SetupData(data);

            var result = set.Add(blog);

            Assert.NotNull(result);
            Assert.Equal(blog, result);
        }

        [Fact]
        public void Can_addRange_sets()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = this.GetFakeDbSet()
                .SetupData(new List<Blog> { new Blog() });

            set.AddRange(data);

            var result = set.ToList();

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void Can_addRange_referencingSelf_sets()
        {

            var blog2 = new Blog() { BlogId = 2 };
            var blog3 = new Blog() { BlogId = 3 };
            var blog1 = new Blog() { BlogId = 1 };
            var data = new List<Blog> { blog1, blog2, blog3 };

            var set = this.GetFakeDbSet().SetupData(data);

            set.AddRange(from s in set select new Blog { BlogId = s.BlogId * 4 });

            var result = set.ToList();

            Assert.Equal(6, result.Count);
        }

        [Fact]
        public void Can_return_entities_from_addRange_sets()
        {
            var blog = new Blog();
            var blog2 = new Blog();
            var range = new List<Blog> { blog, blog2 };

            var set = this.GetFakeDbSet().SetupData();

            var result = set.AddRange(range);

            Assert.NotNull(result);
            Assert.Equal(range, result);
        }

        [Fact]
        public void Can_toList_twice()
        {
            var set = this.GetFakeDbSet()
                .SetupData(new List<Blog> { new Blog() });

            var result = set.ToList();

            var result2 = set.ToList();

            Assert.Single(result2);
        }

        [Fact]
        public async Task Can_find_set_async()
        {
            var data = new List<Blog>
            {
                new Blog { BlogId = 1 },
                new Blog { BlogId = 2 },
                new Blog { BlogId = 3 }
            };

            var set = this.GetFakeDbSet()
                .SetupData(data, objs => data.FirstOrDefault(b => b.BlogId == (int)objs.First()));

            var result = await set
                .FindAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.BlogId);
        }

        [Fact]
        public void Can_specify_asNoTracking()
        {
            var set = this.GetFakeDbSet()
                .SetupData(new List<Blog> { new Blog() });

            var result = set
                .AsNoTracking()
                .ToList();

            Assert.Single(result);
        }

        [Fact]
        public void Can_create_entity()
        {
            var set = this.GetFakeDbSet()
                .SetupData();

            Assert.IsType<Blog>(set.Create());
        }

        private DbSet<Blog> GetFakeDbSet()
        {
            return A.Fake<DbSet<Blog>>(o => o.Implements(typeof(IQueryable<Blog>)).Implements(typeof(IDbAsyncEnumerable<Blog>)));
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
