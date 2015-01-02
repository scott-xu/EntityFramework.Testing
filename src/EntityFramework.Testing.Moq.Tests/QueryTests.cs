using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace EntityFramework.Testing.Moq.Tests
{
    public class QueryTests
    {
        [Fact]
        public void Can_enumerate_set()
        {
            var data = new List<Blog> { new Blog { }, new Blog { } };

            var set = new Mock<DbSet<Blog>>()
            .SetupData(data);

            var count = 0;
            foreach (var item in set.Object)                            
            {
                count++;
            }

            Assert.Equal(2, count);
        }
#if !NET40
        [Fact]
        public async Task Can_enumerate_set_async()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = new Mock<DbSet<Blog>>()
                .SetupData(data);

            var count = 0;
            await set.Object.ForEachAsync(b => count++);

            Assert.Equal(2, count);
        }
#endif
        [Fact]
        public void Can_use_linq_materializer_directly_on_set()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = new Mock<DbSet<Blog>>()
                .SetupData(data);

            var result = set.Object.ToList();

            Assert.Equal(2, result.Count);
        }

#if !NET40
        [Fact]
        public async Task Can_use_linq_materializer_directly_on_set_async()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = new Mock<DbSet<Blog>>()
                .SetupData(data);

            var result = await set.Object.ToListAsync();

            Assert.Equal(2, result.Count);
        }
#endif

        [Fact]
        public void Can_use_linq_opeartors()
        {
            var data = new List<Blog> 
            { 
                new Blog { BlogId = 1 }, 
                new Blog { BlogId = 2 },
                new Blog { BlogId = 3}
            };

            var set = new Mock<DbSet<Blog>>()
                .SetupData(data);

            var result = set.Object
                .Where(b => b.BlogId > 1)
                .OrderByDescending(b => b.BlogId)
                .ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(3, result[0].BlogId);
            Assert.Equal(2, result[1].BlogId);
        }

#if !NET40
        [Fact]
        public async Task Can_use_linq_opeartors_async()
        {
            var data = new List<Blog> 
            { 
                new Blog { BlogId = 1 }, 
                new Blog { BlogId = 2 },
                new Blog { BlogId = 3}
            };

            var set = new Mock<DbSet<Blog>>()
                .SetupData(data);

            var result = await set.Object
                .Where(b => b.BlogId > 1)
                .OrderByDescending(b => b.BlogId)
                .ToListAsync();

            Assert.Equal(2, result.Count);
            Assert.Equal(3, result[0].BlogId);
            Assert.Equal(2, result[1].BlogId);
        }

#endif

        [Fact]
        public void Can_use_include_directly_on_set()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = new Mock<DbSet<Blog>>()
                .SetupData(data);

            var result = set.Object
                .Include(b => b.Posts)
                .ToList();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Can_use_include_after_linq_operator()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = new Mock<DbSet<Blog>>()
                .SetupData(data);

            var result = set.Object
                .OrderBy(b => b.BlogId)
                .Include(b => b.Posts)
                .ToList();

            Assert.Equal(2, result.Count);
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
