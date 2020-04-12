namespace EntityFramework.Testing.Tests
{
    using System.Linq.Expressions;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class ManipulationTests
    {
        class TestSubject
        {
            public int Id { get; set; }
            public int ParentId { get; set; }
            public string Name { get; set; }
        }

        [Fact]
        public void Can_replace_QueryableDefaultIfEmpty_method()
        {
            var baseQuery = new List<TestSubject>().AsQueryable();
            var query = baseQuery.DefaultIfEmpty();

            var visitor = new DefaultIfEmptyRewriter();
            var expression = visitor.Visit(query.Expression);

            var call = Assert.IsAssignableFrom<MethodCallExpression>(expression);
            Assert.Equal("DefaultIfEmpty", call.Method.Name);
            Assert.Equal(2, call.Arguments.Count);
        }

        [Fact]
        public void Can_replace_EnumerableDefaultIfEmpty_method()
        {
            var baseQuery = new List<TestSubject>().AsQueryable();

            var query = from x in baseQuery
                        join y in baseQuery on x.Id equals y.ParentId into z
                        from q in z.DefaultIfEmpty()
                        select new { x.Name, ParentName = q.Name };

            var visitor = new DefaultIfEmptyRewriter();
            var expression = visitor.Visit(query.Expression);

            var selectMany = (MethodCallExpression)expression;
            var call = ((MethodCallExpression)((LambdaExpression)((UnaryExpression)selectMany.Arguments[1]).Operand).Body);
            Assert.Equal("DefaultIfEmpty", call.Method.Name);
            Assert.Equal(2, call.Arguments.Count);
        }

        [Fact]
        public void Can_provide_default_value()
        {
            var baseQuery = new List<TestSubject>().AsQueryable();
            var query = baseQuery.DefaultIfEmpty();

            var visitor = new DefaultIfEmptyRewriter();
            var expression = visitor.Visit(query.Expression);

            var call = Assert.IsAssignableFrom<MethodCallExpression>(expression);
            Assert.Equal("DefaultIfEmpty", call.Method.Name);
            var constant = Assert.IsAssignableFrom<ConstantExpression>(call.Arguments[1]);
            Assert.IsType<TestSubject>(constant.Value);
        }
    }
}