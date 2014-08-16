namespace EntityFramework.Testing.Moq.Ninject.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using global::Moq;
    using global::Ninject;
    using global::Ninject.MockingKernel;
    using global::Ninject.MockingKernel.Moq;

    [TestClass]
    public class MockDbContextTests
    {
        [TestMethod]
        public void Can_auto_mock_dbset()
        {
            using (var kernel = new MoqMockingKernel())
            {
                kernel.Bind<BlogDbContext>().ToMockDbContext();

                var db = kernel.Get<BlogDbContext>();

                Assert.IsNotNull(db);
                Assert.IsNotNull(db.Blogs);
            }
        }

        [TestMethod]
        public void Can_setup_dbset()
        {
            using (var kernel = new MoqMockingKernel())
            {
                kernel.Bind<BlogDbContext>().ToMockDbContext();

                var db = kernel.Get<BlogDbContext>();

                var blogs = new List<Blog> { new Blog(), new Blog() };
                kernel.GetMock<DbSet<Blog>>().SetupData(blogs);

                Assert.AreEqual(2, db.Blogs.Count());
            }
        }

        public class Blog
        {
            public int BlogId { get; set; }
            public string Url { get; set; }
        }

        public class BlogDbContext : DbContext
        {
            public virtual DbSet<Blog> Blogs { get; set; }
        }
    }
}
