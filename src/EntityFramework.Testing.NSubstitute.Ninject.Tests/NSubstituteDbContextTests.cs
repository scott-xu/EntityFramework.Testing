namespace EntityFramework.Testing.NSubstitute.Ninject.Tests
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using global::Ninject;
    using global::Ninject.MockingKernel;
    using global::Ninject.MockingKernel.NSubstitute;
    using global::NSubstitute;
    using EntityFramework.Testing.Ninject.Tests;
    using Xunit;

    public class NSubstituteDbContextTests : DbContextTests
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
            var kernel = new NSubstituteMockingKernel();
            kernel.Load(new EntityFrameworkTestingNSubstituteModule());
            return kernel;
        }
    }
}
