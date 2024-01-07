namespace SportApp.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    internal class TestDbAsyncEnumerator<TEntity> : IDbAsyncEnumerator<TEntity>
    {
        private readonly IEnumerator<TEntity> enumerator;

        public TestDbAsyncEnumerator(IEnumerator<TEntity> enumerator)
        {
            this.enumerator = enumerator;
        }

        public void Dispose()
        {
            this.enumerator.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(this.enumerator.MoveNext());
        }

        public TEntity Current => this.enumerator.Current;

        object IDbAsyncEnumerator.Current
        {
            get { return this.Current; }
        }
    }
}