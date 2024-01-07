namespace SportApp.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;

    internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
    { 
        private readonly IQueryProvider provider;

        internal TestDbAsyncQueryProvider(IQueryProvider provider)
        {
            this.provider = provider;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestDbAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestDbAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return this.provider.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return this.provider.Execute<TResult>(expression);
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.Execute<TResult>(expression));
        }

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.Execute(expression));
        }
    }
}
