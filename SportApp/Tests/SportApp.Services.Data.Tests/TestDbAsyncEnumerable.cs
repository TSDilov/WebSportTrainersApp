namespace SportApp.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;

    using MockQueryable.EntityFrameworkCore;

    internal class TestDbAsyncEnumerable<TEntity> : EnumerableQuery<TEntity>, IDbAsyncEnumerable<TEntity>, IQueryable<TEntity>
    {
        public TestDbAsyncEnumerable(IEnumerable<TEntity> enumerable)
            : base(enumerable)
        {
        }

        public TestDbAsyncEnumerable(Expression expression)
            : base(expression)
        {
        }

        public IDbAsyncEnumerator<TEntity> GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<TEntity>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return this.GetAsyncEnumerator();
        }

        IQueryProvider IQueryable.Provider => new TestDbAsyncQueryProvider<TEntity>(this);
    }
}