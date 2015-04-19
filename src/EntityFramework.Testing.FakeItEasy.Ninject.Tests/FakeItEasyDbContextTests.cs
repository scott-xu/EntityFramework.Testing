namespace EntityFramework.Testing.FakeItEasy.Ninject.Tests
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using EntityFramework.Testing.Ninject.Tests;
    using global::FakeItEasy;
    using global::Ninject;
    using global::Ninject.MockingKernel;
    using global::Ninject.MockingKernel.FakeItEasy;
    using Xunit;

    public class FakeItEasyDbContextTests : DbContextTests
    {
        [Fact]
        public override void Can_setup_dbset()
        {
            using (var kernel = this.CreateMockingKernel())
            {
                var blogs = new List<Blog> { new Blog(), new Blog() };
                kernel.Get<DbSet<Blog>>().SetupData(blogs);

                var db = kernel.Get<BlogDbContext>();

                Assert.Equal(2, db.Blogs.Count());
            }
        }

        protected override MockingKernel CreateMockingKernel()
        {
            var kernel = new FakeItEasyMockingKernel();
            kernel.Load(new EntityFrameworkTestingFakeItEasyModule());
            return kernel;
        }
    }
}
